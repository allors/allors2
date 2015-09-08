namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("3b330b42-b359-4de7-a084-cc96ce1e6420")]
	#endregion
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(EnumerationInterface))]

	[Plural("DebitCreditConstants")]
	public partial class DebitCreditConstantClass : Class
	{

		public static DebitCreditConstantClass Instance {get; internal set;}

		internal DebitCreditConstantClass() : base(MetaPopulation.Instance)
        {
        }
	}
}