namespace Allors.Meta
{
	#region Allors
	[Id("5835aca6-214b-41cf-aefe-e361dda026d7")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]
	public partial class OneTimeChargeClass : Class
	{
		public static OneTimeChargeClass Instance {get; internal set;}

		internal OneTimeChargeClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Price.RoleType].IsRequiredOverride = true;
        }
    }
}