using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allors.Domain
{
    public static class ScoreboardExtensions
    {
        public static bool NulProef(this Scoreboard @this)
        {
            var games = @this.Games;
            
            foreach (Game game in games)
            {
                var scores = game.Scores;
                var som = 0;

                foreach (Score score in scores.Where(v => v.ExistValue))
                {
                    som += score.Value.Value;
                }

                if (som != 0)
                {
                    return false;
                }

            }

            return true;
        }
    }
}