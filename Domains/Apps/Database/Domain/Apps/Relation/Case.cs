// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Case.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Case
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.Case, M.Case.CaseState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCaseState)
            {
                this.CaseState = new CaseStates(this.Strategy.Session).Opened;
            }
        }

        public void AppsClose()
        {
            var closed = new CaseStates(this.Strategy.Session).Closed;
            this.CaseState = closed;
        }

        public void AppsComplete()
        {
            var completed = new CaseStates(this.Strategy.Session).Completed;
            this.CaseState = completed;
        }

        public void AppsReopen()
        {
            var opened = new CaseStates(this.Strategy.Session).Opened;
            this.CaseState = opened;
        }
    }
}