namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("066bf242-2710-4a68-8ff6-ce4d7d88a04a")]
	#endregion
	[Plural("Quotes")]
	[Inherit(typeof(AccessControlledObjectInterface))]

  	public partial class QuoteInterface: Interface
	{
		#region Allors
		[Id("033df6dd-fdf7-44e4-84ca-5c7e100cb3f5")]
		[AssociationId("4b19f443-0d27-447d-8186-e5361a094460")]
		[RoleId("fa17ef86-c074-414e-b223-b62522d68280")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ValidFromDates")]
		public RelationType ValidFromDate;

		#region Allors
		[Id("05e3454a-0a7a-488d-b4b1-f0fd41392ddf")]
		[AssociationId("ca3f0d26-9ead-4691-8f7f-f79272065251")]
		[RoleId("92e46228-ad44-4b9b-b727-23159a59bca3")]
		#endregion
		[Indexed]
		[Type(typeof(QuoteTermClass))]
		[Plural("QuoteTerms")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType QuoteTerm;

		#region Allors
		[Id("20880670-0496-4d24-8c97-69b83867c09e")]
		[AssociationId("c2bfd7fd-7956-4c28-960e-539f8159e46a")]
		[RoleId("3242cf4f-589c-457b-9ecd-59110041ab34")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Issuers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Issuer;

		#region Allors
		[Id("2140e106-2ef3-427a-be94-458c2b8e154d")]
		[AssociationId("9d81ada4-a4f3-44bb-9098-bc1a3e61de19")]
		[RoleId("60581583-2536-4b09-acae-f0f877169dae")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("ValidThroughDates")]
		public RelationType ValidThroughDate;

		#region Allors
		[Id("3da51ccc-24b9-4b03-9218-7da06492224d")]
		[AssociationId("602c70c9-ddc4-4cf5-a79f-0abcc0beba15")]
		[RoleId("d4d93ad0-c59d-40e7-a82c-4fb1e54a85f2")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("9119c598-cd98-43da-bfdf-1e6573112c9e")]
		[AssociationId("d48cd46d-889b-4e2d-a6d6-ee26f30fb3e5")]
		[RoleId("56f5d5ee-1ab5-48f2-a413-7b80dd2c283e")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Receivers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Receiver;

		#region Allors
		[Id("b5bcf357-ef14-424d-ad8d-01a8e3ff478c")]
		[AssociationId("b9338369-9081-4fa7-91c2-140a46ea7d27")]
		[RoleId("984b073d-0213-4539-8d3c-a35a81a71bd5")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;

		#region Allors
		[Id("d7dc81e8-76e7-4c68-9843-a2aaf8293510")]
		[AssociationId("6fbc80d1-e72b-4484-a9b1-e606f15d2435")]
		[RoleId("219cb27f-20b5-48b3-9d89-4b119798b092")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("IssueDates")]
		public RelationType IssueDate;

		#region Allors
		[Id("e250154a-77c5-4a0b-ae3d-28668a9037d1")]
		[AssociationId("b5ba8cfd-2b16-4a50-89cd-46927d59b97a")]
		[RoleId("f5b6881b-c4d5-42e3-a024-0ae4564cb970")]
		#endregion
		[Indexed]
		[Type(typeof(QuoteItemClass))]
		[Plural("QuoteItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType QuoteItem;

		#region Allors
		[Id("e76cbd73-78b7-4ef8-a24c-9ac0db152f7f")]
		[AssociationId("057ad29f-c245-44b2-8a95-71bd6607830b")]
		[RoleId("218e3a6e-b530-41f7-a60e-7587f8072c8c")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("QuoteNumbers")]
		public RelationType QuoteNumber;



		public static QuoteInterface Instance {get; internal set;}

		internal QuoteInterface() : base(MetaPopulation.Instance)
        {
        }
	}
}