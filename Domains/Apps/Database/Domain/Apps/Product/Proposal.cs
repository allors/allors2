// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Proposal.cs" company="Allors bvba">
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
    public partial class Proposal
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new QuoteObjectStates(this.Strategy.Session).Created;
            }

            if (!this.ExistIssueDate)
            {
                this.IssueDate = DateTime.UtcNow;
            }

            if (!this.ExistIssuer)
            {
                this.Issuer = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistQuoteNumber)
            {
                this.QuoteNumber = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation.DeriveNextQuoteNumber();
            }
        }
    }
}
