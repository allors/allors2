namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8e3fd781-f0b5-4e02-b1f6-6364d0559273")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("CommunicationEventPurposes")]
	public partial class CommunicationEventPurposeClass : Class
	{

		public static CommunicationEventPurposeClass Instance {get; internal set;}

		internal CommunicationEventPurposeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}