namespace Allors.Workspace.Meta
{
	using Allors.Meta;

    public partial class MetaObject : MetaInterface
	{
	    public static MetaObject Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Inherited Methods

		// Roles

		// Associations

		internal MetaObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("12504f04-02c6-4778-98fe-04eba12ef8b2"))
			{
				SingularName = "Object",
				PluralName = "Objects"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaCachable : MetaInterface
	{
	    public static MetaCachable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType CacheId;

		// Associations

		internal MetaCachable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b17afc19-9e91-4631-b6d8-43b32a65e0a0"))
			{
				SingularName = "Cachable",
				PluralName = "Cachables"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaVersion : MetaInterface
	{
	    public static MetaVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a6a3c79e-150b-4586-96ea-5ac0e2e638c6"))
			{
				SingularName = "Version",
				PluralName = "Versions"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaVersioned : MetaInterface
	{
	    public static MetaVersioned Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles

		// Associations

		internal MetaVersioned(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("39f9cb84-b321-424a-864c-0b128acaa965"))
			{
				SingularName = "Versioned",
				PluralName = "Versioneds"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaAccessControlledObject : MetaInterface
	{
	    public static MetaAccessControlledObject Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaAccessControlledObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("eb0ff756-3e3d-4cf9-8935-8802a73d2df2"))
			{
				SingularName = "AccessControlledObject",
				PluralName = "AccessControlledObjects"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaDeletable : MetaInterface
	{
	    public static MetaDeletable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles

		// Associations

		internal MetaDeletable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("9279e337-c658-4086-946d-03c75cdb1ad3"))
			{
				SingularName = "Deletable",
				PluralName = "Deletables"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaEnumeration : MetaInterface
	{
	    public static MetaEnumeration Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType LocalisedNames;
        public RoleType Name;
        public RoleType IsActive;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Associations
        public AssociationType TargetNotifications;

		internal MetaEnumeration(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b7bcc22f-03f0-46fd-b738-4e035921d445"))
			{
				SingularName = "Enumeration",
				PluralName = "Enumerations"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaLocalised : MetaInterface
	{
	    public static MetaLocalised Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType Locale;

		// Associations

		internal MetaLocalised(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7979a17c-0829-46df-a0d4-1b01775cfaac"))
			{
				SingularName = "Localised",
				PluralName = "Localiseds"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaObjectState : MetaInterface
	{
	    public static MetaObjectState Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType DeniedPermissions;
        public RoleType Name;
        public RoleType UniqueId;

		// Associations
        public AssociationType PreviousObjectStateTransitionalVersions;
        public AssociationType LastObjectStateTransitionalVersions;
        public AssociationType ObjectStateTransitionalVersions;
        public AssociationType PreviousObjectStateTransitionals;
        public AssociationType LastObjectStateTransitionals;
        public AssociationType ObjectStateTransitionals;
        public AssociationType TargetNotifications;

		internal MetaObjectState(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f991813f-3146-4431-96d0-554aa2186887"))
			{
				SingularName = "ObjectState",
				PluralName = "ObjectStates"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaSecurityTokenOwner : MetaInterface
	{
	    public static MetaSecurityTokenOwner Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;

		// Associations

		internal MetaSecurityTokenOwner(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a69cad9c-c2f1-463f-9af1-873ce65aeea6"))
			{
				SingularName = "SecurityTokenOwner",
				PluralName = "SecurityTokenOwners"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaTransitionalVersion : MetaInterface
	{
	    public static MetaTransitionalVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaTransitionalVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a13c9057-8786-40ca-8421-476e55787d73"))
			{
				SingularName = "TransitionalVersion",
				PluralName = "TransitionalVersions"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaTransitional : MetaInterface
	{
	    public static MetaTransitional Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Associations

		internal MetaTransitional(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("ab2179ad-9eac-4b61-8d84-81cd777c4926"))
			{
				SingularName = "Transitional",
				PluralName = "Transitionals"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaUniquelyIdentifiable : MetaInterface
	{
	    public static MetaUniquelyIdentifiable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType UniqueId;

		// Associations
        public AssociationType TargetNotifications;

		internal MetaUniquelyIdentifiable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("122ccfe1-f902-44c1-9d6c-6f6a0afa9469"))
			{
				SingularName = "UniquelyIdentifiable",
				PluralName = "UniquelyIdentifiables"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaUser : MetaInterface
	{
	    public static MetaUser Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType UserName;
        public RoleType NormalizedUserName;
        public RoleType UserPasswordHash;
        public RoleType UserEmail;
        public RoleType UserEmailConfirmed;
        public RoleType TaskList;
        public RoleType NotificationList;
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;

		// Associations
        public AssociationType SubjectAccessControls;
        public AssociationType EffectiveUserAccessControls;
        public AssociationType UserLogins;
        public AssociationType GuestSingleton;
        public AssociationType MemberUserGroups;
        public AssociationType SenderEmailMessages;
        public AssociationType RecipientEmailMessages;
        public AssociationType UserTaskAssignments;

		internal MetaUser(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a0309c3b-6f80-4777-983e-6e69800df5be"))
			{
				SingularName = "User",
				PluralName = "Users"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaTask : MetaInterface
	{
	    public static MetaTask Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Roles
        public RoleType WorkItem;
        public RoleType DateCreated;
        public RoleType DateClosed;
        public RoleType Participants;
        public RoleType Performer;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Associations
        public AssociationType TaskTaskAssignments;
        public AssociationType TargetNotifications;

		internal MetaTask(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("84eb0e6e-68e1-478c-a35f-6036d45792be"))
			{
				SingularName = "Task",
				PluralName = "Tasks"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaWorkItem : MetaInterface
	{
	    public static MetaWorkItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType WorkItemDescription;

		// Associations
        public AssociationType WorkItemTasks;

		internal MetaWorkItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fbea29c6-6109-4163-a088-9f0b4deac896"))
			{
				SingularName = "WorkItem",
				PluralName = "WorkItems"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaAddress : MetaInterface
	{
	    public static MetaAddress Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType Place;

		// Associations
        public AssociationType AddressPeople;
        public AssociationType MainAddressPeople;
        public AssociationType AddressOrganisation;
        public AssociationType MainAddressOrganisations;

		internal MetaAddress(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("130aa2ff-4f14-4ad7-8a27-f80e8aebfa00"))
			{
				SingularName = "Address",
				PluralName = "Addresses"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaI1 : MetaInterface
	{
	    public static MetaI1 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType InterfaceMethod;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType SuperinterfaceMethod;

		// Roles
        public RoleType I1I1Many2One;
        public RoleType I1I12Many2Manies;
        public RoleType I1I2Many2Manies;
        public RoleType I1I2Many2One;
        public RoleType I1AllorsString;
        public RoleType I1I12Many2One;
        public RoleType I1AllorsDateTime;
        public RoleType I1I2One2Manies;
        public RoleType I1C2One2Manies;
        public RoleType I1C1One2One;
        public RoleType I1AllorsInteger;
        public RoleType I1C2Many2Manies;
        public RoleType I1I1One2Manies;
        public RoleType I1I1Many2Manies;
        public RoleType I1AllorsBoolean;
        public RoleType I1AllorsDecimal;
        public RoleType I1I12One2One;
        public RoleType I1I2One2One;
        public RoleType I1C2One2One;
        public RoleType I1C1One2Manies;
        public RoleType I1AllorsBinary;
        public RoleType I1C1Many2Manies;
        public RoleType I1AllorsDouble;
        public RoleType I1I1One2One;
        public RoleType I1C1Many2One;
        public RoleType I1I12One2Manies;
        public RoleType I1C2Many2One;
        public RoleType I1AllorsUnique;
        public RoleType I12AllorsBinary;
        public RoleType I12C2One2One;
        public RoleType I12AllorsDouble;
        public RoleType I12I1Many2One;
        public RoleType I12AllorsString;
        public RoleType I12I12Many2Manies;
        public RoleType I12AllorsDecimal;
        public RoleType I12I2Many2Manies;
        public RoleType I12C2Many2Manies;
        public RoleType I12I1Many2Manies;
        public RoleType I12I12One2Manies;
        public RoleType Name;
        public RoleType I12C1Many2Manies;
        public RoleType I12I2Many2One;
        public RoleType I12AllorsUnique;
        public RoleType I12AllorsInteger;
        public RoleType I12I1One2Manies;
        public RoleType I12C1One2One;
        public RoleType I12I12One2One;
        public RoleType I12I2One2One;
        public RoleType Dependencies;
        public RoleType I12I2One2Manies;
        public RoleType I12C2Many2One;
        public RoleType I12I12Many2One;
        public RoleType I12AllorsBoolean;
        public RoleType I12I1One2One;
        public RoleType I12C1One2Manies;
        public RoleType I12C1Many2One;
        public RoleType I12AllorsDateTime;

		// Associations
        public AssociationType C1I1Many2ManyC1s;
        public AssociationType C1I1Many2OneC1s;
        public AssociationType C1I1One2ManyC1;
        public AssociationType C1I1One2OneC1;
        public AssociationType C2I1Many2ManyC2s;
        public AssociationType C2I1One2ManyC2;
        public AssociationType C2I1Many2OneC2s;
        public AssociationType C2I1One2OneC2;
        public AssociationType I1I1Many2OneI1s;
        public AssociationType I1I1One2ManyI1;
        public AssociationType I1I1Many2ManyI1s;
        public AssociationType I1I1One2OneI1;
        public AssociationType I12I1Many2OneI12s;
        public AssociationType I12I1Many2ManyI12s;
        public AssociationType I12I1One2ManyI12;
        public AssociationType I12I1One2OneI12;
        public AssociationType I2I1Many2OneI2s;
        public AssociationType I2I1Many2ManyI2s;
        public AssociationType I2I1One2OneI2;
        public AssociationType I2I1One2ManyI2;
        public AssociationType C1I12Many2ManyC1s;
        public AssociationType C1I12Many2OneC1s;
        public AssociationType C1I12One2ManyC1;
        public AssociationType C1I12One2OneC1;
        public AssociationType C2I12Many2OneC2s;
        public AssociationType C2I12One2OneC2;
        public AssociationType C2I12Many2ManyC2s;
        public AssociationType C2I12One2ManyC2;
        public AssociationType I1I12Many2ManyI1s;
        public AssociationType I1I12Many2OneI1s;
        public AssociationType I1I12One2OneI1;
        public AssociationType I1I12One2ManyI1;
        public AssociationType I12I12Many2ManyI12s;
        public AssociationType I12I12One2ManyI12;
        public AssociationType I12I12One2OneI12;
        public AssociationType DependencyI12s;
        public AssociationType I12I12Many2OneI12s;
        public AssociationType I2I12Many2OneI2s;
        public AssociationType I2I12One2ManyI2;
        public AssociationType I2I12One2OneI2;
        public AssociationType I2I12Many2ManyI2s;

		internal MetaI1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fefcf1b6-ac8f-47b0-bed5-939207a2833e"))
			{
				SingularName = "I1",
				PluralName = "I1s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaI12 : MetaInterface
	{
	    public static MetaI12 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType I12AllorsBinary;
        public RoleType I12C2One2One;
        public RoleType I12AllorsDouble;
        public RoleType I12I1Many2One;
        public RoleType I12AllorsString;
        public RoleType I12I12Many2Manies;
        public RoleType I12AllorsDecimal;
        public RoleType I12I2Many2Manies;
        public RoleType I12C2Many2Manies;
        public RoleType I12I1Many2Manies;
        public RoleType I12I12One2Manies;
        public RoleType Name;
        public RoleType I12C1Many2Manies;
        public RoleType I12I2Many2One;
        public RoleType I12AllorsUnique;
        public RoleType I12AllorsInteger;
        public RoleType I12I1One2Manies;
        public RoleType I12C1One2One;
        public RoleType I12I12One2One;
        public RoleType I12I2One2One;
        public RoleType Dependencies;
        public RoleType I12I2One2Manies;
        public RoleType I12C2Many2One;
        public RoleType I12I12Many2One;
        public RoleType I12AllorsBoolean;
        public RoleType I12I1One2One;
        public RoleType I12C1One2Manies;
        public RoleType I12C1Many2One;
        public RoleType I12AllorsDateTime;

		// Associations
        public AssociationType C1I12Many2ManyC1s;
        public AssociationType C1I12Many2OneC1s;
        public AssociationType C1I12One2ManyC1;
        public AssociationType C1I12One2OneC1;
        public AssociationType C2I12Many2OneC2s;
        public AssociationType C2I12One2OneC2;
        public AssociationType C2I12Many2ManyC2s;
        public AssociationType C2I12One2ManyC2;
        public AssociationType I1I12Many2ManyI1s;
        public AssociationType I1I12Many2OneI1s;
        public AssociationType I1I12One2OneI1;
        public AssociationType I1I12One2ManyI1;
        public AssociationType I12I12Many2ManyI12s;
        public AssociationType I12I12One2ManyI12;
        public AssociationType I12I12One2OneI12;
        public AssociationType DependencyI12s;
        public AssociationType I12I12Many2OneI12s;
        public AssociationType I2I12Many2OneI2s;
        public AssociationType I2I12One2ManyI2;
        public AssociationType I2I12One2OneI2;
        public AssociationType I2I12Many2ManyI2s;

		internal MetaI12(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b45ec13c-704f-413d-a662-bdc59a17bfe3"))
			{
				SingularName = "I12",
				PluralName = "I12s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaI2 : MetaInterface
	{
	    public static MetaI2 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType I2I2Many2One;
        public RoleType I2C1Many2One;
        public RoleType I2I12Many2One;
        public RoleType I2AllorsBoolean;
        public RoleType I2C1One2Manies;
        public RoleType I2C1One2One;
        public RoleType I2AllorsDecimal;
        public RoleType I2I2Many2Manies;
        public RoleType I2AllorsBinary;
        public RoleType I2AllorsUnique;
        public RoleType I2I1Many2One;
        public RoleType I2AllorsDateTime;
        public RoleType I2I12One2Manies;
        public RoleType I2I12One2One;
        public RoleType I2C2Many2Manies;
        public RoleType I2I1Many2Manies;
        public RoleType I2C2Many2One;
        public RoleType I2AllorsString;
        public RoleType I2C2One2Manies;
        public RoleType I2I1One2One;
        public RoleType I2I1One2Manies;
        public RoleType I2I12Many2Manies;
        public RoleType I2I2One2One;
        public RoleType I2AllorsInteger;
        public RoleType I2I2One2Manies;
        public RoleType I2C1Many2Manies;
        public RoleType I2C2One2One;
        public RoleType I2AllorsDouble;
        public RoleType I12AllorsBinary;
        public RoleType I12C2One2One;
        public RoleType I12AllorsDouble;
        public RoleType I12I1Many2One;
        public RoleType I12AllorsString;
        public RoleType I12I12Many2Manies;
        public RoleType I12AllorsDecimal;
        public RoleType I12I2Many2Manies;
        public RoleType I12C2Many2Manies;
        public RoleType I12I1Many2Manies;
        public RoleType I12I12One2Manies;
        public RoleType Name;
        public RoleType I12C1Many2Manies;
        public RoleType I12I2Many2One;
        public RoleType I12AllorsUnique;
        public RoleType I12AllorsInteger;
        public RoleType I12I1One2Manies;
        public RoleType I12C1One2One;
        public RoleType I12I12One2One;
        public RoleType I12I2One2One;
        public RoleType Dependencies;
        public RoleType I12I2One2Manies;
        public RoleType I12C2Many2One;
        public RoleType I12I12Many2One;
        public RoleType I12AllorsBoolean;
        public RoleType I12I1One2One;
        public RoleType I12C1One2Manies;
        public RoleType I12C1Many2One;
        public RoleType I12AllorsDateTime;

		// Associations
        public AssociationType C1I2Many2ManyC1s;
        public AssociationType C1I2Many2OneC1s;
        public AssociationType C1I2One2ManyC1;
        public AssociationType C1I2One2OneC1;
        public AssociationType C2I2One2OneC2;
        public AssociationType C2I2Many2ManyC2s;
        public AssociationType C2I2One2ManyC2;
        public AssociationType C2I2Many2OneC2s;
        public AssociationType I1I2Many2ManyI1s;
        public AssociationType I1I2Many2OneI1s;
        public AssociationType I1I2One2ManyI1;
        public AssociationType I1I2One2OneI1;
        public AssociationType I12I2Many2ManyI12s;
        public AssociationType I12I2Many2OneI12s;
        public AssociationType I12I2One2OneI12;
        public AssociationType I12I2One2ManyI12;
        public AssociationType I2I2Many2OneI2s;
        public AssociationType I2I2Many2ManyI2s;
        public AssociationType I2I2One2OneI2;
        public AssociationType I2I2One2ManyI2;
        public AssociationType C1I12Many2ManyC1s;
        public AssociationType C1I12Many2OneC1s;
        public AssociationType C1I12One2ManyC1;
        public AssociationType C1I12One2OneC1;
        public AssociationType C2I12Many2OneC2s;
        public AssociationType C2I12One2OneC2;
        public AssociationType C2I12Many2ManyC2s;
        public AssociationType C2I12One2ManyC2;
        public AssociationType I1I12Many2ManyI1s;
        public AssociationType I1I12Many2OneI1s;
        public AssociationType I1I12One2OneI1;
        public AssociationType I1I12One2ManyI1;
        public AssociationType I12I12Many2ManyI12s;
        public AssociationType I12I12One2ManyI12;
        public AssociationType I12I12One2OneI12;
        public AssociationType DependencyI12s;
        public AssociationType I12I12Many2OneI12s;
        public AssociationType I2I12Many2OneI2s;
        public AssociationType I2I12One2ManyI2;
        public AssociationType I2I12One2OneI2;
        public AssociationType I2I12Many2ManyI2s;

		internal MetaI2(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0"))
			{
				SingularName = "I2",
				PluralName = "I2s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaS1 : MetaInterface
	{
	    public static MetaS1 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods
        public MethodType SuperinterfaceMethod;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles

		// Associations

		internal MetaS1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("253b0d71-9eaa-4d87-9094-3b549d8446b3"))
			{
				SingularName = "S1",
				PluralName = "S1s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaShared : MetaInterface
	{
	    public static MetaShared Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles

		// Associations
        public AssociationType SharedTwos;

		internal MetaShared(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("5c3876c3-c3be-46aa-a598-a68b964d329e"))
			{
				SingularName = "Shared",
				PluralName = "Shareds"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaSyncDepth1 : MetaInterface
	{
	    public static MetaSyncDepth1 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType SyncDepth2;

		// Associations
        public AssociationType SyncDepth1SyncRoots;

		internal MetaSyncDepth1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fbc46730-b636-4333-989c-53d5f76a32a0"))
			{
				SingularName = "SyncDepth1",
				PluralName = "SyncDepth1s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaSyncDepth2 : MetaInterface
	{
	    public static MetaSyncDepth2 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles

		// Associations
        public AssociationType SyncDepth2SyncDepth1s;

		internal MetaSyncDepth2(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b9996f8f-12fb-4e42-8b7f-907433a622b2"))
			{
				SingularName = "SyncDepth2",
				PluralName = "SyncDepth2s"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaSyncRoot : MetaInterface
	{
	    public static MetaSyncRoot Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType SyncDepth1;

		// Associations

		internal MetaSyncRoot(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("2a863dcf-c6fe-4838-8d3a-1212a2076d70"))
			{
				SingularName = "SyncRoot",
				PluralName = "SyncRoots"
				// TODO: XmlDoc
			};
        }
	}

    public partial class MetaValidationI12 : MetaInterface
	{
	    public static MetaValidationI12 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Roles
        public RoleType UniqueId;

		// Associations

		internal MetaValidationI12(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("d61872ee-3778-47e8-8931-003f3f48cbc5"))
			{
				SingularName = "ValidationI12",
				PluralName = "ValidationI12s"
				// TODO: XmlDoc
			};
        }
	}
}