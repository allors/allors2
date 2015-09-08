namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("752a68b0-836e-4cd5-92d5-ebf2bfeda491")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Engagements")]
	public partial class EngagementClass : Class
	{
		#region Allors
		[Id("135792e4-42ad-4bf5-914b-ffc154330cd1")]
		[AssociationId("d0975157-1342-4a01-9fec-2a38f68a6080")]
		[RoleId("3cd0c697-6767-429a-8f34-e648b4fda46c")]
		#endregion
		[Indexed]
		[Type(typeof(AgreementInterface))]
		[Plural("Agreements")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Agreement;

		#region Allors
		[Id("1fb112f4-9628-40a8-9531-2d9ad24103ff")]
		[AssociationId("ad13c054-b721-4941-ad25-cc43936fed36")]
		[RoleId("9c189e14-b734-41b5-9012-8651986039d7")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("PlacingContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PlacingContactMechanism;

		#region Allors
		[Id("2e703224-c40a-45ee-8703-cafe11eda70a")]
		[AssociationId("f50ccbe1-9277-44a3-bb9b-5837124ddb6c")]
		[RoleId("468b2214-51d3-47e0-9207-9711f71f45ca")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("MaximumAmounts")]
		public RelationType MaximumAmount;

		#region Allors
		[Id("4afffb12-3a70-4903-af99-ed814fd6a444")]
		[AssociationId("1bf53c71-0b09-4053-8bab-73d783fdfd62")]
		[RoleId("dc2b8ab4-6eff-488c-955b-6329c1e1bfc3")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("BillToContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToContactMechanism;

		#region Allors
		[Id("55102c87-3cea-4c53-bafb-eb94bdda2b44")]
		[AssociationId("7453a2d2-368d-42dc-9304-c4706215361f")]
		[RoleId("97a86549-607b-48f0-bd92-4b3079a9f659")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("5b758dbc-11f6-4ac4-8f2b-639937814cae")]
		[AssociationId("9257851c-ab25-41c2-bbe5-6fb6ff8942af")]
		[RoleId("16370a01-5eaf-4c19-ad27-f0ede8f1ffef")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("BillToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BillToParty;

		#region Allors
		[Id("5f524932-4787-4bb4-8785-9a4f67dbb6ed")]
		[AssociationId("8dee87d0-f7ae-4f0c-b890-63e8642c4c90")]
		[RoleId("553bf637-0527-40cb-a298-4b41530f0950")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("PlacingParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PlacingParty;

		#region Allors
		[Id("63b73232-d81c-448f-92b0-eac314fcf41d")]
		[AssociationId("fce9a377-1b5a-4b23-bf1f-3f82a984cc4f")]
		[RoleId("882c84e0-c101-42cf-bb98-4939d68a5011")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("TakenViaInternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenViaInternalOrganisation;

		#region Allors
		[Id("6ca9444e-3e1c-4631-ad4e-1025fc85c1a4")]
		[AssociationId("9e82a268-e421-42f4-8b6f-460b3b1ce8aa")]
		[RoleId("108359bc-bb0a-426e-a6eb-7ab6de874721")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDates")]
		public RelationType StartDate;

		#region Allors
		[Id("83acc3c0-e87a-48fd-9c10-5070cd7c2a3d")]
		[AssociationId("4c014a78-e093-46e7-9c92-6404b3351f63")]
		[RoleId("e443cf26-a947-4bee-b0a9-3588434edb09")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("TakenViaContactMechanism")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenViaContactMechanism;

		#region Allors
		[Id("9bc7fbff-11dd-434e-91ff-7cef4e225bb3")]
		[AssociationId("950e4e27-42a4-4feb-bc7c-88a1045f6cc6")]
		[RoleId("f7234a3a-cb7c-4d92-903a-710ef833773b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("EstimatedAmounts")]
		public RelationType EstimatedAmount;

		#region Allors
		[Id("d9df5d5e-e0cc-4c9e-9e0a-dc5423561774")]
		[AssociationId("4fec3a1a-28d4-4984-a82a-aee949ba79d5")]
		[RoleId("abcb554b-44f8-424c-b991-1e56f15c5412")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EndDates")]
		public RelationType EndDate;

		#region Allors
		[Id("e1081976-b7e4-4e8e-85de-bd6ff096b39b")]
		[AssociationId("947ccaf8-9264-4703-86a8-58818128ff84")]
		[RoleId("5670bb8d-64a0-4492-9bb7-383b27144b31")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ContractDates")]
		public RelationType ContractDate;

		#region Allors
		[Id("ec51db38-d0de-430d-82cc-1105f336977b")]
		[AssociationId("f1a3abd2-636b-441a-90fc-9ac4cd9d0936")]
		[RoleId("237743f7-01c5-4762-b034-c43854f22157")]
		#endregion
		[Indexed]
		[Type(typeof(EngagementItemInterface))]
		[Plural("EngagementItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType EngagementItem;

		#region Allors
		[Id("f444a4cf-9722-420b-8ba1-0591492929e5")]
		[AssociationId("2aaf6afe-5fee-460f-ab37-75aa27be40f1")]
		[RoleId("ffb337c1-e7d0-4b86-b694-43b927efad34")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ClientPurchaseOrderNumbers")]
		public RelationType ClientPurchaseOrderNumber;

		#region Allors
		[Id("f7e10109-a4f2-4a54-b810-b00edf8a9330")]
		[AssociationId("5b22ae85-d5ae-41f7-a461-201ae48db339")]
		[RoleId("bdcce46f-e800-43fa-8cbc-2d4c5f65a7b2")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationContactRelationshipClass))]
		[Plural("TakenViaOrganisationContactRelationships")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TakenViaOrganisationContactRelationship;



		public static EngagementClass Instance {get; internal set;}

		internal EngagementClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.BillToParty.RoleType.IsRequired = true;
            this.TakenViaInternalOrganisation.RoleType.IsRequired = true;
            this.BillToContactMechanism.RoleType.IsRequired = true;
        }
    }
}