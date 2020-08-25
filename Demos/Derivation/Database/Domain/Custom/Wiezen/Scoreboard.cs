using System;
using System.Collections.Generic;
using System.Linq;

namespace Allors.Domain
{
    public partial class Scoreboard
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.Sync();
        }

        private void Sync()
        {
            // Fase1: Verwijderen van overbodige scores,
            //        dit zijn scores van players die niet meer in het scoreboard zitten
            foreach (Score score in this.AccumulatedScores)
            {
                var player = score.Player;
                if (!this.Players.Contains(player))
                {
                    // verwijder participants die al een statistic hebben
                    score.Delete();
                }
            }

            // Fase2: creeer score voor de players die nog geen score hebben
            foreach (Person player in this.Players)
            {
                var scores = this.AccumulatedScores.ToArray();
                var score = scores.FirstOrDefault(v => v.Player == player);

                if (score == null)
                {
                    score = new ScoreBuilder(this.strategy.Session).Build();
                    ((ScoreDerivedRoles)score).Player = player;

                    this.AddAccumulatedScore(score);
                }
            }
        }

    }
}
