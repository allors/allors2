namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ef6ce14d-87e9-4704-be8b-595329a6bf20")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("MaritalStatuses")]
	public partial class MaritalStatusClass : Class
	{

		public static MaritalStatusClass Instance {get; internal set;}

		internal MaritalStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}