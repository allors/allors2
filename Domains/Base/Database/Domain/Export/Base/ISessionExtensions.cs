//-------------------------------------------------------------------------------------------------
// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class ISessionExtensions
    {
        public static IValidation Derive(this ISession session, bool throwExceptionOnError = true)
        {
            var derivationService = session.ServiceProvider.GetRequiredService<IDerivationService>();
            var derivation = derivationService.CreateDerivation(session);
            var validation = derivation.Derive();
            if (throwExceptionOnError && validation.HasErrors)
            {
                throw new DerivationException(validation);
            }

            return validation;
        }

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