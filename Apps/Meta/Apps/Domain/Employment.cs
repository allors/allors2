namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6a7e45b2-36b2-4d2e-a29c-0cc13851766f")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]
	[Inherit(typeof(DeletableInterface))]

	[Plural("Employments")]
	public partial class EmploymentClass : Class
	{
		#region Allors
		[Id("776e5cb7-6926-4455-89ed-c1f916018a25")]
		[AssociationId("9d997658-68ca-41a3-9551-9cc793811a4e")]
		[RoleId("28191884-d18f-400b-96df-7da1a328d88e")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("Employers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employer;

		#region Allors
		[Id("a243feb0-e5f0-41b4-9b13-a09bb8413fb3")]
		[AssociationId("03bac42d-dcbc-40f3-a130-7b4f3b37f523")]
		[RoleId("1fb50b4b-2a1b-4139-a376-48f1c72c4645")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Employees")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employee;

		#region Allors
		[Id("ba6d2658-9c07-4254-a664-21df0e2fcb6a")]
		[AssociationId("f512d8bd-5ea3-461c-9310-6ab93696763d")]
		[RoleId("3c2fae70-49b5-407f-823c-db9b9052fb1e")]
		#endregion
		[Indexed]
		[Type(typeof(PayrollPreferenceClass))]
		[Plural("PayrollPreferences")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PayrollPreference;

		#region Allors
		[Id("c8fd6c79-f909-414e-b9e3-5e911e2e2080")]
		[AssociationId("da451dab-03db-4bc5-8641-93ec74570f4f")]
		[RoleId("0bef74ad-3eb2-494e-846e-6ca3bbfb057b")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentTerminationReasonClass))]
		[Plural("EmploymentTerminationReasons")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EmploymentTerminationReason;

		#region Allors
		[Id("e79807d4-dcf8-47e2-b510-e8535f1ec436")]
		[AssociationId("6b4896d8-8bf6-4908-acb9-dc2438263fb7")]
		[RoleId("96ff4ce3-5e0b-408e-9641-edf2e06dc508")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentTerminationClass))]
		[Plural("EmploymentTerminations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EmploymentTermination;



		public static EmploymentClass Instance {get; internal set;}

		internal EmploymentClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Employer.RoleType.IsRequired = true;
            this.Employee.RoleType.IsRequired = true;
        }
    }
}