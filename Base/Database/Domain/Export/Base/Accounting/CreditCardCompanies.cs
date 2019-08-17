// <copyright file="CreditCardCompanies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class CreditCardCompanies
    {
        protected override void BaseSetup(Setup setup)
        {
            new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build();
            new CreditCardCompanyBuilder(this.Session).WithName("Master Card").Build();
            new CreditCardCompanyBuilder(this.Session).WithName("American Express").Build();
        }
    }
}
