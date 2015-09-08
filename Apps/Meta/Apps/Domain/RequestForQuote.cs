namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("874dfe70-2e50-4861-b26d-dc55bc8fa0d0")]
	#endregion
	[Inherit(typeof(RequestInterface))]

	[Plural("RequestsForQuote")]
	public partial class RequestForQuoteClass : Class
	{

		public static RequestForQuoteClass Instance {get; internal set;}

		internal RequestForQuoteClass() : base(MetaPopulation.Instance)
        {
        }
	}
}