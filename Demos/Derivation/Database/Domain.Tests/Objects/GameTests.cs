//------------------------------------------------------------------------------------------------- 
// <copyright file="DemoTests.cs" company="Allors bvba">
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
    using Allors.Meta;
    using System.Linq;
    using Xunit;

    public class GameTests : DomainTest
    {
        private Scoreboard scoreboard;
        private Person player1;
        private Person player2;
        private Person player3;
        private Person player4;

        private GameTypes GameTypes;

        public GameTests()
        {
            var people = new People(this.Session);

            this.player1 = people.FindBy(M.Person.UserName, "speler1");
            this.player2 = people.FindBy(M.Person.UserName, "speler2");
            this.player3 = people.FindBy(M.Person.UserName, "speler3");
            this.player4 = people.FindBy(M.Person.UserName, "speler4");

            this.scoreboard = new ScoreboardBuilder(this.Session)
                .WithPlayer(player1)
                .WithPlayer(player2)
                .WithPlayer(player3)
                .WithPlayer(player4)
                .Build();

            this.GameTypes = new GameTypes(this.Session);

            this.Session.Derive();
        }

        [Fact]
        public void TestStartDateBeforeEndDate()
        {
            // Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            this.Session.Derive();

            // Act
            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(-1);

            var validation = this.Session.Derive(false);

            // Assert
            Assert.True(validation.HasErrors);
        }

        [Fact]
        public void TestStartDateDoesNotEqualsEndDate()
        {
            // Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            this.Session.Derive();

            // Act
            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate;

            var validation = this.Session.Derive(false);

            // Assert
            Assert.True(validation.HasErrors);
        }
    }
}