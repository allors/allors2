// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaxCommunication.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    public partial class FaxCommunication
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();

        }

        public void AppsOnDeriveFromParties()
        {
            this.RemoveFromParties();
            this.AddFromParty(this.Originator);
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();
            this.AddToParty(this.Receiver);
        }

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();
            this.AddInvolvedParty(this.Owner);

            if (this.ExistOriginator)
            {
                this.AddInvolvedParty(this.Originator);
            }

            if (this.ExistReceiver)
            {
                this.AddInvolvedParty(this.Receiver);
            }
        }
       
    }
}
