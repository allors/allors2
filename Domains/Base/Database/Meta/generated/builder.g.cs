namespace Allors.Meta
{
	internal class MetaBuilder
	{
	    private readonly MetaPopulation metaPopulation;

        internal MetaBuilder(MetaPopulation metaPopulation)
		{
			this.metaPopulation = metaPopulation;
		}

		internal void BuildDomains()
		{
            MetaCore.Instance = new MetaCore(this.metaPopulation);
            MetaBase.Instance = new MetaBase(this.metaPopulation);
            MetaCustom.Instance = new MetaCustom(this.metaPopulation);
		}

		internal void BuildDomainInheritances()
		{
            MetaBase.Instance.Domain.AddDirectSuperdomain(MetaCore.Instance.Domain);
            MetaCustom.Instance.Domain.AddDirectSuperdomain(MetaBase.Instance.Domain);
		}

		internal void BuildUnits()
		{
            MetaBinary.Instance = new MetaBinary(this.metaPopulation);
            MetaBoolean.Instance = new MetaBoolean(this.metaPopulation);
            MetaDateTime.Instance = new MetaDateTime(this.metaPopulation);
            MetaDecimal.Instance = new MetaDecimal(this.metaPopulation);
            MetaFloat.Instance = new MetaFloat(this.metaPopulation);
            MetaInteger.Instance = new MetaInteger(this.metaPopulation);
            MetaString.Instance = new MetaString(this.metaPopulation);
            MetaUnique.Instance = new MetaUnique(this.metaPopulation);
		}

		internal void BuildInterfaces()
		{
            MetaObject.Instance = new MetaObject(this.metaPopulation);
            MetaCachable.Instance = new MetaCachable(this.metaPopulation);
            MetaVersion.Instance = new MetaVersion(this.metaPopulation);
            MetaVersioned.Instance = new MetaVersioned(this.metaPopulation);
            MetaAccessControlledObject.Instance = new MetaAccessControlledObject(this.metaPopulation);
            MetaDeletable.Instance = new MetaDeletable(this.metaPopulation);
            MetaEnumeration.Instance = new MetaEnumeration(this.metaPopulation);
            MetaLocalised.Instance = new MetaLocalised(this.metaPopulation);
            MetaObjectState.Instance = new MetaObjectState(this.metaPopulation);
            MetaSecurityTokenOwner.Instance = new MetaSecurityTokenOwner(this.metaPopulation);
            MetaTransitionalVersion.Instance = new MetaTransitionalVersion(this.metaPopulation);
            MetaTransitional.Instance = new MetaTransitional(this.metaPopulation);
            MetaUniquelyIdentifiable.Instance = new MetaUniquelyIdentifiable(this.metaPopulation);
            MetaUser.Instance = new MetaUser(this.metaPopulation);
            MetaTask.Instance = new MetaTask(this.metaPopulation);
            MetaWorkItem.Instance = new MetaWorkItem(this.metaPopulation);
            MetaAddress.Instance = new MetaAddress(this.metaPopulation);
            MetaI1.Instance = new MetaI1(this.metaPopulation);
            MetaI12.Instance = new MetaI12(this.metaPopulation);
            MetaI2.Instance = new MetaI2(this.metaPopulation);
            MetaS1.Instance = new MetaS1(this.metaPopulation);
            MetaShared.Instance = new MetaShared(this.metaPopulation);
            MetaSyncDepth1.Instance = new MetaSyncDepth1(this.metaPopulation);
            MetaSyncDepth2.Instance = new MetaSyncDepth2(this.metaPopulation);
            MetaSyncRoot.Instance = new MetaSyncRoot(this.metaPopulation);
            MetaValidationI12.Instance = new MetaValidationI12(this.metaPopulation);
		}

		internal void BuildClasses()
		{
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
            MetaEmailMessage.Instance = new MetaEmailMessage(this.metaPopulation);
            MetaNotification.Instance = new MetaNotification(this.metaPopulation);
            MetaNotificationList.Instance = new MetaNotificationList(this.metaPopulation);
            MetaTaskAssignment.Instance = new MetaTaskAssignment(this.metaPopulation);
            MetaTaskList.Instance = new MetaTaskList(this.metaPopulation);
            MetaBuild.Instance = new MetaBuild(this.metaPopulation);
            MetaBadUI.Instance = new MetaBadUI(this.metaPopulation);
            MetaC1.Instance = new MetaC1(this.metaPopulation);
            MetaC2.Instance = new MetaC2(this.metaPopulation);
            MetaClassWithoutRoles.Instance = new MetaClassWithoutRoles(this.metaPopulation);
            MetaDependee.Instance = new MetaDependee(this.metaPopulation);
            MetaDependent.Instance = new MetaDependent(this.metaPopulation);
            MetaExtender.Instance = new MetaExtender(this.metaPopulation);
            MetaFirst.Instance = new MetaFirst(this.metaPopulation);
            MetaFour.Instance = new MetaFour(this.metaPopulation);
            MetaFrom.Instance = new MetaFrom(this.metaPopulation);
            MetaGender.Instance = new MetaGender(this.metaPopulation);
            MetaHomeAddress.Instance = new MetaHomeAddress(this.metaPopulation);
            MetaMailboxAddress.Instance = new MetaMailboxAddress(this.metaPopulation);
            MetaOne.Instance = new MetaOne(this.metaPopulation);
            MetaOrder.Instance = new MetaOrder(this.metaPopulation);
            MetaOrderLine.Instance = new MetaOrderLine(this.metaPopulation);
            MetaOrderLineVersion.Instance = new MetaOrderLineVersion(this.metaPopulation);
            MetaPaymentState.Instance = new MetaPaymentState(this.metaPopulation);
            MetaShipmentState.Instance = new MetaShipmentState(this.metaPopulation);
            MetaOrderState.Instance = new MetaOrderState(this.metaPopulation);
            MetaOrderVersion.Instance = new MetaOrderVersion(this.metaPopulation);
            MetaOrganisation.Instance = new MetaOrganisation(this.metaPopulation);
            MetaPlace.Instance = new MetaPlace(this.metaPopulation);
            MetaSecond.Instance = new MetaSecond(this.metaPopulation);
            MetaSimpleJob.Instance = new MetaSimpleJob(this.metaPopulation);
            MetaStatefulCompany.Instance = new MetaStatefulCompany(this.metaPopulation);
            MetaSubdependee.Instance = new MetaSubdependee(this.metaPopulation);
            MetaThird.Instance = new MetaThird(this.metaPopulation);
            MetaThree.Instance = new MetaThree(this.metaPopulation);
            MetaTo.Instance = new MetaTo(this.metaPopulation);
            MetaTwo.Instance = new MetaTwo(this.metaPopulation);
            MetaUnitSample.Instance = new MetaUnitSample(this.metaPopulation);
            MetaValidationC1.Instance = new MetaValidationC1(this.metaPopulation);
            MetaValidationC2.Instance = new MetaValidationC2(this.metaPopulation);
		}

		internal void BuildInheritances()
		{
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLocalisedText.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLocalisedText.Instance.ObjectType, Supertype = MetaLocalised.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAccessControl.Instance.ObjectType, Supertype = MetaCachable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAccessControl.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAccessControl.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaCounter.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaCountry.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaCurrency.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLanguage.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLocale.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLogin.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMedia.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMedia.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMedia.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMediaContent.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMediaContent.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAutomatedAgent.Instance.ObjectType, Supertype = MetaUser.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPermission.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPermission.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaUser.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPerson.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaRole.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaRole.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSecurityToken.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSingleton.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUserGroup.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUserGroup.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaEmailMessage.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaNotification.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaNotificationList.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaNotificationList.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTaskAssignment.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTaskAssignment.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTaskList.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaBuild.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaBadUI.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaC1.Instance.ObjectType, Supertype = MetaI1.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaC1.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaC2.Instance.ObjectType, Supertype = MetaI2.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaClassWithoutRoles.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaDependee.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaDependent.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaExtender.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaFirst.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaFour.Instance.ObjectType, Supertype = MetaShared.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaFrom.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaGender.Instance.ObjectType, Supertype = MetaEnumeration.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaHomeAddress.Instance.ObjectType, Supertype = MetaAddress.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaMailboxAddress.Instance.ObjectType, Supertype = MetaAddress.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOne.Instance.ObjectType, Supertype = MetaShared.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrder.Instance.ObjectType, Supertype = MetaTransitional.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrder.Instance.ObjectType, Supertype = MetaVersioned.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderLine.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderLine.Instance.ObjectType, Supertype = MetaVersioned.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderLineVersion.Instance.ObjectType, Supertype = MetaVersion.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPaymentState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaShipmentState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderState.Instance.ObjectType, Supertype = MetaObjectState.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrderVersion.Instance.ObjectType, Supertype = MetaVersion.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrganisation.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrganisation.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaOrganisation.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaPlace.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSecond.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSimpleJob.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaStatefulCompany.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSubdependee.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaThird.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaThree.Instance.ObjectType, Supertype = MetaShared.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTo.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTwo.Instance.ObjectType, Supertype = MetaShared.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUnitSample.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaValidationC1.Instance.ObjectType, Supertype = MetaValidationI12.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaValidationC2.Instance.ObjectType, Supertype = MetaValidationI12.Instance.Interface};

            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaCachable.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaVersion.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaVersioned.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAccessControlledObject.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaDeletable.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaEnumeration.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaEnumeration.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaLocalised.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaObjectState.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSecurityTokenOwner.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTransitionalVersion.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTransitional.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUniquelyIdentifiable.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUser.Instance.ObjectType, Supertype = MetaSecurityTokenOwner.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUser.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaUser.Instance.ObjectType, Supertype = MetaLocalised.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTask.Instance.ObjectType, Supertype = MetaAccessControlledObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTask.Instance.ObjectType, Supertype = MetaUniquelyIdentifiable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaTask.Instance.ObjectType, Supertype = MetaDeletable.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaWorkItem.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaAddress.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaI1.Instance.ObjectType, Supertype = MetaI12.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaI1.Instance.ObjectType, Supertype = MetaS1.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaI12.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaI2.Instance.ObjectType, Supertype = MetaI12.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaS1.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaShared.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSyncDepth1.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSyncDepth2.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaSyncRoot.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
            new Inheritance(this.metaPopulation){ Subtype = (Composite)MetaValidationI12.Instance.ObjectType, Supertype = MetaObject.Instance.Interface};
		}

		internal void BuildRoles()
		{
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50dc85f0-3d22-4bc1-95d9-153674b89f7a"), new System.Guid("accd061b-20b9-4a24-bb2c-c2f7276f43ab"), new System.Guid("8d3f68e1-fa6e-414f-aa4d-25fcc2c975d6"));
		relationType.AssociationType.ObjectType = MetaLocalisedText.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Text";
		relationType.RoleType.PluralName = "Texts";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = -1;
		MetaLocalisedText.Instance.Text = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0dbbff5c-3dca-4257-b2da-442d263dcd86"), new System.Guid("92e8d639-9205-422b-b4ff-c7e8c2980abf"), new System.Guid("2d9b7157-5409-40d3-bd3e-6911df9aface"));
		relationType.AssociationType.ObjectType = MetaAccessControl.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUserGroup.Instance;
		relationType.RoleType.SingularName = "SubjectGroup";
		relationType.RoleType.PluralName = "SubjectGroups";
		MetaAccessControl.Instance.SubjectGroups = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("37dd1e27-ba75-404c-9410-c6399d28317c"), new System.Guid("3d74101d-97bc-46fb-9548-96cb7aa01b39"), new System.Guid("e0303a17-bf7a-4a7f-bb4b-0a447c56aaaf"));
		relationType.AssociationType.ObjectType = MetaAccessControl.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Subject";
		relationType.RoleType.PluralName = "Subjects";
		MetaAccessControl.Instance.Subjects = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("69a9dae8-678d-4c1c-a464-2e5aa5caf39e"), new System.Guid("ec79e57d-be81-430a-b12f-08ffd1e94af3"), new System.Guid("370d3d12-3164-4753-8a72-1c604bda1b64"));
		relationType.AssociationType.ObjectType = MetaAccessControl.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaRole.Instance;
		relationType.RoleType.SingularName = "Role";
		relationType.RoleType.PluralName = "Roles";
		relationType.RoleType.IsRequired = true;
		MetaAccessControl.Instance.Role = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5e218f37-3b07-4002-87a4-7581a53f01ba"), new System.Guid("be94d5f0-df53-4118-987a-11bce8593a1b"), new System.Guid("46d65185-9e0d-4347-a38f-6afa2431c241"));
		relationType.AssociationType.ObjectType = MetaAccessControl.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPermission.Instance;
		relationType.RoleType.SingularName = "EffectivePermission";
		relationType.RoleType.PluralName = "EffectivePermissions";
		MetaAccessControl.Instance.EffectivePermissions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50ecae85-e5a9-467e-99a3-78703d954b2f"), new System.Guid("01590aea-d75c-45be-af4b-bf56545a4008"), new System.Guid("bac6c53c-e103-42cb-b36d-2faa00ebf574"));
		relationType.AssociationType.ObjectType = MetaAccessControl.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "EffectiveUser";
		relationType.RoleType.PluralName = "EffectiveUsers";
		MetaAccessControl.Instance.EffectiveUsers = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("309d07d9-8dea-4e99-a3b8-53c0d360bc54"), new System.Guid("0c807020-5397-4cdb-8380-52899b7af6b7"), new System.Guid("ab60f6a7-d913-4377-ab47-97f0fb9d8f3b"));
		relationType.AssociationType.ObjectType = MetaCounter.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Value";
		relationType.RoleType.PluralName = "Values";
		relationType.RoleType.IsRequired = true;
		MetaCounter.Instance.Value = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("62009cef-7424-4ec0-8953-e92b3cd6639d"), new System.Guid("323173ee-385c-4f74-8b78-ff05735460f8"), new System.Guid("4ca5a640-5d9e-4910-95ed-6872c7ea13d2"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaCurrency.Instance;
		relationType.RoleType.SingularName = "Currency";
		relationType.RoleType.PluralName = "Currencies";
		MetaCountry.Instance.Currency = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f93acc4e-f89e-4610-ada9-e58f21c165bc"), new System.Guid("ea0efe67-89f2-4317-97e7-f0e14358e718"), new System.Guid("4fe997d6-d221-432b-9f09-4f77735c109b"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 2;
		MetaCountry.Instance.IsoCode = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6b9c977f-b394-440e-9781-5d56733b60da"), new System.Guid("6e3532ae-3528-4114-9274-54fc08effd0d"), new System.Guid("60f1f9a3-13d1-485f-bc77-fda1f9ef1815"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaCountry.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8236a702-a76d-4bb5-9afd-acacb1508261"), new System.Guid("9b682612-50f9-43f3-abde-4d0cb5156f0d"), new System.Guid("99c52c13-ef50-4f68-a32f-fef660aa3044"));
		relationType.AssociationType.ObjectType = MetaCountry.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaCountry.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("294a4bdc-f03a-47a2-a649-419e6b9021a3"), new System.Guid("f9eec7c6-c4cd-4d8c-a5f7-44855328cd7e"), new System.Guid("09d74027-4457-4788-803c-24b857245565"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaCurrency.Instance.IsoCode = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("74c8308b-1b76-4218-9532-f01c9d1e146b"), new System.Guid("2cb43671-c648-4bd4-ac08-7302c29246e7"), new System.Guid("e7c93764-d634-4187-97ed-9248ea56bab2"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaCurrency.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e9fc0472-cf7a-4e02-b061-cb42b6f5c273"), new System.Guid("06b8f2b2-91f0-4b89-ae19-b47de4524556"), new System.Guid("e1301b8f-25cc-4ace-884e-79af1d303f53"));
		relationType.AssociationType.ObjectType = MetaCurrency.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaCurrency.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d2a32d9f-21cc-4f9d-b0d3-a9b75da66907"), new System.Guid("6c860e73-d12e-4e35-897e-ed9f8fd8eba0"), new System.Guid("84f904a6-8dcc-4089-bda6-34325ade6367"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "IsoCode";
		relationType.RoleType.PluralName = "IsoCodes";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.IsoCode = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("be482902-beb5-4a76-8ad0-c1b1c1c0e5c4"), new System.Guid("d3369fa9-afb7-4d5a-b476-3e4d43cce0fd"), new System.Guid("308d73b0-1b65-40a9-88f1-288848849c51"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("842CC899-3F37-455A-AE91-51D29D615E69"), new System.Guid("F3C7A07E-A2F3-4A96-91EF-D53DDF009DD4"), new System.Guid("C78E1736-74B4-4574-A365-421DCADF4D4C"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "NativeName";
		relationType.RoleType.PluralName = "NativeNames";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaLanguage.Instance.NativeName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f091b264-e6b1-4a57-bbfb-8225cbe8190c"), new System.Guid("6650af3b-f537-4c2f-afff-6773552315cd"), new System.Guid("5e9fcced-727d-42a2-95e6-a0f9d8be4ec7"));
		relationType.AssociationType.ObjectType = MetaLanguage.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaLanguage.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2a2c6f77-e6a2-4eab-bfe3-5d35a8abd7f7"), new System.Guid("09422255-fa17-41d8-991b-d21d7b37c6c5"), new System.Guid("647db2b3-997b-4c3a-9ae2-d49b410933c1"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaLocale.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d8cac34a-9bb2-4190-bd2a-ec0b87e04cf5"), new System.Guid("af501892-3c83-41d1-826b-f5c4cb1de7fe"), new System.Guid("ed32b12a-00ad-420b-9dfa-f1c6ce773fcd"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLanguage.Instance;
		relationType.RoleType.SingularName = "Language";
		relationType.RoleType.PluralName = "Languages";
		relationType.RoleType.IsRequired = true;
		MetaLocale.Instance.Language = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ea778b77-2929-4ab4-ad99-bf2f970401a9"), new System.Guid("bb5904f5-feb0-47eb-903a-0351d55f0d8c"), new System.Guid("b2fc6e06-3881-427e-b4cc-8457a65f8076"));
		relationType.AssociationType.ObjectType = MetaLocale.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaCountry.Instance;
		relationType.RoleType.SingularName = "Country";
		relationType.RoleType.PluralName = "Countries";
		relationType.RoleType.IsRequired = true;
		MetaLocale.Instance.Country = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("18262218-a14f-48c3-87a5-87196d3b5974"), new System.Guid("3f067cf5-4fcb-4be4-9afb-15ba37700658"), new System.Guid("e5393717-f46c-4a4c-a87f-3e4684428860"));
		relationType.AssociationType.ObjectType = MetaLogin.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Key";
		relationType.RoleType.PluralName = "Keys";
		relationType.RoleType.Size = 256;
		MetaLogin.Instance.Key = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7a82e721-d0b7-4567-aaef-bd3987ae6d01"), new System.Guid("2f2ef41d-8310-45fd-8ab5-e5d067862e3d"), new System.Guid("c8e3851a-bc57-4acb-934a-4adfc37b1da7"));
		relationType.AssociationType.ObjectType = MetaLogin.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Provider";
		relationType.RoleType.PluralName = "Providers";
		relationType.RoleType.Size = 256;
		MetaLogin.Instance.Provider = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c2d950ad-39d3-40f1-8817-11a026e9890b"), new System.Guid("e8091111-9f92-41a9-b4b1-4e8f277ea575"), new System.Guid("150daf84-13ce-4b5f-83e6-64c7ef4f81c6"));
		relationType.AssociationType.ObjectType = MetaLogin.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "User";
		relationType.RoleType.PluralName = "Users";
		MetaLogin.Instance.User = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("B74C2159-739A-4F1C-ADA7-C2DCC3CDCF83"), new System.Guid("96B21673-F124-4C30-A2F0-DF56D29E03F5"), new System.Guid("DE0FE224-C40D-469C-BDC5-849A7412EFEC"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "Revision";
		relationType.RoleType.PluralName = "Revisions";
		MetaMedia.Instance.Revision = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("67082a51-1502-490b-b8db-537799e550bd"), new System.Guid("e8537dcf-1bd7-46c4-a37c-077bee6a78a1"), new System.Guid("02fe1ce8-c019-4a40-bd6f-b38d2f47a288"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMediaContent.Instance;
		relationType.RoleType.SingularName = "MediaContent";
		relationType.RoleType.PluralName = "MediaContents";
		relationType.RoleType.IsRequired = true;
		MetaMedia.Instance.MediaContent = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("18236718-1835-430C-A936-7EC461EEE2CF"), new System.Guid("8A79E6C5-4BAE-468D-B57C-C7788D3E21E3"), new System.Guid("877ABDC8-8915-4640-8871-8CEF7EF69072"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "InData";
		relationType.RoleType.PluralName = "InDatas";
		relationType.RoleType.Size = -1;
		MetaMedia.Instance.InData = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("79B04065-F13B-43B3-B86E-F3ADBBAAF0C4"), new System.Guid("287B7291-39F0-43E5-8770-811940E81BAE"), new System.Guid("CE17BFC7-5A4E-415A-9AE0-FAE429CEE69C"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "InDataUri";
		relationType.RoleType.PluralName = "InDataUris";
		relationType.RoleType.Size = -1;
		MetaMedia.Instance.InDataUri = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("DDD6C005-0104-44CA-A19C-1150B8BEB4A3"), new System.Guid("4F43B520-404E-436D-A514-71E4AEC55EC8"), new System.Guid("4C4EC21C-A3C0-4720-92E0-CF6532000265"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FileName";
		relationType.RoleType.PluralName = "FileNames";
		relationType.RoleType.Size = 256;
		MetaMedia.Instance.FileName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("29541613-0B16-49AD-8F40-3309A7C7D7B8"), new System.Guid("EFB76140-4A2A-4E7F-B51D-C95BCA774664"), new System.Guid("7CFC8B40-5199-4457-BBBD-27A786721465"));
		relationType.AssociationType.ObjectType = MetaMedia.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Type";
		relationType.RoleType.PluralName = "Types";
		relationType.RoleType.Size = 1024;
		MetaMedia.Instance.Type = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("890598a9-0be4-49ee-8dd8-3581ee9355e6"), new System.Guid("3cf7f10e-dc56-4a50-95a5-fe7fae0be291"), new System.Guid("70823e7d-5829-4db7-99e0-f6c5f2b0e87b"));
		relationType.AssociationType.ObjectType = MetaMediaContent.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Type";
		relationType.RoleType.PluralName = "Types";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 1024;
		MetaMediaContent.Instance.Type = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0756d508-44b7-405e-bf92-bc09e5702e63"), new System.Guid("76e6547b-8dcf-4e69-ae2d-c8f8c33989e9"), new System.Guid("85170945-b020-485b-bb6f-c4122992ebfd"));
		relationType.AssociationType.ObjectType = MetaMediaContent.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "Data";
		relationType.RoleType.PluralName = "Datas";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = -1;
		MetaMediaContent.Instance.Data = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4e158d75-d0b5-4cb7-ad41-e8ed3002d175"), new System.Guid("6f2a83eb-17e9-408e-b18b-9bb2b9a3e812"), new System.Guid("4fac2dd3-8711-4115-96b9-a38f62e2d093"));
		relationType.AssociationType.ObjectType = MetaAutomatedAgent.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaAutomatedAgent.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("58870c93-b066-47b7-95f7-5411a46dbc7e"), new System.Guid("31925ed6-e66c-4718-963f-c8a71d566fe8"), new System.Guid("eee42775-b172-4fde-9042-a0f9b2224ec3"));
		relationType.AssociationType.ObjectType = MetaAutomatedAgent.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Description";
		relationType.RoleType.PluralName = "Descriptions";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaAutomatedAgent.Instance.Description = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("097bb620-f068-440e-8d02-ef9d8be1d0f0"), new System.Guid("3442728c-164a-477c-87be-19a789229585"), new System.Guid("3fd81194-2f6f-43e7-9c6b-91f5e3e118ac"));
		relationType.AssociationType.ObjectType = MetaPermission.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "OperandTypePointer";
		relationType.RoleType.PluralName = "OperandTypePointers";
		relationType.RoleType.IsRequired = true;
		MetaPermission.Instance.OperandTypePointer = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("29b80857-e51b-4dec-b859-887ed74b1626"), new System.Guid("8ffed1eb-b64e-4341-bbb6-348ed7f06e83"), new System.Guid("cadaca05-55ba-4a13-8758-786ff29c8e46"));
		relationType.AssociationType.ObjectType = MetaPermission.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "ConcreteClassPointer";
		relationType.RoleType.PluralName = "ConcreteClassPointers";
		relationType.RoleType.IsRequired = true;
		MetaPermission.Instance.ConcreteClassPointer = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9d73d437-4918-4f20-b9a7-3ce23a04bd7b"), new System.Guid("891734d6-4855-4b33-8b3b-f46fd6103149"), new System.Guid("d29ce0ed-fba8-409d-8675-dc95e1566cfb"));
		relationType.AssociationType.ObjectType = MetaPermission.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "OperationEnum";
		relationType.RoleType.PluralName = "OperationEnums";
		relationType.RoleType.IsRequired = true;
		MetaPermission.Instance.OperationEnum = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ed4b710a-fe24-4143-bb96-ed1bd9beae1a"), new System.Guid("1ea9eca4-eed0-4f61-8903-c99feae961ad"), new System.Guid("f10ea049-6d24-4ca2-8efa-032fcf3000b3"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FirstName";
		relationType.RoleType.PluralName = "FirstNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.FirstName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a3e4664-bb40-4208-8e90-a1b5be323f27"), new System.Guid("9b48ff56-afef-4501-ac97-6173731ff2c9"), new System.Guid("ace04ad8-bf64-4fc3-8216-14a720d3105d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "LastName";
		relationType.RoleType.PluralName = "LastNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.LastName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("eb18bb28-da9c-47b4-a091-2f8f2303dcb6"), new System.Guid("e3a4d7b2-c5f1-4101-9aab-a0135d37a5a5"), new System.Guid("a86fc7a6-dedd-4da9-a250-75c9ec730d22"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "MiddleName";
		relationType.RoleType.PluralName = "MiddleNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.MiddleName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e9e7c874-4d94-42ff-a4c9-414d05ff9533"), new System.Guid("da5e0427-79f7-4259-8a68-0071aa4c6273"), new System.Guid("c922b44f-6c6f-4e8b-901d-6558e79bb558"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAddress.Instance;
		relationType.RoleType.SingularName = "Address";
		relationType.RoleType.PluralName = "Addresses";
		MetaPerson.Instance.Addresses = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2a25125f-3545-4209-afc6-523eb0d8851e"), new System.Guid("94b038b3-2dd6-42a8-9cd6-800ddbef104c"), new System.Guid("fb6dcca2-14a6-4b00-bd3e-81acf59fbbe2"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Age";
		relationType.RoleType.PluralName = "Ages";
		MetaPerson.Instance.Age = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("adf83a86-878d-4148-a9fc-152f56697136"), new System.Guid("b9da077d-bfc7-4b4e-be62-03aded6da22e"), new System.Guid("0ffd9c62-efc6-4438-aaa3-755e4c637c21"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "BirthDate";
		relationType.RoleType.PluralName = "BirthDates";
		MetaPerson.Instance.BirthDate = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("688ebeb9-8a53-4e8d-b284-3faa0a01ef7c"), new System.Guid("8a181cec-7bae-4248-8e24-8abc7e01eea2"), new System.Guid("e431d53c-37ed-4fde-86a9-755f354c1d75"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "FullName";
		relationType.RoleType.PluralName = "FullNames";
		relationType.RoleType.Size = 256;
		MetaPerson.Instance.FullName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("654f6c84-62f2-4c0a-9d68-532ed3f39447"), new System.Guid("5ec6caf4-4752-4a89-92ec-13fd69b444f2"), new System.Guid("34704a90-d513-4fe2-a1ed-ad6d89399c73"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaGender.Instance;
		relationType.RoleType.SingularName = "Gender";
		relationType.RoleType.PluralName = "Genders";
		MetaPerson.Instance.Gender = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a8a3b4b8-c4f2-4054-ab2a-2eac6fd058e4"), new System.Guid("0fdeacf1-35bd-473d-88a9-acd65803f731"), new System.Guid("656f11e4-7652-4b4d-9dda-28cfe16333ec"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsMarried";
		relationType.RoleType.PluralName = "IsMarrieds";
		MetaPerson.Instance.IsMarried = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("54f11f06-8d3f-4d58-bcdc-d40e6820fdad"), new System.Guid("03a7ffcc-4291-4ae1-a2ab-69f7257fbf04"), new System.Guid("abd2a4b3-4b17-48d4-b465-0ffcb5a2664d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsStudent";
		relationType.RoleType.PluralName = "IsStudents";
		MetaPerson.Instance.IsStudent = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6340de2a-c3b1-4893-a7f3-cb924b82fa0e"), new System.Guid("b6ea4ac5-088a-4773-8410-6813d0185d7c"), new System.Guid("5a472c98-481f-407c-b53e-eaaa7e7a5340"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMailboxAddress.Instance;
		relationType.RoleType.SingularName = "MailboxAddress";
		relationType.RoleType.PluralName = "MailboxAddresses";
		MetaPerson.Instance.MailboxAddress = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0375a3d3-1a1b-4cbb-b735-1fe508bcc672"), new System.Guid("ebaedf39-1af9-42b7-83dc-8945450cebf2"), new System.Guid("86685c44-5196-46dd-9260-e40a434e9a52"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAddress.Instance;
		relationType.RoleType.SingularName = "MainAddress";
		relationType.RoleType.PluralName = "MainAddresses";
		MetaPerson.Instance.MainAddress = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b3ddd2df-8a5a-4747-bd4f-1f1eb37386b3"), new System.Guid("912b48f5-215e-4cc0-a83b-56b74d986608"), new System.Guid("f6624fac-db8e-4fb2-9e86-18021b59d31d"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMedia.Instance;
		relationType.RoleType.SingularName = "Photo";
		relationType.RoleType.PluralName = "Photos";
		MetaPerson.Instance.Photo = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6b626ba5-0c45-48c7-8b6b-5ea85e002d90"), new System.Guid("520bb966-6e8a-46a4-a3c0-18422af13cba"), new System.Guid("66e20063-ab51-417a-8ce4-135bb6e115c1"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "ShirtSize";
		relationType.RoleType.PluralName = "ShirtSizes";
		MetaPerson.Instance.ShirtSize = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1b057406-3343-426b-ab5b-ceb93ba02446"), new System.Guid("91d44bdd-7b17-4fa7-aeb7-625571b252b9"), new System.Guid("93d01c4a-0aa3-4d7c-a6d8-139b8ed1ffcc"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Text";
		relationType.RoleType.PluralName = "Texts";
		relationType.RoleType.Size = -1;
		MetaPerson.Instance.Text = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("15de4e58-c5ef-4ebb-9bf6-5ab06a02c5a4"), new System.Guid("be22968c-a450-418f-8f2e-f6140a56589c"), new System.Guid("ad249eb0-6cf2-4bcb-b3d1-3ff1282cd2f9"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "TinyMCEText";
		relationType.RoleType.PluralName = "TinyMCETexts";
		relationType.RoleType.Size = -1;
		MetaPerson.Instance.TinyMCEText = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("afc32e62-c310-421b-8c1d-6f2b0bb88b54"), new System.Guid("c21ebc52-6b32-4af7-847e-d3d7e1c4defe"), new System.Guid("0aab73c3-f997-4dd9-885a-2c1c892adb0e"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Weight";
		relationType.RoleType.PluralName = "Weights";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaPerson.Instance.Weight = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5661A98D-A935-4325-9B28-9D86175B1BD6"), new System.Guid("DEC66A7B-56F5-4010-A2E7-37E25124BC77"), new System.Guid("79FFEED6-E06A-42F4-B12F-D7F7C98B6499"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CycleOne";
		relationType.RoleType.PluralName = "CycleOnes";
		MetaPerson.Instance.CycleOne = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2EB2AF4F-2BF4-475F-BB41-D740197F168E"), new System.Guid("FAA1E59E-29EE-4E10-BFE1-94BFBCF238EA"), new System.Guid("7CEEA115-23C8-46E2-BA76-1FDB1FA85381"));
		relationType.AssociationType.ObjectType = MetaPerson.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CycleMany";
		relationType.RoleType.PluralName = "CycleMany";
		MetaPerson.Instance.CycleMany = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("51e56ae1-72dc-443f-a2a3-f5aa3650f8d2"), new System.Guid("47af1a0f-497d-4a19-887b-79e5fb77c8bd"), new System.Guid("7e6a71b0-2194-47f8-b562-cb4a15e335b6"));
		relationType.AssociationType.ObjectType = MetaRole.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPermission.Instance;
		relationType.RoleType.SingularName = "Permission";
		relationType.RoleType.PluralName = "Permissions";
		MetaRole.Instance.Permissions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("934bcbbe-5286-445c-a1bd-e2fcc786c448"), new System.Guid("05785884-ca83-43de-a6f3-86d3fa7ec82a"), new System.Guid("8d87c74f-53ed-4e1d-a2ea-12190ac233d2"));
		relationType.AssociationType.ObjectType = MetaRole.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaRole.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6503574b-8bab-4da8-a19d-23a9bcffe01e"), new System.Guid("cae9e5c2-afa1-46f4-b930-69d4e810038f"), new System.Guid("ab2b4b9c-87dd-4712-b123-f5f9271c6193"));
		relationType.AssociationType.ObjectType = MetaSecurityToken.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "AccessControl";
		relationType.RoleType.PluralName = "AccessControls";
		MetaSecurityToken.Instance.AccessControls = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9c1634ab-be99-4504-8690-ed4b39fec5bc"), new System.Guid("45a4205d-7c02-40d4-8d97-6d7d59e05def"), new System.Guid("1e051b37-cf30-43ed-a623-dd2928d6d0a3"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "DefaultLocale";
		relationType.RoleType.PluralName = "DefaultLocales";
		MetaSingleton.Instance.DefaultLocale = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9e5a3413-ed33-474f-adf2-149ad5a80719"), new System.Guid("33d5d8b9-3472-48d8-ab1a-83d00d9cb691"), new System.Guid("e75a8956-4d02-49ba-b0cf-747b7a9f350d"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "Locale";
		relationType.RoleType.PluralName = "Locales";
		MetaSingleton.Instance.Locales = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f16652b0-b712-43d7-8d4e-34a22487514d"), new System.Guid("c92466b5-55ba-496a-8880-2821f32f8f8e"), new System.Guid("3a12d798-40c3-40e0-ba9f-9d01b1e39e89"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Guest";
		relationType.RoleType.PluralName = "Guests";
		MetaSingleton.Instance.Guest = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6A6E0852-C984-47B8-939D-8E0B0B042B9D"), new System.Guid("E783AFBE-EF70-4AC1-8C0A-5DFE6FEDFBE0"), new System.Guid("BCF431F6-10CD-4F33-873D-0B2F1A1EA09D"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSecurityToken.Instance;
		relationType.RoleType.SingularName = "InitialSecurityToken";
		relationType.RoleType.PluralName = "InitialSecurityTokens";
		MetaSingleton.Instance.InitialSecurityToken = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f579494b-e550-4be6-9d93-84618ac78704"), new System.Guid("33f17e75-99cc-417e-99f3-c29080f08f0a"), new System.Guid("ca9e3469-583c-4950-ba2c-1bc3a0fc3e96"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSecurityToken.Instance;
		relationType.RoleType.SingularName = "DefaultSecurityToken";
		relationType.RoleType.PluralName = "DefaultSecurityTokens";
		MetaSingleton.Instance.DefaultSecurityToken = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4D17A849-9AC9-4A5D-9F2A-EA0152061A15"), new System.Guid("6854E369-3026-47B1-AF0C-142A5C6FCA8E"), new System.Guid("2C8B5D6D-0AF1-479D-B916-29F080856BD6"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "CreatorsAccessControl";
		relationType.RoleType.PluralName = "CreatorsAccessControls";
		MetaSingleton.Instance.CreatorsAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f7e50cac-ab57-4ebe-b765-d63804924c48"), new System.Guid("cb47a309-ed8f-47d1-879f-478e63b350d8"), new System.Guid("c955b6ef-57b7-404f-bba5-fa7aebf706f6"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "GuestAccessControl";
		relationType.RoleType.PluralName = "GuestAccessControls";
		MetaSingleton.Instance.GuestAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("829aa4a4-8232-4625-8cab-db7dc96da53f"), new System.Guid("56f18f8b-380b-4236-9a85-ed989c1a6e44"), new System.Guid("a3b765ed-bbf6-4bc4-9551-6338705ef03e"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "AdministratorsAccessControl";
		relationType.RoleType.PluralName = "AdministratorsAccessControls";
		MetaSingleton.Instance.AdministratorsAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("3aeb4787-e1cb-460f-9e9f-fcbc3fde1aae"), new System.Guid("174745f3-eae9-451c-acf8-b082ecfa52c8"), new System.Guid("4459b703-7d0e-4f78-a239-a1038c288f96"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "SalesAccessControl";
		relationType.RoleType.PluralName = "SalesAccessControls";
		MetaSingleton.Instance.SalesAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("96840be6-fb16-4450-9357-be7c010a8803"), new System.Guid("061e9a69-39f7-4760-b192-7fd45dc493d2"), new System.Guid("e2c738d1-6c4c-455b-9eb7-d59bcab88328"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "OperationsAccessControl";
		relationType.RoleType.PluralName = "OperationsAccessControls";
		MetaSingleton.Instance.OperationsAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("74bb8158-a222-429d-8421-3b508de5d516"), new System.Guid("be102333-b04a-4942-a7e2-9ef303d39bff"), new System.Guid("a20aef2f-4626-446f-9931-aed22e7a5043"));
		relationType.AssociationType.ObjectType = MetaSingleton.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "ProcurementAccessControl";
		relationType.RoleType.PluralName = "ProcurementAccessControls";
		MetaSingleton.Instance.ProcurementAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("585bb5cf-9ba4-4865-9027-3667185abc4f"), new System.Guid("1e2d1e31-ed80-4435-8850-7663d9c5f41d"), new System.Guid("c552f0b7-95ce-4d45-aaea-56bc8365eee4"));
		relationType.AssociationType.ObjectType = MetaUserGroup.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Member";
		relationType.RoleType.PluralName = "Members";
		MetaUserGroup.Instance.Members = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e94e7f05-78bd-4291-923f-38f82d00e3f4"), new System.Guid("75859e2c-c1a3-4f4c-bb37-4064d0aa81d0"), new System.Guid("9d3c1eec-bf10-4a79-a37f-bc6a20ff2a79"));
		relationType.AssociationType.ObjectType = MetaUserGroup.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.IsUnique = true;
		relationType.RoleType.Size = 256;
		MetaUserGroup.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5de25d18-3c36-418f-9c85-55a480d58bc5"), new System.Guid("1b4eb236-b359-40ff-ba67-2e1623844f78"), new System.Guid("57dc2c2a-949b-497b-880f-b1df13e0e12f"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateCreated";
		relationType.RoleType.PluralName = "DateCreateds";
		relationType.RoleType.IsRequired = true;
		MetaEmailMessage.Instance.DateCreated = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c297ff40-e2ad-46af-94fc-c61af6e6a6d6"), new System.Guid("366767a9-d82d-408d-9c06-7256724aa578"), new System.Guid("29b77e2c-9590-4da9-a616-f67e84187644"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateSending";
		relationType.RoleType.PluralName = "DateSendings";
		MetaEmailMessage.Instance.DateSending = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cc36e90a-dcda-4289-b84f-c947c97847b0"), new System.Guid("9b3d2505-103a-4801-9f16-f1f7ca924f57"), new System.Guid("ae7bedca-c966-4cd5-9a8a-b99f3fc5e0bc"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateSent";
		relationType.RoleType.PluralName = "DateSents";
		MetaEmailMessage.Instance.DateSent = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e16da480-35ab-4383-940a-5298d0b33b9c"), new System.Guid("5be8bb0f-cead-44f6-813b-1125882618b7"), new System.Guid("4cca6d37-fffe-4e78-962c-f4474551e09e"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Sender";
		relationType.RoleType.PluralName = "Senders";
		MetaEmailMessage.Instance.Sender = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d115bcfb-55e5-4ed8-8a21-f8e4dd5f903d"), new System.Guid("55c3f9b5-1a80-419d-93cc-6c19925e350e"), new System.Guid("8e8749da-4411-4dfa-bd78-856f37e1a4ba"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "Recipient";
		relationType.RoleType.PluralName = "Recipients";
		MetaEmailMessage.Instance.Recipients = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("CD9C9D1E-3393-46B4-AD61-7AC03019EE08"), new System.Guid("EC809FF4-98BB-4DFA-9D18-1D321A2BC871"), new System.Guid("6846A2B4-DFC4-436E-81E2-C504DD020546"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "RecipientEmailAddress";
		relationType.RoleType.PluralName = "RecipientEmailAddresses";
		relationType.RoleType.Size = 256;
		MetaEmailMessage.Instance.RecipientEmailAddress = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5666ebec-8205-4e5f-b0df-cacfa1af99ce"), new System.Guid("1adc0465-9b6b-4050-9b0a-e7fe441ccbd5"), new System.Guid("f19705f3-5323-4360-8602-acee1be80c50"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Subject";
		relationType.RoleType.PluralName = "Subjects";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 1024;
		MetaEmailMessage.Instance.Subject = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("25be1f1c-ea8b-471e-ad09-b618927dc15a"), new System.Guid("0b9ec5be-fe85-407c-8a35-434ede55bd3b"), new System.Guid("b331b4dd-7bfa-479d-91f2-9376955207ef"));
		relationType.AssociationType.ObjectType = MetaEmailMessage.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Body";
		relationType.RoleType.PluralName = "Bodies";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = -1;
		MetaEmailMessage.Instance.Body = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9a226bec-31b9-413e-bec1-8dcdf36fa6fb"), new System.Guid("c05db652-e6b0-485b-bcf5-9ec77a20d068"), new System.Guid("db9f708f-ac35-49f4-a62a-9df9863da8bd"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUniquelyIdentifiable.Instance;
		relationType.RoleType.SingularName = "Target";
		relationType.RoleType.PluralName = "Targets";
		MetaNotification.Instance.Target = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50b1be30-d6a9-49e8-84da-a47647e443f0"), new System.Guid("cb7cc442-b05b-48a5-8696-4baab7aa8cce"), new System.Guid("3ee1975d-5019-4043-be5f-65331ef58787"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "Confirmed";
		relationType.RoleType.PluralName = "Confirmeds";
		relationType.RoleType.IsRequired = true;
		MetaNotification.Instance.Confirmed = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("70292962-9e0e-4b57-a710-c8ac34f65b11"), new System.Guid("80e4d1c5-5648-486a-a2ff-3b1690514f20"), new System.Guid("84813900-abe0-4c24-bd2e-5b0d6be5ab6c"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Title";
		relationType.RoleType.PluralName = "Titles";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 1024;
		MetaNotification.Instance.Title = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e83600fc-5411-4c72-9903-80a3741a9165"), new System.Guid("1da1555c-fee6-4084-be37-57a6f58fe591"), new System.Guid("a6f6ed43-b0ab-462f-9be9-fad58d2e8398"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Description";
		relationType.RoleType.PluralName = "Descriptions";
		relationType.RoleType.Size = -1;
		MetaNotification.Instance.Description = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("458a8223-9c0f-4475-93c0-82d5cc133f1b"), new System.Guid("8d657749-a823-422b-9e29-041453ccb267"), new System.Guid("d100a845-df65-4f06-96b8-dcaeb9c3e205"));
		relationType.AssociationType.ObjectType = MetaNotification.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateCreated";
		relationType.RoleType.PluralName = "DateCreateds";
		relationType.RoleType.IsRequired = true;
		MetaNotification.Instance.DateCreated = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4516c5c1-73a0-4fdc-ac3c-aefaf417c8ba"), new System.Guid("7fb512b5-3440-444a-9562-ad3655e551e5"), new System.Guid("9b7d6984-98cb-4367-a6fc-9b07c9101fda"));
		relationType.AssociationType.ObjectType = MetaNotificationList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotification.Instance;
		relationType.RoleType.SingularName = "Notification";
		relationType.RoleType.PluralName = "Notifications";
		MetaNotificationList.Instance.Notifications = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("89487904-053e-470f-bcf9-0e01165b0143"), new System.Guid("2d41d7ef-d107-404f-ac9d-fb81105d3ff7"), new System.Guid("fc089f2e-a625-40f9-bbc0-c9fc05e6e599"));
		relationType.AssociationType.ObjectType = MetaNotificationList.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotification.Instance;
		relationType.RoleType.SingularName = "UnconfirmedNotification";
		relationType.RoleType.PluralName = "UnconfirmedNotifications";
		MetaNotificationList.Instance.UnconfirmedNotifications = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("438fdc30-25ac-4d33-9a55-0ef817c05479"), new System.Guid("34a36081-e093-4d8b-ae87-4a3df329f7a1"), new System.Guid("b752a7c3-433c-4b54-bbc1-0f812d5afb16"));
		relationType.AssociationType.ObjectType = MetaNotificationList.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotification.Instance;
		relationType.RoleType.SingularName = "ConfirmedNotification";
		relationType.RoleType.PluralName = "ConfirmedNotifications";
		MetaNotificationList.Instance.ConfirmedNotifications = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c32c19f1-3f41-4d11-b19d-b8b2aa360166"), new System.Guid("6e08b1d8-f7fa-452d-907a-668bf32702c1"), new System.Guid("407443f4-5afa-484e-be97-88ef19f00b32"));
		relationType.AssociationType.ObjectType = MetaTaskAssignment.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUser.Instance;
		relationType.RoleType.SingularName = "User";
		relationType.RoleType.PluralName = "Users";
		relationType.RoleType.IsRequired = true;
		MetaTaskAssignment.Instance.User = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f4e05932-89c0-4f40-b4b2-f241ac42d8a0"), new System.Guid("d1f61b05-8f54-47b6-87dd-fd7b66ef0b50"), new System.Guid("86589718-3284-43e9-8f3e-a0f5faa30357"));
		relationType.AssociationType.ObjectType = MetaTaskAssignment.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotification.Instance;
		relationType.RoleType.SingularName = "Notification";
		relationType.RoleType.PluralName = "Notifications";
		MetaTaskAssignment.Instance.Notification = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a01f221-480f-4d61-9a12-72e3689a8224"), new System.Guid("e5e52776-c946-42b0-99f4-596ffc1c298f"), new System.Guid("0be6c69a-1d1c-49d0-8247-fa0ff9a8f223"));
		relationType.AssociationType.ObjectType = MetaTaskAssignment.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTask.Instance;
		relationType.RoleType.SingularName = "Task";
		relationType.RoleType.PluralName = "Tasks";
		relationType.RoleType.IsRequired = true;
		MetaTaskAssignment.Instance.Task = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c3467693-cc18-46e5-a0af-d7ab0cbe9faa"), new System.Guid("7976dbaa-9b96-401f-900d-db76fd45f18f"), new System.Guid("3922d9e8-e518-4459-8b52-0723104456ab"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskAssignment.Instance;
		relationType.RoleType.SingularName = "TaskAssignment";
		relationType.RoleType.PluralName = "TaskAssignments";
		MetaTaskList.Instance.TaskAssignments = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2e7381c6-3a58-4a64-8808-4f3532254f08"), new System.Guid("63efedc3-1157-4ae0-b212-9169cd0ac418"), new System.Guid("4f83aaac-7ba1-4fdc-9ddf-781559ff3983"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskAssignment.Instance;
		relationType.RoleType.SingularName = "OpenTaskAssignment";
		relationType.RoleType.PluralName = "OpenTaskAssignments";
		MetaTaskList.Instance.OpenTaskAssignments = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bc07648e-b80c-42f6-a4dd-113fba962c89"), new System.Guid("c3b078cf-27ee-4686-b7ff-ba40a7aba5a7"), new System.Guid("ef37d700-cfa6-4998-9501-9d09bb9ac1d8"));
		relationType.AssociationType.ObjectType = MetaTaskList.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Count";
		relationType.RoleType.PluralName = "Counts";
		MetaTaskList.Instance.Count = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("A3DED776-B516-4C38-9B5F-5DEBFAFD15CB"), new System.Guid("E23C3438-21C5-4E01-A252-033774CF8EA0"), new System.Guid("AAE972FE-192D-4356-AA41-743EEFA32589"));
		relationType.AssociationType.ObjectType = MetaBuild.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "Guid";
		relationType.RoleType.PluralName = "Guids";
		MetaBuild.Instance.Guid = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("19112701-B610-49FC-82B8-FB786EEBCDB4"), new System.Guid("91262C96-A305-40F4-953F-4CDC05FD59F9"), new System.Guid("F81A23A4-AD8F-470E-9717-E3774411C6AE"));
		relationType.AssociationType.ObjectType = MetaBuild.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "String";
		relationType.RoleType.PluralName = "Strings";
		MetaBuild.Instance.String = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a999086-ca90-40a1-90ae-475d231bb1eb"), new System.Guid("0ce20e7c-7be0-4c07-a179-e8d0e77f3de1"), new System.Guid("4ab20876-f8fc-4d39-87d7-8758f044587b"));
		relationType.AssociationType.ObjectType = MetaBadUI.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "PersonsMany";
		relationType.RoleType.PluralName = "PersonsMany";
		MetaBadUI.Instance.PersonsMany = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9ebbb9d1-2ca7-4a7f-9f18-f25c05fd28c1"), new System.Guid("37c64e26-a391-4c7b-98fb-53ccb5fbc795"), new System.Guid("4d2c7c20-b9c7-451b-b6b1-8552322ceddd"));
		relationType.AssociationType.ObjectType = MetaBadUI.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CompanyOne";
		relationType.RoleType.PluralName = "CompanyOnes";
		MetaBadUI.Instance.CompanyOne = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a4db0d75-3dff-45ac-9c1d-623bca046b4a"), new System.Guid("5ed577d8-f048-42b8-9fb4-38b88ebf35f1"), new System.Guid("c1b45f09-59fe-4484-8999-e2a3d9147919"));
		relationType.AssociationType.ObjectType = MetaBadUI.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "PersonOne";
		relationType.RoleType.PluralName = "PersonOnes";
		MetaBadUI.Instance.PersonOne = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a8621048-48b5-43c4-b10b-17225958d177"), new System.Guid("718eaf0b-1b62-43b2-b336-c9820d806b28"), new System.Guid("1663525b-5add-4a96-a596-5d736d466985"));
		relationType.AssociationType.ObjectType = MetaBadUI.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.SingularName = "CompanyMany";
		relationType.RoleType.PluralName = "CompanyManies";
		MetaBadUI.Instance.CompanyMany = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c93a102e-ecdb-4189-a0fc-eeea8b4b85d4"), new System.Guid("2225f3e0-1304-4a55-9b89-29563fe52e3c"), new System.Guid("7f2dc0db-4628-45a8-8cc5-2cc1b87e0eb3"));
		relationType.AssociationType.ObjectType = MetaBadUI.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsString";
		relationType.RoleType.PluralName = "AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaBadUI.Instance.AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("97f31053-0e7b-42a0-90c2-ce6f09c56e86"), new System.Guid("70e42b8b-09e2-4cb1-a632-ff3785ee1c8d"), new System.Guid("e5cd692c-ab97-4cf8-9f8a-1de733526e74"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "C1AllorsBinary";
		relationType.RoleType.PluralName = "C1AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaC1.Instance.C1AllorsBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b4ee673f-bba0-4e24-9cda-3cf993c79a0a"), new System.Guid("948aa9e6-5cb3-48dc-a3b7-3f8770269dae"), new System.Guid("ad456144-a19e-4c89-845b-9391dbc8f372"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "C1AllorsBoolean";
		relationType.RoleType.PluralName = "C1AllorsBooleans";
		MetaC1.Instance.C1AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ef75cc4e-8787-4f1c-ae5c-73577d721467"), new System.Guid("8c8baa81-0c59-485c-b416-c7e6ec972595"), new System.Guid("610129f7-0c35-4649-9f4b-14698d0d1c77"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "C1AllorsDateTime";
		relationType.RoleType.PluralName = "C1AllorsDateTimes";
		MetaC1.Instance.C1AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("87eb0d19-73a7-4aae-aeed-66dc9163233c"), new System.Guid("96e8dfaf-3e1e-4c59-88f3-d47be6c96b74"), new System.Guid("43ccd07d-b9c4-465c-b2f9-083a36315e85"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "C1AllorsDecimal";
		relationType.RoleType.PluralName = "C1AllorsDecimals";
		relationType.RoleType.Precision = 10;
		relationType.RoleType.Scale = 2;
		MetaC1.Instance.C1AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f268783d-42ed-41c1-b0b0-b8a60e30a601"), new System.Guid("6ed0694c-a74f-44c3-835b-897f56343576"), new System.Guid("459d20d8-dadd-44e1-aa8a-396e6eab7538"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "C1AllorsDouble";
		relationType.RoleType.PluralName = "C1AllorsDoubles";
		MetaC1.Instance.C1AllorsDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f4920d94-8cd0-45b6-be00-f18d377368fd"), new System.Guid("c4202876-b670-4193-a459-3f0376e24c38"), new System.Guid("2687f5be-eebe-4ffb-a8b2-538134cb6f73"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "C1AllorsInteger";
		relationType.RoleType.PluralName = "C1AllorsIntegers";
		MetaC1.Instance.C1AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("20713860-8abd-4d71-8ccc-2b4d1b88bce3"), new System.Guid("974aa133-255b-431f-a15d-b6a126d362b5"), new System.Guid("6dc98925-87a7-4959-8095-90eedef0e9a0"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "C1AllorsString";
		relationType.RoleType.PluralName = "C1AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaC1.Instance.C1AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a64abd21-dadf-483d-9499-d19aa8e33791"), new System.Guid("099e3d39-16b5-431a-853b-942a354c3a52"), new System.Guid("c186bb2f-8e19-468d-8a01-561384e5187d"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsStringMax";
		relationType.RoleType.PluralName = "AllorsStringMaxes";
		relationType.RoleType.Size = -1;
		MetaC1.Instance.AllorsStringMax = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cef13620-b7d7-4bfe-8d3b-c0f826da5989"), new System.Guid("6c18bd8f-9084-470b-9dfe-30263c98771b"), new System.Guid("2721249b-dadd-410d-b4e0-9d4a48e615d1"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "C1AllorsUnique";
		relationType.RoleType.PluralName = "C1AllorsUniques";
		MetaC1.Instance.C1AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8c198447-e943-4f5a-b749-9534b181c664"), new System.Guid("154222cb-0eb8-459d-839c-9c8857bd1c7e"), new System.Guid("c403f160-6486-4207-b32c-aa9ade27a28c"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1Many2Many";
		relationType.RoleType.PluralName = "C1C1Many2Manies";
		MetaC1.Instance.C1C1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a8e18ea7-cbf2-4ea7-ae14-9f4bcfdb55de"), new System.Guid("8a546f48-fc09-48ae-997d-4a6de0cd458a"), new System.Guid("e6b21250-194b-4424-8b92-221c6d0e6228"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1Many2One";
		relationType.RoleType.PluralName = "C1C1Many2Ones";
		MetaC1.Instance.C1C1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a0ac5a65-2cbd-4c51-9417-b10150bc5699"), new System.Guid("d595765b-5e67-46f2-b19c-c58563dd1ae0"), new System.Guid("3d121ffa-0ff5-4627-9ec3-879c2830ff04"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1One2Many";
		relationType.RoleType.PluralName = "C1C1One2Manies";
		MetaC1.Instance.C1C1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("79c00218-bb4f-40e9-af7d-61af444a4a54"), new System.Guid("2276c942-dd96-41a6-b52f-cd3862c4692f"), new System.Guid("40ee2908-2556-4bdf-a82f-2ea33e181b91"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C1C1One2One";
		relationType.RoleType.PluralName = "C1C1One2Ones";
		MetaC1.Instance.C1C1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f29d4a52-9ba5-40f6-ba99-050cbd03e554"), new System.Guid("122dc72f-cc92-440c-84e5-fe8340020c43"), new System.Guid("608db13d-1778-44a8-94f0-b86fc0f6992d"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C1C2Many2Many";
		relationType.RoleType.PluralName = "C1C2Many2Manies";
		MetaC1.Instance.C1C2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5490dc63-a8f6-4a86-91ef-fef97a86f119"), new System.Guid("3f307d57-1f39-4aba-822d-9881cef7223c"), new System.Guid("66a06e06-95e4-43ad-9b45-56687f8a2051"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C1C2Many2One";
		relationType.RoleType.PluralName = "C1C2Many2Ones";
		MetaC1.Instance.C1C2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9f6538c2-e6dd-4c27-80ed-2748f645cb95"), new System.Guid("3ddac067-46f1-4302-bb1b-aa0e05dd55ae"), new System.Guid("c749e58c-0f1d-4946-b35d-878221aac72f"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C1C2One2Many";
		relationType.RoleType.PluralName = "C1C2One2Manies";
		MetaC1.Instance.C1C2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e97fc754-c736-4359-9662-19dce9429f89"), new System.Guid("5bd37271-01c0-4cd3-94d5-0284700b3567"), new System.Guid("392f5a47-f181-475c-b5c9-f0b729c8413f"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C1C2One2One";
		relationType.RoleType.PluralName = "C1C2One2Ones";
		MetaC1.Instance.C1C2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("94a2b37d-9431-4496-b992-630cda5b9851"), new System.Guid("a4a31323-7193-4709-828e-88b2c0f3f8aa"), new System.Guid("f225d708-c98f-44ff-9ed8-847cb1ddaacb"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C1I12Many2Many";
		relationType.RoleType.PluralName = "C1I12Many2Manies";
		MetaC1.Instance.C1I12Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bcf4df45-6616-4cdf-8ada-f944f9c7ff1a"), new System.Guid("2128418c-6918-4be8-8a02-2bea142b7fc4"), new System.Guid("b5b4892d-e1d3-4a4b-a8a4-ac6ed0ff930e"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C1I12Many2One";
		relationType.RoleType.PluralName = "C1I12Many2Ones";
		MetaC1.Instance.C1I12Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("98c5f58b-1777-4d9a-8828-37dbf7051510"), new System.Guid("3218ac29-2eac-4dc9-acad-2c708c3df994"), new System.Guid("51b3b28e-9017-4a1e-b5ba-06a9b14d88d6"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C1I12One2Many";
		relationType.RoleType.PluralName = "C1I12One2Manies";
		MetaC1.Instance.C1I12One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b9f2c4c7-6979-40cf-82a2-fa99a5d9e9a4"), new System.Guid("911a9327-0237-4254-99a7-afff0d6a0369"), new System.Guid("50bf56c3-f05f-4172-86e1-aefead4a3a8c"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C1I12One2One";
		relationType.RoleType.PluralName = "C1I12One2Ones";
		MetaC1.Instance.C1I12One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("815878f6-16f2-42f2-9b24-f394ddf789c2"), new System.Guid("eca51eab-3815-410f-b4c5-f7e2a1318791"), new System.Guid("39f62f9e-52d3-47c5-8fd4-44e91d9b78be"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C1I1Many2Many";
		relationType.RoleType.PluralName = "C1I1Many2Manies";
		MetaC1.Instance.C1I1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7bb216f2-8e9c-4dcd-890b-579130ab0a8b"), new System.Guid("531e89ab-a295-4f72-8496-cdd0d8605d37"), new System.Guid("8af8fbc6-2f59-4026-9093-5b335dfb8b7f"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C1I1Many2One";
		relationType.RoleType.PluralName = "C1I1Many2Ones";
		MetaC1.Instance.C1I1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e0656d9a-75a6-4e59-aaa1-3ff03d440059"), new System.Guid("637c5967-fb6c-45d4-81c4-de5559df785f"), new System.Guid("89e4802f-7c61-4deb-a243-f78e79578082"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C1I1One2Many";
		relationType.RoleType.PluralName = "C1I1One2Manies";
		MetaC1.Instance.C1I1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0e7f529b-bc91-4a40-a7e7-a17341c6bf5b"), new System.Guid("1d1374c3-a28d-4904-b98a-3a14ceb2f7ea"), new System.Guid("da5ccb42-7878-45a9-9350-17f0f0a52fd4"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C1I1One2One";
		relationType.RoleType.PluralName = "C1I1One2Ones";
		MetaC1.Instance.C1I1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cda97972-84c8-48e3-99d8-fd7c99c5dbc9"), new System.Guid("8ef5784c-6f76-431e-b59d-075813ad7863"), new System.Guid("ce5170b0-347a-49b7-9925-a7a5c5eb2c75"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C1I2Many2Many";
		relationType.RoleType.PluralName = "C1I2Many2Manies";
		MetaC1.Instance.C1I2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d0341bed-2732-4bcb-b1bb-9f9589de5d03"), new System.Guid("dacd7dfa-6650-438d-b564-49fbf89fea8d"), new System.Guid("2db69dd4-008b-4a17-aba5-6a050f35f7e3"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C1I2Many2One";
		relationType.RoleType.PluralName = "C1I2Many2Ones";
		MetaC1.Instance.C1I2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("82f5fb26-c260-41bc-a784-a2d5e35243bd"), new System.Guid("f5329d84-1301-44ea-85b4-dc7d98554694"), new System.Guid("ca30ba2a-627f-43d1-b467-fe0d7cd015cc"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C1I2One2Many";
		relationType.RoleType.PluralName = "C1I2One2Manies";
		MetaC1.Instance.C1I2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6def7988-4bcf-4964-9de6-c6ede41d5e5a"), new System.Guid("75e47fbe-6ce1-4cc1-a20f-51a861df9cc3"), new System.Guid("e7d1e28d-69ad-4d3a-b35a-2d0aaacb67db"));
		relationType.AssociationType.ObjectType = MetaC1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C1I2One2One";
		relationType.RoleType.PluralName = "C1I2One2Ones";
		MetaC1.Instance.C1I2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("07eaa992-322a-40e9-bf2c-aa33b69f54cd"), new System.Guid("172c107a-e140-4462-9a62-5ef91e6ead2a"), new System.Guid("152c92f0-485e-4a28-b321-d6ed3b730fc0"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "C2AllorsDecimal";
		relationType.RoleType.PluralName = "C2AllorsDecimals";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaC2.Instance.C2AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0c8209e3-b2fc-4c7a-acd2-6b5b8ac89bf4"), new System.Guid("56bb9554-819f-418a-9ce1-a91fa704b371"), new System.Guid("9292cb86-3e04-4cd4-b3fd-a5af7a5ce538"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C2C1One2One";
		relationType.RoleType.PluralName = "C2C1One2Ones";
		MetaC2.Instance.C2C1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("12896fc2-c9e9-4a89-b875-0aeb92e298e5"), new System.Guid("781b282e-b86f-4747-9d5e-d0f7c08b0899"), new System.Guid("f41ddb05-4a96-40fa-859b-8b3d6dfcd86b"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C2C2Many2One";
		relationType.RoleType.PluralName = "C2C2Many2Ones";
		MetaC2.Instance.C2C2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1444d919-6ca1-4642-8d18-9d955c817581"), new System.Guid("9263c1e7-0cda-4129-a16d-da5351adafcb"), new System.Guid("cc1f2cae-2a5d-4584-aa08-4b03fc2176d2"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "C2AllorsUnique";
		relationType.RoleType.PluralName = "C2AllorsUniques";
		MetaC2.Instance.C2AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("165cc83e-935d-4d0d-aec7-5da155300086"), new System.Guid("bc437b29-f883-41c1-b36f-20be37bc9b30"), new System.Guid("b2f83414-aa5c-44fd-a382-56119727785a"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C2I12Many2One";
		relationType.RoleType.PluralName = "C2I12Many2Ones";
		MetaC2.Instance.C2I12Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1d0c57c9-a3d1-4134-bc7d-7bb587d8250f"), new System.Guid("07c026ad-3515-4df0-bee7-ab61d5a9217d"), new System.Guid("c0562ba5-0402-44ea-acd0-9e78d20e7576"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C2I12One2One";
		relationType.RoleType.PluralName = "C2I12One2Ones";
		MetaC2.Instance.C2I12One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1d98eda7-6dba-43f1-a5ce-44f7ed104cf9"), new System.Guid("cae17f3c-a605-4dce-b38d-01c23eea29df"), new System.Guid("d3e84546-02fc-40be-b550-dbd54cd8a139"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C2I1Many2Many";
		relationType.RoleType.PluralName = "C2I1Many2Manies";
		MetaC2.Instance.C2I1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("262ad367-a52c-4d8b-94e2-b477bb098423"), new System.Guid("31be0ad7-0886-406a-a69f-7e38b4526199"), new System.Guid("c52984df-80f8-4622-84e0-0e9f97cfaff3"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "C2AllorsDouble";
		relationType.RoleType.PluralName = "C2AllorsDoubles";
		MetaC2.Instance.C2AllorsDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2ac55066-c748-4f90-9d0f-1090fe02cc76"), new System.Guid("02a5ac2c-d0ac-482d-abee-117ed7eaa5ba"), new System.Guid("28f373c6-62b6-4f5c-b794-c10138043a63"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C2I1One2Many";
		relationType.RoleType.PluralName = "C2I1One2Manies";
		MetaC2.Instance.C2I1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("38063edc-271a-410d-b857-807a9100c7b5"), new System.Guid("6bedcc6b-af15-4f27-93e8-4404d23dfd99"), new System.Guid("642f5531-896d-482f-b746-4ecf08f27027"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C2I2One2One";
		relationType.RoleType.PluralName = "C2I2One2Ones";
		MetaC2.Instance.C2I2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("42f9f4b6-3b35-4168-93cb-35171dbf83f4"), new System.Guid("622f9b4f-efc8-454f-9dd6-884bed5b5f4b"), new System.Guid("f5545dfc-e19a-456a-8469-46708ea5bb68"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "C2AllorsInteger";
		relationType.RoleType.PluralName = "C2AllorsIntegers";
		MetaC2.Instance.C2AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4a963639-72c3-4e9f-9058-bcfc8fe0bc9e"), new System.Guid("e8c9548b-3d75-4f2b-af4f-f953572c587c"), new System.Guid("a1a975a4-7d1e-4734-962e-2f717386783a"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C2I2Many2Many";
		relationType.RoleType.PluralName = "C2I2Many2Manies";
		MetaC2.Instance.C2I2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("50300577-b5fb-4c16-9ac5-41151543f958"), new System.Guid("1f16f92e-ba99-4553-bd1d-b95ba360468a"), new System.Guid("6210478c-86e3-4d8c-8e3c-3b29da3175ca"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C2I12Many2Many";
		relationType.RoleType.PluralName = "C2I12Many2Manies";
		MetaC2.Instance.C2I12Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("60680366-4790-4443-a941-b30cd4bd3848"), new System.Guid("8fa68cfd-8e0c-40c6-881b-4ebe88487ae7"), new System.Guid("bfa632a3-f334-4c92-a1b1-21cfa726ab90"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C2C2One2Many";
		relationType.RoleType.PluralName = "C2C2One2Manies";
		MetaC2.Instance.C2C2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("61daaaae-dd22-405e-aa98-6321d2f8af04"), new System.Guid("a0291a20-3519-44e6-bb8d-b53682c02c52"), new System.Guid("bff48eef-9e8f-45b7-83ff-7b63dac407f1"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "C2AllorsBoolean";
		relationType.RoleType.PluralName = "C2AllorsBooleans";
		MetaC2.Instance.C2AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("65a246a7-cd78-45eb-90db-39f542e7c6cf"), new System.Guid("eb4f1289-1c6c-4964-a9ba-50f991a96564"), new System.Guid("6ff71b5b-723d-424f-9e2f-fb37bb8118fe"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C2I1Many2One";
		relationType.RoleType.PluralName = "C2I1Many2Ones";
		MetaC2.Instance.C2I1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("67780894-fa62-48ba-8f47-7f54106090cd"), new System.Guid("38cd28ba-c584-4d06-b521-dcc8094c5ed3"), new System.Guid("128eb00f-03fc-432e-bec6-8fcfb265a3a9"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "C2I1One2One";
		relationType.RoleType.PluralName = "C2I1One2Ones";
		MetaC2.Instance.C2I1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("70600f67-7b18-4b5c-b11e-2ed180c5b2d6"), new System.Guid("a373cb01-731b-48be-a387-d057fdb70684"), new System.Guid("572738e4-956b-404d-97e8-4bb431ce7692"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C2C1Many2Many";
		relationType.RoleType.PluralName = "C2C1Many2Manies";
		MetaC2.Instance.C2C1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("770eb33c-c8ef-4629-a3a0-20decd92ff62"), new System.Guid("de757393-f81a-413c-897b-a47efd48cc79"), new System.Guid("8ac9a5cd-35a4-4ca3-a1af-ab3f489c7a52"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "C2I12One2Many";
		relationType.RoleType.PluralName = "C2I12One2Manies";
		MetaC2.Instance.C2I12One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7a9129c9-7b6d-4bdd-a630-cfd1392549b7"), new System.Guid("87f7a34c-476f-4670-a670-30451c05842d"), new System.Guid("19f3caa1-c8d1-4257-b4ad-2f8ccb809524"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C2I2One2Many";
		relationType.RoleType.PluralName = "C2I2One2Manies";
		MetaC2.Instance.C2I2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("86ad371b-0afd-420b-a855-38ebb3f39f38"), new System.Guid("23f5e29b-c36b-416f-93da-9ef2d79fc0f1"), new System.Guid("cdf7b6ee-fa50-44a1-9433-d04d61ef3aeb"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C2C2One2One";
		relationType.RoleType.PluralName = "C2C2One2Ones";
		MetaC2.Instance.C2C2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9c7cde3f-9b61-4c79-a5d7-afe1067262ce"), new System.Guid("71d6109e-1c04-4598-88fa-f06308beb45b"), new System.Guid("8a96d544-e96f-45b5-aeee-d9381946ff31"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "C2AllorsString";
		relationType.RoleType.PluralName = "C2AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaC2.Instance.C2AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a5315151-aa0f-42a3-9d5b-2c7f2cb92560"), new System.Guid("f2bf51b6-0375-4d77-8881-d4d313d682ef"), new System.Guid("54dce296-9454-440e-9cf3-1327ea439f0e"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C2C1Many2One";
		relationType.RoleType.PluralName = "C2C1Many2Ones";
		MetaC2.Instance.C2C1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bc6c7fe0-6501-428c-a929-da87a9f4b885"), new System.Guid("794d2637-293c-49cc-a052-246a779825e9"), new System.Guid("73d243be-d8d0-42c7-b354-fd9786b4eaaa"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "C2C2Many2Many";
		relationType.RoleType.PluralName = "C2C2Many2Manies";
		MetaC2.Instance.C2C2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ce23482d-3a22-4202-98e7-5934fd9abd2d"), new System.Guid("6d752249-af37-4f22-9e59-bfae9e6537ee"), new System.Guid("6e9490f2-740f-498c-9c0f-d601c76f28ad"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "C2AllorsDateTime";
		relationType.RoleType.PluralName = "C2AllorsDateTimes";
		MetaC2.Instance.C2AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e08d75a9-9b67-4d20-a476-757f8fb17376"), new System.Guid("7d45be10-724e-46c4-8dac-4acdf7f515ad"), new System.Guid("888cd015-7323-45da-83fe-eb297e8ede51"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "C2I2Many2One";
		relationType.RoleType.PluralName = "C2I2Many2Ones";
		MetaC2.Instance.C2I2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f748949e-de5a-4f2e-85e2-e15516d9bf24"), new System.Guid("92c02837-9e6c-45ad-8772-b97a17afad8c"), new System.Guid("2c172bc6-a87b-4945-b02f-e00a38eb866d"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "C2C1One2Many";
		relationType.RoleType.PluralName = "C2C1One2Manies";
		MetaC2.Instance.C2C1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("fa8ad982-9953-47dd-9905-81cc4572300e"), new System.Guid("604eec66-6a75-465b-93d8-349dcbccb2bd"), new System.Guid("e701ac90-488a-476f-9b13-ea361e8ff450"));
		relationType.AssociationType.ObjectType = MetaC2.Instance;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "C2AllorsBinary";
		relationType.RoleType.PluralName = "C2AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaC2.Instance.C2AllorsBinary = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1b8e0350-c446-48dc-85c0-71130cc1490e"), new System.Guid("97c6a03f-f0c7-4c7d-b40f-1353e34431bd"), new System.Guid("89b8f5f6-5589-42ad-ac9e-1d984c02f7ea"));
		relationType.AssociationType.ObjectType = MetaDependee.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSubdependee.Instance;
		relationType.RoleType.SingularName = "Subdependee";
		relationType.RoleType.PluralName = "Subdependees";
		MetaDependee.Instance.Subdependee = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c1e86449-e5a8-4911-97c7-b03de9142f98"), new System.Guid("2786b8ca-2d71-44cc-8e1e-1896ac5e6c5c"), new System.Guid("af75f294-b20d-4304-8804-32ef9c0a324a"));
		relationType.AssociationType.ObjectType = MetaDependee.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Subcounter";
		relationType.RoleType.PluralName = "Subcounters";
		MetaDependee.Instance.Subcounter = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d58d1f28-3abd-4294-abde-885bdd16f466"), new System.Guid("9a867244-8ea3-402b-9a9c-a78727dbee78"), new System.Guid("5f570211-688e-4050-bf54-997d22a529d5"));
		relationType.AssociationType.ObjectType = MetaDependee.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Counter";
		relationType.RoleType.PluralName = "Counters";
		MetaDependee.Instance.Counter = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e73b8fc5-0148-486a-9379-cfb051b303d2"), new System.Guid("db615c1c-3d08-4faa-b19f-740bd7102fbd"), new System.Guid("bde110ae-8242-4d98-bdc3-feeed8fde742"));
		relationType.AssociationType.ObjectType = MetaDependee.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "DeleteDependent";
		relationType.RoleType.PluralName = "DeleteDependents";
		MetaDependee.Instance.DeleteDependent = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8859af04-ba38-42ce-8ac9-f428c3f92f31"), new System.Guid("cd3972e6-8ad4-4b01-9381-4d18718c7538"), new System.Guid("d6b1d6b6-539b-4b12-9363-18e7e9ab632c"));
		relationType.AssociationType.ObjectType = MetaDependent.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDependee.Instance;
		relationType.RoleType.SingularName = "Dependee";
		relationType.RoleType.PluralName = "Dependees";
		MetaDependent.Instance.Dependee = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9884955e-74ed-4f9d-9362-8e0274c53bf9"), new System.Guid("5b97e356-9bcd-4c4e-be7a-ef577eef5f14"), new System.Guid("d067129b-8440-4fc7-80d3-832ce569fe54"));
		relationType.AssociationType.ObjectType = MetaDependent.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Counter";
		relationType.RoleType.PluralName = "Counters";
		MetaDependent.Instance.Counter = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e971733a-c381-4b5e-8e62-6bbd6d285bd7"), new System.Guid("6269351a-5e08-4b10-a895-ff2f669b259f"), new System.Guid("2b916cdb-93a6-42f1-b4e6-625b941c1874"));
		relationType.AssociationType.ObjectType = MetaDependent.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Subcounter";
		relationType.RoleType.PluralName = "Subcounters";
		MetaDependent.Instance.Subcounter = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("525bbc9e-d488-419f-ac02-0ab6ac409bac"), new System.Guid("7dcdf3d7-25ad-4e8f-9634-63b771990681"), new System.Guid("bf9f7482-5277-40db-a6ac-5d4731cb5537"));
		relationType.AssociationType.ObjectType = MetaExtender.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsString";
		relationType.RoleType.PluralName = "AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaExtender.Instance.AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("24886999-11f0-408f-b094-14b36ac4129b"), new System.Guid("e48ab2ee-c7a5-4d9a-b3ab-263f6aa4cdd1"), new System.Guid("cf5c725d-e567-44de-ab5b-b47bb0bf8647"));
		relationType.AssociationType.ObjectType = MetaFirst.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSecond.Instance;
		relationType.RoleType.SingularName = "Second";
		relationType.RoleType.PluralName = "Seconds";
		MetaFirst.Instance.Second = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b0274351-3403-4384-afb6-2cb49cd03893"), new System.Guid("ec145229-e33a-4807-a0dd-48778cc88ac7"), new System.Guid("12c46bf1-eed0-4e2a-b704-5d40032b4911"));
		relationType.AssociationType.ObjectType = MetaFirst.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "CreateCycle";
		relationType.RoleType.PluralName = "CreateCycles";
		MetaFirst.Instance.CreateCycle = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f2b61dd5-d30c-445a-ae7a-af1c0cc8e278"), new System.Guid("ae9f23b5-20a7-4ecc-b642-503d75c486f1"), new System.Guid("eb6b0565-1440-4b9b-aa23-51cfae3f93dd"));
		relationType.AssociationType.ObjectType = MetaFirst.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsDerived";
		relationType.RoleType.PluralName = "IsDeriveds";
		MetaFirst.Instance.IsDerived = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d9a9896d-e175-410a-9916-9261d83aa229"), new System.Guid("a963f593-cad0-4fa9-96a3-3853f0f7d7c6"), new System.Guid("775a29b8-6e21-4545-9881-d52f6eb7db8b"));
		relationType.AssociationType.ObjectType = MetaFrom.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTo.Instance;
		relationType.RoleType.SingularName = "To";
		relationType.RoleType.PluralName = "Tos";
		MetaFrom.Instance.Tos = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6f0f42c4-9b47-47c2-a632-da8e08116be4"), new System.Guid("652a00b8-f708-4804-80b6-c1fe3211acf2"), new System.Guid("fc273b47-d98a-4afd-90ba-574fbdbfb395"));
		relationType.AssociationType.ObjectType = MetaHomeAddress.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Street";
		relationType.RoleType.PluralName = "Streets";
		relationType.RoleType.Size = 256;
		MetaHomeAddress.Instance.Street = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b181d077-e897-4add-9456-67b9760d32e8"), new System.Guid("5eca1733-0f01-4141-b0d0-d7a2bfd90388"), new System.Guid("d29dbed0-a68a-4075-b893-55e16e6335fd"));
		relationType.AssociationType.ObjectType = MetaHomeAddress.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "HouseNumber";
		relationType.RoleType.PluralName = "HouseNumbers";
		relationType.RoleType.Size = 256;
		MetaHomeAddress.Instance.HouseNumber = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("03c9970e-d9d6-427d-83d0-00e0888f5588"), new System.Guid("8d565792-4315-44eb-9930-55aa30e8f23a"), new System.Guid("10b46f89-7f3a-4571-8621-259a2a501dc7"));
		relationType.AssociationType.ObjectType = MetaMailboxAddress.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "PoBox";
		relationType.RoleType.PluralName = "PoBoxes";
		relationType.RoleType.Size = 256;
		MetaMailboxAddress.Instance.PoBox = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("448878af-c992-4256-baa7-239335a26bc6"), new System.Guid("2c9236ed-892e-4005-9730-5a14f03f71e1"), new System.Guid("355b2e85-e597-4f88-9dca-45cbfbde527f"));
		relationType.AssociationType.ObjectType = MetaOne.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTwo.Instance;
		relationType.RoleType.SingularName = "Two";
		relationType.RoleType.PluralName = "Twos";
		MetaOne.Instance.Two = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7CFAFE73-FEBD-4BFF-B42F-BE9ECF9E74DD"), new System.Guid("C03C9BBB-9590-44AA-BF5D-334B064752D7"), new System.Guid("3498CEC2-0AA8-4AFA-ABED-24C5FA5C8BED"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderState.Instance;
		relationType.RoleType.SingularName = "PreviousOrderState";
		relationType.RoleType.PluralName = "PreviousOrderStates";
		MetaOrder.Instance.PreviousOrderState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("427C6D78-2272-4069-A326-2F551812D6BD"), new System.Guid("12D93BE8-E0CD-4D87-ABC4-DA3741B8968B"), new System.Guid("74F2F239-B7B7-4713-8C18-E9282B49ED5B"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderState.Instance;
		relationType.RoleType.SingularName = "LastOrderState";
		relationType.RoleType.PluralName = "LastOrderStates";
		MetaOrder.Instance.LastOrderState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("B11EBAC9-5867-4A96-A59B-8A160614FFD6"), new System.Guid("0EA6C0AC-F40A-45AD-83EB-CC51EF382886"), new System.Guid("9233E129-7CDD-41F8-82A2-7CF90004799B"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderState.Instance;
		relationType.RoleType.SingularName = "OrderState";
		relationType.RoleType.PluralName = "OrderStates";
		MetaOrder.Instance.OrderState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("412BACF5-F927-42D0-BE29-F2870768FA76"), new System.Guid("C5F41CDE-77D8-4188-9715-84720DEA9848"), new System.Guid("46561AC8-3F7F-4BA2-A4EC-270D49B5211A"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaShipmentState.Instance;
		relationType.RoleType.SingularName = "PreviousShipmentState";
		relationType.RoleType.PluralName = "PreviousShipmentStates";
		MetaOrder.Instance.PreviousShipmentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6C724955-90CA-4069-ACF0-E2A228A928AD"), new System.Guid("3EC54D8B-43EA-476D-BC1A-BDE764BD0C2C"), new System.Guid("936320F5-3172-4864-808E-56468E405CED"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaShipmentState.Instance;
		relationType.RoleType.SingularName = "LastShipmentState";
		relationType.RoleType.PluralName = "LastShipmentStates";
		MetaOrder.Instance.LastShipmentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5FEE0701-6C67-478D-9763-25E1E1C70BA1"), new System.Guid("BDF89048-6AB3-4A75-A8B9-23C7211729A0"), new System.Guid("05CA7F49-65F6-4E53-92B9-866CB39ED059"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaShipmentState.Instance;
		relationType.RoleType.SingularName = "ShipmentState";
		relationType.RoleType.PluralName = "ShipmentStates";
		MetaOrder.Instance.ShipmentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("45981825-4E17-440A-9F60-9DE93DBCA7D3"), new System.Guid("61AC5469-1B08-4D51-A72A-84A44740D089"), new System.Guid("14F708A8-9809-40A4-9DC7-C38991EF9711"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPaymentState.Instance;
		relationType.RoleType.SingularName = "PreviousPaymentState";
		relationType.RoleType.PluralName = "PreviousPaymentStates";
		MetaOrder.Instance.PreviousPaymentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4E56EDF6-F45F-4CEC-8BDA-28536490503A"), new System.Guid("B7B0F3EC-E2C7-4650-BCC3-788D3EBBC240"), new System.Guid("E21C6C3D-A30B-48FC-BC9F-7B817F1B29D0"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPaymentState.Instance;
		relationType.RoleType.SingularName = "LastPaymentState";
		relationType.RoleType.PluralName = "LastPaymentStates";
		MetaOrder.Instance.LastPaymentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("BB076A8A-D2E6-47FA-A334-08B0E7E89F05"), new System.Guid("5B47A7FA-211B-475A-8741-3E2D020C3F9A"), new System.Guid("B8AA8155-7F01-4A32-A00A-CCF92F18A974"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPaymentState.Instance;
		relationType.RoleType.SingularName = "PaymentState";
		relationType.RoleType.PluralName = "PaymentStates";
		MetaOrder.Instance.PaymentState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4819AB04-B36F-42F8-B6DE-1F15FFC65233"), new System.Guid("8431642A-6874-4931-A4E1-CE696BF3AF84"), new System.Guid("F1456D98-BAC8-4C2F-9EA6-C3A5C8955621"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLine.Instance;
		relationType.RoleType.SingularName = "OrderLine";
		relationType.RoleType.PluralName = "OrderLines";
		MetaOrder.Instance.OrderLines = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5aa7fa5c-c0a5-4384-9b24-9ecef17c4848"), new System.Guid("ffcb8a00-571f-4032-b038-82b438f96f74"), new System.Guid("cf1629aa-2aa0-4dc3-9873-fbf3008352ac"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Amount";
		relationType.RoleType.PluralName = "Amounts";
		MetaOrder.Instance.Amount = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("B8F02B30-51A3-44CD-85A3-1E1E13DBC0A4"), new System.Guid("17D327FA-FFF5-40FC-AD7C-E2A57ACA7878"), new System.Guid("F4160293-1445-4033-8E6E-BED07EBC9A46"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderState.Instance;
		relationType.RoleType.SingularName = "NonVersionedCurrentObjectState";
		relationType.RoleType.PluralName = "NonVersionedCurrentObjectStates";
		MetaOrder.Instance.NonVersionedCurrentObjectState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1879ABB2-78D9-40AF-B404-6CEEF76C7EEC"), new System.Guid("CE5AF221-116D-4717-B167-9096A4864797"), new System.Guid("BEF6A273-AC77-4B7F-946D-B749449B4B68"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLine.Instance;
		relationType.RoleType.SingularName = "NonVersionedOrderLine";
		relationType.RoleType.PluralName = "NonVersionedOrderLines";
		MetaOrder.Instance.NonVersionedOrderLines = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("D237EF03-A748-4A89-A009-40D73EFBE9AA"), new System.Guid("741DF0CD-1204-450D-8A96-12D1CC24D47A"), new System.Guid("8FAE9C9C-98D5-44E0-944C-BD983CCFAC1B"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "NonVersionedAmount";
		relationType.RoleType.PluralName = "NonVersionedAmounts";
		MetaOrder.Instance.NonVersionedAmount = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4058FCBA-9323-47C5-B165-A3EED8DE70B6"), new System.Guid("7FD58473-6579-4269-A4A1-D1BFAE6B3542"), new System.Guid("DAB0E0A8-712B-4278-B635-92D367F4D41A"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderVersion.Instance;
		relationType.RoleType.SingularName = "CurrentVersion";
		relationType.RoleType.PluralName = "CurrentVersions";
		MetaOrder.Instance.CurrentVersion = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("DF0E52D4-07B3-45AC-9F36-2C0DE9802C2F"), new System.Guid("08A55411-57F6-4015-858D-BE9177328319"), new System.Guid("BF309243-98E3-457D-A396-3E6BCB06DE6A"));
		relationType.AssociationType.ObjectType = MetaOrder.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderVersion.Instance;
		relationType.RoleType.SingularName = "AllVersion";
		relationType.RoleType.PluralName = "AllVersions";
		MetaOrder.Instance.AllVersions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7022167A-046E-45B3-A14E-AE0290C0F1D6"), new System.Guid("39DC23C3-9F36-48AE-94E6-8401FBAF8A4F"), new System.Guid("7B145C97-85EB-4A51-ACEF-90B9A629EE31"));
		relationType.AssociationType.ObjectType = MetaOrderLine.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Amount";
		relationType.RoleType.PluralName = "Amounts";
		MetaOrderLine.Instance.Amount = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("55F3D531-C58D-4FA7-B745-9E38D8CEC4C6"), new System.Guid("8B5CE991-9CC0-4419-B5A7-E2803F888A8E"), new System.Guid("7663B87D-F17D-4822-A358-546124622073"));
		relationType.AssociationType.ObjectType = MetaOrderLine.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLineVersion.Instance;
		relationType.RoleType.SingularName = "CurrentVersion";
		relationType.RoleType.PluralName = "CurrentVersions";
		MetaOrderLine.Instance.CurrentVersion = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("CFC88B59-87A1-4F9E-ABBE-168694AB6CB5"), new System.Guid("2EA46390-F69F-436D-BCCC-84BEF6CD5997"), new System.Guid("03585BB0-E87E-474F-8A76-0644D5C858F4"));
		relationType.AssociationType.ObjectType = MetaOrderLine.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLineVersion.Instance;
		relationType.RoleType.SingularName = "AllVersion";
		relationType.RoleType.PluralName = "AllVersions";
		MetaOrderLine.Instance.AllVersions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0B9340C2-CE9B-48C7-A476-6D73B8829944"), new System.Guid("566324B9-A7B5-4C1D-AC89-2E228C603684"), new System.Guid("2C78F740-1D90-44BB-AFE6-3360399A1150"));
		relationType.AssociationType.ObjectType = MetaOrderLineVersion.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Amount";
		relationType.RoleType.PluralName = "Amounts";
		MetaOrderLineVersion.Instance.Amount = relationType.RoleType; 
	}



	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("88BE9AFA-122A-469B-BD47-388ECC835EAB"), new System.Guid("D8E59DF6-DC0C-4CAE-B0F2-402B2D927C5F"), new System.Guid("8CD4939F-285F-4983-9C31-2AEB2B89D732"));
		relationType.AssociationType.ObjectType = MetaOrderVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderState.Instance;
		relationType.RoleType.SingularName = "OrderState";
		relationType.RoleType.PluralName = "OrderStates";
		MetaOrderVersion.Instance.OrderState = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("F144557C-B63C-49F7-B713-F2493BCA1E55"), new System.Guid("E3CE3FA0-A40B-424D-907C-95907563EBD2"), new System.Guid("CAC9EEFE-8E17-4D9A-BF50-298B610D514C"));
		relationType.AssociationType.ObjectType = MetaOrderVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaOrderLine.Instance;
		relationType.RoleType.SingularName = "OrderLine";
		relationType.RoleType.PluralName = "OrderLines";
		MetaOrderVersion.Instance.OrderLines = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("49D672D8-3B3D-473A-B050-411251AE5365"), new System.Guid("723738EA-C2A9-4312-B9CF-A855FE71B836"), new System.Guid("6D0FF30F-FD3E-4AB5-B6D2-C7C6E1D4346E"));
		relationType.AssociationType.ObjectType = MetaOrderVersion.Instance;
		relationType.IsDerived = true;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "Amount";
		relationType.RoleType.PluralName = "Amounts";
		MetaOrderVersion.Instance.Amount = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("73f23588-1444-416d-b43c-b3384ca87bfc"), new System.Guid("d1a098bf-a3d8-4b71-948f-a77ae82f02db"), new System.Guid("a365f0ee-a94f-4435-a7b1-c92ac804a845"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAddress.Instance;
		relationType.RoleType.SingularName = "Address";
		relationType.RoleType.PluralName = "Addresses";
		MetaOrganisation.Instance.Addresses = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2cfea5d4-e893-4264-a966-a68716839acd"), new System.Guid("c3c93567-1d78-42ea-a8cf-77549cd1a235"), new System.Guid("d5965473-66cd-44b2-8048-a521c9cdadd0"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Description";
		relationType.RoleType.PluralName = "Descriptions";
		relationType.RoleType.Size = -1;
		MetaOrganisation.Instance.Description = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("49b96f79-c33d-4847-8c64-d50a6adb4985"), new System.Guid("b031ef1a-0102-4b19-b85d-aa9c404596c3"), new System.Guid("b95c7b34-a295-4600-82c8-826cc2186a00"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Employee";
		relationType.RoleType.PluralName = "Employees";
		MetaOrganisation.Instance.Employees = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("17e55fcd-2c82-462b-8e31-b4a515acdaa9"), new System.Guid("e6fc633a-de9d-42a5-af03-b2359b2c2ea4"), new System.Guid("6ab3328a-0fe1-4e98-b10d-eee420a90ffb"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMedia.Instance;
		relationType.RoleType.SingularName = "Image";
		relationType.RoleType.PluralName = "Images";
		MetaOrganisation.Instance.Images = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5fa25b53-e2a7-44c8-b6ff-f9575abb911d"), new System.Guid("6a382c73-c6a2-4d8b-bc85-4623ede54298"), new System.Guid("1c3dec18-978c-470a-8857-5210b9267185"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "Incorporated";
		relationType.RoleType.PluralName = "Incorporateds";
		MetaOrganisation.Instance.Incorporated = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7046c2b4-d458-4343-8446-d23d9c837c84"), new System.Guid("0671f523-a557-41e1-9d05-0e89d8d1ae2d"), new System.Guid("c84a6696-a1e9-4794-86c3-50e1f009c845"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "IncorporationDate";
		relationType.RoleType.PluralName = "IncorporationDates";
		MetaOrganisation.Instance.IncorporationDate = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("01dd273f-cbca-4ee7-8c2d-827808aba481"), new System.Guid("ffc3b92f-860a-4e45-90e1-b9ba7ab27a27"), new System.Guid("e567907e-ca61-4ec1-ab06-62dbb84e5d57"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Information";
		relationType.RoleType.PluralName = "Information";
		relationType.RoleType.Size = -1;
		MetaOrganisation.Instance.Information = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("68c61cea-4e6e-4ed5-819b-7ec794a10870"), new System.Guid("8494ad76-3422-4799-b5a6-caa077e53aca"), new System.Guid("90489246-8590-4578-8b8d-716a25abd27d"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsSupplier";
		relationType.RoleType.PluralName = "IsSuppliers";
		MetaOrganisation.Instance.IsSupplier = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b201d2a0-2335-47a1-aa8d-8416e89a9fec"), new System.Guid("e332003a-0287-4aab-9d95-257146ee4f1c"), new System.Guid("b1f5b479-e4d0-46de-8ad4-347076d9f180"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaMedia.Instance;
		relationType.RoleType.SingularName = "Logo";
		relationType.RoleType.PluralName = "Logos";
		MetaOrganisation.Instance.Logo = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ddcea177-0ed9-4247-93d3-2090496c130c"), new System.Guid("944d024b-81eb-442f-8f50-387a588d2373"), new System.Guid("2c3bc00d-6715-4c1b-be78-753f7f306df0"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAddress.Instance;
		relationType.RoleType.SingularName = "MainAddress";
		relationType.RoleType.PluralName = "MainAddresses";
		MetaOrganisation.Instance.MainAddress = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("dbef262d-7184-4b98-8f1f-cf04e884bb92"), new System.Guid("ed76a631-00c4-4753-b3d4-b3a53b9ecf4a"), new System.Guid("19de0627-fb1c-4f55-9b65-31d8008d0a48"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Manager";
		relationType.RoleType.PluralName = "Managers";
		MetaOrganisation.Instance.Manager = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2cc74901-cda5-4185-bcd8-d51c745a8437"), new System.Guid("896a4589-4caf-4cd2-8365-c4200b12f519"), new System.Guid("baa30557-79ff-406d-b374-9d32519b2de7"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaOrganisation.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("845ff004-516f-4ad5-9870-3d0e966a9f7d"), new System.Guid("3820f65f-0e79-4f30-a973-5d17dca6ad33"), new System.Guid("58d7df91-fbc5-4bcb-9398-a9957949402b"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Owner";
		relationType.RoleType.PluralName = "Owners";
		MetaOrganisation.Instance.Owner = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("15f33fa4-c878-45a0-b40c-c5214bce350b"), new System.Guid("4fdd9abb-f2e7-4f07-860e-27b4207224bd"), new System.Guid("45bef644-dfcf-417a-9356-3c1cfbcada1b"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Shareholder";
		relationType.RoleType.PluralName = "Shareholders";
		MetaOrganisation.Instance.Shareholders = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bac702b8-7874-45c3-a410-102e1caea4a7"), new System.Guid("8c2ce648-3942-4ead-9772-308c29bc905e"), new System.Guid("26a60588-3c90-4f4e-9bb6-8f45fe8f9606"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Size";
		relationType.RoleType.PluralName = "Sizes";
		relationType.RoleType.Size = 256;
		MetaOrganisation.Instance.Size = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("D3DB6E8C-9C10-47BA-92B1-45F5DDFFA5CC"), new System.Guid("4955AC7F-F840-4F24-B44C-C2D3937D2D44"), new System.Guid("9033AE73-83F6-4529-9F81-84FD9D35D597"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "CycleOne";
		relationType.RoleType.PluralName = "CycleOnes";
		MetaOrganisation.Instance.CycleOne = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("C6CCA1C5-5799-4517-87F5-095DA0EEEC64"), new System.Guid("6ABCD4E2-44A7-46B4-BD98-D052F38B7C50"), new System.Guid("E01ACE3C-2314-477C-8997-14266D9711E0"));
		relationType.AssociationType.ObjectType = MetaOrganisation.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "CycleMany";
		relationType.RoleType.PluralName = "CycleMany";
		MetaOrganisation.Instance.CycleMany = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1bf1cc1e-75bf-4a3f-87bd-a2fae2697855"), new System.Guid("dce03fde-fbb1-45e7-b78d-9484fa6487ff"), new System.Guid("d88eaaa2-2622-48ef-960a-1b506d95f238"));
		relationType.AssociationType.ObjectType = MetaPlace.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaCountry.Instance;
		relationType.RoleType.SingularName = "Country";
		relationType.RoleType.PluralName = "Countries";
		MetaPlace.Instance.Country = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d029f486-4bb8-43a1-8356-98b9bee10de4"), new System.Guid("1454029b-b016-41e1-b142-cea20c7b36d1"), new System.Guid("dccca416-913b-406a-9405-c5d037af2fd8"));
		relationType.AssociationType.ObjectType = MetaPlace.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "City";
		relationType.RoleType.PluralName = "Cities";
		relationType.RoleType.Size = 256;
		MetaPlace.Instance.City = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d80d7c6a-138a-43dd-9748-8ffb89b1dabb"), new System.Guid("944c752e-742c-426b-9ac9-c405080d4a8d"), new System.Guid("b54fcc51-e294-4732-82bf-a1117a4e2219"));
		relationType.AssociationType.ObjectType = MetaPlace.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "PostalCode";
		relationType.RoleType.PluralName = "PostalCodes";
		relationType.RoleType.Size = 256;
		MetaPlace.Instance.PostalCode = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4f0eba0d-09b4-4bbc-8e42-15de94921ab5"), new System.Guid("08d8689d-88ce-496d-95e4-f20af0677cac"), new System.Guid("ec263924-1234-4b53-9d33-91e167d6862f"));
		relationType.AssociationType.ObjectType = MetaSecond.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaThird.Instance;
		relationType.RoleType.SingularName = "Third";
		relationType.RoleType.PluralName = "Thirds";
		MetaSecond.Instance.Third = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8a7b7af9-f421-4e96-a1a7-04d4c4bdd1d7"), new System.Guid("e986349f-fc8c-4627-9bf7-966ad6299cff"), new System.Guid("3f37f82c-3f7a-4d4c-b775-4ff09c105f92"));
		relationType.AssociationType.ObjectType = MetaSecond.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsDerived";
		relationType.RoleType.PluralName = "IsDeriveds";
		MetaSecond.Instance.IsDerived = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7cd27660-13c6-4a15-8fd8-5775920cfd28"), new System.Guid("da384d02-5d30-4df5-acb5-ca36c895ef53"), new System.Guid("44b9e3cc-e584-48c0-bfec-916ab14e5f03"));
		relationType.AssociationType.ObjectType = MetaSimpleJob.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Index";
		relationType.RoleType.PluralName = "Indices";
		MetaSimpleJob.Instance.Index = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6c848eeb-7b42-45ea-81ac-fa983e1e0fa9"), new System.Guid("be566287-a26d-46fb-a4f2-1fc8bf1c1de4"), new System.Guid("2a482b25-a154-4306-87f3-b6cd7af3c80d"));
		relationType.AssociationType.ObjectType = MetaStatefulCompany.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Employee";
		relationType.RoleType.PluralName = "Employees";
		MetaStatefulCompany.Instance.Employee = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6e429d87-ea80-465e-9aa6-0f7d546b6bb3"), new System.Guid("de607129-6f68-4db6-a6ca-6ba53cae698d"), new System.Guid("94570d2c-2a5e-451f-905e-6ca61a469a31"));
		relationType.AssociationType.ObjectType = MetaStatefulCompany.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaStatefulCompany.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9940e8ed-189e-42c6-b0d1-7c01920b9fac"), new System.Guid("de4a92c8-4e08-4f37-9d6c-321dcce89e1c"), new System.Guid("3becaaa8-7b49-4616-8d79-a7bf04d9e666"));
		relationType.AssociationType.ObjectType = MetaStatefulCompany.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Manager";
		relationType.RoleType.PluralName = "Managers";
		MetaStatefulCompany.Instance.Manager = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("194930f9-9c3f-458d-93ec-3d7bea4cd538"), new System.Guid("63ed21ba-b310-43fc-afed-a3eeea918204"), new System.Guid("6765f2b5-bf55-4713-a693-946fc0846b27"));
		relationType.AssociationType.ObjectType = MetaSubdependee.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "Subcounter";
		relationType.RoleType.PluralName = "Subcounters";
		MetaSubdependee.Instance.Subcounter = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6ab5a7af-a0f0-4940-9be3-6f6430a9e728"), new System.Guid("a18d4c53-ba36-4936-8650-0d90182e5948"), new System.Guid("7866ac81-e84d-40c6-b9c0-5a038b1e838f"));
		relationType.AssociationType.ObjectType = MetaThird.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsDerived";
		relationType.RoleType.PluralName = "IsDeriveds";
		MetaThird.Instance.IsDerived = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1697f09c-0d3d-4e5e-9f3f-9d3ae0718fd3"), new System.Guid("dc813d9a-84e9-4995-8d2c-0ef449b12024"), new System.Guid("25737278-d039-47c5-8749-19f22ad7a4c3"));
		relationType.AssociationType.ObjectType = MetaThree.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaFour.Instance;
		relationType.RoleType.SingularName = "Four";
		relationType.RoleType.PluralName = "Fours";
		MetaThree.Instance.Four = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4ace9948-4a22-465c-aa40-61c8fd65784d"), new System.Guid("6e20b25f-3ecd-447e-8a93-3977a53452b6"), new System.Guid("f8f85b3d-371c-42df-8414-cf034c339917"));
		relationType.AssociationType.ObjectType = MetaThree.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsString";
		relationType.RoleType.PluralName = "AllorsStrings";
		relationType.RoleType.Size = -1;
		MetaThree.Instance.AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4be564ac-77bc-48b8-b945-7d39f2ea9903"), new System.Guid("7a6714c1-e58a-45ac-8ee5-ca5f22b6d528"), new System.Guid("53e0761a-a9f1-4516-a086-b766650ac28b"));
		relationType.AssociationType.ObjectType = MetaTo.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaTo.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8930c13c-ad5a-4b0e-b3bf-d7cdf6f5b867"), new System.Guid("fd97db6d-d946-47ba-a2a0-88b732457b96"), new System.Guid("39eda296-4e8d-492b-b0c1-756ffcf9a493"));
		relationType.AssociationType.ObjectType = MetaTwo.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaShared.Instance;
		relationType.RoleType.SingularName = "Shared";
		relationType.RoleType.PluralName = "Shareds";
		MetaTwo.Instance.Shared = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("24771d5b-f920-4820-aff7-ea6391b4a45c"), new System.Guid("fe3aa333-e011-4a1e-85dc-ded48329cf00"), new System.Guid("4d4428fc-bac0-47af-ab5e-7c7b87880206"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "AllorsBinary";
		relationType.RoleType.PluralName = "AllorsBinaries";
		MetaUnitSample.Instance.AllorsBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4d6a80f5-0fa7-4867-91f8-37aa92b6707b"), new System.Guid("13f88cf7-aaec-48a1-a896-401df84da34b"), new System.Guid("a462ce40-5885-48c6-b327-7e4c096a99fa"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "AllorsDateTime";
		relationType.RoleType.PluralName = "AllorsDateTimes";
		MetaUnitSample.Instance.AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5a788ebe-65e9-4d5e-853a-91bb4addabb5"), new System.Guid("7620281d-3d8a-470a-9258-7a6d1b818b46"), new System.Guid("b5dd13eb-8923-4a66-94df-af5fadb42f1c"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "AllorsBoolean";
		relationType.RoleType.PluralName = "AllorsBooleans";
		MetaUnitSample.Instance.AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("74a35820-ef8c-4373-9447-6215ee8279c0"), new System.Guid("e5f7a565-372a-42ed-8da5-ffe6dd599f70"), new System.Guid("4a95fb0d-6849-499e-a140-6c942fb06f4d"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "AllorsDouble";
		relationType.RoleType.PluralName = "AllorsDoubles";
		MetaUnitSample.Instance.AllorsDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b817ba76-876e-44ea-8e5a-51d552d4045e"), new System.Guid("80683240-71d5-4329-abd0-87c367b44fec"), new System.Guid("07070cb0-6e65-4a00-8754-50cf594ed9e1"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "AllorsInteger";
		relationType.RoleType.PluralName = "AllorsIntegers";
		MetaUnitSample.Instance.AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c724c733-972a-411c-aecb-e865c2628a90"), new System.Guid("e4917fda-a605-4f6f-8f63-579ec688b629"), new System.Guid("f27c150a-ce8d-4ff3-9507-ccb0b91aa0c2"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "AllorsString";
		relationType.RoleType.PluralName = "AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaUnitSample.Instance.AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ed58ae4c-24e0-4dd1-8b1c-0909df1e0fcd"), new System.Guid("f117e164-ce37-4c12-a79e-38cda962adae"), new System.Guid("25dd4abf-c6da-4739-aed0-8528d1c00b8b"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "AllorsUnique";
		relationType.RoleType.PluralName = "AllorsUniques";
		MetaUnitSample.Instance.AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f746da51-ea2d-4e22-9ecb-46d4dbc1b084"), new System.Guid("3936ee9b-3bd6-44de-9340-4047749a6c2c"), new System.Guid("1408cd42-3125-48c7-86d7-4a5f71e75e25"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "AllorsDecimal";
		relationType.RoleType.PluralName = "AllorsDecimals";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaUnitSample.Instance.AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6E05C521-B90A-459E-931A-940B4D769C6A"), new System.Guid("A3EBF97C-B23A-46C5-AA34-AC81F97089A4"), new System.Guid("EF528E3E-065C-4762-AB4A-637B285A89EB"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "RequiredBinary";
		relationType.RoleType.PluralName = "RequiredBinaries";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0A17B766-9A60-4061-8FCB-AADFC6C13FAF"), new System.Guid("07516E8D-E5D2-4975-82B2-94BD419F061D"), new System.Guid("6E4AA664-3F19-46C1-BA3E-C220E62A9800"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "RequiredDateTime";
		relationType.RoleType.PluralName = "RequiredDateTimes";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("22BEF3E8-1178-4717-9BD1-D6F34569B63C"), new System.Guid("B6698E9B-E371-4906-97F5-C44E18155FDA"), new System.Guid("520E7D24-AEFB-4FE9-BE12-69823E2F1C37"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "RequiredBoolean";
		relationType.RoleType.PluralName = "RequiredBooleans";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("FAC655F6-6D89-4CE5-B8E9-388F35294DD0"), new System.Guid("EA8C33BC-450F-4DB3-A76C-CEC9AEF751CB"), new System.Guid("4D837324-8433-491E-9E0C-85959EE087F7"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "RequiredDouble";
		relationType.RoleType.PluralName = "RequiredDoubles";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("3257637E-CE68-49B8-879C-E428810DD316"), new System.Guid("9AD8FC7A-8645-4D61-AC8B-27B048BB920F"), new System.Guid("5FA9588F-D201-4A85-9A8F-708095D96F1A"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "RequiredInteger";
		relationType.RoleType.PluralName = "RequiredIntegers";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("38405B1B-8469-47D9-BDDF-66B753F52A52"), new System.Guid("52463620-5352-4577-97B9-07A662FB0D10"), new System.Guid("34A1060C-BC52-4051-ACB5-BFF3A55C8300"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "RequiredString";
		relationType.RoleType.PluralName = "RequiredStrings";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("336175A6-29FE-4A6A-A21E-5F3B97BFF99D"), new System.Guid("D9E3E7DE-07DB-4243-9311-4220DB6E767B"), new System.Guid("59D7AE57-E7D9-4921-97F5-A1BD02A7E632"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "RequiredUnique";
		relationType.RoleType.PluralName = "RequiredUniques";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("A5905304-6BB6-4B15-85F7-8C4225D6E6B9"), new System.Guid("DC6E60C0-B3D2-43C2-A6BF-222D1652D6D5"), new System.Guid("B87C5F55-5F37-4709-9214-571A2E4C2BC2"));
		relationType.AssociationType.ObjectType = MetaUnitSample.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "RequiredDecimal";
		relationType.RoleType.PluralName = "RequiredDecimals";
		relationType.RoleType.IsRequired = true;
		MetaUnitSample.Instance.RequiredDecimal = relationType.RoleType; 
	}



	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("EF6F1F4C-5B62-49DC-9D05-0F02973ACCB3"), new System.Guid("1137FDD3-07E6-432E-8C42-273EF24863D5"), new System.Guid("D6A473F7-4EFF-4D3D-BDB2-59F5EE8B0E52"));
		relationType.AssociationType.ObjectType = MetaCachable.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "CacheId";
		relationType.RoleType.PluralName = "CacheIds";
		relationType.RoleType.IsRequired = true;
		MetaCachable.Instance.CacheId = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9FAEB940-A3A0-4E7A-B889-BCFD92F6A882"), new System.Guid("4C4BD3D4-6642-48AA-8C29-46C02DCDC749"), new System.Guid("FD06C364-1033-423C-B297-DC6EDF15F4FD"));
		relationType.AssociationType.ObjectType = MetaVersion.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "DerivationId";
		relationType.RoleType.PluralName = "DerivationIds";
		MetaVersion.Instance.DerivationId = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ADF611C3-047A-4BAE-95E3-776022D5CE7B"), new System.Guid("7145B062-AEE9-4B30-ADB8-C691969C6874"), new System.Guid("B38C700C-7AD9-4962-9F53-35B8AEF22E09"));
		relationType.AssociationType.ObjectType = MetaVersion.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DerivationTimeStamp";
		relationType.RoleType.PluralName = "DerivationTimeStamps";
		MetaVersion.Instance.DerivationTimeStamp = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5c70ca14-4601-4c65-9b0d-cb189f90be27"), new System.Guid("267053f0-43b4-4cc7-a0e2-103992b2d0c5"), new System.Guid("867765fa-49dc-462f-b430-3c0e264c5283"));
		relationType.AssociationType.ObjectType = MetaAccessControlledObject.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPermission.Instance;
		relationType.RoleType.SingularName = "DeniedPermission";
		relationType.RoleType.PluralName = "DeniedPermissions";
		MetaAccessControlledObject.Instance.DeniedPermissions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b816fccd-08e0-46e0-a49c-7213c3604416"), new System.Guid("1739db0d-fe6b-42e1-a6a5-286536ff4f56"), new System.Guid("9f722315-385a-42ab-b84e-83063b0e5b0d"));
		relationType.AssociationType.ObjectType = MetaAccessControlledObject.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSecurityToken.Instance;
		relationType.RoleType.SingularName = "SecurityToken";
		relationType.RoleType.PluralName = "SecurityTokens";
		MetaAccessControlledObject.Instance.SecurityTokens = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("07e034f1-246a-4115-9662-4c798f31343f"), new System.Guid("bcf428fd-0263-488c-b9ac-963ceca1c972"), new System.Guid("919fdad7-830e-4b12-b23c-f433951236af"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocalisedText.Instance;
		relationType.RoleType.SingularName = "LocalisedName";
		relationType.RoleType.PluralName = "LocalisedNames";
		MetaEnumeration.Instance.LocalisedNames = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("3d3ae4d0-bac6-4645-8a53-3e9f7f9af086"), new System.Guid("004cc333-b8ae-4952-ae13-f2ab80eb018c"), new System.Guid("5850860d-c772-402f-815b-7634c9a1e697"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.IsRequired = true;
		relationType.RoleType.Size = 256;
		MetaEnumeration.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f57bb62e-77a8-4519-81e6-539d54b71cb7"), new System.Guid("a8993304-52c0-4b53-9982-6caa5675467a"), new System.Guid("0c6faf5a-eac9-454c-bd53-3b8409e56d34"));
		relationType.AssociationType.ObjectType = MetaEnumeration.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "IsActive";
		relationType.RoleType.PluralName = "IsActives";
		relationType.RoleType.IsRequired = true;
		MetaEnumeration.Instance.IsActive = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8c005a4e-5ffe-45fd-b279-778e274f4d83"), new System.Guid("6684d98b-cd43-4612-bf9d-afefe02a0d43"), new System.Guid("d43b92ac-9e6f-4238-9625-1e889be054cf"));
		relationType.AssociationType.ObjectType = MetaLocalised.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaLocale.Instance;
		relationType.RoleType.SingularName = "Locale";
		relationType.RoleType.PluralName = "Locales";
		relationType.RoleType.IsRequired = true;
		MetaLocalised.Instance.Locale = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("59338f0b-40e7-49e8-ba1a-3ecebf96aebe"), new System.Guid("fca0f3f6-bdd6-4405-93b3-35dd769bff0e"), new System.Guid("c338f087-559c-4239-9c6a-1f691e58ed16"));
		relationType.AssociationType.ObjectType = MetaObjectState.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPermission.Instance;
		relationType.RoleType.SingularName = "DeniedPermission";
		relationType.RoleType.PluralName = "DeniedPermissions";
		MetaObjectState.Instance.DeniedPermissions = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b86f9e42-fe10-4302-ab7c-6c6c7d357c39"), new System.Guid("052ec640-3150-458a-99d5-0edce6eb6149"), new System.Guid("945cbba6-4b09-4b87-931e-861b147c3823"));
		relationType.AssociationType.ObjectType = MetaObjectState.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaObjectState.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5fb15e8b-011c-46f7-83dd-485d4cc4f9f2"), new System.Guid("cdc21c1c-918e-4622-a01f-a3de06a8c802"), new System.Guid("2acda9b3-89e8-475f-9d70-b9cde334409c"));
		relationType.AssociationType.ObjectType = MetaSecurityTokenOwner.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSecurityToken.Instance;
		relationType.RoleType.SingularName = "OwnerSecurityToken";
		relationType.RoleType.PluralName = "OwnerSecurityTokens";
		relationType.RoleType.IsRequired = true;
		MetaSecurityTokenOwner.Instance.OwnerSecurityToken = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("056914ed-a658-4ae5-b859-97300e1b8911"), new System.Guid("04b811b2-71b1-46a9-9ef5-1c061a035f07"), new System.Guid("ea2ecc92-0657-4ae9-a21d-4487353e7d00"));
		relationType.AssociationType.ObjectType = MetaSecurityTokenOwner.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaAccessControl.Instance;
		relationType.RoleType.SingularName = "OwnerAccessControl";
		relationType.RoleType.PluralName = "OwnerAccessControls";
		MetaSecurityTokenOwner.Instance.OwnerAccessControl = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("96685F17-ABE3-459C-BF9F-8C5F05788C04"), new System.Guid("40D11625-EF9F-4358-9FC0-5C29248E41DA"), new System.Guid("3893BB57-1EA6-4DEE-8248-483269CA30DA"));
		relationType.AssociationType.ObjectType = MetaTransitionalVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsSynced = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "PreviousObjectState";
		relationType.RoleType.PluralName = "PreviousObjectStates";
		MetaTransitionalVersion.Instance.PreviousObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("39C43EB4-AA16-4CF8-93A0-60066CB746E8"), new System.Guid("AEB8A1DC-D214-429E-9A78-6FD60B419BE0"), new System.Guid("DAC764A7-417C-4E24-985C-63171F7DC347"));
		relationType.AssociationType.ObjectType = MetaTransitionalVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsSynced = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "LastObjectState";
		relationType.RoleType.PluralName = "LastObjectStates";
		MetaTransitionalVersion.Instance.LastObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("F2472C1F-8D2A-4400-B372-34C2B03207B6"), new System.Guid("08C19B44-2015-4BCA-B0E2-AB8CA626485F"), new System.Guid("6C37AE50-8727-4391-A0E8-3596D5E2070F"));
		relationType.AssociationType.ObjectType = MetaTransitionalVersion.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsSynced = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "ObjectState";
		relationType.RoleType.PluralName = "ObjectStates";
		MetaTransitionalVersion.Instance.ObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("D9D86241-5FC7-4EDB-9FAA-FF5CA291F16C"), new System.Guid("C6D64EB2-4921-4AD9-9DC3-12BDCB8E7D97"), new System.Guid("292A7D78-3DA8-401C-A4D1-513F61114615"));
		relationType.AssociationType.ObjectType = MetaTransitional.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "PreviousObjectState";
		relationType.RoleType.PluralName = "PreviousObjectStates";
		MetaTransitional.Instance.PreviousObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2BC8AFDF-92BE-4088-9E35-C1C942CFE74B"), new System.Guid("549BC4A5-42B5-46D8-B487-9D1255BC1B8E"), new System.Guid("CA573AAD-72CC-4315-971D-43526D1A964B"));
		relationType.AssociationType.ObjectType = MetaTransitional.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "LastObjectState";
		relationType.RoleType.PluralName = "LastObjectStates";
		MetaTransitional.Instance.LastObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("52962C45-8A3E-4136-A968-C333CBE12685"), new System.Guid("B49A45EE-302E-4893-BEAD-88764D0774FF"), new System.Guid("08BBEF2B-47A4-48B0-86E2-522F3B584426"));
		relationType.AssociationType.ObjectType = MetaTransitional.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaObjectState.Instance;
		relationType.RoleType.SingularName = "ObjectState";
		relationType.RoleType.PluralName = "ObjectStates";
		MetaTransitional.Instance.ObjectStates = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e1842d87-8157-40e7-b06e-4375f311f2c3"), new System.Guid("fe413e96-cfcf-4e8d-9f23-0fa4f457fdf1"), new System.Guid("d73fd9a4-13ee-4fa9-8925-d93eca328bf6"));
		relationType.AssociationType.ObjectType = MetaUniquelyIdentifiable.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "UniqueId";
		relationType.RoleType.PluralName = "UniqueIds";
		relationType.RoleType.IsRequired = true;
		MetaUniquelyIdentifiable.Instance.UniqueId = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5e8ab257-1a1c-4448-aacc-71dbaaba525b"), new System.Guid("eca7ef36-8928-4116-bfce-1896a685fe8c"), new System.Guid("3b7d40a0-18ea-4018-b797-6417723e1890"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "UserName";
		relationType.RoleType.PluralName = "UserNames";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.UserName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7397B596-D8FA-4E3C-8E0E-EA24790FE2E4"), new System.Guid("19CAD82C-6538-4C46-AA3F-75C082CC8204"), new System.Guid("FAF89920-880F-4600-BAF1-A27A5268444A"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "NormalizedUserName";
		relationType.RoleType.PluralName = "NormalizedUserNames";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.NormalizedUserName = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ea0c7596-c0b8-4984-bc25-cb4b4857954e"), new System.Guid("8537ddb5-8ce2-4f35-a16f-207f2519ba9c"), new System.Guid("75ee3ec2-02bb-4666-a6f0-bac84c844dfa"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "UserPasswordHash";
		relationType.RoleType.PluralName = "UserPasswordHashes";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.UserPasswordHash = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c1ae3652-5854-4b68-9890-a954067767fc"), new System.Guid("111104a2-1181-4958-92f6-6528cef79af7"), new System.Guid("58e35754-91a9-4956-aa66-ca48d05c7042"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "UserEmail";
		relationType.RoleType.PluralName = "UserEmails";
		relationType.RoleType.Size = 256;
		MetaUser.Instance.UserEmail = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0b3b650b-fcd4-4475-b5c4-e2ee4f39b0be"), new System.Guid("c89a8e3f-6f76-41ac-b4dc-839f9080d917"), new System.Guid("1b1409b8-add7-494c-a895-002fc969ac7b"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "UserEmailConfirmed";
		relationType.RoleType.PluralName = "UserEmailConfirmeds";
		MetaUser.Instance.UserEmailConfirmed = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b4c09bec-168f-4f05-8ec6-919d1a24ae96"), new System.Guid("3d05bc18-c205-424a-ab26-fec24eafbd78"), new System.Guid("484ecaae-3f39-451b-a689-a55601df6778"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaTaskList.Instance;
		relationType.RoleType.SingularName = "TaskList";
		relationType.RoleType.PluralName = "TaskLists";
		MetaUser.Instance.TaskList = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bed34563-4ed8-4c6b-88d2-b4199e521d74"), new System.Guid("e678c2f8-5c66-4886-ad21-2be98101f759"), new System.Guid("79e9a907-9237-4aab-9340-277d593748f5"));
		relationType.AssociationType.ObjectType = MetaUser.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaNotificationList.Instance;
		relationType.RoleType.SingularName = "NotificationList";
		relationType.RoleType.PluralName = "NotificationLists";
		MetaUser.Instance.NotificationList = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f247de73-70fe-47e4-a763-22ee9c68a476"), new System.Guid("2e1ebe97-52d3-46fc-94c2-3203a13856c7"), new System.Guid("4ca8997f-9232-4c84-8f37-e977071eb316"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.Workspace = true;
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
		var relationType = new RelationType(this.metaPopulation, new System.Guid("8ebd9048-a344-417c-bae7-359ca9a74aa1"), new System.Guid("af6cbf34-5f71-498b-a2ec-ef698eeae799"), new System.Guid("ceba2888-2a6e-4822-881b-1101b48f80f3"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateCreated";
		relationType.RoleType.PluralName = "DateCreateds";
		relationType.RoleType.IsRequired = true;
		MetaTask.Instance.DateCreated = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5ad0b9f5-669c-4b05-8c97-89b59a227da2"), new System.Guid("b3182870-cbe0-4da1-aaeb-804df5a9f869"), new System.Guid("eacac26b-fea7-49f8-abb6-57d63accd548"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.Workspace = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "DateClosed";
		relationType.RoleType.PluralName = "DateCloseds";
		MetaTask.Instance.DateClosed = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("55375d57-34b0-43d0-9fac-e9788e1b6cd2"), new System.Guid("0d421578-35fc-4309-b8b6-cda0c9cf948c"), new System.Guid("a7c8f58f-358a-4ae9-9299-0ef560190541"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Participant";
		relationType.RoleType.PluralName = "Participants";
		MetaTask.Instance.Participants = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ea8abc59-b625-4d25-85bd-dd04bfe55086"), new System.Guid("90150444-fc18-47fd-a6fd-7740006e64ca"), new System.Guid("34320d76-6803-4615-8444-cc3ea8bb0315"));
		relationType.AssociationType.ObjectType = MetaTask.Instance;
		relationType.Workspace = true;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPerson.Instance;
		relationType.RoleType.SingularName = "Performer";
		relationType.RoleType.PluralName = "Performers";
		MetaTask.Instance.Performer = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7e6d392b-00e7-4095-8525-d9f4ef8cfaa3"), new System.Guid("b20f1b54-87a4-4fc2-91db-8848d6d40ad1"), new System.Guid("cf456f4d-8c76-4bfe-9996-89b660c9b153"));
		relationType.AssociationType.ObjectType = MetaWorkItem.Instance;
		relationType.Workspace = true;
		relationType.IsDerived = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "WorkItemDescription";
		relationType.RoleType.PluralName = "WorkItemDescriptions";
		relationType.RoleType.Size = 256;
		MetaWorkItem.Instance.WorkItemDescription = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("36e7d935-a9c7-484d-8551-9bdc5bdeab68"), new System.Guid("113a8abd-e587-45a3-b118-92e60182c94b"), new System.Guid("4f7016f6-1b87-4ac4-8363-7f8210108928"));
		relationType.AssociationType.ObjectType = MetaAddress.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaPlace.Instance;
		relationType.RoleType.SingularName = "Place";
		relationType.RoleType.PluralName = "Places";
		MetaAddress.Instance.Place = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("06b72534-49a8-4f6d-a991-bc4aaf6f939f"), new System.Guid("854c6ec4-51d4-4d68-bd26-4168c26707de"), new System.Guid("9fd09ce4-3f52-4889-b018-fd9374656e8c"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I1I1Many2One";
		relationType.RoleType.PluralName = "I1I1Many2Ones";
		MetaI1.Instance.I1I1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0a2895ec-0102-493d-9b94-e12e94b4a403"), new System.Guid("295bfc0e-e123-4ac8-84da-45e8d77b1865"), new System.Guid("94c8ca3f-45d6-4f70-8b4a-5d469b0ee897"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I1I12Many2Many";
		relationType.RoleType.PluralName = "I1I12Many2Manies";
		MetaI1.Instance.I1I12Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0acbea28-f8aa-477c-b296-b8976d9b10a5"), new System.Guid("5b4da68a-6aeb-4d5c-8e09-5bef3b1358a9"), new System.Guid("5e8608ed-7987-40d0-a877-a244d6520554"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I1I2Many2Many";
		relationType.RoleType.PluralName = "I1I2Many2Manies";
		MetaI1.Instance.I1I2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("194580f4-e0e3-4b52-b9ba-6020171be4e9"), new System.Guid("39a81eb4-e1bb-45ef-8126-21cf233ba684"), new System.Guid("98017570-bc3b-442b-9e51-b16565fa443c"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I1I2Many2One";
		relationType.RoleType.PluralName = "I1I2Many2Ones";
		MetaI1.Instance.I1I2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("28ceffc2-c776-4a0a-9825-a6d1bcb265dc"), new System.Guid("0287a603-59e5-4241-8b2e-a21698476e67"), new System.Guid("fec573a7-5ab3-4f30-9b50-7d720b4af4b4"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.Workspace = true;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "I1AllorsString";
		relationType.RoleType.PluralName = "I1AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaI1.Instance.I1AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("2e85d74a-8d13-4bc0-ae4f-42b305e79373"), new System.Guid("d6ccfcb8-623e-4852-a878-d7cb377af853"), new System.Guid("ec030f88-1060-4c2b-bda1-d9c5dc4fc9d3"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I1I12Many2One";
		relationType.RoleType.PluralName = "I1I12Many2Ones";
		MetaI1.Instance.I1I12Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("32fc21cc-4be7-4a0e-ac71-df135be95e68"), new System.Guid("e0006bdc-74e2-4067-871c-6f0b53eba5de"), new System.Guid("12824c37-d0d2-4cb9-9481-cad7f5f54976"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "I1AllorsDateTime";
		relationType.RoleType.PluralName = "I1AllorsDateTimes";
		MetaI1.Instance.I1AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("39e28141-fd6b-4f49-8884-d5400f6c57ff"), new System.Guid("9118c09c-e8c2-4685-a464-9be9ece2e746"), new System.Guid("a4b456e2-b45f-4398-875b-4ba99ead49fe"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I1I2One2Many";
		relationType.RoleType.PluralName = "I1I2One2Manies";
		MetaI1.Instance.I1I2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4506a14b-22f1-41fe-972b-40fab7c6dd31"), new System.Guid("54c659d3-98ff-45e6-b734-bc45f13428d8"), new System.Guid("d75a5613-4ed9-494f-accf-352d9e115ba9"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I1C2One2Many";
		relationType.RoleType.PluralName = "I1C2One2Manies";
		MetaI1.Instance.I1C2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("593914b1-af95-4992-9703-2b60f4ea0926"), new System.Guid("ee0f3844-928b-4968-9077-afd255554c8b"), new System.Guid("bca02f1e-a026-4c0b-9762-1bd52d49b953"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I1C1One2One";
		relationType.RoleType.PluralName = "I1C1One2Ones";
		MetaI1.Instance.I1C1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5cb44331-fd8c-4f73-8994-161f702849b6"), new System.Guid("2484aae6-db3b-4795-be76-016b33cbb679"), new System.Guid("c9f9dd15-54b4-4847-8b7e-ac88063804a3"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "I1AllorsInteger";
		relationType.RoleType.PluralName = "I1AllorsIntegers";
		MetaI1.Instance.I1AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6199e5b4-133d-4d0e-9941-207316164ec8"), new System.Guid("75342efb-659c-43a9-8340-1e110087141c"), new System.Guid("920f26a7-971a-4771-81b1-af3972c997ff"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I1C2Many2Many";
		relationType.RoleType.PluralName = "I1C2Many2Manies";
		MetaI1.Instance.I1C2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("670c753e-8ea0-40b1-bfc9-7388074191d3"), new System.Guid("b1c6c329-09e3-4b07-8ddf-e6a4fd8d0285"), new System.Guid("6d36c9f9-1426-46a5-8d4f-7275a51c9c17"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I1I1One2Many";
		relationType.RoleType.PluralName = "I1I1One2Manies";
		MetaI1.Instance.I1I1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6bb3ba6d-ffc7-4700-9723-c323b9b2d233"), new System.Guid("86623fe9-c7cc-4328-85d9-b0dfce2b9a59"), new System.Guid("9c64a761-136a-43aa-bef9-6bcd1259d591"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I1I1Many2Many";
		relationType.RoleType.PluralName = "I1I1Many2Manies";
		MetaI1.Instance.I1I1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6c3d04be-6f95-44b8-863a-245e150e3110"), new System.Guid("e6c314af-d366-4169-b28d-9dc83d694079"), new System.Guid("631a2bdb-ceca-43b2-abb8-9c9ea743c9de"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "I1AllorsBoolean";
		relationType.RoleType.PluralName = "I1AllorsBooleans";
		MetaI1.Instance.I1AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("818b4013-5ef1-4455-9f0d-9a39fa3425bb"), new System.Guid("335902bc-6bfa-4c7b-b52f-9a617c746afd"), new System.Guid("56e68d93-a62f-4090-a93a-8f0f364b08bd"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "I1AllorsDecimal";
		relationType.RoleType.PluralName = "I1AllorsDecimals";
		relationType.RoleType.Precision = 10;
		relationType.RoleType.Scale = 2;
		MetaI1.Instance.I1AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a51d9d21-40ec-44b9-853d-8c18f54d659d"), new System.Guid("1d785350-3f68-4f8d-86d4-74a0cd8adac7"), new System.Guid("222d2644-197d-4420-a01a-276b35ad61d1"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I1I12One2One";
		relationType.RoleType.PluralName = "I1I12One2Ones";
		MetaI1.Instance.I1I12One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a5761a0e-5c10-407a-bd68-0c4f69d78968"), new System.Guid("b6cf882a-e27a-40e3-9a0d-43ade4d236b6"), new System.Guid("3950129b-6ac5-4eae-b5c2-de12500b0561"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I1I2One2One";
		relationType.RoleType.PluralName = "I1I2One2Ones";
		MetaI1.Instance.I1I2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b6e0fce0-14fc-46e3-995d-1b6e3699ed96"), new System.Guid("ddc18ebf-0b61-441f-854a-0f964859035e"), new System.Guid("3899bad1-d563-4f65-85b1-2b274b6a278f"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I1C2One2One";
		relationType.RoleType.PluralName = "I1C2One2Ones";
		MetaI1.Instance.I1C2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b89092f1-8775-4b6a-99ef-f8626bc770bd"), new System.Guid("d0b99a68-2104-4c4d-ba4c-73d725e406e9"), new System.Guid("6303d423-6cc4-4933-9546-4b6b39fa0ae4"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I1C1One2Many";
		relationType.RoleType.PluralName = "I1C1One2Manies";
		MetaI1.Instance.I1C1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b9c67658-4abc-41f3-9434-c8512a482179"), new System.Guid("ba4fa583-b169-4327-a60a-fc0d2c142b3f"), new System.Guid("bbd469af-25f5-47aa-86f6-80d3bba53ce5"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "I1AllorsBinary";
		relationType.RoleType.PluralName = "I1AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaI1.Instance.I1AllorsBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bcc9eee6-fa07-4d37-be84-b691bfce24be"), new System.Guid("b6c7354a-4997-4764-826a-0c9989431d3b"), new System.Guid("7da3b7ea-2e1a-400c-adbf-436d35720ae9"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I1C1Many2Many";
		relationType.RoleType.PluralName = "I1C1Many2Manies";
		MetaI1.Instance.I1C1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cdb758bf-ecaf-4d99-88fb-58df9258c13c"), new System.Guid("62961c44-f0ab-4edf-9aa7-63312643e6b4"), new System.Guid("e33e809e-bbd3-4ecc-b46e-e233c5c93ce6"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "I1AllorsDouble";
		relationType.RoleType.PluralName = "I1AllorsDoubles";
		MetaI1.Instance.I1AllorsDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e1b13216-7210-4c24-a668-83b40162a21b"), new System.Guid("f14f50da-635f-47d0-9f3d-28364b767637"), new System.Guid("911abf5b-ea84-4ffe-b6fb-558b4af50503"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I1I1One2One";
		relationType.RoleType.PluralName = "I1I1One2Ones";
		MetaI1.Instance.I1I1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e3126228-342a-4415-a2e8-d52eceaeaf89"), new System.Guid("202575b6-aaff-46ce-9e8a-e976a8a9d263"), new System.Guid("2598d7df-a764-4b6e-bf91-5234404b97c2"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I1C1Many2One";
		relationType.RoleType.PluralName = "I1C1Many2Ones";
		MetaI1.Instance.I1C1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("e386cca6-e738-4c37-8bfc-b23057d7a0be"), new System.Guid("a3af5653-20c0-410c-9a6f-160e10e2fe69"), new System.Guid("6c708f4b-9fb1-412b-84c8-02f03efede5e"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I1I12One2Many";
		relationType.RoleType.PluralName = "I1I12One2Manies";
		MetaI1.Instance.I1I12One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ef1a0a5e-1794-4478-9d0a-517182355206"), new System.Guid("7b80b14e-dd35-4e7c-ba85-ac7860a5dc28"), new System.Guid("1d51d303-f68b-4dca-9299-a6376e13c90e"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I1C2Many2One";
		relationType.RoleType.PluralName = "I1C2Many2Ones";
		MetaI1.Instance.I1C2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f9d7411e-7993-4e43-a7e2-726f1e44e29c"), new System.Guid("84ae4441-5f83-4196-8439-483311b05055"), new System.Guid("5ebf419f-1c7f-46f2-844c-0f54321888ee"));
		relationType.AssociationType.ObjectType = MetaI1.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "I1AllorsUnique";
		relationType.RoleType.PluralName = "I1AllorsUniques";
		MetaI1.Instance.I1AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("042d1311-1c06-4d7c-b68e-eb734f9c7327"), new System.Guid("0d3f0f95-aaa2-47bb-9e2b-654d2747b2b1"), new System.Guid("f7809a25-1b10-4eb0-9309-aeea6efcd7cb"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "I12AllorsBinary";
		relationType.RoleType.PluralName = "I12AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaI12.Instance.I12AllorsBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("107c212d-cc1c-41b2-9c1d-b40c0102072c"), new System.Guid("0a1b3b66-6bb2-4062-b3bb-991987dd5194"), new System.Guid("4c448b25-b56c-4486-b0c8-ac04a3249677"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I12C2One2One";
		relationType.RoleType.PluralName = "I12C2One2Ones";
		MetaI12.Instance.I12C2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1611cb5d-4676-4e85-bfc5-5572e8ff1138"), new System.Guid("4af20cc8-a610-4493-9420-7cd110cc6755"), new System.Guid("5f2eff86-71bf-480d-a6ad-1c93fc68b08d"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "I12AllorsDouble";
		relationType.RoleType.PluralName = "I12AllorsDoubles";
		MetaI12.Instance.I12AllorsDouble = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("167b53c0-644c-467e-9f7c-fcb9415d02c6"), new System.Guid("d039c8f9-217a-46cc-b428-7480d4991e1e"), new System.Guid("2e3dc9b9-3700-4090-bafa-2c60050d52d5"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I12I1Many2One";
		relationType.RoleType.PluralName = "I12I1Many2Ones";
		MetaI12.Instance.I12I1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("199a84c4-c7cb-4f23-8b6c-078b14525e18"), new System.Guid("65ed1ff6-eb81-410d-8817-62d61765408a"), new System.Guid("c778c7a7-9cf7-4a7e-8408-e4eb1ca94ce8"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "I12AllorsString";
		relationType.RoleType.PluralName = "I12AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaI12.Instance.I12AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1bf2abe0-9273-4fb9-b491-020320f1f8db"), new System.Guid("732fc964-194e-4ece-bd39-bb3c47b83ff9"), new System.Guid("b311c57d-9565-48c1-80d8-1d3cf5a498ea"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I12I12Many2Many";
		relationType.RoleType.PluralName = "I12I12Many2Manies";
		MetaI12.Instance.I12I12Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("41a74fec-cfbc-43ca-a6e7-890f0dd1eddb"), new System.Guid("7293e939-ad0b-4b62-935d-44a5309f2515"), new System.Guid("295a4e46-3133-4aff-a1dc-5101e584fb8a"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "I12AllorsDecimal";
		relationType.RoleType.PluralName = "I12AllorsDecimals";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaI12.Instance.I12AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4a2b2f43-037d-4149-8a1e-401e5df963ba"), new System.Guid("cd90d290-95da-4137-aaf1-bcb59f10e9cb"), new System.Guid("f266759c-34c5-49a8-8d92-e2bbcb41c86a"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I12I2Many2Many";
		relationType.RoleType.PluralName = "I12I2Many2Manies";
		MetaI12.Instance.I12I2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("51ebb024-c847-4165-b216-b3b6e8883961"), new System.Guid("04bca123-7c45-43f4-a5ed-8691b0cbb0e3"), new System.Guid("f5928b47-5a57-4be8-a0a9-a729e8e467bb"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I12C2Many2Many";
		relationType.RoleType.PluralName = "I12C2Many2Manies";
		MetaI12.Instance.I12C2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("59ae05e3-573c-4ea4-9181-2c545236ed1e"), new System.Guid("064f5e1b-b5c8-45ee-baf1-094f6a723ede"), new System.Guid("397b339e-0277-4700-a5d1-d9d0ac23c362"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I12I1Many2Many";
		relationType.RoleType.PluralName = "I12I1Many2Manies";
		MetaI12.Instance.I12I1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5e473f63-b1d7-4530-b64f-26435fb5063c"), new System.Guid("83e23750-52eb-4b3f-a675-bfe32570357b"), new System.Guid("d786aeb4-03bb-419a-90c9-e6ddaa940e93"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I12I12One2Many";
		relationType.RoleType.PluralName = "I12I12One2Manies";
		MetaI12.Instance.I12I12One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6daafb16-1bc3-4f15-8e25-1a982c5bb3c5"), new System.Guid("d39d3782-71a6-4b63-aaeb-0a6da0db153d"), new System.Guid("a89707e2-e3e1-4d24-9c56-180671e3409c"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "Name";
		relationType.RoleType.PluralName = "Names";
		relationType.RoleType.Size = 256;
		MetaI12.Instance.Name = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7827af95-147f-4803-865a-b418d567da68"), new System.Guid("7e707f89-6dd2-44a4-8f85-e00666af4d00"), new System.Guid("a4c1f678-a3ae-4707-81e9-b3f3411a5d93"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I12C1Many2Many";
		relationType.RoleType.PluralName = "I12C1Many2Manies";
		MetaI12.Instance.I12C1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("7f6fdb73-3e19-40e7-8feb-6ddbdf2e745a"), new System.Guid("644f55c6-8d39-4602-89bb-5797c9c8e1fd"), new System.Guid("2073096f-8918-4432-8fa2-42f4fd1a53a1"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I12I2Many2One";
		relationType.RoleType.PluralName = "I12I2Many2Ones";
		MetaI12.Instance.I12I2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("93a59d0a-278d-435b-967e-551523f0cb85"), new System.Guid("9c700ad0-e33e-4731-ac3a-4063c2da655b"), new System.Guid("839c7aaa-f044-4b93-97aa-00beeed8f3eb"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "I12AllorsUnique";
		relationType.RoleType.PluralName = "I12AllorsUniques";
		MetaI12.Instance.I12AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("95551e3a-bad2-4136-923f-c8e5f0f2aec7"), new System.Guid("f57afc9e-3832-4ae1-b3a0-659d7f00604c"), new System.Guid("cbd73ad2-a4cd-4b65-a3cd-55bb7c6f52bc"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "I12AllorsInteger";
		relationType.RoleType.PluralName = "I12AllorsIntegers";
		MetaI12.Instance.I12AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("95c77a0f-7f4c-4142-a93f-f688cfd554af"), new System.Guid("870af1ab-075f-4e19-a283-6e6875d362bb"), new System.Guid("29f38fb4-8e6a-4f70-9ee9-f6819b9d759e"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I12I1One2Many";
		relationType.RoleType.PluralName = "I12I1One2Manies";
		MetaI12.Instance.I12I1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9aefdda0-e547-4c9b-bf28-431669f8ea2e"), new System.Guid("f4399c8b-3394-4c2a-9ff0-16b2ece87fdf"), new System.Guid("ee9379c4-ef6a-4c6e-8190-bc71c36ac009"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I12C1One2One";
		relationType.RoleType.PluralName = "I12C1One2Ones";
		MetaI12.Instance.I12C1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("a89b4c06-bba5-4b05-bd6f-c32bc195c32f"), new System.Guid("8dd3e2b6-805f-4c93-98d8-4864e6807760"), new System.Guid("e68fba09-6113-4b49-a6fa-a09e46a004f1"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I12I12One2One";
		relationType.RoleType.PluralName = "I12I12One2Ones";
		MetaI12.Instance.I12I12One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ac920d1d-290b-484b-9283-3829337182bc"), new System.Guid("991e5b73-a9b0-40a4-8230-b3fb7cc46761"), new System.Guid("07702752-2c97-4b44-8c43-7c1f2a5e3d0d"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I12I2One2One";
		relationType.RoleType.PluralName = "I12I2One2Ones";
		MetaI12.Instance.I12I2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b2e3ddda-0cc3-4cfd-a114-9040882ec58a"), new System.Guid("014cf60e-595f-42d5-9146-e7d860396f4d"), new System.Guid("d5c22b99-6984-4042-98fd-93fe60dfe5d7"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "Dependency";
		relationType.RoleType.PluralName = "Dependencies";
		MetaI12.Instance.Dependencies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b2f568a1-51ba-4b6b-a1f1-b82bdec382b5"), new System.Guid("6f37656a-21d0-4574-8eac-5342f7c6850d"), new System.Guid("09a2a7a1-4713-4c5c-828d-8be40f33d1ae"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I12I2One2Many";
		relationType.RoleType.PluralName = "I12I2One2Manies";
		MetaI12.Instance.I12I2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c018face-b292-455c-a2c0-8f71377fb6cb"), new System.Guid("3239eb17-dc55-465f-854c-1d2d024bca94"), new System.Guid("2ff52878-3ade-4afe-9961-8f79336bb0a2"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I12C2Many2One";
		relationType.RoleType.PluralName = "I12C2Many2Ones";
		MetaI12.Instance.I12C2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("c6ecc142-0fbd-48b7-98ae-994fa9b5b814"), new System.Guid("c7469ffd-ffd7-4913-962c-8a7a0b4df3dd"), new System.Guid("1d091625-ec4a-486d-a9be-8f87fe300967"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I12I12Many2One";
		relationType.RoleType.PluralName = "I12I12Many2Ones";
		MetaI12.Instance.I12I12Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ccdd1ae2-263e-4221-9841-4cff1907ee8d"), new System.Guid("55be99e6-71fd-4483-b211-c3080e6ffa05"), new System.Guid("79723949-b9ad-40bf-baee-96d001942855"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "I12AllorsBoolean";
		relationType.RoleType.PluralName = "I12AllorsBooleans";
		MetaI12.Instance.I12AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("ce0f7d58-b415-43f3-989b-9d8b34754e4b"), new System.Guid("33bd508e-d754-4533-9ecd-9c8ce8d10c88"), new System.Guid("72545574-d138-467c-8f21-0c5d15b1d793"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I12I1One2One";
		relationType.RoleType.PluralName = "I12I1One2Ones";
		MetaI12.Instance.I12I1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f302dd07-1abc-409e-aa71-ec9f7ac439aa"), new System.Guid("99b3bf26-3437-4b5b-a786-28c095975a48"), new System.Guid("ee291df6-6a3e-4d92-a779-879679e1b688"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I12C1One2Many";
		relationType.RoleType.PluralName = "I12C1One2Manies";
		MetaI12.Instance.I12C1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f6436bc9-e307-4001-8f1f-5b37553ab3c6"), new System.Guid("63297178-60c1-4cbc-a68d-2842385ba266"), new System.Guid("6e5b98b0-1af3-4e99-8781-37ea97792a24"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I12C1Many2One";
		relationType.RoleType.PluralName = "I12C1Many2Ones";
		MetaI12.Instance.I12C1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("fa6656dc-3a7a-4701-bc6b-3cd06aaa4483"), new System.Guid("6e4d05f3-52e3-4937-b8d2-8d9d52e7c8bf"), new System.Guid("823e8329-0a90-49ed-9b2c-4bfb9db2ee02"));
		relationType.AssociationType.ObjectType = MetaI12.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "I12AllorsDateTime";
		relationType.RoleType.PluralName = "I12AllorsDateTimes";
		MetaI12.Instance.I12AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("01d9ff41-d503-421e-93a6-5563e1787543"), new System.Guid("359ca62a-c74c-4936-a62d-9b8774174e8d"), new System.Guid("141b832f-7321-43b8-8033-dbad3f80edc3"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I2I2Many2One";
		relationType.RoleType.PluralName = "I2I2Many2Ones";
		MetaI2.Instance.I2I2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("1f763206-c575-4e34-9e6b-997d434d3f42"), new System.Guid("923f6373-cbf8-46b1-9b4b-185015ff59ac"), new System.Guid("9edd1eb9-2b9a-4375-a669-68c1859eace2"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I2C1Many2One";
		relationType.RoleType.PluralName = "I2C1Many2Ones";
		MetaI2.Instance.I2C1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("23e9c15f-097f-4452-9bac-d7cf2a65134a"), new System.Guid("278afe09-d0e7-4a41-a60b-b3a01fd14c93"), new System.Guid("e538ab5e-80f2-4a34-81e7-c9b92414dda1"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I2I12Many2One";
		relationType.RoleType.PluralName = "I2I12Many2Ones";
		MetaI2.Instance.I2I12Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("35040d7c-ab7f-4a99-9d09-e01e24ca3cb9"), new System.Guid("d1f0ae79-1820-47a5-8869-496c3578a53d"), new System.Guid("0d2c6dbe-9bb2-414c-8f19-5381fe69ac64"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaBoolean.Instance;
		relationType.RoleType.SingularName = "I2AllorsBoolean";
		relationType.RoleType.PluralName = "I2AllorsBooleans";
		MetaI2.Instance.I2AllorsBoolean = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("40b8edb3-e8c4-46c0-855b-4b18e0e8d7f3"), new System.Guid("078e1b17-f239-44b2-87d6-6350dd37ac1d"), new System.Guid("805d7871-bc51-4572-be01-e47ac8fef22a"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I2C1One2Many";
		relationType.RoleType.PluralName = "I2C1One2Manies";
		MetaI2.Instance.I2C1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("49736daf-d0bd-4216-97fa-958cfa21a4f0"), new System.Guid("02a80ccd-31c9-422c-8ad9-d96916dd7741"), new System.Guid("6ac5d426-9156-4467-8a04-85ccb6c964e2"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I2C1One2One";
		relationType.RoleType.PluralName = "I2C1One2Ones";
		MetaI2.Instance.I2C1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("4f095abd-8803-4610-87f0-2847ddd5e9f4"), new System.Guid("5371c058-628e-4a1c-b654-ad0b7013eb17"), new System.Guid("ec80b71e-a933-4eb3-ab14-00b26c3bc805"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaDecimal.Instance;
		relationType.RoleType.SingularName = "I2AllorsDecimal";
		relationType.RoleType.PluralName = "I2AllorsDecimals";
		relationType.RoleType.Precision = 19;
		relationType.RoleType.Scale = 2;
		MetaI2.Instance.I2AllorsDecimal = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("5ebbc734-23dd-494f-af2d-8e75caaa3e26"), new System.Guid("4d6c09d6-5644-47bb-a50a-464350053833"), new System.Guid("3aab87f3-2eab-4f81-9c1b-fd2e162a93b8"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I2I2Many2Many";
		relationType.RoleType.PluralName = "I2I2Many2Manies";
		MetaI2.Instance.I2I2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("62a8a93d-3744-49de-9f9a-9997b6ef4da6"), new System.Guid("f9be65e7-6e36-42df-bb85-5198d0c12b74"), new System.Guid("e3ae23bc-5934-4c0d-a709-adb00110772d"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaBinary.Instance;
		relationType.RoleType.SingularName = "I2AllorsBinary";
		relationType.RoleType.PluralName = "I2AllorsBinaries";
		relationType.RoleType.Size = -1;
		MetaI2.Instance.I2AllorsBinary = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("663559c4-ef64-4e78-89b4-bfa00691c627"), new System.Guid("9513c57f-478a-423e-ba15-b9132bc28cd0"), new System.Guid("3f03fb6f-b0ba-4c78-b86a-9c4a1c574dd4"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "I2AllorsUnique";
		relationType.RoleType.PluralName = "I2AllorsUniques";
		MetaI2.Instance.I2AllorsUnique = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("6bb406bc-627b-444c-9c16-df9878e05e9c"), new System.Guid("16647879-8af1-4f1c-8ef5-2cec85aa31f4"), new System.Guid("edee2f1c-3e94-45b5-80f4-160faa2074c4"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I2I1Many2One";
		relationType.RoleType.PluralName = "I2I1Many2Ones";
		MetaI2.Instance.I2I1Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("81d9eb2f-55a7-4d1c-853d-4369eb691ba5"), new System.Guid("db4d3b11-77bd-408e-ad41-4a03272a88e1"), new System.Guid("bdcffe2b-ffa7-4eb1-be24-8d8ab0b4dce2"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaDateTime.Instance;
		relationType.RoleType.SingularName = "I2AllorsDateTime";
		relationType.RoleType.PluralName = "I2AllorsDateTimes";
		MetaI2.Instance.I2AllorsDateTime = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("83dc0581-e04a-4f51-a44e-4fef63d44356"), new System.Guid("b1c5cbb7-3d5f-48b8-b182-aa8a0cc3e72a"), new System.Guid("9598153e-9c1c-438a-a8a8-9822092a6a07"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I2I12One2Many";
		relationType.RoleType.PluralName = "I2I12One2Manies";
		MetaI2.Instance.I2I12One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("87499e99-ed77-44c1-89d6-b4f570b6f217"), new System.Guid("e5201e06-3fbf-4b9c-aa65-1ee4ee9fabfb"), new System.Guid("e4c9f00e-7c3d-4b58-92f0-ccce24b55589"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I2I12One2One";
		relationType.RoleType.PluralName = "I2I12One2Ones";
		MetaI2.Instance.I2I12One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("92fdb313-0b90-48f6-b054-a4ab38f880ba"), new System.Guid("a45ffec8-5e4e-4b21-9d68-9b0050472ed2"), new System.Guid("17e159a2-f5a6-4828-9fef-796fcc9085e8"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I2C2Many2Many";
		relationType.RoleType.PluralName = "I2C2Many2Manies";
		MetaI2.Instance.I2C2Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9bed0518-1946-4e23-9d4b-e4cda439984c"), new System.Guid("7b4a8937-258c-4129-a282-89d5ab924d68"), new System.Guid("2e78a543-949f-4130-b659-80a9a60ad6ab"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I2I1Many2Many";
		relationType.RoleType.PluralName = "I2I1Many2Manies";
		MetaI2.Instance.I2I1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9f361b97-0b04-496d-ac60-718760c2a4e2"), new System.Guid("c51f6fd4-c290-41b6-b594-19e9bcbbee6a"), new System.Guid("f60f8fa4-4e73-472d-b0b0-67f202c1e969"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I2C2Many2One";
		relationType.RoleType.PluralName = "I2C2Many2Ones";
		MetaI2.Instance.I2C2Many2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("9f91841c-f63f-4ffa-bee6-62e100f3cd15"), new System.Guid("3164fd30-297e-4e2a-86d6-fad6754f1d59"), new System.Guid("7afb53c1-2fe3-44b6-b1d2-d5a9f6100076"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaString.Instance;
		relationType.RoleType.SingularName = "I2AllorsString";
		relationType.RoleType.PluralName = "I2AllorsStrings";
		relationType.RoleType.Size = 256;
		MetaI2.Instance.I2AllorsString = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b39fdd23-d7dd-473f-9705-df2f29be5ffe"), new System.Guid("8ddc9cbf-8e5c-4166-a2b0-6127c142da78"), new System.Guid("7cdd2b76-6c35-4e81-a1da-f5d0a300014b"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I2C2One2Many";
		relationType.RoleType.PluralName = "I2C2One2Manies";
		MetaI2.Instance.I2C2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("b640bf16-0dc0-4203-aa76-f456371239ae"), new System.Guid("257fa0c6-43ea-4fe9-8142-dbc172d1e138"), new System.Guid("26deb364-bd5e-4b5d-b28a-19689ab3c00d"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I2I1One2One";
		relationType.RoleType.PluralName = "I2I1One2Ones";
		MetaI2.Instance.I2I1One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("bbb01166-2671-4ca1-8b1e-12e6ae8aeb03"), new System.Guid("ee0766c7-0ef6-4ca0-b4a1-c399bc8df823"), new System.Guid("d8f011c4-3057-4384-9045-9c34b13db5c3"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI1.Instance;
		relationType.RoleType.SingularName = "I2I1One2Many";
		relationType.RoleType.PluralName = "I2I1One2Manies";
		MetaI2.Instance.I2I1One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cb9f21e0-a841-45de-8ba4-991b4ceca616"), new System.Guid("1127ff1b-1657-4e18-bdc9-bc90cd8a3c15"), new System.Guid("d838e921-ff63-4e4f-afd8-42dc29d23555"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI12.Instance;
		relationType.RoleType.SingularName = "I2I12Many2Many";
		relationType.RoleType.PluralName = "I2I12Many2Manies";
		MetaI2.Instance.I2I12Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("cc4c704c-ab7e-45d4-baa9-b67cfff9448e"), new System.Guid("d15cb643-1ace-4dfe-b0af-e02e4273bbbb"), new System.Guid("12c2c263-7839-4734-9307-bcde6930a2b7"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I2I2One2One";
		relationType.RoleType.PluralName = "I2I2One2Ones";
		MetaI2.Instance.I2I2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("d30dd036-6d28-48df-873b-3a76da8c029e"), new System.Guid("012e0afc-ebc7-4ae4-9fa0-49c72f3daebf"), new System.Guid("69c063b7-156f-4b7f-89eb-10c7eaf39ad5"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaInteger.Instance;
		relationType.RoleType.SingularName = "I2AllorsInteger";
		relationType.RoleType.PluralName = "I2AllorsIntegers";
		MetaI2.Instance.I2AllorsInteger = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("deb9cbd3-386f-4599-802c-be50945b9f1d"), new System.Guid("3fcc8e73-5f3c-4ce0-8f45-daa813278d7e"), new System.Guid("c7d68f0d-24b1-40c9-9431-78763b776bee"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaI2.Instance;
		relationType.RoleType.SingularName = "I2I2One2Many";
		relationType.RoleType.PluralName = "I2I2One2Manies";
		MetaI2.Instance.I2I2One2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f364c9fe-ad36-4305-80fd-4921451c70a5"), new System.Guid("db6935b0-684c-48ce-97d0-6b7183a73adb"), new System.Guid("6ed084f6-8809-46d9-a3ec-4b086ddafb0a"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToMany;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC1.Instance;
		relationType.RoleType.SingularName = "I2C1Many2Many";
		relationType.RoleType.PluralName = "I2C1Many2Manies";
		MetaI2.Instance.I2C1Many2Manies = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("f85c2d97-10b9-478d-9b82-2700d95d5cb1"), new System.Guid("bfb08e5e-afc6-4f27-975f-5fb9af5bacc4"), new System.Guid("666c65ad-8bf7-40be-a51a-e69d3e0bfe01"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.OneToOne;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaC2.Instance;
		relationType.RoleType.SingularName = "I2C2One2One";
		relationType.RoleType.PluralName = "I2C2One2Ones";
		MetaI2.Instance.I2C2One2One = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("fbad33e7-ede1-41fc-97e9-ddf33a0f6459"), new System.Guid("c138d77b-e8bf-4945-962e-f74e338caad4"), new System.Guid("12ea1f33-0eed-4476-9cab-1fd62ed146a3"));
		relationType.AssociationType.ObjectType = MetaI2.Instance;
		relationType.RoleType.ObjectType = MetaFloat.Instance;
		relationType.RoleType.SingularName = "I2AllorsDouble";
		relationType.RoleType.PluralName = "I2AllorsDoubles";
		MetaI2.Instance.I2AllorsDouble = relationType.RoleType; 
	}


	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("BC3991AE-475D-4CA2-A8E1-6DF5CCC65CE0"), new System.Guid("F16FD291-8211-4FC8-98A5-2915951538CC"), new System.Guid("3A73B3DA-F8BE-4971-969B-1A4A64A94FE2"));
		relationType.AssociationType.ObjectType = MetaSyncDepth1.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsSynced = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSyncDepth2.Instance;
		relationType.RoleType.SingularName = "SyncDepth2";
		relationType.RoleType.PluralName = "SyncDepth2s";
		MetaSyncDepth1.Instance.SyncDepth2 = relationType.RoleType; 
	}

	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("615C6C58-513A-456F-A0CE-E472D173DCB0"), new System.Guid("089D3E5D-6A3C-4B94-9162-65DEE526AA1F"), new System.Guid("D1C3F53B-9E51-4574-A180-B3A927E41A0B"));
		relationType.AssociationType.ObjectType = MetaSyncRoot.Instance;
		relationType.AssignedMultiplicity = Allors.Multiplicity.ManyToOne;
		relationType.IsSynced = true;
		relationType.IsIndexed = true;
		relationType.RoleType.ObjectType = MetaSyncDepth1.Instance;
		relationType.RoleType.SingularName = "SyncDepth1";
		relationType.RoleType.PluralName = "SyncDepth1s";
		MetaSyncRoot.Instance.SyncDepth1 = relationType.RoleType; 
	}
	{
		var relationType = new RelationType(this.metaPopulation, new System.Guid("0b89b096-a69a-495c-acfe-b24a9b27e320"), new System.Guid("e178ed0f-7764-4836-bd6f-fcb7f5d62346"), new System.Guid("007a6d25-8506-483d-9140-414c0056d812"));
		relationType.AssociationType.ObjectType = MetaValidationI12.Instance;
		relationType.RoleType.ObjectType = MetaUnique.Instance;
		relationType.RoleType.SingularName = "UniqueId";
		relationType.RoleType.PluralName = "UniqueIds";
		MetaValidationI12.Instance.UniqueId = relationType.RoleType; 
	}
		}

		internal void BuildInheritedRoles()
		{
            MetaVersion.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaVersion.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;



            MetaEnumeration.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaEnumeration.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;
            MetaEnumeration.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;

            MetaObjectState.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;

            MetaTransitionalVersion.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaTransitionalVersion.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;
            MetaTransitional.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaTransitional.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;

            MetaUser.Instance.OwnerSecurityToken = MetaSecurityTokenOwner.Instance.OwnerSecurityToken;
            MetaUser.Instance.OwnerAccessControl = MetaSecurityTokenOwner.Instance.OwnerAccessControl;
            MetaUser.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaUser.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;
            MetaUser.Instance.Locale = MetaLocalised.Instance.Locale;
            MetaTask.Instance.DeniedPermissions = MetaAccessControlledObject.Instance.DeniedPermissions;
            MetaTask.Instance.SecurityTokens = MetaAccessControlledObject.Instance.SecurityTokens;
            MetaTask.Instance.UniqueId = MetaUniquelyIdentifiable.Instance.UniqueId;


            MetaI1.Instance.I12AllorsBinary = MetaI12.Instance.I12AllorsBinary;
            MetaI1.Instance.I12C2One2One = MetaI12.Instance.I12C2One2One;
            MetaI1.Instance.I12AllorsDouble = MetaI12.Instance.I12AllorsDouble;
            MetaI1.Instance.I12I1Many2One = MetaI12.Instance.I12I1Many2One;
            MetaI1.Instance.I12AllorsString = MetaI12.Instance.I12AllorsString;
            MetaI1.Instance.I12I12Many2Manies = MetaI12.Instance.I12I12Many2Manies;
            MetaI1.Instance.I12AllorsDecimal = MetaI12.Instance.I12AllorsDecimal;
            MetaI1.Instance.I12I2Many2Manies = MetaI12.Instance.I12I2Many2Manies;
            MetaI1.Instance.I12C2Many2Manies = MetaI12.Instance.I12C2Many2Manies;
            MetaI1.Instance.I12I1Many2Manies = MetaI12.Instance.I12I1Many2Manies;
            MetaI1.Instance.I12I12One2Manies = MetaI12.Instance.I12I12One2Manies;
            MetaI1.Instance.Name = MetaI12.Instance.Name;
            MetaI1.Instance.I12C1Many2Manies = MetaI12.Instance.I12C1Many2Manies;
            MetaI1.Instance.I12I2Many2One = MetaI12.Instance.I12I2Many2One;
            MetaI1.Instance.I12AllorsUnique = MetaI12.Instance.I12AllorsUnique;
            MetaI1.Instance.I12AllorsInteger = MetaI12.Instance.I12AllorsInteger;
            MetaI1.Instance.I12I1One2Manies = MetaI12.Instance.I12I1One2Manies;
            MetaI1.Instance.I12C1One2One = MetaI12.Instance.I12C1One2One;
            MetaI1.Instance.I12I12One2One = MetaI12.Instance.I12I12One2One;
            MetaI1.Instance.I12I2One2One = MetaI12.Instance.I12I2One2One;
            MetaI1.Instance.Dependencies = MetaI12.Instance.Dependencies;
            MetaI1.Instance.I12I2One2Manies = MetaI12.Instance.I12I2One2Manies;
            MetaI1.Instance.I12C2Many2One = MetaI12.Instance.I12C2Many2One;
            MetaI1.Instance.I12I12Many2One = MetaI12.Instance.I12I12Many2One;
            MetaI1.Instance.I12AllorsBoolean = MetaI12.Instance.I12AllorsBoolean;
            MetaI1.Instance.I12I1One2One = MetaI12.Instance.I12I1One2One;
            MetaI1.Instance.I12C1One2Manies = MetaI12.Instance.I12C1One2Manies;
            MetaI1.Instance.I12C1Many2One = MetaI12.Instance.I12C1Many2One;
            MetaI1.Instance.I12AllorsDateTime = MetaI12.Instance.I12AllorsDateTime;

            MetaI2.Instance.I12AllorsBinary = MetaI12.Instance.I12AllorsBinary;
            MetaI2.Instance.I12C2One2One = MetaI12.Instance.I12C2One2One;
            MetaI2.Instance.I12AllorsDouble = MetaI12.Instance.I12AllorsDouble;
            MetaI2.Instance.I12I1Many2One = MetaI12.Instance.I12I1Many2One;
            MetaI2.Instance.I12AllorsString = MetaI12.Instance.I12AllorsString;
            MetaI2.Instance.I12I12Many2Manies = MetaI12.Instance.I12I12Many2Manies;
            MetaI2.Instance.I12AllorsDecimal = MetaI12.Instance.I12AllorsDecimal;
            MetaI2.Instance.I12I2Many2Manies = MetaI12.Instance.I12I2Many2Manies;
            MetaI2.Instance.I12C2Many2Manies = MetaI12.Instance.I12C2Many2Manies;
            MetaI2.Instance.I12I1Many2Manies = MetaI12.Instance.I12I1Many2Manies;
            MetaI2.Instance.I12I12One2Manies = MetaI12.Instance.I12I12One2Manies;
            MetaI2.Instance.Name = MetaI12.Instance.Name;
            MetaI2.Instance.I12C1Many2Manies = MetaI12.Instance.I12C1Many2Manies;
            MetaI2.Instance.I12I2Many2One = MetaI12.Instance.I12I2Many2One;
            MetaI2.Instance.I12AllorsUnique = MetaI12.Instance.I12AllorsUnique;
            MetaI2.Instance.I12AllorsInteger = MetaI12.Instance.I12AllorsInteger;
            MetaI2.Instance.I12I1One2Manies = MetaI12.Instance.I12I1One2Manies;
            MetaI2.Instance.I12C1One2One = MetaI12.Instance.I12C1One2One;
            MetaI2.Instance.I12I12One2One = MetaI12.Instance.I12I12One2One;
            MetaI2.Instance.I12I2One2One = MetaI12.Instance.I12I2One2One;
            MetaI2.Instance.Dependencies = MetaI12.Instance.Dependencies;
            MetaI2.Instance.I12I2One2Manies = MetaI12.Instance.I12I2One2Manies;
            MetaI2.Instance.I12C2Many2One = MetaI12.Instance.I12C2Many2One;
            MetaI2.Instance.I12I12Many2One = MetaI12.Instance.I12I12Many2One;
            MetaI2.Instance.I12AllorsBoolean = MetaI12.Instance.I12AllorsBoolean;
            MetaI2.Instance.I12I1One2One = MetaI12.Instance.I12I1One2One;
            MetaI2.Instance.I12C1One2Manies = MetaI12.Instance.I12C1One2Manies;
            MetaI2.Instance.I12C1Many2One = MetaI12.Instance.I12C1Many2One;
            MetaI2.Instance.I12AllorsDateTime = MetaI12.Instance.I12AllorsDateTime;






		}

		internal void BuildImplementedRoles()
		{
            MetaLocalisedText.Instance.DeniedPermissions = MetaLocalisedText.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaLocalisedText.Instance.SecurityTokens = MetaLocalisedText.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaLocalisedText.Instance.Locale = MetaLocalisedText.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];
            MetaAccessControl.Instance.DeniedPermissions = MetaAccessControl.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaAccessControl.Instance.SecurityTokens = MetaAccessControl.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaAccessControl.Instance.CacheId = MetaAccessControl.Instance.Class.ConcreteRoleTypeByRoleType[MetaCachable.Instance.CacheId];
            MetaCounter.Instance.UniqueId = MetaCounter.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaCountry.Instance.DeniedPermissions = MetaCountry.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaCountry.Instance.SecurityTokens = MetaCountry.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaCurrency.Instance.DeniedPermissions = MetaCurrency.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaCurrency.Instance.SecurityTokens = MetaCurrency.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaLanguage.Instance.DeniedPermissions = MetaLanguage.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaLanguage.Instance.SecurityTokens = MetaLanguage.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaLocale.Instance.DeniedPermissions = MetaLocale.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaLocale.Instance.SecurityTokens = MetaLocale.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];

            MetaMedia.Instance.UniqueId = MetaMedia.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaMedia.Instance.DeniedPermissions = MetaMedia.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaMedia.Instance.SecurityTokens = MetaMedia.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaMediaContent.Instance.DeniedPermissions = MetaMediaContent.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaMediaContent.Instance.SecurityTokens = MetaMediaContent.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaAutomatedAgent.Instance.OwnerSecurityToken = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaSecurityTokenOwner.Instance.OwnerSecurityToken];
            MetaAutomatedAgent.Instance.OwnerAccessControl = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaSecurityTokenOwner.Instance.OwnerAccessControl];
            MetaAutomatedAgent.Instance.UserEmailConfirmed = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmailConfirmed];
            MetaAutomatedAgent.Instance.UserName = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserName];
            MetaAutomatedAgent.Instance.NormalizedUserName = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NormalizedUserName];
            MetaAutomatedAgent.Instance.UserEmail = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmail];
            MetaAutomatedAgent.Instance.UserPasswordHash = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserPasswordHash];
            MetaAutomatedAgent.Instance.TaskList = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.TaskList];
            MetaAutomatedAgent.Instance.NotificationList = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NotificationList];
            MetaAutomatedAgent.Instance.DeniedPermissions = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaAutomatedAgent.Instance.SecurityTokens = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaAutomatedAgent.Instance.Locale = MetaAutomatedAgent.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];
            MetaPermission.Instance.DeniedPermissions = MetaPermission.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaPermission.Instance.SecurityTokens = MetaPermission.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaPerson.Instance.UserEmailConfirmed = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmailConfirmed];
            MetaPerson.Instance.UserName = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserName];
            MetaPerson.Instance.NormalizedUserName = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NormalizedUserName];
            MetaPerson.Instance.UserEmail = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserEmail];
            MetaPerson.Instance.UserPasswordHash = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.UserPasswordHash];
            MetaPerson.Instance.TaskList = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.TaskList];
            MetaPerson.Instance.NotificationList = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUser.Instance.NotificationList];
            MetaPerson.Instance.OwnerSecurityToken = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaSecurityTokenOwner.Instance.OwnerSecurityToken];
            MetaPerson.Instance.OwnerAccessControl = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaSecurityTokenOwner.Instance.OwnerAccessControl];
            MetaPerson.Instance.DeniedPermissions = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaPerson.Instance.SecurityTokens = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaPerson.Instance.Locale = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaLocalised.Instance.Locale];
            MetaPerson.Instance.UniqueId = MetaPerson.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaRole.Instance.DeniedPermissions = MetaRole.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaRole.Instance.SecurityTokens = MetaRole.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaRole.Instance.UniqueId = MetaRole.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];

            MetaSingleton.Instance.DeniedPermissions = MetaSingleton.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaSingleton.Instance.SecurityTokens = MetaSingleton.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaUserGroup.Instance.UniqueId = MetaUserGroup.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaUserGroup.Instance.DeniedPermissions = MetaUserGroup.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaUserGroup.Instance.SecurityTokens = MetaUserGroup.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];

            MetaNotification.Instance.DeniedPermissions = MetaNotification.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaNotification.Instance.SecurityTokens = MetaNotification.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaNotificationList.Instance.DeniedPermissions = MetaNotificationList.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaNotificationList.Instance.SecurityTokens = MetaNotificationList.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaTaskAssignment.Instance.DeniedPermissions = MetaTaskAssignment.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaTaskAssignment.Instance.SecurityTokens = MetaTaskAssignment.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];



            MetaC1.Instance.I1I1Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I1Many2One];
            MetaC1.Instance.I1I12Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I12Many2Manies];
            MetaC1.Instance.I1I2Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I2Many2Manies];
            MetaC1.Instance.I1I2Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I2Many2One];
            MetaC1.Instance.I1AllorsString = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsString];
            MetaC1.Instance.I1I12Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I12Many2One];
            MetaC1.Instance.I1AllorsDateTime = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsDateTime];
            MetaC1.Instance.I1I2One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I2One2Manies];
            MetaC1.Instance.I1C2One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C2One2Manies];
            MetaC1.Instance.I1C1One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C1One2One];
            MetaC1.Instance.I1AllorsInteger = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsInteger];
            MetaC1.Instance.I1C2Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C2Many2Manies];
            MetaC1.Instance.I1I1One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I1One2Manies];
            MetaC1.Instance.I1I1Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I1Many2Manies];
            MetaC1.Instance.I1AllorsBoolean = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsBoolean];
            MetaC1.Instance.I1AllorsDecimal = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsDecimal];
            MetaC1.Instance.I1I12One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I12One2One];
            MetaC1.Instance.I1I2One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I2One2One];
            MetaC1.Instance.I1C2One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C2One2One];
            MetaC1.Instance.I1C1One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C1One2Manies];
            MetaC1.Instance.I1AllorsBinary = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsBinary];
            MetaC1.Instance.I1C1Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C1Many2Manies];
            MetaC1.Instance.I1AllorsDouble = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsDouble];
            MetaC1.Instance.I1I1One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I1One2One];
            MetaC1.Instance.I1C1Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C1Many2One];
            MetaC1.Instance.I1I12One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1I12One2Manies];
            MetaC1.Instance.I1C2Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1C2Many2One];
            MetaC1.Instance.I1AllorsUnique = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI1.Instance.I1AllorsUnique];
            MetaC1.Instance.I12AllorsBinary = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsBinary];
            MetaC1.Instance.I12C2One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2One2One];
            MetaC1.Instance.I12AllorsDouble = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDouble];
            MetaC1.Instance.I12I1Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1Many2One];
            MetaC1.Instance.I12AllorsString = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsString];
            MetaC1.Instance.I12I12Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12Many2Manies];
            MetaC1.Instance.I12AllorsDecimal = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDecimal];
            MetaC1.Instance.I12I2Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2Many2Manies];
            MetaC1.Instance.I12C2Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2Many2Manies];
            MetaC1.Instance.I12I1Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1Many2Manies];
            MetaC1.Instance.I12I12One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12One2Manies];
            MetaC1.Instance.Name = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.Name];
            MetaC1.Instance.I12C1Many2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1Many2Manies];
            MetaC1.Instance.I12I2Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2Many2One];
            MetaC1.Instance.I12AllorsUnique = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsUnique];
            MetaC1.Instance.I12AllorsInteger = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsInteger];
            MetaC1.Instance.I12I1One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1One2Manies];
            MetaC1.Instance.I12C1One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1One2One];
            MetaC1.Instance.I12I12One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12One2One];
            MetaC1.Instance.I12I2One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2One2One];
            MetaC1.Instance.Dependencies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.Dependencies];
            MetaC1.Instance.I12I2One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2One2Manies];
            MetaC1.Instance.I12C2Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2Many2One];
            MetaC1.Instance.I12I12Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12Many2One];
            MetaC1.Instance.I12AllorsBoolean = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsBoolean];
            MetaC1.Instance.I12I1One2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1One2One];
            MetaC1.Instance.I12C1One2Manies = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1One2Manies];
            MetaC1.Instance.I12C1Many2One = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1Many2One];
            MetaC1.Instance.I12AllorsDateTime = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDateTime];
            MetaC1.Instance.DeniedPermissions = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaC1.Instance.SecurityTokens = MetaC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaC2.Instance.I2I2Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I2Many2One];
            MetaC2.Instance.I2C1Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C1Many2One];
            MetaC2.Instance.I2I12Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I12Many2One];
            MetaC2.Instance.I2AllorsBoolean = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsBoolean];
            MetaC2.Instance.I2C1One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C1One2Manies];
            MetaC2.Instance.I2C1One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C1One2One];
            MetaC2.Instance.I2AllorsDecimal = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsDecimal];
            MetaC2.Instance.I2I2Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I2Many2Manies];
            MetaC2.Instance.I2AllorsBinary = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsBinary];
            MetaC2.Instance.I2AllorsUnique = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsUnique];
            MetaC2.Instance.I2I1Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I1Many2One];
            MetaC2.Instance.I2AllorsDateTime = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsDateTime];
            MetaC2.Instance.I2I12One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I12One2Manies];
            MetaC2.Instance.I2I12One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I12One2One];
            MetaC2.Instance.I2C2Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C2Many2Manies];
            MetaC2.Instance.I2I1Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I1Many2Manies];
            MetaC2.Instance.I2C2Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C2Many2One];
            MetaC2.Instance.I2AllorsString = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsString];
            MetaC2.Instance.I2C2One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C2One2Manies];
            MetaC2.Instance.I2I1One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I1One2One];
            MetaC2.Instance.I2I1One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I1One2Manies];
            MetaC2.Instance.I2I12Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I12Many2Manies];
            MetaC2.Instance.I2I2One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I2One2One];
            MetaC2.Instance.I2AllorsInteger = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsInteger];
            MetaC2.Instance.I2I2One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2I2One2Manies];
            MetaC2.Instance.I2C1Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C1Many2Manies];
            MetaC2.Instance.I2C2One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2C2One2One];
            MetaC2.Instance.I2AllorsDouble = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI2.Instance.I2AllorsDouble];
            MetaC2.Instance.I12AllorsBinary = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsBinary];
            MetaC2.Instance.I12C2One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2One2One];
            MetaC2.Instance.I12AllorsDouble = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDouble];
            MetaC2.Instance.I12I1Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1Many2One];
            MetaC2.Instance.I12AllorsString = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsString];
            MetaC2.Instance.I12I12Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12Many2Manies];
            MetaC2.Instance.I12AllorsDecimal = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDecimal];
            MetaC2.Instance.I12I2Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2Many2Manies];
            MetaC2.Instance.I12C2Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2Many2Manies];
            MetaC2.Instance.I12I1Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1Many2Manies];
            MetaC2.Instance.I12I12One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12One2Manies];
            MetaC2.Instance.Name = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.Name];
            MetaC2.Instance.I12C1Many2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1Many2Manies];
            MetaC2.Instance.I12I2Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2Many2One];
            MetaC2.Instance.I12AllorsUnique = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsUnique];
            MetaC2.Instance.I12AllorsInteger = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsInteger];
            MetaC2.Instance.I12I1One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1One2Manies];
            MetaC2.Instance.I12C1One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1One2One];
            MetaC2.Instance.I12I12One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12One2One];
            MetaC2.Instance.I12I2One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2One2One];
            MetaC2.Instance.Dependencies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.Dependencies];
            MetaC2.Instance.I12I2One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I2One2Manies];
            MetaC2.Instance.I12C2Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C2Many2One];
            MetaC2.Instance.I12I12Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I12Many2One];
            MetaC2.Instance.I12AllorsBoolean = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsBoolean];
            MetaC2.Instance.I12I1One2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12I1One2One];
            MetaC2.Instance.I12C1One2Manies = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1One2Manies];
            MetaC2.Instance.I12C1Many2One = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12C1Many2One];
            MetaC2.Instance.I12AllorsDateTime = MetaC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaI12.Instance.I12AllorsDateTime];







            MetaGender.Instance.LocalisedNames = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.LocalisedNames];
            MetaGender.Instance.Name = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.Name];
            MetaGender.Instance.IsActive = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaEnumeration.Instance.IsActive];
            MetaGender.Instance.DeniedPermissions = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaGender.Instance.SecurityTokens = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaGender.Instance.UniqueId = MetaGender.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaHomeAddress.Instance.Place = MetaHomeAddress.Instance.Class.ConcreteRoleTypeByRoleType[MetaAddress.Instance.Place];
            MetaMailboxAddress.Instance.Place = MetaMailboxAddress.Instance.Class.ConcreteRoleTypeByRoleType[MetaAddress.Instance.Place];

            MetaOrder.Instance.DeniedPermissions = MetaOrder.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaOrder.Instance.SecurityTokens = MetaOrder.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaOrder.Instance.PreviousObjectStates = MetaOrder.Instance.Class.ConcreteRoleTypeByRoleType[MetaTransitional.Instance.PreviousObjectStates];
            MetaOrder.Instance.LastObjectStates = MetaOrder.Instance.Class.ConcreteRoleTypeByRoleType[MetaTransitional.Instance.LastObjectStates];
            MetaOrder.Instance.ObjectStates = MetaOrder.Instance.Class.ConcreteRoleTypeByRoleType[MetaTransitional.Instance.ObjectStates];
            MetaOrderLine.Instance.DeniedPermissions = MetaOrderLine.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaOrderLine.Instance.SecurityTokens = MetaOrderLine.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaOrderLineVersion.Instance.DeniedPermissions = MetaOrderLineVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaOrderLineVersion.Instance.SecurityTokens = MetaOrderLineVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaOrderLineVersion.Instance.DerivationId = MetaOrderLineVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationId];
            MetaOrderLineVersion.Instance.DerivationTimeStamp = MetaOrderLineVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationTimeStamp];
            MetaPaymentState.Instance.DeniedPermissions = MetaPaymentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.DeniedPermissions];
            MetaPaymentState.Instance.Name = MetaPaymentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaPaymentState.Instance.UniqueId = MetaPaymentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaShipmentState.Instance.DeniedPermissions = MetaShipmentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.DeniedPermissions];
            MetaShipmentState.Instance.Name = MetaShipmentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaShipmentState.Instance.UniqueId = MetaShipmentState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaOrderState.Instance.DeniedPermissions = MetaOrderState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.DeniedPermissions];
            MetaOrderState.Instance.Name = MetaOrderState.Instance.Class.ConcreteRoleTypeByRoleType[MetaObjectState.Instance.Name];
            MetaOrderState.Instance.UniqueId = MetaOrderState.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaOrderVersion.Instance.DerivationId = MetaOrderVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationId];
            MetaOrderVersion.Instance.DerivationTimeStamp = MetaOrderVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaVersion.Instance.DerivationTimeStamp];
            MetaOrderVersion.Instance.DeniedPermissions = MetaOrderVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaOrderVersion.Instance.SecurityTokens = MetaOrderVersion.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaOrganisation.Instance.UniqueId = MetaOrganisation.Instance.Class.ConcreteRoleTypeByRoleType[MetaUniquelyIdentifiable.Instance.UniqueId];
            MetaOrganisation.Instance.DeniedPermissions = MetaOrganisation.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaOrganisation.Instance.SecurityTokens = MetaOrganisation.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];









            MetaUnitSample.Instance.DeniedPermissions = MetaUnitSample.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.DeniedPermissions];
            MetaUnitSample.Instance.SecurityTokens = MetaUnitSample.Instance.Class.ConcreteRoleTypeByRoleType[MetaAccessControlledObject.Instance.SecurityTokens];
            MetaValidationC1.Instance.UniqueId = MetaValidationC1.Instance.Class.ConcreteRoleTypeByRoleType[MetaValidationI12.Instance.UniqueId];
            MetaValidationC2.Instance.UniqueId = MetaValidationC2.Instance.Class.ConcreteRoleTypeByRoleType[MetaValidationI12.Instance.UniqueId];
		}

		internal void BuildAssociations()
		{
            MetaLocalisedText.Instance.CountryWhereLocalisedName = MetaCountry.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.CurrencyWhereLocalisedName = MetaCurrency.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.LanguageWhereLocalisedName = MetaLanguage.Instance.LocalisedNames.AssociationType;
            MetaLocalisedText.Instance.EnumerationWhereLocalisedName = MetaEnumeration.Instance.LocalisedNames.AssociationType;
            MetaAccessControl.Instance.SecurityTokensWhereAccessControl = MetaSecurityToken.Instance.AccessControls.AssociationType;
            MetaAccessControl.Instance.SingletonWhereCreatorsAccessControl = MetaSingleton.Instance.CreatorsAccessControl.AssociationType;
            MetaAccessControl.Instance.SingletonWhereGuestAccessControl = MetaSingleton.Instance.GuestAccessControl.AssociationType;
            MetaAccessControl.Instance.SingletonWhereAdministratorsAccessControl = MetaSingleton.Instance.AdministratorsAccessControl.AssociationType;
            MetaAccessControl.Instance.SingletonWhereSalesAccessControl = MetaSingleton.Instance.SalesAccessControl.AssociationType;
            MetaAccessControl.Instance.SingletonWhereOperationsAccessControl = MetaSingleton.Instance.OperationsAccessControl.AssociationType;
            MetaAccessControl.Instance.SingletonWhereProcurementAccessControl = MetaSingleton.Instance.ProcurementAccessControl.AssociationType;
            MetaAccessControl.Instance.SecurityTokenOwnerWhereOwnerAccessControl = MetaSecurityTokenOwner.Instance.OwnerAccessControl.AssociationType;

            MetaCountry.Instance.LocalesWhereCountry = MetaLocale.Instance.Country.AssociationType;
            MetaCountry.Instance.PlacesWhereCountry = MetaPlace.Instance.Country.AssociationType;
            MetaCurrency.Instance.CountriesWhereCurrency = MetaCountry.Instance.Currency.AssociationType;
            MetaLanguage.Instance.LocalesWhereLanguage = MetaLocale.Instance.Language.AssociationType;
            MetaLocale.Instance.SingletonsWhereDefaultLocale = MetaSingleton.Instance.DefaultLocale.AssociationType;
            MetaLocale.Instance.SingletonWhereLocale = MetaSingleton.Instance.Locales.AssociationType;
            MetaLocale.Instance.LocalisedsWhereLocale = MetaLocalised.Instance.Locale.AssociationType;

            MetaMedia.Instance.PeopleWherePhoto = MetaPerson.Instance.Photo.AssociationType;
            MetaMedia.Instance.OrganisationWhereImage = MetaOrganisation.Instance.Images.AssociationType;
            MetaMedia.Instance.OrganisationsWhereLogo = MetaOrganisation.Instance.Logo.AssociationType;
            MetaMediaContent.Instance.MediaWhereMediaContent = MetaMedia.Instance.MediaContent.AssociationType;

            MetaPermission.Instance.AccessControlsWhereEffectivePermission = MetaAccessControl.Instance.EffectivePermissions.AssociationType;
            MetaPermission.Instance.RolesWherePermission = MetaRole.Instance.Permissions.AssociationType;
            MetaPermission.Instance.AccessControlledObjectsWhereDeniedPermission = MetaAccessControlledObject.Instance.DeniedPermissions.AssociationType;
            MetaPermission.Instance.ObjectStatesWhereDeniedPermission = MetaObjectState.Instance.DeniedPermissions.AssociationType;
            MetaPerson.Instance.BadUIWherePersonsMany = MetaBadUI.Instance.PersonsMany.AssociationType;
            MetaPerson.Instance.BadUIsWherePersonOne = MetaBadUI.Instance.PersonOne.AssociationType;
            MetaPerson.Instance.OrganisationWhereEmployee = MetaOrganisation.Instance.Employees.AssociationType;
            MetaPerson.Instance.OrganisationWhereManager = MetaOrganisation.Instance.Manager.AssociationType;
            MetaPerson.Instance.OrganisationsWhereOwner = MetaOrganisation.Instance.Owner.AssociationType;
            MetaPerson.Instance.OrganisationsWhereShareholder = MetaOrganisation.Instance.Shareholders.AssociationType;
            MetaPerson.Instance.OrganisationsWhereCycleOne = MetaOrganisation.Instance.CycleOne.AssociationType;
            MetaPerson.Instance.OrganisationsWhereCycleMany = MetaOrganisation.Instance.CycleMany.AssociationType;
            MetaPerson.Instance.StatefulCompaniesWhereEmployee = MetaStatefulCompany.Instance.Employee.AssociationType;
            MetaPerson.Instance.StatefulCompaniesWhereManager = MetaStatefulCompany.Instance.Manager.AssociationType;
            MetaPerson.Instance.TasksWhereParticipant = MetaTask.Instance.Participants.AssociationType;
            MetaPerson.Instance.TasksWherePerformer = MetaTask.Instance.Performer.AssociationType;
            MetaRole.Instance.AccessControlsWhereRole = MetaAccessControl.Instance.Role.AssociationType;
            MetaSecurityToken.Instance.SingletonsWhereInitialSecurityToken = MetaSingleton.Instance.InitialSecurityToken.AssociationType;
            MetaSecurityToken.Instance.SingletonsWhereDefaultSecurityToken = MetaSingleton.Instance.DefaultSecurityToken.AssociationType;
            MetaSecurityToken.Instance.AccessControlledObjectsWhereSecurityToken = MetaAccessControlledObject.Instance.SecurityTokens.AssociationType;
            MetaSecurityToken.Instance.SecurityTokenOwnerWhereOwnerSecurityToken = MetaSecurityTokenOwner.Instance.OwnerSecurityToken.AssociationType;

            MetaUserGroup.Instance.AccessControlsWhereSubjectGroup = MetaAccessControl.Instance.SubjectGroups.AssociationType;

            MetaNotification.Instance.NotificationListWhereNotification = MetaNotificationList.Instance.Notifications.AssociationType;
            MetaNotification.Instance.NotificationListWhereUnconfirmedNotification = MetaNotificationList.Instance.UnconfirmedNotifications.AssociationType;
            MetaNotification.Instance.NotificationListWhereConfirmedNotification = MetaNotificationList.Instance.ConfirmedNotifications.AssociationType;
            MetaNotification.Instance.TaskAssignmentWhereNotification = MetaTaskAssignment.Instance.Notification.AssociationType;
            MetaNotificationList.Instance.UserWhereNotificationList = MetaUser.Instance.NotificationList.AssociationType;
            MetaTaskAssignment.Instance.TaskListWhereTaskAssignment = MetaTaskList.Instance.TaskAssignments.AssociationType;
            MetaTaskAssignment.Instance.TaskListWhereOpenTaskAssignment = MetaTaskList.Instance.OpenTaskAssignments.AssociationType;
            MetaTaskList.Instance.UserWhereTaskList = MetaUser.Instance.TaskList.AssociationType;


            MetaC1.Instance.C1sWhereC1C1Many2Many = MetaC1.Instance.C1C1Many2Manies.AssociationType;
            MetaC1.Instance.C1sWhereC1C1Many2One = MetaC1.Instance.C1C1Many2One.AssociationType;
            MetaC1.Instance.C1WhereC1C1One2Many = MetaC1.Instance.C1C1One2Manies.AssociationType;
            MetaC1.Instance.C1WhereC1C1One2One = MetaC1.Instance.C1C1One2One.AssociationType;
            MetaC1.Instance.C2WhereC2C1One2One = MetaC2.Instance.C2C1One2One.AssociationType;
            MetaC1.Instance.C2sWhereC2C1Many2Many = MetaC2.Instance.C2C1Many2Manies.AssociationType;
            MetaC1.Instance.C2sWhereC2C1Many2One = MetaC2.Instance.C2C1Many2One.AssociationType;
            MetaC1.Instance.C2WhereC2C1One2Many = MetaC2.Instance.C2C1One2Manies.AssociationType;
            MetaC1.Instance.I1WhereI1C1One2One = MetaI1.Instance.I1C1One2One.AssociationType;
            MetaC1.Instance.I1WhereI1C1One2Many = MetaI1.Instance.I1C1One2Manies.AssociationType;
            MetaC1.Instance.I1sWhereI1C1Many2Many = MetaI1.Instance.I1C1Many2Manies.AssociationType;
            MetaC1.Instance.I1sWhereI1C1Many2One = MetaI1.Instance.I1C1Many2One.AssociationType;
            MetaC1.Instance.I12sWhereI12C1Many2Many = MetaI12.Instance.I12C1Many2Manies.AssociationType;
            MetaC1.Instance.I12WhereI12C1One2One = MetaI12.Instance.I12C1One2One.AssociationType;
            MetaC1.Instance.I12WhereI12C1One2Many = MetaI12.Instance.I12C1One2Manies.AssociationType;
            MetaC1.Instance.I12sWhereI12C1Many2One = MetaI12.Instance.I12C1Many2One.AssociationType;
            MetaC1.Instance.I2sWhereI2C1Many2One = MetaI2.Instance.I2C1Many2One.AssociationType;
            MetaC1.Instance.I2WhereI2C1One2Many = MetaI2.Instance.I2C1One2Manies.AssociationType;
            MetaC1.Instance.I2WhereI2C1One2One = MetaI2.Instance.I2C1One2One.AssociationType;
            MetaC1.Instance.I2sWhereI2C1Many2Many = MetaI2.Instance.I2C1Many2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1C2Many2Many = MetaC1.Instance.C1C2Many2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1C2Many2One = MetaC1.Instance.C1C2Many2One.AssociationType;
            MetaC2.Instance.C1WhereC1C2One2Many = MetaC1.Instance.C1C2One2Manies.AssociationType;
            MetaC2.Instance.C1WhereC1C2One2One = MetaC1.Instance.C1C2One2One.AssociationType;
            MetaC2.Instance.C2sWhereC2C2Many2One = MetaC2.Instance.C2C2Many2One.AssociationType;
            MetaC2.Instance.C2WhereC2C2One2Many = MetaC2.Instance.C2C2One2Manies.AssociationType;
            MetaC2.Instance.C2WhereC2C2One2One = MetaC2.Instance.C2C2One2One.AssociationType;
            MetaC2.Instance.C2sWhereC2C2Many2Many = MetaC2.Instance.C2C2Many2Manies.AssociationType;
            MetaC2.Instance.I1WhereI1C2One2Many = MetaI1.Instance.I1C2One2Manies.AssociationType;
            MetaC2.Instance.I1sWhereI1C2Many2Many = MetaI1.Instance.I1C2Many2Manies.AssociationType;
            MetaC2.Instance.I1WhereI1C2One2One = MetaI1.Instance.I1C2One2One.AssociationType;
            MetaC2.Instance.I1sWhereI1C2Many2One = MetaI1.Instance.I1C2Many2One.AssociationType;
            MetaC2.Instance.I12WhereI12C2One2One = MetaI12.Instance.I12C2One2One.AssociationType;
            MetaC2.Instance.I12sWhereI12C2Many2Many = MetaI12.Instance.I12C2Many2Manies.AssociationType;
            MetaC2.Instance.I12sWhereI12C2Many2One = MetaI12.Instance.I12C2Many2One.AssociationType;
            MetaC2.Instance.I2sWhereI2C2Many2Many = MetaI2.Instance.I2C2Many2Manies.AssociationType;
            MetaC2.Instance.I2sWhereI2C2Many2One = MetaI2.Instance.I2C2Many2One.AssociationType;
            MetaC2.Instance.I2WhereI2C2One2Many = MetaI2.Instance.I2C2One2Manies.AssociationType;
            MetaC2.Instance.I2WhereI2C2One2One = MetaI2.Instance.I2C2One2One.AssociationType;

            MetaDependee.Instance.DependentWhereDependee = MetaDependent.Instance.Dependee.AssociationType;



            MetaFour.Instance.ThreesWhereFour = MetaThree.Instance.Four.AssociationType;

            MetaGender.Instance.PeopleWhereGender = MetaPerson.Instance.Gender.AssociationType;

            MetaMailboxAddress.Instance.PeopleWhereMailboxAddress = MetaPerson.Instance.MailboxAddress.AssociationType;


            MetaOrderLine.Instance.OrderWhereOrderLine = MetaOrder.Instance.OrderLines.AssociationType;
            MetaOrderLine.Instance.OrderWhereNonVersionedOrderLine = MetaOrder.Instance.NonVersionedOrderLines.AssociationType;
            MetaOrderLine.Instance.OrderVersionsWhereOrderLine = MetaOrderVersion.Instance.OrderLines.AssociationType;
            MetaOrderLineVersion.Instance.OrderLineWhereCurrentVersion = MetaOrderLine.Instance.CurrentVersion.AssociationType;
            MetaOrderLineVersion.Instance.OrderLineWhereAllVersion = MetaOrderLine.Instance.AllVersions.AssociationType;
            MetaPaymentState.Instance.OrdersWherePreviousPaymentState = MetaOrder.Instance.PreviousPaymentState.AssociationType;
            MetaPaymentState.Instance.OrdersWhereLastPaymentState = MetaOrder.Instance.LastPaymentState.AssociationType;
            MetaPaymentState.Instance.OrdersWherePaymentState = MetaOrder.Instance.PaymentState.AssociationType;
            MetaShipmentState.Instance.OrdersWherePreviousShipmentState = MetaOrder.Instance.PreviousShipmentState.AssociationType;
            MetaShipmentState.Instance.OrdersWhereLastShipmentState = MetaOrder.Instance.LastShipmentState.AssociationType;
            MetaShipmentState.Instance.OrdersWhereShipmentState = MetaOrder.Instance.ShipmentState.AssociationType;
            MetaOrderState.Instance.OrdersWherePreviousOrderState = MetaOrder.Instance.PreviousOrderState.AssociationType;
            MetaOrderState.Instance.OrdersWhereLastOrderState = MetaOrder.Instance.LastOrderState.AssociationType;
            MetaOrderState.Instance.OrdersWhereOrderState = MetaOrder.Instance.OrderState.AssociationType;
            MetaOrderState.Instance.OrdersWhereNonVersionedCurrentObjectState = MetaOrder.Instance.NonVersionedCurrentObjectState.AssociationType;
            MetaOrderState.Instance.OrderVersionsWhereOrderState = MetaOrderVersion.Instance.OrderState.AssociationType;
            MetaOrderVersion.Instance.OrderWhereCurrentVersion = MetaOrder.Instance.CurrentVersion.AssociationType;
            MetaOrderVersion.Instance.OrderWhereAllVersion = MetaOrder.Instance.AllVersions.AssociationType;
            MetaOrganisation.Instance.PeopleWhereCycleOne = MetaPerson.Instance.CycleOne.AssociationType;
            MetaOrganisation.Instance.PeopleWhereCycleMany = MetaPerson.Instance.CycleMany.AssociationType;
            MetaOrganisation.Instance.BadUIsWhereCompanyOne = MetaBadUI.Instance.CompanyOne.AssociationType;
            MetaOrganisation.Instance.BadUIsWhereCompanyMany = MetaBadUI.Instance.CompanyMany.AssociationType;
            MetaPlace.Instance.AddressesWherePlace = MetaAddress.Instance.Place.AssociationType;
            MetaSecond.Instance.FirstWhereSecond = MetaFirst.Instance.Second.AssociationType;


            MetaSubdependee.Instance.DependeeWhereSubdependee = MetaDependee.Instance.Subdependee.AssociationType;
            MetaThird.Instance.SecondWhereThird = MetaSecond.Instance.Third.AssociationType;

            MetaTo.Instance.FromWhereTo = MetaFrom.Instance.Tos.AssociationType;
            MetaTwo.Instance.OnesWhereTwo = MetaOne.Instance.Two.AssociationType;











            MetaObjectState.Instance.TransitionalVersionsWherePreviousObjectState = MetaTransitionalVersion.Instance.PreviousObjectStates.AssociationType;
            MetaObjectState.Instance.TransitionalVersionsWhereLastObjectState = MetaTransitionalVersion.Instance.LastObjectStates.AssociationType;
            MetaObjectState.Instance.TransitionalVersionsWhereObjectState = MetaTransitionalVersion.Instance.ObjectStates.AssociationType;
            MetaObjectState.Instance.TransitionalsWherePreviousObjectState = MetaTransitional.Instance.PreviousObjectStates.AssociationType;
            MetaObjectState.Instance.TransitionalsWhereLastObjectState = MetaTransitional.Instance.LastObjectStates.AssociationType;
            MetaObjectState.Instance.TransitionalsWhereObjectState = MetaTransitional.Instance.ObjectStates.AssociationType;



            MetaUniquelyIdentifiable.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;
            MetaUser.Instance.AccessControlsWhereSubject = MetaAccessControl.Instance.Subjects.AssociationType;
            MetaUser.Instance.AccessControlsWhereEffectiveUser = MetaAccessControl.Instance.EffectiveUsers.AssociationType;
            MetaUser.Instance.LoginsWhereUser = MetaLogin.Instance.User.AssociationType;
            MetaUser.Instance.SingletonWhereGuest = MetaSingleton.Instance.Guest.AssociationType;
            MetaUser.Instance.UserGroupsWhereMember = MetaUserGroup.Instance.Members.AssociationType;
            MetaUser.Instance.EmailMessagesWhereSender = MetaEmailMessage.Instance.Sender.AssociationType;
            MetaUser.Instance.EmailMessagesWhereRecipient = MetaEmailMessage.Instance.Recipients.AssociationType;
            MetaUser.Instance.TaskAssignmentsWhereUser = MetaTaskAssignment.Instance.User.AssociationType;
            MetaTask.Instance.TaskAssignmentsWhereTask = MetaTaskAssignment.Instance.Task.AssociationType;
            MetaWorkItem.Instance.TasksWhereWorkItem = MetaTask.Instance.WorkItem.AssociationType;
            MetaAddress.Instance.PeopleWhereAddress = MetaPerson.Instance.Addresses.AssociationType;
            MetaAddress.Instance.PeopleWhereMainAddress = MetaPerson.Instance.MainAddress.AssociationType;
            MetaAddress.Instance.OrganisationWhereAddress = MetaOrganisation.Instance.Addresses.AssociationType;
            MetaAddress.Instance.OrganisationsWhereMainAddress = MetaOrganisation.Instance.MainAddress.AssociationType;
            MetaI1.Instance.C1sWhereC1I1Many2Many = MetaC1.Instance.C1I1Many2Manies.AssociationType;
            MetaI1.Instance.C1sWhereC1I1Many2One = MetaC1.Instance.C1I1Many2One.AssociationType;
            MetaI1.Instance.C1WhereC1I1One2Many = MetaC1.Instance.C1I1One2Manies.AssociationType;
            MetaI1.Instance.C1WhereC1I1One2One = MetaC1.Instance.C1I1One2One.AssociationType;
            MetaI1.Instance.C2sWhereC2I1Many2Many = MetaC2.Instance.C2I1Many2Manies.AssociationType;
            MetaI1.Instance.C2WhereC2I1One2Many = MetaC2.Instance.C2I1One2Manies.AssociationType;
            MetaI1.Instance.C2sWhereC2I1Many2One = MetaC2.Instance.C2I1Many2One.AssociationType;
            MetaI1.Instance.C2WhereC2I1One2One = MetaC2.Instance.C2I1One2One.AssociationType;
            MetaI1.Instance.I1sWhereI1I1Many2One = MetaI1.Instance.I1I1Many2One.AssociationType;
            MetaI1.Instance.I1WhereI1I1One2Many = MetaI1.Instance.I1I1One2Manies.AssociationType;
            MetaI1.Instance.I1sWhereI1I1Many2Many = MetaI1.Instance.I1I1Many2Manies.AssociationType;
            MetaI1.Instance.I1WhereI1I1One2One = MetaI1.Instance.I1I1One2One.AssociationType;
            MetaI1.Instance.I12sWhereI12I1Many2One = MetaI12.Instance.I12I1Many2One.AssociationType;
            MetaI1.Instance.I12sWhereI12I1Many2Many = MetaI12.Instance.I12I1Many2Manies.AssociationType;
            MetaI1.Instance.I12WhereI12I1One2Many = MetaI12.Instance.I12I1One2Manies.AssociationType;
            MetaI1.Instance.I12WhereI12I1One2One = MetaI12.Instance.I12I1One2One.AssociationType;
            MetaI1.Instance.I2sWhereI2I1Many2One = MetaI2.Instance.I2I1Many2One.AssociationType;
            MetaI1.Instance.I2sWhereI2I1Many2Many = MetaI2.Instance.I2I1Many2Manies.AssociationType;
            MetaI1.Instance.I2WhereI2I1One2One = MetaI2.Instance.I2I1One2One.AssociationType;
            MetaI1.Instance.I2WhereI2I1One2Many = MetaI2.Instance.I2I1One2Manies.AssociationType;
            MetaI12.Instance.C1sWhereC1I12Many2Many = MetaC1.Instance.C1I12Many2Manies.AssociationType;
            MetaI12.Instance.C1sWhereC1I12Many2One = MetaC1.Instance.C1I12Many2One.AssociationType;
            MetaI12.Instance.C1WhereC1I12One2Many = MetaC1.Instance.C1I12One2Manies.AssociationType;
            MetaI12.Instance.C1WhereC1I12One2One = MetaC1.Instance.C1I12One2One.AssociationType;
            MetaI12.Instance.C2sWhereC2I12Many2One = MetaC2.Instance.C2I12Many2One.AssociationType;
            MetaI12.Instance.C2WhereC2I12One2One = MetaC2.Instance.C2I12One2One.AssociationType;
            MetaI12.Instance.C2sWhereC2I12Many2Many = MetaC2.Instance.C2I12Many2Manies.AssociationType;
            MetaI12.Instance.C2WhereC2I12One2Many = MetaC2.Instance.C2I12One2Manies.AssociationType;
            MetaI12.Instance.I1sWhereI1I12Many2Many = MetaI1.Instance.I1I12Many2Manies.AssociationType;
            MetaI12.Instance.I1sWhereI1I12Many2One = MetaI1.Instance.I1I12Many2One.AssociationType;
            MetaI12.Instance.I1WhereI1I12One2One = MetaI1.Instance.I1I12One2One.AssociationType;
            MetaI12.Instance.I1WhereI1I12One2Many = MetaI1.Instance.I1I12One2Manies.AssociationType;
            MetaI12.Instance.I12sWhereI12I12Many2Many = MetaI12.Instance.I12I12Many2Manies.AssociationType;
            MetaI12.Instance.I12WhereI12I12One2Many = MetaI12.Instance.I12I12One2Manies.AssociationType;
            MetaI12.Instance.I12WhereI12I12One2One = MetaI12.Instance.I12I12One2One.AssociationType;
            MetaI12.Instance.I12sWhereDependency = MetaI12.Instance.Dependencies.AssociationType;
            MetaI12.Instance.I12sWhereI12I12Many2One = MetaI12.Instance.I12I12Many2One.AssociationType;
            MetaI12.Instance.I2sWhereI2I12Many2One = MetaI2.Instance.I2I12Many2One.AssociationType;
            MetaI12.Instance.I2WhereI2I12One2Many = MetaI2.Instance.I2I12One2Manies.AssociationType;
            MetaI12.Instance.I2WhereI2I12One2One = MetaI2.Instance.I2I12One2One.AssociationType;
            MetaI12.Instance.I2sWhereI2I12Many2Many = MetaI2.Instance.I2I12Many2Manies.AssociationType;
            MetaI2.Instance.C1sWhereC1I2Many2Many = MetaC1.Instance.C1I2Many2Manies.AssociationType;
            MetaI2.Instance.C1sWhereC1I2Many2One = MetaC1.Instance.C1I2Many2One.AssociationType;
            MetaI2.Instance.C1WhereC1I2One2Many = MetaC1.Instance.C1I2One2Manies.AssociationType;
            MetaI2.Instance.C1WhereC1I2One2One = MetaC1.Instance.C1I2One2One.AssociationType;
            MetaI2.Instance.C2WhereC2I2One2One = MetaC2.Instance.C2I2One2One.AssociationType;
            MetaI2.Instance.C2sWhereC2I2Many2Many = MetaC2.Instance.C2I2Many2Manies.AssociationType;
            MetaI2.Instance.C2WhereC2I2One2Many = MetaC2.Instance.C2I2One2Manies.AssociationType;
            MetaI2.Instance.C2sWhereC2I2Many2One = MetaC2.Instance.C2I2Many2One.AssociationType;
            MetaI2.Instance.I1sWhereI1I2Many2Many = MetaI1.Instance.I1I2Many2Manies.AssociationType;
            MetaI2.Instance.I1sWhereI1I2Many2One = MetaI1.Instance.I1I2Many2One.AssociationType;
            MetaI2.Instance.I1WhereI1I2One2Many = MetaI1.Instance.I1I2One2Manies.AssociationType;
            MetaI2.Instance.I1WhereI1I2One2One = MetaI1.Instance.I1I2One2One.AssociationType;
            MetaI2.Instance.I12sWhereI12I2Many2Many = MetaI12.Instance.I12I2Many2Manies.AssociationType;
            MetaI2.Instance.I12sWhereI12I2Many2One = MetaI12.Instance.I12I2Many2One.AssociationType;
            MetaI2.Instance.I12WhereI12I2One2One = MetaI12.Instance.I12I2One2One.AssociationType;
            MetaI2.Instance.I12WhereI12I2One2Many = MetaI12.Instance.I12I2One2Manies.AssociationType;
            MetaI2.Instance.I2sWhereI2I2Many2One = MetaI2.Instance.I2I2Many2One.AssociationType;
            MetaI2.Instance.I2sWhereI2I2Many2Many = MetaI2.Instance.I2I2Many2Manies.AssociationType;
            MetaI2.Instance.I2WhereI2I2One2One = MetaI2.Instance.I2I2One2One.AssociationType;
            MetaI2.Instance.I2WhereI2I2One2Many = MetaI2.Instance.I2I2One2Manies.AssociationType;

            MetaShared.Instance.TwosWhereShared = MetaTwo.Instance.Shared.AssociationType;
            MetaSyncDepth1.Instance.SyncRootsWhereSyncDepth1 = MetaSyncRoot.Instance.SyncDepth1.AssociationType;
            MetaSyncDepth2.Instance.SyncDepth1sWhereSyncDepth2 = MetaSyncDepth1.Instance.SyncDepth2.AssociationType;


		}

		internal void BuildInheritedAssociations()
		{
            MetaCounter.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;





            MetaMedia.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;

            MetaAutomatedAgent.Instance.AccessControlsWhereSubject = MetaAccessControl.Instance.Subjects.AssociationType;
            MetaAutomatedAgent.Instance.AccessControlsWhereEffectiveUser = MetaAccessControl.Instance.EffectiveUsers.AssociationType;
            MetaAutomatedAgent.Instance.LoginsWhereUser = MetaLogin.Instance.User.AssociationType;
            MetaAutomatedAgent.Instance.SingletonWhereGuest = MetaSingleton.Instance.Guest.AssociationType;
            MetaAutomatedAgent.Instance.UserGroupsWhereMember = MetaUserGroup.Instance.Members.AssociationType;
            MetaAutomatedAgent.Instance.EmailMessagesWhereSender = MetaEmailMessage.Instance.Sender.AssociationType;
            MetaAutomatedAgent.Instance.EmailMessagesWhereRecipient = MetaEmailMessage.Instance.Recipients.AssociationType;
            MetaAutomatedAgent.Instance.TaskAssignmentsWhereUser = MetaTaskAssignment.Instance.User.AssociationType;

            MetaPerson.Instance.AccessControlsWhereSubject = MetaAccessControl.Instance.Subjects.AssociationType;
            MetaPerson.Instance.AccessControlsWhereEffectiveUser = MetaAccessControl.Instance.EffectiveUsers.AssociationType;
            MetaPerson.Instance.LoginsWhereUser = MetaLogin.Instance.User.AssociationType;
            MetaPerson.Instance.SingletonWhereGuest = MetaSingleton.Instance.Guest.AssociationType;
            MetaPerson.Instance.UserGroupsWhereMember = MetaUserGroup.Instance.Members.AssociationType;
            MetaPerson.Instance.EmailMessagesWhereSender = MetaEmailMessage.Instance.Sender.AssociationType;
            MetaPerson.Instance.EmailMessagesWhereRecipient = MetaEmailMessage.Instance.Recipients.AssociationType;
            MetaPerson.Instance.TaskAssignmentsWhereUser = MetaTaskAssignment.Instance.User.AssociationType;
            MetaPerson.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;
            MetaRole.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;


            MetaUserGroup.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;







            MetaC1.Instance.C1sWhereC1I1Many2Many = MetaC1.Instance.C1I1Many2Manies.AssociationType;
            MetaC1.Instance.C1sWhereC1I1Many2One = MetaC1.Instance.C1I1Many2One.AssociationType;
            MetaC1.Instance.C1WhereC1I1One2Many = MetaC1.Instance.C1I1One2Manies.AssociationType;
            MetaC1.Instance.C1WhereC1I1One2One = MetaC1.Instance.C1I1One2One.AssociationType;
            MetaC1.Instance.C2sWhereC2I1Many2Many = MetaC2.Instance.C2I1Many2Manies.AssociationType;
            MetaC1.Instance.C2WhereC2I1One2Many = MetaC2.Instance.C2I1One2Manies.AssociationType;
            MetaC1.Instance.C2sWhereC2I1Many2One = MetaC2.Instance.C2I1Many2One.AssociationType;
            MetaC1.Instance.C2WhereC2I1One2One = MetaC2.Instance.C2I1One2One.AssociationType;
            MetaC1.Instance.I1sWhereI1I1Many2One = MetaI1.Instance.I1I1Many2One.AssociationType;
            MetaC1.Instance.I1WhereI1I1One2Many = MetaI1.Instance.I1I1One2Manies.AssociationType;
            MetaC1.Instance.I1sWhereI1I1Many2Many = MetaI1.Instance.I1I1Many2Manies.AssociationType;
            MetaC1.Instance.I1WhereI1I1One2One = MetaI1.Instance.I1I1One2One.AssociationType;
            MetaC1.Instance.I12sWhereI12I1Many2One = MetaI12.Instance.I12I1Many2One.AssociationType;
            MetaC1.Instance.I12sWhereI12I1Many2Many = MetaI12.Instance.I12I1Many2Manies.AssociationType;
            MetaC1.Instance.I12WhereI12I1One2Many = MetaI12.Instance.I12I1One2Manies.AssociationType;
            MetaC1.Instance.I12WhereI12I1One2One = MetaI12.Instance.I12I1One2One.AssociationType;
            MetaC1.Instance.I2sWhereI2I1Many2One = MetaI2.Instance.I2I1Many2One.AssociationType;
            MetaC1.Instance.I2sWhereI2I1Many2Many = MetaI2.Instance.I2I1Many2Manies.AssociationType;
            MetaC1.Instance.I2WhereI2I1One2One = MetaI2.Instance.I2I1One2One.AssociationType;
            MetaC1.Instance.I2WhereI2I1One2Many = MetaI2.Instance.I2I1One2Manies.AssociationType;
            MetaC1.Instance.C1sWhereC1I12Many2Many = MetaC1.Instance.C1I12Many2Manies.AssociationType;
            MetaC1.Instance.C1sWhereC1I12Many2One = MetaC1.Instance.C1I12Many2One.AssociationType;
            MetaC1.Instance.C1WhereC1I12One2Many = MetaC1.Instance.C1I12One2Manies.AssociationType;
            MetaC1.Instance.C1WhereC1I12One2One = MetaC1.Instance.C1I12One2One.AssociationType;
            MetaC1.Instance.C2sWhereC2I12Many2One = MetaC2.Instance.C2I12Many2One.AssociationType;
            MetaC1.Instance.C2WhereC2I12One2One = MetaC2.Instance.C2I12One2One.AssociationType;
            MetaC1.Instance.C2sWhereC2I12Many2Many = MetaC2.Instance.C2I12Many2Manies.AssociationType;
            MetaC1.Instance.C2WhereC2I12One2Many = MetaC2.Instance.C2I12One2Manies.AssociationType;
            MetaC1.Instance.I1sWhereI1I12Many2Many = MetaI1.Instance.I1I12Many2Manies.AssociationType;
            MetaC1.Instance.I1sWhereI1I12Many2One = MetaI1.Instance.I1I12Many2One.AssociationType;
            MetaC1.Instance.I1WhereI1I12One2One = MetaI1.Instance.I1I12One2One.AssociationType;
            MetaC1.Instance.I1WhereI1I12One2Many = MetaI1.Instance.I1I12One2Manies.AssociationType;
            MetaC1.Instance.I12sWhereI12I12Many2Many = MetaI12.Instance.I12I12Many2Manies.AssociationType;
            MetaC1.Instance.I12WhereI12I12One2Many = MetaI12.Instance.I12I12One2Manies.AssociationType;
            MetaC1.Instance.I12WhereI12I12One2One = MetaI12.Instance.I12I12One2One.AssociationType;
            MetaC1.Instance.I12sWhereDependency = MetaI12.Instance.Dependencies.AssociationType;
            MetaC1.Instance.I12sWhereI12I12Many2One = MetaI12.Instance.I12I12Many2One.AssociationType;
            MetaC1.Instance.I2sWhereI2I12Many2One = MetaI2.Instance.I2I12Many2One.AssociationType;
            MetaC1.Instance.I2WhereI2I12One2Many = MetaI2.Instance.I2I12One2Manies.AssociationType;
            MetaC1.Instance.I2WhereI2I12One2One = MetaI2.Instance.I2I12One2One.AssociationType;
            MetaC1.Instance.I2sWhereI2I12Many2Many = MetaI2.Instance.I2I12Many2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1I2Many2Many = MetaC1.Instance.C1I2Many2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1I2Many2One = MetaC1.Instance.C1I2Many2One.AssociationType;
            MetaC2.Instance.C1WhereC1I2One2Many = MetaC1.Instance.C1I2One2Manies.AssociationType;
            MetaC2.Instance.C1WhereC1I2One2One = MetaC1.Instance.C1I2One2One.AssociationType;
            MetaC2.Instance.C2WhereC2I2One2One = MetaC2.Instance.C2I2One2One.AssociationType;
            MetaC2.Instance.C2sWhereC2I2Many2Many = MetaC2.Instance.C2I2Many2Manies.AssociationType;
            MetaC2.Instance.C2WhereC2I2One2Many = MetaC2.Instance.C2I2One2Manies.AssociationType;
            MetaC2.Instance.C2sWhereC2I2Many2One = MetaC2.Instance.C2I2Many2One.AssociationType;
            MetaC2.Instance.I1sWhereI1I2Many2Many = MetaI1.Instance.I1I2Many2Manies.AssociationType;
            MetaC2.Instance.I1sWhereI1I2Many2One = MetaI1.Instance.I1I2Many2One.AssociationType;
            MetaC2.Instance.I1WhereI1I2One2Many = MetaI1.Instance.I1I2One2Manies.AssociationType;
            MetaC2.Instance.I1WhereI1I2One2One = MetaI1.Instance.I1I2One2One.AssociationType;
            MetaC2.Instance.I12sWhereI12I2Many2Many = MetaI12.Instance.I12I2Many2Manies.AssociationType;
            MetaC2.Instance.I12sWhereI12I2Many2One = MetaI12.Instance.I12I2Many2One.AssociationType;
            MetaC2.Instance.I12WhereI12I2One2One = MetaI12.Instance.I12I2One2One.AssociationType;
            MetaC2.Instance.I12WhereI12I2One2Many = MetaI12.Instance.I12I2One2Manies.AssociationType;
            MetaC2.Instance.I2sWhereI2I2Many2One = MetaI2.Instance.I2I2Many2One.AssociationType;
            MetaC2.Instance.I2sWhereI2I2Many2Many = MetaI2.Instance.I2I2Many2Manies.AssociationType;
            MetaC2.Instance.I2WhereI2I2One2One = MetaI2.Instance.I2I2One2One.AssociationType;
            MetaC2.Instance.I2WhereI2I2One2Many = MetaI2.Instance.I2I2One2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1I12Many2Many = MetaC1.Instance.C1I12Many2Manies.AssociationType;
            MetaC2.Instance.C1sWhereC1I12Many2One = MetaC1.Instance.C1I12Many2One.AssociationType;
            MetaC2.Instance.C1WhereC1I12One2Many = MetaC1.Instance.C1I12One2Manies.AssociationType;
            MetaC2.Instance.C1WhereC1I12One2One = MetaC1.Instance.C1I12One2One.AssociationType;
            MetaC2.Instance.C2sWhereC2I12Many2One = MetaC2.Instance.C2I12Many2One.AssociationType;
            MetaC2.Instance.C2WhereC2I12One2One = MetaC2.Instance.C2I12One2One.AssociationType;
            MetaC2.Instance.C2sWhereC2I12Many2Many = MetaC2.Instance.C2I12Many2Manies.AssociationType;
            MetaC2.Instance.C2WhereC2I12One2Many = MetaC2.Instance.C2I12One2Manies.AssociationType;
            MetaC2.Instance.I1sWhereI1I12Many2Many = MetaI1.Instance.I1I12Many2Manies.AssociationType;
            MetaC2.Instance.I1sWhereI1I12Many2One = MetaI1.Instance.I1I12Many2One.AssociationType;
            MetaC2.Instance.I1WhereI1I12One2One = MetaI1.Instance.I1I12One2One.AssociationType;
            MetaC2.Instance.I1WhereI1I12One2Many = MetaI1.Instance.I1I12One2Manies.AssociationType;
            MetaC2.Instance.I12sWhereI12I12Many2Many = MetaI12.Instance.I12I12Many2Manies.AssociationType;
            MetaC2.Instance.I12WhereI12I12One2Many = MetaI12.Instance.I12I12One2Manies.AssociationType;
            MetaC2.Instance.I12WhereI12I12One2One = MetaI12.Instance.I12I12One2One.AssociationType;
            MetaC2.Instance.I12sWhereDependency = MetaI12.Instance.Dependencies.AssociationType;
            MetaC2.Instance.I12sWhereI12I12Many2One = MetaI12.Instance.I12I12Many2One.AssociationType;
            MetaC2.Instance.I2sWhereI2I12Many2One = MetaI2.Instance.I2I12Many2One.AssociationType;
            MetaC2.Instance.I2WhereI2I12One2Many = MetaI2.Instance.I2I12One2Manies.AssociationType;
            MetaC2.Instance.I2WhereI2I12One2One = MetaI2.Instance.I2I12One2One.AssociationType;
            MetaC2.Instance.I2sWhereI2I12Many2Many = MetaI2.Instance.I2I12Many2Manies.AssociationType;





            MetaFour.Instance.TwosWhereShared = MetaTwo.Instance.Shared.AssociationType;

            MetaGender.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;
            MetaHomeAddress.Instance.PeopleWhereAddress = MetaPerson.Instance.Addresses.AssociationType;
            MetaHomeAddress.Instance.PeopleWhereMainAddress = MetaPerson.Instance.MainAddress.AssociationType;
            MetaHomeAddress.Instance.OrganisationWhereAddress = MetaOrganisation.Instance.Addresses.AssociationType;
            MetaHomeAddress.Instance.OrganisationsWhereMainAddress = MetaOrganisation.Instance.MainAddress.AssociationType;
            MetaMailboxAddress.Instance.PeopleWhereAddress = MetaPerson.Instance.Addresses.AssociationType;
            MetaMailboxAddress.Instance.PeopleWhereMainAddress = MetaPerson.Instance.MainAddress.AssociationType;
            MetaMailboxAddress.Instance.OrganisationWhereAddress = MetaOrganisation.Instance.Addresses.AssociationType;
            MetaMailboxAddress.Instance.OrganisationsWhereMainAddress = MetaOrganisation.Instance.MainAddress.AssociationType;
            MetaOne.Instance.TwosWhereShared = MetaTwo.Instance.Shared.AssociationType;



            MetaPaymentState.Instance.TransitionalVersionsWherePreviousObjectState = MetaTransitionalVersion.Instance.PreviousObjectStates.AssociationType;
            MetaPaymentState.Instance.TransitionalVersionsWhereLastObjectState = MetaTransitionalVersion.Instance.LastObjectStates.AssociationType;
            MetaPaymentState.Instance.TransitionalVersionsWhereObjectState = MetaTransitionalVersion.Instance.ObjectStates.AssociationType;
            MetaPaymentState.Instance.TransitionalsWherePreviousObjectState = MetaTransitional.Instance.PreviousObjectStates.AssociationType;
            MetaPaymentState.Instance.TransitionalsWhereLastObjectState = MetaTransitional.Instance.LastObjectStates.AssociationType;
            MetaPaymentState.Instance.TransitionalsWhereObjectState = MetaTransitional.Instance.ObjectStates.AssociationType;
            MetaPaymentState.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;
            MetaShipmentState.Instance.TransitionalVersionsWherePreviousObjectState = MetaTransitionalVersion.Instance.PreviousObjectStates.AssociationType;
            MetaShipmentState.Instance.TransitionalVersionsWhereLastObjectState = MetaTransitionalVersion.Instance.LastObjectStates.AssociationType;
            MetaShipmentState.Instance.TransitionalVersionsWhereObjectState = MetaTransitionalVersion.Instance.ObjectStates.AssociationType;
            MetaShipmentState.Instance.TransitionalsWherePreviousObjectState = MetaTransitional.Instance.PreviousObjectStates.AssociationType;
            MetaShipmentState.Instance.TransitionalsWhereLastObjectState = MetaTransitional.Instance.LastObjectStates.AssociationType;
            MetaShipmentState.Instance.TransitionalsWhereObjectState = MetaTransitional.Instance.ObjectStates.AssociationType;
            MetaShipmentState.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;
            MetaOrderState.Instance.TransitionalVersionsWherePreviousObjectState = MetaTransitionalVersion.Instance.PreviousObjectStates.AssociationType;
            MetaOrderState.Instance.TransitionalVersionsWhereLastObjectState = MetaTransitionalVersion.Instance.LastObjectStates.AssociationType;
            MetaOrderState.Instance.TransitionalVersionsWhereObjectState = MetaTransitionalVersion.Instance.ObjectStates.AssociationType;
            MetaOrderState.Instance.TransitionalsWherePreviousObjectState = MetaTransitional.Instance.PreviousObjectStates.AssociationType;
            MetaOrderState.Instance.TransitionalsWhereLastObjectState = MetaTransitional.Instance.LastObjectStates.AssociationType;
            MetaOrderState.Instance.TransitionalsWhereObjectState = MetaTransitional.Instance.ObjectStates.AssociationType;
            MetaOrderState.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;

            MetaOrganisation.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;






            MetaThree.Instance.TwosWhereShared = MetaTwo.Instance.Shared.AssociationType;

            MetaTwo.Instance.TwosWhereShared = MetaTwo.Instance.Shared.AssociationType;









            MetaEnumeration.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;

            MetaObjectState.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;





            MetaTask.Instance.NotificationsWhereTarget = MetaNotification.Instance.Target.AssociationType;


            MetaI1.Instance.C1sWhereC1I12Many2Many = MetaC1.Instance.C1I12Many2Manies.AssociationType;
            MetaI1.Instance.C1sWhereC1I12Many2One = MetaC1.Instance.C1I12Many2One.AssociationType;
            MetaI1.Instance.C1WhereC1I12One2Many = MetaC1.Instance.C1I12One2Manies.AssociationType;
            MetaI1.Instance.C1WhereC1I12One2One = MetaC1.Instance.C1I12One2One.AssociationType;
            MetaI1.Instance.C2sWhereC2I12Many2One = MetaC2.Instance.C2I12Many2One.AssociationType;
            MetaI1.Instance.C2WhereC2I12One2One = MetaC2.Instance.C2I12One2One.AssociationType;
            MetaI1.Instance.C2sWhereC2I12Many2Many = MetaC2.Instance.C2I12Many2Manies.AssociationType;
            MetaI1.Instance.C2WhereC2I12One2Many = MetaC2.Instance.C2I12One2Manies.AssociationType;
            MetaI1.Instance.I1sWhereI1I12Many2Many = MetaI1.Instance.I1I12Many2Manies.AssociationType;
            MetaI1.Instance.I1sWhereI1I12Many2One = MetaI1.Instance.I1I12Many2One.AssociationType;
            MetaI1.Instance.I1WhereI1I12One2One = MetaI1.Instance.I1I12One2One.AssociationType;
            MetaI1.Instance.I1WhereI1I12One2Many = MetaI1.Instance.I1I12One2Manies.AssociationType;
            MetaI1.Instance.I12sWhereI12I12Many2Many = MetaI12.Instance.I12I12Many2Manies.AssociationType;
            MetaI1.Instance.I12WhereI12I12One2Many = MetaI12.Instance.I12I12One2Manies.AssociationType;
            MetaI1.Instance.I12WhereI12I12One2One = MetaI12.Instance.I12I12One2One.AssociationType;
            MetaI1.Instance.I12sWhereDependency = MetaI12.Instance.Dependencies.AssociationType;
            MetaI1.Instance.I12sWhereI12I12Many2One = MetaI12.Instance.I12I12Many2One.AssociationType;
            MetaI1.Instance.I2sWhereI2I12Many2One = MetaI2.Instance.I2I12Many2One.AssociationType;
            MetaI1.Instance.I2WhereI2I12One2Many = MetaI2.Instance.I2I12One2Manies.AssociationType;
            MetaI1.Instance.I2WhereI2I12One2One = MetaI2.Instance.I2I12One2One.AssociationType;
            MetaI1.Instance.I2sWhereI2I12Many2Many = MetaI2.Instance.I2I12Many2Manies.AssociationType;

            MetaI2.Instance.C1sWhereC1I12Many2Many = MetaC1.Instance.C1I12Many2Manies.AssociationType;
            MetaI2.Instance.C1sWhereC1I12Many2One = MetaC1.Instance.C1I12Many2One.AssociationType;
            MetaI2.Instance.C1WhereC1I12One2Many = MetaC1.Instance.C1I12One2Manies.AssociationType;
            MetaI2.Instance.C1WhereC1I12One2One = MetaC1.Instance.C1I12One2One.AssociationType;
            MetaI2.Instance.C2sWhereC2I12Many2One = MetaC2.Instance.C2I12Many2One.AssociationType;
            MetaI2.Instance.C2WhereC2I12One2One = MetaC2.Instance.C2I12One2One.AssociationType;
            MetaI2.Instance.C2sWhereC2I12Many2Many = MetaC2.Instance.C2I12Many2Manies.AssociationType;
            MetaI2.Instance.C2WhereC2I12One2Many = MetaC2.Instance.C2I12One2Manies.AssociationType;
            MetaI2.Instance.I1sWhereI1I12Many2Many = MetaI1.Instance.I1I12Many2Manies.AssociationType;
            MetaI2.Instance.I1sWhereI1I12Many2One = MetaI1.Instance.I1I12Many2One.AssociationType;
            MetaI2.Instance.I1WhereI1I12One2One = MetaI1.Instance.I1I12One2One.AssociationType;
            MetaI2.Instance.I1WhereI1I12One2Many = MetaI1.Instance.I1I12One2Manies.AssociationType;
            MetaI2.Instance.I12sWhereI12I12Many2Many = MetaI12.Instance.I12I12Many2Manies.AssociationType;
            MetaI2.Instance.I12WhereI12I12One2Many = MetaI12.Instance.I12I12One2Manies.AssociationType;
            MetaI2.Instance.I12WhereI12I12One2One = MetaI12.Instance.I12I12One2One.AssociationType;
            MetaI2.Instance.I12sWhereDependency = MetaI12.Instance.Dependencies.AssociationType;
            MetaI2.Instance.I12sWhereI12I12Many2One = MetaI12.Instance.I12I12Many2One.AssociationType;
            MetaI2.Instance.I2sWhereI2I12Many2One = MetaI2.Instance.I2I12Many2One.AssociationType;
            MetaI2.Instance.I2WhereI2I12One2Many = MetaI2.Instance.I2I12One2Manies.AssociationType;
            MetaI2.Instance.I2WhereI2I12One2One = MetaI2.Instance.I2I12One2One.AssociationType;
            MetaI2.Instance.I2sWhereI2I12Many2Many = MetaI2.Instance.I2I12Many2Manies.AssociationType;






		}

		internal void BuildDefinedMethods()
		{
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("FAF120ED-09D1-4E42-86A6-F0D9FF75E03C"));
				method.ObjectType = MetaPerson.Instance;
				method.Name = "Method";
				MetaPerson.Instance.Method = method; 
			}











			{
				var method = new MethodType(this.metaPopulation, new System.Guid("09A6A387-A1B5-4038-B074-3A01C81CBDA2"));
				method.ObjectType = MetaC1.Instance;
				method.Name = "ClassMethod";
				method.Workspace = true;
				MetaC1.Instance.ClassMethod = method; 
			}
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("26FE4FD7-68C3-4DDA-8A44-87857B35B000"));
				method.ObjectType = MetaC1.Instance;
				method.Name = "Sum";
				MetaC1.Instance.Sum = method; 
			}



















			{
				var method = new MethodType(this.metaPopulation, new System.Guid("1869873F-F2F0-4D03-A0F9-7DC73491C117"));
				method.ObjectType = MetaOrganisation.Instance;
				method.Name = "JustDoIt";
				method.Workspace = true;
				MetaOrganisation.Instance.JustDoIt = method; 
			}












			{
				var method = new MethodType(this.metaPopulation, new System.Guid("FDD32313-CF62-4166-9167-EF90BE3A3C75"));
				method.ObjectType = MetaObject.Instance;
				method.Name = "OnBuild";
				MetaObject.Instance.OnBuild = method; 
			}
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("2B827E22-155D-4AA8-BA9F-46A64D7C79C8"));
				method.ObjectType = MetaObject.Instance;
				method.Name = "OnPostBuild";
				MetaObject.Instance.OnPostBuild = method; 
			}
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("B33F8EAE-17DC-4BF9-AFBB-E7FC38F42695"));
				method.ObjectType = MetaObject.Instance;
				method.Name = "OnPreDerive";
				MetaObject.Instance.OnPreDerive = method; 
			}
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("C107F8B3-12DC-4FF9-8CBF-A7DEC046244F"));
				method.ObjectType = MetaObject.Instance;
				method.Name = "OnDerive";
				MetaObject.Instance.OnDerive = method; 
			}
			{
				var method = new MethodType(this.metaPopulation, new System.Guid("07AFF35D-F4CB-48FE-A39A-176B1931FAB7"));
				method.ObjectType = MetaObject.Instance;
				method.Name = "OnPostDerive";
				MetaObject.Instance.OnPostDerive = method; 
			}




			{
				var method = new MethodType(this.metaPopulation, new System.Guid("430702D2-E02B-45AD-9B22-B8331DC75A3F"));
				method.ObjectType = MetaDeletable.Instance;
				method.Name = "Delete";
				method.Workspace = true;
				MetaDeletable.Instance.Delete = method; 
			}











			{
				var method = new MethodType(this.metaPopulation, new System.Guid("A360CF09-7B55-421B-A45D-D100BAF3D0D6"));
				method.ObjectType = MetaI1.Instance;
				method.Name = "InterfaceMethod";
				MetaI1.Instance.InterfaceMethod = method; 
			}


			{
				var method = new MethodType(this.metaPopulation, new System.Guid("2E52966D-6760-45A0-B687-0A0B6198A770"));
				method.ObjectType = MetaS1.Instance;
				method.Name = "SuperinterfaceMethod";
				MetaS1.Instance.SuperinterfaceMethod = method; 
			}





		}

		internal void BuildInheritedMethods()
		{
				MetaLocalisedText.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaLocalisedText.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaLocalisedText.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaLocalisedText.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaLocalisedText.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaAccessControl.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaAccessControl.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaAccessControl.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaAccessControl.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaAccessControl.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaAccessControl.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaCounter.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaCounter.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaCounter.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaCounter.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaCounter.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaCountry.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaCountry.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaCountry.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaCountry.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaCountry.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaCurrency.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaCurrency.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaCurrency.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaCurrency.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaCurrency.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaLanguage.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaLanguage.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaLanguage.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaLanguage.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaLanguage.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaLocale.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaLocale.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaLocale.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaLocale.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaLocale.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaLogin.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaLogin.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaLogin.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaLogin.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaLogin.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaLogin.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaMedia.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaMedia.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaMedia.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaMedia.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaMedia.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaMedia.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaMediaContent.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaMediaContent.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaMediaContent.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaMediaContent.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaMediaContent.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaMediaContent.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaAutomatedAgent.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaAutomatedAgent.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaAutomatedAgent.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaAutomatedAgent.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaAutomatedAgent.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaPermission.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaPermission.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaPermission.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaPermission.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaPermission.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaPermission.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaPerson.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaPerson.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaPerson.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaPerson.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaPerson.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaPerson.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaRole.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaRole.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaRole.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaRole.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaRole.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaSecurityToken.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaSecurityToken.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaSecurityToken.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaSecurityToken.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaSecurityToken.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaSecurityToken.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaSingleton.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaSingleton.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaSingleton.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaSingleton.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaSingleton.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaUserGroup.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaUserGroup.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaUserGroup.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaUserGroup.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaUserGroup.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaEmailMessage.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaEmailMessage.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaEmailMessage.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaEmailMessage.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaEmailMessage.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaNotification.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaNotification.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaNotification.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaNotification.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaNotification.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaNotificationList.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaNotificationList.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaNotificationList.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaNotificationList.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaNotificationList.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaNotificationList.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaTaskAssignment.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaTaskAssignment.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaTaskAssignment.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaTaskAssignment.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaTaskAssignment.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaTaskAssignment.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaTaskList.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaTaskList.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaTaskList.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaTaskList.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaTaskList.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaTaskList.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaBuild.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaBuild.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaBuild.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaBuild.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaBuild.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaBadUI.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaBadUI.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaBadUI.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaBadUI.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaBadUI.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaC1.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaC1.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaC1.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaC1.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaC1.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaC1.Instance.InterfaceMethod = MetaI1.Instance.InterfaceMethod;
				MetaC1.Instance.SuperinterfaceMethod = MetaS1.Instance.SuperinterfaceMethod;
				MetaC2.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaC2.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaC2.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaC2.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaC2.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaClassWithoutRoles.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaClassWithoutRoles.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaClassWithoutRoles.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaClassWithoutRoles.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaClassWithoutRoles.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaDependee.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaDependee.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaDependee.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaDependee.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaDependee.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaDependent.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaDependent.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaDependent.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaDependent.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaDependent.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaDependent.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaExtender.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaExtender.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaExtender.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaExtender.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaExtender.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaFirst.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaFirst.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaFirst.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaFirst.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaFirst.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaFour.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaFour.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaFour.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaFour.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaFour.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaFrom.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaFrom.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaFrom.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaFrom.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaFrom.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaGender.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaGender.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaGender.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaGender.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaGender.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaHomeAddress.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaHomeAddress.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaHomeAddress.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaHomeAddress.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaHomeAddress.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaMailboxAddress.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaMailboxAddress.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaMailboxAddress.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaMailboxAddress.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaMailboxAddress.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOne.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOne.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOne.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOne.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOne.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrder.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrder.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrder.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrder.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrder.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrderLine.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrderLine.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrderLine.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrderLine.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrderLine.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrderLineVersion.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrderLineVersion.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrderLineVersion.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrderLineVersion.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrderLineVersion.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaPaymentState.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaPaymentState.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaPaymentState.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaPaymentState.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaPaymentState.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaShipmentState.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaShipmentState.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaShipmentState.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaShipmentState.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaShipmentState.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrderState.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrderState.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrderState.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrderState.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrderState.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrderVersion.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrderVersion.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrderVersion.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrderVersion.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrderVersion.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrganisation.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaOrganisation.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaOrganisation.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaOrganisation.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaOrganisation.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaOrganisation.Instance.Delete = MetaDeletable.Instance.Delete;
				MetaPlace.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaPlace.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaPlace.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaPlace.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaPlace.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaSecond.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaSecond.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaSecond.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaSecond.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaSecond.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaSimpleJob.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaSimpleJob.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaSimpleJob.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaSimpleJob.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaSimpleJob.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaStatefulCompany.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaStatefulCompany.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaStatefulCompany.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaStatefulCompany.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaStatefulCompany.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaSubdependee.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaSubdependee.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaSubdependee.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaSubdependee.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaSubdependee.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaThird.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaThird.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaThird.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaThird.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaThird.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaThree.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaThree.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaThree.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaThree.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaThree.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaTo.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaTo.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaTo.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaTo.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaTo.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaTwo.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaTwo.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaTwo.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaTwo.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaTwo.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaUnitSample.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaUnitSample.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaUnitSample.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaUnitSample.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaUnitSample.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaValidationC1.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaValidationC1.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaValidationC1.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaValidationC1.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaValidationC1.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;
				MetaValidationC2.Instance.OnBuild = MetaObject.Instance.OnBuild;
				MetaValidationC2.Instance.OnPostBuild = MetaObject.Instance.OnPostBuild;
				MetaValidationC2.Instance.OnPreDerive = MetaObject.Instance.OnPreDerive;
				MetaValidationC2.Instance.OnDerive = MetaObject.Instance.OnDerive;
				MetaValidationC2.Instance.OnPostDerive = MetaObject.Instance.OnPostDerive;


























		}

		internal void ExtendInterfaces()
		{
            MetaObject.Instance.Extend();
            MetaCachable.Instance.Extend();
            MetaVersion.Instance.Extend();
            MetaVersioned.Instance.Extend();
            MetaAccessControlledObject.Instance.Extend();
            MetaDeletable.Instance.Extend();
            MetaEnumeration.Instance.Extend();
            MetaLocalised.Instance.Extend();
            MetaObjectState.Instance.Extend();
            MetaSecurityTokenOwner.Instance.Extend();
            MetaTransitionalVersion.Instance.Extend();
            MetaTransitional.Instance.Extend();
            MetaUniquelyIdentifiable.Instance.Extend();
            MetaUser.Instance.Extend();
            MetaTask.Instance.Extend();
            MetaWorkItem.Instance.Extend();
            MetaAddress.Instance.Extend();
            MetaI1.Instance.Extend();
            MetaI12.Instance.Extend();
            MetaI2.Instance.Extend();
            MetaS1.Instance.Extend();
            MetaShared.Instance.Extend();
            MetaSyncDepth1.Instance.Extend();
            MetaSyncDepth2.Instance.Extend();
            MetaSyncRoot.Instance.Extend();
            MetaValidationI12.Instance.Extend();
		}

		internal void ExtendClasses()
		{
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
            MetaEmailMessage.Instance.Extend();
            MetaNotification.Instance.Extend();
            MetaNotificationList.Instance.Extend();
            MetaTaskAssignment.Instance.Extend();
            MetaTaskList.Instance.Extend();
            MetaBuild.Instance.Extend();
            MetaBadUI.Instance.Extend();
            MetaC1.Instance.Extend();
            MetaC2.Instance.Extend();
            MetaClassWithoutRoles.Instance.Extend();
            MetaDependee.Instance.Extend();
            MetaDependent.Instance.Extend();
            MetaExtender.Instance.Extend();
            MetaFirst.Instance.Extend();
            MetaFour.Instance.Extend();
            MetaFrom.Instance.Extend();
            MetaGender.Instance.Extend();
            MetaHomeAddress.Instance.Extend();
            MetaMailboxAddress.Instance.Extend();
            MetaOne.Instance.Extend();
            MetaOrder.Instance.Extend();
            MetaOrderLine.Instance.Extend();
            MetaOrderLineVersion.Instance.Extend();
            MetaPaymentState.Instance.Extend();
            MetaShipmentState.Instance.Extend();
            MetaOrderState.Instance.Extend();
            MetaOrderVersion.Instance.Extend();
            MetaOrganisation.Instance.Extend();
            MetaPlace.Instance.Extend();
            MetaSecond.Instance.Extend();
            MetaSimpleJob.Instance.Extend();
            MetaStatefulCompany.Instance.Extend();
            MetaSubdependee.Instance.Extend();
            MetaThird.Instance.Extend();
            MetaThree.Instance.Extend();
            MetaTo.Instance.Extend();
            MetaTwo.Instance.Extend();
            MetaUnitSample.Instance.Extend();
            MetaValidationC1.Instance.Extend();
            MetaValidationC2.Instance.Extend();
		}
	}
}