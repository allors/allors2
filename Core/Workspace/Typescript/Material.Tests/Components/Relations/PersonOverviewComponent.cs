// <copyright file="PersonOverviewComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
