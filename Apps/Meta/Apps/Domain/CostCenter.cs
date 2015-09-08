namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2ab70094-5481-4ecc-ae15-cb2131fbc2f1")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

	[Plural("CostCenters")]
	public partial class CostCenterClass : Class
	{
		#region Allors
		[Id("2a2125fd-c715-4a0f-8c1a-c1207f02a494")]
		[AssociationId("9a61f338-9bf3-45cf-abc0-89eb1cecf9c0")]
		[RoleId("f3ec0b58-3245-4b95-9f01-8a01f333750c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("76947134-0cae-4244-a8f3-fbb018930fd3")]
		[AssociationId("dfb847f1-46fe-4adc-94a1-c2d57d911348")]
		[RoleId("7e795eb5-b79d-455f-919b-bdfed4d926c3")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("InternalTransferGlAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalTransferGlAccount;

		#region Allors
		[Id("83a7ca20-8a73-4f8e-9729-731d25f70313")]
		[AssociationId("e4ccdfcc-790f-41d2-a225-0b46862aed11")]
		[RoleId("28b04874-6757-4cf9-a290-ed35ecba9d14")]
		#endregion
		[Indexed]
		[Type(typeof(CostCenterCategoryClass))]
		[Plural("CostCenterCategories")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CostCenterCategory;

		#region Allors
		[Id("975003f1-203e-4cbe-97d2-7f6ccc95f75a")]
		[AssociationId("8487030b-f156-4a0b-bcf0-22c880ded449")]
		[RoleId("341c226e-3da2-4976-9552-97e5b5796b1f")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("RedistributedCostsGlAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType RedistributedCostsGlAccount;

		#region Allors
		[Id("a3168a59-38ea-4359-b258-c9cbd656ce35")]
		[AssociationId("1f62b015-938d-4c36-9d96-879b28c237e0")]
		[RoleId("a401120c-42dd-4237-8afb-dcaa1e8e19f5")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("d7e01e38-d271-4c9c-847e-d26d9d4957af")]
		[AssociationId("89c07010-e93a-49c7-9a53-bb5588c38808")]
		[RoleId("922adffb-ad26-445c-b431-b19e9ee79842")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Actives")]
		public RelationType Active;

		#region Allors
		[Id("e6332140-65e7-4475-aea1-a80424640696")]
		[AssociationId("acd62c99-b86d-426a-8d7c-baca21d30665")]
		[RoleId("9cfa8fb1-fa68-4bb3-8440-5fe030a71c9d")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("UseGlAccountsOfBooking")]
		public RelationType UseGlAccountOfBooking;



		public static CostCenterClass Instance {get; internal set;}

		internal CostCenterClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}