// <copyright file="Singletons.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class AccessControls
    {
        public static readonly Guid SalesId = new Guid("9DD281CA-E699-4A2E-8C4F-BCA6CC7B227F");
        public static readonly Guid OperationsId = new Guid("88F6061E-9677-4AA1-ACAC-D7972D527941");
        public static readonly Guid ProcurementId = new Guid("91083059-28D5-419E-B47D-D88E7A621D54");

        public AccessControl Sales => this.Cache[SalesId];

        public AccessControl Operations => this.Cache[OperationsId];

        public AccessControl Procurement => this.Cache[ProcurementId];

        protected override void CustomSetup(Setup setup)
        {
            if (setup.Config.SetupSecurity)
            {
                var merge = this.Cache.Merger().Action();

                var roles = new Roles(this.Session);
                var userGroups = new UserGroups(this.Session);

                merge(SalesId, v =>
                {
                    v.Role = roles.Creator;
                    v.AddSubjectGroup(userGroups.Sales);
                });

                merge(OperationsId, v =>
                {
                    v.Role = roles.Creator;
                    v.AddSubjectGroup(userGroups.Operations);
                });

                merge(ProcurementId, v =>
                {
                    v.Role = roles.Administrator;
                    v.AddSubjectGroup(userGroups.Procurement);
                });
            }
        }
    }
}
