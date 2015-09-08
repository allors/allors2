namespace Allors.Meta
{
	#region Allors
	[Id("9426c214-c85d-491b-a5a6-9f573c3341a0")]
	#endregion
	[Inherit(typeof(CommunicationEventInterface))]
	public partial class EmailCommunicationClass : Class
	{
		#region Allors
		[Id("25b8aa5e-e7c5-4689-b1ed-d9a0ba47b8eb")]
		[AssociationId("11649936-a5fa-488e-8d17-e80619c4d634")]
		[RoleId("6219fd3b-4f38-4f8f-8a5a-783f908ef55a")]
		#endregion
		[Indexed]
		[Type(typeof(EmailAddressClass))]
		[Plural("Originators")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Originator;

		#region Allors
		[Id("4026fcf7-3fc2-494b-9c4a-3e19eed74134")]
		[AssociationId("f2febf7f-7917-4499-8546-cae1e53d6791")]
		[RoleId("50439b5a-2251-469c-8512-f9dc65b0d9f6")]
		#endregion
		[Indexed]
		[Type(typeof(EmailAddressClass))]
		[Plural("Addressees")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType Addressee;

		#region Allors
		[Id("4f696f91-e185-4d3d-bf40-40e6c2b02eb4")]
		[AssociationId("a19fe8f6-a3b9-4d59-b2e6-cfc19cc01a58")]
		[RoleId("661f4ae9-684b-4b56-9ec6-7bf9fbfea4ab")]
		#endregion
		[Indexed]
		[Type(typeof(EmailAddressClass))]
		[Plural("CarbonCopies")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType CarbonCopy;

		#region Allors
		[Id("dd7506bb-4daa-4da7-8f20-3f607c944959")]
		[AssociationId("42fb79f1-c891-41bf-be4b-a2717bd94e69")]
		[RoleId("6d75e51a-7994-43bb-9e99-cd0a88d9d8f2")]
		#endregion
		[Indexed]
		[Type(typeof(EmailAddressClass))]
		[Plural("BlindCopies")]
		[Multiplicity(Multiplicity.ManyToMany)]
		public RelationType BlindCopy;

		#region Allors
		[Id("e12818ad-4ffd-4d91-8142-4ac9bfcbc146")]
		[AssociationId("a44a8d84-2510-45fd-add1-646f84be072d")]
		[RoleId("ae354426-6273-4b09-aabf-3f6d25f86e56")]
		#endregion
		[Indexed]
		[Type(typeof(EmailTemplateClass))]
		[Plural("EmailTemplates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType EmailTemplate;
        
		public static EmailCommunicationClass Instance {get; internal set;}

		internal EmailCommunicationClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.ConcreteRoleTypeByRoleType[CommunicationEventInterface.Instance.Subject.RoleType].IsRequiredOverride = true;
        }
    }
}