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

    using NUnit.Framework;

    [TestFixture]
    public class BudgetTests : DomainTest
    {
        [Test]
        public void GivenOperatingBudget_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var budget = new OperatingBudgetBuilder(this.DatabaseSession)
                .WithDescription("Budget")
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new BudgetObjectStates(this.DatabaseSession).Opened, budget.CurrentObjectState);
            Assert.AreEqual(budget.LastObjectState, budget.CurrentObjectState);
        }

        [Test]
        public void GivenOperatingBudget_WhenBuild_ThenPreviousObjectStateIsNUll()
        {
            var budget = new OperatingBudgetBuilder(this.DatabaseSession)
                .WithDescription("Budget")
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(budget.PreviousObjectState);
        }

        [Test]
        public void GivenOperatingBudget_WhenConfirmed_ThenCurrentBudgetStatusMustBeDerived()
        {
            var budget = new OperatingBudgetBuilder(this.DatabaseSession)
                .WithDescription("Budget")
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, budget.BudgetStatuses.Count);
            Assert.AreEqual(new BudgetObjectStates(this.DatabaseSession).Opened, budget.CurrentBudgetStatus.BudgetObjectState);

            budget.Close();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, budget.BudgetStatuses.Count);
            Assert.AreEqual(new BudgetObjectStates(this.DatabaseSession).Closed, budget.CurrentBudgetStatus.BudgetObjectState);
        }

        [Test]
        public void GivenOperatingBudget_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OperatingBudgetBuilder(this.DatabaseSession);
            var budget = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Budget");
            budget = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            budget = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Assert.AreEqual(budget.CurrentBudgetStatus.BudgetObjectState, new BudgetObjectStates(this.DatabaseSession).Opened);
            Assert.AreEqual(budget.CurrentObjectState, new BudgetObjectStates(this.DatabaseSession).Opened);
            Assert.AreEqual(budget.CurrentObjectState, budget.LastObjectState);
        }
    }
}