 namespace Allors.Meta
{
    public partial class MetaParty
    {
        internal override void AppsExtend()
        {
            this.BankAccounts.RelationType.Workspace = true;
            this.BillingAccounts.RelationType.Workspace = true;
            this.BillingAddress.RelationType.Workspace = true;
            this.BillingInquiriesFax.RelationType.Workspace = true;
            this.BillingInquiriesPhone.RelationType.Workspace = true;
            this.CellPhoneNumber.RelationType.Workspace = true;
            this.CreatedBy.RelationType.Workspace = true;
            this.CreationDate.RelationType.Workspace = true;
            this.CreditCards.RelationType.Workspace = true;
            this.CurrentContacts.RelationType.Workspace = true;
            this.CurrentOrganisationContactRelationships.RelationType.Workspace = true;
            this.CurrentPartyContactMechanisms.RelationType.Workspace = true;
            this.DefaultPaymentMethod.RelationType.Workspace = true;
            this.DefaultShipmentMethod.RelationType.Workspace = true;
            this.GeneralCorrespondence.RelationType.Workspace = true;
            this.GeneralFaxNumber.RelationType.Workspace = true;
            this.GeneralEmail.RelationType.Workspace = true;
            this.GeneralPhoneNumber.RelationType.Workspace = true;
            this.HomeAddress.RelationType.Workspace = true;
            this.InactiveContacts.RelationType.Workspace = true;
            this.InactiveOrganisationContactRelationships.RelationType.Workspace = true;
            this.InactivePartyContactMechanisms.RelationType.Workspace = true;
            this.InternetAddress.RelationType.Workspace = true;
            this.LastModifiedBy.RelationType.Workspace = true;
            this.LastModifiedDate.RelationType.Workspace = true;
            this.Locale.RelationType.Workspace = true;
            this.OpenOrderAmount.RelationType.Workspace = true;
            this.OrderAddress.RelationType.Workspace = true;
            this.OrderInquiriesFax.RelationType.Workspace = true;
            this.OrderInquiriesPhone.RelationType.Workspace = true;
            this.PartyContactMechanisms.RelationType.Workspace = true;
            this.PersonalEmailAddress.RelationType.Workspace = true;
            this.PreferredCurrency.RelationType.Workspace = true;
            this.ShippingAddress.RelationType.Workspace = true;
            this.ShippingInquiriesFax.RelationType.Workspace = true;
            this.ShippingInquiriesPhone.RelationType.Workspace = true;
        }
    }
}