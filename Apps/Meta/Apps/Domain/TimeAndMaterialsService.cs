namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("da504b46-2fd0-4500-ae23-61fa73151077")]
	#endregion
	[Inherit(typeof(ServiceInterface))]

	[Plural("TimeAndMaterialsServices")]
	public partial class TimeAndMaterialsServiceClass : Class
	{

		public static TimeAndMaterialsServiceClass Instance {get; internal set;}

		internal TimeAndMaterialsServiceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}