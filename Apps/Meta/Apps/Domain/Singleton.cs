namespace Allors.Meta
{
	public partial class SingletonClass : Class
	{
		#region Allors
		[Id("9dee4a94-26d5-410f-a3e3-3fcde21c5c89")]
		[AssociationId("0322b71b-0389-4393-8b1f-1b3fb12bb7b1")]
		[RoleId("68f80e6a-7ff4-4f07-b2c5-728459c376ae")]
		#endregion
		[Indexed]
		[Type(typeof(CurrencyClass))]
		[Plural("DefaultCurrencies")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultCurrency;

		#region Allors
		[Id("a0fdc553-8081-43fa-ae1a-b9f7767d2d3e")]
		[AssociationId("c36bd0ce-d912-4935-b2e2-5aecc822a524")]
		[RoleId("65e3b040-4191-4f26-a51b-6c2a17ec35c7")]
		#endregion
		[Indexed]
		[Type(typeof(MediaClass))]
		[Plural("NoImageAvailableImages")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType NoImageAvailableImage;

		#region Allors
		[Id("f154f01e-e8bb-49c0-be80-ef6c6c195ff3")]
		[AssociationId("2c42c9e4-72e3-4673-8653-aaf586ebb06a")]
		[RoleId("979d1e59-7a9f-462a-9927-efb8ad2cada5")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("DefaultInternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType DefaultInternalOrganisation;
	}
}