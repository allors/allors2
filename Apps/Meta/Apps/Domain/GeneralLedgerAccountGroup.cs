namespace Allors.Meta
{
	#region Allors
	[Id("4a600c96-b813-46fc-8674-06bd3f85eae4")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	public partial class GeneralLedgerAccountGroupClass : Class
	{
		#region Allors
		[Id("3ab2ad60-3560-4817-9862-7f60c55bbc32")]
		[AssociationId("5ab6a428-e5e3-4265-8263-0e4ead0cb5f5")]
		[RoleId("b8f88fa3-9f8e-4e2c-be79-df02a37cfa40")]
		#endregion
		[Indexed]
		[Type(typeof(GeneralLedgerAccountGroupClass))]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Parent;

		#region Allors
		[Id("a48c3601-3d4c-43af-9502-d6beda764118")]
		[AssociationId("04b08f63-a2ac-43c2-889d-dbc8ebe86483")]
		[RoleId("7bd5e5e8-8605-46b2-b174-f345feb60f31")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		public RelationType Description;

		public static GeneralLedgerAccountGroupClass Instance {get; internal set;}

		internal GeneralLedgerAccountGroupClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}