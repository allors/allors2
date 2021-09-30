// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    using Allors;
    using Allors.Meta;

    /// <summary>
    /// A living human being.
    /// </summary>
    public partial class Person
    {
        public static Extent<Person> ExtentByLastName(ISession session) => session.Extent<Person>().AddSort(M.Person.LastName);

        public override string ToString()
        {
            if (this.ExistLastName)
            {
                if (this.ExistFirstName)
                {
                    return string.Concat(this.LastName, " ", this.FirstName);
                }

                return this.LastName;
            }

            return this.UserName;
        }

        public void CustomOnInit(ObjectOnInit method)
        {
            if (this.ExistOrganisationWhereManager)
            {
                this.OrganisationWhereManager.AddEmployee(this);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.FullName = this.FirstName + " " + this.LastName;
            this.Address = this.MainAddress;
        }
    }
}
