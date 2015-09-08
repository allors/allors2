namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9492bd39-0f07-4978-a987-0393ca34b504")]
	#endregion
	[Inherit(typeof(FinancialAccountInterface))]
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("CreditCards")]
	public partial class CreditCardClass : Class
	{
		#region Allors
		[Id("07d663c5-4716-4e76-a280-ec635216791f")]
		[AssociationId("e8db5958-e57e-4860-adc9-831c4e513c41")]
		[RoleId("73942abf-a46a-4be4-868b-7c5d195504aa")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("NamesOnCard")]
		public RelationType NameOnCard;

		#region Allors
		[Id("0916d4d2-5f82-46da-967e-7b48012e4019")]
		[AssociationId("21cc3945-4cc1-43c7-a0a3-0fc9af562c5a")]
		[RoleId("bfba8caa-0f75-4e18-8d97-9427e3b5df97")]
		#endregion
		[Indexed]
		[Type(typeof(CreditCardCompanyClass))]
		[Plural("CreditCardCompanies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CreditCardCompany;

		#region Allors
		[Id("4dfa0fda-0001-4635-b8d1-4fd4ce723ed2")]
		[AssociationId("d7ac25b9-d7ec-4f88-82c2-680422891bd7")]
		[RoleId("0ad6e1c1-845d-446a-8557-342c95eee357")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("ExpirationYears")]
		public RelationType ExpirationYear;

		#region Allors
		[Id("7fa0d04e-b2df-49f8-8aa2-2d546ca843d6")]
		[AssociationId("adee3f7d-ded7-469b-9f43-6ed23f3893de")]
		[RoleId("d8e31f7d-a381-438d-9d16-0fff5ab60139")]
		#endregion
		[Type(typeof(AllorsIntegerUnit))]
		[Plural("ExpirationMonths")]
		public RelationType ExpirationMonth;

		#region Allors
		[Id("b5484c11-52d4-45f7-b25a-bf4c05e2c9a0")]
		[AssociationId("15df289b-6c03-4fc4-8d8b-31edc394de8d")]
		[RoleId("683f29d5-cc38-4165-8fd9-f97483130bac")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("CardNumbers")]
		public RelationType CardNumber;



		public static CreditCardClass Instance {get; internal set;}

		internal CreditCardClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CardNumber.RoleType.IsRequired = true;
            this.CardNumber.RoleType.IsUnique = true;
            this.CreditCardCompany.RoleType.IsRequired = true;
            this.ExpirationMonth.RoleType.IsRequired = true;
            this.ExpirationYear.RoleType.IsRequired = true;
            this.NameOnCard.RoleType.IsRequired = true;
        }
    }
}