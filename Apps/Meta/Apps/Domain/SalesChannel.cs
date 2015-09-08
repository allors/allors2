namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("db1678af-6541-4a35-a3b9-cffd0f821bd2")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("SalesChannels")]
	public partial class SalesChannelClass : Class
	{

		public static SalesChannelClass Instance {get; internal set;}

		internal SalesChannelClass() : base(MetaPopulation.Instance)
        {
        }
	}
}