namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9b6bf786-1c6c-4c4e-b940-7314d9c4ba71")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("BudgetRevisions")]
	public partial class BudgetRevisionClass : Class
	{
		#region Allors
		[Id("5124634a-dc8b-477a-8ae2-d4ad577a13bb")]
		[AssociationId("fa00944b-f6a3-4c61-9739-6a8a109d32d5")]
		[RoleId("a1230395-837b-4021-8075-642fdf1d7d2c")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("RevisionDates")]
		public RelationType RevisionDate;



		public static BudgetRevisionClass Instance {get; internal set;}

		internal BudgetRevisionClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.RevisionDate.RoleType.IsRequired = true;
        }
    }
}