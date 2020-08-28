// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using System.Linq;
    using Xunit;

    public class ScoreboardTests : DomainTest
    {
        private readonly Scoreboard scoreboard;
        private readonly Person player1;
        private readonly Person player2;
        private readonly Person player3;
        private readonly Person player4;
        private readonly Person player5;

        private readonly GameTypes gameTypes;

        public ScoreboardTests()
        {
            var people = new People(this.Session);

            this.player1 = people.FindBy(M.Person.UserName, "player1");
            this.player2 = people.FindBy(M.Person.UserName, "player2");
            this.player3 = people.FindBy(M.Person.UserName, "player3");
            this.player4 = people.FindBy(M.Person.UserName, "player4");
            this.player5 = people.FindBy(M.Person.UserName, "player5");

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
        public void TestZeroWithValues()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            this.Session.Derive();

            var scores = game.Scores.ToArray();

            //Act
            scores[0].Value = -5;
            scores[1].Value = -5;
            scores[2].Value = 5;
            scores[3].Value = 5;

            //Assert
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestZeroWithoutValues()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            this.Session.Derive();

            var scores = game.Scores.ToArray();

            //Act
            scores[0].Value = null;
            scores[1].Value = null;
            scores[2].Value = null;
            scores[3].Value = null;

            //Assert
            Assert.True(this.scoreboard.ZeroTest());
        }

        [Fact]
        public void TestAccumulatedScoresWithOneGame()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;

            game.AddDeclarer(this.player1);
            game.ExtraTrick = 0;

            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(6, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-2, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-2, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-2, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithTwoGameAndPlayerChangeInBetweenGames()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;

            game.AddDeclarer(this.player1);
            game.ExtraTrick = 0;

            game.AddWinner(this.player1);

            this.Session.Derive();

            //Arrange
            this.scoreboard.RemovePlayer(this.player4);
            this.scoreboard.AddPlayer(this.player5);

            var game2 = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            //Act
            game2.GameType = this.gameTypes.SmallSlam;

            game2.AddDeclarer(this.player1);
            game2.ExtraTrick = 0;

            game2.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(12, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-4, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-4, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-2, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player5).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithTwoGames()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            var game2 = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;

            game.AddDeclarer(this.player1);
            game.ExtraTrick = 0;

            game.AddWinner(this.player1);

            game2.GameType = this.gameTypes.SmallSlam;

            game2.AddDeclarer(this.player1);
            game2.ExtraTrick = 0;

            game2.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(12, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-4, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-4, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-4, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithNoGames()
        {
            //Arrange

            //Act
            this.Session.Derive();

            //Assert
            Assert.Equal(0, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player1).Value);
            Assert.Equal(0, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player2).Value);
            Assert.Equal(0, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player3).Value);
            Assert.Equal(0, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player4).Value);
        }

        [Fact]
        public void TestAccumulatedScoresWithMultipleGameTypes()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            var game2 = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game2);

            game2.StartDate = this.Session.Now();
            game2.EndDate = game2.StartDate.Value.AddHours(1);

            var game3 = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game3);

            game3.StartDate = this.Session.Now();
            game3.EndDate = game3.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.SmallSlam;

            game.AddDeclarer(this.player1);
            game.ExtraTrick = 0;

            game.AddWinner(this.player1);

            game2.GameType = this.gameTypes.Trull;

            game2.AddDeclarer(this.player1);
            game2.AddDeclarer(this.player2);
            game2.ExtraTrick = 2;

            game2.AddWinner(this.player1);
            game2.AddWinner(this.player2);

            game3.GameType = this.gameTypes.Misery;

            game3.AddDeclarer(this.player1);

            game3.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(29, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player1).Value);
            Assert.Equal(1, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-15, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-15, this.scoreboard.AccumulatedScores.First(v => v.Player == this.player4).Value);
        }
    }
}
