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

    public class ScoreboardTests : DomainTest
    {
        private Scoreboard scoreboard;
        private Person player1;
        private Person player2;
        private Person player3;
        private Person player4;
        private Person player5;

        private GameTypes GameTypes;

        public ScoreboardTests()
        {
            var people = new People(this.Session);

            this.player1 = people.FindBy(M.Person.UserName, "speler1");
            this.player2 = people.FindBy(M.Person.UserName, "speler2");
            this.player3 = people.FindBy(M.Person.UserName, "speler3");
            this.player4 = people.FindBy(M.Person.UserName, "speler4");
            this.player5 = people.FindBy(M.Person.UserName, "speler5");

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
        public void TestNulProefWithValues()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            this.Session.Derive();

            var scores = game.Scores.ToArray();

            //Act
            scores[0].Value = -5;
            scores[1].Value = -5;
            scores[2].Value = 5;
            scores[3].Value = 5;

            //Assert
            Assert.True(scoreboard.NulProef());
        }

        [Fact]
        public void TestNulProefWithoutValues()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            this.Session.Derive();

            var scores = game.Scores.ToArray();

            //Act
            scores[0].Value = null;
            scores[1].Value = null;
            scores[2].Value = null;
            scores[3].Value = null;

            //Assert
            Assert.True(scoreboard.NulProef());
        }

        [Fact]
        public void TestAccumulatedScoresWithOneGame()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;

            game.AddDeclarer(player1);
            game.Overslagen = 0;

            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(6, scoreboard.AccumulatedScores.First(v => v.Player == player1).Value);
            Assert.Equal(-2, scoreboard.AccumulatedScores.First(v => v.Player == player2).Value);
            Assert.Equal(-2, scoreboard.AccumulatedScores.First(v => v.Player == player3).Value);
            Assert.Equal(-2, scoreboard.AccumulatedScores.First(v => v.Player == player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithTwoGameAndPlayerChangeInBetweenGames()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;

            game.AddDeclarer(player1);
            game.Overslagen = 0;

            game.AddWinner(this.player1);

            this.Session.Derive();

            //Arrange
            this.scoreboard.RemovePlayer(this.player4);
            this.scoreboard.AddPlayer(player5);

            var game2 = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            //Act
            game2.GameType = this.GameTypes.AlleenGaan;

            game2.AddDeclarer(player1);
            game2.Overslagen = 0;

            game2.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(12, scoreboard.AccumulatedScores.First(v => v.Player == player1).Value);
            Assert.Equal(-4, scoreboard.AccumulatedScores.First(v => v.Player == player2).Value);
            Assert.Equal(-4, scoreboard.AccumulatedScores.First(v => v.Player == player3).Value);
            Assert.Equal(-2, scoreboard.AccumulatedScores.First(v => v.Player == player5).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithTwoGames()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            var game2 = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;

            game.AddDeclarer(player1);
            game.Overslagen = 0;

            game.AddWinner(this.player1);

            game2.GameType = this.GameTypes.AlleenGaan;

            game2.AddDeclarer(player1);
            game2.Overslagen = 0;

            game2.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(12, scoreboard.AccumulatedScores.First(v => v.Player == player1).Value);
            Assert.Equal(-4, scoreboard.AccumulatedScores.First(v => v.Player == player2).Value);
            Assert.Equal(-4, scoreboard.AccumulatedScores.First(v => v.Player == player3).Value);
            Assert.Equal(-4, scoreboard.AccumulatedScores.First(v => v.Player == player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithNoGames()
        {
            //Arrange

            //Act
            this.Session.Derive();

            //Assert
            Assert.Equal(0, scoreboard.AccumulatedScores.First(v => v.Player == player1).Value);
            Assert.Equal(0, scoreboard.AccumulatedScores.First(v => v.Player == player2).Value);
            Assert.Equal(0, scoreboard.AccumulatedScores.First(v => v.Player == player3).Value);
            Assert.Equal(0, scoreboard.AccumulatedScores.First(v => v.Player == player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithMultipleGameTypes()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            var game2 = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            var game3 = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game3);

            game3.StartDate = this.Session.Now();
            game3.EndDate = game3.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;

            game.AddDeclarer(player1);
            game.Overslagen = 0;

            game.AddWinner(this.player1);

            game2.GameType = this.GameTypes.Troel;

            game2.AddDeclarer(player1);
            game2.AddDeclarer(player2);
            game2.Overslagen = 2;

            game2.AddWinner(this.player1);
            game2.AddWinner(this.player2);

            game3.GameType = this.GameTypes.Miserie;

            game3.AddDeclarer(player1);

            game3.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(29, scoreboard.AccumulatedScores.First(v => v.Player == player1).Value);
            Assert.Equal(1, scoreboard.AccumulatedScores.First(v => v.Player == player2).Value);
            Assert.Equal(-15, scoreboard.AccumulatedScores.First(v => v.Player == player3).Value);
            Assert.Equal(-15, scoreboard.AccumulatedScores.First(v => v.Player == player4).Value);
        }
    }
}