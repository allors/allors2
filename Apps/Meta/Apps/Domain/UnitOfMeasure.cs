namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("5cd7ea86-8bc6-4b72-a8f6-788e6453acdc")]
	#endregion
	[Inherit(typeof(IUnitOfMeasureInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(EnumerationInterface))]

	[Plural("UnitsOfMeasure")]
	public partial class UnitOfMeasureClass : Class
	{

		public static UnitOfMeasureClass Instance {get; internal set;}

		internal UnitOfMeasureClass() : base(MetaPopulation.Instance)
        {
        }
	}
}