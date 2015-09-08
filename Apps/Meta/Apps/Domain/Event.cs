namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("aad26d12-9e80-410c-ab99-57064bd3dd2e")]
	#endregion
	[Plural("Events")]
	public partial class EventClass : Class
	{
		#region Allors
		[Id("189505d9-434f-4d12-a6ab-44edcf44801c")]
		[AssociationId("edd0f108-0d6c-414a-8460-2a6f2e4c8f6b")]
		[RoleId("ea95aeb1-2d78-4d96-b725-cb5bc7268176")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("RegistrationsRequired")]
		public RelationType RegistrationRequired;

		#region Allors
		[Id("1a4f5119-23c5-4cbe-afdb-565c0e8f9e80")]
		[AssociationId("1ba99ae5-2e3b-4e41-ba52-75f724860ee3")]
		[RoleId("1f2c258c-30c3-4e2d-a1ad-263fe0680381")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Links")]
		public RelationType Link;

		#region Allors
		[Id("6eb8fbc4-7fbd-4eb6-8944-01737b1182cc")]
		[AssociationId("a3aa3fe3-8d70-435b-b567-823d4771d3fa")]
		[RoleId("f7d30205-c1fa-4cfa-9194-21301e5812fb")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Locations")]
		public RelationType Location;

		#region Allors
		[Id("78cfaf88-c3c4-41d1-b9f0-f69a82646930")]
		[AssociationId("c23f6022-4df9-46ce-9eed-7dabf1f1f502")]
		[RoleId("2eaa85f3-e70f-4a4c-96a1-64e68457261c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Texts")]
		public RelationType Text;

		#region Allors
		[Id("79b05cf2-2175-4724-acdd-88bc05f15881")]
		[AssociationId("7276942a-8c26-466f-aa32-698454184454")]
		[RoleId("def8b1dc-c837-40a1-bbcc-4bb00b0250e0")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("AnnouncementTexts")]
		public RelationType AnnouncementText;

		#region Allors
		[Id("7a66f2bc-bfb1-420a-a383-acf3092ca48b")]
		[AssociationId("d3943099-a5ec-413b-9079-239c67bdc696")]
		[RoleId("1c1aead6-f157-4d23-a9f0-0565e2b7ff82")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("Froms")]
		public RelationType From;

		#region Allors
		[Id("7d73d60c-bcb2-4be6-bc60-e4420a8d0417")]
		[AssociationId("09cdba21-c34e-465e-847b-8062232c6d85")]
		[RoleId("f80dabde-e6f0-4044-9468-f96641bdd49a")]
		#endregion
		[Indexed]
		[Type(typeof(LocaleClass))]
		[Plural("Locales")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Locale;

		#region Allors
		[Id("b044d498-2995-41d2-8487-0ec323b011bc")]
		[AssociationId("b517e0c7-6b49-4f27-bbb3-3cd291fd14fd")]
		[RoleId("6f371186-d82b-42ac-a7f1-a8382454a332")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Titles")]
		public RelationType Title;

		#region Allors
		[Id("cbc5a9f6-cd08-41aa-a4aa-dac9a8a802ac")]
		[AssociationId("ec42a541-030f-4fbe-9fba-145c8fbc8e87")]
		[RoleId("ecc373d4-636f-4114-8635-55a97e629607")]
		#endregion
		[Indexed]
		[Type(typeof(MediaClass))]
		[Plural("Photos")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Photo;

		#region Allors
		[Id("d9d15920-705f-4ca3-bfa1-47bd5d5b7238")]
		[AssociationId("a5a2ab0f-d7c9-44c7-9fd5-be9cc9ea1666")]
		[RoleId("79827257-f70d-4961-8fa0-4798a4f4a28d")]
		#endregion
		[Type(typeof(AllorsBooleanUnit))]
		[Plural("Announces")]
		public RelationType Announce;

		#region Allors
		[Id("de61dd0d-1f8e-4a55-9fe4-f44cf35b6a31")]
		[AssociationId("90352035-7b90-414f-be38-7f3e4d5fbd95")]
		[RoleId("b8c6fe1f-7c7e-41ae-8f03-32a18e4920e5")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("Tos")]
		public RelationType To;



		public static EventClass Instance {get; internal set;}

		internal EventClass() : base(MetaPopulation.Instance)
        {
        }
	}
}