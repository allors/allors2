namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f444b4be-1703-49b4-918b-2d1aaf27ce5a")]
	#endregion
	[Inherit(typeof(FacilityInterface))]

	[Plural("Offices")]
	public partial class OfficeClass : Class
	{

		public static OfficeClass Instance {get; internal set;}

		internal OfficeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}