namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d54b4bba-a84c-4826-85ba-7340714035c7")]
	#endregion
	[Inherit(typeof(ContactMechanismInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(GeoLocatableInterface))]

	[Plural("PostalAddresses")]
	public partial class PostalAddressClass : Class
	{
		#region Allors
		[Id("24216a78-41d8-4ffc-958a-2411530eeb94")]
		[AssociationId("649eb363-210c-4567-be0a-bcd3e666294e")]
		[RoleId("1e9fb472-c39d-4e46-a58c-3cbf2b99c2cd")]
		#endregion
		[Indexed]
		[Type(typeof(GeographicBoundaryInterface))]
		[Plural("GeographicBoundaries")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType GeographicBoundary;

		#region Allors
		[Id("5440794c-8569-46fb-a5cb-42dc523e1264")]
		[AssociationId("1f14c608-3744-4697-a226-443196a57e94")]
		[RoleId("db609423-3100-46a1-890d-0dbef16daf3f")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Addresses3")]
		public RelationType Address3;

		#region Allors
		[Id("5c513411-ca39-4f39-844d-54cf0468a702")]
		[AssociationId("d72a1dc4-a61b-4f91-89ac-29f633b6944b")]
		[RoleId("13e57a25-6265-4a56-a4e7-c914a0c57cb9")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CountryClass))]
		[Plural("Countries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Country;

		#region Allors
		[Id("9475dd68-43f7-4195-bf57-8ce82333980e")]
		[AssociationId("c0a0b7b4-5f1a-4b8b-a858-5de3abc5e66f")]
		[RoleId("58ffb62f-f270-4a64-b2a9-26b2cec8eaee")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Addresses2")]
		public RelationType Address2;

		#region Allors
		[Id("9801fa63-ac82-4774-bf84-d2752b95b8a3")]
		[AssociationId("6eb0ec18-2d30-4529-b741-785aad15842f")]
		[RoleId("a477adeb-04b9-449c-b61c-4a1384fe10aa")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(CityClass))]
		[Plural("Cities")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType City;

		#region Allors
		[Id("c83eb0ff-8503-4f2a-9280-f8e46b382b6a")]
		[AssociationId("2976fdd4-19c4-4913-8875-1bf413da02fd")]
		[RoleId("04c32889-628b-4f9f-8504-65a73268055d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Addresses1")]
		public RelationType Address1;

		#region Allors
		[Id("dfec7833-a7c1-4c27-bbdb-7bc2cc9e8f30")]
		[AssociationId("c206f13e-d4eb-4818-81ef-e134251698cd")]
		[RoleId("f6a78ca4-be61-41ff-803b-05934f8691e7")]
		#endregion
		[Indexed]
		[Type(typeof(PostalBoundaryClass))]
		[Plural("PostalBoundaries")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType PostalBoundary;

		#region Allors
		[Id("e642d557-a842-4357-be4a-ed7da965d592")]
		[AssociationId("ad1d5592-71a7-4410-91e3-f00ea1c29ce1")]
		[RoleId("23f50fbf-71df-4532-9b45-17e5ac3ad7f4")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PostalCodeClass))]
		[Plural("PostalCodes")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PostalCode;

		#region Allors
		[Id("f2ba6a39-2e34-42bc-accb-0d8838311994")]
		[AssociationId("6d2b7cbb-1825-40a9-9000-2854a9cd6a26")]
		[RoleId("53349be7-9048-494f-96e0-367468eb9dad")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Directions")]
		public RelationType Directions;

		#region Allors
		[Id("f97e6fa7-3c30-4ab1-9b9b-d2c9ad257009")]
		[AssociationId("d57e1d69-6f84-4601-8ae8-0807d6bf4d5c")]
		[RoleId("26ca4fa5-70e5-4f38-a731-ca3a34f24a91")]
		#endregion
		[Derived]
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("FormattedFullAddresses")]
		public RelationType FormattedFullAddress;



		public static PostalAddressClass Instance {get; internal set;}

		internal PostalAddressClass() : base(MetaPopulation.Instance)
        {
        }
	}
}