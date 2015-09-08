namespace Allors.Meta
{
	#region Allors
	[Id("ce5c78ee-f892-4ced-9b21-51d84c77127f")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	public partial class GeneralLedgerAccountTypeClass : Class
	{
		#region Allors
		[Id("e01a0752-531b-4ee3-a58e-711f377247e1")]
		[AssociationId("dcfb5761-0d99-4a8f-afc9-2c0e64cd1c68")]
		[RoleId("7d579eae-a239-4f55-9719-02f39dbc42d8")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		public static GeneralLedgerAccountTypeClass Instance {get; internal set;}

		internal GeneralLedgerAccountTypeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}