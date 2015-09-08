namespace Allors.Meta
{
	#region Allors
	[Id("f15e6b0e-0222-4f9b-8ae2-20c20f3b3673")]
	#endregion
	[Inherit(typeof(ServiceEntryInterface))]
	[Plural("ExpenseEntries")]
	public partial class ExpenseEntryClass : Class
	{
		#region Allors
		[Id("0bb04781-d5b4-455c-8880-b5bfbc9d69f8")]
		[AssociationId("cc956cd1-4910-4977-afc5-e76f8bb2dc16")]
		[RoleId("821a410a-afa4-4e6e-b505-3732a864554a")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		public RelationType Amount;

		public static ExpenseEntryClass Instance {get; internal set;}

		internal ExpenseEntryClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[ServiceEntryInterface.Instance.Description.RoleType].IsRequiredOverride = true;
        }
    }
}