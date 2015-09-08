namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c0e14757-9825-4a86-95d9-b87c68efcb9c")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("OrganisationUnits")]
	public partial class OrganisationUnitClass : Class
	{

		public static OrganisationUnitClass Instance {get; internal set;}

		internal OrganisationUnitClass() : base(MetaPopulation.Instance)
        {
        }
	}
}