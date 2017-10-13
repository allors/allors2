namespace Allors
{
    using System;

    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtensions
    {
        public static DateTime Now(this ISession session)
        {
            var now = DateTime.UtcNow;

            var timeService = session.ServiceProvider.GetRequiredService<ITimeService>();
            var timeshift = timeService.Shift;
            if (timeshift != null)
            {
                now = now.Add((TimeSpan)timeshift);
            }

            return now;
        }
    }
}
