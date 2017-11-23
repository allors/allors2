namespace Allors.Workspace.Meta
{
	using Allors.Meta;

	internal class MetaBuilder
	{
	    private readonly MetaPopulation metaPopulation;

        internal MetaBuilder(MetaPopulation metaPopulation)
		{
			this.metaPopulation = metaPopulation;
		}

		internal void BuildDomains()
		{
			// Domains
            MetaCustom.Instance = new MetaCustom(this.metaPopulation);
            MetaBase.Instance = new MetaBase(this.metaPopulation);
            MetaCore.Instance = new MetaCore(this.metaPopulation);

			// Domain Inheritance
            MetaCustom.Instance.Domain.AddDirectSuperdomain(MetaBase.Instance.Domain);
            MetaBase.Instance.Domain.AddDirectSuperdomain(MetaCore.Instance.Domain);

		}

		internal void BuildObjectTypes()
		{

			// Units
            MetaBinary.Instance = new MetaBinary(this.metaPopulation);
            MetaBoolean.Instance = new MetaBoolean(this.metaPopulation);
            MetaDateTime.Instance = new MetaDateTime(this.metaPopulation);
            MetaDecimal.Instance = new MetaDecimal(this.metaPopulation);
            MetaFloat.Instance = new MetaFloat(this.metaPopulation);
            MetaInteger.Instance = new MetaInteger(this.metaPopulation);
            MetaString.Instance = new MetaString(this.metaPopulation);
            MetaUnique.Instance = new MetaUnique(this.metaPopulation);

			// Interfaces
            MetaVersion.Instance = new MetaVersion(this.metaPopulation);
            MetaDeletable.Instance = new MetaDeletable(this.metaPopulation);
            MetaEnumeration.Instance = new MetaEnumeration(this.metaPopulation);
            MetaLocalised.Instance = new MetaLocalised(this.metaPopulation);
            MetaObjectState.Instance = new MetaObjectState(this.metaPopulation);
            MetaUniquelyIdentifiable.Instance = new MetaUniquelyIdentifiable(this.metaPopulation);
            MetaUser.Instance = new MetaUser(this.metaPopulation);
            MetaTask.Instance = new MetaTask(this.metaPopulation);
            MetaWorkItem.Instance = new MetaWorkItem(this.metaPopulation);
            MetaI1.Instance = new MetaI1(this.metaPopulation);

			// Classes
            MetaLocalisedText.Instance = new MetaLocalisedText(this.metaPopulation);
            MetaAccessControl.Instance = new MetaAccessControl(this.metaPopulation);
            MetaCounter.Instance = new MetaCounter(this.metaPopulation);
            MetaCountry.Instance = new MetaCountry(this.metaPopulation);
            MetaCurrency.Instance = new MetaCurrency(this.metaPopulation);
            MetaLanguage.Instance = new MetaLanguage(this.metaPopulation);
            MetaLocale.Instance = new MetaLocale(this.metaPopulation);
            MetaLogin.Instance = new MetaLogin(this.metaPopulation);
            MetaMedia.Instance = new MetaMedia(this.metaPopulation);
            MetaMediaContent.Instance = new MetaMediaContent(this.metaPopulation);
            MetaAutomatedAgent.Instance = new MetaAutomatedAgent(this.metaPopulation);
            MetaPermission.Instance = new MetaPermission(this.metaPopulation);
            MetaPerson.Instance = new MetaPerson(this.metaPopulation);
            MetaRole.Instance = new MetaRole(this.metaPopulation);
            MetaSecurityToken.Instance = new MetaSecurityToken(this.metaPopulation);
            MetaSingleton.Instance = new MetaSingleton(this.metaPopulation);
            MetaUserGroup.Instance = new MetaUserGroup(this.metaPopulation);
            MetaNotification.Instance = new MetaNotification(this.metaPopulation);
            MetaNotificationList.Instance = new MetaNotificationList(this.metaPopulation);
            MetaTaskAssignment.Instance = new MetaTaskAssignment(this.metaPopulation);
            MetaTaskList.Instance = new MetaTaskList(this.metaPopulation);
            MetaC1.Instance = new MetaC1(this.metaPopulation);
            MetaDependent.Instance = new MetaDependent(this.metaPopulation);
            MetaGender.Instance = new MetaGender(this.metaPopulation);
            MetaOrder.Instance = new MetaOrder(this.metaPopulation);
            MetaOrderLine.Instance = new MetaOrderLine(this.metaPopulation);
            MetaOrderLineVersion.Instance = new MetaOrderLineVersion(this.metaPopulation);
            MetaPaymentState.Instance = new MetaPaymentState(this.metaPopulation);
            MetaShipmentState.Instance = new MetaShipmentState(this.metaPopulation);
            MetaOrderState.Instance = new MetaOrderState(this.metaPopulation);
            MetaOrderVersion.Instance = new MetaOrderVersion(this.metaPopulation);
            MetaOrganisation.Instance = new MetaOrganisation(this.metaPopulation);
            MetaUnitSample.Instance = new MetaUnitSample(this.metaPopulation);

			// Inheritance
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaEnumeration.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};

            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaObjectState.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};

            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUser.Instance.ObjectType, Supertype = MetaLocalised.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTask.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTask.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};


            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLocalisedText.Instance.ObjectType, Supertype = MetaLocalised.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAccessControl.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaCounter.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};




            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLogin.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMedia.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMedia.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMediaContent.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAutomatedAgent.Instance.ObjectType, Supertype = MetaUser.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPermission.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaUser.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaRole.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSecurityToken.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};

            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUserGroup.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};

            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaNotificationList.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTaskAssignment.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTaskList.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaC1.Instance.ObjectType, Supertype = MetaI1.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaDependent.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaGender.Instance.ObjectType, Supertype = MetaEnumeration.Instance.Interface};


            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderLineVersion.Instance.ObjectType, Supertype = MetaVersion.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPaymentState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaShipmentState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderVersion.Instance.ObjectType, Supertype = MetaVersion.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrganisation.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrganisation.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};

		}

		internal void BuildOperandTypes()
		{
			// Exclusive Roles
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("adf611c3-047a-4bae-95e3-776022d5ce7b"), new System.Guid("7145b062-aee9-4b30-adb8-c691969c6874"), new System.Guid("b38c700c-7ad9-4962-9f53-35b8aef22e09"));
		relationType.AssociationType.ObjectType = MetaVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DerivationTimeStamp";
		relationType.RoleType.PluralName = "DerivationTimeStamps";
		MetaVersion.Instance.DerivationTimeStamp = relationType.RoleType; 
	}

	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("07e034f1-246a-4115-9662-4c798f31343f"), new System.Guid("bcf428fd-0263-488c-b9ac-963ceca1c972"), new System.Guid("919fdad7-830e-4b12-b23c-f433951236af"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaEnumeration.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("3d3ae4d0-bac6-4645-8a53-3e9f7f9af086"), new System.Guid("004cc333-b8ae-4952-ae13-f2ab80eb018c"), new System.Guid("5850860d-c772-402f-815b-7634c9a1e697"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaEnumeration.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f57bb62e-77a8-4519-81e6-539d54b71cb7"), new System.Guid("a8993304-52c0-4b53-9982-6caa5675467a"), new System.Guid("0c6faf5a-eac9-454c-bd53-3b8409e56d34"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsActive";
		relationType.RoleType.PluralName = "IsActives";
		relationType.RoleType.IsRequired = true;
		MetaEnumeration.Instance.IsActive = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8c005a4e-5ffe-45fd-b279-778e274f4d83"), new System.Guid("6684d98b-cd43-4612-bf9d-afefe02a0d43"), new System.Guid("d43b92ac-9e6f-4238-9625-1e889be054cf"));
		relationType.AssociationType.ObjectType = MetaLocalised.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "Locale";
		relationType.RoleType.PluralName = "Locales";
		relationType.RoleType.IsRequired = true;
		MetaLocalised.Instance.Locale = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b86f9e42-fe10-4302-ab7c-6c6c7d357c39"), new System.Guid("052ec640-3150-458a-99d5-0edce6eb6149"), new System.Guid("945cbba6-4b09-4b87-931e-861b147c3823"));
		relationType.AssociationType.ObjectType = MetaObjectState.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaObjectState.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e1842d87-8157-40e7-b06e-4375f311f2c3"), new System.Guid("fe413e96-cfcf-4e8d-9f23-0fa4f457fdf1"), new System.Guid("d73fd9a4-13ee-4fa9-8925-d93eca328bf6"));
		relationType.AssociationType.ObjectType = MetaUniquelyIdentifiable.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "UniqueId";
		relationType.RoleType.PluralName = "UniqueIds";
		relationType.RoleType.IsRequired = true;
		MetaUniquelyIdentifiable.Instance.UniqueId = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5e8ab257-1a1c-4448-aacc-71dbaaba525b"), new System.Guid("eca7ef36-8928-4116-bfce-1896a685fe8c"), new System.Guid("3b7d40a0-18ea-4018-b797-6417723e1890"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "UserName";
		relationType.RoleType.PluralName = "UserNames";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.UserName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7397b596-d8fa-4e3c-8e0e-ea24790fe2e4"), new System.Guid("19cad82c-6538-4c46-aa3f-75c082cc8204"), new System.Guid("faf89920-880f-4600-baf1-a27a5268444a"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "NormalizedUserName";
		relationType.RoleType.PluralName = "NormalizedUserNames";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.NormalizedUserName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c1ae3652-5854-4b68-9890-a954067767fc"), new System.Guid("111104a2-1181-4958-92f6-6528cef79af7"), new System.Guid("58e35754-91a9-4956-aa66-ca48d05c7042"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "UserEmail";
		relationType.RoleType.PluralName = "UserEmails";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.UserEmail = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0b3b650b-fcd4-4475-b5c4-e2ee4f39b0be"), new System.Guid("c89a8e3f-6f76-41ac-b4dc-839f9080d917"), new System.Guid("1b1409b8-add7-494c-a895-002fc969ac7b"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "UserEmailConfirmed";
		relationType.RoleType.PluralName = "UserEmailConfirmeds";
		MetaUser.Instance.UserEmailConfirmed = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b4c09bec-168f-4f05-8ec6-919d1a24ae96"), new System.Guid("3d05bc18-c205-424a-ab26-fec24eafbd78"), new System.Guid("484ecaae-3f39-451b-a689-a55601df6778"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskList.Instance;
		relationType.RoleType.SingularName = "TaskList";
		relationType.RoleType.PluralName = "TaskLists";
		MetaUser.Instance.TaskList = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bed34563-4ed8-4c6b-88d2-b4199e521d74"), new System.Guid("e678c2f8-5c66-4886-ad21-2be98101f759"), new System.Guid("79e9a907-9237-4aab-9340-277d593748f5"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotificationList.Instance;
		relationType.RoleType.SingularName = "NotificationList";
		relationType.RoleType.PluralName = "NotificationLists";
		MetaUser.Instance.NotificationList = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f247de73-70fe-47e4-a763-22ee9c68a476"), new System.Guid("2e1ebe97-52d3-46fc-94c2-3203a13856c7"), new System.Guid("4ca8997f-9232-4c84-8f37-e977071eb316"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaWorkItem.Instance;
		relationType.RoleType.SingularName = "WorkItem";
		relationType.RoleType.PluralName = "WorkItems";
		relationType.RoleType.IsRequired = true;
		MetaTask.Instance.WorkItem = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8ebd9048-a344-417c-bae7-359ca9a74aa1"), new System.Guid("af6cbf34-5f71-498b-a2ec-ef698eeae799"), new System.Guid("ceba2888-2a6e-4822-881b-1101b48f80f3"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateCreated";
		relationType.RoleType.PluralName = "DateCreateds";
		relationType.RoleType.IsRequired = true;
		MetaTask.Instance.DateCreated = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5ad0b9f5-669c-4b05-8c97-89b59a227da2"), new System.Guid("b3182870-cbe0-4da1-aaeb-804df5a9f869"), new System.Guid("eacac26b-fea7-49f8-abb6-57d63accd548"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateClosed";
		relationType.RoleType.PluralName = "DateCloseds";
		MetaTask.Instance.DateClosed = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("55375d57-34b0-43d0-9fac-e9788e1b6cd2"), new System.Guid("0d421578-35fc-4309-b8b6-cda0c9cf948c"), new System.Guid("a7c8f58f-358a-4ae9-9299-0ef560190541"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Participant";
		relationType.RoleType.PluralName = "Participants";
		MetaTask.Instance.Participants = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ea8abc59-b625-4d25-85bd-dd04bfe55086"), new System.Guid("90150444-fc18-47fd-a6fd-7740006e64ca"), new System.Guid("34320d76-6803-4615-8444-cc3ea8bb0315"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Performer";
		relationType.RoleType.PluralName = "Performers";
		MetaTask.Instance.Performer = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7e6d392b-00e7-4095-8525-d9f4ef8cfaa3"), new System.Guid("b20f1b54-87a4-4fc2-91db-8848d6d40ad1"), new System.Guid("cf456f4d-8c76-4bfe-9996-89b660c9b153"));
		relationType.AssociationType.ObjectType = MetaWorkItem.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "WorkItemDescription";
		relationType.RoleType.PluralName = "WorkItemDescriptions";
		relationType.RoleType.Size = 256;
		MetaWorkItem.Instance.WorkItemDescription = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("28ceffc2-c776-4a0a-9825-a6d1bcb265dc"), new System.Guid("0287a603-59e5-4241-8b2e-a21698476e67"), new System.Guid("fec573a7-5ab3-4f30-9b50-7d720b4af4b4"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "I1AllorsString";
		relationType.RoleType.PluralName = "I1AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaI1.Instance.I1AllorsString = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50dc85f0-3d22-4bc1-95d9-153674b89f7a"), new System.Guid("accd061b-20b9-4a24-bb2c-c2f7276f43ab"), new System.Guid("8d3f68e1-fa6e-414f-aa4d-25fcc2c975d6"));
		relationType.AssociationType.ObjectType = MetaLocalisedText.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Text";
		relationType.RoleType.PluralName = "Texts";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = -1;
		MetaLocalisedText.Instance.Text = relationType.RoleType; 
	}


	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("62009cef-7424-4ec0-8953-e92b3cd6639d"), new System.Guid("323173ee-385c-4f74-8b78-ff05735460f8"), new System.Guid("4ca5a640-5d9e-4910-95ed-6872c7ea13d2"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaCurrency.Instance;
		relationType.RoleType.SingularName = "Currency";
		relationType.RoleType.PluralName = "Currencies";
		MetaCountry.Instance.Currency = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f93acc4e-f89e-4610-ada9-e58f21c165bc"), new System.Guid("ea0efe67-89f2-4317-97e7-f0e14358e718"), new System.Guid("4fe997d6-d221-432b-9f09-4f77735c109b"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 2;
		MetaCountry.Instance.IsoCode = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6b9c977f-b394-440e-9781-5d56733b60da"), new System.Guid("6e3532ae-3528-4114-9274-54fc08effd0d"), new System.Guid("60f1f9a3-13d1-485f-bc77-fda1f9ef1815"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaCountry.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8236a702-a76d-4bb5-9afd-acacb1508261"), new System.Guid("9b682612-50f9-43f3-abde-4d0cb5156f0d"), new System.Guid("99c52c13-ef50-4f68-a32f-fef660aa3044"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaCountry.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("294a4bdc-f03a-47a2-a649-419e6b9021a3"), new System.Guid("f9eec7c6-c4cd-4d8c-a5f7-44855328cd7e"), new System.Guid("09d74027-4457-4788-803c-24b857245565"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaCurrency.Instance.IsoCode = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("74c8308b-1b76-4218-9532-f01c9d1e146b"), new System.Guid("2cb43671-c648-4bd4-ac08-7302c29246e7"), new System.Guid("e7c93764-d634-4187-97ed-9248ea56bab2"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaCurrency.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e9fc0472-cf7a-4e02-b061-cb42b6f5c273"), new System.Guid("06b8f2b2-91f0-4b89-ae19-b47de4524556"), new System.Guid("e1301b8f-25cc-4ace-884e-79af1d303f53"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaCurrency.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d2a32d9f-21cc-4f9d-b0d3-a9b75da66907"), new System.Guid("6c860e73-d12e-4e35-897e-ed9f8fd8eba0"), new System.Guid("84f904a6-8dcc-4089-bda6-34325ade6367"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.IsoCode = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("be482902-beb5-4a76-8ad0-c1b1c1c0e5c4"), new System.Guid("d3369fa9-afb7-4d5a-b476-3e4d43cce0fd"), new System.Guid("308d73b0-1b65-40a9-88f1-288848849c51"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("842cc899-3f37-455a-ae91-51d29d615e69"), new System.Guid("f3c7a07e-a2f3-4a96-91ef-d53ddf009dd4"), new System.Guid("c78e1736-74b4-4574-a365-421dcadf4d4c"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "NativeName";
		relationType.RoleType.PluralName = "NativeNames";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.NativeName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f091b264-e6b1-4a57-bbfb-8225cbe8190c"), new System.Guid("6650af3b-f537-4c2f-afff-6773552315cd"), new System.Guid("5e9fcced-727d-42a2-95e6-a0f9d8be4ec7"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaLanguage.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2a2c6f77-e6a2-4eab-bfe3-5d35a8abd7f7"), new System.Guid("09422255-fa17-41d8-991b-d21d7b37c6c5"), new System.Guid("647db2b3-997b-4c3a-9ae2-d49b410933c1"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaLocale.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d8cac34a-9bb2-4190-bd2a-ec0b87e04cf5"), new System.Guid("af501892-3c83-41d1-826b-f5c4cb1de7fe"), new System.Guid("ed32b12a-00ad-420b-9dfa-f1c6ce773fcd"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLanguage.Instance;
		relationType.RoleType.SingularName = "Language";
		relationType.RoleType.PluralName = "Languages";
		relationType.RoleType.IsRequired = true;
		MetaLocale.Instance.Language = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ea778b77-2929-4ab4-ad99-bf2f970401a9"), new System.Guid("bb5904f5-feb0-47eb-903a-0351d55f0d8c"), new System.Guid("b2fc6e06-3881-427e-b4cc-8457a65f8076"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaCountry.Instance;
		relationType.RoleType.SingularName = "Country";
		relationType.RoleType.PluralName = "Countries";
		relationType.RoleType.IsRequired = true;
		MetaLocale.Instance.Country = relationType.RoleType; 
	}

	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b74c2159-739a-4f1c-ada7-c2dcc3cdcf83"), new System.Guid("96b21673-f124-4c30-a2f0-df56d29e03f5"), new System.Guid("de0fe224-c40d-469c-bdc5-849a7412efec"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "Revision";
		relationType.RoleType.PluralName = "Revisions";
		MetaMedia.Instance.Revision = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("67082a51-1502-490b-b8db-537799e550bd"), new System.Guid("e8537dcf-1bd7-46c4-a37c-077bee6a78a1"), new System.Guid("02fe1ce8-c019-4a40-bd6f-b38d2f47a288"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMediaContent.Instance;
		relationType.RoleType.SingularName = "MediaContent";
		relationType.RoleType.PluralName = "MediaContents";
		relationType.RoleType.IsRequired = true;
		MetaMedia.Instance.MediaContent = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("18236718-1835-430c-a936-7ec461eee2cf"), new System.Guid("8a79e6c5-4bae-468d-b57c-c7788d3e21e3"), new System.Guid("877abdc8-8915-4640-8871-8cef7ef69072"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "InData";
		relationType.RoleType.PluralName = "InDatas";
		relationType.RoleType.Size = -1;
		MetaMedia.Instance.InData = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("79b04065-f13b-43b3-b86e-f3adbbaaf0c4"), new System.Guid("287b7291-39f0-43e5-8770-811940e81bae"), new System.Guid("ce17bfc7-5a4e-415a-9ae0-fae429cee69c"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "InDataUri";
		relationType.RoleType.PluralName = "InDataUris";
		relationType.RoleType.Size = -1;
		MetaMedia.Instance.InDataUri = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ddd6c005-0104-44ca-a19c-1150b8beb4a3"), new System.Guid("4f43b520-404e-436d-a514-71e4aec55ec8"), new System.Guid("4c4ec21c-a3c0-4720-92e0-cf6532000265"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FileName";
		relationType.RoleType.PluralName = "FileNames";
		relationType.RoleType.Size = 256;
		MetaMedia.Instance.FileName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("29541613-0b16-49ad-8f40-3309a7c7d7b8"), new System.Guid("efb76140-4a2a-4e7f-b51d-c95bca774664"), new System.Guid("7cfc8b40-5199-4457-bbbd-27a786721465"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Type";
		relationType.RoleType.PluralName = "Types";
		relationType.RoleType.Size = 1024;
		MetaMedia.Instance.Type = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("890598a9-0be4-49ee-8dd8-3581ee9355e6"), new System.Guid("3cf7f10e-dc56-4a50-95a5-fe7fae0be291"), new System.Guid("70823e7d-5829-4db7-99e0-f6c5f2b0e87b"));
		relationType.AssociationType.ObjectType = MetaMediaContent.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Type";
		relationType.RoleType.PluralName = "Types";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 1024;
		MetaMediaContent.Instance.Type = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0756d508-44b7-405e-bf92-bc09e5702e63"), new System.Guid("76e6547b-8dcf-4e69-ae2d-c8f8c33989e9"), new System.Guid("85170945-b020-485b-bb6f-c4122992ebfd"));
		relationType.AssociationType.ObjectType = MetaMediaContent.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "Data";
		relationType.RoleType.PluralName = "Datas";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = -1;
		MetaMediaContent.Instance.Data = relationType.RoleType; 
	}


	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ed4b710a-fe24-4143-bb96-ed1bd9beae1a"), new System.Guid("1ea9eca4-eed0-4f61-8903-c99feae961ad"), new System.Guid("f10ea049-6d24-4ca2-8efa-032fcf3000b3"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FirstName";
		relationType.RoleType.PluralName = "FirstNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.FirstName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a3e4664-bb40-4208-8e90-a1b5be323f27"), new System.Guid("9b48ff56-afef-4501-ac97-6173731ff2c9"), new System.Guid("ace04ad8-bf64-4fc3-8216-14a720d3105d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "LastName";
		relationType.RoleType.PluralName = "LastNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.LastName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("eb18bb28-da9c-47b4-a091-2f8f2303dcb6"), new System.Guid("e3a4d7b2-c5f1-4101-9aab-a0135d37a5a5"), new System.Guid("a86fc7a6-dedd-4da9-a250-75c9ec730d22"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "MiddleName";
		relationType.RoleType.PluralName = "MiddleNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.MiddleName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("adf83a86-878d-4148-a9fc-152f56697136"), new System.Guid("b9da077d-bfc7-4b4e-be62-03aded6da22e"), new System.Guid("0ffd9c62-efc6-4438-aaa3-755e4c637c21"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "BirthDate";
		relationType.RoleType.PluralName = "BirthDates";
		MetaPerson.Instance.BirthDate = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("688ebeb9-8a53-4e8d-b284-3faa0a01ef7c"), new System.Guid("8a181cec-7bae-4248-8e24-8abc7e01eea2"), new System.Guid("e431d53c-37ed-4fde-86a9-755f354c1d75"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FullName";
		relationType.RoleType.PluralName = "FullNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.FullName = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("54f11f06-8d3f-4d58-bcdc-d40e6820fdad"), new System.Guid("03a7ffcc-4291-4ae1-a2ab-69f7257fbf04"), new System.Guid("abd2a4b3-4b17-48d4-b465-0ffcb5a2664d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsStudent";
		relationType.RoleType.PluralName = "IsStudents";
		MetaPerson.Instance.IsStudent = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b3ddd2df-8a5a-4747-bd4f-1f1eb37386b3"), new System.Guid("912b48f5-215e-4cc0-a83b-56b74d986608"), new System.Guid("f6624fac-db8e-4fb2-9e86-18021b59d31d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMedia.Instance;
		relationType.RoleType.SingularName = "Photo";
		relationType.RoleType.PluralName = "Photos";
		MetaPerson.Instance.Photo = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("afc32e62-c310-421b-8c1d-6f2b0bb88b54"), new System.Guid("c21ebc52-6b32-4af7-847e-d3d7e1c4defe"), new System.Guid("0aab73c3-f997-4dd9-885a-2c1c892adb0e"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Weight";
		relationType.RoleType.PluralName = "Weights";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaPerson.Instance.Weight = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5661a98d-a935-4325-9b28-9d86175b1bd6"), new System.Guid("dec66a7b-56f5-4010-a2e7-37e25124bc77"), new System.Guid("79ffeed6-e06a-42f4-b12f-d7f7c98b6499"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CycleOne";
		relationType.RoleType.PluralName = "CycleOnes";
		MetaPerson.Instance.CycleOne = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2eb2af4f-2bf4-475f-bb41-d740197f168e"), new System.Guid("faa1e59e-29ee-4e10-bfe1-94bfbcf238ea"), new System.Guid("7ceea115-23c8-46e2-ba76-1fdb1fa85381"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CycleMany";
		relationType.RoleType.PluralName = "CycleMany";
		MetaPerson.Instance.CycleMany = relationType.RoleType; 
	}


	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9c1634ab-be99-4504-8690-ed4b39fec5bc"), new System.Guid("45a4205d-7c02-40d4-8d97-6d7d59e05def"), new System.Guid("1e051b37-cf30-43ed-a623-dd2928d6d0a3"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "DefaultLocale";
		relationType.RoleType.PluralName = "DefaultLocales";
		MetaSingleton.Instance.DefaultLocale = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9e5a3413-ed33-474f-adf2-149ad5a80719"), new System.Guid("33d5d8b9-3472-48d8-ab1a-83d00d9cb691"), new System.Guid("e75a8956-4d02-49ba-b0cf-747b7a9f350d"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "Locale";
		relationType.RoleType.PluralName = "Locales";
		MetaSingleton.Instance.Locales = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f16652b0-b712-43d7-8d4e-34a22487514d"), new System.Guid("c92466b5-55ba-496a-8880-2821f32f8f8e"), new System.Guid("3a12d798-40c3-40e0-ba9f-9d01b1e39e89"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Guest";
		relationType.RoleType.PluralName = "Guests";
		MetaSingleton.Instance.Guest = relationType.RoleType; 
	}

	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50b1be30-d6a9-49e8-84da-a47647e443f0"), new System.Guid("cb7cc442-b05b-48a5-8696-4baab7aa8cce"), new System.Guid("3ee1975d-5019-4043-be5f-65331ef58787"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "Confirmed";
		relationType.RoleType.PluralName = "Confirmeds";
		relationType.RoleType.IsRequired = true;
		MetaNotification.Instance.Confirmed = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("70292962-9e0e-4b57-a710-c8ac34f65b11"), new System.Guid("80e4d1c5-5648-486a-a2ff-3b1690514f20"), new System.Guid("84813900-abe0-4c24-bd2e-5b0d6be5ab6c"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Title";
		relationType.RoleType.PluralName = "Titles";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 1024;
		MetaNotification.Instance.Title = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e83600fc-5411-4c72-9903-80a3741a9165"), new System.Guid("1da1555c-fee6-4084-be37-57a6f58fe591"), new System.Guid("a6f6ed43-b0ab-462f-9be9-fad58d2e8398"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Description";
		relationType.RoleType.PluralName = "Descriptions";
		relationType.RoleType.Size = -1;
		MetaNotification.Instance.Description = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("458a8223-9c0f-4475-93c0-82d5cc133f1b"), new System.Guid("8d657749-a823-422b-9e29-041453ccb267"), new System.Guid("d100a845-df65-4f06-96b8-dcaeb9c3e205"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateCreated";
		relationType.RoleType.PluralName = "DateCreateds";
		relationType.RoleType.IsRequired = true;
		MetaNotification.Instance.DateCreated = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("89487904-053e-470f-bcf9-0e01165b0143"), new System.Guid("2d41d7ef-d107-404f-ac9d-fb81105d3ff7"), new System.Guid("fc089f2e-a625-40f9-bbc0-c9fc05e6e599"));
		relationType.AssociationType.ObjectType = MetaNotificationList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotification.Instance;
		relationType.RoleType.SingularName = "UnconfirmedNotification";
		relationType.RoleType.PluralName = "UnconfirmedNotifications";
		MetaNotificationList.Instance.UnconfirmedNotifications = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c32c19f1-3f41-4d11-b19d-b8b2aa360166"), new System.Guid("6e08b1d8-f7fa-452d-907a-668bf32702c1"), new System.Guid("407443f4-5afa-484e-be97-88ef19f00b32"));
		relationType.AssociationType.ObjectType = MetaTaskAssignment.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "User";
		relationType.RoleType.PluralName = "Users";
		relationType.RoleType.IsRequired = true;
		MetaTaskAssignment.Instance.User = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a01f221-480f-4d61-9a12-72e3689a8224"), new System.Guid("e5e52776-c946-42b0-99f4-596ffc1c298f"), new System.Guid("0be6c69a-1d1c-49d0-8247-fa0ff9a8f223"));
		relationType.AssociationType.ObjectType = MetaTaskAssignment.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTask.Instance;
		relationType.RoleType.SingularName = "Task";
		relationType.RoleType.PluralName = "Tasks";
		relationType.RoleType.IsRequired = true;
		MetaTaskAssignment.Instance.Task = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c3467693-cc18-46e5-a0af-d7ab0cbe9faa"), new System.Guid("7976dbaa-9b96-401f-900d-db76fd45f18f"), new System.Guid("3922d9e8-e518-4459-8b52-0723104456ab"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskAssignment.Instance;
		relationType.RoleType.SingularName = "TaskAssignment";
		relationType.RoleType.PluralName = "TaskAssignments";
		MetaTaskList.Instance.TaskAssignments = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2e7381c6-3a58-4a64-8808-4f3532254f08"), new System.Guid("63efedc3-1157-4ae0-b212-9169cd0ac418"), new System.Guid("4f83aaac-7ba1-4fdc-9ddf-781559ff3983"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskAssignment.Instance;
		relationType.RoleType.SingularName = "OpenTaskAssignment";
		relationType.RoleType.PluralName = "OpenTaskAssignments";
		MetaTaskList.Instance.OpenTaskAssignments = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bc07648e-b80c-42f6-a4dd-113fba962c89"), new System.Guid("c3b078cf-27ee-4686-b7ff-ba40a7aba5a7"), new System.Guid("ef37d700-cfa6-4998-9501-9d09bb9ac1d8"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Count";
		relationType.RoleType.PluralName = "Counts";
		MetaTaskList.Instance.Count = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("97f31053-0e7b-42a0-90c2-ce6f09c56e86"), new System.Guid("70e42b8b-09e2-4cb1-a632-ff3785ee1c8d"), new System.Guid("e5cd692c-ab97-4cf8-9f8a-1de733526e74"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "C1AllorsBinary";
		relationType.RoleType.PluralName = "C1AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaC1.Instance.C1AllorsBinary = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("20713860-8abd-4d71-8ccc-2b4d1b88bce3"), new System.Guid("974aa133-255b-431f-a15d-b6a126d362b5"), new System.Guid("6dc98925-87a7-4959-8095-90eedef0e9a0"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "C1AllorsString";
		relationType.RoleType.PluralName = "C1AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaC1.Instance.C1AllorsString = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a0ac5a65-2cbd-4c51-9417-b10150bc5699"), new System.Guid("d595765b-5e67-46f2-b19c-c58563dd1ae0"), new System.Guid("3d121ffa-0ff5-4627-9ec3-879c2830ff04"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1One2Many";
		relationType.RoleType.PluralName = "C1C1One2Manies";
		MetaC1.Instance.C1C1One2Manies = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("79c00218-bb4f-40e9-af7d-61af444a4a54"), new System.Guid("2276c942-dd96-41a6-b52f-cd3862c4692f"), new System.Guid("40ee2908-2556-4bdf-a82f-2ea33e181b91"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1One2One";
		relationType.RoleType.PluralName = "C1C1One2Ones";
		MetaC1.Instance.C1C1One2One = relationType.RoleType; 
	}


	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4058fcba-9323-47c5-b165-a3eed8de70b6"), new System.Guid("7fd58473-6579-4269-a4a1-d1bfae6b3542"), new System.Guid("dab0e0a8-712b-4278-b635-92d367f4d41a"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderVersion.Instance;
		relationType.RoleType.SingularName = "CurrentVersion";
		relationType.RoleType.PluralName = "CurrentVersions";
		MetaOrder.Instance.CurrentVersion = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("df0e52d4-07b3-45ac-9f36-2c0de9802c2f"), new System.Guid("08a55411-57f6-4015-858d-be9177328319"), new System.Guid("bf309243-98e3-457d-a396-3e6bcb06de6a"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderVersion.Instance;
		relationType.RoleType.SingularName = "AllVersion";
		relationType.RoleType.PluralName = "AllVersions";
		MetaOrder.Instance.AllVersions = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("55f3d531-c58d-4fa7-b745-9e38d8cec4c6"), new System.Guid("8b5ce991-9cc0-4419-b5a7-e2803f888a8e"), new System.Guid("7663b87d-f17d-4822-a358-546124622073"));
		relationType.AssociationType.ObjectType = MetaOrderLine.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLineVersion.Instance;
		relationType.RoleType.SingularName = "CurrentVersion";
		relationType.RoleType.PluralName = "CurrentVersions";
		MetaOrderLine.Instance.CurrentVersion = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cfc88b59-87a1-4f9e-abbe-168694ab6cb5"), new System.Guid("2ea46390-f69f-436d-bccc-84bef6cd5997"), new System.Guid("03585bb0-e87e-474f-8a76-0644d5c858f4"));
		relationType.AssociationType.ObjectType = MetaOrderLine.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLineVersion.Instance;
		relationType.RoleType.SingularName = "AllVersion";
		relationType.RoleType.PluralName = "AllVersions";
		MetaOrderLine.Instance.AllVersions = relationType.RoleType; 
	}





	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("49b96f79-c33d-4847-8c64-d50a6adb4985"), new System.Guid("b031ef1a-0102-4b19-b85d-aa9c404596c3"), new System.Guid("b95c7b34-a295-4600-82c8-826cc2186a00"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Employee";
		relationType.RoleType.PluralName = "Employees";
		MetaOrganisation.Instance.Employees = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("dbef262d-7184-4b98-8f1f-cf04e884bb92"), new System.Guid("ed76a631-00c4-4753-b3d4-b3a53b9ecf4a"), new System.Guid("19de0627-fb1c-4f55-9b65-31d8008d0a48"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Manager";
		relationType.RoleType.PluralName = "Managers";
		MetaOrganisation.Instance.Manager = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2cc74901-cda5-4185-bcd8-d51c745a8437"), new System.Guid("896a4589-4caf-4cd2-8365-c4200b12f519"), new System.Guid("baa30557-79ff-406d-b374-9d32519b2de7"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaOrganisation.Instance.Name = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("845ff004-516f-4ad5-9870-3d0e966a9f7d"), new System.Guid("3820f65f-0e79-4f30-a973-5d17dca6ad33"), new System.Guid("58d7df91-fbc5-4bcb-9398-a9957949402b"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Owner";
		relationType.RoleType.PluralName = "Owners";
		MetaOrganisation.Instance.Owner = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("15f33fa4-c878-45a0-b40c-c5214bce350b"), new System.Guid("4fdd9abb-f2e7-4f07-860e-27b4207224bd"), new System.Guid("45bef644-dfcf-417a-9356-3c1cfbcada1b"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Shareholder";
		relationType.RoleType.PluralName = "Shareholders";
		MetaOrganisation.Instance.Shareholders = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d3db6e8c-9c10-47ba-92b1-45f5ddffa5cc"), new System.Guid("4955ac7f-f840-4f24-b44c-c2d3937d2d44"), new System.Guid("9033ae73-83f6-4529-9f81-84fd9d35d597"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "CycleOne";
		relationType.RoleType.PluralName = "CycleOnes";
		MetaOrganisation.Instance.CycleOne = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c6cca1c5-5799-4517-87f5-095da0eeec64"), new System.Guid("6abcd4e2-44a7-46b4-bd98-d052f38b7c50"), new System.Guid("e01ace3c-2314-477c-8997-14266d9711e0"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "CycleMany";
		relationType.RoleType.PluralName = "CycleMany";
		MetaOrganisation.Instance.CycleMany = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("24771d5b-f920-4820-aff7-ea6391b4a45c"), new System.Guid("fe3aa333-e011-4a1e-85dc-ded48329cf00"), new System.Guid("4d4428fc-bac0-47af-ab5e-7c7b87880206"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "AllorsBinary";
		relationType.RoleType.PluralName = "AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaUnitSample.Instance.AllorsBinary = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4d6a80f5-0fa7-4867-91f8-37aa92b6707b"), new System.Guid("13f88cf7-aaec-48a1-a896-401df84da34b"), new System.Guid("a462ce40-5885-48c6-b327-7e4c096a99fa"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "AllorsDateTime";
		relationType.RoleType.PluralName = "AllorsDateTimes";
		MetaUnitSample.Instance.AllorsDateTime = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5a788ebe-65e9-4d5e-853a-91bb4addabb5"), new System.Guid("7620281d-3d8a-470a-9258-7a6d1b818b46"), new System.Guid("b5dd13eb-8923-4a66-94df-af5fadb42f1c"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "AllorsBoolean";
		relationType.RoleType.PluralName = "AllorsBooleans";
		MetaUnitSample.Instance.AllorsBoolean = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("74a35820-ef8c-4373-9447-6215ee8279c0"), new System.Guid("e5f7a565-372a-42ed-8da5-ffe6dd599f70"), new System.Guid("4a95fb0d-6849-499e-a140-6c942fb06f4d"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "AllorsDouble";
		relationType.RoleType.PluralName = "AllorsDoubles";
		MetaUnitSample.Instance.AllorsDouble = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b817ba76-876e-44ea-8e5a-51d552d4045e"), new System.Guid("80683240-71d5-4329-abd0-87c367b44fec"), new System.Guid("07070cb0-6e65-4a00-8754-50cf594ed9e1"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "AllorsInteger";
		relationType.RoleType.PluralName = "AllorsIntegers";
		MetaUnitSample.Instance.AllorsInteger = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c724c733-972a-411c-aecb-e865c2628a90"), new System.Guid("e4917fda-a605-4f6f-8f63-579ec688b629"), new System.Guid("f27c150a-ce8d-4ff3-9507-ccb0b91aa0c2"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsString";
		relationType.RoleType.PluralName = "AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaUnitSample.Instance.AllorsString = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ed58ae4c-24e0-4dd1-8b1c-0909df1e0fcd"), new System.Guid("f117e164-ce37-4c12-a79e-38cda962adae"), new System.Guid("25dd4abf-c6da-4739-aed0-8528d1c00b8b"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "AllorsUnique";
		relationType.RoleType.PluralName = "AllorsUniques";
		MetaUnitSample.Instance.AllorsUnique = relationType.RoleType; 
	}
	{
		// TODO: Groups
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f746da51-ea2d-4e22-9ecb-46d4dbc1b084"), new System.Guid("3936ee9b-3bd6-44de-9340-4047749a6c2c"), new System.Guid("1408cd42-3125-48c7-86d7-4a5f71e75e25"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "AllorsDecimal";
		relationType.RoleType.PluralName = "AllorsDecimals";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaUnitSample.Instance.AllorsDecimal = relationType.RoleType; 
	}

			// Inherited Roles
            MetaEnumeration.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;

            MetaObjectState.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;

            MetaUser.Instance.Locale = MetaLocalised.Instance.Locale;
            MetaTask.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;



			// Implemented Roles
            MetaLocalisedText.Instance.Locale = MetaLocalisedText.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];

            MetaCounter.Instance.UniqueId = MetaCounter.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];





            MetaMedia.Instance.UniqueId = MetaMedia.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];

            MetaAutomatedAgent.Instance.UserName = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserName];
            MetaAutomatedAgent.Instance.NormalizedUserName = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NormalizedUserName];
            MetaAutomatedAgent.Instance.UserEmail = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmail];
            MetaAutomatedAgent.Instance.UserEmailConfirmed = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmailConfirmed];
            MetaAutomatedAgent.Instance.TaskList = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.TaskList];
            MetaAutomatedAgent.Instance.NotificationList = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NotificationList];
            MetaAutomatedAgent.Instance.Locale = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];

            MetaPerson.Instance.UserName = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserName];
            MetaPerson.Instance.NormalizedUserName = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NormalizedUserName];
            MetaPerson.Instance.UserEmail = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmail];
            MetaPerson.Instance.UserEmailConfirmed = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmailConfirmed];
            MetaPerson.Instance.TaskList = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.TaskList];
            MetaPerson.Instance.NotificationList = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NotificationList];
            MetaPerson.Instance.Locale = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];
            MetaPerson.Instance.UniqueId = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaRole.Instance.UniqueId = MetaRole.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];


            MetaUserGroup.Instance.UniqueId = MetaUserGroup.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];




            MetaC1.Instance.I1AllorsString = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsString];

            MetaGender.Instance.LocalisedNames = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.LocalisedNames];
            MetaGender.Instance.Name = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.Name];
            MetaGender.Instance.IsActive = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.IsActive];
            MetaGender.Instance.UniqueId = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];


            MetaOrderLineVersion.Instance.DerivationTimeStamp = MetaOrderLineVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationTimeStamp];
            MetaPaymentState.Instance.Name = MetaPaymentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaPaymentState.Instance.UniqueId = MetaPaymentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaShipmentState.Instance.Name = MetaShipmentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaShipmentState.Instance.UniqueId = MetaShipmentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaOrderState.Instance.Name = MetaOrderState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaOrderState.Instance.UniqueId = MetaOrderState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaOrderVersion.Instance.DerivationTimeStamp = MetaOrderVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationTimeStamp];
            MetaOrganisation.Instance.UniqueId = MetaOrganisation.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];


			// Associations
            MetaUser.Instance.GuestSingleton = MetaSingleton.Instance.Guest.AssociationType;
            MetaUser.Instance.UserTaskAssignments = MetaTaskAssignment.Instance.User.AssociationType;
            MetaTask.Instance.TaskTaskAssignments = MetaTaskAssignment.Instance.Task.AssociationType;
            MetaWorkItem.Instance.WorkItemTasks = MetaTask.Instance.WorkItem.AssociationType;

            MetaLocalisedText.Instance.LocalisedNameCountry = MetaCountry.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.LocalisedNameCurrency = MetaCurrency.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.LocalisedNameLanguage = MetaLanguage.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.LocalisedNameEnumeration = MetaEnumeration.Instance.LocalisedNames.AssociationType;


            MetaCountry.Instance.CountryLocales = MetaLocale.Instance.Country.AssociationType;
            MetaCurrency.Instance.CurrencyCountries = MetaCountry.Instance.Currency.AssociationType;
            MetaLanguage.Instance.LanguageLocales = MetaLocale.Instance.Language.AssociationType;
            MetaLocale.Instance.DefaultLocaleSingletons = MetaSingleton.Instance.DefaultLocale.AssociationType;
            MetaLocale.Instance.LocaleSingleton = MetaSingleton.Instance.Locales.AssociationType;
            MetaLocale.Instance.LocaleLocaliseds = MetaLocalised.Instance.Locale.AssociationType;

            MetaMedia.Instance.PhotoPeople = MetaPerson.Instance.Photo.AssociationType;
            MetaMediaContent.Instance.MediaContentMedia = MetaMedia.Instance.MediaContent.AssociationType;


            MetaPerson.Instance.EmployeeOrganisation = MetaOrganisation.Instance.Employees.AssociationType;
            MetaPerson.Instance.ManagerOrganisation = MetaOrganisation.Instance.Manager.AssociationType;
            MetaPerson.Instance.OwnerOrganisations = MetaOrganisation.Instance.Owner.AssociationType;
            MetaPerson.Instance.ShareholderOrganisations = MetaOrganisation.Instance.Shareholders.AssociationType;
            MetaPerson.Instance.CycleOneOrganisations = MetaOrganisation.Instance.CycleOne.AssociationType;
            MetaPerson.Instance.CycleManyOrganisations = MetaOrganisation.Instance.CycleMany.AssociationType;
            MetaPerson.Instance.ParticipantTasks = MetaTask.Instance.Participants.AssociationType;
            MetaPerson.Instance.PerformerTasks = MetaTask.Instance.Performer.AssociationType;




            MetaNotification.Instance.UnconfirmedNotificationNotificationList = MetaNotificationList.Instance.UnconfirmedNotifications.AssociationType;
            MetaNotificationList.Instance.NotificationListUser = MetaUser.Instance.NotificationList.AssociationType;
            MetaTaskAssignment.Instance.TaskAssignmentTaskList = MetaTaskList.Instance.TaskAssignments.AssociationType;
            MetaTaskAssignment.Instance.OpenTaskAssignmentTaskList = MetaTaskList.Instance.OpenTaskAssignments.AssociationType;
            MetaTaskList.Instance.TaskListUser = MetaUser.Instance.TaskList.AssociationType;
            MetaC1.Instance.C1C1One2ManyC1 = MetaC1.Instance.C1C1One2Manies.AssociationType;
            MetaC1.Instance.C1C1One2OneC1 = MetaC1.Instance.C1C1One2One.AssociationType;




            MetaOrderLineVersion.Instance.CurrentVersionOrderLine = MetaOrderLine.Instance.CurrentVersion.AssociationType;
            MetaOrderLineVersion.Instance.AllVersionOrderLine = MetaOrderLine.Instance.AllVersions.AssociationType;



            MetaOrderVersion.Instance.CurrentVersionOrder = MetaOrder.Instance.CurrentVersion.AssociationType;
            MetaOrderVersion.Instance.AllVersionOrder = MetaOrder.Instance.AllVersions.AssociationType;
            MetaOrganisation.Instance.CycleOnePeople = MetaPerson.Instance.CycleOne.AssociationType;
            MetaOrganisation.Instance.CycleManyPeople = MetaPerson.Instance.CycleMany.AssociationType;


			// InheritedAssociations
            MetaAutomatedAgent.Instance.GuestSingleton = MetaSingleton.Instance.Guest.AssociationType;
            MetaAutomatedAgent.Instance.UserTaskAssignments = MetaTaskAssignment.Instance.User.AssociationType;

            MetaPerson.Instance.GuestSingleton = MetaSingleton.Instance.Guest.AssociationType;
            MetaPerson.Instance.UserTaskAssignments = MetaTaskAssignment.Instance.User.AssociationType;





















			// Defined Methods
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("430702d2-e02b-45ad-9b22-b8331dc75a3f"));
				method.ObjectType = MetaDeletable.Instance;
				method.Name = "Delete";
				// TODO: Group and XmlDoc
				MetaDeletable.Instance.Delete = method; 
			}





























			{
				var method = new MethodType(this.metaPopulation, new System.Guid("09a6a387-a1b5-4038-b074-3a01c81cbda2"));
				method.ObjectType = MetaC1.Instance;
				method.Name = "ClassMethod";
				// TODO: Group and XmlDoc
				MetaC1.Instance.ClassMethod = method; 
			}









			{
				var method = new MethodType(this.metaPopulation, new System.Guid("1869873f-f2f0-4d03-a0f9-7dc73491c117"));
				method.ObjectType = MetaOrganisation.Instance;
				method.Name = "JustDoIt";
				// TODO: Group and XmlDoc
				MetaOrganisation.Instance.JustDoIt = method; 
			}


			// Inherited Methods
				MetaTask.Instance.Delete = MetaDeletable.Instance.Delete;



				MetaAccessControl.Instance.Delete = MetaDeletable.Instance.Delete;





				MetaLogin.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaMedia.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaMediaContent.Instance.Delete = MetaDeletable.Instance.Delete;

				MetaPermission.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaPerson.Instance.Delete = MetaDeletable.Instance.Delete;

				MetaSecurityToken.Instance.Delete = MetaDeletable.Instance.Delete;



				MetaNotificationList.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaTaskAssignment.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaTaskList.Instance.Delete = MetaDeletable.Instance.Delete;

				MetaDependent.Instance.Delete = MetaDeletable.Instance.Delete;








				MetaOrganisation.Instance.Delete = MetaDeletable.Instance.Delete;




			// Extend
            MetaVersion.Instance.Extend();
            MetaDeletable.Instance.Extend();
            MetaEnumeration.Instance.Extend();
            MetaLocalised.Instance.Extend();
            MetaObjectState.Instance.Extend();
            MetaUniquelyIdentifiable.Instance.Extend();
            MetaUser.Instance.Extend();
            MetaTask.Instance.Extend();
            MetaWorkItem.Instance.Extend();
            MetaI1.Instance.Extend();

            MetaLocalisedText.Instance.Extend();
            MetaAccessControl.Instance.Extend();
            MetaCounter.Instance.Extend();
            MetaCountry.Instance.Extend();
            MetaCurrency.Instance.Extend();
            MetaLanguage.Instance.Extend();
            MetaLocale.Instance.Extend();
            MetaLogin.Instance.Extend();
            MetaMedia.Instance.Extend();
            MetaMediaContent.Instance.Extend();
            MetaAutomatedAgent.Instance.Extend();
            MetaPermission.Instance.Extend();
            MetaPerson.Instance.Extend();
            MetaRole.Instance.Extend();
            MetaSecurityToken.Instance.Extend();
            MetaSingleton.Instance.Extend();
            MetaUserGroup.Instance.Extend();
            MetaNotification.Instance.Extend();
            MetaNotificationList.Instance.Extend();
            MetaTaskAssignment.Instance.Extend();
            MetaTaskList.Instance.Extend();
            MetaC1.Instance.Extend();
            MetaDependent.Instance.Extend();
            MetaGender.Instance.Extend();
            MetaOrder.Instance.Extend();
            MetaOrderLine.Instance.Extend();
            MetaOrderLineVersion.Instance.Extend();
            MetaPaymentState.Instance.Extend();
            MetaShipmentState.Instance.Extend();
            MetaOrderState.Instance.Extend();
            MetaOrderVersion.Instance.Extend();
            MetaOrganisation.Instance.Extend();
            MetaUnitSample.Instance.Extend();
		}
	}
}