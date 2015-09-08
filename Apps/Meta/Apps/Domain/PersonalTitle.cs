namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1a4166b3-9d9c-427b-a0d8-da53b0e601a2")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PersonalTitles")]
	public partial class PersonalTitleClass : Class
	{

		public static PersonalTitleClass Instance {get; internal set;}

		internal PersonalTitleClass() : base(MetaPopulation.Instance)
        {
        }
	}
}