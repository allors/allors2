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

    public class AlleenGaanAndVragenEnMeegaanAndTroelTests : DomainTest
    {
        private Scoreboard scoreboard;
        private Person player1;
        private Person player2;
        private Person player3;
        private Person player4;

        private GameTypes GameTypes;

        public AlleenGaanAndVragenEnMeegaanAndTroelTests()
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
        public void TestAlleenGaanWithOneDeclarerAndNoWinnerAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;
            game.AddDeclarer(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(-6, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestAlleenGaanWithOneDeclarerAndNoWinnerAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;
            game.AddDeclarer(player1);
            game.Overslagen = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestAlleenGaanWithOneDeclarerAndOneWinnerAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;
            game.AddDeclarer(player1);
            game.AddWinner(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(6, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestAlleenGaanWithOneDeclarerAndOneWinnerAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;
            game.AddDeclarer(player1);
            game.AddWinner(player1);
            game.Overslagen = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestAlleenGaanWithOneDeclarerAndOneWinnerAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.AlleenGaan;
            game.AddDeclarer(player1);
            game.AddWinner(player1);
            game.Overslagen = 8;

            this.Session.Derive();

            //Assert
            Assert.Equal(60, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestVragenEnMeegaanWithTwoDeclarersAndNoWinnersAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.VragenEnMeegaan;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(-2, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestVragenEnMeegaanWithTwoDeclarersAndNoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.VragenEnMeegaan;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.Overslagen = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(-5, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestVragenEnMeegaanWithTwoDeclarersAndTwoWinnersAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.VragenEnMeegaan;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(2, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestVragenEnMeegaanWithTwoDeclarersAndTwoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.VragenEnMeegaan;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            game.Overslagen = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(5, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestVragenEnMeegaanWithTwoDeclarersAndTwoWinnersAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.VragenEnMeegaan;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            game.Overslagen = 5;

            this.Session.Derive();

            //Assert
            Assert.Equal(14, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(14, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-14, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-14, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestTroelWithTwoDeclarersAndTwoWinnersAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Troel;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            game.Overslagen = 5;

            this.Session.Derive();

            //Assert
            Assert.Equal(28, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(28, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-28, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-28, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestTroelWithTwoDeclarersAndTwoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Troel;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            game.Overslagen = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(10, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(10, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }
    }
}