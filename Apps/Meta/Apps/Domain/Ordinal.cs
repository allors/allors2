namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("385a2ae6-368c-4c3f-ad34-f8d69d8ca6cd")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Ordinals")]
	public partial class OrdinalClass : Class
	{

		public static OrdinalClass Instance {get; internal set;}

		internal OrdinalClass() : base(MetaPopulation.Instance)
        {
        }
	}
}