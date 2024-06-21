using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.ApiModels;
using MyRestaurant.Common.Enums;
using MyRestaurant.Contracts.Services;
using MyRestaurant.Data;
using MyRestaurant.Data.Entities;

namespace MyRestaurant.Core.Services
{
    /// <summary>
    /// A default implementation of <see cref="ICustomerManager"/>.
    /// </summary>
    public class CustomerManager : ICustomerManager
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IErrorDescriber _errorDescriber;
        
        public CustomerManager(
            RestaurantDbContext dbContext,
            IMapper mapper,
            IErrorDescriber errorDescriber)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _errorDescriber = errorDescriber;
        }

        public async Task<CustomerModel> CreateAsync(CustomerModel customer, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customer?.PhoneNumber) || string.IsNullOrWhiteSpace(customer.Email))
            {
                return null;
            }

            var customerQuery = BuildFetchCustomerQuery(customer);
            var customerEntity = await customerQuery
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(cancellationToken);

            if (customerEntity == null)
            {
                customerEntity = _mapper.Map<CustomerEntity>(customer);
                await _dbContext.Customers.AddAsync(customerEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<CustomerModel>(customerEntity);
        }
        
        public async Task<ClientsGroupModel> CustomerArrivedAsync(ClientsGroupModel clientsGroup, CancellationToken cancellationToken = default)
        {
            var maxSeats = await _dbContext.Tables.MaxAsync(x => x.Size, cancellationToken);
            
            if (clientsGroup.Size > maxSeats)
            {
                _errorDescriber.DescribeWithException(ErrorCode.RestaurantCannotServeLargeGroup, clientsGroup.Size.ToString());
            }

            var customer = await CreateAsync(clientsGroup.Customer, cancellationToken);

            var clientGroupEntity = _mapper.Map<ClientsGroupEntity>(clientsGroup);
            clientGroupEntity.Id = Guid.NewGuid();
            clientGroupEntity.CustomerId = customer?.Id;

            await _dbContext.ClientsGroups.AddAsync(clientGroupEntity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            clientsGroup.Id = clientGroupEntity.Id;
            clientsGroup.Customer.Id = clientGroupEntity.CustomerId ?? 0;
            
            return clientsGroup;
        }

        private IQueryable<CustomerEntity> BuildFetchCustomerQuery(CustomerModel customer)
        {
            var phoneNumber = customer.PhoneNumber?.Trim();
            var email = customer.Email?.Trim();
            
            var customerQuery = _dbContext.Customers.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(phoneNumber) && !string.IsNullOrWhiteSpace(email))
            {
                customerQuery = customerQuery.Where(x => x.PhoneNumber == phoneNumber || x.Email == email);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    customerQuery = customerQuery.Where(x => x.PhoneNumber == phoneNumber);
                }
                
                if (!string.IsNullOrWhiteSpace(email))
                {
                    customerQuery = customerQuery.Where(x => x.Email == email);
                }
            }

            return customerQuery;
        }
    }
}
