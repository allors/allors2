namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9a1d67c5-159c-41e0-9b5c-5ffdfe257b8d")]
	#endregion
	[Inherit(typeof(ContainerInterface))]

	[Plural("Shelfs")]
	public partial class ShelfClass : Class
	{

		public static ShelfClass Instance {get; internal set;}

		internal ShelfClass() : base(MetaPopulation.Instance)
        {
        }
	}
}