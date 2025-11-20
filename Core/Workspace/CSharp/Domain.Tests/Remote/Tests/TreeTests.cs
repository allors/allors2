// <copyright file="PullTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Remote
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Xunit;
    using Result = Allors.Workspace.Data.Result;

    public class TreeTests : RemoteTest
    {
        [Fact]
        public void Users()
        {
            var context = new Context(this.Database, this.Workspace);

            var pull = new Pull
            {
                Extent = new Extent(M.User.ObjectType),
                Results = new[]
                {
                    new Result
                    {
                        Fetch = new Fetch
                        {
                            Include = new UserNodeBuilder(v => v.Person_Address()),
                        },
                    },
                },
            };

            var result = context.Load(pull).Result;

            var users = result.GetCollection<User>();

            var personWithAddress = (Person)users.Single(v => (v as Person)?.ExistAddress == true);

            Assert.NotNull(personWithAddress);
            Assert.Equal("Jane", personWithAddress.FirstName);
        }
    }
}
