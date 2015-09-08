namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("22bc5b67-8015-49c5-bc47-6f9e7e678943")]
	#endregion
	[Inherit(typeof(FinancialAccountInterface))]

	[Plural("BankAccounts")]
	public partial class BankAccountClass : Class
	{
		#region Allors
		[Id("52677328-d903-4e97-83c1-b55668ced66d")]
		[AssociationId("6895f657-2e32-4a12-af0c-bb2d5d633174")]
		[RoleId("ddf52c63-b6d5-4bae-9d54-f1c71e76c289")]
		#endregion
		[Indexed]
		[Type(typeof(BankClass))]
		[Plural("Banks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Bank;

		#region Allors
		[Id("53bb9d62-a8e5-417c-9392-c54cf99bc24b")]
		[AssociationId("65fc437f-ae06-4d85-a300-3508edeec4c1")]
		[RoleId("2d1f34f5-6a15-4b4d-901e-f8b8dcf1df01")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("NamesOnAccount")]
		public RelationType NameOnAccount;

		#region Allors
		[Id("93447a57-a049-4eaa-98ec-6fec60bdb64c")]
		[AssociationId("68e37671-a29b-44fa-9f19-2efe76a409f3")]
		[RoleId("f22eb146-8e3f-4ea6-85f5-3a2b0d08ecc5")]
		#endregion
		[Indexed]
		[Type(typeof(ContactMechanismInterface))]
		[Plural("ContactMechanisms")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ContactMechanism;

		#region Allors
		[Id("a7d242b4-4d39-4254-beb2-914eb556f7b7")]
		[AssociationId("2911fab2-a04f-4afc-961d-4fac26f01ae3")]
		[RoleId("ab751b97-c9d9-46bc-9209-6a0b3c191ea0")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("Currencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Currency;

		#region Allors
		[Id("ac2d58e5-ad74-4afe-b9f0-aeb9dfdcd4b3")]
		[AssociationId("55e4252b-7543-4384-8fe5-65aff3648744")]
		[RoleId("3783ab93-3ca8-4a04-be01-5831b7f3ab02")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Ibans")]
		public RelationType Iban;

		#region Allors
		[Id("b06a858d-a8ee-41b8-a747-7fd46336ae4f")]
		[AssociationId("00656807-27c8-4803-a1e3-aad812af2f9e")]
		[RoleId("08604620-9d2f-4b98-bba6-16147c0d9978")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Branches")]
		public RelationType Branch;

		#region Allors
		[Id("ecaedf71-98a2-425d-8046-cc8865fdbe73")]
		[AssociationId("9174bacb-3955-462a-a53f-ec251466da1b")]
		[RoleId("7d0da5fc-29a4-4a53-a834-9c58662145d0")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("ContactPersons")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType ContactPerson;



		public static BankAccountClass Instance {get; internal set;}

		internal BankAccountClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Iban.RoleType.IsRequired = true;
            this.Iban.RoleType.IsUnique = true;
        }
    }
}