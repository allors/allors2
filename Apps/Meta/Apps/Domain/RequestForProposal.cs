namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0112ddd0-14de-43e2-97d3-981766dd957e")]
	#endregion
	[Inherit(typeof(RequestInterface))]

	[Plural("RequestsForProposal")]
	public partial class RequestForProposalClass : Class
	{

		public static RequestForProposalClass Instance {get; internal set;}

		internal RequestForProposalClass() : base(MetaPopulation.Instance)
        {
        }
	}
}