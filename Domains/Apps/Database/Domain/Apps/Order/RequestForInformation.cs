// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForInformation.cs" company="Allors bvba">
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
    public partial class RequestForInformation
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment) ||
                !object.Equals(this.Description, this.CurrentVersion.Description) ||
                !object.Equals(this.RequestDate, this.CurrentVersion.RequestDate) ||
                !object.Equals(this.RequiredResponseDate, this.CurrentVersion.RequiredResponseDate) ||
                !object.Equals(this.RequestItems, this.CurrentVersion.RequestItems) ||
                !object.Equals(this.RequestNumber, this.CurrentVersion.RequestNumber) ||
                !object.Equals(this.RespondingParties, this.CurrentVersion.RespondingParties) ||
                !object.Equals(this.Originator, this.CurrentVersion.Originator) ||
                !object.Equals(this.Currency, this.CurrentVersion.Currency) ||
                !object.Equals(this.FullfillContactMechanism, this.CurrentVersion.FullfillContactMechanism) ||
                !object.Equals(this.EmailAddress, this.CurrentVersion.EmailAddress) ||
                !object.Equals(this.TelephoneNumber, this.CurrentVersion.TelephoneNumber) ||
                !object.Equals(this.TelephoneCountryCode, this.CurrentVersion.TelephoneCountryCode) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new RequestForInformationVersionBuilder(this.Strategy.Session).WithRequestForInformation(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }
    }
}
