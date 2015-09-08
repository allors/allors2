namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("acbe7b46-bcfe-4e8b-b8a7-7b9eeac4d6e2")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("EuSalesListTypes")]
	public partial class EuSalesListTypeClass : Class
	{

		public static EuSalesListTypeClass Instance {get; internal set;}

		internal EuSalesListTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}