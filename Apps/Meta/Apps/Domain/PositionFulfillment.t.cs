namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6a03924c-914b-4660-b7e8-5174caa0dff9")]
	#endregion
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PositionFulfillments")]
	public partial class PositionFulfillmentClass : Class
	{
		#region Allors
		[Id("30631f6e-3e70-4394-9540-0572230cd461")]
		[AssociationId("ebcfbd12-ea78-4dd1-b102-05110c7d4a95")]
		[RoleId("3fc029a2-3465-4518-830a-348bd2235a71")]
		#endregion
		[Indexed]
		[Type(typeof(PositionClass))]
		[Plural("Positions")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Position;

		#region Allors
		[Id("4de369bb-6fa3-4fd4-9056-0e70a72c9b9f")]
		[AssociationId("23fa9951-ceb1-44b2-af36-f3e4955018d1")]
		[RoleId("76c3f430-bf53-4e6f-89af-ea91afbd6795")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Persons")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Person;



		public static PositionFulfillmentClass Instance {get; internal set;}

		internal PositionFulfillmentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}