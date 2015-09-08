namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("87939632-40ff-4a3a-a874-74790e810890")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PurchaseShipmentStatuses")]
	public partial class PurchaseShipmentStatusClass : Class
	{
		#region Allors
		[Id("01d6a244-e174-4a91-8f27-5af54401bed1")]
		[AssociationId("09311427-0d20-4c65-85eb-371d1bcfb23f")]
		[RoleId("125cbf28-2721-4e1b-8cb5-ce3f5a6a464e")]
		#endregion
		[Indexed]
		[Type(typeof(PurchaseShipmentObjectStateClass))]
		[Plural("PurchaseShipmentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PurchaseShipmentObjectState;

		#region Allors
		[Id("a243d65e-81ac-49e7-af1b-1b97faa7360e")]
		[AssociationId("9d74270a-7197-43ee-92c9-8f06bd1b48db")]
		[RoleId("fac16474-a909-4566-b55e-5849927aa431")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;



		public static PurchaseShipmentStatusClass Instance {get; internal set;}

		internal PurchaseShipmentStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}