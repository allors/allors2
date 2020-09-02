// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using System.Linq;
    using Xunit;

    public class OpenMiseryTests : DomainTest
    {
        private readonly Scoreboard scoreboard;
        private readonly Person player1;
        private readonly Person player2;
        private readonly Person player3;
        private readonly Person player4;

        private readonly GameTypes gameTypes;

        public OpenMiseryTests()
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
        public void TestMiserieWithOneDeclarerAndOneWinner()
        {
            //Arrange
            var game = new GameBuilder(this.Session).Build();
            this.scoreboard.AddGame(game);

            game.StartDate = this.Session.Now();
            game.EndDate = game.StartDate.Value.AddHours(1);

            //Act
            game.GameType = this.gameTypes.OpenMisery;
            game.AddDeclarer(this.player1);
            game.AddWinner(this.player1);

            this.Session.Derive();

            //Assert
            Assert.Equal(30, game.Scores.First(v => v.Player == this.player1).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == this.player2).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == this.player3).Value);
            Assert.Equal(-10, game.Scores.First(v => v.Player == this.player4).Value);
            Assert.True(this.scoreboard.ZeroTest());
        }
    }
}
