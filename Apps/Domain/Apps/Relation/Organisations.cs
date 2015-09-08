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

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };
            config.GrantAdministrator(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.Name, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.LegalForm, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.LogoImage, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.TaxNumber, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.CurrentSalesReps, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.OpenOrderAmount, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);

            config.GrantSupplier(this.ObjectType, Meta.Name, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.LegalForm, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.LogoImage, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.TaxNumber, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.CurrentSalesReps, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.OpenOrderAmount, Operation.Read);
            config.GrantSupplier(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);
          
            config.GrantPartner(this.ObjectType, Meta.Name, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.LegalForm, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.LogoImage, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.TaxNumber, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.CurrentSalesReps, Operation.Read);
            config.GrantPartner(this.ObjectType, Meta.OpenOrderAmount, Operation.Read);
            config.GrantPartner(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);
        }
    }
}