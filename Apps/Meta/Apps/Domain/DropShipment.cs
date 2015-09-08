namespace Allors.Meta
{
	#region Allors
	[Id("a981c832-dd3a-4b97-9bc9-d2dd83872bf2")]
	#endregion
	[Inherit(typeof(ShipmentInterface))]
	public partial class DropShipmentClass : Class
	{
		#region Allors
		[Id("0984f98c-fc64-4c86-aeb6-1d804d1506db")]
		[AssociationId("f7de3d8b-e404-4652-8eb1-dc58f8307e14")]
		[RoleId("9ac05629-f7ae-422e-8131-78389ba7ecf9")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(DropShipmentStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("44230591-89df-46ec-882c-09bbac7fd5d2")]
		[AssociationId("fa5c5391-6bf5-435c-ba35-08d5315216db")]
		[RoleId("a3b29fd7-cf97-4cbf-9329-681542e8de75")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(DropShipmentStatusClass))]
		[Plural("CurrentShipmentStatus")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("a7d6815b-9d6c-44c4-a80f-bc2fd8aa1ea7")]
		[AssociationId("14c83374-67ae-480b-a67d-597e8614480e")]
		[RoleId("9b4e523e-215a-4b2a-bd99-1540113e5fc3")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(DropShipmentObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		public static DropShipmentClass Instance {get; internal set;}

		internal DropShipmentClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToAddress.RoleType].IsRequiredOverride = true;
        }
    }
}