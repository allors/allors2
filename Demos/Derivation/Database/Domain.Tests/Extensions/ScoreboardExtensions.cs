namespace Allors.Domain
{
    using System.Linq;

    public static class ScoreboardExtensions
    {
        public static bool ZeroTest(this Scoreboard @this)
        {
            var games = @this.Games;
            
            foreach (Game game in games)
            {
                var scores = game.Scores;
                var som = 0;

                foreach (var score in scores.Where(v => v.ExistValue))
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
