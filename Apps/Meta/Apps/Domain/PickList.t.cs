namespace Allors.Meta
{
	#region Allors
	[Id("27b6630a-35d0-4352-9223-b5b6c8d7496b")]
	#endregion
	[Inherit(typeof(AccessControlledObjectInterface))]
	[Inherit(typeof(PrintableInterface))]
	[Inherit(typeof(TransitionalInterface))]
	[Inherit(typeof(UniquelyIdentifiableInterface))]
	public partial class PickListClass : Class
	{
        #region Allors
        [Id("CCBD7DB6-EC0F-4D70-9833-BC2A9E3A9292")]
        #endregion
        public MethodType Hold;

        #region Allors
        [Id("B88AF2FA-0940-4C3B-90E7-9937DF6C05AC")]
        #endregion
        public MethodType Continue;

        #region Allors
        [Id("41E4C5C4-2CFE-4B7F-80FD-E4C0263FDF62")]
        #endregion
        public MethodType Cancel;

        #region Allors
        [Id("CAC524A5-47A9-4FFD-ABC2-D5D3C0ABBFDD")]
        #endregion
        public MethodType SetPicked;

        #region Allors
        [Id("0bdfcd8a-af37-41a7-be2c-db7848d4fd05")]
		[AssociationId("88919577-6835-4c84-9e3d-a1ec50fc5c2b")]
		[RoleId("6042abcd-a859-42bb-818d-9409f7b08d7a")]
		#endregion
		[Indexed]
		[Type(typeof(CustomerShipmentClass))]
		[Plural("CustomerShipmentsCorrection")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CustomerShipmentCorrection;

		#region Allors
		[Id("1176ffe1-efff-4c02-b4df-5bba9052f6da")]
		[AssociationId("dcb3602c-f60e-4798-b32d-2a69f9e1056b")]
		[RoleId("920c6a7e-b8b8-4155-9209-4c8ed24a023a")]
		#endregion
		[Type(typeof(AllorsDateTimeUnit))]
		[Plural("CreationDates")]
		public RelationType CreationDate;

		#region Allors
		[Id("3bb68c85-4e2b-42b8-b5fb-18a66c58c283")]
		[AssociationId("11fddfe2-9b04-4b53-a4ff-6f571e73c32a")]
		[RoleId("a139b102-f8a9-43f1-9b14-d3c76f7be294")]
		#endregion
		[Indexed]
		[Type(typeof(PickListItemClass))]
		[Plural("PickListItems")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PickListItem;

		#region Allors
		[Id("4231c38e-e54c-480d-9e0f-2fe8bd101da1")]
		[AssociationId("b4d28461-6b82-4843-90ee-a5c3c0cddfc0")]
		[RoleId("11fa5c06-67ce-44e0-b205-e60be00e9922")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PickListObjectStateClass))]
		[Plural("CurrentObjectStates")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType CurrentObjectState;

		#region Allors
		[Id("62239709-cd1f-4582-99f7-8f18e875e241")]
		[AssociationId("61ae7eeb-259c-44bb-9de7-aff577a66669")]
		[RoleId("fe4d009e-1ea4-43d2-b4ce-96a1d9af5cf7")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PickListStatusClass))]
		[Plural("CurrentPickListStatuses")]
		[Multiplicity(Multiplicity.OneToOne)]
		public RelationType CurrentPickListStatus;

		#region Allors
		[Id("6572f862-31b2-4be9-b7dc-7fff5d21f620")]
		[AssociationId("2a502d47-1319-45a4-ad52-70dd41435732")]
		[RoleId("76ddffff-4968-4b4b-8b52-58a1a05a774d")]
		#endregion
		[Indexed]
		[Type(typeof(PersonClass))]
		[Plural("Pickers")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Picker;

		#region Allors
		[Id("7b5e6ef5-e5c0-4e7c-8955-b6c18f136fee")]
		[AssociationId("ede5efc3-a840-44b5-8389-611c05ae4df2")]
		[RoleId("ec323cf6-acad-4e8b-bb73-0323e9aee277")]
		#endregion
		[Derived]
		[Indexed]
		[Type(typeof(PickListStatusClass))]
		[Plural("PickListStatuses")]
		[Multiplicity(Multiplicity.OneToMany)]
		public RelationType PickListStatus;

		#region Allors
		[Id("ae75482e-2c41-46d4-ab73-f3aac368fd50")]
		[AssociationId("6b8acd68-6aba-4092-8c87-cdc62d9a4c82")]
		[RoleId("61785577-8ab7-457c-870f-69ecb7c41f8b")]
		#endregion
		[Indexed]
		[Type(typeof(PartyInterface))]
		[Plural("ShipToParties")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType ShipToParty;

		#region Allors
		[Id("e334e938-35e7-4217-91fa-efb231f71a37")]
		[AssociationId("0706d8f1-764d-4ab9-b63a-1b0213cc9dbd")]
		[RoleId("4c3d2de1-6735-40fc-bfe9-65a64aaf966c")]
		#endregion
		[Indexed]
		[Type(typeof(StoreClass))]
		[Plural("Stores")]
		[Multiplicity(Multiplicity.ManyToOne)]
		public RelationType Store;
        
		public static PickListClass Instance {get; internal set;}

		internal PickListClass() : base(MetaPopulation.Instance)
        {
        }

        internal override void AppsExtend()
        {
            this.CreationDate.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
    }
}