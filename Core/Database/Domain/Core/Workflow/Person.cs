// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Person
    {
        public bool IsAdministrator
        {
            get
            {
                var administrators = new UserGroups(this.Session()).Administrators;
                return administrators.Members.Contains(this);
            }
        }

        public void CoreDelete(DeletableDelete method)
        {
            if (this.ExistOwnerAccessControl)
            {
                this.OwnerAccessControl.Delete();
            }

            if (this.ExistOwnerSecurityToken)
            {
                this.OwnerSecurityToken.Delete();
            }
        }

        public void CoreOnBuild(ObjectOnBuild method) => this.DeriveOwnerSecurity();

        public void CoreOnDerive(ObjectOnDerive method) => this.DeriveOwnerSecurity();

        public void DeriveOwnerSecurity()
        {
            if (!this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(this.Strategy.Session).Owner;
                this.OwnerAccessControl = new AccessControlBuilder(this.Strategy.Session)
                        .WithRole(ownerRole)
                        .WithSubject(this)
                        .Build();
            }

            if (!this.ExistOwnerSecurityToken)
            {
                this.OwnerSecurityToken = new SecurityTokenBuilder(this.Strategy.Session)
                    .WithAccessControl(this.OwnerAccessControl)
                    .Build();
            }
        }
    }
}
