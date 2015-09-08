namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("97e31ffb-b478-4599-a145-54880d4ffbe1")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PartyRelationshipStatuses")]
	public partial class PartyRelationshipStatusClass : Class
	{

		public static PartyRelationshipStatusClass Instance {get; internal set;}

		internal PartyRelationshipStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}