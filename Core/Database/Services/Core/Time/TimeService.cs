// <copyright file="TimeService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;

    public class TimeService : ITimeService
    {
        public TimeService(IStateService stateService)
        {
            this.Clear();
            stateService.Register(this);
        }

        public TimeSpan? Shift { get; set; }

        public DateTime Now() => this.Shift.HasValue ? DateTime.UtcNow.Add(this.Shift.Value) : DateTime.UtcNow;

        public void Clear() => this.Shift = null;
    }
}
