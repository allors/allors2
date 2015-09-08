namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("f7b039f4-10f4-4939-8059-5f190fd13b09")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("EmploymentTerminationReasons")]
	public partial class EmploymentTerminationReasonClass : Class
	{

		public static EmploymentTerminationReasonClass Instance {get; internal set;}

		internal EmploymentTerminationReasonClass() : base(MetaPopulation.Instance)
        {
        }
	}
}