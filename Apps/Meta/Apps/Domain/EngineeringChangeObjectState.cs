namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("e3f78cf6-6367-4b0f-9ac0-b887e7187c5e")]
	#endregion
	[Inherit(typeof(ObjectStateInterface))]

	[Plural("EngineeringChangeObjectStates")]
	public partial class EngineeringChangeObjectStateClass : Class
	{

		public static EngineeringChangeObjectStateClass Instance {get; internal set;}

		internal EngineeringChangeObjectStateClass() : base(MetaPopulation.Instance)
        {
        }
	}
}