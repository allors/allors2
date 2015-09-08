namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("a3f63642-b397-4281-ba7e-8c77e9f30658")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VatTariffs")]
	public partial class VatTariffClass : Class
	{

		public static VatTariffClass Instance {get; internal set;}

		internal VatTariffClass() : base(MetaPopulation.Instance)
        {
        }
	}
}