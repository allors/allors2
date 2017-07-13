// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryTests.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain.Query;
    using Allors.Meta;

    using Xunit;

    public class QueryTests : DomainTest
    {
        [Fact]
        public void Type()
        {
            var query = new Query
                            {
                                ObjectType = M.Person.ObjectType
                            };

            var queryExtent = this.Session.Query(query);

            var extent = this.Session.Extent(M.Person.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void RoleEquals()
        {
            var query = new Query
                            {
                                ObjectType = M.Person.ObjectType,
                                Predicate = new Equals
                                                {
                                                    RoleType = M.Person.FirstName,
                                                    Value = "John"
                                                }
                            };

            var queryExtent = this.Session.Query(query);

            var extent = this.Session.Extent(M.Person.ObjectType);
            extent.Filter.AddEquals(M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void And()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var query = new Query
                            {
                                ObjectType = M.Person.ObjectType,
                                Predicate = new And
                                                {
                                                    Predicates = new List<Predicate>
                                                                {
                                                                    new Equals
                                                                       {
                                                                           RoleType = M.Person.FirstName,
                                                                           Value = "John"
                                                                       },
                                                                    new Equals
                                                                        {
                                                                            RoleType = M.Person.LastName,
                                                                            Value = "Doe"
                                                                        }
                                                                }
                                }
                            };

            var queryExtent = this.Session.Query(query);

            var extent = this.Session.Extent(M.Person.ObjectType);
            var and = extent.Filter.AddAnd();
            and.AddEquals(M.Person.FirstName, "John");
            and.AddEquals(M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

    }
}
