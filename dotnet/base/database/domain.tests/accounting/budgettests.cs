// <copyright file="BudgetTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
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
