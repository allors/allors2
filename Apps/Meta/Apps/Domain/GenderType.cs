namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f35745a9-a8d3-4002-a484-6f0fb00a69a2")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("GenderTypes")]
	public partial class GenderTypeClass : Class
	{

		public static GenderTypeClass Instance {get; internal set;}

		internal GenderTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}