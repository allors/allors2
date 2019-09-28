// <copyright file="ParametrizedTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Tests
{
    using System.Collections.Generic;

    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Data;
    using Xunit;

    public class ParametrizedTests : DomainTest
    {
        [Fact]
        public void EqualsWithParameters()
        {
            var filter = new Filter(M.Person.ObjectType)
            {
                Predicate = new Equals { PropertyType = M.Person.FirstName, Argument = "firstName" },
            };

            var arguments = new Dictionary<string, string> { { "firstName", "John" } };
            var queryExtent = filter.Build(this.Session, arguments);

            var extent = this.Session.Extent(M.Person.ObjectType);
            extent.Filter.AddEquals(M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void EqualsWithoutParameters()
        {
            var filter = new Filter(M.Person.ObjectType)
            {
                Predicate = new Equals { PropertyType = M.Person.FirstName, Argument = "firstName" },
            };

            var queryExtent = filter.Build(this.Session);

            var extent = this.Session.Extent(M.Person.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void AndWithParameters()
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
                                            Argument = "firstName",
                                        },
                                    new Equals
                                        {
                                            PropertyType = M.Person.LastName,
                                            Argument = "lastName"
                                        },
                                },
                },
            };

            var arguments = new Dictionary<string, string>
                                {
                                    { "firstName", "John" },
                                    { "lastName", "Doe" },
                                };
            var queryExtent = filter.Build(this.Session, arguments);

            var extent = this.Session.Extent(M.Person.ObjectType);
            var and = extent.Filter.AddAnd();
            and.AddEquals(M.Person.FirstName, "John");
            and.AddEquals(M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void AndWithoutParameters()
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
                                    Argument = "firstName",
                                },
                            new Equals
                                {
                                    PropertyType = M.Person.LastName,
                                    Argument = "lastName"
                                },
                        },
                },
            };
            {
                var arguments = new Dictionary<string, string>
                                    {
                                        { "firstName", "John" },
                                    };
                var queryExtent = filter.Build(this.Session, arguments);

                var extent = this.Session.Extent(M.Person.ObjectType);
                extent.Filter.AddEquals(M.Person.FirstName, "John");

                Assert.Equal(extent.ToArray(), queryExtent.ToArray());
            }

            {
                var queryExtent = filter.Build(this.Session);

                var extent = this.Session.Extent(M.Person.ObjectType);

                Assert.Equal(extent.ToArray(), queryExtent.ToArray());
            }
        }

        [Fact]
        public void NestedWithParameters()
        {
            var filter = new Filter(M.Organisation.ObjectType)
            {
                Predicate = new ContainedIn
                {
                    PropertyType = M.Organisation.Employees,
                    Extent = new Filter(M.Person.ObjectType)
                    {
                        Predicate = new Equals
                        {
                            PropertyType = M.Person.Gender,
                            Argument = "gender",
                        },
                    },
                },
            };

            var male = new Genders(this.Session).Male;

            var arguments = new Dictionary<string, string> { { "gender", male.Id.ToString() } };
            var queryExtent = filter.Build(this.Session, arguments);

            var employees = this.Session.Extent(M.Person.ObjectType);
            employees.Filter.AddEquals(M.Person.Gender, male);
            var extent = this.Session.Extent(M.Organisation.ObjectType);
            extent.Filter.AddContainedIn(M.Organisation.Employees, employees);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void NestedWithoutParameters()
        {
            var filter = new Filter(M.Organisation.ObjectType)
            {
                Predicate = new ContainedIn
                {
                    PropertyType = M.Organisation.Employees,
                    Extent = new Filter(M.Person.ObjectType)
                    {
                        Predicate = new Equals
                        {
                            PropertyType = M.Person.Gender,
                            Argument = "gender",
                        },
                    },
                },
            };

            var arguments = new Dictionary<string, string>();
            var queryExtent = filter.Build(this.Session, arguments);

            var extent = this.Session.Extent(M.Organisation.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void AndNestedContainedInWithoutParameters()
        {
            var filter = new Filter(M.Organisation.ObjectType)
            {
                Predicate = new And
                {
                    Operands = new IPredicate[]
                    {
                        new ContainedIn
                        {
                            PropertyType = M.Organisation.Employees,
                            Extent = new Filter(M.Person.ObjectType)
                            {
                                Predicate = new ContainedIn
                                {
                                    PropertyType = M.Person.Gender,
                                    Argument = "gender",
                                },
                            },
                        },
                    },
                },
            };

            var parameters = new Dictionary<string, string>();
            var queryExtent = filter.Build(this.Session, parameters);

            var extent = this.Session.Extent(M.Organisation.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void AndNestedContainsWithoutParameters()
        {
            var filter = new Filter(M.Organisation.ObjectType)
            {
                Predicate = new And
                {
                    Operands = new IPredicate[]
                    {
                        new ContainedIn
                            {
                                PropertyType = M.Organisation.Employees,
                                Extent = new Filter(M.Person.ObjectType)
                                             {
                                                 Predicate = new Contains
                                                                 {
                                                                     PropertyType = M.Person.Gender,
                                                                     Argument = "gender",
                                                                 },
                                             },
                            },
                    },
                },
            };

            var arguments = new Dictionary<string, string>();
            var queryExtent = filter.Build(this.Session, arguments);

            var extent = this.Session.Extent(M.Organisation.ObjectType);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }
    }
}
