namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("23848955-69ae-40ce-b973-0d416ae80c78")]
	#endregion
	[Inherit(typeof(PaymentMethodInterface))]
	[Inherit(typeof(FinancialAccountInterface))]

	[Plural("OwnCreditCards")]
	public partial class OwnCreditCardClass : Class
	{
		#region Allors
		[Id("7ca9a38c-4318-4bb6-8bc6-50e5dfe9c701")]
		[AssociationId("3dc97f13-b6b7-47eb-ae6c-b57b45a2f129")]
		[RoleId("0bfa9940-e320-4e52-903a-b6765830069a")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Owners")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Owner;

		#region Allors
		[Id("e2514c8b-5980-4e58-a75f-20890ed79516")]
		[AssociationId("2f572644-647a-4d4e-b085-400ba3a88f7a")]
		[RoleId("81d792be-5f29-415e-8290-66b98a95e9e3")]
		#endregion
		[Indexed]
		[Type(typeof(CreditCardClass))]
		[Plural("CreditCards")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CreditCard;



		public static OwnCreditCardClass Instance {get; internal set;}

		internal OwnCreditCardClass() : base(MetaPopulation.Instance)
        {
        }
	}
}