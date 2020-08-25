using System;
using System.Collections.Generic;
using System.Linq;

namespace Allors.Domain
{

    public partial class Score
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            //TODO: check for change
            var accumulatedScore = this.GameWhereScore?.ScoreboardWhereGame?.AccumulatedScores?
                .FirstOrDefault(v => v.Player == this.Player);

            iteration.AddDependency(accumulatedScore, this);
            iteration.Mark(accumulatedScore);
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            if (this.ExistScoreboardWhereAccumulatedScore)
            {
                var scoreboard = this.ScoreboardWhereAccumulatedScore;

                this.Value = scoreboard.Games.SelectMany(v => v.Scores)
                    .Where(v => v.Player == this.Player)
                    .Aggregate(0, (acc, v) => acc + v.Value ?? 0);

            }

            if (this.ExistGameWhereScore)
            {
                var game = this.GameWhereScore;

                if (game.ExistEndDate && game.ExistGameType)
                {
                    var gameType = game.GameType;
                    var declarers = game.Declarers.ToList();
                    var winners = game.Winners.ToList();

                    var winning = winners.Contains(this.Player);
                    var declaring = declarers.Contains(this.Player);


                    if (gameType.IsMiserie || gameType.IsMiserieOpTafel)
                    {
                        // 
                        switch (declarers.Count)
                        {
                            case 1:
                                if (declaring)
                                {
                                    this.Value = winning ? 15 : -15;
                                }
                                else
                                {
                                    this.Value = winners.Count() == 0 ? 5 : -5;
                                }
                                break;
                            case 2:
                                if (declaring)
                                {
                                    switch (winners.Count())
                                    {
                                        case 0:
                                            this.Value = -15;
                                            break;
                                        case 1:
                                            this.Value = winning ? 15 : -15;
                                            break;
                                        default:
                                            this.Value = 15;
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (winners.Count())
                                    {
                                        case 0:
                                            this.Value = 15;
                                            break;
                                        case 1:
                                            this.Value = 0;
                                            break;
                                        default:
                                            this.Value = -15;
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                if (declaring)
                                {
                                    switch (winners.Count())
                                    {
                                        case 0:
                                            this.Value = 0;
                                            break;
                                        case 1:
                                            this.Value = winning ? 45 : -15;
                                            break;
                                        case 2:
                                            this.Value = winning ? 15 : -15;
                                            break;
                                        default:
                                            this.Value = winning ? 15 : -45;
                                            break;
                                    }
                                }
                                break;
                        }
                        if (gameType.IsMiserieOpTafel)
                        {
                            this.Value = this.Value * 2;
                        }
                    }

                    if (gameType.IsSoloSlim || gameType.IsSolo || gameType.IsAbondance)
                    {
                        if (declaring)
                        {
                            this.Value = winning ? 15 : -15;
                        }
                        else
                        {
                            this.Value = winners.Count() == 0 ? 5 : -5;
                        }

                        if (gameType.IsSolo)
                        {
                            this.Value = this.Value * 2;
                        }

                        if (gameType.IsSoloSlim)
                        {
                            this.Value = this.Value * 3;
                        }
                    }


                    if (gameType.IsAlleenGaan || gameType.IsVragenEnMeegaan || gameType.IsTroel)
                    {
                        int aantalDefendersPerPersoon = 3;
                        var overslagenOmDubbelTeZijn = 8;

                        if (gameType.IsVragenEnMeegaan || gameType.IsTroel)
                        {
                            aantalDefendersPerPersoon = 1;
                            overslagenOmDubbelTeZijn = 5;
                        }

                        var overslagen = this.GameWhereScore.Overslagen ?? 0;

                        if (declaring)
                        {
                            int punten = aantalDefendersPerPersoon * 2 + (aantalDefendersPerPersoon * overslagen);
                            int puntenWinst = punten;
                            if (overslagen == overslagenOmDubbelTeZijn)
                            {
                                puntenWinst = punten * 2;
                            }
                            this.Value = winning ? puntenWinst : -punten;
                        }
                        else
                        {
                            int punten = 2 + (overslagen);
                            if (overslagen == overslagenOmDubbelTeZijn)
                            {
                                punten = punten * 2;
                            }
                            this.Value = winners.Count() == 0 ? punten : -punten;
                        }
                        if (gameType.IsTroel)
                        {
                            this.Value = this.Value * 2;
                        }
                    }

                }
                else
                {
                    this.RemoveValue();
                }
            }
        }
    }
}
