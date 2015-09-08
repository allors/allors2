namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("efb6f7a2-edec-40dd-a03a-d4e15abc438d")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("Manifests")]
	public partial class ManifestClass : Class
	{

		public static ManifestClass Instance {get; internal set;}

		internal ManifestClass() : base(MetaPopulation.Instance)
        {
        }
	}
}