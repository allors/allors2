namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("dc8ce136-7088-4128-8f69-4d5cb2ca2648")]
	#endregion
	[Inherit(typeof(PartSpecificationInterface))]

	[Plural("ConstraintSpecifications")]
	public partial class ConstraintSpecificationClass : Class
	{

		public static ConstraintSpecificationClass Instance {get; internal set;}

		internal ConstraintSpecificationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}