namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("7c0d1b2d-88bf-41dd-b19d-b6b0ed1cb179")]
	#endregion
	[Inherit(typeof(FacilityInterface))]

	[Plural("Floors")]
	public partial class FloorClass : Class
	{

		public static FloorClass Instance {get; internal set;}

		internal FloorClass() : base(MetaPopulation.Instance)
        {
        }
	}
}