// <copyright file="Person.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaPerson
    {
        public Node[] AngularHome;

        internal override void CustomExtend()
        {
            this.Delete.Workspace = true;

            this.FirstName.RelationType.Workspace = true;
            this.LastName.RelationType.Workspace = true;
            this.MiddleName.RelationType.Workspace = true;

            var person = this;
            this.AngularHome = new[]
                {
                    new Node(person.Photo),
                };
        }
    }
}
