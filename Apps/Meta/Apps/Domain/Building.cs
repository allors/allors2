namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("2ba5e05c-f1ab-4143-ae7e-4db7389ff34e")]
	#endregion
	[Inherit(typeof(FacilityInterface))]

	[Plural("Buildings")]
	public partial class BuildingClass : Class
	{

		public static BuildingClass Instance {get; internal set;}

		internal BuildingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}