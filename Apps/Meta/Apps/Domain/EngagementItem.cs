namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("aa3bf631-5aa5-48ab-a249-ef61f640fb72")]
	#endregion
	[Plural("EngagementItems")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class EngagementItemInterface: Interface
	{
		#region Allors
		[Id("141333b6-2cc9-487e-acc1-86d314f2b30a")]
		[AssociationId("17fbbe0c-7d74-46ba-b5dd-a115536dd1a6")]
		[RoleId("10b8af44-2efd-4549-981c-8471860dfb55")]
		#endregion
		[Indexed]
		[Type(typeof(QuoteItemClass))]
		[Plural("QuoteItems")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType QuoteItem;

		#region Allors
		[Id("2a187dcd-5004-4722-a0ec-e571cd5b5bc6")]
		[AssociationId("f733d61f-a981-4a80-9816-dc10e0d1e2c9")]
		[RoleId("a8912656-740c-4216-93f6-8fff119c385e")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("33fe3f86-8b73-4a70-b9c0-62ac27531ac3")]
		[AssociationId("24a3d499-1f30-4b0e-8a27-a42808c4b1a2")]
		[RoleId("5e4915f7-955d-41a9-9c38-d8b6f7837ea4")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ExpectedStartDates")]
		public RelationType ExpectedStartDate;

		#region Allors
		[Id("3635cb84-2d4f-4fa1-ac18-4c8a6cc129c5")]
		[AssociationId("b58461be-8138-42e1-9e4b-e095ae66fc90")]
		[RoleId("afc29589-892c-41ca-94b3-92a775009a6e")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ExpectedEndDates")]
		public RelationType ExpectedEndDate;

		#region Allors
		[Id("40b24df7-6834-401a-a598-82203af63f99")]
		[AssociationId("04cbacfd-910f-4707-b952-ffdaaab28c60")]
		[RoleId("3345748e-d859-47f4-bb45-1920469b1cfc")]
		#endregion
		[Indexed]
		[Type(typeof(WorkEffortInterface))]
		[Plural("EngagementWorkFulfillments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EngagementWorkFulfillment;

		#region Allors
		[Id("9133f59e-048d-4020-88e4-5a4bc36d663b")]
		[AssociationId("46ad58c7-3125-4307-93ae-58c386e98899")]
		[RoleId("3065d420-15ec-47d3-9fa6-56a79d4c315b")]
		#endregion
		[Indexed]
		[Type(typeof(EngagementRateClass))]
		[Plural("EngagementRates")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType EngagementRate;

		#region Allors
		[Id("9e1f4da4-41af-4030-b67f-79f1f49fa076")]
		[AssociationId("b5361ebf-2809-4fe7-8f24-bd68ec61c9b8")]
		[RoleId("124043c0-dd7e-4d94-9c0c-a3804c343f11")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(EngagementRateClass))]
		[Plural("CurrentEngagementRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentEngagementRate;

		#region Allors
		[Id("b445f2d6-55a6-4cb4-9550-5be8863eddb6")]
		[AssociationId("21509869-1643-402a-a5eb-9657f1f01af9")]
		[RoleId("8844d711-33d6-4d19-ad21-edcd60851f1d")]
		#endregion
		[Indexed]
		[Type(typeof(EngagementItemInterface))]
		[Plural("OrderedWiths")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType OrderedWith;

		#region Allors
		[Id("c2ec3c6b-af56-4c6b-bdaf-76d3ea340bf7")]
		[AssociationId("d9a53328-0414-4403-bd54-37b48ec05823")]
		[RoleId("a2dd5921-6ec7-4d7b-8aae-9a7e685688d1")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("CurrentAssignedProfessionals")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentAssignedProfessional;

		#region Allors
		[Id("c7204c16-67b1-4e6d-b787-ce8ab9c6c111")]
		[AssociationId("d417f454-c1fa-41da-8b00-653b27d875a4")]
		[RoleId("eaa02501-f6d8-4d12-b11e-523bf70805a4")]
		#endregion
		[Indexed]
		[Type(typeof(ProductInterface))]
		[Plural("Products")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Product;

		#region Allors
		[Id("dbb3d0c5-836d-477b-a42f-b260f3316458")]
		[AssociationId("888670c7-e42c-41eb-994f-91af9d2d93f3")]
		[RoleId("ce43a83c-0289-42b5-9330-0341fa847809")]
		#endregion
		[Indexed]
		[Type(typeof(ProductFeatureInterface))]
		[Plural("ProductFeatures")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ProductFeature;



		public static EngagementItemInterface Instance {get; internal set;}

		internal EngagementItemInterface() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}