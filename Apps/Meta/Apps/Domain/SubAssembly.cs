namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("b1a10fe4-2d84-452b-b0cb-e96e55014856")]
	#endregion
	[Inherit(typeof(PartInterface))]

	[Plural("SubAssemblies")]
	public partial class SubAssemblyClass : Class
	{

		public static SubAssemblyClass Instance {get; internal set;}

		internal SubAssemblyClass() : base(MetaPopulation.Instance)
        {
        }
	}
}