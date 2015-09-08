//------------------------------------------------------------------------------------------------- 
// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
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
// <summary>Defines the role type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    public partial class UserGroups
    {
        public static readonly Guid CustomersId = new Guid("C6D22B23-2790-45CF-B25B-D1CD87AD6798");
        public static readonly Guid SuppliersId = new Guid("5A8198E7-ED93-4237-B3E8-86680F789650");
        public static readonly Guid PartnersId = new Guid("A2C66505-5278-4B8A-8231-5D6B9F207992");

        public static readonly Guid OperationsId = new Guid("4EA028A4-57C6-46A1-AC4B-E18204F9B498");
        public static readonly Guid SalesId = new Guid("1511E4E2-829F-4133-8824-B94ED46E6BED");
        public static readonly Guid ProcurementId = new Guid("FF887B58-CDA3-4C76-8308-0F005E362E0E");

        private static ISet<Guid> administratorIdSet;
       
        public static ISet<Guid> AdministratorIdSet
        {
            get
            {
                return administratorIdSet ?? (administratorIdSet = new HashSet<Guid> { AdministratorsId });
            }
        }

        public UserGroup Customers
        {
            get { return this.Cache.Get(CustomersId); }
        }

        public UserGroup Suppliers
        {
            get { return this.Cache.Get(SuppliersId); }
        }

        public UserGroup Partners
        {
            get { return this.Cache.Get(PartnersId); }
        }

        public UserGroup Operations
        {
            get { return this.Cache.Get(OperationsId); }
        }

        public UserGroup Sales
        {
            get { return this.Cache.Get(SalesId); }
        }

        public UserGroup Procurement
        {
            get { return this.Cache.Get(ProcurementId); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new UserGroupBuilder(Session).WithName("customers").WithUniqueId(CustomersId).WithRole(new Roles(Session).Customer).Build();
            new UserGroupBuilder(Session).WithName("suppliers").WithUniqueId(SuppliersId).WithRole(new Roles(Session).Supplier).Build();
            new UserGroupBuilder(Session).WithName("partners").WithUniqueId(PartnersId).WithRole(new Roles(Session).Partner).Build();

            new UserGroupBuilder(Session).WithName("operations").WithUniqueId(OperationsId).WithRole(new Roles(Session).Operations).Build();
            new UserGroupBuilder(Session).WithName("sales").WithUniqueId(SalesId).WithRole(new Roles(Session).Sales).Build();
            new UserGroupBuilder(Session).WithName("procurement").WithUniqueId(ProcurementId).WithRole(new Roles(Session).Procurement).Build();

            new SecurityCache(this.Session).Invalidate();
        }
    }
}
