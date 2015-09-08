namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5138c0e3-1b28-4297-bf45-697624ee5c19")]
	#endregion
	[Inherit(typeof(ElectronicAddressInterface))]

	[Plural("WebAddresses")]
	public partial class WebAddressClass : Class
	{

		public static WebAddressClass Instance {get; internal set;}

		internal WebAddressClass() : base(MetaPopulation.Instance)
        {
        }
	}
}