// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Persons.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public partial class Persons
    {
        public static readonly Guid AdministratorId = new Guid("FF791BA1-6E02-4F64-83A3-E6BEE1208C11");
        public static readonly Guid GuestId = new Guid("1261CB56-67F2-4725-AF7D-604A117ABBEC");

        public static void AppsOnDeriveCommissions(ISession session)
        {
            foreach (Person person in session.Extent<Person>())
            {
                if (person.ExistSalesRepRevenuesWhereSalesRep)
                {
                    person.DeriveCommission();
                }
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(Meta.ObjectType, Roles.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            config.GrantCustomer(this.ObjectType, Meta.BirthDate, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Citizenship, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.FirstName, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Gender, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.LastName, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.MaritalStatus, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.MiddleName, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.MothersMaidenName, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Passports, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Picture, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.PreferredCurrency, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Titles, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.Salutation, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.SocialSecurityNumber, Operation.Read, Operation.Write);
            config.GrantCustomer(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);

            config.GrantSales(this.ObjectType, Meta.BirthDate, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Citizenship, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.FirstName, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Gender, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.LastName, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.MaritalStatus, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.MiddleName, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.MothersMaidenName, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Passports, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Picture, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.PreferredCurrency, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Titles, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.Salutation, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.SocialSecurityNumber, Operation.Read, Operation.Write);
            config.GrantSales(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);

            config.GrantSupplier(this.ObjectType, Meta.BirthDate, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Citizenship, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.FirstName, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Gender, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.LastName, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.MaritalStatus, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.MiddleName, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.MothersMaidenName, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Passports, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Picture, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.PreferredCurrency, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Titles, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.Salutation, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.SocialSecurityNumber, Operation.Read, Operation.Write);
            config.GrantSupplier(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);

            config.GrantPartner(this.ObjectType, Meta.BirthDate, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Citizenship, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.FirstName, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Gender, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.LastName, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.MaritalStatus, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.MiddleName, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.MothersMaidenName, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.PartyContactMechanisms, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Passports, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Picture, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.PreferredCurrency, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Locale, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Titles, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.Salutation, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.SocialSecurityNumber, Operation.Read, Operation.Write);
            config.GrantPartner(this.ObjectType, Meta.BankAccounts, Operation.Read, Operation.Write);
        }
    }
}