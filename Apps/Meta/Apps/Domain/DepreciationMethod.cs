namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("63ca0535-95e5-4b2d-847d-d619a5365605")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("DepreciationMethods")]
	public partial class DepreciationMethodClass : Class
	{
		#region Allors
		[Id("a87fd42b-7be3-4cd4-9393-64b1cf03c050")]
		[AssociationId("9957bc91-53a9-431c-8eea-2e0dc04adde7")]
		[RoleId("67ecfd2b-4fc4-4474-81f8-cb8b720b30c4")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Formulas")]
		public RelationType Formula;

		#region Allors
		[Id("b0a81d90-f6bc-4169-b76c-497a3a1f04bf")]
		[AssociationId("6af9db7e-6d96-4b91-9a7f-0f1005e49f65")]
		[RoleId("2d1ef7fc-bd11-4380-a917-a29fa14fa89d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;



		public static DepreciationMethodClass Instance {get; internal set;}

		internal DepreciationMethodClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Formula.RoleType.IsRequired = true;
        }
    }
}