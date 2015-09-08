namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f194d2e1-d246-40eb-9eab-70ee2521703a")]
	#endregion
	[Plural("ProductAssociations")]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

  	public partial class ProductAssociationInterface: Interface
	{

		public static ProductAssociationInterface Instance {get; internal set;}

		internal ProductAssociationInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}