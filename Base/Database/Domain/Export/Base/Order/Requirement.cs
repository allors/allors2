// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Requirement.cs" company="Allors bvba">
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

    public partial class Requirement
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.Requirement, M.Requirement.RequirementState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistRequirementState)
            {
                this.RequirementState = new RequirementStates(this.Strategy.Session).Active;
            }
        }

        public void BaseClose(RequirementClose method)
        {
            this.RequirementState = new RequirementStates(this.Strategy.Session).Closed;
        }

        public void BaseReopen(RequirementReopen method)
        {
            this.RequirementState = new RequirementStates(this.Strategy.Session).Active;
        }

        public void BaseCancel(RequirementCancel method)
        {
            this.RequirementState = new RequirementStates(this.Strategy.Session).Cancelled;
        }

        public void BaseHold(RequirementHold method)
        {
            this.RequirementState = new RequirementStates(this.Strategy.Session).OnHold;
        }
    }
}
