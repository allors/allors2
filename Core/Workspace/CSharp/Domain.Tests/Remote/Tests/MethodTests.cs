// <copyright file="ObjectTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Remote
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Nito.AsyncEx;
    using Xunit;

    public class MethofTests : RemoteTest
    {
        [Fact]
        public void Call() =>
            AsyncContext.Run(
                async () =>
                {
                    var context = new Context(this.Database, this.Workspace);

                    var pull = new[]
                    {
                        new Pull
                        {
                            Extent = new Extent(M.Organisation.ObjectType),
                        },
                    };

                    var organisation = (await context.Load(pull)).GetCollection<Organisation>().First();

                    Assert.False(organisation.JustDidIt);

                    var result = await context.Invoke(organisation.JustDoIt);

                    Assert.False(result.HasErrors);

                    pull = new[]
                    {
                        new Pull
                        {
                            Object = organisation,
                        },
                    };

                    organisation = (await context.Load(pull)).GetObject<Organisation>();

                    context.Session.Reset();

                    Assert.True(organisation.JustDidIt);
                });

    }
}
