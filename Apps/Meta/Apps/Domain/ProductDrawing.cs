namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1fb8d537-a870-4793-95a1-7742749e16fc")]
	#endregion
	[Inherit(typeof(DocumentInterface))]

	[Plural("ProductDrawings")]
	public partial class ProductDrawingClass : Class
	{

		public static ProductDrawingClass Instance {get; internal set;}

		internal ProductDrawingClass() : base(MetaPopulation.Instance)
        {
        }
	}
}