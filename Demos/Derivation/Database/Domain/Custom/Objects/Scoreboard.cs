namespace Allors.Domain
{
    using System.Linq;

    public partial class Scoreboard
    {
        public void CustomOnDerive(ObjectOnDerive method) => this.Sync();

        private void Sync()
        {
            // Phase1: Removing unnecessary scores, these are scores from players who are no longer in the scoreboard
            foreach (Score score in this.AccumulatedScores)
            {
                var player = score.Player;
                if (!this.Players.Contains(player))
                {
                    // Remove participants who already have a statistic
                    score.Delete();
                }
            }

            // Phase2: Create score for the players who don't have a score yet
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
