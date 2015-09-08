namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("6360b45d-3556-41c6-b183-f42a15b9424f")]
	#endregion
	[Inherit(typeof(ServiceEntryInterface))]

	[Plural("TimeEntries")]
	public partial class TimeEntryClass : Class
	{
		#region Allors
		[Id("1b07c419-42af-480b-87ba-1c001995dc51")]
		[AssociationId("2c605991-8d65-4b8f-9daf-e085af5b12c0")]
		[RoleId("90872970-372a-4f8d-9c53-c753aca9f99f")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Costs")]
		public RelationType Cost;

		#region Allors
		[Id("1bb9affa-1390-4f54-92b5-64997e55525e")]
		[AssociationId("0f0341bb-d719-4989-a39b-02b1c1ce98b9")]
		[RoleId("ff8087ac-403d-46e4-b799-316bbdb6616e")]
		#endregion
		[Derived]
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("GrossMargins")]
		public RelationType GrossMargin;

		#region Allors
		[Id("258a33cc-06ea-45a0-9b15-1b6d58385910")]
		[AssociationId("4909a04f-fd14-46ce-9c4c-bc7a2cc21914")]
		[RoleId("cff49ef3-5b51-4501-a5c8-59b4d5714f4e")]
		#endregion
		[Indexed]
		[Type(typeof(QuoteTermClass))]
		[Plural("QuoteTerms")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType QuoteTerm;

		#region Allors
		[Id("2c33de6e-b4fd-47e4-b254-2991f33f01f1")]
		[AssociationId("c8b7e4be-fbc5-414c-8e30-3947925c24b8")]
		[RoleId("1cca252a-d6a1-4945-991a-dd85090bb41d")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("BillingRates")]
		public RelationType BillingRate;

		#region Allors
		[Id("409ff1fb-1531-4829-9d6b-7b3e7113594a")]
		[AssociationId("54a57392-59ed-4583-99f1-1f2a97ca65c5")]
		[RoleId("724e2645-553a-4810-a62d-4c7595877042")]
		#endregion
		[Indexed]
		[Type(typeof(UnitOfMeasureClass))]
		[Plural("UnitsOfMeasure")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType UnitOfMeasure;

		#region Allors
		[Id("c163457c-6a36-45ab-8c62-e555128afbfc")]
		[AssociationId("01112e75-888e-4dac-93e0-185afe6988af")]
		[RoleId("56c9d8a5-45d0-4bb4-8809-43740938b824")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("AmountsOfTime")]
		public RelationType AmountOfTime;



		public static TimeEntryClass Instance {get; internal set;}

		internal TimeEntryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}