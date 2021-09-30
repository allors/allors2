// <copyright file="SalesOrderTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;

    using Xunit;

    public class SalesOrderTransferTests : DomainTest
    {
    }

    [Trait("Category", "Security")]
    public class SalesOrderTransferSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };
    }
}
