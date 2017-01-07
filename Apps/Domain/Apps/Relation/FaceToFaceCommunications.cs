// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceToFaceCommunications.cs" company="Allors bvba">
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
    using Meta;

    public partial class FaceToFaceCommunications
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            ObjectState scheduled = new CommunicationEventObjectStates(this.Session).Scheduled;
            ObjectState cancelled = new CommunicationEventObjectStates(this.Session).Cancelled;
            ObjectState closed = new CommunicationEventObjectStates(this.Session).Completed;

            config.Deny(this.ObjectType, scheduled, M.CommunicationEvent.Reopen);
            config.Deny(this.ObjectType, closed, M.CommunicationEvent.Close, M.CommunicationEvent.Cancel);

            config.Deny(this.ObjectType, closed, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
        }
    }
}