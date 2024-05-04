using ContentManagementService.Business.Implementation;
using ContentManagementService.Business.Interface;
using ContentManagementService.Core.AppSettings;
using ContentManagementService.Data.Implementation;
using ContentManagementService.Data.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContentManagementService.API
{
    public static class ServicesConfigurator
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAppSettings(configuration);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserResolver, UserResolver>();

            services.ConfigureDataAccessServices();

            services.ConfigureBusinessServices();
        }

        private static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
        }

        private static void ConfigureDataAccessServices(this IServiceCollection services)
        {
            //services.AddScoped<IBaseServiceDataAccess, BaseServiceDataAccess>();
            services.AddScoped<IPostServiceDataAccess, PostServiceDataAccess>();
        }

        private static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IPostService, PostService>();
        }
    }
}
