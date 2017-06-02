//------------------------------------------------------------------------------------------------- 
// <copyright file="Person.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Person type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using global::System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    /// <summary>
    /// A living human being.
    /// </summary>
    public partial class Person
    {
        public static Extent<Person> ExtentByLastName(ISession session)
        {
            return session.Extent<Person>().AddSort(M.Person.LastName);
        }

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

            return base.ToString();
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            // Validation
            if (!Users.GuestUserName.Equals(this.UserName) && !Users.AdministratorUserName.Equals(this.UserName))
            {
                derivation.Validation.AssertExists(this, M.Person.LastName);
            }

            // Derivation
            if (this.ExistFirstName && this.ExistLastName)
            {
                this.FullName = this.FirstName + " " + this.LastName;
            }
            else if (this.ExistFirstName)
            {
                this.FullName = this.FirstName;
            }
            else
            {
                this.FullName = this.LastName;
            }
        }
    }
}
