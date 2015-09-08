namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b5d29ed3-b850-4607-9f49-9a920a2bffa1")]
	#endregion
	[Inherit(typeof(ContainerInterface))]

	[Plural("Bins")]
	public partial class BinClass : Class
	{

		public static BinClass Instance {get; internal set;}

		internal BinClass() : base(MetaPopulation.Instance)
        {
        }
	}
}