//------------------------------------------------------------------------------------------------- 
// <copyright file="BudgetTests.cs" company="Allors bvba">
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

    using Xunit;

    
    public class BudgetTests : DomainTest
    {
        [Fact]
        public void GivenOperatingBudget_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var budget = new OperatingBudgetBuilder(this.DatabaseSession)
                .WithDescription("Budget")
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new BudgetStates(this.DatabaseSession).Opened, budget.BudgetState);
            Assert.Equal(budget.LastBudgetState, budget.BudgetState);
        }

        [Fact]
        public void GivenOperatingBudget_WhenBuild_ThenPreviousObjectStateIsNUll()
        {
            var budget = new OperatingBudgetBuilder(this.DatabaseSession)
                .WithDescription("Budget")
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.DatabaseSession.Derive();

            Assert.Null(budget.PreviousBudgetState);
        }

        [Fact]
        public void GivenOperatingBudget_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OperatingBudgetBuilder(this.DatabaseSession);
            var budget = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Budget");
            budget = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            budget = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}