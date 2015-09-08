namespace Allors.Meta
{
	#region Allors
	[Id("41f1aa5a-5043-42bb-aaf5-7d57a9deaccb")]
	#endregion
	[Inherit(typeof(BudgetInterface))]
	public partial class CapitalBudgetClass : Class
	{
		public static CapitalBudgetClass Instance {get; internal set;}

		internal CapitalBudgetClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[BudgetInterface.Instance.CurrentObjectState.RoleType].IsRequiredOverride = true;
        }
    }
}