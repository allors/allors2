// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceLocator.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services.Base
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class ServiceLocator : IServiceLocator
    {
        public Func<IUserService> UserServiceFactory { get; set; }

        public Func<ITimeService> TimeServiceFactory { get; set; }

        public Func<IMailService> MailServiceFactory { get; set; }

        public Func<ISecurityService> SecurityServiceFactory { get; set; }

        public IUserService CreateUserService()
        {
            return this.UserServiceFactory();
        }

        public ITimeService CreateTimeService()
        {
            return this.TimeServiceFactory();
        }

        public IMailService CreateMailService()
        {
            return this.MailServiceFactory();
        }

        public ISecurityService CreateSecurityService()
        {
            return this.SecurityServiceFactory();
        }

        public ServiceLocator Assert()
        {
            var missingFactories = this.GetType()
                .GetProperties()
                .Where(v => v.GetValue(this) == null)
                .ToArray();

            if (missingFactories.Length > 0)
            {
                var missingFactoryNames = string.Join(",", missingFactories.Select(v => v.Name));
                throw new Exception($"Missing service factories: {missingFactoryNames}");
            }

            return this;
        }
    }

}