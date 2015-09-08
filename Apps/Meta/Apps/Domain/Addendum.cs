namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7baa7594-6890-4e1e-8c06-fc49b3ea262d")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("Addenda")]
	public partial class AddendumClass : Class
	{
		#region Allors
		[Id("2aaa6623-6f1a-4b40-91f0-4014108549d6")]
		[AssociationId("071735c4-bfbe-4f30-87a4-fbb4accc540c")]
		[RoleId("d9dea2e1-6582-4ce4-863f-4819d2cffe96")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Texts")]
		public RelationType Text;

		#region Allors
		[Id("30b99ed6-cb44-4401-b5bd-76c0099153d4")]
		[AssociationId("002ba83d-d60f-4365-90e0-4df952697ae7")]
		[RoleId("cfa04c20-ecc5-4942-b898-2966bf5052aa")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EffictiveDates")]
		public RelationType EffictiveDate;

		#region Allors
		[Id("45a9d28e-f131-44a8-aea5-1a9776be709e")]
		[AssociationId("4b41aff4-1882-4771-a85b-358cabdb6e3c")]
		[RoleId("8b37c47b-3dec-46e6-b669-6497cfdaf14b")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("f14af73d-8d7d-4c5b-bc6a-957830fd0a80")]
		[AssociationId("5430d382-14ff-4af1-8e1b-3b11142612e4")]
		[RoleId("51fc58ba-e9fb-4919-94e8-c8594f6e4ea5")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("CreationDates")]
		public RelationType CreationDate;



		public static AddendumClass Instance {get; internal set;}

		internal AddendumClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CreationDate.RoleType.IsRequired = true;
            this.Description.RoleType.IsRequired = true;
        }
    }
}