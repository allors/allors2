namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("8ff46109-8ae7-4da5-a1f9-f19d4cf4e27e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VarianceReasons")]
	public partial class VarianceReasonClass : Class
	{

		public static VarianceReasonClass Instance {get; internal set;}

		internal VarianceReasonClass() : base(MetaPopulation.Instance)
        {
        }
	}
}