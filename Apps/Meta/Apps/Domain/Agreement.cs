namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4deca253-7135-4ceb-b984-6adaf1515630")]
	#endregion
	[Plural("Agreements")]
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	[Inherit(typeof(PeriodInterface))]

  	public partial class AgreementInterface: Interface
	{
		#region Allors
		[Id("2ddce7b3-c763-45ea-8e1b-5ef8a0ea8e4a")]
		[AssociationId("d27ed7da-6a94-40ee-b790-8754282a2a1b")]
		[RoleId("f199641e-5574-4733-b4e9-42f6ccb713a8")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("AgreementDates")]
		public RelationType AgreementDate;

		#region Allors
		[Id("34f0e272-7c56-4d92-a187-c40d9d907110")]
		[AssociationId("537bbe1f-ab09-4cbe-92d6-21e199dfcbf5")]
		[RoleId("3fdf6e81-1581-40ca-a1ba-647f33ede850")]
		#endregion
		[Indexed]
		[Type(typeof(AddendumClass))]
		[Plural("Addenda")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Addendum;

		#region Allors
		[Id("6bdc1767-2bbf-40de-9c2c-a84d1b376a6e")]
		[AssociationId("cc8c0485-68bb-46fb-b5e5-d9a970f33ad1")]
		[RoleId("14384f18-d46b-4f01-9414-9c6568b35e80")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Description")]
		public RelationType Description;

		#region Allors
		[Id("9d0e9ea7-31d7-4c01-96f2-97c3e17b3f18")]
		[AssociationId("2d2697e9-bbd2-4146-b96f-1bc36dca274c")]
		[RoleId("5ef5b4ca-6faa-4cf2-bfad-fa2f2902dbde")]
		#endregion
		[Indexed]
		[Type(typeof(AgreementTermInterface))]
		[Plural("AgreementTerms")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType AgreementTerm;

		#region Allors
		[Id("9f4db098-c486-4d88-9df9-cd7c79294575")]
		[AssociationId("89a37bf1-7c48-428e-b44c-113793c663aa")]
		[RoleId("3469f600-8da7-4d56-b58a-e525487149fc")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(-1)]
		[Plural("Texts")]
		public RelationType Text;

		#region Allors
		[Id("d5c90527-cae6-4a4f-9fd7-96f93dad59c7")]
		[AssociationId("ceb36b51-89ef-4335-a29f-c3c0f0fc3c06")]
		[RoleId("061d6861-26d9-4008-8612-b72e78bae14f")]
		#endregion
		[Indexed]
		[Type(typeof(AgreementItemInterface))]
		[Plural("AgreementItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType AgreementItem;

		#region Allors
		[Id("daff1ce2-4d60-426c-a45c-a82b63751657")]
		[AssociationId("5a11ccff-0d68-4b2c-a7b3-7ba90d9818b0")]
		[RoleId("da9244c2-9225-4448-b2ec-f3ee83d3ef15")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("AgreementNumbers")]
		public RelationType AgreementNumber;



		public static AgreementInterface Instance {get; internal set;}

		internal AgreementInterface() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
    }
}