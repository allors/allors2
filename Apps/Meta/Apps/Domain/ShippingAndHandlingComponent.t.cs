namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("1a174f59-c8cd-49ad-b0f4-a561cdcdcfb2")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("ShippingAndHandlingComponents")]
	public partial class ShippingAndHandlingComponentClass : Class
	{
		#region Allors
		[Id("0021e1ff-bfc3-4d0b-8168-a8f5789121f7")]
		[AssociationId("f1c6cb2b-7c7a-4ca5-b594-152238131cb2")]
		[RoleId("09d4c34a-b5b8-490c-85e3-00470bb8270e")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Costs")]
		public RelationType Cost;

		#region Allors
		[Id("4dfb4bda-1add-45d5-92c7-6393186301f0")]
		[AssociationId("44088ee8-b84a-494c-a59c-3164c511176c")]
		[RoleId("eac922da-3beb-41b2-a3ca-f91120f927bd")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentMethodClass))]
		[Plural("ShipmentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentMethod;

		#region Allors
		[Id("a029fb4c-4f80-4216-8fc9-9d9b44997816")]
		[AssociationId("9e7b4c12-5168-4fe3-adaf-f8d14f7be01f")]
		[RoleId("b0e26bbb-aef7-40ca-9a64-d78bc02affb9")]
		#endregion
		[Indexed]
		[Type(typeof(CarrierClass))]
		[Plural("Carriers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Carrier;

		#region Allors
		[Id("ab4377d4-69c6-4b0c-b9d4-e3a01c1a6a94")]
		[AssociationId("2ad57105-a5c4-4e7f-a6df-79af9cddf9ca")]
		[RoleId("742dcf46-5fa5-44b4-bf02-582681b0f6aa")]
		#endregion
		[Indexed]
		[Type(typeof(ShipmentValueClass))]
		[Plural("ShipmentValues")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipmentValue;

		#region Allors
		[Id("df4727ab-29a8-448c-97b4-c16033e03dcf")]
		[AssociationId("a57b1bd3-a060-41c1-91bd-ecc428dd9b55")]
		[RoleId("a554ced2-84e0-41bb-97d9-d0b04ef56679")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("eb0b4419-e09c-43c6-a221-27e54872026e")]
		[AssociationId("699e4c3c-5cb2-44ab-8e8c-187d0006e4e9")]
		[RoleId("6c6e0c2f-6475-415d-b916-c4489f7f4fc5")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("SpecifiedFors")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SpecifiedFor;

		#region Allors
		[Id("f2bfd9d5-01b2-4bec-8dc2-018cc2187037")]
		[AssociationId("cf282301-2e6c-43ed-8447-cc09edcb9810")]
		[RoleId("a3ee85b7-be6a-4e2b-a15d-57872bb57783")]
		#endregion
		[Indexed]
		[Type(typeof(GeographicBoundaryInterface))]
		[Plural("GeographicBoundaries")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeographicBoundary;



		public static ShippingAndHandlingComponentClass Instance {get; internal set;}

		internal ShippingAndHandlingComponentClass() : base(MetaPopulation.Instance)
        {
        }
	}
}