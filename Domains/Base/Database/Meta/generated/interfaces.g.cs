namespace Allors.Meta
{
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

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("12504f04-02c6-4778-98fe-04eba12ef8b2"))
			{
				SingularName = "Object",
			};
        }
	}

    public partial class MetaCachable : MetaInterface
	{
	    public static MetaCachable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType CacheId;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaCachable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("B17AFC19-9E91-4631-B6D8-43B32A65E0A0"))
			{
				SingularName = "Cachable",
			};
        }
	}

    public partial class MetaVersion : MetaInterface
	{
	    public static MetaVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DerivationId;
        public RoleType DerivationTimeStamp;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("A6A3C79E-150B-4586-96EA-5AC0E2E638C6"))
			{
				SingularName = "Version",
			};
        }
	}

    public partial class MetaVersioned : MetaInterface
	{
	    public static MetaVersioned Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaVersioned(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("39F9CB84-B321-424A-864C-0B128ACAA965"))
			{
				SingularName = "Versioned",
			};
        }
	}

    public partial class MetaAccessControlledObject : MetaInterface
	{
	    public static MetaAccessControlledObject Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaAccessControlledObject(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("eb0ff756-3e3d-4cf9-8935-8802a73d2df2"))
			{
				SingularName = "AccessControlledObject",
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

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaDeletable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("9279e337-c658-4086-946d-03c75cdb1ad3"))
			{
				SingularName = "Deletable",
			};
        }
	}

    public partial class MetaEnumeration : MetaInterface
	{
	    public static MetaEnumeration Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType LocalisedNames;
        public RoleType Name;
        public RoleType IsActive;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaEnumeration(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b7bcc22f-03f0-46fd-b738-4e035921d445"))
			{
				SingularName = "Enumeration",
			};
        }
	}

    public partial class MetaLocalised : MetaInterface
	{
	    public static MetaLocalised Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Locale;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaLocalised(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("7979a17c-0829-46df-a0d4-1b01775cfaac"))
			{
				SingularName = "Localised",
			};
        }
	}

    public partial class MetaObjectState : MetaInterface
	{
	    public static MetaObjectState Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType DeniedPermissions;
        public RoleType Name;

		// Inherited Roles
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaObjectState(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("f991813f-3146-4431-96d0-554aa2186887"))
			{
				SingularName = "ObjectState",
			};
        }
	}

    public partial class MetaSecurityTokenOwner : MetaInterface
	{
	    public static MetaSecurityTokenOwner Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaSecurityTokenOwner(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a69cad9c-c2f1-463f-9af1-873ce65aeea6"))
			{
				SingularName = "SecurityTokenOwner",
			};
        }
	}

    public partial class MetaTransitionalVersion : MetaInterface
	{
	    public static MetaTransitionalVersion Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaTransitionalVersion(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("A13C9057-8786-40CA-8421-476E55787D73"))
			{
				SingularName = "TransitionalVersion",
			};
        }
	}

    public partial class MetaTransitional : MetaInterface
	{
	    public static MetaTransitional Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType PreviousObjectStates;
        public RoleType LastObjectStates;
        public RoleType ObjectStates;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations

		internal MetaTransitional(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("ab2179ad-9eac-4b61-8d84-81cd777c4926"))
			{
				SingularName = "Transitional",
			};
        }
	}

    public partial class MetaUniquelyIdentifiable : MetaInterface
	{
	    public static MetaUniquelyIdentifiable Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType UniqueId;

		// Inherited Roles

		// Defined Associations
        public AssociationType NotificationsWhereTarget;

		// Inherited Associations

		internal MetaUniquelyIdentifiable(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("122ccfe1-f902-44c1-9d6c-6f6a0afa9469"))
			{
				SingularName = "UniquelyIdentifiable",
			};
        }
	}

    public partial class MetaUser : MetaInterface
	{
	    public static MetaUser Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType UserName;
        public RoleType NormalizedUserName;
        public RoleType UserPasswordHash;
        public RoleType UserEmail;
        public RoleType UserEmailConfirmed;
        public RoleType TaskList;
        public RoleType NotificationList;

		// Inherited Roles
        public RoleType OwnerSecurityToken;
        public RoleType OwnerAccessControl;
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType Locale;

		// Defined Associations
        public AssociationType AccessControlsWhereSubject;
        public AssociationType AccessControlsWhereEffectiveUser;
        public AssociationType LoginsWhereUser;
        public AssociationType SingletonWhereGuest;
        public AssociationType UserGroupsWhereMember;
        public AssociationType EmailMessagesWhereSender;
        public AssociationType EmailMessagesWhereRecipient;
        public AssociationType TaskAssignmentsWhereUser;

		// Inherited Associations

		internal MetaUser(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("a0309c3b-6f80-4777-983e-6e69800df5be"))
			{
				SingularName = "User",
			};
        }
	}

    public partial class MetaTask : MetaInterface
	{
	    public static MetaTask Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType WorkItem;
        public RoleType DateCreated;
        public RoleType DateClosed;
        public RoleType Participants;
        public RoleType Performer;

		// Inherited Roles
        public RoleType DeniedPermissions;
        public RoleType SecurityTokens;
        public RoleType UniqueId;

		// Defined Associations
        public AssociationType TaskAssignmentsWhereTask;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;

		internal MetaTask(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("84eb0e6e-68e1-478c-a35f-6036d45792be"))
			{
				SingularName = "Task",
			};
        }
	}

    public partial class MetaWorkItem : MetaInterface
	{
	    public static MetaWorkItem Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType WorkItemDescription;

		// Inherited Roles

		// Defined Associations
        public AssociationType TasksWhereWorkItem;

		// Inherited Associations

		internal MetaWorkItem(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fbea29c6-6109-4163-a088-9f0b4deac896"))
			{
				SingularName = "WorkItem",
			};
        }
	}

    public partial class MetaAddress : MetaInterface
	{
	    public static MetaAddress Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType Place;

		// Inherited Roles

		// Defined Associations
        public AssociationType PeopleWhereAddress;
        public AssociationType PeopleWhereMainAddress;
        public AssociationType OrganisationWhereAddress;
        public AssociationType OrganisationsWhereMainAddress;

		// Inherited Associations

		internal MetaAddress(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("130aa2ff-4f14-4ad7-8a27-f80e8aebfa00"))
			{
				SingularName = "Address",
				PluralName = "Addresses",
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

		// Defined Roles
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

		// Inherited Roles
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

		// Defined Associations
        public AssociationType C1sWhereC1I1Many2Many;
        public AssociationType C1sWhereC1I1Many2One;
        public AssociationType C1WhereC1I1One2Many;
        public AssociationType C1WhereC1I1One2One;
        public AssociationType C2sWhereC2I1Many2Many;
        public AssociationType C2WhereC2I1One2Many;
        public AssociationType C2sWhereC2I1Many2One;
        public AssociationType C2WhereC2I1One2One;
        public AssociationType I1sWhereI1I1Many2One;
        public AssociationType I1WhereI1I1One2Many;
        public AssociationType I1sWhereI1I1Many2Many;
        public AssociationType I1WhereI1I1One2One;
        public AssociationType I12sWhereI12I1Many2One;
        public AssociationType I12sWhereI12I1Many2Many;
        public AssociationType I12WhereI12I1One2Many;
        public AssociationType I12WhereI12I1One2One;
        public AssociationType I2sWhereI2I1Many2One;
        public AssociationType I2sWhereI2I1Many2Many;
        public AssociationType I2WhereI2I1One2One;
        public AssociationType I2WhereI2I1One2Many;

		// Inherited Associations
        public AssociationType C1sWhereC1I12Many2Many;
        public AssociationType C1sWhereC1I12Many2One;
        public AssociationType C1WhereC1I12One2Many;
        public AssociationType C1WhereC1I12One2One;
        public AssociationType C2sWhereC2I12Many2One;
        public AssociationType C2WhereC2I12One2One;
        public AssociationType C2sWhereC2I12Many2Many;
        public AssociationType C2WhereC2I12One2Many;
        public AssociationType I1sWhereI1I12Many2Many;
        public AssociationType I1sWhereI1I12Many2One;
        public AssociationType I1WhereI1I12One2One;
        public AssociationType I1WhereI1I12One2Many;
        public AssociationType I12sWhereI12I12Many2Many;
        public AssociationType I12WhereI12I12One2Many;
        public AssociationType I12WhereI12I12One2One;
        public AssociationType I12sWhereDependency;
        public AssociationType I12sWhereI12I12Many2One;
        public AssociationType I2sWhereI2I12Many2One;
        public AssociationType I2WhereI2I12One2Many;
        public AssociationType I2WhereI2I12One2One;
        public AssociationType I2sWhereI2I12Many2Many;

		internal MetaI1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("fefcf1b6-ac8f-47b0-bed5-939207a2833e"))
			{
				SingularName = "I1",
			};
        }
	}

    public partial class MetaI12 : MetaInterface
	{
	    public static MetaI12 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
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

		// Inherited Roles

		// Defined Associations
        public AssociationType C1sWhereC1I12Many2Many;
        public AssociationType C1sWhereC1I12Many2One;
        public AssociationType C1WhereC1I12One2Many;
        public AssociationType C1WhereC1I12One2One;
        public AssociationType C2sWhereC2I12Many2One;
        public AssociationType C2WhereC2I12One2One;
        public AssociationType C2sWhereC2I12Many2Many;
        public AssociationType C2WhereC2I12One2Many;
        public AssociationType I1sWhereI1I12Many2Many;
        public AssociationType I1sWhereI1I12Many2One;
        public AssociationType I1WhereI1I12One2One;
        public AssociationType I1WhereI1I12One2Many;
        public AssociationType I12sWhereI12I12Many2Many;
        public AssociationType I12WhereI12I12One2Many;
        public AssociationType I12WhereI12I12One2One;
        public AssociationType I12sWhereDependency;
        public AssociationType I12sWhereI12I12Many2One;
        public AssociationType I2sWhereI2I12Many2One;
        public AssociationType I2WhereI2I12One2Many;
        public AssociationType I2WhereI2I12One2One;
        public AssociationType I2sWhereI2I12Many2Many;

		// Inherited Associations

		internal MetaI12(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("b45ec13c-704f-413d-a662-bdc59a17bfe3"))
			{
				SingularName = "I12",
			};
        }
	}

    public partial class MetaI2 : MetaInterface
	{
	    public static MetaI2 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
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

		// Inherited Roles
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

		// Defined Associations
        public AssociationType C1sWhereC1I2Many2Many;
        public AssociationType C1sWhereC1I2Many2One;
        public AssociationType C1WhereC1I2One2Many;
        public AssociationType C1WhereC1I2One2One;
        public AssociationType C2WhereC2I2One2One;
        public AssociationType C2sWhereC2I2Many2Many;
        public AssociationType C2WhereC2I2One2Many;
        public AssociationType C2sWhereC2I2Many2One;
        public AssociationType I1sWhereI1I2Many2Many;
        public AssociationType I1sWhereI1I2Many2One;
        public AssociationType I1WhereI1I2One2Many;
        public AssociationType I1WhereI1I2One2One;
        public AssociationType I12sWhereI12I2Many2Many;
        public AssociationType I12sWhereI12I2Many2One;
        public AssociationType I12WhereI12I2One2One;
        public AssociationType I12WhereI12I2One2Many;
        public AssociationType I2sWhereI2I2Many2One;
        public AssociationType I2sWhereI2I2Many2Many;
        public AssociationType I2WhereI2I2One2One;
        public AssociationType I2WhereI2I2One2Many;

		// Inherited Associations
        public AssociationType C1sWhereC1I12Many2Many;
        public AssociationType C1sWhereC1I12Many2One;
        public AssociationType C1WhereC1I12One2Many;
        public AssociationType C1WhereC1I12One2One;
        public AssociationType C2sWhereC2I12Many2One;
        public AssociationType C2WhereC2I12One2One;
        public AssociationType C2sWhereC2I12Many2Many;
        public AssociationType C2WhereC2I12One2Many;
        public AssociationType I1sWhereI1I12Many2Many;
        public AssociationType I1sWhereI1I12Many2One;
        public AssociationType I1WhereI1I12One2One;
        public AssociationType I1WhereI1I12One2Many;
        public AssociationType I12sWhereI12I12Many2Many;
        public AssociationType I12WhereI12I12One2Many;
        public AssociationType I12WhereI12I12One2One;
        public AssociationType I12sWhereDependency;
        public AssociationType I12sWhereI12I12Many2One;
        public AssociationType I2sWhereI2I12Many2One;
        public AssociationType I2WhereI2I12One2Many;
        public AssociationType I2WhereI2I12One2One;
        public AssociationType I2sWhereI2I12Many2Many;

		internal MetaI2(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0"))
			{
				SingularName = "I2",
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

		// Defined Roles

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaS1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("253b0d71-9eaa-4d87-9094-3b549d8446b3"))
			{
				SingularName = "S1",
			};
        }
	}

    public partial class MetaShared : MetaInterface
	{
	    public static MetaShared Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations
        public AssociationType TwosWhereShared;

		// Inherited Associations

		internal MetaShared(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("5c3876c3-c3be-46aa-a598-a68b964d329e"))
			{
				SingularName = "Shared",
			};
        }
	}

    public partial class MetaSyncDepth1 : MetaInterface
	{
	    public static MetaSyncDepth1 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType SyncDepth2;

		// Inherited Roles

		// Defined Associations
        public AssociationType SyncRootsWhereSyncDepth1;

		// Inherited Associations

		internal MetaSyncDepth1(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("FBC46730-B636-4333-989C-53D5F76A32A0"))
			{
				SingularName = "SyncDepth1",
			};
        }
	}

    public partial class MetaSyncDepth2 : MetaInterface
	{
	    public static MetaSyncDepth2 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles

		// Inherited Roles

		// Defined Associations
        public AssociationType SyncDepth1sWhereSyncDepth2;

		// Inherited Associations

		internal MetaSyncDepth2(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("B9996F8F-12FB-4E42-8B7F-907433A622B2"))
			{
				SingularName = "SyncDepth2",
			};
        }
	}

    public partial class MetaSyncRoot : MetaInterface
	{
	    public static MetaSyncRoot Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType SyncDepth1;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaSyncRoot(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("2A863DCF-C6FE-4838-8D3A-1212A2076D70"))
			{
				SingularName = "SyncRoot",
			};
        }
	}

    public partial class MetaValidationI12 : MetaInterface
	{
	    public static MetaValidationI12 Instance { get; internal set;}

		public override Interface Interface { get; }

		// Defined Methods

		// Inherited Methods

		// Defined Roles
        public RoleType UniqueId;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations

		internal MetaValidationI12(MetaPopulation metaPopulation)
        {
			this.Interface = new Interface(metaPopulation, new System.Guid("d61872ee-3778-47e8-8931-003f3f48cbc5"))
			{
				SingularName = "ValidationI12",
			};
        }
	}
}