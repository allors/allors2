namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("ef5fb351-2f0f-454a-b7b2-104af42b2c72")]
	#endregion
	[Inherit(typeof(PaymentInterface))]

	[Plural("PayChecks")]
	public partial class PayCheckClass : Class
	{
		#region Allors
		[Id("59ddff84-5e67-4210-b721-955e08f8453e")]
		[AssociationId("5d445586-f239-4e3b-a3cb-368d46df306f")]
		[RoleId("9a3b62ee-6197-4670-ad8b-c01201ea2235")]
		#endregion
		[Indexed]
		[Type(typeof(DeductionClass))]
		[Plural("Deductions")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType Deduction;

		#region Allors
		[Id("5db6f5b5-e24e-44fd-bc41-4e0466e97906")]
		[AssociationId("53d7d8c9-7028-4ec8-82af-6373e21e3532")]
		[RoleId("c2e4cf65-7a57-4dcd-ab49-c6cbc6b9d9fb")]
		#endregion
		[Indexed]
		[Type(typeof(EmploymentClass))]
		[Plural("Employments")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Employment;



		public static PayCheckClass Instance {get; internal set;}

		internal PayCheckClass() : base(MetaPopulation.Instance)
        {
        }
	}
}