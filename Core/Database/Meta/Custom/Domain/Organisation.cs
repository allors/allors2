// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaOrganisation
    {
        public Tree AngularEmployees { get; private set; }

        public Tree AngularShareholders { get; private set; }

        internal override void CustomExtend()
        {
            this.Name.IsRequired = true;

            var organisation = this;
            var person = MetaPerson.Instance;

            this.AngularEmployees = new Tree()
                .Add(organisation.Employees);

            this.AngularShareholders = new Tree()
                .Add(organisation.Shareholders, new Tree()
                        .Add(person.Photo));
        }
    }
}
