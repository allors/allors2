namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("69db99bc-97f7-4e2e-903c-74afb55992af")]
	#endregion
	[Inherit(typeof(EnumerationInterface))]

	[Plural("VatRegimes")]
	public partial class VatRegimeClass : Class
	{
		#region Allors
		[Id("2071cc28-c8bf-43dc-a5e5-ec5735756dfa")]
		[AssociationId("fca4a435-bd82-496b-ab1d-c2b6cb10494f")]
		[RoleId("baf416cf-3222-4c93-8fb7-f4257b2b9ef9")]
		#endregion
		[Indexed]
		[Type(typeof(VatRateClass))]
		[Plural("VatRates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType VatRate;

		#region Allors
		[Id("a037f9f0-1aff-4ad0-8ee9-36ae4609d398")]
		[AssociationId("25db54a8-873d-4736-8408-f1d9e65c49e4")]
		[RoleId("238996a2-ec4f-47f4-8336-8fee91383649")]
		#endregion
		[Indexed]
		[Type(typeof(OrganisationGlAccountClass))]
		[Plural("GeneralLedgerAccounts")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType GeneralLedgerAccount;



		public static VatRegimeClass Instance {get; internal set;}

		internal VatRegimeClass() : base(MetaPopulation.Instance)
        {
        }
	}
}