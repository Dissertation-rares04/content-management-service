using ContentManagementService.Core.AppSettings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ContentManagementService.Data.Implementation
{
    public class BaseServiceDataAccess
    {
        protected readonly IMongoDatabase _mongoDatabase;

        public BaseServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }
    }
}
