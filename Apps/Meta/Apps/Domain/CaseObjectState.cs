namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6ea1f500-13a2-4f5a-8026-a1d5a57170ac")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("CaseObjectStates")]
	public partial class CaseObjectStateClass : Class
	{

		public static CaseObjectStateClass Instance {get; internal set;}

		internal CaseObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}