using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyRestaurant.ApiModels;
using MyRestaurant.Common;
using MyRestaurant.Data;
using MyRestaurant.Data.Entities;
using Newtonsoft.Json;

namespace MyRestaurant.WebApi.Services
{
    /// <summary>
    /// Contains methods that are invoked at app startup in order to initialize needed items.
    /// </summary>
    public class AppInitializer
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly AppErrorDictionary _errorDictionary;

        public AppInitializer(
            RestaurantDbContext dbContext,
            AppErrorDictionary errorDictionary
            )
        {
            _dbContext = dbContext;
            _errorDictionary = errorDictionary;
        }

        public async Task InitializeAsync()
        {
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "app_errors.json");
            var fileContent = await File.ReadAllTextAsync(fileName);
            var errors = JsonConvert.DeserializeObject<AppErrorModel[]>(fileContent);
            Array.ForEach(errors, error => _errorDictionary.Add(error.ErrorCode, error.Description));

            if (!(await _dbContext.Tables.AnyAsync()))
            {
                fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources", "sample_data.json");
                fileContent = await File.ReadAllTextAsync(fileName);
                var tables = JsonConvert.DeserializeObject<TableEntity[]>(fileContent);
                await _dbContext.Tables.AddRangeAsync(tables);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
