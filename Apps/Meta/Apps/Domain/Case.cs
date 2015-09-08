namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a0705b81-2eef-4c51-9454-a31bcedc20a3")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

	[Plural("Cases")]
	public partial class CaseClass : Class
	{
		#region Allors
		[Id("2286f83b-7992-4aa0-80fe-ad19e3c8c572")]
		[AssociationId("484381bd-6dbc-4a78-bc59-c21422b942b2")]
		[RoleId("c9951d63-5b1a-4053-9756-16b46a336288")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CaseStatusClass))]
		[Plural("CurrentCaseStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentCaseStatus;

		#region Allors
		[Id("51bfbe94-46a5-411f-ac10-8623bfc4472c")]
		[AssociationId("b8b5d65b-14c9-4ab0-89b9-4124d60cfeb7")]
		[RoleId("b49c43fd-798c-4608-a055-af04d97aa72d")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CaseStatusClass))]
		[Plural("CaseStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType CaseStatus;

		#region Allors
		[Id("65e310b5-1358-450c-aec2-985dcc724cdd")]
		[AssociationId("d815e7c2-fe40-470c-9ab9-007f7bc0465b")]
		[RoleId("fee6ebfb-3ce6-473b-9142-ea70ade93709")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDates")]
		public RelationType StartDate;

		#region Allors
		[Id("6f68ff93-e70c-4581-b0c7-94b2f75e6860")]
		[AssociationId("8fb16366-5def-43eb-a68b-2bac5169564b")]
		[RoleId("546330b8-f772-45f0-b2f0-66cf4a9ebffc")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CaseObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("87f64957-53f9-446e-ac1f-323a00da027f")]
		[AssociationId("289d52aa-fb69-4e7d-ba49-e4521614e19b")]
		[RoleId("dec26736-f037-48c1-a4b2-0247b9abf92b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static CaseClass Instance {get; internal set;}

		internal CaseClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}