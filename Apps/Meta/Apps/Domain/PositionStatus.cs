namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4250a005-4fec-4118-a5b4-725886c59269")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("PositionStatuses")]
	public partial class PositionStatusClass : Class
	{

		public static PositionStatusClass Instance {get; internal set;}

		internal PositionStatusClass() : base(MetaPopulation.Instance)
        {
        }
	}
}