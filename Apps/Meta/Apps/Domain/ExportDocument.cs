namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("efe15d5d-f07c-497e-98c2-dd64f624840f")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("ExportDocuments")]
	public partial class ExportDocumentClass : Class
	{

		public static ExportDocumentClass Instance {get; internal set;}

		internal ExportDocumentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}