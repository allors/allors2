namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c4b51143-7e9c-4f1d-a34f-cc99f29a12e9")]
	#endregion
	[Inherit(typeof(PartSpecificationInterface))]

	[Plural("Tolerances")]
	public partial class ToleranceClass : Class
	{

		public static ToleranceClass Instance {get; internal set;}

		internal ToleranceClass() : base(MetaPopulation.Instance)
        {
        }
	}
}