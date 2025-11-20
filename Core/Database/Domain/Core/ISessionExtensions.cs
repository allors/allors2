// <copyright file="ISessionExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ISessionExtension type.</summary>

namespace Allors
{
    using System;
    using Domain.Derivations;
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
            var timeShift = timeService.Shift;
            if (timeShift != null)
            {
                now = now.Add((TimeSpan)timeShift);
            }

            return now;
        }
    }
}
