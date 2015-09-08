namespace Allors.Meta
{
	#region Allors
	[Id("592260cc-365c-4769-b067-e95dd49609f5")]
	#endregion
	[Inherit(typeof(PaymentInterface))]
	public partial class ReceiptClass : Class
	{
		public static ReceiptClass Instance {get; internal set;}

		internal ReceiptClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[PaymentInterface.Instance.EffectiveDate.RoleType].IsRequiredOverride = true;
        }
    }
}