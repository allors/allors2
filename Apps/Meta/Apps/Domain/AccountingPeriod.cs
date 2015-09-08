namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6b56e13b-d075-40f1-8e33-a9a4c6cadb96")]
	#endregion
	[Inherit(typeof(BudgetInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("AccountingPeriods")]
	public partial class AccountingPeriodClass : Class
	{
		#region Allors
		[Id("0fd97106-1e39-4629-a7bd-ad263bc2d296")]
		[AssociationId("816f8a0b-3c3a-4dd2-a50e-5c3cd197c592")]
		[RoleId("2d803ef5-2e9a-46fa-8690-5d5ef00f6785")]
		#endregion
		[Indexed]
		[Type(typeof(AccountingPeriodClass))]
		[Plural("Parents")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Parent;

		#region Allors
		[Id("93b16073-8196-40c2-8777-5719fe1e6360")]
		[AssociationId("50a13e06-7df7-4d56-b498-5eea8415bb48")]
		[RoleId("e6b86e57-d8d2-41aa-b238-2fe027d74813")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Actives")]
		public RelationType Active;

		#region Allors
		[Id("babffef0-47ad-44ad-9a55-ffefb0fec783")]
		[AssociationId("b490215a-8185-40c8-bb31-087906d10911")]
		[RoleId("9fdaab7a-5e4a-4ec1-85bb-0610c0d0493b")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("PeriodNumbers")]
		public RelationType PeriodNumber;

		#region Allors
		[Id("d776c4f4-9408-4083-8eb4-a4f940f6066f")]
		[AssociationId("8789a4bf-fd21-48d1-ae0b-26ebd100c0ea")]
		[RoleId("98fec7aa-6357-4b8e-baf6-0a8ef3d221dc")]
		#endregion
		[Indexed]
		[Type(typeof(TimeFrequencyClass))]
		[Plural("TimeFrequencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TimeFrequency;



		public static AccountingPeriodClass Instance {get; internal set;}

		internal AccountingPeriodClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Active.RoleType.IsRequired = true;
            this.PeriodNumber.RoleType.IsRequired = true;
            this.TimeFrequency.RoleType.IsRequired = true;
        }
    }
}