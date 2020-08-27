// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using System.Linq;
    using Xunit;

    public class MiserieTests : DomainTest
    {
        private readonly Scoreboard scoreboard;
        private readonly Person player1;
        private readonly Person player2;
        private readonly Person player3;
        private readonly Person player4;

        private readonly GameTypes gameTypes;

        public MiserieTests()
        {
            var people = new People(this.Session);

            this.player1 = people.FindBy(M.Person.UserName, "player1");
            this.player2 = people.FindBy(M.Person.UserName, "player2");
            this.player3 = people.FindBy(M.Person.UserName, "player3");
            this.player4 = people.FindBy(M.Person.UserName, "player4");

            this.scoreboard = new ScoreboardBuilder(this.Session)
                .WithPlayer(this.player1)
                .WithPlayer(this.player2)
                .WithPlayer(this.player3)
                .WithPlayer(this.player4)
                .Build();

            this.gameTypes = new GameTypes(this.Session);

            this.Session.Derive();
        }

        [Fact]
        public void TestSync()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();

            //Act
            this.scoreboard.AddGame(game);
            this.Session.Derive();

            //Assert
            Assert.Equal(4, game.Scores.Count);

        }

        [Fact]
        public void TestMiserieWithoutDeclarers()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            //Act
            game.GameType = this.gameTypes.Misery;
            this.Session.Derive();

            //Assert
            Assert.Null(game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Null(game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Null(game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Null(game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithOneDeclarerAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithOneDeclarerAndZeroWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndZeroWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithTwoDeclarersAndTwoWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndZeroWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);
            game.AddDeclarer(this.player3);
            game.AddDeclarer(this.player4);

            this.Session.Derive();

            //Assert
            Assert.Equal(0, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(0, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-0, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-0, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);
            game.AddDeclarer(this.player3);
            game.AddDeclarer(this.player4);

            game.AddWinner(player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(45, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndTwoWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);
            game.AddDeclarer(this.player3);
            game.AddDeclarer(this.player4);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestMiserieWithFourDeclarersAndThreeWinners()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Misery;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);
            game.AddDeclarer(this.player3);
            game.AddDeclarer(this.player4);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);
            game.AddWinner(this.player3);

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-45, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }
    }
}
