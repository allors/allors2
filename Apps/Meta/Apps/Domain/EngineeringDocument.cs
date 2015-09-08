namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8da5bb9b-593b-4c10-91c2-1e9cc2c226d1")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("EngineeringDocuments")]
	public partial class EngineeringDocumentClass : Class
	{

		public static EngineeringDocumentClass Instance {get; internal set;}

		internal EngineeringDocumentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}