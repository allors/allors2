namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("4d599ed2-c5e3-4c1d-8128-6ff61f9072c3")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PositionTypes")]
	public partial class PositionTypeClass : Class
	{
		#region Allors
		[Id("08ca7d83-ca74-4cc1-9d8a-6cc254c7bd5b")]
		[AssociationId("9c14fc30-8b9c-4aaa-8e85-e635c0191111")]
		[RoleId("692c0d2f-0e62-4601-b7d4-21e496596f5d")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Descriptions")]
		public RelationType Description;

		#region Allors
		[Id("44d5c360-a82d-40ca-a56c-e377327a4858")]
		[AssociationId("0588e142-76ff-43a7-ae6e-63427fc18b43")]
		[RoleId("6c00b475-38d9-4f2a-a53b-5a82434db39a")]
		#endregion
		[Indexed]
		[Type(typeof(ResponsibilityClass))]
		[Plural("Responsibilities")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Responsibility;

		#region Allors
		[Id("520649d5-7775-43d0-ab4b-762b2ec6557e")]
		[AssociationId("e63e57e3-ae72-456a-9dd4-881ac8c07525")]
		[RoleId("a56a7a77-1233-46c4-86b1-f6ac24d7a1f8")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("BenefitPercentages")]
		public RelationType BenefitPercentage;

		#region Allors
		[Id("8e8e40ff-d11d-4805-abde-845a1b3f1241")]
		[AssociationId("f20d568c-3bd8-4383-9cae-052e10065c8e")]
		[RoleId("169055d1-d2ec-4a10-8792-574d8577b273")]
		#endregion
		[Type(typeof(AllorsStringUnit))]
		[Size(256)]
		[Plural("Titles")]
		public RelationType Title;

		#region Allors
		[Id("aa3886a5-a407-4598-900c-8fc3bcfc0604")]
		[AssociationId("bb552053-d6e7-470b-a8d9-81ed85950b19")]
		[RoleId("827e2eda-b1bf-4040-9c3a-ff728a44f4c3")]
		#endregion
		[Indexed]
		[Type(typeof(PositionTypeRateClass))]
		[Plural("PositionTypeRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType PositionTypeRate;



		public static PositionTypeClass Instance {get; internal set;}

		internal PositionTypeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}