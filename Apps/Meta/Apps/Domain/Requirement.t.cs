namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d3f90525-b7fe-4f81-bccd-adf4f57260bc")]
	#endregion
	[Plural("Requirements")]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class RequirementInterface: Interface
	{
		#region Allors
		[Id("0f2c9ca2-9f2a-403e-8110-311fc0622326")]
		[AssociationId("099c426c-7b3f-4c9a-9059-525851488030")]
		[RoleId("178dfe82-99e2-4026-84f9-223e10e852c7")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("RequiredByDates")]
		public RelationType RequiredByDate;

		#region Allors
		[Id("2b828f2b-201d-4ae2-b64c-b2c5be713653")]
		[AssociationId("8bd1a8cc-4f4d-41ad-b4fb-d43d4759c0e4")]
		[RoleId("041107e2-0936-48a6-86dd-58ace8cbf7ac")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Authorizers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Authorizer;

		#region Allors
		[Id("36511540-8c83-467c-9ed0-ff5dee38c378")]
		[AssociationId("047c0186-3878-4895-9946-4b5a32c5bae1")]
		[RoleId("c80ac083-3b01-432c-81e1-56da054a5023")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(RequirementStatusClass))]
		[Plural("RequirementStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType RequirementStatus;

		#region Allors
		[Id("3a6ba1d0-3efb-44f3-b90b-7e504ed11140")]
		[AssociationId("5e36946c-46d4-4cd4-9ba7-e1c94746ffe9")]
		[RoleId("93f93798-b587-4f8f-9a82-2e0e9c870a52")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Reasons")]
		public RelationType Reason;

		#region Allors
		[Id("3ecf2b1e-ac3d-4533-9da1-341111fca04d")]
		[AssociationId("ea9f2ab4-6774-44eb-91ce-545f499ae792")]
		[RoleId("483b60d4-f3b7-47da-abb4-c7cefee78e2a")]
		#endregion
		[Indexed]
		[Type(typeof(RequirementInterface))]
		[Plural("Children")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Child;

		#region Allors
		[Id("43e11ee6-dcee-4a2c-80a7-8e04ee36ceb8")]
		[AssociationId("d2351d54-e600-400b-a350-9d2f81b5cf3d")]
		[RoleId("0d52a5f8-3852-4483-9f0d-a6877fc3b5a0")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("NeededFors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType NeededFor;

		#region Allors
		[Id("5ed2803c-02d4-4187-8155-bee79e1a0829")]
		[AssociationId("e0d08055-60ad-4417-b861-ef3b44f00e79")]
		[RoleId("c4abf003-69be-4e79-8958-701aac912d13")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;

		#region Allors
		[Id("ad98fd99-98b3-4876-b4af-b0c6aa7f41eb")]
		[AssociationId("d7a1af36-aea0-4e99-ab8b-b264c6bad301")]
		[RoleId("5d88d9ac-6895-4a72-811b-2c02c9daed9b")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(RequirementObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("b21d89b3-dfbf-484d-afa8-d6ee43cbef6c")]
		[AssociationId("7a949028-b354-4749-b048-ba487958fb01")]
		[RoleId("5a205868-1893-444d-8fa2-5ad1361555b9")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(RequirementStatusClass))]
		[Plural("CurrentRequirementStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentRequirementStatus;

		#region Allors
		[Id("b6b7e1e9-6cce-4ca0-a085-0afd3a58ec50")]
		[AssociationId("fc02e70b-da78-4f1e-aac3-8b4ba32cea90")]
		[RoleId("1137e61a-5efc-4c7c-9073-0f02c03b9408")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("Facilities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Facility;

		#region Allors
		[Id("bfce13c0-b5c2-46f0-b0fd-d0d288f8dc07")]
		[AssociationId("7c7ea2fb-451e-4a94-b5fd-cdeab8d97844")]
		[RoleId("f3923b48-a297-43b6-b318-bdafac87c36b")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ServicedBys")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ServicedBy;

		#region Allors
		[Id("c34694b4-bd8e-46e9-8bf1-fb1296738ab4")]
		[AssociationId("3bd6d711-d49b-4477-9173-e4f8a17f1d8b")]
		[RoleId("6f53fe03-c9a2-43b8-b38e-99597d751a82")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("EstimatedBudgets")]
		public RelationType EstimatedBudget;

		#region Allors
		[Id("d902fe48-c91f-43fe-b402-e0d87606124a")]
		[AssociationId("dfda3196-d793-4f58-af1e-661d943c8908")]
		[RoleId("943f924a-5e11-4e5e-9f3a-fc3df42acfc7")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("f553ad3c-675f-4b97-95c9-42f4d85eb5f9")]
		[AssociationId("995dbc52-905b-4572-a41f-8d39584f4132")]
		[RoleId("81fa089d-cc7f-4893-8186-ef6c98780b68")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("Quantities")]
		public RelationType Quantity;



		public static RequirementInterface Instance {get; internal set;}

		internal RequirementInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}