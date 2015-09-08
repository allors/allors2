namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("0aaee7b0-0a32-4d0b-a3ed-a448608fe935")]
	#endregion
	[Plural("OrganisationClassifications")]
	[Inherit(typeof(PartyClassificationInterface))]

  	public partial class OrganisationClassificationInterface: Interface
	{

		public static OrganisationClassificationInterface Instance {get; internal set;}

		internal OrganisationClassificationInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}