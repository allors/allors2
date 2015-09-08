namespace Allors.Meta
{
	#region Allors
	[Id("7dd7114a-9e74-45d5-b904-415514af5628")]
	#endregion
	[Inherit(typeof(ShipmentInterface))]
	public partial class CustomerReturnClass : Class
	{
		#region Allors
		[Id("29a65898-2f91-4163-a5ed-ccb8cd5b17cb")]
		[AssociationId("145f4e1b-b26d-4e44-8cc9-3afb537c58b2")]
		[RoleId("71416b87-4614-4d87-886c-4fe2eb936f40")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerReturnStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("e7586be1-f751-4ac6-940b-a65b50834619")]
		[AssociationId("ca71aca7-fa06-44d1-830a-3eaf5e2355a2")]
		[RoleId("3fb0c486-6e24-4f53-b7cf-f98596402d55")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerReturnStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		#region Allors
		[Id("fe3fd846-2d69-4d62-941b-dabc40a15e1f")]
		[AssociationId("82695003-f47f-4324-9a7c-d89949981354")]
		[RoleId("b2d65c28-fbff-430b-a7ca-39201ce655ad")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CustomerReturnObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		public static CustomerReturnClass Instance {get; internal set;}

		internal CustomerReturnClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToAddress.RoleType].IsRequiredOverride = true;
        }
    }
}