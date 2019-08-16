// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
            var filter = new Filter(M.Person.ObjectType)
            {
                Predicate = new Equals
                {
                    PropertyType = M.Person.FirstName,
                    Value = "John"
                }
            };

            var queryExtent = filter.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);
            extent.Filter.AddEquals(M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void And()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var filter = new Filter(M.Person.ObjectType)
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

            var queryExtent = filter.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);
            var and = extent.Filter.AddAnd();
            and.AddEquals(M.Person.FirstName, "John");
            and.AddEquals(M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }
    }
}
