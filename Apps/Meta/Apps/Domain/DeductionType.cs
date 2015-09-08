namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("66b30b62-5e6c-4747-a72e-bc4ac2cb1125")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("DeductionTypes")]
	public partial class DeductionTypeClass : Class
	{

		public static DeductionTypeClass Instance {get; internal set;}

		internal DeductionTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}