// <copyright file="PersonOverviewComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace src.allors.material.custom.relations.people.person
{
    public partial class PersonOverviewComponent
    {
        public PersonComponent EditAndNavigate()
        {
            this.Edit.Click();
            return new PersonComponent(this.Driver);
        }
    }
}
