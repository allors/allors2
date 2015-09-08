namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ebf12996-6fc7-4f22-b9be-cedaaba7bfb2")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("TemplatePurposes")]
	public partial class TemplatePurposeClass : Class
	{

		public static TemplatePurposeClass Instance {get; internal set;}

		internal TemplatePurposeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}