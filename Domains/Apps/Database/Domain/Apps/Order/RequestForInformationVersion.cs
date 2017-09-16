// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForInformationVersion.cs" company="Allors bvba">
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
    public partial class RequestForInformationVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (RequestForInformationVersionBuilder) method.Builder;
            var requestForInformation = builder.RequestForInformation;

            if (requestForInformation != null)
            {
                this.InternalComment = requestForInformation.InternalComment;
                this.Description = requestForInformation.Description;
                this.RequestDate = requestForInformation.RequestDate;
                this.RequiredResponseDate = requestForInformation.RequiredResponseDate;
                this.RequestItems = requestForInformation.RequestItems;
                this.RequestNumber = requestForInformation.RequestNumber;
                this.RespondingParties = requestForInformation.RespondingParties;
                this.Originator = requestForInformation.Originator;
                this.Currency = requestForInformation.Currency;
                this.FullfillContactMechanism = requestForInformation.FullfillContactMechanism;
                this.EmailAddress = requestForInformation.EmailAddress;
                this.TelephoneNumber = requestForInformation.TelephoneNumber;
                this.TelephoneCountryCode = requestForInformation.TelephoneCountryCode;
                this.CurrentObjectState = requestForInformation.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}