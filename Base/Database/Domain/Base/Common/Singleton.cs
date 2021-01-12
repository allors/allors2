// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

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
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.IsCreated(this) || changeSet.HasChangedRole(this, this.Meta.AdditionalLocales))
            {
                foreach (Good product in new Goods(this.Strategy.Session).Extent())
                {
                    iteration.Mark(product);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var session = this.Strategy.Session;

            if (!this.ExistLogoImage)
            {
                this.LogoImage = new MediaBuilder(this.Strategy.Session).WithInFileName("allors.png").WithInData(this.GetResourceBytes("allors.png")).Build();
            }

            this.Locales = this.AdditionalLocales;
            this.AddLocale(this.DefaultLocale);
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

        public int SortableNumber(string prefix, string identification, string year)
        {
            if (prefix != null)
            {
                if (prefix.Contains("{year}"))
                {
                    var number = int.Parse(identification.Substring(prefix.Length - 2)).ToString("000000");

                    // this.Store.SalesInvoiceNumberPrefix.Length - 2 because of {} in this string
                    return int.Parse(string.Concat(year, number));
                }
                else
                {
                    return int.Parse(identification.Substring(prefix.Length));
                }
            }
            else
            {
                return int.Parse(identification);
            }
        }
    }
}
