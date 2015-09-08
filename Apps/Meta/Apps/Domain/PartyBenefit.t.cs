namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("d520cf1a-8d3a-4380-8b88-85cd63a5ad05")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]

	[Plural("PartyBenefits")]
	public partial class PartyBenefitClass : Class
	{
		#region Allors
		[Id("15638ba3-73c7-4c32-aaa7-a91d4a5e9951")]
		[AssociationId("e904820e-49c4-4fa7-9f91-55e9430bcf38")]
		[RoleId("0af63f52-dd36-45b4-9123-4d12a74d502a")]
		#endregion
		[Indexed]
		[Type(typeof(TimeFrequencyClass))]
		[Plural("TimeFrequencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TimeFrequency;

		#region Allors
		[Id("1c4a69e7-62c7-4e6b-b7a5-69817d1788df")]
		[AssociationId("67280aad-73cd-4366-8a4f-2d38257e022e")]
		[RoleId("0d391786-7d0b-488f-95db-f449f85459ec")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Costs")]
		public RelationType Cost;

		#region Allors
		[Id("320e98c9-adff-41cf-894a-500730cf6c09")]
		[AssociationId("b9693920-2e4d-41e2-8925-c6e40b0ed673")]
		[RoleId("2d48ba77-63e8-4397-9004-09329058f01b")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ActualEmployerPaidPercentages")]
		public RelationType ActualEmployerPaidPercentage;

		#region Allors
		[Id("9a8fcada-bf2c-450d-a941-e0c7ec414cf3")]
		[AssociationId("56813128-50b2-4fbf-ad0f-0385930a6805")]
		[RoleId("95e9e94b-0fad-47e0-bfcf-f131e6962694")]
		#endregion
		[Indexed]
		[Type(typeof(BenefitClass))]
		[Plural("Benefits")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Benefit;

		#region Allors
		[Id("e4bd1497-824b-477a-9842-a87b4193b430")]
		[AssociationId("fc6f6c2a-5732-4c3d-8db0-58e3a4f26d6c")]
		[RoleId("53891641-e7a9-46a6-bd2c-56b7f23b0ab5")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("ActualAvailableTimes")]
		public RelationType ActualAvailableTime;

		#region Allors
		[Id("f377fde6-39b4-47e1-a3c0-e574f416f6ad")]
		[AssociationId("8a5b1e5f-e945-43ac-a1ae-72c1c0457a73")]
		[RoleId("c4d10070-cbf6-4ebf-a9f1-a53a8d6d41b4")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentClass))]
		[Plural("Employments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employment;



		public static PartyBenefitClass Instance {get; internal set;}

		internal PartyBenefitClass() : base(MetaPopulation.Instance)
        {
        }
	}
}