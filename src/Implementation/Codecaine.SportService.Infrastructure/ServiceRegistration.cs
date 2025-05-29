using Codecaine.Common;
using Codecaine.Common.AspNetCore.OpenApi;
using Codecaine.Common.Authentication;
using Codecaine.Common.Authentication.Providers.KeyCloak;
using Codecaine.Common.Authentication.Providers.KeyCloak.Setting;
using Codecaine.Common.Caching;
using Codecaine.Common.Caching.Redis;
using Codecaine.Common.Caching.Settings;
using Codecaine.Common.Domain;
using Codecaine.Common.Messaging;
using Codecaine.Common.Messaging.MassTransit;
using Codecaine.Common.Notifications;
using Codecaine.Common.Notifications.Email;
using Codecaine.Common.Notifications.Sms;
using Codecaine.Common.Notifications.Whatsapp;
using Codecaine.Common.Persistence;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.Common.Persistence.MongoDB;
using Codecaine.Common.Persistence.MongoDB.Interfaces;
using Codecaine.Common.Storage;
using Codecaine.Common.Storage.Factory;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Codecaine.Common.Storage.Providers.Minio;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Infrastructure.DataAccess;
using Codecaine.SportService.Infrastructure.DataAccess.Repositories;
using Codecaine.SportService.Infrastructure.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Net.Mail;



namespace Codecaine.SportService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Persistence
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString__DataBase") ?? "";
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<DataContext>());
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<DataContext>());

            // Persistence MongoDb

            services.AddOptions<MongoDbSetting>().BindConfiguration("MongoDbSetting");
            services.AddScoped<MongoDbContext>(); // Register the implementation once
            services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<MongoDbContext>());
            services.AddScoped<INoSqlUnitOfWork>(sp => sp.GetRequiredService<MongoDbContext>());

            BsonClassMap.RegisterClassMap<Entity>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(p => p.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });

            BsonClassMap.RegisterClassMap<Player>(cm =>
            {
                cm.AutoMap();
                var guidSerializer = new GuidSerializer(GuidRepresentation.Standard);

                cm.MapMember(p => p.CreatedBy)
                  .SetSerializer(new NullableSerializer<Guid>(guidSerializer));

                cm.MapMember(p => p.ModifiedBy)
                  .SetSerializer(new NullableSerializer<Guid>(guidSerializer));
            });

            // Repositories
            services.AddScoped<ISportTypeRepository, SportTypeRepository>();
            services.AddScoped<ISportVariantRepository, SportVariantRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            // MassTransit Publisher
            services.AddScoped<IMessagePublisher, MessagePublisher>();
            services.AddMassTransitRabbitMq();

            // Caching
            services.AddOptions<CachingSetting>().BindConfiguration(CachingSetting.DefaultSectionName);
            services.AddScoped<ICacheService, CacheService>();

            // Authentication
            var realmName = Environment.GetEnvironmentVariable("Authentication__RealmName");
            var baseUrl = Environment.GetEnvironmentVariable("Authentication__BaseUrl");
            services.AddOptions<AuthenticationSetting>().BindConfiguration(AuthenticationSetting.DefaultSectionName);
            services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.MetadataAddress = $"{baseUrl}/realms/{realmName}/.well-known/openid-configuration";
                o.Authority = $"{baseUrl}/realms/{realmName}";
                o.Audience = "account";
                o.RequireHttpsMetadata = false; // ---> Will be issue if not configure https TODO : configure https 
            });

            services.AddOpenApi(options =>
            {

                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });

            services.AddAuthorization();

            // Common Library
            services.AddCommonLibrary();

            // Storage Providers --> Minio

            services.AddSingleton<IAmazonS3UtilityWrapper, AmazonS3UtilityWrapper>();
            services.AddSingleton<IStorageProviderFactory, StorageProviderFactory>();

            var accessKey = Environment.GetEnvironmentVariable("StorageProvider__AccessKey");
            var secretKey = Environment.GetEnvironmentVariable("StorageProvider__SecretKey");
            var bucketName = Environment.GetEnvironmentVariable("StorageProvider__BucketName");
            var region = Environment.GetEnvironmentVariable("StorageProvider__Region");
            var endPoint = Environment.GetEnvironmentVariable("StorageProvider__EndPoint");
            var minioProvider = new MinioProvider(
                    bucketName,
                    accessKey,
                    secretKey,
                    region,
                    endPoint
                );

            services.AddScoped<IStorageProvider>(sp =>
            {
                var storageProviderFactory = sp.GetRequiredService<IStorageProviderFactory>();
                return storageProviderFactory.CreateProvider(minioProvider);
            });

            // Notification
            services.AddScoped<INotificationSender, NotificationDispatcher>();
            services.AddScoped<INotificationChannelSender, EmailNotificationSender>();
            services.AddScoped<INotificationChannelSender, WhatsappNotificationSender>();
            services.AddScoped<INotificationChannelSender, SmsNotificationSender>();

            // Notification Email
            var fromEmail = Environment.GetEnvironmentVariable("FluentEmailSetting__FromEmail");
            var smtpServer = Environment.GetEnvironmentVariable("FluentEmailSetting__SmtpServer");
            var smtpPort = Environment.GetEnvironmentVariable("FluentEmailSetting__SmtpPort");
            var smtpUser = Environment.GetEnvironmentVariable("FluentEmailSetting__SmtpUser");
            var smtpPassword = Environment.GetEnvironmentVariable("FluentEmailSetting__SmtpPassword");
            services.AddFluentEmail(fromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(smtpServer)
                    {
                         Port = int.Parse(smtpPort),
                         Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword),
                         EnableSsl = true,
                    });

            // Notification Whatsapp
            services.AddOptions<WhatsappSetting>().BindConfiguration(WhatsappSetting.DefaultSectionName);


            return services;
        }

        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services)
        {
            string host = Environment.GetEnvironmentVariable("RabbitMq__Host") ?? "";
            string userName = Environment.GetEnvironmentVariable("RabbitMq__UserName") ?? "";
            string password = Environment.GetEnvironmentVariable("RabbitMq__Password") ?? "";
            string defaultQueueName = Environment.GetEnvironmentVariable("RabbitMq__DefaultQueueName") ?? "";


            services.AddMassTransit(x =>
            {
                x.AddConsumer<CodecaineMessageConsumer>(); // Register Consumer
              

                // Add multiple consumers if needed

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(userName);
                        h.Password(password);
                    });

                    cfg.UseRawJsonSerializer();

                    cfg.ReceiveEndpoint(defaultQueueName, e =>
                    {
                        e.ConfigureConsumer<CodecaineMessageConsumer>(context);
                    });                    

                    // Configure other consumers here

                    cfg.ConfigureEndpoints(context);
                });


            });

            return services;
        }

       
    }
}
