// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisations.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    public partial class Organisations
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };
            config.GrantAdministrator(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.Name, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.LegalForm, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.LogoImage, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.TaxNumber, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.CurrentSalesReps, Operations.Read);
            config.GrantCustomer(this.ObjectType, Meta.OpenOrderAmount, Operations.Read);
            config.GrantCustomer(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);

            config.GrantSupplier(this.ObjectType, Meta.Name, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.LegalForm, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.LogoImage, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.TaxNumber, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.CurrentSalesReps, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.OpenOrderAmount, Operations.Read);
            config.GrantSupplier(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);
          
            config.GrantPartner(this.ObjectType, Meta.Name, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.LegalForm, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.LogoImage, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.TaxNumber, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.CurrentSalesReps, Operations.Read);
            config.GrantPartner(this.ObjectType, Meta.OpenOrderAmount, Operations.Read);
            config.GrantPartner(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);
        }
    }
}