// <copyright file="TimeSheetTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Xunit;

    public class TimeSheetTests : DomainTest
    {
        [Fact]
        public void GivenTimeSheet_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            // Arrange
            var timeSheet = new TimeSheetBuilder(this.Session).Build();

            // Act
            var derivation = this.Session.Derive(false);
            var originalCount = derivation.Errors.Count();

            // Assert
            Assert.True(derivation.HasErrors);

            //// Re-arrange
            var worker = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            timeSheet.Worker = worker;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            Assert.False(derivation.HasErrors);
        }
    }
}
