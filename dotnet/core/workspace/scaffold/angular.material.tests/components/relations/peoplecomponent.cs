// <copyright file="PeopleComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace libs.angular.material.custom.src.relations.people
{
    using Allors.Domain;
    using Components;
    using person;

    public partial class PeopleComponent
    {
        public MatTable Table => new MatTable(this.Driver);

        public PersonOverviewComponent Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();

            return new PersonOverviewComponent(this.Driver);
        }
    }
}
