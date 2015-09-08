namespace Allors.Meta
{
	#region Allors
	[Id("71e50a16-fc60-4177-aed0-e89c7f10f465")]
	#endregion
	[Inherit(typeof(FacilityInterface))]
	public partial class WarehouseClass : Class
	{
		public static WarehouseClass Instance {get; internal set;}

		internal WarehouseClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[FacilityInterface.Instance.Owner.RoleType].IsRequiredOverride = true;
        }
    }
}