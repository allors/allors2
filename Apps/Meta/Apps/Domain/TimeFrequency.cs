namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1aba0c3c-2a1c-414d-86df-5a9b8c672587")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]
	[Inherit(typeof(IUnitOfMeasureInterface))]

	[Plural("TimeFrequencies")]
	public partial class TimeFrequencyClass : Class
	{

		public static TimeFrequencyClass Instance {get; internal set;}

		internal TimeFrequencyClass() : base(MetaPopulation.Instance)
        {
        }
	}
}