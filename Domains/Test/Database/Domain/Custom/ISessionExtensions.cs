namespace Allors
{
    using System;

    public static partial class ISessionExtensions
    {
        public static DateTime Now(this ISession session)
        {
            var now = DateTime.UtcNow;
            if (Config.TimeShift != null)
            {
                now = now.Add(Config.TimeShift.Value);
            }

            return now;
        }
    }
}
