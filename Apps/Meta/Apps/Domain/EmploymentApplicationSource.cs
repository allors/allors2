namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("74cd22cf-1796-4c65-85df-9c3e09883843")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("EmploymentApplicationSources")]
	public partial class EmploymentApplicationSourceClass : Class
	{

		public static EmploymentApplicationSourceClass Instance {get; internal set;}

		internal EmploymentApplicationSourceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}