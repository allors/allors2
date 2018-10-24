//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkTaskTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Should;
    using Xunit;

    using Meta;
    
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
            derivation.HasErrors.ShouldBeTrue();

            //// Re-arrange
            var worker = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Worker").Build();
            timeSheet.Worker = worker;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.Count().ShouldEqual(originalCount - 1);

            //// Re-arrange
            var today = DateTime.UtcNow;
            timeSheet.FromDate = today;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.Count().ShouldEqual(originalCount - 1);

            //// Re-arrange
            var tomorrow = DateTime.UtcNow.AddDays(1);
            timeSheet.ThroughDate = tomorrow;
            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeFalse();
        }
    }
}