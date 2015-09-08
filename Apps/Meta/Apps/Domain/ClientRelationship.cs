namespace Allors.Meta
{
	using System;

	#region Allors
	[Id("aadaf02e-0bb8-4862-a354-488f39aa8f4e")]
	#endregion
	[Inherit(typeof(PartyRelationshipInterface))]

	[Plural("ClientRelationships")]
	public partial class ClientRelationshipClass : Class
	{
		#region Allors
		[Id("d611f21a-1045-40ea-b05b-0c29913d5f1c")]
		[AssociationId("115baf34-414a-4cfa-8d1f-dfbeb7264077")]
		[RoleId("69161130-4a2e-430e-92a2-b8ab0e6ef8dc")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("Clients")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Client;

		#region Allors
		[Id("e081884c-3db2-4be3-9c85-9167528d751b")]
		[AssociationId("32544879-3730-449a-9835-8decbfe9f4fc")]
		[RoleId("2f9c92b5-7cf2-42ba-924d-4b5d0c73956c")]
		#endregion
		[Indexed]
		[Type(typeof(InternalOrganisationClass))]
		[Plural("InternalOrganisations")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType InternalOrganisation;



		public static ClientRelationshipClass Instance {get; internal set;}

		internal ClientRelationshipClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.Client.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
        }
    }
}