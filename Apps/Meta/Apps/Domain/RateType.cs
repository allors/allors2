namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("096448e3-991d-481e-b323-39064387141c")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("RateTypes")]
	public partial class RateTypeClass : Class
	{

		public static RateTypeClass Instance {get; internal set;}

		internal RateTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}