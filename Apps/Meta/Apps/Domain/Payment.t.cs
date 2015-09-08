namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("9f20a35c-d814-4690-a96f-2bcd25f6c6a2")]
	#endregion
	[Plural("Payments")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]

  	public partial class PaymentInterface: Interface
	{
		#region Allors
		[Id("4c8b7a4f-f151-419e-8365-ce0da0b3a709")]
		[AssociationId("32007a7b-e849-41c3-96f5-61d253607f98")]
		[RoleId("5d06b93d-b58e-48ba-b1b8-215b2e84bf4d")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		#region Allors
		[Id("5be2e66e-4714-4dc1-a0f2-a9f600815e41")]
		[AssociationId("321a8622-1b74-4b48-bfe2-7a9478879f06")]
		[RoleId("e877dc1e-18ba-4286-888f-831e0433544d")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentMethodInterface))]
		[Plural("PaymentMethods")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PaymentMethod;

		#region Allors
		[Id("7afc9649-43c9-4a60-a298-27361ba59765")]
		[AssociationId("41547bdb-9d10-42fb-a75f-b0c8d9b8d09e")]
		[RoleId("6038fc56-abb9-41e6-965c-d71648d9f5ce")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("EffectiveDates")]
		public RelationType EffectiveDate;

		#region Allors
		[Id("a80a1ed7-473b-493b-a9ab-23a682c6ae44")]
		[AssociationId("3e95c4c2-6164-486a-a483-e0552a142e13")]
		[RoleId("495d6adc-8fff-4754-99cf-b4f2a65e6b44")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("SendingParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SendingParty;

		#region Allors
		[Id("b0c79092-c5d0-426b-b06d-ccec574bb7d9")]
		[AssociationId("d85e6a8c-5fa9-455d-bc94-6d02b47e7cd8")]
		[RoleId("967768b1-8ec5-4b58-8e8c-2513e5528bb2")]
		#endregion
		[Indexed]
		[Type(typeof(PaymentApplicationClass))]
		[Plural("PaymentApplications")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PaymentApplication;

		#region Allors
		[Id("f49e4d28-12a9-4575-818b-b475bec0c9d1")]
		[AssociationId("9760d670-085b-4573-85c2-96356d362d4e")]
		[RoleId("779b1e2c-4be5-4324-b141-192cca8a7b56")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("ReferenceNumbers")]
		public RelationType ReferenceNumber;

		#region Allors
		[Id("faafa75e-496c-4220-ae3f-ab7d1e317484")]
		[AssociationId("c5800c84-707e-443c-a8ae-bc4e5598bc08")]
		[RoleId("46c1c87e-c4af-4383-814d-5452b2faae94")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ReceivingParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ReceivingParty;



		public static PaymentInterface Instance {get; internal set;}

		internal PaymentInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}