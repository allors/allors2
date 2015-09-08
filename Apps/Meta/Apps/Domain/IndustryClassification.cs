namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("da27b432-85e4-4c83-bdb0-64cefb347e8a")]
	#endregion
	[Inherit(typeof(OrganisationClassificationInterface))]

	[Plural("IndustryClassifications")]
	public partial class IndustryClassificationClass : Class
	{

		public static IndustryClassificationClass Instance {get; internal set;}

		internal IndustryClassificationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}