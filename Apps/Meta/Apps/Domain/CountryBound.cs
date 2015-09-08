namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("eaebcfe7-0d65-43ab-857c-b171086a1982")]
	#endregion
	[Plural("CountryBounds")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class CountryBoundInterface: Interface
	{
		#region Allors
		[Id("095460a7-fffa-4c94-8b51-a4fd9fb80a2e")]
		[AssociationId("f5aa22da-64f3-447a-864c-4db5b77d221b")]
		[RoleId("799ab886-ce30-4270-8293-6c302d17e3e3")]
		#endregion
		[Indexed]
		[Type(typeof(CountryClass))]
		[Plural("Countries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Country;



		public static CountryBoundInterface Instance {get; internal set;}

		internal CountryBoundInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}