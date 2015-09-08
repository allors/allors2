namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c2b88d46-c321-48c4-9493-22a886d91bf9")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("TransferStatuses")]
	public partial class TransferStatusClass : Class
	{
		#region Allors
		[Id("05a4a5a6-cdaf-4ec8-9a34-cbd40753789b")]
		[AssociationId("a259de23-8d18-4d51-81e3-42796a144b5b")]
		[RoleId("b3fd264c-91a5-425b-b9a0-48eb5cc765fd")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("a08cde84-30e0-4f99-b6b5-35b45c3fa2b8")]
		[AssociationId("0fb9e813-bd7d-40c8-a1c2-10a569e873c8")]
		[RoleId("63627877-78be-4ffc-aa0d-740049add137")]
		#endregion
		[Indexed]
		[Type(typeof(TransferObjectStateClass))]
		[Plural("TransferObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TransferObjectState;



		public static TransferStatusClass Instance {get; internal set;}

		internal TransferStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}