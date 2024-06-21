using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.ApiModels;
using MyRestaurant.Common.Enums;
using MyRestaurant.Common.Helpers;
using MyRestaurant.Contracts.Services;
using MyRestaurant.Data;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Core.Services
{
    /// <summary>
    /// A default implementation of <see cref="IRestManager"/>.
    /// </summary>
    public class RestManager : IRestManager
    {
        private static readonly string RestLookupSavepointName = "RestLookupSavepointName";
        private static readonly string RestLeaveSavepointName = "RestLeaveSavepointName";
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly IErrorDescriber _errorDescriber;
        
        public RestManager(
            IMapper mapper,
            RestaurantDbContext dbContext,
            IErrorDescriber errorDescriber)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _errorDescriber = errorDescriber;
        }

        public async Task<TableModel> LookupAsync(ClientsGroupModel clientsGroup, CancellationToken cancellationToken = default)
        {
            await ValidateCustomerAsync(clientsGroup, cancellationToken);
            
            TableModel tableToReturn = null;
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                await transaction.CreateSavepointAsync(RestLookupSavepointName, cancellationToken);

                await AccommodatedQueuedClientsGroupAsync(cancellationToken);

                var emptyTables = await GetEmptyTablesAsync(clientsGroup.Size, cancellationToken);
                if (emptyTables.Count > 0)
                {
                    var emptyTable = emptyTables.First();
                    await _dbContext.AccommodatedClientsGroups.AddAsync(new AccommodatedClientsGroupEntity
                    {
                        TableId = emptyTable.Id,
                        ClientsGroupId = clientsGroup.Id
                    }, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    tableToReturn = _mapper.Map<TableModel>(emptyTable);
                }
                else
                {
                    var availableSeats = await GetAccommodatedClientsGroupAsync(clientsGroup.Size, cancellationToken);
                    if (availableSeats.Count > 0)
                    {
                        var availableSeat = availableSeats.First();

                        await _dbContext.AccommodatedClientsGroups.AddAsync(new AccommodatedClientsGroupEntity
                        {
                            TableId = availableSeat.TableId,
                            ClientsGroupId = clientsGroup.Id
                        }, cancellationToken);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        tableToReturn = new TableModel()
                        {
                            Id = availableSeat.TableId,
                            Size = availableSeat.TableSize
                        };
                    }
                }

                if (tableToReturn == null)
                {
                    _dbContext.QueuedClientsGroups.AddAsync(new QueuedClientsGroupEntity()
                    {
                        ClientsGroupId = clientsGroup.Id,
                        QueuedOn = TimeHelper.ServerTimeNow
                    }, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch(Exception exc)
            {
                await transaction.RollbackToSavepointAsync(RestLookupSavepointName, cancellationToken);
                tableToReturn = null;
                _errorDescriber.DescribeWithException(ErrorCode.UnhandledError);
            }

            return tableToReturn;
        }

        public async Task AccommodatedQueuedClientsGroupAsync(CancellationToken cancellationToken)
        {
            var availableSeats = await GetAccommodatedClientsGroupAsync(cancellationToken: cancellationToken);

            var emptyTables = await GetEmptyTablesAsync(cancellationToken: cancellationToken);

            var maxAvailableSeats = int.Max(emptyTables.Count > 0 ? emptyTables.Max(c => c.Size) : 0, availableSeats.Count > 0 ? availableSeats.Max(d => d.AvailableSeats) : 0);

            if (emptyTables.Count > 0 || availableSeats.Count > 0)
            {
                // Checking queue before assigning to new client group
                var queuedClients = await _dbContext.QueuedClientsGroups
                                                      .Include(x => x.ClientsGroup)
                                                      .Where(x => x.ClientsGroup.Size <= maxAvailableSeats)
                                                      .OrderBy(d => d.Id)
                                                      .AsNoTracking()
                                                      .ToListAsync(cancellationToken);

                if (queuedClients.Count > 0)
                {
                    var processedClients = new bool[queuedClients.Count];
                    for (int i = 0; i < queuedClients.Count; i++)
                    {
                        if (!processedClients[i])
                        {
                            var client = queuedClients[i];
                            var emptyTable = await AccommodateClientsGroupAsync(emptyTables, x => x.Size >= client.ClientsGroup.Size, client, cancellationToken);
                            if (emptyTable != null)
                            {
                                // marking customer and table as processed
                                processedClients[i] = true;
                                if (emptyTable.Size > client.ClientsGroup.Size)
                                {
                                    availableSeats.Add(new AccommodatedClientsGroupModel
                                    {
                                        TableId = emptyTable.Id,
                                        TableSize = emptyTable.Size,
                                        AvailableSeats = emptyTable.Size - client.ClientsGroup.Size
                                    });
                                }
                            }    
                        }
                    }

                    for (int i = 0; i < queuedClients.Count; i++)
                    {
                        if (!processedClients[i])
                        {
                            var client = queuedClients[i];
                            var availableSeat = availableSeats.FirstOrDefault(c => c.AvailableSeats >= client.ClientsGroup.Size);
                            if (availableSeat != null)
                            {
                                availableSeat.AvailableSeats -= client.ClientsGroup.Size;
                                processedClients[i] = true;
                                await _dbContext.AccommodatedClientsGroups.AddAsync(new AccommodatedClientsGroupEntity
                                {
                                    TableId = availableSeat.TableId,
                                    ClientsGroupId = client.ClientsGroupId
                                }, cancellationToken);
                                
                                await _dbContext.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }
                }
            }
        }

        public async Task<List<AccommodatedClientsGroupModel>> GetAccommodatedClientsGroupAsync(int? requiredSize = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.AccommodatedClientsGroups
                                  .Select(x => new
                                   {
                                       x.TableId,
                                       TableSize = x.Table.Size,
                                       ClientsGroupSize = x.ClientsGroup.Size
                                   })
                                  .GroupBy(x => new { x.TableId, x.TableSize })
                                  .Select(x => new AccommodatedClientsGroupModel
                                   {
                                       TableId = x.Key.TableId,
                                       TableSize = x.Key.TableSize,
                                       AvailableSeats = x.Key.TableSize - x.Sum(c => c.ClientsGroupSize)
                                   });
            query = requiredSize > 0
                ? query.Where(x => x.AvailableSeats > requiredSize)
                : query.Where(x => x.AvailableSeats > 0);
            
            var availableSeats = await query.AsNoTracking().ToListAsync(cancellationToken);

            return availableSeats;
        }

        public async Task<List<TableModel>> GetEmptyTablesAsync(int? requiredSiez = null, CancellationToken cancellationToken = default)
        {
            var tablesQuery = _dbContext.Tables
                                        .Where(x => _dbContext.AccommodatedClientsGroups
                                                              .Select(c => c.TableId)
                                                              .Distinct()
                                                              .Contains(x.Id) == false);
            if (requiredSiez.HasValue)
            {
                tablesQuery = tablesQuery.Where(c => c.Size >= requiredSiez.Value);
            }

            var emptyTables = await tablesQuery
                                   .OrderBy(x => x.Id)
                                   .AsNoTracking()
                                   .ToListAsync(cancellationToken);

            return _mapper.Map<List<TableModel>>(emptyTables);
        }

        public async Task CustomerLeavingAsync(ClientsGroupModel model, CancellationToken cancellationToken = default)
        {
            await ValidateCustomerAsync(model, cancellationToken);
            
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                await transaction.CreateSavepointAsync(RestLeaveSavepointName, cancellationToken);
                await _dbContext.AccommodatedClientsGroups
                                .Where(x => x.ClientsGroupId == model.Id)
                                .ExecuteDeleteAsync(cancellationToken);

                await _dbContext.QueuedClientsGroups.Where(x => x.ClientsGroupId == model.Id)
                                .ExecuteDeleteAsync(cancellationToken);

                await AccommodatedQueuedClientsGroupAsync(cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
                
                await transaction.CommitAsync(cancellationToken);
            }
            catch 
            {
                await transaction.RollbackToSavepointAsync(RestLeaveSavepointName, cancellationToken);
                _errorDescriber.DescribeWithException(ErrorCode.UnhandledError);
            }
        }

        public async Task<IEnumerable<TableModel>> GetAvailableSeatsAsync(CancellationToken cancellationToken = default)
        {
            var availableSeats = await GetAccommodatedClientsGroupAsync(cancellationToken: cancellationToken);
            var emptyTables = await GetEmptyTablesAsync(cancellationToken: cancellationToken);
            
            return emptyTables.Union(availableSeats.Select(c => new TableModel() { Id = c.TableId, Size = c.AvailableSeats}));
        }

        private async Task<TableModel> AccommodateClientsGroupAsync(
            List<TableModel> emptyTables, 
            Predicate<TableModel> match,
            QueuedClientsGroupEntity client,
            CancellationToken cancellationToken)
        {
            var emptyTable = emptyTables.Find(match);
            if (emptyTable != null)
            {
                await _dbContext.AccommodatedClientsGroups.AddAsync(new AccommodatedClientsGroupEntity
                {
                    TableId = emptyTable.Id,
                    ClientsGroupId = client.ClientsGroupId
                }, cancellationToken);

                await _dbContext.QueuedClientsGroups.Where(x => x.ClientsGroupId == client.ClientsGroupId)
                                .ExecuteDeleteAsync(cancellationToken);
                            
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return emptyTable;
        }

        private async Task ValidateCustomerAsync(ClientsGroupModel clientsGroup, CancellationToken cancellationToken)
        {
            var customerEntity = await _dbContext.ClientsGroups
                                                 .AsNoTracking()
                                                 .SingleOrDefaultAsync(c => c.Id == clientsGroup.Id, cancellationToken);

            if (customerEntity == null)
            {
                _errorDescriber.DescribeWithException(ErrorCode.ClientsGroupNotFound, clientsGroup.Id.ToString());
            }

            if (customerEntity.Size != clientsGroup.Size)
            {
                _errorDescriber.DescribeWithException(ErrorCode.ClientsGroupSizeMismatch);
            }
        }
    }
}
