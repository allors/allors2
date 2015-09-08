namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("22f87630-11dd-480e-a721-9836af7685b1")]
	#endregion
	[Inherit(typeof(PeriodInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PartRevisions")]
	public partial class PartRevisionClass : Class
	{
		#region Allors
		[Id("6d1b4cec-abff-46db-a446-0f8889426b28")]
		[AssociationId("82cf09e8-535f-45fe-876e-484dfb3ea102")]
		[RoleId("946a84d0-36f8-4805-bbdd-a0779c9d008c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Reasons")]
		public RelationType Reason;

		#region Allors
		[Id("84561abd-08bc-4d28-b25c-22787d8bd7f0")]
		[AssociationId("4f700281-794b-4250-8bbe-f4fbbbcf8243")]
		[RoleId("8c408bc0-82f2-4343-93d2-87047c024ef9")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("SupersededByParts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SupersededByPart;

		#region Allors
		[Id("8a064340-def3-4d9f-89d6-3325b8a41f4d")]
		[AssociationId("6c674199-8f5f-469c-8f94-f35d64304968")]
		[RoleId("190e180b-cf6f-485d-80b2-9042e0fe04a7")]
		#endregion
		[Indexed]
		[Type(typeof(PartInterface))]
		[Plural("Parts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Part;



		public static PartRevisionClass Instance {get; internal set;}

		internal PartRevisionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}