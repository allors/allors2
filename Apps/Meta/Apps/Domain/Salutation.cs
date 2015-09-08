namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("91d1ad08-2eae-4d9e-8a2e-223eeae138af")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(EnumerationInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

	[Plural("Salutations")]
	public partial class SalutationClass : Class
	{

		public static SalutationClass Instance {get; internal set;}

		internal SalutationClass() : base(MetaPopulation.Instance)
        {
        }
	}
}