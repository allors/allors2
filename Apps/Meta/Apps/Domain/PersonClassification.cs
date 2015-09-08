namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("77f63100-054a-459b-8864-e69b646ff307")]
	#endregion
	[Plural("PersonClassifications")]
	[Inherit(typeof(PartyClassificationInterface))]

  	public partial class PersonClassificationInterface: Interface
	{

		public static PersonClassificationInterface Instance {get; internal set;}

		internal PersonClassificationInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}