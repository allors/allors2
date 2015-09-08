namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("17d7e31c-9b12-4e0b-a3a7-e687e3991e23")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("RatingTypes")]
	public partial class RatingTypeClass : Class
	{

		public static RatingTypeClass Instance {get; internal set;}

		internal RatingTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}