namespace Allors.Domain
{
    using Allors.Domain.Derivations.Errors;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Game
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            //TODO: check for change
            iteration.AddDependency(this.ScoreboardWhereGame, this);
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistStartDate && this.ExistEndDate)
            {
                if (this.EndDate.Value <= this.StartDate.Value)
                {
                    derivation.Validation.AddError(new DerivationErrorGeneric(derivation.Validation, this, this.Meta.EndDate, "End date should be after start date"));
                }
            }
            this.Defenders = this.ScoreboardWhereGame?.Players.Except(this.Declarers).ToArray();

            this.Sync();

        }

        private void Sync()
        {
            if (this.ExistScoreboardWhereGame)
            {
                var players = new List<Person>(this.ScoreboardWhereGame.Players);

                // Phase1: Removing unnecessary statistics and keeping track of participants without statistics
                foreach (Score score in this.Scores)
                {
                    var player = score.Player;
                    if (players.Contains(player))
                    {
                        // Remove participants who already have a statistic
                        players.Remove(player);
                    }
                    else
                    {
                        // Delete statistic of which the participant no longer occurs
                        score.Delete();
                    }
                }

                // Phase2: Create statistics for the participants who did not have a statistic yet
                foreach (var player in players)
                {
                    var score = new ScoreBuilder(this.strategy.Session)
                        .Build();

                    ((ScoreDerivedRoles)score).Player = player;

                    this.AddScore(score);
                }

            }
            else
            {
                foreach (Score score in this.Scores)
                {
                    score.Delete();
                }
            }

        }
    }
}
