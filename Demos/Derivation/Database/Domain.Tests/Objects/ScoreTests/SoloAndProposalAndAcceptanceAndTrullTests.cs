// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using System.Linq;
    using Xunit;

    public class SoloAndProposalAndAcceptanceAndTrullTests : DomainTest
    {
        private readonly Scoreboard scoreboard;
        private readonly Person player1;
        private readonly Person player2;
        private readonly Person player3;
        private readonly Person player4;

        private readonly GameTypes gameTypes;

        public SoloAndProposalAndAcceptanceAndTrullTests()
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
        public void TestSoloWithOneDeclarerAndNoWinnerAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;
            game.AddDeclarer(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(-6, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestSoloWithOneDeclarerAndNoWinnerAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;
            game.AddDeclarer(this.player1);
            game.ExtraTrick = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(-15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestSoloWithOneDeclarerAndOneWinnerAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;
            game.AddDeclarer(this.player1);
            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(6, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestSoloWithOneDeclarerAndOneWinnerAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;
            game.AddDeclarer(this.player1);
            game.AddWinner(this.player1);
            game.ExtraTrick = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(15, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestSoloWithOneDeclarerAndOneWinnerAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;
            game.AddDeclarer(this.player1);
            game.AddWinner(this.player1);
            game.ExtraTrick = 8;

            this.Session.Derive();

            //Assert
            Assert.Equal(60, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-20, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestProposalAndAcceptanceWithTwoDeclarersAndNoWinnersAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.ProposalAndAcceptance;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestProposalAndAcceptanceWithTwoDeclarersAndNoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.ProposalAndAcceptance;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.ExtraTrick = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestProposalAndAcceptanceWithTwoDeclarersAndTwoWinnersAndNoTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.ProposalAndAcceptance;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            this.Session.Derive();

            //Assert
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(2, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-2, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestProposalAndAcceptanceWithTwoDeclarersAndTwoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.ProposalAndAcceptance;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            game.ExtraTrick = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(5, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-5, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestProposalAndAcceptanceWithTwoDeclarersAndTwoWinnersAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.ProposalAndAcceptance;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            game.ExtraTrick = 5;

            this.Session.Derive();

            //Assert
            Assert.Equal(14, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(14, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-14, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-14, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestTrullWithTwoDeclarersAndTwoWinnersAndAllTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Trull;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            game.ExtraTrick = 5;

            this.Session.Derive();

            //Assert
            Assert.Equal(28, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(28, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-28, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-28, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestTrullWithTwoDeclarersAndTwoWinnersAndTricks()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.Trull;
            game.AddDeclarer(this.player1);
            game.AddDeclarer(this.player2);

            game.AddWinner(this.player1);
            game.AddWinner(this.player2);

            game.ExtraTrick = 3;

            this.Session.Derive();

            //Assert
            Assert.Equal(10, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(10, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }
    }
}
