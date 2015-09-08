namespace Allors.Meta
{
	#region Allors
	[Id("d4cfdb68-9128-4afc-8670-192e55115499")]
	#endregion
	[Inherit(typeof(PriceComponentInterface))]
	public partial class ManufacturerSuggestedRetailPriceClass : Class
	{
		public static ManufacturerSuggestedRetailPriceClass Instance {get; internal set;}

		internal ManufacturerSuggestedRetailPriceClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PriceComponentInterface.Instance.Price.RoleType].IsRequiredOverride = true;
        }
    }
}