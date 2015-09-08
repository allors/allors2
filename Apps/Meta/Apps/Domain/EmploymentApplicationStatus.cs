namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c7c24ce4-3455-4cec-a733-64a436434b3e")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("EmploymentApplicationStatuses")]
	public partial class EmploymentApplicationStatusClass : Class
	{

		public static EmploymentApplicationStatusClass Instance {get; internal set;}

		internal EmploymentApplicationStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}