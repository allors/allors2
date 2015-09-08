namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("14a85148-0d92-4869-8a94-b102f047931f")]
	#endregion
	[Inherit(typeof(PartBillOfMaterialInterface))]

	[Plural("EngineeringBoms")]
	public partial class EngineeringBomClass : Class
	{

		public static EngineeringBomClass Instance {get; internal set;}

		internal EngineeringBomClass() : base(MetaPopulation.Instance)
        {
        }
	}
}