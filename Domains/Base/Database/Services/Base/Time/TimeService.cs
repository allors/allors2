// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

        public DateTime Now()
        {
            return this.Shift.HasValue ? DateTime.UtcNow.Add(this.Shift.Value) : DateTime.UtcNow;
        }

        public void Clear()
        {
            this.Shift = null;
        }
    }
}