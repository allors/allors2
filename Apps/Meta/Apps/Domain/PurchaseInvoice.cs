namespace Allors.Meta
{
	#region Allors
	[Id("7d7e4b6d-eebd-460c-b771-a93cd8d64bce")]
	#endregion
	[Inherit(typeof(InvoiceInterface))]
	public partial class PurchaseInvoiceClass : Class
	{
        #region Allors
        [Id("ECD12D89-5B32-416C-8478-06FF904C6A61")]
        #endregion
        public MethodType Ready;

        #region Allors
        [Id("16C0CC36-B908-4912-B420-2FD3E31BB966")]
        #endregion
        public MethodType Approve;

        #region Allors
        [Id("46BB5168-5250-4B5A-9DF0-045AFB589AAD")]
        #endregion
        public MethodType Cancel;

        #region Allors
        [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
		[AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
		[RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseInvoiceItemClass))]
		[Plural("PurchaseInvoiceItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PurchaseInvoiceItem;

		#region Allors
		[Id("86859b7b-e627-43fe-ba75-711d4c104807")]
		[AssociationId("ba1aeb33-0351-4fbf-b80c-881cdf4ded5c")]
		[RoleId("7caa47ab-1f54-4fad-87b8-639b37269635")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("BilledToInternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BilledToInternalOrganisation;

		#region Allors
		[Id("8f9e98b7-c87c-47c7-a267-3044c7414534")]
		[AssociationId("1b4b2f6b-7294-428f-b0ea-beb43050557a")]
		[RoleId("bea637e9-c320-4bdb-ac4b-d571e3fa0c8d")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceStatusClass))]
		[Plural("CurrentInvoiceStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentInvoiceStatus;

		#region Allors
		[Id("bc059d0f-e9bd-41e8-82ff-9615a01ec24a")]
		[AssociationId("770c0376-8552-4d0c-b45f-b759018c3c85")]
		[RoleId("5658422f-4097-49db-b97c-79bab6f337b4")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
		[AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
		[RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("BilledFromParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BilledFromParty;

		#region Allors
		[Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
		[AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
		[RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseInvoiceTypeClass))]
		[Plural("PurchaseInvoiceTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseInvoiceType;

		#region Allors
		[Id("ed3987d4-3dd1-4483-bcbb-8f1f0b18ff84")]
		[AssociationId("1c1d90ff-5910-4f39-b6ad-aa12a6e6c60e")]
		[RoleId("d23c55ff-857b-40bc-b041-15f0ceb910a5")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceStatusClass))]
		[Plural("InvoiceStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceStatus;

		public static PurchaseInvoiceClass Instance {get; internal set;}

		internal PurchaseInvoiceClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.PurchaseInvoiceType.RoleType.IsRequired = true;
            this.BilledToInternalOrganisation.RoleType.IsRequired = true;
            this.BilledFromParty.RoleType.IsRequired = true;

            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}