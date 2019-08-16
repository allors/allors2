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
            var budget = new OperatingBudgetBuilder(this.Session)
                .WithDescription("Budget")
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1))
                .Build();

            this.Session.Derive();

            Assert.Equal(new BudgetStates(this.Session).Opened, budget.BudgetState);
            Assert.Equal(budget.LastBudgetState, budget.BudgetState);
        }

        [Fact]
        public void GivenOperatingBudget_WhenBuild_ThenPreviousObjectStateIsNUll()
        {
            var budget = new OperatingBudgetBuilder(this.Session)
                .WithDescription("Budget")
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1))
                .Build();

            this.Session.Derive();

            Assert.Null(budget.PreviousBudgetState);
        }

        [Fact]
        public void GivenOperatingBudget_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OperatingBudgetBuilder(this.Session);
            var budget = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Budget");
            budget = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}