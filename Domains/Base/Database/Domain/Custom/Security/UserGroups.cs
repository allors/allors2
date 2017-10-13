//------------------------------------------------------------------------------------------------- 
// <copyright file="UserGroups.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
    using global::System;

    public partial class UserGroups
    {
        public static readonly Guid OperationsId = new Guid("4EA028A4-57C6-46A1-AC4B-E18204F9B498");
        public static readonly Guid SalesId = new Guid("1511E4E2-829F-4133-8824-B94ED46E6BED");
        public static readonly Guid ProcurementId = new Guid("FF887B58-CDA3-4C76-8308-0F005E362E0E");

        public UserGroup Operations => this.Sticky[OperationsId];

        public UserGroup Sales => this.Sticky[SalesId];

        public UserGroup Procurement => this.Sticky[ProcurementId];

        protected override void CustomSetup(Setup setup)
        {
            base.CustomSetup(setup);

            new UserGroupBuilder(this.Session).WithName("operations").WithUniqueId(OperationsId).Build();
            new UserGroupBuilder(this.Session).WithName("sales").WithUniqueId(SalesId).Build();
            new UserGroupBuilder(this.Session).WithName("procurement").WithUniqueId(ProcurementId).Build();
        }
    }
}
