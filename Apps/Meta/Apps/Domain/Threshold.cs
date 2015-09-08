namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c7b56330-1fb6-46c7-a042-04a4cf671ec1")]
	#endregion
	[Inherit(typeof(AgreementTermInterface))]

	[Plural("Thresholds")]
	public partial class ThresholdClass : Class
	{

		public static ThresholdClass Instance {get; internal set;}

		internal ThresholdClass() : base(MetaPopulation.Instance)
        {
        }
	}
}