// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors.Data;
    using Allors.Meta;

    using Xunit;

    public class FilterTests : DomainTest
    {
        [Fact]
        public void Type()
        {
            var query = new Filter(M.Person.ObjectType);
            var queryExtent = query.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void RoleEquals()
        {
            var query = new Filter(M.Person.ObjectType)
                            {
                                Predicate = new Equals
                                                {
                                                    PropertyType = M.Person.FirstName,
                                                    Value = "John"
                                                }
                            };

            var queryExtent = query.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);
            extent.Filter.AddEquals(M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void And()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var query = new Filter(M.Person.ObjectType)
                            {
                                Predicate = new And
                                                {
                                                    Operands = new IPredicate[]
                                                                {
                                                                    new Equals
                                                                       {
                                                                           PropertyType = M.Person.FirstName,
                                                                           Value = "John"
                                                                       },
                                                                    new Equals
                                                                        {
                                                                            PropertyType = M.Person.LastName,
                                                                            Value = "Doe"
                                                                        }
                                                                }
                                }
                            };

            var queryExtent = query.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);
            var and = extent.Filter.AddAnd();
            and.AddEquals(M.Person.FirstName, "John");
            and.AddEquals(M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

    }
}
