// <copyright file="SyncTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Tests
{
    using Xunit;

    public class SyncTests : DomainTest
    {
        [Fact]
        public void IsSync()
        {
            var metaPopulation = this.Session.Database.MetaPopulation;
            foreach (var composite in metaPopulation.Composites)
            {
                switch (composite.Name)
                {
                    case "SyncDepthI1":
                    case "SyncDepthC1":
                    case "SyncDepth2":
                        Assert.True(composite.IsSynced);
                        break;

                    default:
                        Assert.False(composite.IsSynced);
                        break;
                }
            }
        }
    }
}
