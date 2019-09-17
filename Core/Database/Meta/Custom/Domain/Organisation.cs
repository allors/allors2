// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta
{
    using Allors.Data;

    public partial class MetaOrganisation
    {
        public TreeNode[] AngularEmployees { get; private set; }

        public TreeNode[] AngularShareholders { get; private set; }

        internal override void CustomExtend()
        {
            this.Name.IsRequired = true;

            var organisation = this;
            var person = MetaPerson.Instance;

            this.AngularEmployees = new[]
            {
                new TreeNode(organisation.Employees),
            };

            this.AngularShareholders = new[] {
                new TreeNode(organisation.Shareholders)
                    .Add(person.Photo),
            };
        }
    }
}
