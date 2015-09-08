namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ded168ad-b674-47ab-855c-46b3e1939e32")]
	#endregion
	[Inherit(typeof(DeploymentUsageInterface))]

	[Plural("ActivityUsages")]
	public partial class ActivityUsageClass : Class
	{
		#region Allors
		[Id("1c8929c2-090a-41f2-8a22-691a63df4ff7")]
		[AssociationId("ab9d6daf-e245-4281-9ff0-fb865c275f79")]
		[RoleId("9acc53b1-4e7a-46c7-a34a-158f5eb05d07")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Quantities")]
		public RelationType Quantity;

		#region Allors
		[Id("b7672e5b-5ddc-46ba-82f2-4804f8b43ebf")]
		[AssociationId("3c0cd8a9-c033-4ff1-9ff5-60e90cfffdf5")]
		[RoleId("ed7d8046-4596-4055-af88-b2e4c9da6898")]
		#endregion
		[Indexed]
		[Type(typeof(UnitOfMeasureClass))]
		[Plural("UnitsOfMeasure")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType UnitOfMeasure;



		public static ActivityUsageClass Instance {get; internal set;}

		internal ActivityUsageClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.UnitOfMeasure.RoleType.IsRequired = true;
        }
    }
}