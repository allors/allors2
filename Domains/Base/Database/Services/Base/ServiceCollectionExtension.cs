//------------------------------------------------------------------------------------------------- 
// <copyright file="ServiceCollectionExtension.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using Microsoft.AspNetCore.Http;
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