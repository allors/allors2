namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9a484067-2003-42f1-b4c4-877e519bb8be")]
	#endregion
	[Inherit(typeof(PartInterface))]

	[Plural("RawMaterials")]
	public partial class RawMaterialClass : Class
	{

		public static RawMaterialClass Instance {get; internal set;}

		internal RawMaterialClass() : base(MetaPopulation.Instance)
        {
        }
	}
}