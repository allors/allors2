namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0091574c-edac-4376-8d03-c7e2c2d8132f")]
	#endregion
	[Plural("PartSpecifications")]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class PartSpecificationInterface: Interface
	{
		#region Allors
		[Id("202bc60e-5702-4dce-a41a-8dc5e198090c")]
		[AssociationId("854fcf78-d6fe-40c8-a988-54df5fb5933c")]
		[RoleId("e60cafba-ec5a-4578-83e4-a4a63d4e49a6")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartSpecificationStatusClass))]
		[Plural("PartSpecificationStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PartSpecificationStatus;

		#region Allors
		[Id("4bfcdcc0-d6d3-4335-92ce-a8b1271f4124")]
		[AssociationId("792ce48c-749e-4bd3-b0a9-3ab93e802d8d")]
		[RoleId("1ef186ee-e996-4e79-bb81-7f7c406702d1")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartSpecificationObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("6a83ef4b-1ef5-4782-b9fd-19e3231c29c5")]
		[AssociationId("93f4241d-23ea-46ad-bcaa-fd1f5c909c43")]
		[RoleId("c2b4a79f-c245-40d5-834e-5939c7748462")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("DocumentationDates")]
		public RelationType DocumentationDate;

		#region Allors
		[Id("79f03090-e058-439c-9398-738f08be2be1")]
		[AssociationId("8d5e16e5-2a18-4779-ad87-537db639c94e")]
		[RoleId("f5451e3d-67ed-416f-aeeb-45daf876fd0d")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PartSpecificationStatusClass))]
		[Plural("CurrenPartSpecificationStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentPartSpecificationStatus;

		#region Allors
		[Id("e20b0fd5-f10a-44df-8bef-f454e7d23bce")]
		[AssociationId("0c7ad60f-57c9-469b-b8e4-dabeae4398ee")]
		[RoleId("6a208020-712c-4ce8-b69b-ea4523ba2e85")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static PartSpecificationInterface Instance {get; internal set;}

		internal PartSpecificationInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}