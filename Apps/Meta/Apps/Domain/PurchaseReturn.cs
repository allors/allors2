namespace Allors.Meta
{
	#region Allors
	[Id("a0cf565a-2dcf-4513-9110-8c34468d993f")]
	#endregion
	[Inherit(typeof(ShipmentInterface))]
	public partial class PurchaseReturnClass : Class
	{
		#region Allors
		[Id("01d0a8b8-0361-440f-8d96-967578262318")]
		[AssociationId("9a79ad26-180a-45fd-8b50-ca8c641e9f77")]
		[RoleId("e44876b6-c198-493a-8efc-4a4d09bd2b00")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseReturnStatusClass))]
		[Plural("CurrentShipmentStatus")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("91b10295-d8d6-4240-914c-9ee8a6c21b96")]
		[AssociationId("47441947-8d72-4730-ab25-077dc80b4ca1")]
		[RoleId("ba9b3f52-0a0e-46ec-b3fb-d9330ebd5269")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseReturnObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("a5f3cf87-1730-4841-9df4-2638b10f3222")]
		[AssociationId("b1cb7246-2417-4618-bb03-decb38a0fc9f")]
		[RoleId("5ede1e3f-bed7-4603-adfe-f576e23a2e2f")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PurchaseReturnStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		public static PurchaseReturnClass Instance {get; internal set;}

		internal PurchaseReturnClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToAddress.RoleType].IsRequiredOverride = true;
        }
    }
}