// <copyright file="ServiceCollectionExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Services
{
    using System;
    using Domain;
    using Domain.Derivations;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static void AddAllors(this IServiceCollection services, Func<ISession, IDerivation> factory = null)
        {
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
            services.AddSingleton<IDerivationService>(new DerivationService { Factory = factory });
            services.AddSingleton<IBarcodeService, BarcodeService>();

            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
