namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("93e3b3df-b227-479a-9b05-ec10190e7d51")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("HazardousMaterialsDocuments")]
	public partial class HazardousMaterialsDocumentClass : Class
	{

		public static HazardousMaterialsDocumentClass Instance {get; internal set;}

		internal HazardousMaterialsDocumentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}