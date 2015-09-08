namespace Allors.Meta
{
	#region Allors
	[Id("a917f763-e54a-4693-bf7b-d8e7aead8fe6")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(AgreementTermInterface))]
	public partial class InvoiceTermClass : Class
	{
		public static InvoiceTermClass Instance {get; internal set;}

		internal InvoiceTermClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[AgreementTermInterface.Instance.TermType.RoleType].IsRequiredOverride = true;
        }
    }
}