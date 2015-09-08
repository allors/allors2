namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("360cf15d-c360-4d68-b693-7d1544388169")]
	#endregion
	[Inherit(typeof(QuoteInterface))]

	[Plural("Proposals")]
	public partial class ProposalClass : Class
	{

		public static ProposalClass Instance {get; internal set;}

		internal ProposalClass() : base(MetaPopulation.Instance)
        {
        }
	}
}