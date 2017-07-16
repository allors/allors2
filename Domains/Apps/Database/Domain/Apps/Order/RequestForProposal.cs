// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForProposal.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public partial class RequestForProposal
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new RequestObjectStates(this.Strategy.Session).Draft;
            }

            if (!this.ExistRequestDate)
            {
                this.RequestDate = DateTime.UtcNow;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistRequestNumber)
            {
                this.RequestNumber = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation.DeriveNextRequestNumber();
            }
        }

        private void DeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.LastObjectState))
            {
                var currentStatus = new RequestStatusBuilder(this.Strategy.Session).WithRequestObjectState(this.CurrentObjectState).Build();
                this.AddRequestStatus(currentStatus);
                this.CurrentRequestStatus = currentStatus;
            }
        }

        public void AppsCancel(RequestCancel method)
        {
            this.CurrentObjectState = new RequestObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsReject(RequestReject method)
        {
            this.CurrentObjectState = new RequestObjectStates(this.Strategy.Session).Rejected;
        }

        public void AppsSubmit(RequestSubmit method)
        {
            this.CurrentObjectState = new RequestObjectStates(this.Strategy.Session).Submitted;
        }

    }
}
