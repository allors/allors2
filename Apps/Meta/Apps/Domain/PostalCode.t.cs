namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9d0065b8-2760-4ec5-928a-9ebd128bbfdd")]
	#endregion
	[Inherit(typeof(CountryBoundInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(GeographicBoundaryInterface))]

	[Plural("PostalCodes")]
	public partial class PostalCodeClass : Class
	{
		#region Allors
		[Id("20267bfe-b651-4ed7-bd22-f4300022e39c")]
		[AssociationId("48a9b292-452c-48be-9cb3-2b20f23a510e")]
		[RoleId("12e48856-88e9-4e97-aa32-fd532d2f050d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Codes")]
		public RelationType Code;



		public static PostalCodeClass Instance {get; internal set;}

		internal PostalCodeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}