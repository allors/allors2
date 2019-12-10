// <copyright file="CreditCardCompanies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CreditCardCompanies
    {
        private static readonly Guid VisaId = new Guid("E2425837-1586-4A99-9389-1EB9348414C8");
        private static readonly Guid MasterCardId = new Guid("02BEC092-F9AC-49A2-92CC-D1E916E1F240");
        private static readonly Guid AmericanExpressId = new Guid("6977672C-C212-429B-A86B-BA905F0833B2");

        private UniquelyIdentifiableSticky<CreditCardCompany> cache;

        public CreditCardCompany Visa => this.Cache[VisaId];

        public CreditCardCompany LiFo => this.Cache[MasterCardId];

        public CreditCardCompany Average => this.Cache[AmericanExpressId];

        private UniquelyIdentifiableSticky<CreditCardCompany> Cache => this.cache ??= new UniquelyIdentifiableSticky<CreditCardCompany>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(VisaId, v => v.Name = "Visa");
            merge(MasterCardId, v => v.Name = "Master Card");
            merge(AmericanExpressId, v => v.Name = "American Express");
        }
    }
}
