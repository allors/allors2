// <copyright file="SerialisedItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;
    using System.Linq;
    using Allors.Domain.TestPopulation;
    using Resources;
    using System.Collections.Generic;

    public class SerialisedItemTests : DomainTest
    {
        [Fact]
        public void GivenSerializedItem_WhenAddingWithSameSerialNumber_ThenError()
        {
            var good = new UnifiedGoodBuilder(this.Session).WithSerialisedDefaults(this.InternalOrganisation).Build();
            var serialNumber = good.SerialisedItems.First.SerialNumber;

            var newItem = new SerialisedItemBuilder(this.Session).WithSerialNumber(serialNumber).Build();
            good.AddSerialisedItem(newItem);

            var expectedErrorMessage = ErrorMessages.SameSerialNumber;

            var errors = new List<string>(this.Session.Derive(false).Errors.Select(v => v.Message));
            Assert.Contains(expectedErrorMessage, errors);
        }
    }
}
