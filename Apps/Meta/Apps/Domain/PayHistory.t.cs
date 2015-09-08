namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("208a5af6-8dd8-4a48-acb2-2ecb89e8d322")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PeriodInterface))]

	[Plural("PayHistories")]
	public partial class PayHistoryClass : Class
	{
		#region Allors
		[Id("2f14e234-c808-4059-bb29-48e6d9493b7b")]
		[AssociationId("e44919c4-eac2-4be0-a244-b2cacdf1c4c4")]
		[RoleId("0441ebf6-3607-44e7-98ca-831e146cf9d7")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentClass))]
		[Plural("Employments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employment;

		#region Allors
		[Id("6d26369b-eea2-4712-a7d1-56884a3cc715")]
		[AssociationId("6e23ddf7-9766-4f56-bd4f-587bb6f00e00")]
		[RoleId("9d1f6129-281c-413d-ba78-fdb99c84a8b2")]
		#endregion
		[Indexed]
		[Type(typeof(TimeFrequencyClass))]
		[Plural("TimeFrequencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType TimeFrequency;

		#region Allors
		[Id("b3f1071f-7e71-4ef1-aa9b-545ad694f44c")]
		[AssociationId("717107b5-fafc-4cca-b85d-364d819a7529")]
		[RoleId("3f7535b3-76dc-47c8-9668-895596bafc16")]
		#endregion
		[Indexed]
		[Type(typeof(SalaryStepClass))]
		[Plural("SalarySteps")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType SalaryStep;

		#region Allors
		[Id("b7ef1bf8-b16b-400e-903e-d0a7454572a0")]
		[AssociationId("9717c46c-8c64-477a-916a-98594dd21039")]
		[RoleId("fcae3d2d-fe78-4501-8c8e-bda78822c6f2")]
		#endregion
		[Type(typeof(AllorsDecimalUnit))]
		[Precision(19)]
		[Scale(2)]
		[Plural("Amounts")]
		public RelationType Amount;



		public static PayHistoryClass Instance {get; internal set;}

		internal PayHistoryClass() : base(MetaPopulation.Instance)
        {
        }
	}
}