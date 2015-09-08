namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("68a0c645-4671-4dda-87a5-53395934a9fc")]
	#endregion
	[Inherit(typeof(PartBillOfMaterialInterface))]

	[Plural("ManufacturingBoms")]
	public partial class ManufacturingBomClass : Class
	{

		public static ManufacturingBomClass Instance {get; internal set;}

		internal ManufacturingBomClass() : base(MetaPopulation.Instance)
        {
        }
	}
}