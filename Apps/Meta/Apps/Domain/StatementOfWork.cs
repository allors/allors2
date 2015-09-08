namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5459f555-cf6a-49c1-8015-b43cad74da17")]
	#endregion
	[Inherit(typeof(QuoteInterface))]

	[Plural("StatementsOfWork")]
	public partial class StatementOfWorkClass : Class
	{

		public static StatementOfWorkClass Instance {get; internal set;}

		internal StatementOfWorkClass() : base(MetaPopulation.Instance)
        {
        }
	}
}