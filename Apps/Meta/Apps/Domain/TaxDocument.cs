namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0d03a71b-c58e-405d-a995-c467a0b25d5b")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("TaxDocuments")]
	public partial class TaxDocumentClass : Class
	{

		public static TaxDocumentClass Instance {get; internal set;}

		internal TaxDocumentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}