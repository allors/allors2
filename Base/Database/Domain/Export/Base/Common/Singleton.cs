
// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Allors.Meta;

    /// <summary>
    /// The Application object serves as the singleton for
    /// your population.
    /// It is the ideal place to hold application settings
    /// (e.g. the domain, the guest user, ...).
    /// </summary>
    public partial class Singleton
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRole(this, this.Meta.AdditionalLocales))
            {
                foreach (Good product in new Goods(this.Strategy.Session).Extent())
                {
                    derivation.Add(product);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var session = this.Strategy.Session;

            if (!this.ExistLogoImage)
            {
                this.LogoImage = new MediaBuilder(this.Strategy.Session).WithInData(this.GetResourceBytes("allors.png")).Build();
            }

            this.SalesAccountManagerUserGroup.Members = this.SalesAccountManagers.ToArray();
        }

        private byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames().First(v => v.Contains(name));
            var resource = assembly.GetManifestResourceStream(manifestResourceName);
            if (resource != null)
            {
                using (var ms = new MemoryStream())
                {
                    resource.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
