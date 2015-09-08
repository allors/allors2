namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6940c300-47e6-44f2-b93b-d70bed9de602")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("EmploymentApplications")]
	public partial class EmploymentApplicationClass : Class
	{
		#region Allors
		[Id("528de310-3268-4b17-ab42-49dea27d5aee")]
		[AssociationId("ca9bf054-52cf-40f1-995f-0e504b5bee9b")]
		[RoleId("9b07c065-678f-4d21-878f-4ac2029dddc5")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ApplicationDates")]
		public RelationType ApplicationDate;

		#region Allors
		[Id("75cc1a7c-6bf7-4798-9ddc-fd1b283aed19")]
		[AssociationId("edffb19c-1b3d-45fc-bc52-d44bd51fc6e2")]
		[RoleId("502bf1bc-596d-44e7-b9f3-148433028740")]
		#endregion
		[Indexed]
		[Type(typeof(PositionClass))]
		[Plural("Positions")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Position;

		#region Allors
		[Id("7d3147e2-9709-42bc-a6cd-5b922bfc143d")]
		[AssociationId("e8ec31ed-ebd4-4a2c-8948-7170adf61572")]
		[RoleId("af280063-af23-4afb-9dd6-8f44141c275e")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentApplicationStatusClass))]
		[Plural("EmploymentApplicationStatuses")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EmploymentApplicationStatus;

		#region Allors
		[Id("a4c14261-14a2-404c-814f-6475368d685a")]
		[AssociationId("f9cf5e5a-d262-4898-91f3-a69b3612f0a8")]
		[RoleId("6d7984e5-d1c7-4a53-99a1-49e125db39b9")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Persons")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Person;

		#region Allors
		[Id("b0799b22-bff3-49d7-8f9a-3ea41c540778")]
		[AssociationId("90dc458e-243e-42d6-950d-3994f7617981")]
		[RoleId("4222311d-cb11-4cec-a547-eb45cfe94732")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentApplicationSourceClass))]
		[Plural("EmploymentApplicationSources")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EmploymentApplicationSource;



		public static EmploymentApplicationClass Instance {get; internal set;}

		internal EmploymentApplicationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Person.RoleType.IsRequired = true;
            this.ApplicationDate.RoleType.IsRequired = true;
            this.Position.RoleType.IsRequired = true;
        }
    }
}