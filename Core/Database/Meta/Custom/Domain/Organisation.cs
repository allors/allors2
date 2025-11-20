// <copyright file="Organisation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaOrganisation
    {
        public Node[] AngularEmployees { get; private set; }

        public Node[] AngularShareholders { get; private set; }

        internal override void CustomExtend()
        {
            this.Name.IsRequired = true;

            var organisation = this;
            var person = MetaPerson.Instance;

            this.AngularEmployees = new[]
            {
                new Node(organisation.Employees),
            };

            this.AngularShareholders = new[] {
                new Node(organisation.Shareholders)
                    .Add(person.Photo),
            };
        }
    }
}
