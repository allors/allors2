namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1409bf2c-3ea8-4a62-ac7e-e1e113eacb7a")]
	#endregion
	[Inherit(typeof(PartSpecificationInterface))]

	[Plural("OperatingConditions")]
	public partial class OperatingConditionClass : Class
	{

		public static OperatingConditionClass Instance {get; internal set;}

		internal OperatingConditionClass() : base(MetaPopulation.Instance)
        {
        }
	}
}