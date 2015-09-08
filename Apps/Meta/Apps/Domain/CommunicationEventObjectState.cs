namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f6ad2546-e977-4176-b03d-d30fb101270c")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("CommunicationEventObjectStates")]
	public partial class CommunicationEventObjectStateClass : Class
	{

		public static CommunicationEventObjectStateClass Instance {get; internal set;}

		internal CommunicationEventObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}