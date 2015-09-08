namespace Allors.Meta
{
	#region Allors
	[Id("cd66a79f-c4b8-4c33-b6ec-1928809b6b88")]
	#endregion
	[Inherit(typeof(ShipmentInterface))]
	public partial class TransferClass : Class
	{
		#region Allors
		[Id("01757aca-7f45-4061-8721-1fa3d8cca852")]
		[AssociationId("b0b86e04-cd64-4a19-94dd-86ba558b478b")]
		[RoleId("d775ad19-df10-4941-b384-d0de7c3ed943")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(TransferObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("2fc36280-2378-4c2d-aab1-b2f038a5cfa5")]
		[AssociationId("731be3ab-46e5-4ff9-acc7-c7d106f32896")]
		[RoleId("ea288e25-6d3c-4138-86fc-4e0fb86a088e")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(TransferStatusClass))]
		[Plural("CurrentShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentShipmentStatus;

		#region Allors
		[Id("e415cf27-7ae7-48a7-a889-ad90a7384a68")]
		[AssociationId("b205e173-5355-4dcc-a615-521b46e3759a")]
		[RoleId("96976d0f-10b8-4c67-a9a1-9b87b64eb46c")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(TransferStatusClass))]
		[Plural("ShipmentStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ShipmentStatus;

		public static TransferClass Instance {get; internal set;}

		internal TransferClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ShipmentInterface.Instance.ShipToAddress.RoleType].IsRequiredOverride = true;
        }
    }
}