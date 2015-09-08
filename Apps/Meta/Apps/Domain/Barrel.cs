namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4cd7ab57-544c-4900-a854-4aa9c5284b81")]
	#endregion
	[Inherit(typeof(ContainerInterface))]

	[Plural("Barrels")]
	public partial class BarrelClass : Class
	{

		public static BarrelClass Instance {get; internal set;}

		internal BarrelClass() : base(MetaPopulation.Instance)
        {
        }
	}
}