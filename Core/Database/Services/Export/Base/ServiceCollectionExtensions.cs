//------------------------------------------------------------------------------------------------- 
// <copyright file="ServiceCollectionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static void AddAllors(this IServiceCollection services, ServiceConfig config = null)
        {
            config = config ?? new ServiceConfig();

            services.AddSingleton<ITreeService, TreeService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddSingleton<IStickyService, StickyService>();
            services.AddSingleton<IStateService, StateService>();
            services.AddSingleton<ITimeService, TimeService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IExtentService, ExtentService>();
            services.AddSingleton<IFetchService, FetchService>();
            services.AddSingleton<IDerivationService>(new DerivationService(config.DerivationConfig));
            services.AddSingleton<IBarcodeService, BarcodeService>();

            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
