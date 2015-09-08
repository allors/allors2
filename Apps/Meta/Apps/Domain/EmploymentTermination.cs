namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("129e6fe8-01d0-40ad-bc6a-e5449c19274f")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("EmploymentTerminations")]
	public partial class EmploymentTerminationClass : Class
	{

		public static EmploymentTerminationClass Instance {get; internal set;}

		internal EmploymentTerminationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}