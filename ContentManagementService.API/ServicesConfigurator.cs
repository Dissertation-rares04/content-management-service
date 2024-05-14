using ContentManagementService.Business.Implementation;
using ContentManagementService.Business.Interface;
using ContentManagementService.Core.AppSettings;
using ContentManagementService.Data.Implementation;
using ContentManagementService.Data.Interface;
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
            services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
            services.Configure<KafkaTopics>(configuration.GetSection("KafkaTopics"));
        }

        private static void ConfigureDataAccessServices(this IServiceCollection services)
        {
            //services.AddScoped<IBaseServiceDataAccess, BaseServiceDataAccess>();
            services.AddScoped<IPostServiceDataAccess, PostServiceDataAccess>();
            services.AddScoped<ICommentServiceDataAccess, CommentServiceDataAccess>();
        }

        private static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
        }
    }
}
