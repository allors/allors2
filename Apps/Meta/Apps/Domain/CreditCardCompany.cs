namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("86d934de-a5cf-46d3-aad3-2626c43ebc85")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CreditCardCompanies")]
	public partial class CreditCardCompanyClass : Class
	{
		#region Allors
		[Id("05860987-77be-4d8d-823d-99dd0e2cc822")]
		[AssociationId("002eff4d-2bcc-40bb-b311-7ae86207bdc7")]
		[RoleId("c9fe6f93-933e-4859-aaa2-ef3f5e2c8b44")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Name")]
		public RelationType Name;



		public static CreditCardCompanyClass Instance {get; internal set;}

		internal CreditCardCompanyClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
    }
}