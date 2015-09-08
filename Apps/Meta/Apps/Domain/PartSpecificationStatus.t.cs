namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f1ae6225-15d0-4359-8188-afb73265a617")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PartSpecificationStatuses")]
	public partial class PartSpecificationStatusClass : Class
	{
		#region Allors
		[Id("14fdbce7-3494-48fb-a885-3b688b0c4e15")]
		[AssociationId("9b060799-5fca-4e1f-96c0-953444f4b6ac")]
		[RoleId("53c22224-4741-4b2a-ac1f-2174c1bda312")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("3b3db2a8-bd50-422b-8605-01142cac2654")]
		[AssociationId("f3ae080e-2c22-46e2-9a78-2178f32eab55")]
		[RoleId("a62f66c9-5ed6-4059-8c2e-3a01e268f4eb")]
		#endregion
		[Indexed]
		[Type(typeof(PartSpecificationObjectStateClass))]
		[Plural("PartSpecificationObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PartSpecificationObjectState;



		public static PartSpecificationStatusClass Instance {get; internal set;}

		internal PartSpecificationStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}