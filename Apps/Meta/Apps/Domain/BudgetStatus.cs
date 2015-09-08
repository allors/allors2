namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4c163351-b42e-4bd3-8cbf-db110eba05fc")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("BudgetStatuses")]
	public partial class BudgetStatusClass : Class
	{
		#region Allors
		[Id("070418ab-f9aa-4286-9395-879b06cf832a")]
		[AssociationId("ee3be6af-f2b5-411a-a07b-24eb676bd923")]
		[RoleId("ceee8ab2-a8da-45d8-be09-61e353e8b1a3")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("StartDateTimes")]
		public RelationType StartDateTime;

		#region Allors
		[Id("125c0c29-4f69-4e0b-b885-76c1e908737e")]
		[AssociationId("f5e1e19d-2c13-4163-b796-a8e0b7a80fcc")]
		[RoleId("554bd320-adce-40ac-83b0-5710e69a0b25")]
		#endregion
		[Indexed]
		[Type(typeof(BudgetObjectStateClass))]
		[Plural("BudgetObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BudgetObjectState;



		public static BudgetStatusClass Instance {get; internal set;}

		internal BudgetStatusClass() : base(MetaPopulation.Instance)
        {
        }
        internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.BudgetObjectState.RoleType.IsRequired = true;
        }
    }
}