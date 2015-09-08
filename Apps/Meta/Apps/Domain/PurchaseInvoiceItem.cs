namespace Allors.Meta
{
	#region Allors
	[Id("1ee19062-e36d-4836-b0e6-928a3957bd57")]
	#endregion
	[Inherit(typeof(InvoiceItemInterface))]
	public partial class PurchaseInvoiceItemClass : Class
	{
		#region Allors
		[Id("56e47122-faaa-4211-806c-1c19695fe434")]
		[AssociationId("826db2b1-3048-4237-8e83-0c472a166d49")]
		[RoleId("893de8bc-93eb-4864-89ba-efdb66b32fd5")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseInvoiceItemTypeClass))]
		[Plural("PurchaseInvoiceItemTypes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseInvoiceItemType;

		#region Allors
		[Id("65eebcc4-d5ef-4933-8640-973b67c65127")]
		[AssociationId("40703e06-25f8-425d-aa95-3c73fafbfa81")]
		[RoleId("05f86785-08d8-4282-9734-6230e807181b")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("Parts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Part;

		#region Allors
		[Id("99b3395c-bb6a-4a4f-b22f-900e76e22520")]
		[AssociationId("7ed44fc9-fc12-4a68-8938-1573ec28da2f")]
		[RoleId("7b17b8a8-fda9-4707-a7a3-e263b51bcd4f")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceItemStatusClass))]
		[Plural("CurrentInvoiceItemStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentInvoiceItemStatus;

		#region Allors
		[Id("c850d9db-682d-4a05-b62e-ab67eb19bd0d")]
		[AssociationId("b79e9e5b-f4a5-4bd0-bc46-ab55eea2f027")]
		[RoleId("136970e5-2ec7-4036-9abc-c84747d59d54")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceItemStatusClass))]
		[Plural("InvoiceItemStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType InvoiceItemStatus;

		#region Allors
		[Id("dbe5c72f-63e0-47a5-a5f5-f8a3ff83fd57")]
		[AssociationId("f8082d94-30fa-4a58-8bb0-bc5bb4f045ef")]
		[RoleId("69360188-077f-49f0-ba88-abb1f546d72c")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseInvoiceItemObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;
        
		public static PurchaseInvoiceItemClass Instance {get; internal set;}

		internal PurchaseInvoiceItemClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Part.RoleType.IsRequired = true;
            this.PurchaseInvoiceItemType.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[InvoiceItemInterface.Instance.ActualUnitPrice.RoleType].IsRequiredOverride = true;
        }
    }
}