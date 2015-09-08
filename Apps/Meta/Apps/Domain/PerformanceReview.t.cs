namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("89c49578-bb5d-4589-b908-bf09c6495011")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(CommentableInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("PerformanceReviews")]
	public partial class PerformanceReviewClass : Class
	{
		#region Allors
		[Id("22ec2f64-1099-49aa-908b-abb2703ccf33")]
		[AssociationId("2b66e451-52c1-4e83-97e0-a59784862660")]
		[RoleId("d5a94f8a-e657-406a-a9ff-64fec9e5b67c")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Managers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Manager;

		#region Allors
		[Id("3704d6ac-52c1-4af0-ad6e-151defc2fa05")]
		[AssociationId("840545c0-3f1e-44e0-96cd-48a0dc34e937")]
		[RoleId("dbd0ecc2-ba54-45d5-a4c8-7c3476e64ce1")]
		#endregion
		[Indexed]
		[Type(typeof(PayHistoryClass))]
		[Plural("PayHistories")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PayHistory;

		#region Allors
		[Id("a16503ae-6371-4e97-9d34-f21a0f52002f")]
		[AssociationId("7002201c-53f7-457f-8c8c-4990fc4ed175")]
		[RoleId("220d3993-fbca-4082-887c-ab7e9261d4da")]
		#endregion
		[Indexed]
		[Type(typeof(PayCheckClass))]
		[Plural("BonusPayChecks")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType BonusPayCheck;

		#region Allors
		[Id("a5057413-950e-4825-8036-7f398c4b5d39")]
		[AssociationId("86796848-4a49-43c1-879e-1e77063af4e0")]
		[RoleId("7bbb3e7e-c3a0-4b63-84e7-4bb923425ec1")]
		#endregion
		[Indexed]
		[Type(typeof(PerformanceReviewItemClass))]
		[Plural("PerformanceReviewItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PerformanceReviewItem;

		#region Allors
		[Id("ddeb9c39-9bfc-437d-8f5a-434028d8ad6f")]
		[AssociationId("1e857746-32cb-44af-9e05-3fb7568def9a")]
		[RoleId("77390fa9-3f73-41f8-8adc-558c7839400e")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Employees")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employee;

		#region Allors
		[Id("f3210e4a-a8ee-442c-85a5-34290deffe2a")]
		[AssociationId("b91a9331-cc16-401f-9ee7-d697389431f7")]
		[RoleId("9aeadbaf-24ad-4ced-96a0-1f4ee2ea0859")]
		#endregion
		[Indexed]
		[Type(typeof(PositionClass))]
		[Plural("ResultingPositions")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ResultingPosition;



		public static PerformanceReviewClass Instance {get; internal set;}

		internal PerformanceReviewClass() : base(MetaPopulation.Instance)
        {
        }
	}
}