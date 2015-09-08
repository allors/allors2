namespace Allors.Meta
{
	#region Allors
	[Id("11c608b0-4755-4e74-b720-4eb94e83c24d")]
	#endregion
	[Inherit(typeof(DeletableInterface))]
	[Inherit(typeof(PriceComponentInterface))]
	public partial class BasePriceClass : Class
	{
		public static BasePriceClass Instance {get; internal set;}

		internal BasePriceClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Price.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Currency.RoleType].IsRequiredOverride = true;
        }
    }
}