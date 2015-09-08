namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6d4739a9-c3c4-4570-a337-49f667c6243b")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("MarketingMaterials")]
	public partial class MarketingMaterialClass : Class
	{

		public static MarketingMaterialClass Instance {get; internal set;}

		internal MarketingMaterialClass() : base(MetaPopulation.Instance)
        {
        }
	}
}