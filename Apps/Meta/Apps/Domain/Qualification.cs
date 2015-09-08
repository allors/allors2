namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("c8077ff8-f443-44b5-93f5-15ad7f4a258d")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("Qualifications")]
	public partial class QualificationClass : Class
	{

		public static QualificationClass Instance {get; internal set;}

		internal QualificationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}