namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c2214ff4-d592-4f0d-9215-e431b23dc9c2")]
	#endregion
	[Inherit(typeof(QuoteInterface))]

	[Plural("ProductQuotes")]
	public partial class ProductQuoteClass : Class
	{

		public static ProductQuoteClass Instance {get; internal set;}

		internal ProductQuoteClass() : base(MetaPopulation.Instance)
        {
        }
	}
}