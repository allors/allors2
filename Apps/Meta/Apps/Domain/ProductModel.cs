namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("99ea8125-7d86-4cb6-b453-27752c434fc7")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("ProductModels")]
	public partial class ProductModelClass : Class
	{

		public static ProductModelClass Instance {get; internal set;}

		internal ProductModelClass() : base(MetaPopulation.Instance)
        {
        }
	}
}