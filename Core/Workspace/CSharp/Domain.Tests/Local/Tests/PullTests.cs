// <copyright file="PullTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Local
{
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Xunit;

    public class PullTests : LocalTest
    {
        [Fact]
        public void Pull()
        {
            var context = new Context(this.Database, this.Workspace);

            var pull = new Pull
            {
                Extent = new Filter(M.Person.ObjectType),
            };

            var result = context.Load(pull).Result;

            var people = result.GetCollection<Person>("People");

            result = context.Load(pull).Result;

            var people2 = result.GetCollection<Person>("People");
        }
    }
}
