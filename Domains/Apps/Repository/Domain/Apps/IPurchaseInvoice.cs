namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("A493423E-2975-475D-9DB6-521827D3B3C7")]
    #endregion
    public partial interface IPurchaseInvoice : IInvoice 
    {
        #region Allors
        [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
        [AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
        [RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }
        
        #region Allors
        [Id("86859b7b-e627-43fe-ba75-711d4c104807")]
        [AssociationId("ba1aeb33-0351-4fbf-b80c-881cdf4ded5c")]
        [RoleId("7caa47ab-1f54-4fad-87b8-639b37269635")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InternalOrganisation BilledToInternalOrganisation { get; set; }
        
        #region Allors
        [Id("bc059d0f-e9bd-41e8-82ff-9615a01ec24a")]
        [AssociationId("770c0376-8552-4d0c-b45f-b759018c3c85")]
        [RoleId("5658422f-4097-49db-b97c-79bab6f337b4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        PurchaseInvoiceObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
        [AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
        [RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        Party BilledFromParty { get; set; }
        
        #region Allors
        [Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
        [AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
        [RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseInvoiceType PurchaseInvoiceType { get; set; }
    }
}