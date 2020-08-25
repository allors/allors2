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

    public class MiserieTests : DomainTest
    {
        private Scoreboard scoreboard;
        private Person player1;
        private Person player2;
        private Person player3;
        private Person player4;

        private GameTypes GameTypes;

        public MiserieTests()
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
        public void TestSync()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();

            //Act
            scoreboard.AddGame(game);
            this.Session.Derive();

            //Assert
            Assert.Equal(4, game.Scores.Count);

        }

        [Fact]
        public void TestMiserieWithoutDeclarers()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            //Act
            game.GameType = this.GameTypes.Miserie;
            this.Session.Derive();

            //Assert
            Assert.Null(game.Scores.First(v => v.Player == player1).Value);
            Assert.Null(game.Scores.First(v => v.Player == player2).Value);
            Assert.Null(game.Scores.First(v => v.Player == player3).Value);
            Assert.Null(game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithOneDeclarerAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithOneDeclarerAndZeroWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndZeroWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndTwoWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);

            game.AddWinner(player1);
            game.AddWinner(player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndZeroWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);
            game.AddDeclarer(player3);
            game.AddDeclarer(player4);

            this.Session.Derive();

            //Assert
            Assert.Equal(0, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-0, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-0, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);
            game.AddDeclarer(player3);
            game.AddDeclarer(player4);

            game.AddWinner(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(45, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndTwoWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);
            game.AddDeclarer(player3);
            game.AddDeclarer(player4);

            game.AddWinner(player1);
            game.AddWinner(player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndThreeWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.GameTypes.Miserie;
            game.AddDeclarer(player1);
            game.AddDeclarer(player2);
            game.AddDeclarer(player3);
            game.AddDeclarer(player4);

            game.AddWinner(player1);
            game.AddWinner(player2);
            game.AddWinner(player3);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player2).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == player3).Value);
            Assert.Equal(-45, game.Scores.First(v => v.Player == player4).Value);
            Assert.True(this.scoreboard.NulProef());
        }
    }
}