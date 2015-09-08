namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d042eeae-5c17-4936-861b-aaa9dfaed254")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("SerializedInventoryItemObjectStates")]
	public partial class SerializedInventoryItemObjectStateClass : Class
	{

		public static SerializedInventoryItemObjectStateClass Instance {get; internal set;}

		internal SerializedInventoryItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}