namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("cdd79e23-a132-48b0-b88f-a03bd029f49d")]
	#endregion
	[Plural("Facilities")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(GeoLocatableInterface))]

  	public partial class FacilityInterface: Interface
	{
		#region Allors
		[Id("1a7f255a-3e94-41df-b71d-10ab36f38ffb")]
		[AssociationId("1341fb5d-26b6-4c07-bb31-a444c451c547")]
		[RoleId("cd2ee41e-ffba-4a59-9b9c-0d3eb581420c")]
		#endregion
		[Indexed]
		[Type(typeof(FacilityInterface))]
		[Plural("MadeUpOfs")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType MadeUpOf;

		#region Allors
		[Id("1daad895-cf57-4110-a4e0-117e0212c3e4")]
		[AssociationId("304a1b7b-215a-4fad-ab99-d0a974e8b0c0")]
		[RoleId("5ab48116-f33a-484e-9e6e-05a912efc9d5")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("SquareFootages")]
		public RelationType SquareFootage;

		#region Allors
		[Id("2df0999d-97cb-4c76-9f3e-076376e60e38")]
		[AssociationId("d29a1df5-e08f-4f7c-876c-a1ab737206a5")]
		[RoleId("5dd1abff-5a4c-4d30-8c69-1bcc83e5460e")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("4b55ee38-64e9-4c11-a204-36e2f460c5f8")]
		[AssociationId("87db14ec-a82a-4107-bae1-8ea945a68bce")]
		[RoleId("d576e2ee-dcf0-4f06-a496-42bceaf94399")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("FacilityContactMechanisms")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType FacilityContactMechanism;

		#region Allors
		[Id("b8f50794-848b-42be-9114-5eea579f5f71")]
		[AssociationId("7c9d689d-c38d-48b1-b1f0-5b211828ae8a")]
		[RoleId("d9ee92cb-3131-4442-be42-269ae294378d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Names")]
		public RelationType Name;

		#region Allors
		[Id("c73693db-9eae-4d81-a801-2ef4d619544b")]
		[AssociationId("e72fd5d0-f80f-4ecc-a6f7-a0f697c91e0b")]
		[RoleId("61dedec6-5aa1-4717-9b7b-34b77a1b31b9")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("Owners")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Owner;



		public static FacilityInterface Instance {get; internal set;}

		internal FacilityInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}