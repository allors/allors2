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
    using Allors.Meta;

    public partial class People
    {
        public static readonly Guid AdministratorId = new Guid("FF791BA1-6E02-4F64-83A3-E6BEE1208C11");
        public static readonly Guid GuestId = new Guid("1261CB56-67F2-4725-AF7D-604A117ABBEC");

        public static void AppsOnDeriveCommissions(ISession session)
        {
            foreach (Person person in session.Extent<Person>())
            {
                if (person.ExistSalesRepRevenuesWhereSalesRep)
                {
                    person.AppsOnDeriveCommission();
                }
            }
        }

        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.Role);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };
            config.GrantOwner(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.BirthDate, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Citizenship, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.FirstName, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Gender, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.LastName, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.MaritalStatus, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.MiddleName, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.MothersMaidenName, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Passports, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Picture, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.PreferredCurrency, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Titles, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.Salutation, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.SocialSecurityNumber, Operations.Read, Operations.Write);
            config.GrantCustomer(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);

            config.GrantSales(this.ObjectType, Meta.BirthDate, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Citizenship, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.FirstName, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Gender, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.LastName, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.MaritalStatus, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.MiddleName, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.MothersMaidenName, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Passports, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Picture, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.PreferredCurrency, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Titles, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.Salutation, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.SocialSecurityNumber, Operations.Read, Operations.Write);
            config.GrantSales(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);

            config.GrantSupplier(this.ObjectType, Meta.BirthDate, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Citizenship, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.FirstName, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Gender, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.LastName, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.MaritalStatus, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.MiddleName, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.MothersMaidenName, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Passports, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Picture, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.PreferredCurrency, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Titles, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.Salutation, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.SocialSecurityNumber, Operations.Read, Operations.Write);
            config.GrantSupplier(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);

            config.GrantPartner(this.ObjectType, Meta.BirthDate, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Citizenship, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.FirstName, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Gender, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.LastName, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.MaritalStatus, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.MiddleName, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.MothersMaidenName, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.PartyContactMechanisms, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Passports, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Picture, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.PreferredCurrency, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Locale, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Titles, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.Salutation, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.SocialSecurityNumber, Operations.Read, Operations.Write);
            config.GrantPartner(this.ObjectType, Meta.BankAccounts, Operations.Read, Operations.Write);
        }
    }
}