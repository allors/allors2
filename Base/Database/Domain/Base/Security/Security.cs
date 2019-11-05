// <copyright file="Security.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class Security
    {
        public void GrantExceptEmployee(ObjectType objectType, ICollection<IOperandType> excepts, params Operations[] operations) => this.GrantExcept(Roles.EmployeeId, objectType, excepts, operations);

        public void GrantExceptGuest(ObjectType objectType, ICollection<IOperandType> excepts, params Operations[] operations) => this.GrantExcept(Roles.GuestId, objectType, excepts, operations);

        private void BaseOnPreSetup()
        {
        }

        private void BaseOnPostSetup()
        {
        }
    }
}
