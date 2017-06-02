namespace Allors
{
    using System;

    public static partial class ISessionExtensions
    {
        public static DateTime Now(this ISession session)
        {
            var now = DateTime.UtcNow;
            var timeshift = session.Database["TimeShift"];
            if (timeshift != null)
            {
                now = now.Add((TimeSpan)timeshift);
            }

            return now;
        }
    }
}
