namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9dd17a3f-0e3c-4d87-b840-2f23a96dd165")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("NonSerializedInventoryItemObjectStates")]
	public partial class NonSerializedInventoryItemObjectStateClass : Class
	{

		public static NonSerializedInventoryItemObjectStateClass Instance {get; internal set;}

		internal NonSerializedInventoryItemObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}