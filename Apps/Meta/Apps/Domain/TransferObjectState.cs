namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9f3d9ae6-cbbf-4cfb-900d-bc66edccbf95")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("TransferObjectStates")]
	public partial class TransferObjectStateClass : Class
	{

		public static TransferObjectStateClass Instance {get; internal set;}

		internal TransferObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}