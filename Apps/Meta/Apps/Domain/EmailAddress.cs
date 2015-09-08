namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f4b7ea51-eac4-479b-92e8-5109cfeceb77")]
	#endregion
	[Inherit(typeof(ElectronicAddressInterface))]

	[Plural("EmailAddresses")]
	public partial class EmailAddressClass : Class
	{

		public static EmailAddressClass Instance {get; internal set;}

		internal EmailAddressClass() : base(MetaPopulation.Instance)
        {
        }
	}
}