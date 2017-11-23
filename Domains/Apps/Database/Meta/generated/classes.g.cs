namespace Allors.Meta
{
    public partial class MetaCounter : MetaClass
	{
	    public static MetaCounter Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Value;

		// Inherited Roles
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InternalOrganisationsWherePurchaseInvoiceCounter;
        public AssociationType InternalOrganisationsWherePurchaseOrderCounter;
        public AssociationType InternalOrganisationsWhereAccountingTransactionCounter;
        public AssociationType InternalOrganisationsWhereIncomingShipmentCounter;
        public AssociationType InternalOrganisationsWhereSubAccountCounter;
        public AssociationType InternalOrganisationsWhereQuoteCounter;
        public AssociationType InternalOrganisationsWhereRequestCounter;
        public AssociationType StoresWhereSalesOrderCounter;
        public AssociationType StoresWhereSalesInvoiceCounter;
        public AssociationType StoresWhereOutgoingShipmentCounter;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCounter(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0568354f-e3d9-439e-baac-b7dce31b956a"))
			{
				SingularName = "Counter",
				PluralName = "Counters",
			};
        }

	}

    public partial class MetaSingleton : MetaClass
	{
	    public static MetaSingleton Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DefaultLocale;
        public RoleType Locales;
        public RoleType Guest;
        public RoleType InitialSecurityToken;
        public RoleType DefaultSecurityToken;
        public RoleType CreatorsAccessControl;
        public RoleType GuestAccessControl;
        public RoleType AdministratorsAccessControl;
        public RoleType AdministratorSecurityToken;
        public RoleType PreviousCurrency;
        public RoleType PreferredCurrency;
        public RoleType NoImageAvailableImage;
        public RoleType InternalOrganisation;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations


		internal MetaSingleton(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("313b97a5-328c-4600-9dd2-b5bc146fb13b"))
			{
				SingularName = "Singleton",
				PluralName = "Singletons",
			};
        }

	}

    public partial class MetaMedia : MetaClass
	{
	    public static MetaMedia Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revision;
        public RoleType MediaContent;
        public RoleType InData;
        public RoleType InDataUri;
        public RoleType FileName;
        public RoleType Type;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType SingletonsWhereNoImageAvailableImage;
        public AssociationType PersonWherePicture;
        public AssociationType BanksWhereLogo;
        public AssociationType CataloguesWhereCatalogueImage;
        public AssociationType EventsWherePhoto;
        public AssociationType GoodWherePrimaryPhoto;
        public AssociationType GoodsWherePhoto;
        public AssociationType InternalOrganisationsWhereLogoImage;
        public AssociationType OrganisationVersionsWhereLogoImage;
        public AssociationType PersonVersionWherePicture;
        public AssociationType OrganisationsWhereLogoImage;
        public AssociationType ProductCategoriesWhereCategoryImage;
        public AssociationType StoresWhereLogoImage;
        public AssociationType CommunicationEventVersionsWhereDocument;
        public AssociationType CommunicationEventWhereDocument;
        public AssociationType PartyVersionWhereContent;
        public AssociationType PartyWhereContent;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaMedia(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da5b86a3-4f33-4c0d-965d-f4fbc1179374"))
			{
				SingularName = "Media",
				PluralName = "Medias",
			};
        }

	}

    public partial class MetaMediaContent : MetaClass
	{
	    public static MetaMediaContent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Type;
        public RoleType Data;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType MediaWhereMediaContent;

		// Inherited Associations


		internal MetaMediaContent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c20422e-cb3e-4402-bb40-dacaf584405e"))
			{
				SingularName = "MediaContent",
				PluralName = "MediaContents",
			};
        }

	}

    public partial class MetaCountry : MetaClass
	{
	    public static MetaCountry Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Currency;
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType LocalisedNames;
        public RoleType VatRates;
        public RoleType IbanLength;
        public RoleType EuMemberState;
        public RoleType TelephoneCode;
        public RoleType IbanRegex;
        public RoleType VatForm;
        public RoleType UriExtension;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Cities;

		// Defined Associations
        public AssociationType LocalesWhereCountry;
        public AssociationType BanksWhereCountry;
        public AssociationType CitizenshipsWhereCountry;
        public AssociationType InternalOrganisationsWhereEuListingState;
        public AssociationType IncoTermsWhereIncoTermCountry;
        public AssociationType PostalAddressesWhereCountry;
        public AssociationType PostalBoundariesWhereCountry;
        public AssociationType CountryBoundsWhereCountry;

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaCountry(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c22bf60e-6428-4d10-8194-94f7be396f28"))
			{
				SingularName = "Country",
				PluralName = "Countries",
			};
        }

	}

    public partial class MetaCurrency : MetaClass
	{
	    public static MetaCurrency Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType LocalisedNames;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Description;
        public ConcreteRoleType UnitOfMeasureConversions;
        public ConcreteRoleType Abbreviation;

		// Defined Associations
        public AssociationType SingletonsWherePreviousCurrency;
        public AssociationType SingletonsWherePreferredCurrency;
        public AssociationType CountriesWhereCurrency;
        public AssociationType AmountDuesWhereCurrency;
        public AssociationType BankAccountsWhereCurrency;
        public AssociationType InternalOrganisationRevenuesWhereCurrency;
        public AssociationType PackageRevenuesWhereCurrency;
        public AssociationType PartyPackageRevenuesWhereCurrency;
        public AssociationType PartyProductCategoryRevenuesWhereCurrency;
        public AssociationType PartyProductRevenuesWhereCurrency;
        public AssociationType PartyRevenuesWhereCurrency;
        public AssociationType ProductCategoryRevenuesWhereCurrency;
        public AssociationType ProductPurchasePricesWhereCurrency;
        public AssociationType ProductRevenuesWhereCurrency;
        public AssociationType SalesChannelRevenuesWhereCurrency;
        public AssociationType SalesRepCommissionsWhereCurrency;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereCurrency;
        public AssociationType SalesRepPartyRevenuesWhereCurrency;
        public AssociationType SalesRepProductCategoryRevenuesWhereCurrency;
        public AssociationType SalesRepRevenuesWhereCurrency;
        public AssociationType ShippingAndHandlingComponentsWhereCurrency;
        public AssociationType StoreRevenuesWhereCurrency;
        public AssociationType EstimatedProductCostsWhereCurrency;
        public AssociationType InvoiceVersionsWhereCustomerCurrency;
        public AssociationType InvoicesWhereCustomerCurrency;
        public AssociationType OrderVersionsWhereCustomerCurrency;
        public AssociationType PartyVersionsWherePreferredCurrency;
        public AssociationType QuoteVersionsWhereCurrency;
        public AssociationType RequestVersionsWhereCurrency;
        public AssociationType OrdersWhereCustomerCurrency;
        public AssociationType PartiesWherePreferredCurrency;
        public AssociationType PriceComponentsWhereCurrency;
        public AssociationType QuotesWhereCurrency;
        public AssociationType RequestsWhereCurrency;

		// Inherited Associations
        public AssociationType UnitOfMeasureConversionsWhereToUnitOfMeasure;
        public AssociationType NotificationsWhereTarget;


		internal MetaCurrency(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fd397adf-40b4-4ef8-b449-dd5a24273df3"))
			{
				SingularName = "Currency",
				PluralName = "Currencies",
			};
        }

	}

    public partial class MetaLanguage : MetaClass
	{
	    public static MetaLanguage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType IsoCode;
        public RoleType Name;
        public RoleType NativeName;
        public RoleType LocalisedNames;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType LocalesWhereLanguage;

		// Inherited Associations


		internal MetaLanguage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4a0eca4b-281f-488d-9c7e-497de882c044"))
			{
				SingularName = "Language",
				PluralName = "Languages",
			};
        }

	}

    public partial class MetaLocale : MetaClass
	{
	    public static MetaLocale Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType Language;
        public RoleType Country;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType SingletonsWhereDefaultLocale;
        public AssociationType SingletonWhereLocale;
        public AssociationType EventsWhereLocale;
        public AssociationType NewsItemsWhereLocale;
        public AssociationType LocalisedsWhereLocale;

		// Inherited Associations


		internal MetaLocale(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("45033ae6-85b5-4ced-87ce-02518e6c27fd"))
			{
				SingularName = "Locale",
				PluralName = "Locales",
			};
        }

	}

    public partial class MetaLocalisedText : MetaClass
	{
	    public static MetaLocalisedText Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Text;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Locale;

		// Defined Associations
        public AssociationType CountryWhereLocalisedName;
        public AssociationType CurrencyWhereLocalisedName;
        public AssociationType LanguageWhereLocalisedName;
        public AssociationType CatalogueWhereLocalisedName;
        public AssociationType CatalogueWhereLocalisedDescription;
        public AssociationType ProductCategoryWhereLocalisedName;
        public AssociationType ProductCategoryWhereLocalisedDescription;
        public AssociationType EnumerationWhereLocalisedName;
        public AssociationType ProductWhereLocalisedName;
        public AssociationType ProductWhereLocalisedDescription;
        public AssociationType ProductWhereLocalisedComment;

		// Inherited Associations


		internal MetaLocalisedText(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("020f5d4d-4a59-4d7b-865a-d72fc70e4d97"))
			{
				SingularName = "LocalisedText",
				PluralName = "LocalisedTexts",
			};
        }

	}

    public partial class MetaAccessControl : MetaClass
	{
	    public static MetaAccessControl Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType SubjectGroups;
        public RoleType Subjects;
        public RoleType Role;
        public RoleType EffectivePermissions;
        public RoleType EffectiveUsers;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType CacheId;

		// Defined Associations
        public AssociationType SingletonWhereCreatorsAccessControl;
        public AssociationType SingletonWhereGuestAccessControl;
        public AssociationType SingletonWhereAdministratorsAccessControl;
        public AssociationType SecurityTokensWhereAccessControl;
        public AssociationType OrganisationVersionWhereContactsAccessControl;
        public AssociationType OrganisationWhereContactsAccessControl;
        public AssociationType SecurityTokenOwnerWhereOwnerAccessControl;
        public AssociationType CommunicationEventVersionsWhereOwnerAccessControl;
        public AssociationType CommunicationEventWhereOwnerAccessControl;
        public AssociationType WorkEffortVersionsWhereOwnerAccessControl;
        public AssociationType WorkEffortWhereOwnerAccessControl;

		// Inherited Associations


		internal MetaAccessControl(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c4d93d5e-34c3-4731-9d37-47a8e801d9a8"))
			{
				SingularName = "AccessControl",
				PluralName = "AccessControls",
			};
        }

	}

    public partial class MetaLogin : MetaClass
	{
	    public static MetaLogin Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Key;
        public RoleType Provider;
        public RoleType User;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaLogin(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ad7277a8-eda4-4128-a990-b47fe43d120a"))
			{
				SingularName = "Login",
				PluralName = "Logins",
			};
        }

	}

    public partial class MetaPermission : MetaClass
	{
	    public static MetaPermission Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType OperandTypePointer;
        public RoleType ConcreteClassPointer;
        public RoleType OperationEnum;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AccessControlsWhereEffectivePermission;
        public AssociationType RolesWherePermission;
        public AssociationType AccessControlledObjectsWhereDeniedPermission;
        public AssociationType ObjectStatesWhereDeniedPermission;

		// Inherited Associations


		internal MetaPermission(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7fded183-3337-4196-afb0-3266377944bc"))
			{
				SingularName = "Permission",
				PluralName = "Permissions",
			};
        }

	}

    public partial class MetaRole : MetaClass
	{
	    public static MetaRole Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Permissions;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType AccessControlsWhereRole;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaRole(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("af6fe5f4-e5bc-4099-bcd1-97528af6505d"))
			{
				SingularName = "Role",
				PluralName = "Roles",
			};
        }

	}

    public partial class MetaSecurityToken : MetaClass
	{
	    public static MetaSecurityToken Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType AccessControls;

		// Inherited Roles

		// Defined Associations
        public AssociationType SingletonsWhereInitialSecurityToken;
        public AssociationType SingletonsWhereDefaultSecurityToken;
        public AssociationType SingletonsWhereAdministratorSecurityToken;
        public AssociationType OrganisationVersionWhereContactsSecurityToken;
        public AssociationType OrganisationWhereContactsSecurityToken;
        public AssociationType AccessControlledObjectsWhereSecurityToken;
        public AssociationType SecurityTokenOwnerWhereOwnerSecurityToken;
        public AssociationType CommunicationEventVersionsWhereOwnerSecurityToken;
        public AssociationType CommunicationEventWhereOwnerSecurityToken;
        public AssociationType WorkEffortVersionsWhereOwnerSecurityToken;
        public AssociationType WorkEffortWhereOwnerSecurityToken;

		// Inherited Associations


		internal MetaSecurityToken(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a53f1aed-0e3f-4c3c-9600-dc579cccf893"))
			{
				SingularName = "SecurityToken",
				PluralName = "SecurityTokens",
			};
        }

	}

    public partial class MetaAutomatedAgent : MetaClass
	{
	    public static MetaAutomatedAgent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType Description;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType UserEmailConfirmed;
        public ConcreteRoleType UserName;
        public ConcreteRoleType NormalizedUserName;
        public ConcreteRoleType UserEmail;
        public ConcreteRoleType UserPasswordHash;
        public ConcreteRoleType TaskList;
        public ConcreteRoleType NotificationList;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Locale;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Comment;
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;

		// Defined Associations

		// Inherited Associations
        public AssociationType SingletonWhereGuest;
        public AssociationType AccessControlsWhereSubject;
        public AssociationType AccessControlsWhereEffectiveUser;
        public AssociationType LoginsWhereUser;
        public AssociationType EmailMessagesWhereSender;
        public AssociationType EmailMessagesWhereRecipient;
        public AssociationType TaskAssignmentsWhereUser;
        public AssociationType UserGroupsWhereMember;
        public AssociationType AuditablesWhereCreatedBy;
        public AssociationType AuditablesWhereLastModifiedBy;
        public AssociationType CommunicationEventVersionsWhereCreatedBy;
        public AssociationType CommunicationEventVersionsWhereLastModifiedBy;
        public AssociationType InvoiceVersionsWhereCreatedBy;
        public AssociationType InvoiceVersionsWhereLastModifiedBy;
        public AssociationType OrderVersionsWhereCreatedBy;
        public AssociationType OrderVersionsWhereLastModifiedBy;
        public AssociationType PartyVersionsWhereCreatedBy;
        public AssociationType PartyVersionsWhereLastModifiedBy;
        public AssociationType CustomerRelationshipsWhereCustomer;
        public AssociationType EngagementsWhereBillToParty;
        public AssociationType EngagementsWherePlacingParty;
        public AssociationType FaceToFaceCommunicationsWhereParticipant;
        public AssociationType FaceToFaceCommunicationVersionsWhereParticipant;
        public AssociationType FaxCommunicationsWhereOriginator;
        public AssociationType FaxCommunicationsWhereReceiver;
        public AssociationType FaxCommunicationVersionsWhereOriginator;
        public AssociationType FaxCommunicationVersionsWhereReceiver;
        public AssociationType GoodsWhereManufacturedBy;
        public AssociationType GoodsWhereSuppliedBy;
        public AssociationType InternalOrganisationWhereCurrentCustomer;
        public AssociationType InternalOrganisationWhereActiveCustomer;
        public AssociationType InternalOrganisationWhereActiveSupplier;
        public AssociationType LetterCorrespondencesWhereOriginator;
        public AssociationType LetterCorrespondencesWhereReceiver;
        public AssociationType LetterCorrespondenceVersionsWhereOriginator;
        public AssociationType LetterCorrespondenceVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereCaller;
        public AssociationType PickListVersionsWhereShipToParty;
        public AssociationType PurchaseInvoiceVersionsWhereBilledFromParty;
        public AssociationType PurchaseOrderVersionsWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrderVersionsWhereTakenViaSupplier;
        public AssociationType QuoteItemVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereNeededFor;
        public AssociationType RequirementVersionsWhereOriginator;
        public AssociationType RequirementVersionsWhereServicedBy;
        public AssociationType SalesInvoiceVersionsWherePreviousBillToCustomer;
        public AssociationType SalesInvoiceVersionsWherePreviousShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereBillToCustomer;
        public AssociationType SalesInvoiceVersionsWhereShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereCustomer;
        public AssociationType SalesOrderItemVersionsWhereShipToParty;
        public AssociationType SalesOrderItemVersionsWhereAssignedShipToParty;
        public AssociationType SalesOrderVersionsWhereShipToCustomer;
        public AssociationType SalesOrderVersionsWhereBillToCustomer;
        public AssociationType SalesOrderVersionsWherePreviousShipToCustomer;
        public AssociationType SalesOrderVersionsWhereCustomer;
        public AssociationType SalesOrderVersionsWherePreviousBillToCustomer;
        public AssociationType SalesOrderVersionsWherePlacingCustomer;
        public AssociationType OrganisationGlAccountsWhereParty;
        public AssociationType PartyFixedAssetAssignmentsWhereParty;
        public AssociationType PartyPackageRevenuesWhereParty;
        public AssociationType PartyProductCategoryRevenuesWhereParty;
        public AssociationType PartyProductRevenuesWhereParty;
        public AssociationType PartyRevenuesWhereParty;
        public AssociationType PhoneCommunicationsWhereReceiver;
        public AssociationType PhoneCommunicationsWhereCaller;
        public AssociationType PickListsWhereShipToParty;
        public AssociationType PurchaseInvoicesWhereBilledFromParty;
        public AssociationType PurchaseOrdersWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrdersWhereTakenViaSupplier;
        public AssociationType QuoteItemsWhereAuthorizer;
        public AssociationType RequirementsWhereAuthorizer;
        public AssociationType RequirementsWhereNeededFor;
        public AssociationType RequirementsWhereOriginator;
        public AssociationType RequirementsWhereServicedBy;
        public AssociationType RespondingPartiesWhereParty;
        public AssociationType SalesInvoicesWherePreviousBillToCustomer;
        public AssociationType SalesInvoicesWherePreviousShipToCustomer;
        public AssociationType SalesInvoicesWhereBillToCustomer;
        public AssociationType SalesInvoicesWhereShipToCustomer;
        public AssociationType SalesInvoicesWhereCustomer;
        public AssociationType SalesOrdersWhereShipToCustomer;
        public AssociationType SalesOrdersWhereBillToCustomer;
        public AssociationType SalesOrdersWherePreviousShipToCustomer;
        public AssociationType SalesOrdersWhereCustomer;
        public AssociationType SalesOrdersWherePreviousBillToCustomer;
        public AssociationType SalesOrdersWherePlacingCustomer;
        public AssociationType SalesOrderItemsWhereShipToParty;
        public AssociationType SalesOrderItemsWhereAssignedShipToParty;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereParty;
        public AssociationType SalesRepPartyRevenuesWhereParty;
        public AssociationType SalesRepRelationshipsWhereCustomer;
        public AssociationType SubContractorRelationshipsWhereContractor;
        public AssociationType SubContractorRelationshipsWhereSubContractor;
        public AssociationType SupplierOfferingsWhereSupplier;
        public AssociationType WebSiteCommunicationVersionsWhereOriginator;
        public AssociationType WebSiteCommunicationVersionsWhereReceiver;
        public AssociationType WebSiteCommunicationsWhereOriginator;
        public AssociationType WebSiteCommunicationsWhereReceiver;
        public AssociationType WorkEffortPartyAssignmentsWhereParty;
        public AssociationType CommunicationEventVersionsWhereToParty;
        public AssociationType CommunicationEventVersionsWhereInvolvedParty;
        public AssociationType CommunicationEventVersionsWhereFromParty;
        public AssociationType CommunicationEventsWhereToParty;
        public AssociationType CommunicationEventsWhereInvolvedParty;
        public AssociationType CommunicationEventsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereToParty;
        public AssociationType PartyRelationshipsWhereParty;
        public AssociationType QuoteVersionsWhereReceiver;
        public AssociationType RequestVersionsWhereOriginator;
        public AssociationType PaymentsWhereSendingParty;
        public AssociationType PaymentsWhereReceivingParty;
        public AssociationType QuotesWhereReceiver;
        public AssociationType RequestsWhereOriginator;
        public AssociationType ShipmentVersionsWhereBillToParty;
        public AssociationType ShipmentVersionsWhereShipToParty;
        public AssociationType ShipmentVersionsWhereShipFromParty;
        public AssociationType ShipmentsWhereBillToParty;
        public AssociationType ShipmentsWhereShipToParty;
        public AssociationType ShipmentsWhereShipFromParty;
        public AssociationType NotificationsWhereTarget;


		internal MetaAutomatedAgent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3587d2e1-c3f6-4c55-a96c-016e0501d99c"))
			{
				SingularName = "AutomatedAgent",
				PluralName = "AutomatedAgents",
			};
        }

	}

    public partial class MetaEmailMessage : MetaClass
	{
	    public static MetaEmailMessage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DateCreated;
        public RoleType DateSending;
        public RoleType DateSent;
        public RoleType Sender;
        public RoleType Recipients;
        public RoleType RecipientEmailAddress;
        public RoleType Subject;
        public RoleType Body;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaEmailMessage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ab20998b-62b1-4064-a7b9-cc9416edf77a"))
			{
				SingularName = "EmailMessage",
				PluralName = "EmailMessages",
			};
        }

	}

    public partial class MetaNotification : MetaClass
	{
	    public static MetaNotification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Target;
        public RoleType Confirmed;
        public RoleType Title;
        public RoleType Description;
        public RoleType DateCreated;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType NotificationListWhereNotification;
        public AssociationType NotificationListWhereUnconfirmedNotification;
        public AssociationType NotificationListWhereConfirmedNotification;
        public AssociationType TaskAssignmentWhereNotification;

		// Inherited Associations


		internal MetaNotification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("73dcdc68-7571-4ed1-86db-77c914fe2f62"))
			{
				SingularName = "Notification",
				PluralName = "Notifications",
			};
        }

	}

    public partial class MetaNotificationList : MetaClass
	{
	    public static MetaNotificationList Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Notifications;
        public RoleType UnconfirmedNotifications;
        public RoleType ConfirmedNotifications;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType UserWhereNotificationList;

		// Inherited Associations


		internal MetaNotificationList(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b6579993-4ff1-4853-b048-1f8e67419c00"))
			{
				SingularName = "NotificationList",
				PluralName = "NotificationLists",
			};
        }

	}

    public partial class MetaPerson : MetaClass
	{
	    public static MetaPerson Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType FirstName;
        public RoleType LastName;
        public RoleType MiddleName;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType PersonRoles;
        public RoleType Salutation;
        public RoleType YTDCommission;
        public RoleType PersonClassifications;
        public RoleType Citizenship;
        public RoleType LastYearsCommission;
        public RoleType GivenName;
        public RoleType Titles;
        public RoleType MothersMaidenName;
        public RoleType BirthDate;
        public RoleType Height;
        public RoleType PersonTrainings;
        public RoleType Gender;
        public RoleType Weight;
        public RoleType Hobbies;
        public RoleType TotalYearsWorkExperience;
        public RoleType Passports;
        public RoleType MaritalStatus;
        public RoleType Picture;
        public RoleType SocialSecurityNumber;
        public RoleType DeceasedDate;
        public RoleType Function;

		// Inherited Roles
        public ConcreteRoleType UserEmailConfirmed;
        public ConcreteRoleType UserName;
        public ConcreteRoleType NormalizedUserName;
        public ConcreteRoleType UserEmail;
        public ConcreteRoleType UserPasswordHash;
        public ConcreteRoleType TaskList;
        public ConcreteRoleType NotificationList;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Locale;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType Comment;

		// Defined Associations
        public AssociationType BankAccountWhereContactPerson;
        public AssociationType CashesWherePersonResponsible;
        public AssociationType EmploymentsWhereEmployee;
        public AssociationType EmploymentApplicationsWherePerson;
        public AssociationType EngineeringChangesWhereRequestor;
        public AssociationType EngineeringChangesWhereAuthorizer;
        public AssociationType EngineeringChangesWhereDesigner;
        public AssociationType EngineeringChangesWhereTester;
        public AssociationType EventRegistrationsWherePerson;
        public AssociationType InternalOrganisationWhereCurrentSalesRep;
        public AssociationType InternalOrganisationWhereSalesRep;
        public AssociationType InternalOrganisationWhereActiveEmployee;
        public AssociationType PickListVersionsWherePicker;
        public AssociationType SalesInvoiceItemVersionsWhereSalesRep;
        public AssociationType SalesInvoiceVersionsWhereSalesRep;
        public AssociationType SalesOrderItemVersionsWhereSalesRep;
        public AssociationType SalesOrderVersionsWhereSalesRep;
        public AssociationType OrganisationContactRelationshipsWhereContact;
        public AssociationType OwnCreditCardsWhereOwner;
        public AssociationType PerformanceNotesWhereGivenByManager;
        public AssociationType PerformanceNotesWhereEmployee;
        public AssociationType PerformanceReviewsWhereManager;
        public AssociationType PerformanceReviewsWhereEmployee;
        public AssociationType PickListsWherePicker;
        public AssociationType PositionFulfillmentsWherePerson;
        public AssociationType ProfessionalAssignmentsWhereProfessional;
        public AssociationType ProfessionalServicesRelationshipsWhereProfessional;
        public AssociationType RequirementCommunicationsWhereAssociatedProfessional;
        public AssociationType SalesInvoicesWhereSalesRep;
        public AssociationType SalesInvoiceItemsWhereSalesRep;
        public AssociationType SalesOrdersWhereSalesRep;
        public AssociationType SalesOrderItemsWhereSalesRep;
        public AssociationType SalesRepCommissionsWhereSalesRep;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereSalesRep;
        public AssociationType SalesRepPartyRevenuesWhereSalesRep;
        public AssociationType SalesRepProductCategoryRevenuesWhereSalesRep;
        public AssociationType SalesRepRelationshipsWhereSalesRepresentative;
        public AssociationType SalesRepRevenuesWhereSalesRep;
        public AssociationType ServiceEntryHeadersWhereSubmittedBy;
        public AssociationType WorkEffortAssignmentsWhereProfessional;
        public AssociationType TasksWhereParticipant;
        public AssociationType TasksWherePerformer;
        public AssociationType CommunicationEventVersionsWhereOwner;
        public AssociationType CommunicationEventsWhereOwner;
        public AssociationType EngagementItemsWhereCurrentAssignedProfessional;
        public AssociationType InvoicesWhereContactPerson;
        public AssociationType PartyVersionsWhereInactiveContact;
        public AssociationType PartyVersionsWhereCurrentSalesRep;
        public AssociationType PartyVersionsWhereCurrentContact;
        public AssociationType OrdersWhereContactPerson;
        public AssociationType PartiesWhereInactiveContact;
        public AssociationType PartiesWhereCurrentSalesRep;
        public AssociationType PartiesWhereCurrentContact;
        public AssociationType QuotesWhereContactPerson;
        public AssociationType RequestsWhereContactPerson;
        public AssociationType WorkEffortsWhereOwner;

		// Inherited Associations
        public AssociationType SingletonWhereGuest;
        public AssociationType AccessControlsWhereSubject;
        public AssociationType AccessControlsWhereEffectiveUser;
        public AssociationType LoginsWhereUser;
        public AssociationType EmailMessagesWhereSender;
        public AssociationType EmailMessagesWhereRecipient;
        public AssociationType TaskAssignmentsWhereUser;
        public AssociationType UserGroupsWhereMember;
        public AssociationType AuditablesWhereCreatedBy;
        public AssociationType AuditablesWhereLastModifiedBy;
        public AssociationType CommunicationEventVersionsWhereCreatedBy;
        public AssociationType CommunicationEventVersionsWhereLastModifiedBy;
        public AssociationType InvoiceVersionsWhereCreatedBy;
        public AssociationType InvoiceVersionsWhereLastModifiedBy;
        public AssociationType OrderVersionsWhereCreatedBy;
        public AssociationType OrderVersionsWhereLastModifiedBy;
        public AssociationType PartyVersionsWhereCreatedBy;
        public AssociationType PartyVersionsWhereLastModifiedBy;
        public AssociationType CustomerRelationshipsWhereCustomer;
        public AssociationType EngagementsWhereBillToParty;
        public AssociationType EngagementsWherePlacingParty;
        public AssociationType FaceToFaceCommunicationsWhereParticipant;
        public AssociationType FaceToFaceCommunicationVersionsWhereParticipant;
        public AssociationType FaxCommunicationsWhereOriginator;
        public AssociationType FaxCommunicationsWhereReceiver;
        public AssociationType FaxCommunicationVersionsWhereOriginator;
        public AssociationType FaxCommunicationVersionsWhereReceiver;
        public AssociationType GoodsWhereManufacturedBy;
        public AssociationType GoodsWhereSuppliedBy;
        public AssociationType InternalOrganisationWhereCurrentCustomer;
        public AssociationType InternalOrganisationWhereActiveCustomer;
        public AssociationType InternalOrganisationWhereActiveSupplier;
        public AssociationType LetterCorrespondencesWhereOriginator;
        public AssociationType LetterCorrespondencesWhereReceiver;
        public AssociationType LetterCorrespondenceVersionsWhereOriginator;
        public AssociationType LetterCorrespondenceVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereCaller;
        public AssociationType PickListVersionsWhereShipToParty;
        public AssociationType PurchaseInvoiceVersionsWhereBilledFromParty;
        public AssociationType PurchaseOrderVersionsWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrderVersionsWhereTakenViaSupplier;
        public AssociationType QuoteItemVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereNeededFor;
        public AssociationType RequirementVersionsWhereOriginator;
        public AssociationType RequirementVersionsWhereServicedBy;
        public AssociationType SalesInvoiceVersionsWherePreviousBillToCustomer;
        public AssociationType SalesInvoiceVersionsWherePreviousShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereBillToCustomer;
        public AssociationType SalesInvoiceVersionsWhereShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereCustomer;
        public AssociationType SalesOrderItemVersionsWhereShipToParty;
        public AssociationType SalesOrderItemVersionsWhereAssignedShipToParty;
        public AssociationType SalesOrderVersionsWhereShipToCustomer;
        public AssociationType SalesOrderVersionsWhereBillToCustomer;
        public AssociationType SalesOrderVersionsWherePreviousShipToCustomer;
        public AssociationType SalesOrderVersionsWhereCustomer;
        public AssociationType SalesOrderVersionsWherePreviousBillToCustomer;
        public AssociationType SalesOrderVersionsWherePlacingCustomer;
        public AssociationType OrganisationGlAccountsWhereParty;
        public AssociationType PartyFixedAssetAssignmentsWhereParty;
        public AssociationType PartyPackageRevenuesWhereParty;
        public AssociationType PartyProductCategoryRevenuesWhereParty;
        public AssociationType PartyProductRevenuesWhereParty;
        public AssociationType PartyRevenuesWhereParty;
        public AssociationType PhoneCommunicationsWhereReceiver;
        public AssociationType PhoneCommunicationsWhereCaller;
        public AssociationType PickListsWhereShipToParty;
        public AssociationType PurchaseInvoicesWhereBilledFromParty;
        public AssociationType PurchaseOrdersWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrdersWhereTakenViaSupplier;
        public AssociationType QuoteItemsWhereAuthorizer;
        public AssociationType RequirementsWhereAuthorizer;
        public AssociationType RequirementsWhereNeededFor;
        public AssociationType RequirementsWhereOriginator;
        public AssociationType RequirementsWhereServicedBy;
        public AssociationType RespondingPartiesWhereParty;
        public AssociationType SalesInvoicesWherePreviousBillToCustomer;
        public AssociationType SalesInvoicesWherePreviousShipToCustomer;
        public AssociationType SalesInvoicesWhereBillToCustomer;
        public AssociationType SalesInvoicesWhereShipToCustomer;
        public AssociationType SalesInvoicesWhereCustomer;
        public AssociationType SalesOrdersWhereShipToCustomer;
        public AssociationType SalesOrdersWhereBillToCustomer;
        public AssociationType SalesOrdersWherePreviousShipToCustomer;
        public AssociationType SalesOrdersWhereCustomer;
        public AssociationType SalesOrdersWherePreviousBillToCustomer;
        public AssociationType SalesOrdersWherePlacingCustomer;
        public AssociationType SalesOrderItemsWhereShipToParty;
        public AssociationType SalesOrderItemsWhereAssignedShipToParty;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereParty;
        public AssociationType SalesRepPartyRevenuesWhereParty;
        public AssociationType SalesRepRelationshipsWhereCustomer;
        public AssociationType SubContractorRelationshipsWhereContractor;
        public AssociationType SubContractorRelationshipsWhereSubContractor;
        public AssociationType SupplierOfferingsWhereSupplier;
        public AssociationType WebSiteCommunicationVersionsWhereOriginator;
        public AssociationType WebSiteCommunicationVersionsWhereReceiver;
        public AssociationType WebSiteCommunicationsWhereOriginator;
        public AssociationType WebSiteCommunicationsWhereReceiver;
        public AssociationType WorkEffortPartyAssignmentsWhereParty;
        public AssociationType CommunicationEventVersionsWhereToParty;
        public AssociationType CommunicationEventVersionsWhereInvolvedParty;
        public AssociationType CommunicationEventVersionsWhereFromParty;
        public AssociationType CommunicationEventsWhereToParty;
        public AssociationType CommunicationEventsWhereInvolvedParty;
        public AssociationType CommunicationEventsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereToParty;
        public AssociationType PartyRelationshipsWhereParty;
        public AssociationType QuoteVersionsWhereReceiver;
        public AssociationType RequestVersionsWhereOriginator;
        public AssociationType PaymentsWhereSendingParty;
        public AssociationType PaymentsWhereReceivingParty;
        public AssociationType QuotesWhereReceiver;
        public AssociationType RequestsWhereOriginator;
        public AssociationType ShipmentVersionsWhereBillToParty;
        public AssociationType ShipmentVersionsWhereShipToParty;
        public AssociationType ShipmentVersionsWhereShipFromParty;
        public AssociationType ShipmentsWhereBillToParty;
        public AssociationType ShipmentsWhereShipToParty;
        public AssociationType ShipmentsWhereShipFromParty;
        public AssociationType NotificationsWhereTarget;


		internal MetaPerson(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c799ca62-a554-467d-9aa2-1663293bb37f"))
			{
				SingularName = "Person",
				PluralName = "People",
			};
        }

	}

    public partial class MetaTaskAssignment : MetaClass
	{
	    public static MetaTaskAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType User;
        public RoleType Notification;
        public RoleType Task;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType TaskListWhereTaskAssignment;
        public AssociationType TaskListWhereOpenTaskAssignment;

		// Inherited Associations


		internal MetaTaskAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4092d0b4-c6f4-4b81-b023-66be3f4c90bd"))
			{
				SingularName = "TaskAssignment",
				PluralName = "TaskAssignments",
			};
        }

	}

    public partial class MetaTaskList : MetaClass
	{
	    public static MetaTaskList Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType TaskAssignments;
        public RoleType OpenTaskAssignments;
        public RoleType Count;

		// Inherited Roles

		// Defined Associations
        public AssociationType UserWhereTaskList;

		// Inherited Associations


		internal MetaTaskList(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1c2303a1-f3ce-4084-a1ad-fc25156ac542"))
			{
				SingularName = "TaskList",
				PluralName = "TaskLists",
			};
        }

	}

    public partial class MetaUserGroup : MetaClass
	{
	    public static MetaUserGroup Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Members;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AccessControlsWhereSubjectGroup;
        public AssociationType OrganisationVersionWhereOwnerUserGroup;
        public AssociationType OrganisationVersionWhereContactsUserGroup;
        public AssociationType OrganisationWhereOwnerUserGroup;
        public AssociationType OrganisationWhereContactsUserGroup;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaUserGroup(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("60065f5d-a3c2-4418-880d-1026ab607319"))
			{
				SingularName = "UserGroup",
				PluralName = "UserGroups",
			};
        }

	}

    public partial class MetaAccountAdjustment : MetaClass
	{
	    public static MetaAccountAdjustment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType FinancialAccountWhereFinancialAccountTransaction;


		internal MetaAccountAdjustment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4211ece6-a127-4359-9fa4-6537943a37a5"))
			{
				SingularName = "AccountAdjustment",
				PluralName = "AccountAdjustments",
			};
        }

	}

    public partial class MetaAccountingPeriod : MetaClass
	{
	    public static MetaAccountingPeriod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Close;
        public MethodType Reopen;

		// Defined Roles
        public RoleType Parent;
        public RoleType Active;
        public RoleType PeriodNumber;
        public RoleType TimeFrequency;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType Description;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;
        public ConcreteRoleType PreviousBudgetState;
        public ConcreteRoleType LastBudgetState;
        public ConcreteRoleType BudgetState;

		// Defined Associations
        public AssociationType AccountingPeriodsWhereParent;
        public AssociationType AccountingPeriodVersionWhereParent;
        public AssociationType InternalOrganisationWhereActualAccountingPeriod;
        public AssociationType OrganisationGlAccountBalancesWhereAccountingPeriod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaAccountingPeriod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6b56e13b-d075-40f1-8e33-a9a4c6cadb96"))
			{
				SingularName = "AccountingPeriod",
				PluralName = "AccountingPeriods",
			};
        }

	}

    public partial class MetaAccountingPeriodVersion : MetaClass
	{
	    public static MetaAccountingPeriodVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Parent;
        public RoleType Active;
        public RoleType PeriodNumber;
        public RoleType TimeFrequency;

		// Inherited Roles
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType BudgetState;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType Description;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;

		// Defined Associations
        public AssociationType AccountingPeriodWhereCurrentVersion;
        public AssociationType AccountingPeriodWhereAllVersion;

		// Inherited Associations


		internal MetaAccountingPeriodVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3A39A249-7AF8-413D-8EC1-EE395A216A29"))
			{
				SingularName = "AccountingPeriodVersion",
				PluralName = "AccountingPeriodVersions",
			};
        }

	}

    public partial class MetaAccountingTransactionDetail : MetaClass
	{
	    public static MetaAccountingTransactionDetail Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType AssociatedWith;
        public RoleType OrganisationGlAccountBalance;
        public RoleType Amount;
        public RoleType Debit;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AccountingTransactionDetailsWhereAssociatedWith;
        public AssociationType AccountingTransactionWhereAccountingTransactionDetail;

		// Inherited Associations


		internal MetaAccountingTransactionDetail(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e41be1b2-715b-4bc0-b095-ac23d9950ee4"))
			{
				SingularName = "AccountingTransactionDetail",
				PluralName = "AccountingTransactionDetails",
			};
        }

	}

    public partial class MetaAccountingTransactionNumber : MetaClass
	{
	    public static MetaAccountingTransactionNumber Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Number;
        public RoleType Year;
        public RoleType AccountingTransactionType;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InternalOrganisationWhereAccountingTransactionNumber;
        public AssociationType AccountingTransactionWhereAccountingTransactionNumber;

		// Inherited Associations


		internal MetaAccountingTransactionNumber(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0b9034b1-288a-48a7-9d46-3ca6dcb7ca3f"))
			{
				SingularName = "AccountingTransactionNumber",
				PluralName = "AccountingTransactionNumbers",
			};
        }

	}

    public partial class MetaAccountingTransactionType : MetaClass
	{
	    public static MetaAccountingTransactionType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType AccountingTransactionNumbersWhereAccountingTransactionType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaAccountingTransactionType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3277910f-c4ee-40b6-8028-21f879e8da04"))
			{
				SingularName = "AccountingTransactionType",
				PluralName = "AccountingTransactionTypes",
			};
        }

	}

    public partial class MetaActivityUsage : MetaClass
	{
	    public static MetaActivityUsage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Quantity;
        public RoleType UnitOfMeasure;

		// Inherited Roles
        public ConcreteRoleType TimeFrequency;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DeploymentsWhereDeploymentUsage;


		internal MetaActivityUsage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ded168ad-b674-47ab-855c-46b3e1939e32"))
			{
				SingularName = "ActivityUsage",
				PluralName = "ActivityUsages",
			};
        }

	}

    public partial class MetaAddendum : MetaClass
	{
	    public static MetaAddendum Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Text;
        public RoleType EffictiveDate;
        public RoleType Description;
        public RoleType CreationDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AgreementWhereAddenda;
        public AssociationType AgreementItemWhereAddenda;

		// Inherited Associations


		internal MetaAddendum(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7baa7594-6890-4e1e-8c06-fc49b3ea262d"))
			{
				SingularName = "Addendum",
				PluralName = "Addendums",
			};
        }

	}

    public partial class MetaAgreementExhibit : MetaClass
	{
	    public static MetaAgreementExhibit Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Text;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Children;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementItem;
        public AssociationType AgreementItemWhereChild;


		internal MetaAgreementExhibit(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2830c388-b002-44d6-91b6-b2b43fa778f3"))
			{
				SingularName = "AgreementExhibit",
				PluralName = "AgreementExhibits",
			};
        }

	}

    public partial class MetaAgreementPricingProgram : MetaClass
	{
	    public static MetaAgreementPricingProgram Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Text;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Children;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWhereAgreementPricingProgram;

		// Inherited Associations
        public AssociationType AgreementWhereAgreementItem;
        public AssociationType AgreementItemWhereChild;


		internal MetaAgreementPricingProgram(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("72237d95-e9c0-42c1-afe3-ec34f2e6cbfb"))
			{
				SingularName = "AgreementPricingProgram",
				PluralName = "AgreementPricingPrograms",
			};
        }

	}

    public partial class MetaAgreementSection : MetaClass
	{
	    public static MetaAgreementSection Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Text;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Children;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementItem;
        public AssociationType AgreementItemWhereChild;


		internal MetaAgreementSection(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e31d6dd2-b5b2-4fd8-949f-0df688ed2e9b"))
			{
				SingularName = "AgreementSection",
				PluralName = "AgreementSections",
			};
        }

	}

    public partial class MetaAmortization : MetaClass
	{
	    public static MetaAmortization Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaAmortization(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7fd1760c-ee1f-4d04-8a93-dfebc82757c1"))
			{
				SingularName = "Amortization",
				PluralName = "Amortizations",
			};
        }

	}

    public partial class MetaAmountDue : MetaClass
	{
	    public static MetaAmountDue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Amount;
        public RoleType PaymentMethod;
        public RoleType TransactionDate;
        public RoleType BlockedForDunning;
        public RoleType AmountVat;
        public RoleType BankAccount;
        public RoleType ReconciliationDate;
        public RoleType InvoiceNumber;
        public RoleType DunningStep;
        public RoleType SubAccountNumber;
        public RoleType TransactionNumber;
        public RoleType Side;
        public RoleType Currency;
        public RoleType BlockedForPayment;
        public RoleType DateLastReminder;
        public RoleType YourReference;
        public RoleType OurReference;
        public RoleType ReconciliationNumber;
        public RoleType DueDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaAmountDue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("848053ee-18d8-4962-81c3-bd6c7837565a"))
			{
				SingularName = "AmountDue",
				PluralName = "AmountDues",
			};
        }

	}

    public partial class MetaAssetAssignmentStatus : MetaClass
	{
	    public static MetaAssetAssignmentStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartyFixedAssetAssignmentsWhereAssetAssignmentStatus;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereAssetAssignmentStatus;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaAssetAssignmentStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("644660d4-d5d0-4bd9-8cba-17696af0b9ed"))
			{
				SingularName = "AssetAssignmentStatus",
				PluralName = "AssetAssignmentStatuses",
			};
        }

	}

    public partial class MetaAutomatedAgentVersion : MetaClass
	{
	    public static MetaAutomatedAgentVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;

		// Defined Associations
        public AssociationType AutomatedAgentWhereCurrentVersion;
        public AssociationType AutomatedAgentWhereAllVersion;

		// Inherited Associations


		internal MetaAutomatedAgentVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("464F8908-CB15-4B5E-AFF7-227D70C17BD2"))
			{
				SingularName = "AutomatedAgentVersion",
				PluralName = "AutomatedAgentVersions",
			};
        }

	}

    public partial class MetaBank : MetaClass
	{
	    public static MetaBank Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Logo;
        public RoleType Bic;
        public RoleType SwiftCode;
        public RoleType Country;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType BankAccountsWhereBank;

		// Inherited Associations


		internal MetaBank(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a24a8e12-7067-4bfb-8fc0-225a824d1a05"))
			{
				SingularName = "Bank",
				PluralName = "Banks",
			};
        }

	}

    public partial class MetaBankAccount : MetaClass
	{
	    public static MetaBankAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Bank;
        public RoleType NameOnAccount;
        public RoleType ContactMechanisms;
        public RoleType Currency;
        public RoleType Iban;
        public RoleType Branch;
        public RoleType ContactPersons;

		// Inherited Roles
        public ConcreteRoleType FinancialAccountTransactions;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AmountDuesWhereBankAccount;
        public AssociationType InternalOrganisationWhereBankAccount;
        public AssociationType OwnBankAccountsWhereBankAccount;
        public AssociationType PartyVersionWhereBankAccount;
        public AssociationType PartyWhereBankAccount;

		// Inherited Associations


		internal MetaBankAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("22bc5b67-8015-49c5-bc47-6f9e7e678943"))
			{
				SingularName = "BankAccount",
				PluralName = "BankAccounts",
			};
        }

	}

    public partial class MetaBasePrice : MetaClass
	{
	    public static MetaBasePrice Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaBasePrice(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("11c608b0-4755-4e74-b720-4eb94e83c24d"))
			{
				SingularName = "BasePrice",
				PluralName = "BasePrices",
			};
        }

	}

    public partial class MetaBenefit : MetaClass
	{
	    public static MetaBenefit Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType EmployerPaidPercentage;
        public RoleType Description;
        public RoleType Name;
        public RoleType AvailableTime;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PartyBenefitsWhereBenefit;

		// Inherited Associations


		internal MetaBenefit(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8cea6932-d589-4b5b-99b8-ffba33936f8f"))
			{
				SingularName = "Benefit",
				PluralName = "Benefits",
			};
        }

	}

    public partial class MetaBillingAccount : MetaClass
	{
	    public static MetaBillingAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType ContactMechanism;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PaymentApplicationsWhereBillingAccount;
        public AssociationType InvoiceVersionsWhereBillingAccount;
        public AssociationType InvoicesWhereBillingAccount;
        public AssociationType PartyVersionWhereBillingAccount;
        public AssociationType PartyWhereBillingAccount;

		// Inherited Associations


		internal MetaBillingAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("34d08c66-6d7a-4089-b862-c93feda67ef1"))
			{
				SingularName = "BillingAccount",
				PluralName = "BillingAccounts",
			};
        }

	}

    public partial class MetaBillOfLading : MetaClass
	{
	    public static MetaBillOfLading Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaBillOfLading(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5c5c17d1-2132-403b-8819-e3c1aa7bd6a9"))
			{
				SingularName = "BillOfLading",
				PluralName = "BillOfLadings",
			};
        }

	}

    public partial class MetaBrand : MetaClass
	{
	    public static MetaBrand Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType ProductCategories;
        public RoleType Models;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaBrand(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0a7ac589-946b-4d49-b7e0-7e0b9bc90111"))
			{
				SingularName = "Brand",
				PluralName = "Brands",
			};
        }

	}

    public partial class MetaBudgetItem : MetaClass
	{
	    public static MetaBudgetItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Purpose;
        public RoleType Justification;
        public RoleType Children;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType BudgetItemWhereChild;
        public AssociationType BudgetRevisionImpactsWhereBudgetItem;
        public AssociationType GlBudgetAllocationsWhereBudgetItem;
        public AssociationType PaymentBudgetAllocationsWhereBudgetItem;
        public AssociationType PositionsWhereApprovedBudgetItem;
        public AssociationType RequirementBudgetAllocationsWhereBudgetItem;
        public AssociationType BudgetWhereBudgetItem;
        public AssociationType BudgetVersionWhereBudgetItem;
        public AssociationType OrderItemVersionsWhereBudgetItem;
        public AssociationType OrderItemsWhereBudgetItem;

		// Inherited Associations


		internal MetaBudgetItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b397c075-215a-4d5b-b962-ea48540a64fa"))
			{
				SingularName = "BudgetItem",
				PluralName = "BudgetItems",
			};
        }

	}

    public partial class MetaBudgetState : MetaClass
	{
	    public static MetaBudgetState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType BudgetsWherePreviousBudgetState;
        public AssociationType BudgetsWhereLastBudgetState;
        public AssociationType BudgetsWhereBudgetState;
        public AssociationType BudgetVersionsWhereBudgetState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaBudgetState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f8ae512e-bca5-498b-860b-11c06ab04d72"))
			{
				SingularName = "BudgetState",
				PluralName = "BudgetStates",
			};
        }

	}

    public partial class MetaBudgetReview : MetaClass
	{
	    public static MetaBudgetReview Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ReviewDate;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType BudgetWhereBudgetReview;
        public AssociationType BudgetVersionWhereBudgetReview;

		// Inherited Associations


		internal MetaBudgetReview(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d12719f0-2c0e-4a9d-869b-4a209fc35a56"))
			{
				SingularName = "BudgetReview",
				PluralName = "BudgetReviews",
			};
        }

	}

    public partial class MetaBudgetRevision : MetaClass
	{
	    public static MetaBudgetRevision Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RevisionDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType BudgetRevisionImpactsWhereBudgetRevision;
        public AssociationType BudgetWhereBudgetRevision;
        public AssociationType BudgetVersionWhereBudgetRevision;

		// Inherited Associations


		internal MetaBudgetRevision(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9b6bf786-1c6c-4c4e-b940-7314d9c4ba71"))
			{
				SingularName = "BudgetRevision",
				PluralName = "BudgetRevisions",
			};
        }

	}

    public partial class MetaBudgetRevisionImpact : MetaClass
	{
	    public static MetaBudgetRevisionImpact Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType BudgetItem;
        public RoleType Reason;
        public RoleType Deleted;
        public RoleType Added;
        public RoleType RevisedAmount;
        public RoleType BudgetRevision;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaBudgetRevisionImpact(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ebae3ca2-5dca-486d-bbc0-30550313f153"))
			{
				SingularName = "BudgetRevisionImpact",
				PluralName = "BudgetRevisionImpacts",
			};
        }

	}

    public partial class MetaCapitalBudget : MetaClass
	{
	    public static MetaCapitalBudget Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Close;
        public MethodType Reopen;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;
        public ConcreteRoleType PreviousBudgetState;
        public ConcreteRoleType LastBudgetState;
        public ConcreteRoleType BudgetState;
        public ConcreteRoleType Description;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCapitalBudget(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("41f1aa5a-5043-42bb-aaf5-7d57a9deaccb"))
			{
				SingularName = "CapitalBudget",
				PluralName = "CapitalBudgets",
			};
        }

	}

    public partial class MetaCapitalBudgetVersion : MetaClass
	{
	    public static MetaCapitalBudgetVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType BudgetState;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType Description;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;

		// Defined Associations

		// Inherited Associations


		internal MetaCapitalBudgetVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1BBF6140-F74B-4503-8C9E-8C71F3F670C7"))
			{
				SingularName = "CapitalBudgetVersion",
				PluralName = "CapitalBudgetVersions",
			};
        }

	}

    public partial class MetaCapitalization : MetaClass
	{
	    public static MetaCapitalization Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaCapitalization(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a0a753be-15ca-49e2-8f5f-f956fa132f49"))
			{
				SingularName = "Capitalization",
				PluralName = "Capitalizations",
			};
        }

	}

    public partial class MetaCarrier : MetaClass
	{
	    public static MetaCarrier Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ShippingAndHandlingComponentsWhereCarrier;
        public AssociationType StoresWhereDefaultCarrier;
        public AssociationType ShipmentVersionsWhereCarrier;
        public AssociationType ShipmentsWhereCarrier;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCarrier(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4f46f32a-04e6-4ccc-829b-68fb3336f870"))
			{
				SingularName = "Carrier",
				PluralName = "Carriers",
			};
        }

	}

    public partial class MetaCaseVersion : MetaClass
	{
	    public static MetaCaseVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CaseState;
        public RoleType StartDate;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType CaseWhereCurrentVersion;
        public AssociationType CaseWhereAllVersion;

		// Inherited Associations


		internal MetaCaseVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B15B38A6-0A6C-4AB3-81CA-AF44647F90C1"))
			{
				SingularName = "CaseVersion",
				PluralName = "CaseVersions",
			};
        }

	}

    public partial class MetaCase : MetaClass
	{
	    public static MetaCase Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousCaseState;
        public RoleType LastCaseState;
        public RoleType CaseState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType StartDate;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;

		// Defined Associations
        public AssociationType CommunicationEventVersionsWhereCase;
        public AssociationType CommunicationEventsWhereCase;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCase(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a0705b81-2eef-4c51-9454-a31bcedc20a3"))
			{
				SingularName = "Case",
				PluralName = "Cases",
			};
        }

	}

    public partial class MetaCaseState : MetaClass
	{
	    public static MetaCaseState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CaseVersionsWhereCaseState;
        public AssociationType CasesWherePreviousCaseState;
        public AssociationType CasesWhereLastCaseState;
        public AssociationType CasesWhereCaseState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaCaseState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6ea1f500-13a2-4f5a-8026-a1d5a57170ac"))
			{
				SingularName = "CaseState",
				PluralName = "CaseStates",
			};
        }

	}

    public partial class MetaCash : MetaClass
	{
	    public static MetaCash Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PersonResponsible;

		// Inherited Roles
        public ConcreteRoleType BalanceLimit;
        public ConcreteRoleType CurrentBalance;
        public ConcreteRoleType Journal;
        public ConcreteRoleType Description;
        public ConcreteRoleType GlPaymentInTransit;
        public ConcreteRoleType Remarks;
        public ConcreteRoleType GeneralLedgerAccount;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType AmountDuesWherePaymentMethod;
        public AssociationType CustomerShipmentsWherePaymentMethod;
        public AssociationType CustomerShipmentVersionsWherePaymentMethod;
        public AssociationType InternalOrganisationWhereActivePaymentMethod;
        public AssociationType InternalOrganisationWhereDefaultPaymentMethod;
        public AssociationType SalesInvoiceVersionsWherePaymentMethod;
        public AssociationType SalesOrderVersionsWherePaymentMethod;
        public AssociationType PayrollPreferencesWherePaymentMethod;
        public AssociationType SalesInvoicesWherePaymentMethod;
        public AssociationType SalesOrdersWherePaymentMethod;
        public AssociationType StoresWhereDefaultPaymentMethod;
        public AssociationType StoresWherePaymentMethod;
        public AssociationType PartyVersionsWhereDefaultPaymentMethod;
        public AssociationType PartiesWhereDefaultPaymentMethod;
        public AssociationType PaymentsWherePaymentMethod;
        public AssociationType NotificationsWhereTarget;


		internal MetaCash(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("87fbf592-45a1-4ef2-85ca-f47d4c51abca"))
			{
				SingularName = "Cash",
				PluralName = "Cashes",
			};
        }

	}

    public partial class MetaCatalogue : MetaClass
	{
	    public static MetaCatalogue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType Description;
        public RoleType LocalisedNames;
        public RoleType LocalisedDescriptions;
        public RoleType CatalogueImage;
        public RoleType ProductCategories;
        public RoleType CatScope;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType StoreWhereCatalogue;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCatalogue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("974DCB55-4D12-460F-A45D-9EBCCA54DA0B"))
			{
				SingularName = "Catalogue",
				PluralName = "Catalogues",
			};
        }

	}

    public partial class MetaCatScope : MetaClass
	{
	    public static MetaCatScope Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CataloguesWhereCatScope;
        public AssociationType ProductCategoriesWhereCatScope;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCatScope(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("36ED4300-C413-43C9-BCEC-303678B2AA2C"))
			{
				SingularName = "CatScope",
				PluralName = "CatScopes",
			};
        }

	}

    public partial class MetaChartOfAccounts : MetaClass
	{
	    public static MetaChartOfAccounts Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType GeneralLedgerAccounts;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaChartOfAccounts(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6cf4845d-65a0-4957-95e9-f2b5327d6515"))
			{
				SingularName = "ChartOfAccounts",
				PluralName = "ChartsOfAccounts",
			};
        }

	}

    public partial class MetaCitizenship : MetaClass
	{
	    public static MetaCitizenship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Passports;
        public RoleType Country;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PersonWhereCitizenship;
        public AssociationType PersonVersionWhereCitizenship;

		// Inherited Associations


		internal MetaCitizenship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("38b0ac1b-497c-4286-976e-64b3d523ad9d"))
			{
				SingularName = "Citizenship",
				PluralName = "Citizenships",
			};
        }

	}

    public partial class MetaCity : MetaClass
	{
	    public static MetaCity Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType State;

		// Inherited Roles
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Country;

		// Defined Associations
        public AssociationType PostalAddressesWhereCity;
        public AssociationType CityBoundWhereCity;

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaCity(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f6dceab0-f4a7-435e-abce-ac9f7bd28ae4"))
			{
				SingularName = "City",
				PluralName = "Cities",
			};
        }

	}

    public partial class MetaClientAgreement : MetaClass
	{
	    public static MetaClientAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AgreementDate;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType Text;
        public ConcreteRoleType AgreementItems;
        public ConcreteRoleType AgreementNumber;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;
        public AssociationType NotificationsWhereTarget;


		internal MetaClientAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d726e301-4e4a-4ccb-9a6e-bc6fc4a327ab"))
			{
				SingularName = "ClientAgreement",
				PluralName = "ClientAgreements",
			};
        }

	}

    public partial class MetaColour : MetaClass
	{
	    public static MetaColour Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaColour(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8bae9154-ec37-4139-b52c-6c3df860fb20"))
			{
				SingularName = "Colour",
				PluralName = "Colours",
			};
        }

	}

    public partial class MetaCommunicationEventState : MetaClass
	{
	    public static MetaCommunicationEventState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CommunicationEventVersionsWhereCommunicationEventState;
        public AssociationType CommunicationEventsWherePreviousCommunicationEventState;
        public AssociationType CommunicationEventsWhereLastCommunicationEventState;
        public AssociationType CommunicationEventsWhereCommunicationEventState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaCommunicationEventState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f6ad2546-e977-4176-b03d-d30fb101270c"))
			{
				SingularName = "CommunicationEventState",
				PluralName = "CommunicationEventStates",
			};
        }

	}

    public partial class MetaCommunicationEventPurpose : MetaClass
	{
	    public static MetaCommunicationEventPurpose Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CommunicationEventVersionsWhereEventPurpose;
        public AssociationType CommunicationEventsWhereEventPurpose;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCommunicationEventPurpose(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8e3fd781-f0b5-4e02-b1f6-6364d0559273"))
			{
				SingularName = "CommunicationEventPurpose",
				PluralName = "CommunicationEventPurposes",
			};
        }

	}

    public partial class MetaContactMechanismPurpose : MetaClass
	{
	    public static MetaContactMechanismPurpose Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartyContactMechanismsWhereContactPurpose;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaContactMechanismPurpose(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0c6880e7-b41c-47a6-ab40-83e391c7a025"))
			{
				SingularName = "ContactMechanismPurpose",
				PluralName = "ContactMechanismPurposes",
			};
        }

	}

    public partial class MetaContactMechanismType : MetaClass
	{
	    public static MetaContactMechanismType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType ContactMechanismsWhereContactMechanismType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaContactMechanismType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8FCE70A7-0F94-4A28-99F6-52F9EB230EAA"))
			{
				SingularName = "ContactMechanismType",
				PluralName = "ContactMechanismTypes",
			};
        }

	}

    public partial class MetaCostCenter : MetaClass
	{
	    public static MetaCostCenter Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType InternalTransferGlAccount;
        public RoleType CostCenterCategories;
        public RoleType RedistributedCostsGlAccount;
        public RoleType Name;
        public RoleType Active;
        public RoleType UseGlAccountOfBooking;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType GeneralLedgerAccountsWhereDefaultCostCenter;
        public AssociationType GeneralLedgerAccountsWhereCostCentersAllowed;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCostCenter(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2ab70094-5481-4ecc-ae15-cb2131fbc2f1"))
			{
				SingularName = "CostCenter",
				PluralName = "CostCenters",
			};
        }

	}

    public partial class MetaCostCenterCategory : MetaClass
	{
	    public static MetaCostCenterCategory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Parent;
        public RoleType Ancestors;
        public RoleType Children;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CostCentersWhereCostCenterCategory;
        public AssociationType CostCenterCategoriesWhereParent;
        public AssociationType CostCenterCategoriesWhereAncestor;
        public AssociationType CostCenterCategoriesWhereChild;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCostCenterCategory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("11214660-3c3a-42e9-8f12-f475d823da64"))
			{
				SingularName = "CostCenterCategory",
				PluralName = "CostCenterCategories",
			};
        }

	}

    public partial class MetaCostCenterSplitMethod : MetaClass
	{
	    public static MetaCostCenterSplitMethod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InternalOrganisationsWhereCostCenterSplitMethod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCostCenterSplitMethod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("cabc3b20-0456-47d9-a030-6df1d1f8ea9e"))
			{
				SingularName = "CostCenterSplitMethod",
				PluralName = "CostCenterSplitMethods",
			};
        }

	}

    public partial class MetaCostOfGoodsSoldMethod : MetaClass
	{
	    public static MetaCostOfGoodsSoldMethod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InternalOrganisationsWhereCostOfGoodsSoldMethod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaCostOfGoodsSoldMethod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("52ee223f-14e7-46e7-8e24-c6fdf19fa5d1"))
			{
				SingularName = "CostOfGoodsSoldMethod",
				PluralName = "CostOfGoodsSoldMethods",
			};
        }

	}

    public partial class MetaCounty : MetaClass
	{
	    public static MetaCounty Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType State;

		// Inherited Roles
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Cities;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaCounty(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e6f97f86-6aec-4dde-b828-4de04d42c248"))
			{
				SingularName = "County",
				PluralName = "Counties",
			};
        }

	}

    public partial class MetaCreditCard : MetaClass
	{
	    public static MetaCreditCard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType NameOnCard;
        public RoleType CreditCardCompany;
        public RoleType ExpirationYear;
        public RoleType ExpirationMonth;
        public RoleType CardNumber;

		// Inherited Roles
        public ConcreteRoleType FinancialAccountTransactions;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType OwnCreditCardsWhereCreditCard;
        public AssociationType PartyVersionWhereCreditCard;
        public AssociationType PartyWhereCreditCard;

		// Inherited Associations


		internal MetaCreditCard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9492bd39-0f07-4978-a987-0393ca34b504"))
			{
				SingularName = "CreditCard",
				PluralName = "CreditCards",
			};
        }

	}

    public partial class MetaCreditCardCompany : MetaClass
	{
	    public static MetaCreditCardCompany Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType CreditCardsWhereCreditCardCompany;

		// Inherited Associations


		internal MetaCreditCardCompany(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("86d934de-a5cf-46d3-aad3-2626c43ebc85"))
			{
				SingularName = "CreditCardCompany",
				PluralName = "CreditCardCompanies",
			};
        }

	}

    public partial class MetaCreditLine : MetaClass
	{
	    public static MetaCreditLine Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaCreditLine(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5bdc88b6-c45f-4835-aa50-26405f1314e3"))
			{
				SingularName = "CreditLine",
				PluralName = "CreditLines",
			};
        }

	}

    public partial class MetaCustomEngagementItem : MetaClass
	{
	    public static MetaCustomEngagementItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DescriptionOfWork;
        public RoleType AgreedUponPrice;

		// Inherited Roles
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType Description;
        public ConcreteRoleType ExpectedStartDate;
        public ConcreteRoleType ExpectedEndDate;
        public ConcreteRoleType EngagementWorkFulfillment;
        public ConcreteRoleType EngagementRates;
        public ConcreteRoleType CurrentEngagementRate;
        public ConcreteRoleType OrderedWiths;
        public ConcreteRoleType CurrentAssignedProfessional;
        public ConcreteRoleType Product;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;


		internal MetaCustomEngagementItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("78022da7-d11c-4ab7-96f5-099d6608c4bb"))
			{
				SingularName = "CustomEngagementItem",
				PluralName = "CustomEngagementItems",
			};
        }

	}

    public partial class MetaCustomerRelationship : MetaClass
	{
	    public static MetaCustomerRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Customer;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaCustomerRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b9f21f4-2f2c-47a9-9c76-15f5ef4f5e00"))
			{
				SingularName = "CustomerRelationship",
				PluralName = "CustomerRelationships",
			};
        }

	}

    public partial class MetaCustomerReturn : MetaClass
	{
	    public static MetaCustomerReturn Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousCustomerReturnState;
        public RoleType LastCustomerReturnState;
        public RoleType CustomerReturnState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaCustomerReturn(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7dd7114a-9e74-45d5-b904-415514af5628"))
			{
				SingularName = "CustomerReturn",
				PluralName = "CustomerReturns",
			};
        }

	}

    public partial class MetaCustomerReturnState : MetaClass
	{
	    public static MetaCustomerReturnState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CustomerReturnsWherePreviousCustomerReturnState;
        public AssociationType CustomerReturnsWhereLastCustomerReturnState;
        public AssociationType CustomerReturnsWhereCustomerReturnState;
        public AssociationType CustomerReturnVersionsWhereCustomerReturnState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaCustomerReturnState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("671951f1-78fd-4b05-ac15-eafb2a35a6f8"))
			{
				SingularName = "CustomerReturnState",
				PluralName = "CustomerReturnStates",
			};
        }

	}

    public partial class MetaCustomerReturnVersion : MetaClass
	{
	    public static MetaCustomerReturnVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CustomerReturnState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType CustomerReturnWhereCurrentVersion;
        public AssociationType CustomerReturnWhereAllVersion;

		// Inherited Associations


		internal MetaCustomerReturnVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4A4C7B91-B5BA-4E89-8F24-0B97AC3BC3EA"))
			{
				SingularName = "CustomerReturnVersion",
				PluralName = "CustomerReturnVersions",
			};
        }

	}

    public partial class MetaCustomerShipment : MetaClass
	{
	    public static MetaCustomerShipment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Hold;
        public MethodType PutOnHold;
        public MethodType Cancel;
        public MethodType Continue;
        public MethodType Ship;
        public MethodType ProcessOnContinue;
        public MethodType SetPicked;
        public MethodType SetPacked;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousCustomerShipmentState;
        public RoleType LastCustomerShipmentState;
        public RoleType CustomerShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType ReleasedManually;
        public RoleType PaymentMethod;
        public RoleType WithoutCharges;
        public RoleType HeldManually;
        public RoleType ShipmentValue;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType PickListVersionWhereCustomerShipmentCorrection;
        public AssociationType PickListWhereCustomerShipmentCorrection;

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaCustomerShipment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9301efcb-2f08-4825-aa60-752c031e4697"))
			{
				SingularName = "CustomerShipment",
				PluralName = "CustomerShipments",
			};
        }

	}

    public partial class MetaCustomerShipmentState : MetaClass
	{
	    public static MetaCustomerShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CustomerShipmentsWherePreviousCustomerShipmentState;
        public AssociationType CustomerShipmentsWhereLastCustomerShipmentState;
        public AssociationType CustomerShipmentsWhereCustomerShipmentState;
        public AssociationType CustomerShipmentVersionsWhereCustomerShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaCustomerShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f2d5bb8b-b50f-45e5-accb-c752a4445ad2"))
			{
				SingularName = "CustomerShipmentState",
				PluralName = "CustomerShipmentStates",
			};
        }

	}

    public partial class MetaCustomerShipmentVersion : MetaClass
	{
	    public static MetaCustomerShipmentVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CustomerShipmentState;
        public RoleType ReleasedManually;
        public RoleType PaymentMethod;
        public RoleType WithoutCharges;
        public RoleType HeldManually;
        public RoleType ShipmentValue;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType CustomerShipmentWhereCurrentVersion;
        public AssociationType CustomerShipmentWhereAllVersion;

		// Inherited Associations


		internal MetaCustomerShipmentVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("EB27ECDA-EE0D-4BC5-8FB1-88CF8501D7B0"))
			{
				SingularName = "CustomerShipmentVersion",
				PluralName = "CustomerShipmentVersions",
			};
        }

	}

    public partial class MetaCustomOrganisationClassification : MetaClass
	{
	    public static MetaCustomOrganisationClassification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType OrganisationVersionsWhereOrganisationClassification;
        public AssociationType OrganisationsWhereOrganisationClassification;
        public AssociationType PartyVersionsWherePartyClassification;
        public AssociationType PartiesWherePartyClassification;
        public AssociationType PriceComponentsWherePartyClassification;


		internal MetaCustomOrganisationClassification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("DE43963D-3505-4B29-8F1F-C24E517D9497"))
			{
				SingularName = "CustomOrganisationClassification",
				PluralName = "CustomOrganisationClassifications",
			};
        }

	}

    public partial class MetaDebitCreditConstant : MetaClass
	{
	    public static MetaDebitCreditConstant Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AmountDuesWhereSide;
        public AssociationType GeneralLedgerAccountsWhereSide;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaDebitCreditConstant(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b330b42-b359-4de7-a084-cc96ce1e6420"))
			{
				SingularName = "DebitCreditConstant",
				PluralName = "DebitCreditConstants",
			};
        }

	}

    public partial class MetaDeduction : MetaClass
	{
	    public static MetaDeduction Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DeductionType;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PayCheckWhereDeduction;

		// Inherited Associations


		internal MetaDeduction(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c04ccfcf-ae3f-4e7f-9e19-503ba547b678"))
			{
				SingularName = "Deduction",
				PluralName = "Deductions",
			};
        }

	}

    public partial class MetaDeductionType : MetaClass
	{
	    public static MetaDeductionType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType DeductionsWhereDeductionType;
        public AssociationType PayrollPreferencesWhereDeductionType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaDeductionType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("66b30b62-5e6c-4747-a72e-bc4ac2cb1125"))
			{
				SingularName = "DeductionType",
				PluralName = "DeductionTypes",
			};
        }

	}

    public partial class MetaDeliverable : MetaClass
	{
	    public static MetaDeliverable Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType DeliverableType;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType QuoteItemVersionsWhereDeliverable;
        public AssociationType RequestItemVersionsWhereDeliverable;
        public AssociationType QuoteItemsWhereDeliverable;
        public AssociationType RequestItemsWhereDeliverable;
        public AssociationType WorkEffortTypesWhereDeliverableToProduce;
        public AssociationType WorkEffortVersionsWhereDeliverablesProduced;
        public AssociationType WorkEffortWhereDeliverablesProduced;

		// Inherited Associations


		internal MetaDeliverable(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("68a6803d-0e65-4141-ac51-25f4c2e49914"))
			{
				SingularName = "Deliverable",
				PluralName = "Deliverables",
			};
        }

	}

    public partial class MetaDeliverableBasedService : MetaClass
	{
	    public static MetaDeliverableBasedService Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType PrimaryProductCategory;
        public ConcreteRoleType SupportDiscontinuationDate;
        public ConcreteRoleType SalesDiscontinuationDate;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType LocalisedDescriptions;
        public ConcreteRoleType LocalisedComments;
        public ConcreteRoleType Description;
        public ConcreteRoleType VirtualProductPriceComponents;
        public ConcreteRoleType IntrastatCode;
        public ConcreteRoleType ProductCategoriesExpanded;
        public ConcreteRoleType ProductComplement;
        public ConcreteRoleType OptionalFeatures;
        public ConcreteRoleType Variants;
        public ConcreteRoleType Name;
        public ConcreteRoleType IntroductionDate;
        public ConcreteRoleType Documents;
        public ConcreteRoleType StandardFeatures;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType ProductObsolescences;
        public ConcreteRoleType SelectableFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType ProductCategories;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ProductDeliverySkillRequirementsWhereService;
        public AssociationType GeneralLedgerAccountsWhereDefaultCostUnit;
        public AssociationType GeneralLedgerAccountsWhereCostUnitsAllowed;
        public AssociationType GoodsWhereProductSubstitution;
        public AssociationType GoodsWhereProductIncompatibility;
        public AssociationType MarketingPackageWhereProductsUsedIn;
        public AssociationType MarketingPackagesWhereProduct;
        public AssociationType PurchaseOrderItemVersionsWhereProduct;
        public AssociationType QuoteItemVersionsWhereProduct;
        public AssociationType RequestItemVersionsWhereProduct;
        public AssociationType SalesInvoiceItemVersionsWhereProduct;
        public AssociationType SalesOrderItemVersionsWherePreviousProduct;
        public AssociationType SalesOrderItemVersionsWhereProduct;
        public AssociationType OrganisationGlAccountsWhereProduct;
        public AssociationType PartyProductRevenuesWhereProduct;
        public AssociationType ProductCategoriesWhereAllProduct;
        public AssociationType ProductConfigurationsWhereProductsUsedIn;
        public AssociationType ProductConfigurationsWhereProduct;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereAvailableFor;
        public AssociationType ProductRevenuesWhereProduct;
        public AssociationType PurchaseOrderItemsWhereProduct;
        public AssociationType QuoteItemsWhereProduct;
        public AssociationType RequestItemsWhereProduct;
        public AssociationType SalesInvoiceItemsWhereProduct;
        public AssociationType SalesOrderItemsWherePreviousProduct;
        public AssociationType SalesOrderItemsWhereProduct;
        public AssociationType SupplierOfferingsWhereProduct;
        public AssociationType WorkEffortTypesWhereProductToProduce;
        public AssociationType EngagementItemsWhereProduct;
        public AssociationType PriceComponentsWhereProduct;
        public AssociationType ProductsWhereProductComplement;
        public AssociationType ProductWhereVariant;
        public AssociationType ProductsWhereProductObsolescence;
        public AssociationType NotificationsWhereTarget;


		internal MetaDeliverableBasedService(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("98fc5441-2037-4134-b143-a9797af9d7f1"))
			{
				SingularName = "DeliverableBasedService",
				PluralName = "DeliverableBasedServices",
			};
        }

	}

    public partial class MetaDeliverableOrderItem : MetaClass
	{
	    public static MetaDeliverableOrderItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType AgreedUponPrice;

		// Inherited Roles
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType Description;
        public ConcreteRoleType ExpectedStartDate;
        public ConcreteRoleType ExpectedEndDate;
        public ConcreteRoleType EngagementWorkFulfillment;
        public ConcreteRoleType EngagementRates;
        public ConcreteRoleType CurrentEngagementRate;
        public ConcreteRoleType OrderedWiths;
        public ConcreteRoleType CurrentAssignedProfessional;
        public ConcreteRoleType Product;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;


		internal MetaDeliverableOrderItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("66bd584c-37c4-4969-874b-7a459195fd25"))
			{
				SingularName = "DeliverableOrderItem",
				PluralName = "DeliverableOrderItems",
			};
        }

	}

    public partial class MetaDeliverableTurnover : MetaClass
	{
	    public static MetaDeliverableTurnover Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType ThroughDateTime;
        public ConcreteRoleType EngagementItem;
        public ConcreteRoleType IsBillable;
        public ConcreteRoleType FromDateTime;
        public ConcreteRoleType Description;
        public ConcreteRoleType WorkEffort;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ServiceEntryBillingsWhereServiceEntry;
        public AssociationType ServiceEntryHeaderWhereServiceEntry;


		internal MetaDeliverableTurnover(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("48733d8e-506a-4add-a230-907221ca7a9a"))
			{
				SingularName = "DeliverableTurnover",
				PluralName = "DeliverableTurnovers",
			};
        }

	}

    public partial class MetaDeliverableType : MetaClass
	{
	    public static MetaDeliverableType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType DeliverablesWhereDeliverableType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaDeliverableType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b1208ddd-9c28-46c3-8d05-2ea1ee29945d"))
			{
				SingularName = "DeliverableType",
				PluralName = "DeliverableTypes",
			};
        }

	}

    public partial class MetaDeployment : MetaClass
	{
	    public static MetaDeployment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ProductOffering;
        public RoleType DeploymentUsage;
        public RoleType SerializedInventoryItem;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaDeployment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ee23df25-f7d7-4974-b62e-ee3cba56b709"))
			{
				SingularName = "Deployment",
				PluralName = "Deployments",
			};
        }

	}

    public partial class MetaDeposit : MetaClass
	{
	    public static MetaDeposit Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Receipts;

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType FinancialAccountWhereFinancialAccountTransaction;


		internal MetaDeposit(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("52458d42-94ee-4757-bcfb-bc9c45ed6dc6"))
			{
				SingularName = "Deposit",
				PluralName = "Deposits",
			};
        }

	}

    public partial class MetaDepreciation : MetaClass
	{
	    public static MetaDepreciation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType FixedAsset;

		// Inherited Roles
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaDepreciation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7107db4e-8406-4fe3-8136-271077c287f8"))
			{
				SingularName = "Depreciation",
				PluralName = "Depreciations",
			};
        }

	}

    public partial class MetaDepreciationMethod : MetaClass
	{
	    public static MetaDepreciationMethod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Formula;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaDepreciationMethod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("63ca0535-95e5-4b2d-847d-d619a5365605"))
			{
				SingularName = "DepreciationMethod",
				PluralName = "DepreciationMethods",
			};
        }

	}

    public partial class MetaDesiredProductFeature : MetaClass
	{
	    public static MetaDesiredProductFeature Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Required;
        public RoleType ProductFeature;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaDesiredProductFeature(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("dda88fe9-14b3-463b-ae66-25dd1b136e16"))
			{
				SingularName = "DesiredProductFeature",
				PluralName = "DesiredProductFeatures",
			};
        }

	}

    public partial class MetaDimension : MetaClass
	{
	    public static MetaDimension Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Unit;
        public RoleType UnitOfMeasure;

		// Inherited Roles
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaDimension(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("26981f3f-f683-4a59-91ad-7a0e4243aea6"))
			{
				SingularName = "Dimension",
				PluralName = "Dimensions",
			};
        }

	}

    public partial class MetaDisbursement : MetaClass
	{
	    public static MetaDisbursement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType PaymentMethod;
        public ConcreteRoleType EffectiveDate;
        public ConcreteRoleType SendingParty;
        public ConcreteRoleType PaymentApplications;
        public ConcreteRoleType ReferenceNumber;
        public ConcreteRoleType ReceivingParty;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType DisbursementAccountingTransactionWhereDisbursement;
        public AssociationType WithdrawalWhereDisbursement;

		// Inherited Associations
        public AssociationType PaymentBudgetAllocationsWherePayment;
        public AssociationType NotificationsWhereTarget;


		internal MetaDisbursement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d152e0a4-c76f-4945-8c0f-ad1e5f70ad07"))
			{
				SingularName = "Disbursement",
				PluralName = "Disbursements",
			};
        }

	}

    public partial class MetaDisbursementAccountingTransaction : MetaClass
	{
	    public static MetaDisbursementAccountingTransaction Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Disbursement;

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaDisbursementAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a3a5aeea-3c8b-43ab-94f1-49a1bd2d7254"))
			{
				SingularName = "DisbursementAccountingTransaction",
				PluralName = "DisbursementAccountingTransactions",
			};
        }

	}

    public partial class MetaDiscountAdjustmentVersion : MetaClass
	{
	    public static MetaDiscountAdjustmentVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType Amount;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType Percentage;

		// Defined Associations
        public AssociationType DiscountAdjustmentWhereCurrentVersion;
        public AssociationType DiscountAdjustmentWhereAllVersion;

		// Inherited Associations


		internal MetaDiscountAdjustmentVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("458F158D-F613-44FC-849A-5438302FA7EB"))
			{
				SingularName = "DiscountAdjustmentVersion",
				PluralName = "DiscountAdjustmentVersions",
			};
        }

	}

    public partial class MetaDiscountAdjustment : MetaClass
	{
	    public static MetaDiscountAdjustment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType Percentage;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InvoiceVersionsWhereDiscountAdjustment;
        public AssociationType InvoiceWhereDiscountAdjustment;
        public AssociationType OrderVersionsWhereDiscountAdjustment;
        public AssociationType PriceableVersionsWhereDiscountAdjustment;
        public AssociationType OrderWhereDiscountAdjustment;
        public AssociationType PriceableWhereDiscountAdjustment;

		// Inherited Associations


		internal MetaDiscountAdjustment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0346a1e2-03c7-4f1e-94ae-35fdf64143a9"))
			{
				SingularName = "DiscountAdjustment",
				PluralName = "DiscountAdjustments",
			};
        }

	}

    public partial class MetaDiscountComponent : MetaClass
	{
	    public static MetaDiscountComponent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Percentage;

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaDiscountComponent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c0b927c4-7197-4295-8edf-057b6b4b3a6a"))
			{
				SingularName = "DiscountComponent",
				PluralName = "DiscountComponents",
			};
        }

	}

    public partial class MetaDropShipment : MetaClass
	{
	    public static MetaDropShipment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousDropShipmentState;
        public RoleType LastDropShipmentState;
        public RoleType DropShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaDropShipment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a981c832-dd3a-4b97-9bc9-d2dd83872bf2"))
			{
				SingularName = "DropShipment",
				PluralName = "DropShipments",
			};
        }

	}

    public partial class MetaDropShipmentState : MetaClass
	{
	    public static MetaDropShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType DropShipmentsWherePreviousDropShipmentState;
        public AssociationType DropShipmentsWhereLastDropShipmentState;
        public AssociationType DropShipmentsWhereDropShipmentState;
        public AssociationType DropShipmentVersionsWhereDropShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaDropShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("89d2037a-4bc2-4929-b333-5358ac4a14e5"))
			{
				SingularName = "DropShipmentState",
				PluralName = "DropShipmentStates",
			};
        }

	}

    public partial class MetaDropShipmentVersion : MetaClass
	{
	    public static MetaDropShipmentVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DropShipmentState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType DropShipmentWhereCurrentVersion;
        public AssociationType DropShipmentWhereAllVersion;

		// Inherited Associations


		internal MetaDropShipmentVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("97E9E1C9-E35F-4139-8206-2C34021A0799"))
			{
				SingularName = "DropShipmentVersion",
				PluralName = "DropShipmentVersions",
			};
        }

	}

    public partial class MetaDunningType : MetaClass
	{
	    public static MetaDunningType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartyVersionsWhereDunningType;
        public AssociationType PartiesWhereDunningType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaDunningType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4117ba43-c7fd-4ba5-965e-50e2ce5b5058"))
			{
				SingularName = "DunningType",
				PluralName = "DunningTypes",
			};
        }

	}

    public partial class MetaEmailAddress : MetaClass
	{
	    public static MetaEmailAddress Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType ElectronicAddressString;
        public ConcreteRoleType Description;
        public ConcreteRoleType FollowTo;
        public ConcreteRoleType ContactMechanismType;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType EmailCommunicationsWhereOriginator;
        public AssociationType EmailCommunicationsWhereAddressee;
        public AssociationType EmailCommunicationsWhereCarbonCopy;
        public AssociationType EmailCommunicationsWhereBlindCopy;
        public AssociationType EmailCommunicationVersionsWhereOriginator;
        public AssociationType EmailCommunicationVersionsWhereAddressee;
        public AssociationType EmailCommunicationVersionsWhereCarbonCopy;
        public AssociationType EmailCommunicationVersionsWhereBlindCopy;
        public AssociationType PartiesWhereGeneralEmail;
        public AssociationType PartyWherePersonalEmailAddress;

		// Inherited Associations
        public AssociationType PartyVersionsWhereGeneralEmail;
        public AssociationType PartyVersionWherePersonalEmailAddress;
        public AssociationType PartyVersionsWhereInternetAddress;
        public AssociationType PartiesWhereInternetAddress;
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;


		internal MetaEmailAddress(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f4b7ea51-eac4-479b-92e8-5109cfeceb77"))
			{
				SingularName = "EmailAddress",
				PluralName = "EmailAddresses",
			};
        }

	}

    public partial class MetaEmailCommunication : MetaClass
	{
	    public static MetaEmailCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType Originator;
        public RoleType Addressees;
        public RoleType CarbonCopies;
        public RoleType BlindCopies;
        public RoleType EmailTemplate;
        public RoleType IncomingMail;

		// Inherited Roles
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaEmailCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9426c214-c85d-491b-a5a6-9f573c3341a0"))
			{
				SingularName = "EmailCommunication",
				PluralName = "EmailCommunications",
			};
        }

	}

    public partial class MetaEmailCommunicationVersion : MetaClass
	{
	    public static MetaEmailCommunicationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Originator;
        public RoleType Addressees;
        public RoleType CarbonCopies;
        public RoleType BlindCopies;
        public RoleType EmailTemplate;
        public RoleType IncomingMail;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType EmailCommunicationWhereCurrentVersion;
        public AssociationType EmailCommunicationWhereAllVersion;

		// Inherited Associations


		internal MetaEmailCommunicationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("EB4F71AE-3BA1-4421-82AB-11F6F3E8C4D5"))
			{
				SingularName = "EmailCommunicationVersion",
				PluralName = "EmailCommunicationVersions",
			};
        }

	}

    public partial class MetaEmailTemplate : MetaClass
	{
	    public static MetaEmailTemplate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType BodyTemplate;
        public RoleType SubjectTemplate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EmailCommunicationsWhereEmailTemplate;
        public AssociationType EmailCommunicationVersionsWhereEmailTemplate;

		// Inherited Associations


		internal MetaEmailTemplate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c78a49b1-9918-4f15-95f3-c537c82f59fd"))
			{
				SingularName = "EmailTemplate",
				PluralName = "EmailTemplates",
			};
        }

	}

    public partial class MetaEmployment : MetaClass
	{
	    public static MetaEmployment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Employee;
        public RoleType PayrollPreferences;
        public RoleType EmploymentTerminationReason;
        public RoleType EmploymentTermination;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaEmployment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6a7e45b2-36b2-4d2e-a29c-0cc13851766f"))
			{
				SingularName = "Employment",
				PluralName = "Employments",
			};
        }

	}

    public partial class MetaEmploymentAgreement : MetaClass
	{
	    public static MetaEmploymentAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AgreementDate;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType Text;
        public ConcreteRoleType AgreementItems;
        public ConcreteRoleType AgreementNumber;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;
        public AssociationType NotificationsWhereTarget;


		internal MetaEmploymentAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d402d086-0d7a-4e98-bcb1-8f8e1cfabb99"))
			{
				SingularName = "EmploymentAgreement",
				PluralName = "EmploymentAgreements",
			};
        }

	}

    public partial class MetaEmploymentApplication : MetaClass
	{
	    public static MetaEmploymentApplication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ApplicationDate;
        public RoleType Position;
        public RoleType EmploymentApplicationStatus;
        public RoleType Person;
        public RoleType EmploymentApplicationSource;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaEmploymentApplication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6940c300-47e6-44f2-b93b-d70bed9de602"))
			{
				SingularName = "EmploymentApplication",
				PluralName = "EmploymentApplications",
			};
        }

	}

    public partial class MetaEmploymentApplicationSource : MetaClass
	{
	    public static MetaEmploymentApplicationSource Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EmploymentApplicationsWhereEmploymentApplicationSource;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaEmploymentApplicationSource(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("74cd22cf-1796-4c65-85df-9c3e09883843"))
			{
				SingularName = "EmploymentApplicationSource",
				PluralName = "EmploymentApplicationSources",
			};
        }

	}

    public partial class MetaEmploymentApplicationStatus : MetaClass
	{
	    public static MetaEmploymentApplicationStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EmploymentApplicationsWhereEmploymentApplicationStatus;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaEmploymentApplicationStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c7c24ce4-3455-4cec-a733-64a436434b3e"))
			{
				SingularName = "EmploymentApplicationStatus",
				PluralName = "EmploymentApplicationStatuses",
			};
        }

	}

    public partial class MetaEmploymentTermination : MetaClass
	{
	    public static MetaEmploymentTermination Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EmploymentsWhereEmploymentTermination;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaEmploymentTermination(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("129e6fe8-01d0-40ad-bc6a-e5449c19274f"))
			{
				SingularName = "EmploymentTermination",
				PluralName = "EmploymentTerminations",
			};
        }

	}

    public partial class MetaEmploymentTerminationReason : MetaClass
	{
	    public static MetaEmploymentTerminationReason Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EmploymentsWhereEmploymentTerminationReason;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaEmploymentTerminationReason(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f7b039f4-10f4-4939-8059-5f190fd13b09"))
			{
				SingularName = "EmploymentTerminationReason",
				PluralName = "EmploymentTerminationReasons",
			};
        }

	}

    public partial class MetaEngagement : MetaClass
	{
	    public static MetaEngagement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Agreement;
        public RoleType PlacingContactMechanism;
        public RoleType MaximumAmount;
        public RoleType BillToContactMechanism;
        public RoleType Description;
        public RoleType BillToParty;
        public RoleType PlacingParty;
        public RoleType StartDate;
        public RoleType TakenViaContactMechanism;
        public RoleType EstimatedAmount;
        public RoleType EndDate;
        public RoleType ContractDate;
        public RoleType EngagementItems;
        public RoleType ClientPurchaseOrderNumber;
        public RoleType TakenViaOrganisationContactRelationship;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaEngagement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("752a68b0-836e-4cd5-92d5-ebf2bfeda491"))
			{
				SingularName = "Engagement",
				PluralName = "Engagements",
			};
        }

	}

    public partial class MetaEngagementRate : MetaClass
	{
	    public static MetaEngagementRate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType BillingRate;
        public RoleType RatingType;
        public RoleType Cost;
        public RoleType GoverningPriceComponents;
        public RoleType ChangeReason;
        public RoleType UnitOfMeasure;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EngagementItemWhereEngagementRate;
        public AssociationType EngagementItemsWhereCurrentEngagementRate;

		// Inherited Associations


		internal MetaEngagementRate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6b666a30-7a55-4986-8411-b6179768e70b"))
			{
				SingularName = "EngagementRate",
				PluralName = "EngagementRates",
			};
        }

	}

    public partial class MetaEngineeringBom : MetaClass
	{
	    public static MetaEngineeringBom Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Part;
        public ConcreteRoleType Instruction;
        public ConcreteRoleType QuantityUsed;
        public ConcreteRoleType ComponentPart;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngineeringChangesWherePartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWhereSubstitutionPartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWherePartBillOfMaterial;


		internal MetaEngineeringBom(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("14a85148-0d92-4869-8a94-b102f047931f"))
			{
				SingularName = "EngineeringBom",
				PluralName = "EngineeringBoms",
			};
        }

	}

    public partial class MetaEngineeringChange : MetaClass
	{
	    public static MetaEngineeringChange Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Requestor;
        public RoleType Authorizer;
        public RoleType Description;
        public RoleType Designer;
        public RoleType PartSpecifications;
        public RoleType PartBillOfMaterials;
        public RoleType CurrentObjectState;
        public RoleType EngineeringChangeStatuses;
        public RoleType Tester;
        public RoleType CurrentEngineeringChangeStatus;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaEngineeringChange(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c6c4537a-21f8-4d62-b584-3c609fb2210f"))
			{
				SingularName = "EngineeringChange",
				PluralName = "EngineeringChanges",
			};
        }

	}

    public partial class MetaEngineeringChangeObjectState : MetaClass
	{
	    public static MetaEngineeringChangeObjectState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EngineeringChangesWhereCurrentObjectState;
        public AssociationType EngineeringChangeStatusesWhereEngineeringChangeObjectState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaEngineeringChangeObjectState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e3f78cf6-6367-4b0f-9ac0-b887e7187c5e"))
			{
				SingularName = "EngineeringChangeObjectState",
				PluralName = "EngineeringChangeObjectStates",
			};
        }

	}

    public partial class MetaEngineeringChangeStatus : MetaClass
	{
	    public static MetaEngineeringChangeStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType StartDateTime;
        public RoleType EngineeringChangeObjectState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EngineeringChangeWhereEngineeringChangeStatus;
        public AssociationType EngineeringChangeWhereCurrentEngineeringChangeStatus;

		// Inherited Associations


		internal MetaEngineeringChangeStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d149dd80-1cdc-4a29-bb0b-b88823d718bc"))
			{
				SingularName = "EngineeringChangeStatus",
				PluralName = "EngineeringChangeStatuses",
			};
        }

	}

    public partial class MetaEngineeringDocument : MetaClass
	{
	    public static MetaEngineeringDocument Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaEngineeringDocument(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8da5bb9b-593b-4c10-91c2-1e9cc2c226d1"))
			{
				SingularName = "EngineeringDocument",
				PluralName = "EngineeringDocuments",
			};
        }

	}

    public partial class MetaEquipment : MetaClass
	{
	    public static MetaEquipment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType LastServiceDate;
        public ConcreteRoleType AcquiredDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType ProductionCapacity;
        public ConcreteRoleType NextServiceDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType DepreciationsWhereFixedAsset;
        public AssociationType PartyFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetStandardsWhereFixedAsset;
        public AssociationType WorkEffortTypesWhereFixedAssetToRepair;


		internal MetaEquipment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da852ff9-0c87-4fa6-a93a-90d97d28029c"))
			{
				SingularName = "Equipment",
				PluralName = "Equipments",
			};
        }

	}

    public partial class MetaEstimatedLaborCost : MetaClass
	{
	    public static MetaEstimatedLaborCost Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Cost;
        public ConcreteRoleType Currency;
        public ConcreteRoleType Organisation;
        public ConcreteRoleType Description;
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ProductWhereEstimatedProductCost;
        public AssociationType ProductFeatureWhereEstimatedProductCost;


		internal MetaEstimatedLaborCost(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2a84fcce-91f6-4d8b-9840-2ddd5f4b3dac"))
			{
				SingularName = "EstimatedLaborCost",
				PluralName = "EstimatedLaborCosts",
			};
        }

	}

    public partial class MetaEstimatedMaterialCost : MetaClass
	{
	    public static MetaEstimatedMaterialCost Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Cost;
        public ConcreteRoleType Currency;
        public ConcreteRoleType Organisation;
        public ConcreteRoleType Description;
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ProductWhereEstimatedProductCost;
        public AssociationType ProductFeatureWhereEstimatedProductCost;


		internal MetaEstimatedMaterialCost(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("cb6a8e8a-04a6-437b-b952-f502cca2a2db"))
			{
				SingularName = "EstimatedMaterialCost",
				PluralName = "EstimatedMaterialCosts",
			};
        }

	}

    public partial class MetaEstimatedOtherCost : MetaClass
	{
	    public static MetaEstimatedOtherCost Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Cost;
        public ConcreteRoleType Currency;
        public ConcreteRoleType Organisation;
        public ConcreteRoleType Description;
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ProductWhereEstimatedProductCost;
        public AssociationType ProductFeatureWhereEstimatedProductCost;


		internal MetaEstimatedOtherCost(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9b637b39-f61a-4985-bb1b-876ed769f448"))
			{
				SingularName = "EstimatedOtherCost",
				PluralName = "EstimatedOtherCosts",
			};
        }

	}

    public partial class MetaEuSalesListType : MetaClass
	{
	    public static MetaEuSalesListType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType VatRatesWhereEuSalesListType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaEuSalesListType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("acbe7b46-bcfe-4e8b-b8a7-7b9eeac4d6e2"))
			{
				SingularName = "EuSalesListType",
				PluralName = "EuSalesListTypes",
			};
        }

	}

    public partial class MetaEvent : MetaClass
	{
	    public static MetaEvent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RegistrationRequired;
        public RoleType Link;
        public RoleType Location;
        public RoleType Text;
        public RoleType AnnouncementText;
        public RoleType From;
        public RoleType Locale;
        public RoleType Title;
        public RoleType Photo;
        public RoleType Announce;
        public RoleType To;

		// Inherited Roles

		// Defined Associations
        public AssociationType EventRegistrationsWhereEvent;

		// Inherited Associations


		internal MetaEvent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("aad26d12-9e80-410c-ab99-57064bd3dd2e"))
			{
				SingularName = "Event",
				PluralName = "Events",
			};
        }

	}

    public partial class MetaEventRegistration : MetaClass
	{
	    public static MetaEventRegistration Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Person;
        public RoleType Event;
        public RoleType AllorsDateTime;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaEventRegistration(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2b5efcb9-54ba-4d59-833b-716d321cc7cb"))
			{
				SingularName = "EventRegistration",
				PluralName = "EventRegistrations",
			};
        }

	}

    public partial class MetaExpenseEntry : MetaClass
	{
	    public static MetaExpenseEntry Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType ThroughDateTime;
        public ConcreteRoleType EngagementItem;
        public ConcreteRoleType IsBillable;
        public ConcreteRoleType FromDateTime;
        public ConcreteRoleType Description;
        public ConcreteRoleType WorkEffort;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ServiceEntryBillingsWhereServiceEntry;
        public AssociationType ServiceEntryHeaderWhereServiceEntry;


		internal MetaExpenseEntry(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f15e6b0e-0222-4f9b-8ae2-20c20f3b3673"))
			{
				SingularName = "ExpenseEntry",
				PluralName = "ExpenseEntries",
			};
        }

	}

    public partial class MetaExportDocument : MetaClass
	{
	    public static MetaExportDocument Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaExportDocument(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("efe15d5d-f07c-497e-98c2-dd64f624840f"))
			{
				SingularName = "ExportDocument",
				PluralName = "ExportDocuments",
			};
        }

	}

    public partial class MetaFaceToFaceCommunication : MetaClass
	{
	    public static MetaFaceToFaceCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType Participants;
        public RoleType Location;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaFaceToFaceCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d0f9fc0d-a3c5-46cc-ab00-4c724995fc14"))
			{
				SingularName = "FaceToFaceCommunication",
				PluralName = "FaceToFaceCommunications",
			};
        }

	}

    public partial class MetaFaceToFaceCommunicationVersion : MetaClass
	{
	    public static MetaFaceToFaceCommunicationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Participants;
        public RoleType Location;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType FaceToFaceCommunicationWhereCurrentVersion;
        public AssociationType FaceToFaceCommunicationWhereAllVersion;

		// Inherited Associations


		internal MetaFaceToFaceCommunicationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("12DB12F5-88CC-46D2-A664-28797C143D92"))
			{
				SingularName = "FaceToFaceCommunicationVersion",
				PluralName = "FaceToFaceCommunicationVersions",
			};
        }

	}

    public partial class MetaFacility : MetaClass
	{
	    public static MetaFacility Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType FacilityType;
        public RoleType MadeUpOf;
        public RoleType SquareFootage;
        public RoleType Description;
        public RoleType FacilityContactMechanisms;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;

		// Defined Associations
        public AssociationType FacilitiesWhereMadeUpOf;
        public AssociationType InternalOrganisationsWhereDefaultFacility;
        public AssociationType PurchaseOrderVersionsWhereFacility;
        public AssociationType PurchaseShipmentVersionsWhereFacility;
        public AssociationType RequirementVersionsWhereFacility;
        public AssociationType PurchaseOrdersWhereFacility;
        public AssociationType PurchaseShipmentsWhereFacility;
        public AssociationType RequirementsWhereFacility;
        public AssociationType ShipmentRouteSegmentsWhereFromFacility;
        public AssociationType ShipmentRouteSegmentsWhereToFacility;
        public AssociationType StoresWhereDefaultFacility;
        public AssociationType WorkEffortPartyAssignmentsWhereFacility;
        public AssociationType InventoryItemVersionsWhereFacility;
        public AssociationType InventoryItemsWhereFacility;
        public AssociationType WorkEffortVersionsWhereFacility;
        public AssociationType WorkEffortsWhereFacility;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaFacility(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("cdd79e23-a132-48b0-b88f-a03bd029f49d"))
			{
				SingularName = "Facility",
				PluralName = "Facilities",
			};
        }

	}

    public partial class MetaFaxCommunication : MetaClass
	{
	    public static MetaFaxCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType Originator;
        public RoleType Receiver;
        public RoleType OutgoingFaxNumber;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaFaxCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1e67320b-9680-4477-bf1b-70ccd24ab758"))
			{
				SingularName = "FaxCommunication",
				PluralName = "FaxCommunications",
			};
        }

	}

    public partial class MetaFaxCommunicationVersion : MetaClass
	{
	    public static MetaFaxCommunicationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Originator;
        public RoleType Receiver;
        public RoleType OutgoingFaxNumber;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType FaxCommunicationWhereCurrentVersion;
        public AssociationType FaxCommunicationWhereAllVersion;

		// Inherited Associations


		internal MetaFaxCommunicationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B64D00D7-03C7-4D7A-B0A2-0825234C2070"))
			{
				SingularName = "FaxCommunicationVersion",
				PluralName = "FaxCommunicationVersions",
			};
        }

	}

    public partial class MetaFee : MetaClass
	{
	    public static MetaFee Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType Percentage;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InvoiceVersionsWhereFee;
        public AssociationType InvoiceWhereFee;
        public AssociationType OrderVersionsWhereFee;
        public AssociationType OrderWhereFee;

		// Inherited Associations


		internal MetaFee(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fb3dd618-eeb5-4ef6-87ca-abfe91dc603f"))
			{
				SingularName = "Fee",
				PluralName = "Fees",
			};
        }

	}

    public partial class MetaFinancialAccountAdjustment : MetaClass
	{
	    public static MetaFinancialAccountAdjustment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType FinancialAccountWhereFinancialAccountTransaction;


		internal MetaFinancialAccountAdjustment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("12ba6843-bae1-41d1-9ef2-c19d74b0a365"))
			{
				SingularName = "FinancialAccountAdjustment",
				PluralName = "FinancialAccountAdjustments",
			};
        }

	}

    public partial class MetaFinancialTerm : MetaClass
	{
	    public static MetaFinancialTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaFinancialTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a73aa458-2293-4578-be67-ad32e36a4991"))
			{
				SingularName = "FinancialTerm",
				PluralName = "FinancialTerms",
			};
        }

	}

    public partial class MetaFinishedGood : MetaClass
	{
	    public static MetaFinishedGood Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType PartSpecifications;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType Documents;
        public ConcreteRoleType ManufacturerId;
        public ConcreteRoleType ReorderLevel;
        public ConcreteRoleType ReorderQuantity;
        public ConcreteRoleType PriceComponents;
        public ConcreteRoleType InventoryItemKind;
        public ConcreteRoleType Sku;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType GoodsWhereFinishedGood;

		// Inherited Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePart;
        public AssociationType PurchaseOrderItemVersionsWherePart;
        public AssociationType PartRevisionsWhereSupersededByPart;
        public AssociationType PartRevisionsWherePart;
        public AssociationType PartSubstitutesWhereSubstitutionPart;
        public AssociationType PartSubstitutesWherePart;
        public AssociationType PurchaseInvoiceItemsWherePart;
        public AssociationType PurchaseOrderItemsWherePart;
        public AssociationType ShipmentItemsWherePart;
        public AssociationType SupplierOfferingsWherePart;
        public AssociationType WorkEffortPartStandardsWherePart;
        public AssociationType InventoryItemVersionsWherePart;
        public AssociationType InventoryItemsWherePart;
        public AssociationType PartBillOfMaterialsWherePart;
        public AssociationType PartBillOfMaterialsWhereComponentPart;
        public AssociationType NotificationsWhereTarget;


		internal MetaFinishedGood(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("364071a2-bcda-4bdc-b0f9-0e56d28604d6"))
			{
				SingularName = "FinishedGood",
				PluralName = "FinishedGoods",
			};
        }

	}

    public partial class MetaFiscalYearInvoiceNumber : MetaClass
	{
	    public static MetaFiscalYearInvoiceNumber Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType NextSalesInvoiceNumber;
        public RoleType FiscalYear;

		// Inherited Roles

		// Defined Associations
        public AssociationType StoreWhereFiscalYearInvoiceNumber;

		// Inherited Associations


		internal MetaFiscalYearInvoiceNumber(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("341fa885-0161-406b-89e6-08b1c92cd3b3"))
			{
				SingularName = "FiscalYearInvoiceNumber",
				PluralName = "FiscalYearInvoiceNumbers",
			};
        }

	}

    public partial class MetaFacilityType : MetaClass
	{
	    public static MetaFacilityType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType FacilitiesWhereFacilityType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaFacilityType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("65B821F6-FE91-4411-9716-20C1D9D86E91"))
			{
				SingularName = "FacilityType",
				PluralName = "FacilityTypes",
			};
        }

	}

    public partial class MetaGenderType : MetaClass
	{
	    public static MetaGenderType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PeopleWhereGender;
        public AssociationType PersonVersionsWhereGender;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaGenderType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f35745a9-a8d3-4002-a484-6f0fb00a69a2"))
			{
				SingularName = "GenderType",
				PluralName = "GenderTypes",
			};
        }

	}

    public partial class MetaGeneralLedgerAccount : MetaClass
	{
	    public static MetaGeneralLedgerAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType DefaultCostUnit;
        public RoleType DefaultCostCenter;
        public RoleType Description;
        public RoleType GeneralLedgerAccountType;
        public RoleType CashAccount;
        public RoleType CostCenterAccount;
        public RoleType Side;
        public RoleType BalanceSheetAccount;
        public RoleType ReconciliationAccount;
        public RoleType Name;
        public RoleType CostCenterRequired;
        public RoleType CostUnitRequired;
        public RoleType GeneralLedgerAccountGroup;
        public RoleType CostCentersAllowed;
        public RoleType CostUnitAccount;
        public RoleType AccountNumber;
        public RoleType CostUnitsAllowed;
        public RoleType Protected;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ChartOfAccountsWhereGeneralLedgerAccount;
        public AssociationType GlBudgetAllocationsWhereGeneralLedgerAccount;
        public AssociationType InternalOrganisationWhereSalesPaymentDifferencesAccount;
        public AssociationType InternalOrganisationWhereGeneralLedgerAccount;
        public AssociationType InternalOrganisationsWhereRetainedEarningsAccount;
        public AssociationType InternalOrganisationsWhereSalesPaymentDiscountDifferencesAccount;
        public AssociationType InternalOrganisationsWherePurchasePaymentDifferencesAccount;
        public AssociationType InternalOrganisationsWhereSuspenceAccount;
        public AssociationType InternalOrganisationsWhereNetIncomeAccount;
        public AssociationType InternalOrganisationsWherePurchasePaymentDiscountDifferencesAccount;
        public AssociationType InternalOrganisationsWhereCalculationDifferencesAccount;
        public AssociationType InternalOrganisationsWhereGlAccount;
        public AssociationType OrganisationGlAccountsWhereGeneralLedgerAccount;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaGeneralLedgerAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1a0e396b-69bd-4e77-a602-3d7f7938fd74"))
			{
				SingularName = "GeneralLedgerAccount",
				PluralName = "GeneralLedgerAccounts",
			};
        }

	}

    public partial class MetaGeneralLedgerAccountGroup : MetaClass
	{
	    public static MetaGeneralLedgerAccountGroup Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Parent;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType GeneralLedgerAccountsWhereGeneralLedgerAccountGroup;
        public AssociationType GeneralLedgerAccountGroupsWhereParent;

		// Inherited Associations


		internal MetaGeneralLedgerAccountGroup(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4a600c96-b813-46fc-8674-06bd3f85eae4"))
			{
				SingularName = "GeneralLedgerAccountGroup",
				PluralName = "GeneralLedgerAccountGroups",
			};
        }

	}

    public partial class MetaGeneralLedgerAccountType : MetaClass
	{
	    public static MetaGeneralLedgerAccountType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType GeneralLedgerAccountsWhereGeneralLedgerAccountType;

		// Inherited Associations


		internal MetaGeneralLedgerAccountType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ce5c78ee-f892-4ced-9b21-51d84c77127f"))
			{
				SingularName = "GeneralLedgerAccountType",
				PluralName = "GeneralLedgerAccountTypes",
			};
        }

	}

    public partial class MetaGlBudgetAllocation : MetaClass
	{
	    public static MetaGlBudgetAllocation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType GeneralLedgerAccount;
        public RoleType BudgetItem;
        public RoleType AllocationPercentage;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaGlBudgetAllocation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("084829bc-d347-489a-9557-9ff1ac7fb5a0"))
			{
				SingularName = "GlBudgetAllocation",
				PluralName = "GlBudgetAllocations",
			};
        }

	}

    public partial class MetaGood : MetaClass
	{
	    public static MetaGood Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType QuantityOnHand;
        public RoleType AvailableToPromise;
        public RoleType InventoryItemKind;
        public RoleType BarCode;
        public RoleType FinishedGood;
        public RoleType Sku;
        public RoleType ArticleNumber;
        public RoleType ManufacturedBy;
        public RoleType ManufacturerId;
        public RoleType SuppliedBy;
        public RoleType ProductSubstitutions;
        public RoleType ProductIncompatibilities;
        public RoleType PrimaryPhoto;
        public RoleType Photos;
        public RoleType Keywords;

		// Inherited Roles
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType PrimaryProductCategory;
        public ConcreteRoleType SupportDiscontinuationDate;
        public ConcreteRoleType SalesDiscontinuationDate;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType LocalisedDescriptions;
        public ConcreteRoleType LocalisedComments;
        public ConcreteRoleType Description;
        public ConcreteRoleType VirtualProductPriceComponents;
        public ConcreteRoleType IntrastatCode;
        public ConcreteRoleType ProductCategoriesExpanded;
        public ConcreteRoleType ProductComplement;
        public ConcreteRoleType OptionalFeatures;
        public ConcreteRoleType Variants;
        public ConcreteRoleType Name;
        public ConcreteRoleType IntroductionDate;
        public ConcreteRoleType Documents;
        public ConcreteRoleType StandardFeatures;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType ProductObsolescences;
        public ConcreteRoleType SelectableFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType ProductCategories;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations
        public AssociationType DeploymentsWhereProductOffering;
        public AssociationType ShipmentItemsWhereGood;
        public AssociationType WorkEffortGoodStandardsWhereGood;
        public AssociationType InventoryItemVersionsWhereGood;
        public AssociationType InventoryItemsWhereGood;

		// Inherited Associations
        public AssociationType GeneralLedgerAccountsWhereDefaultCostUnit;
        public AssociationType GeneralLedgerAccountsWhereCostUnitsAllowed;
        public AssociationType GoodsWhereProductSubstitution;
        public AssociationType GoodsWhereProductIncompatibility;
        public AssociationType MarketingPackageWhereProductsUsedIn;
        public AssociationType MarketingPackagesWhereProduct;
        public AssociationType PurchaseOrderItemVersionsWhereProduct;
        public AssociationType QuoteItemVersionsWhereProduct;
        public AssociationType RequestItemVersionsWhereProduct;
        public AssociationType SalesInvoiceItemVersionsWhereProduct;
        public AssociationType SalesOrderItemVersionsWherePreviousProduct;
        public AssociationType SalesOrderItemVersionsWhereProduct;
        public AssociationType OrganisationGlAccountsWhereProduct;
        public AssociationType PartyProductRevenuesWhereProduct;
        public AssociationType ProductCategoriesWhereAllProduct;
        public AssociationType ProductConfigurationsWhereProductsUsedIn;
        public AssociationType ProductConfigurationsWhereProduct;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereAvailableFor;
        public AssociationType ProductRevenuesWhereProduct;
        public AssociationType PurchaseOrderItemsWhereProduct;
        public AssociationType QuoteItemsWhereProduct;
        public AssociationType RequestItemsWhereProduct;
        public AssociationType SalesInvoiceItemsWhereProduct;
        public AssociationType SalesOrderItemsWherePreviousProduct;
        public AssociationType SalesOrderItemsWhereProduct;
        public AssociationType SupplierOfferingsWhereProduct;
        public AssociationType WorkEffortTypesWhereProductToProduce;
        public AssociationType EngagementItemsWhereProduct;
        public AssociationType PriceComponentsWhereProduct;
        public AssociationType ProductsWhereProductComplement;
        public AssociationType ProductWhereVariant;
        public AssociationType ProductsWhereProductObsolescence;
        public AssociationType NotificationsWhereTarget;


		internal MetaGood(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e3e87d40-b4f0-4953-9716-db13b35d716b"))
			{
				SingularName = "Good",
				PluralName = "Goods",
			};
        }

	}

    public partial class MetaGoodOrderItem : MetaClass
	{
	    public static MetaGoodOrderItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Price;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType Description;
        public ConcreteRoleType ExpectedStartDate;
        public ConcreteRoleType ExpectedEndDate;
        public ConcreteRoleType EngagementWorkFulfillment;
        public ConcreteRoleType EngagementRates;
        public ConcreteRoleType CurrentEngagementRate;
        public ConcreteRoleType OrderedWiths;
        public ConcreteRoleType CurrentAssignedProfessional;
        public ConcreteRoleType Product;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;


		internal MetaGoodOrderItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c1b6fac9-8e69-4c07-8cec-e9b52c690e72"))
			{
				SingularName = "GoodOrderItem",
				PluralName = "GoodOrderItems",
			};
        }

	}

    public partial class MetaHazardousMaterialsDocument : MetaClass
	{
	    public static MetaHazardousMaterialsDocument Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaHazardousMaterialsDocument(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("93e3b3df-b227-479a-9b05-ec10190e7d51"))
			{
				SingularName = "HazardousMaterialsDocument",
				PluralName = "HazardousMaterialsDocuments",
			};
        }

	}

    public partial class MetaHobby : MetaClass
	{
	    public static MetaHobby Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PeopleWhereHobby;
        public AssociationType PersonVersionsWhereHobby;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaHobby(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2f18f79f-dd13-4e89-b3fa-95d789dd383e"))
			{
				SingularName = "Hobby",
				PluralName = "Hobbies",
			};
        }

	}

    public partial class MetaIncentive : MetaClass
	{
	    public static MetaIncentive Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaIncentive(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("150d21f7-20dd-4951-848f-f74a69dadb5b"))
			{
				SingularName = "Incentive",
				PluralName = "Incentives",
			};
        }

	}

    public partial class MetaIndustryClassification : MetaClass
	{
	    public static MetaIndustryClassification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType OrganisationVersionsWhereIndustryClassification;
        public AssociationType OrganisationsWhereIndustryClassification;

		// Inherited Associations
        public AssociationType OrganisationVersionsWhereOrganisationClassification;
        public AssociationType OrganisationsWhereOrganisationClassification;
        public AssociationType PartyVersionsWherePartyClassification;
        public AssociationType PartiesWherePartyClassification;
        public AssociationType PriceComponentsWherePartyClassification;


		internal MetaIndustryClassification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da27b432-85e4-4c83-bdb0-64cefb347e8a"))
			{
				SingularName = "IndustryClassification",
				PluralName = "IndustryClassifications",
			};
        }

	}

    public partial class MetaInternalOrganisation : MetaClass
	{
	    public static MetaInternalOrganisation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseOrderNumberPrefix;
        public RoleType TransactionReferenceNumber;
        public RoleType JournalEntryNumbers;
        public RoleType EuListingState;
        public RoleType PurchaseInvoiceCounter;
        public RoleType ActualAccountingPeriod;
        public RoleType InvoiceSequence;
        public RoleType ActivePaymentMethods;
        public RoleType MaximumAllowedPaymentDifference;
        public RoleType LogoImage;
        public RoleType CostCenterSplitMethod;
        public RoleType PurchaseOrderCounter;
        public RoleType LegalForm;
        public RoleType SalesPaymentDifferencesAccount;
        public RoleType Name;
        public RoleType PurchaseTransactionReferenceNumber;
        public RoleType FiscalYearStartMonth;
        public RoleType CostOfGoodsSoldMethod;
        public RoleType VatDeactivated;
        public RoleType FiscalYearStartDay;
        public RoleType GeneralLedgerAccounts;
        public RoleType AccountingTransactionCounter;
        public RoleType IncomingShipmentCounter;
        public RoleType RetainedEarningsAccount;
        public RoleType PurchaseInvoiceNumberPrefix;
        public RoleType SalesPaymentDiscountDifferencesAccount;
        public RoleType SubAccountCounter;
        public RoleType AccountingTransactionNumbers;
        public RoleType TransactionReferenceNumberPrefix;
        public RoleType QuoteCounter;
        public RoleType RequestCounter;
        public RoleType PurchasePaymentDifferencesAccount;
        public RoleType SuspenceAccount;
        public RoleType NetIncomeAccount;
        public RoleType DoAccounting;
        public RoleType DefaultFacility;
        public RoleType PurchasePaymentDiscountDifferencesAccount;
        public RoleType QuoteNumberPrefix;
        public RoleType PurchaseTransactionReferenceNumberPrefix;
        public RoleType TaxNumber;
        public RoleType CalculationDifferencesAccount;
        public RoleType IncomingShipmentNumberPrefix;
        public RoleType RequestNumberPrefix;
        public RoleType CurrentSalesReps;
        public RoleType CurrentCustomers;
        public RoleType CurrentSuppliers;
        public RoleType BankAccounts;
        public RoleType DefaultPaymentMethod;
        public RoleType VatRegime;
        public RoleType SalesReps;
        public RoleType GlAccount;
        public RoleType BillingAddress;
        public RoleType OrderAddress;
        public RoleType ShippingAddress;
        public RoleType BillingInquiriesFax;
        public RoleType BillingInquiriesPhone;
        public RoleType CellPhoneNumber;
        public RoleType GeneralFaxNumber;
        public RoleType GeneralPhoneNumber;
        public RoleType HeadQuarter;
        public RoleType InternetAddress;
        public RoleType OrderInquiriesFax;
        public RoleType OrderInquiriesPhone;
        public RoleType GeneralEmailAddress;
        public RoleType SalesOffice;
        public RoleType ShippingInquiriesFax;
        public RoleType ShippingInquiriesPhone;
        public RoleType GeneralCorrespondence;
        public RoleType ActiveCustomers;
        public RoleType ActiveEmployees;
        public RoleType ActiveSuppliers;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType SingletonsWhereInternalOrganisation;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaInternalOrganisation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c81441c8-9ac9-440e-a926-c96230b2701f"))
			{
				SingularName = "InternalOrganisation",
				PluralName = "InternalOrganisations",
			};
        }

	}

    public partial class MetaInternalOrganisationRevenue : MetaClass
	{
	    public static MetaInternalOrganisationRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Month;
        public RoleType Year;
        public RoleType Revenue;
        public RoleType Currency;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaInternalOrganisationRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("930565df-e12c-43c3-9679-a2b42d5a8782"))
			{
				SingularName = "InternalOrganisationRevenue",
				PluralName = "InternalOrganisationRevenues",
			};
        }

	}

    public partial class MetaInventoryItemKind : MetaClass
	{
	    public static MetaInventoryItemKind Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType GoodsWhereInventoryItemKind;
        public AssociationType PartsWhereInventoryItemKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaInventoryItemKind(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("759f97a9-3105-49b4-81a0-c94c3700397c"))
			{
				SingularName = "InventoryItemKind",
				PluralName = "InventoryItemKinds",
			};
        }

	}

    public partial class MetaInventoryItemVariance : MetaClass
	{
	    public static MetaInventoryItemVariance Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Quantity;
        public RoleType ItemVarianceAccountingTransaction;
        public RoleType InventoryDate;
        public RoleType Reason;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations
        public AssociationType InventoryItemVersionsWhereInventoryItemVariance;
        public AssociationType InventoryItemWhereInventoryItemVariance;

		// Inherited Associations


		internal MetaInventoryItemVariance(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b00e2650-283f-4326-bdd3-46a2890e2037"))
			{
				SingularName = "InventoryItemVariance",
				PluralName = "InventoryItemVariances",
			};
        }

	}

    public partial class MetaInvestmentAccount : MetaClass
	{
	    public static MetaInvestmentAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType FinancialAccountTransactions;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaInvestmentAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8a06c50b-5951-465e-86b8-43e733f20b90"))
			{
				SingularName = "InvestmentAccount",
				PluralName = "InvestmentAccounts",
			};
        }

	}

    public partial class MetaIncoTerm : MetaClass
	{
	    public static MetaIncoTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType incoTermCity;
        public RoleType IncoTermCountry;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaIncoTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("BAF23654-7A2D-49A6-81F2-309A61363447"))
			{
				SingularName = "IncoTerm",
				PluralName = "IncoTerms",
			};
        }

	}

    public partial class MetaIncoTermType : MetaClass
	{
	    public static MetaIncoTermType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations

		// Inherited Associations
        public AssociationType OrderTermsWhereTermType;
        public AssociationType QuoteTermsWhereTermType;
        public AssociationType AgreementTermsWhereTermType;
        public AssociationType NotificationsWhereTarget;


		internal MetaIncoTermType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5B218A7B-2C46-40D9-8D23-C045904AC083"))
			{
				SingularName = "IncoTermType",
				PluralName = "IncoTermTypes",
			};
        }

	}

    public partial class MetaInvoiceSequence : MetaClass
	{
	    public static MetaInvoiceSequence Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InternalOrganisationsWhereInvoiceSequence;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaInvoiceSequence(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b8e751c-6778-44cb-93a0-d35b86b724e0"))
			{
				SingularName = "InvoiceSequence",
				PluralName = "InvoiceSequences",
			};
        }

	}

    public partial class MetaInvoiceTerm : MetaClass
	{
	    public static MetaInvoiceTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;

		// Defined Associations
        public AssociationType InvoiceVersionsWhereInvoiceTerm;
        public AssociationType InvoiceWhereInvoiceTerm;

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaInvoiceTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a917f763-e54a-4693-bf7b-d8e7aead8fe6"))
			{
				SingularName = "InvoiceTerm",
				PluralName = "InvoiceTerms",
			};
        }

	}

    public partial class MetaInvoiceVatRateItem : MetaClass
	{
	    public static MetaInvoiceVatRateItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType BaseAmount;
        public RoleType VatRates;
        public RoleType VatAmount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InvoiceItemVersionsWhereInvoiceVatRateItem;
        public AssociationType InvoiceItemWhereInvoiceVatRateItem;

		// Inherited Associations


		internal MetaInvoiceVatRateItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6e380347-21e3-4a00-819f-ed11e6882d03"))
			{
				SingularName = "InvoiceVatRateItem",
				PluralName = "InvoiceVatRateItems",
			};
        }

	}

    public partial class MetaItemIssuance : MetaClass
	{
	    public static MetaItemIssuance Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType IssuanceDateTime;
        public RoleType InventoryItem;
        public RoleType Quantity;
        public RoleType ShipmentItem;
        public RoleType PickListItem;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaItemIssuance(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("441f6007-022d-4d77-bc2d-04c7a876e1bd"))
			{
				SingularName = "ItemIssuance",
				PluralName = "ItemIssuances",
			};
        }

	}

    public partial class MetaItemVarianceAccountingTransaction : MetaClass
	{
	    public static MetaItemVarianceAccountingTransaction Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InventoryItemVarianceWhereItemVarianceAccountingTransaction;

		// Inherited Associations


		internal MetaItemVarianceAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4af573b7-a87f-400c-97e4-80bda17376e0"))
			{
				SingularName = "ItemVarianceAccountingTransaction",
				PluralName = "ItemVarianceAccountingTransactions",
			};
        }

	}

    public partial class MetaJournal : MetaClass
	{
	    public static MetaJournal Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType UseAsDefault;
        public RoleType GlPaymentInTransit;
        public RoleType JournalType;
        public RoleType Description;
        public RoleType BlockUnpaidTransactions;
        public RoleType ContraAccount;
        public RoleType PreviousJournalType;
        public RoleType PreviousContraAccount;
        public RoleType JournalEntries;
        public RoleType CloseWhenInBalance;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PaymentMethodsWhereJournal;

		// Inherited Associations


		internal MetaJournal(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d3446420-6d2a-4d18-a6eb-0405da9f7cc5"))
			{
				SingularName = "Journal",
				PluralName = "Journals",
			};
        }

	}

    public partial class MetaJournalEntry : MetaClass
	{
	    public static MetaJournalEntry Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType EntryNumber;
        public RoleType EntryDate;
        public RoleType JournalDate;
        public RoleType JournalEntryDetails;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType JournalWhereJournalEntry;

		// Inherited Associations


		internal MetaJournalEntry(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("11d75a7a-2e86-4430-a6af-2916440c9ecb"))
			{
				SingularName = "JournalEntry",
				PluralName = "JournalEntries",
			};
        }

	}

    public partial class MetaJournalEntryDetail : MetaClass
	{
	    public static MetaJournalEntryDetail Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType GeneralLedgerAccount;
        public RoleType Amount;
        public RoleType Debit;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType JournalEntryWhereJournalEntryDetail;

		// Inherited Associations


		internal MetaJournalEntryDetail(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9ffd634a-27b9-46a5-bf77-4ae25a9b9ebf"))
			{
				SingularName = "JournalEntryDetail",
				PluralName = "JournalEntryDetails",
			};
        }

	}

    public partial class MetaJournalEntryNumber : MetaClass
	{
	    public static MetaJournalEntryNumber Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType JournalType;
        public RoleType Number;
        public RoleType Year;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InternalOrganisationWhereJournalEntryNumber;

		// Inherited Associations


		internal MetaJournalEntryNumber(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c47bf25f-7d16-4dcd-af3b-5e893a1cdd92"))
			{
				SingularName = "JournalEntryNumber",
				PluralName = "JournalEntryNumbers",
			};
        }

	}

    public partial class MetaJournalType : MetaClass
	{
	    public static MetaJournalType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType JournalsWhereJournalType;
        public AssociationType JournalsWherePreviousJournalType;
        public AssociationType JournalEntryNumbersWhereJournalType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaJournalType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7b23440c-d26b-42f5-a94b-e26872e63e7d"))
			{
				SingularName = "JournalType",
				PluralName = "JournalTypes",
			};
        }

	}

    public partial class MetaLegalForm : MetaClass
	{
	    public static MetaLegalForm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InternalOrganisationsWhereLegalForm;
        public AssociationType OrganisationVersionsWhereLegalForm;
        public AssociationType OrganisationsWhereLegalForm;

		// Inherited Associations


		internal MetaLegalForm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("528cf616-6c67-42e1-af69-b5e6cb1192ea"))
			{
				SingularName = "LegalForm",
				PluralName = "LegalForms",
			};
        }

	}

    public partial class MetaLegalTerm : MetaClass
	{
	    public static MetaLegalTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaLegalTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("14a2576c-3ea7-4016-aba2-44172fb7a952"))
			{
				SingularName = "LegalTerm",
				PluralName = "LegalTerms",
			};
        }

	}

    public partial class MetaLetterCorrespondence : MetaClass
	{
	    public static MetaLetterCorrespondence Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType PostalAddresses;
        public RoleType Originators;
        public RoleType Receivers;
        public RoleType IncomingLetter;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaLetterCorrespondence(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("05964e28-2c1d-4837-a887-2255f157e889"))
			{
				SingularName = "LetterCorrespondence",
				PluralName = "LetterCorrespondences",
			};
        }

	}

    public partial class MetaLetterCorrespondenceVersion : MetaClass
	{
	    public static MetaLetterCorrespondenceVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PostalAddresses;
        public RoleType Originators;
        public RoleType Receivers;
        public RoleType IncomingLetter;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType LetterCorrespondenceWhereCurrentVersion;
        public AssociationType LetterCorrespondenceWhereAllVersion;

		// Inherited Associations


		internal MetaLetterCorrespondenceVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("A298A2F8-4D4A-4CBA-B929-75DC5AA9E3D4"))
			{
				SingularName = "LetterCorrespondenceVersion",
				PluralName = "LetterCorrespondenceVersions",
			};
        }

	}

    public partial class MetaLot : MetaClass
	{
	    public static MetaLot Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ExpirationDate;
        public RoleType Quantity;
        public RoleType LotNumber;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InventoryItemVersionsWhereLot;
        public AssociationType InventoryItemsWhereLot;

		// Inherited Associations


		internal MetaLot(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d900e278-7add-4e90-8bea-0a65d03f4fa7"))
			{
				SingularName = "Lot",
				PluralName = "Lots",
			};
        }

	}

    public partial class MetaManifest : MetaClass
	{
	    public static MetaManifest Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaManifest(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("efb6f7a2-edec-40dd-a03a-d4e15abc438d"))
			{
				SingularName = "Manifest",
				PluralName = "Manifests",
			};
        }

	}

    public partial class MetaManufacturerSuggestedRetailPrice : MetaClass
	{
	    public static MetaManufacturerSuggestedRetailPrice Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaManufacturerSuggestedRetailPrice(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d4cfdb68-9128-4afc-8670-192e55115499"))
			{
				SingularName = "ManufacturerSuggestedRetailPrice",
				PluralName = "ManufacturerSuggestedRetailPrices",
			};
        }

	}

    public partial class MetaManufacturingBom : MetaClass
	{
	    public static MetaManufacturingBom Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Part;
        public ConcreteRoleType Instruction;
        public ConcreteRoleType QuantityUsed;
        public ConcreteRoleType ComponentPart;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngineeringChangesWherePartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWhereSubstitutionPartBillOfMaterial;
        public AssociationType PartBillOfMaterialSubstitutesWherePartBillOfMaterial;


		internal MetaManufacturingBom(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("68a0c645-4671-4dda-87a5-53395934a9fc"))
			{
				SingularName = "ManufacturingBom",
				PluralName = "ManufacturingBoms",
			};
        }

	}

    public partial class MetaManufacturingConfiguration : MetaClass
	{
	    public static MetaManufacturingConfiguration Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType InventoryItem;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType ComponentInventoryItem;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaManufacturingConfiguration(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b6c168d6-3d5c-4f5f-b6c6-d348600f1483"))
			{
				SingularName = "ManufacturingConfiguration",
				PluralName = "ManufacturingConfigurations",
			};
        }

	}

    public partial class MetaMaritalStatus : MetaClass
	{
	    public static MetaMaritalStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PersonWhereMaritalStatus;
        public AssociationType PersonVersionWhereMaritalStatus;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaMaritalStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ef6ce14d-87e9-4704-be8b-595329a6bf20"))
			{
				SingularName = "MaritalStatus",
				PluralName = "MaritalStatuses",
			};
        }

	}

    public partial class MetaMarketingMaterial : MetaClass
	{
	    public static MetaMarketingMaterial Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaMarketingMaterial(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6d4739a9-c3c4-4570-a337-49f667c6243b"))
			{
				SingularName = "MarketingMaterial",
				PluralName = "MarketingMaterials",
			};
        }

	}

    public partial class MetaMarketingPackage : MetaClass
	{
	    public static MetaMarketingPackage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Instruction;
        public RoleType ProductsUsedIn;
        public RoleType Product;
        public RoleType Description;
        public RoleType QuantityUsed;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaMarketingPackage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("42adee8e-5994-42e3-afe1-aa3d3089d594"))
			{
				SingularName = "MarketingPackage",
				PluralName = "MarketingPackages",
			};
        }

	}

    public partial class MetaMaterialsUsage : MetaClass
	{
	    public static MetaMaterialsUsage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType ThroughDateTime;
        public ConcreteRoleType EngagementItem;
        public ConcreteRoleType IsBillable;
        public ConcreteRoleType FromDateTime;
        public ConcreteRoleType Description;
        public ConcreteRoleType WorkEffort;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType ServiceEntryBillingsWhereServiceEntry;
        public AssociationType ServiceEntryHeaderWhereServiceEntry;


		internal MetaMaterialsUsage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f77787aa-66af-4d6a-bbe1-ce3d93020185"))
			{
				SingularName = "MaterialsUsage",
				PluralName = "MaterialsUsages",
			};
        }

	}

    public partial class MetaModel : MetaClass
	{
	    public static MetaModel Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations
        public AssociationType BrandWhereModel;

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaModel(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("273e69b7-6cda-44d4-b1d6-605b32a6a70d"))
			{
				SingularName = "Model",
				PluralName = "Models",
			};
        }

	}

    public partial class MetaNeededSkill : MetaClass
	{
	    public static MetaNeededSkill Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SkillLevel;
        public RoleType YearsExperience;
        public RoleType Skill;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType RequestItemVersionsWhereNeededSkill;
        public AssociationType RequestItemsWhereNeededSkill;

		// Inherited Associations


		internal MetaNeededSkill(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5e31a968-5f7d-4109-9821-b94459f13382"))
			{
				SingularName = "NeededSkill",
				PluralName = "NeededSkills",
			};
        }

	}

    public partial class MetaNewsItem : MetaClass
	{
	    public static MetaNewsItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType IsPublished;
        public RoleType Title;
        public RoleType DisplayOrder;
        public RoleType Locale;
        public RoleType LongText;
        public RoleType Text;
        public RoleType Date;
        public RoleType Announcement;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaNewsItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d50ffc20-9e2d-4362-8e3f-b54d7368d487"))
			{
				SingularName = "NewsItem",
				PluralName = "NewsItems",
			};
        }

	}

    public partial class MetaNonSerialisedInventoryItem : MetaClass
	{
	    public static MetaNonSerialisedInventoryItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousNonSerialisedInventoryItemState;
        public RoleType LastNonSerialisedInventoryItemState;
        public RoleType NonSerialisedInventoryItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType QuantityCommittedOut;
        public RoleType QuantityOnHand;
        public RoleType PreviousQuantityOnHand;
        public RoleType AvailableToPromise;
        public RoleType QuantityExpectedIn;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType ProductCharacteristicValues;
        public ConcreteRoleType InventoryItemVariances;
        public ConcreteRoleType Part;
        public ConcreteRoleType Name;
        public ConcreteRoleType Lot;
        public ConcreteRoleType Sku;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType DerivedProductCategories;
        public ConcreteRoleType Good;
        public ConcreteRoleType ProductType;
        public ConcreteRoleType Facility;

		// Defined Associations
        public AssociationType SalesOrderItemVersionsWherePreviousReservedFromInventoryItem;
        public AssociationType SalesOrderItemVersionsWhereReservedFromInventoryItem;
        public AssociationType SalesOrderItemsWherePreviousReservedFromInventoryItem;
        public AssociationType SalesOrderItemsWhereReservedFromInventoryItem;
        public AssociationType ShipmentReceiptsWhereInventoryItem;

		// Inherited Associations
        public AssociationType ItemIssuancesWhereInventoryItem;
        public AssociationType PickListItemsWhereInventoryItem;
        public AssociationType ShipmentItemsWhereInventoryItem;
        public AssociationType WorkEffortInventoryAssignmentsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereComponentInventoryItem;
        public AssociationType WorkEffortVersionsWhereInventoryItemsProduced;
        public AssociationType WorkEffortsWhereInventoryItemsProduced;
        public AssociationType NotificationsWhereTarget;


		internal MetaNonSerialisedInventoryItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5b294591-e20a-4bad-940a-27ae7b2f8770"))
			{
				SingularName = "NonSerialisedInventoryItem",
				PluralName = "NonSerialisedInventoryItems",
			};
        }

	}

    public partial class MetaNonSerialisedInventoryItemState : MetaClass
	{
	    public static MetaNonSerialisedInventoryItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType NonSerialisedInventoryItemsWherePreviousNonSerialisedInventoryItemState;
        public AssociationType NonSerialisedInventoryItemsWhereLastNonSerialisedInventoryItemState;
        public AssociationType NonSerialisedInventoryItemsWhereNonSerialisedInventoryItemState;
        public AssociationType NonSerialisedInventoryItemVersionsWhereNonSerialisedInventoryItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaNonSerialisedInventoryItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("41D19E80-8ABB-4515-AA44-3E0AF1146AE7"))
			{
				SingularName = "NonSerialisedInventoryItemState",
				PluralName = "NonSerialisedInventoryItemStates",
			};
        }

	}

    public partial class MetaNonSerialisedInventoryItemVersion : MetaClass
	{
	    public static MetaNonSerialisedInventoryItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType NonSerialisedInventoryItemState;
        public RoleType QuantityCommittedOut;
        public RoleType QuantityOnHand;
        public RoleType PreviousQuantityOnHand;
        public RoleType AvailableToPromise;
        public RoleType QuantityExpectedIn;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType ProductCharacteristicValues;
        public ConcreteRoleType InventoryItemVariances;
        public ConcreteRoleType Part;
        public ConcreteRoleType Name;
        public ConcreteRoleType Lot;
        public ConcreteRoleType Sku;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType DerivedProductCategories;
        public ConcreteRoleType Good;
        public ConcreteRoleType ProductType;
        public ConcreteRoleType Facility;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType NonSerialisedInventoryItemWhereCurrentVersion;
        public AssociationType NonSerialisedInventoryItemWhereAllVersion;

		// Inherited Associations


		internal MetaNonSerialisedInventoryItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("21C27A88-F99A-4871-B9D3-00C78F648574"))
			{
				SingularName = "NonSerialisedInventoryItemVersion",
				PluralName = "NonSerialisedInventoryItemVersions",
			};
        }

	}

    public partial class MetaNonSerializedInventoryItemObjectState : MetaClass
	{
	    public static MetaNonSerializedInventoryItemObjectState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType NonSerializedInventoryItemStatusesWhereNonSerializedInventoryItemObjectState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaNonSerializedInventoryItemObjectState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9dd17a3f-0e3c-4d87-b840-2f23a96dd165"))
			{
				SingularName = "NonSerializedInventoryItemObjectState",
				PluralName = "NonSerializedInventoryItemObjectStates",
			};
        }

	}

    public partial class MetaNonSerializedInventoryItemStatus : MetaClass
	{
	    public static MetaNonSerializedInventoryItemStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType StartDateTime;
        public RoleType NonSerializedInventoryItemObjectState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaNonSerializedInventoryItemStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("700360b9-56be-4e51-9610-f1e5951dd765"))
			{
				SingularName = "NonSerializedInventoryItemStatus",
				PluralName = "NonSerializedInventoryItemStatuses",
			};
        }

	}

    public partial class MetaOperatingBudgetVersion : MetaClass
	{
	    public static MetaOperatingBudgetVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType BudgetState;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType Description;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;

		// Defined Associations
        public AssociationType OperatingBudgetWhereCurrentVersion;
        public AssociationType OperatingBudgetWhereAllVersion;

		// Inherited Associations


		internal MetaOperatingBudgetVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B6594D57-4A7D-4747-B173-68F326C87E4D"))
			{
				SingularName = "OperatingBudgetVersion",
				PluralName = "OperatingBudgetVersions",
			};
        }

	}

    public partial class MetaOrganisationVersion : MetaClass
	{
	    public static MetaOrganisationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType OrganisationRoles;
        public RoleType ContactsSecurityToken;
        public RoleType ContactsAccessControl;
        public RoleType OwnerUserGroup;
        public RoleType LegalForm;
        public RoleType Name;
        public RoleType ContactsUserGroup;
        public RoleType LogoImage;
        public RoleType TaxNumber;
        public RoleType IndustryClassifications;
        public RoleType OrganisationClassifications;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;

		// Defined Associations
        public AssociationType OrganisationWhereCurrentVersion;
        public AssociationType OrganisationWhereAllVersion;

		// Inherited Associations


		internal MetaOrganisationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("E1AFA103-7032-416B-AC7B-274A7E35381A"))
			{
				SingularName = "OrganisationVersion",
				PluralName = "OrganisationVersions",
			};
        }

	}

    public partial class MetaPartSpecificationVersion : MetaClass
	{
	    public static MetaPartSpecificationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PartSpecificationState;
        public RoleType DocumentationDate;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType PartSpecificationWhereCurrentVersion;
        public AssociationType PartSpecificationWhereAllVersion;

		// Inherited Associations


		internal MetaPartSpecificationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8C6B5DB8-778D-43E0-B7F9-C61E082F468A"))
			{
				SingularName = "PartSpecificationVersion",
				PluralName = "PartSpecificationVersions",
			};
        }

	}

    public partial class MetaPartSpecificationType : MetaClass
	{
	    public static MetaPartSpecificationType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPartSpecificationType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("FE8B234D-3340-404C-ADC6-15D1E103045C"))
			{
				SingularName = "PartSpecificationType",
				PluralName = "PartSpecificationTypes",
			};
        }

	}

    public partial class MetaOwnership : MetaClass
	{
	    public static MetaOwnership Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SerialisedInventoryItemVersionsWhereOwnership;
        public AssociationType SerialisedInventoryItemsWhereOwnership;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOwnership(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4D3DD400-C1D3-4045-8F62-C9ACBCEC06D0"))
			{
				SingularName = "Ownership",
				PluralName = "Ownerships",
			};
        }

	}

    public partial class MetaPersonVersion : MetaClass
	{
	    public static MetaPersonVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PersonRoles;
        public RoleType Salutation;
        public RoleType YTDCommission;
        public RoleType PersonClassifications;
        public RoleType Citizenship;
        public RoleType LastYearsCommission;
        public RoleType GivenName;
        public RoleType Titles;
        public RoleType MothersMaidenName;
        public RoleType BirthDate;
        public RoleType Height;
        public RoleType PersonTrainings;
        public RoleType Gender;
        public RoleType Weight;
        public RoleType Hobbies;
        public RoleType TotalYearsWorkExperience;
        public RoleType Passports;
        public RoleType MaritalStatus;
        public RoleType Picture;
        public RoleType SocialSecurityNumber;
        public RoleType DeceasedDate;
        public RoleType Function;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;

		// Defined Associations
        public AssociationType PersonWhereCurrentVersion;
        public AssociationType PersonWhereAllVersion;

		// Inherited Associations


		internal MetaPersonVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("E42FCDBF-5DEF-4743-BDE8-4028AC6A00A5"))
			{
				SingularName = "PersonVersion",
				PluralName = "PersonVersions",
			};
        }

	}

    public partial class MetaPhoneCommunicationVersion : MetaClass
	{
	    public static MetaPhoneCommunicationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType LeftVoiceMail;
        public RoleType IncomingCall;
        public RoleType Receivers;
        public RoleType Callers;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType PhoneCommunicationWhereCurrentVersion;
        public AssociationType PhoneCommunicationWhereAllVersion;

		// Inherited Associations


		internal MetaPhoneCommunicationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("00022659-5830-4A1F-A463-C135D5B65992"))
			{
				SingularName = "PhoneCommunicationVersion",
				PluralName = "PhoneCommunicationVersions",
			};
        }

	}

    public partial class MetaPickListVersion : MetaClass
	{
	    public static MetaPickListVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PickListState;
        public RoleType CustomerShipmentCorrection;
        public RoleType CreationDate;
        public RoleType PickListItems;
        public RoleType Picker;
        public RoleType ShipToParty;
        public RoleType Store;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType PickListWhereCurrentVersion;
        public AssociationType PickListWhereAllVersion;

		// Inherited Associations


		internal MetaPickListVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("15B7482D-17F1-4184-9C57-222D41215553"))
			{
				SingularName = "PickListVersion",
				PluralName = "PickListVersions",
			};
        }

	}

    public partial class MetaProcessFlow : MetaClass
	{
	    public static MetaProcessFlow Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType StoresWhereProcessFlow;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaProcessFlow(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8172AA9C-73DF-41F9-A36F-A18D77E58471"))
			{
				SingularName = "ProcessFlow",
				PluralName = "ProcessFlows",
			};
        }

	}

    public partial class MetaProductQuoteVersion : MetaClass
	{
	    public static MetaProductQuoteVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType Request;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType ProductQuoteWhereCurrentVersion;
        public AssociationType ProductQuoteWhereAllVersion;

		// Inherited Associations


		internal MetaProductQuoteVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("BA171BD2-7757-4D4B-AEF7-3E9670F2CF38"))
			{
				SingularName = "ProductQuoteVersion",
				PluralName = "ProductQuoteVersions",
			};
        }

	}

    public partial class MetaProposalVersion : MetaClass
	{
	    public static MetaProposalVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType Request;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType ProposalWhereCurrentVersion;
        public AssociationType ProposalWhereAllVersion;

		// Inherited Associations


		internal MetaProposalVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("EC1A374B-E1D8-4808-A767-8E7910117C08"))
			{
				SingularName = "ProposalVersion",
				PluralName = "ProposalVersions",
			};
        }

	}

    public partial class MetaPurchaseInvoiceItemVersion : MetaClass
	{
	    public static MetaPurchaseInvoiceItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseInvoiceItemState;
        public RoleType PurchaseInvoiceItemType;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalInvoiceAdjustment;
        public ConcreteRoleType InvoiceVatRateItems;
        public ConcreteRoleType AdjustmentFor;
        public ConcreteRoleType SerializedInventoryItem;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalInvoiceAdjustmentCustomerCurrency;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType Description;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType PurchaseInvoiceItemWhereCurrentVersion;
        public AssociationType PurchaseInvoiceItemWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseInvoiceItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("E431E1B1-9CF0-4BA2-AF57-E2EF8B8AE711"))
			{
				SingularName = "PurchaseInvoiceItemVersion",
				PluralName = "PurchaseInvoiceItemVersions",
			};
        }

	}

    public partial class MetaPurchaseInvoiceVersion : MetaClass
	{
	    public static MetaPurchaseInvoiceVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseInvoiceState;
        public RoleType PurchaseInvoiceItems;
        public RoleType BilledFromParty;
        public RoleType PurchaseInvoiceType;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType Description;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType BillingAccount;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType InvoiceDate;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType InvoiceNumber;
        public ConcreteRoleType Message;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType PurchaseInvoiceWhereCurrentVersion;
        public AssociationType PurchaseInvoiceWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseInvoiceVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("C23DBDD0-8933-4582-8995-8767EFDA82D5"))
			{
				SingularName = "PurchaseInvoiceVersion",
				PluralName = "PurchaseInvoiceVersions",
			};
        }

	}

    public partial class MetaPurchaseOrderItemVersion : MetaClass
	{
	    public static MetaPurchaseOrderItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseOrderItemState;
        public RoleType QuantityReceived;
        public RoleType Product;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType BudgetItem;
        public ConcreteRoleType PreviousQuantity;
        public ConcreteRoleType QuantityOrdered;
        public ConcreteRoleType Description;
        public ConcreteRoleType CorrespondingPurchaseOrder;
        public ConcreteRoleType TotalOrderAdjustmentCustomerCurrency;
        public ConcreteRoleType TotalOrderAdjustment;
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType AssignedDeliveryDate;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType ShippingInstruction;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Message;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType PurchaseOrderItemWhereCurrentVersion;
        public AssociationType PurchaseOrderItemWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseOrderItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("FB750088-6AB2-4DED-9AC0-4E4E8ABE88A8"))
			{
				SingularName = "PurchaseOrderItemVersion",
				PluralName = "PurchaseOrderItemVersions",
			};
        }

	}

    public partial class MetaPurchaseOrderPaymentState : MetaClass
	{
	    public static MetaPurchaseOrderPaymentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseOrdersWherePreviousPurchaseOrderPaymentState;
        public AssociationType PurchaseOrdersWhereLastPurchaseOrderPaymentState;
        public AssociationType PurchaseOrdersWherePurchaseOrderPaymentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseOrderPaymentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9BF8812F-44BD-411A-9385-09E4EE25B831"))
			{
				SingularName = "PurchaseOrderPaymentState",
				PluralName = "PurchaseOrderPaymentStates",
			};
        }

	}

    public partial class MetaPurchaseOrderShipmentState : MetaClass
	{
	    public static MetaPurchaseOrderShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseOrdersWherePreviousPurchaseOrderShipmentOrderState;
        public AssociationType PurchaseOrdersWhereLastPurchaseOrderShipmentState;
        public AssociationType PurchaseOrdersWherePurchaseOrderShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseOrderShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B7F6899B-2CA6-4A49-B9BA-B7AD3D9077F1"))
			{
				SingularName = "PurchaseOrderShipmentState",
				PluralName = "PurchaseOrderShipmentStates",
			};
        }

	}

    public partial class MetaPurchaseOrderVersion : MetaClass
	{
	    public static MetaPurchaseOrderVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseOrderState;
        public RoleType PurchaseOrderItems;
        public RoleType PreviousTakenViaSupplier;
        public RoleType TakenViaSupplier;
        public RoleType TakenViaContactMechanism;
        public RoleType BillToContactMechanism;
        public RoleType Facility;
        public RoleType ShipToAddress;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType ValidOrderItems;
        public ConcreteRoleType OrderNumber;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType OrderDate;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType PurchaseOrderWhereCurrentVersion;
        public AssociationType PurchaseOrderWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseOrderVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5B8C17C9-17BF-4B80-9246-AF7125EAE976"))
			{
				SingularName = "PurchaseOrderVersion",
				PluralName = "PurchaseOrderVersions",
			};
        }

	}

    public partial class MetaPurchaseReturnVersion : MetaClass
	{
	    public static MetaPurchaseReturnVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseReturnState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType PurchaseReturnWhereCurrentVersion;
        public AssociationType PurchaseReturnWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseReturnVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("07064A1A-8132-49B1-94A6-A2B948B2BBCD"))
			{
				SingularName = "PurchaseReturnVersion",
				PluralName = "PurchaseReturnVersions",
			};
        }

	}

    public partial class MetaPurchaseShipmentVersion : MetaClass
	{
	    public static MetaPurchaseShipmentVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PurchaseShipmentState;
        public RoleType Facility;
        public RoleType PurchaseOrder;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType PurchaseShipmentWhereCurrentVersion;
        public AssociationType PurchaseShipmentWhereAllVersion;

		// Inherited Associations


		internal MetaPurchaseShipmentVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("BC4FAF1C-9ADC-4FAE-BCC5-818DA779CA6E"))
			{
				SingularName = "PurchaseShipmentVersion",
				PluralName = "PurchaseShipmentVersions",
			};
        }

	}

    public partial class MetaQuoteItemVersion : MetaClass
	{
	    public static MetaQuoteItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType QuoteItemState;
        public RoleType InternalComment;
        public RoleType Authorizer;
        public RoleType Deliverable;
        public RoleType Product;
        public RoleType EstimatedDeliveryDate;
        public RoleType RequiredByDate;
        public RoleType UnitOfMeasure;
        public RoleType ProductFeature;
        public RoleType UnitPrice;
        public RoleType Skill;
        public RoleType WorkEffort;
        public RoleType QuoteTerms;
        public RoleType Quantity;
        public RoleType RequestItem;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType QuoteItemWhereCurrentVersion;
        public AssociationType QuoteItemWhereAllVersion;

		// Inherited Associations


		internal MetaQuoteItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6D38838C-CA7A-4ACC-B240-E4A1F3AE2DC9"))
			{
				SingularName = "QuoteItemVersion",
				PluralName = "QuoteItemVersions",
			};
        }

	}

    public partial class MetaRequestForInformationVersion : MetaClass
	{
	    public static MetaRequestForInformationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType RequestForInformationWhereCurrentVersion;
        public AssociationType RequestForInformationWhereAllVersion;

		// Inherited Associations


		internal MetaRequestForInformationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("CB28933F-4308-43FD-B347-F6773EEC16B5"))
			{
				SingularName = "RequestForInformationVersion",
				PluralName = "RequestForInformationVersions",
			};
        }

	}

    public partial class MetaRequestForProposalVersion : MetaClass
	{
	    public static MetaRequestForProposalVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType RequestForProposalWhereCurrentVersion;
        public AssociationType RequestForProposalWhereAllVersion;

		// Inherited Associations


		internal MetaRequestForProposalVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("EB8B32AD-6F37-4E6F-8E20-59E88DA51573"))
			{
				SingularName = "RequestForProposalVersion",
				PluralName = "RequestForProposalVersions",
			};
        }

	}

    public partial class MetaRequestForQuoteVersion : MetaClass
	{
	    public static MetaRequestForQuoteVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType RequestForQuoteWhereCurrentVersion;
        public AssociationType RequestForQuoteWhereAllVersion;

		// Inherited Associations


		internal MetaRequestForQuoteVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("16C260E7-F4F6-4EAD-80F1-4B7EAF29D6E4"))
			{
				SingularName = "RequestForQuoteVersion",
				PluralName = "RequestForQuoteVersions",
			};
        }

	}

    public partial class MetaRequestItemVersion : MetaClass
	{
	    public static MetaRequestItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RequestItemState;
        public RoleType InternalComment;
        public RoleType Description;
        public RoleType Quantity;
        public RoleType UnitOfMeasure;
        public RoleType Requirements;
        public RoleType Deliverable;
        public RoleType ProductFeature;
        public RoleType NeededSkill;
        public RoleType Product;
        public RoleType MaximumAllowedPrice;
        public RoleType RequiredByDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType RequestItemWhereCurrentVersion;
        public AssociationType RequestItemWhereAllVersion;

		// Inherited Associations


		internal MetaRequestItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("CBEEED83-1411-4081-8605-8D2F4628BB52"))
			{
				SingularName = "RequestItemVersion",
				PluralName = "RequestItemVersions",
			};
        }

	}

    public partial class MetaRequirementVersion : MetaClass
	{
	    public static MetaRequirementVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RequirementState;
        public RoleType RequiredByDate;
        public RoleType RequirementType;
        public RoleType Authorizer;
        public RoleType Reason;
        public RoleType Children;
        public RoleType NeededFor;
        public RoleType Originator;
        public RoleType Facility;
        public RoleType ServicedBy;
        public RoleType EstimatedBudget;
        public RoleType Description;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType RequirementWhereCurrentVersion;
        public AssociationType RequirementWhereAllVersion;

		// Inherited Associations


		internal MetaRequirementVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B0A09032-FEC1-4047-8264-8DBD68C281A0"))
			{
				SingularName = "RequirementVersion",
				PluralName = "RequirementVersions",
			};
        }

	}

    public partial class MetaSalesInvoiceItemVersion : MetaClass
	{
	    public static MetaSalesInvoiceItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SalesInvoiceItemState;
        public RoleType ProductFeature;
        public RoleType RequiredProfitMargin;
        public RoleType InitialMarkupPercentage;
        public RoleType MaintainedMarkupPercentage;
        public RoleType Product;
        public RoleType UnitPurchasePrice;
        public RoleType SalesOrderItem;
        public RoleType SalesInvoiceItemType;
        public RoleType SalesRep;
        public RoleType InitialProfitMargin;
        public RoleType MaintainedProfitMargin;
        public RoleType TimeEntries;
        public RoleType RequiredMarkupPercentage;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalInvoiceAdjustment;
        public ConcreteRoleType InvoiceVatRateItems;
        public ConcreteRoleType AdjustmentFor;
        public ConcreteRoleType SerializedInventoryItem;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalInvoiceAdjustmentCustomerCurrency;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType Description;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType SalesInvoiceItemWhereCurrentVersion;
        public AssociationType SalesInvoiceItemWhereAllVersion;

		// Inherited Associations


		internal MetaSalesInvoiceItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("F838CABD-3769-47D8-8623-66D2723B5D1B"))
			{
				SingularName = "SalesInvoiceItemVersion",
				PluralName = "SalesInvoiceItemVersions",
			};
        }

	}

    public partial class MetaSalesInvoiceVersion : MetaClass
	{
	    public static MetaSalesInvoiceVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SalesInvoiceState;
        public RoleType TotalListPrice;
        public RoleType BillToContactMechanism;
        public RoleType PreviousBillToCustomer;
        public RoleType SalesInvoiceType;
        public RoleType InitialProfitMargin;
        public RoleType PaymentMethod;
        public RoleType SalesOrder;
        public RoleType InitialMarkupPercentage;
        public RoleType MaintainedMarkupPercentage;
        public RoleType SalesReps;
        public RoleType Shipment;
        public RoleType MaintainedProfitMargin;
        public RoleType PreviousShipToCustomer;
        public RoleType BillToCustomer;
        public RoleType SalesInvoiceItems;
        public RoleType TotalListPriceCustomerCurrency;
        public RoleType ShipToCustomer;
        public RoleType BilledFromContactMechanism;
        public RoleType TotalPurchasePrice;
        public RoleType SalesChannel;
        public RoleType Customers;
        public RoleType ShipToAddress;
        public RoleType Store;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType Description;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType BillingAccount;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType InvoiceDate;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType InvoiceNumber;
        public ConcreteRoleType Message;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType SalesInvoiceWhereCurrentVersion;
        public AssociationType SalesInvoiceWhereAllVersion;

		// Inherited Associations


		internal MetaSalesInvoiceVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0982A8D9-4F69-4F4A-A7C2-2AC7ABBE9924"))
			{
				SingularName = "SalesInvoiceVersion",
				PluralName = "SalesInvoiceVersions",
			};
        }

	}

    public partial class MetaSalesOrderItemShipmentState : MetaClass
	{
	    public static MetaSalesOrderItemShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrderItemsWherePreviousSalesOrderItemShipmentState;
        public AssociationType SalesOrderItemsWhereLastSalesOrderItemShipmentState;
        public AssociationType SalesOrderItemsWhereSalesOrderItemShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderItemShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4DD8B6F2-9BDE-4832-8927-D55A015A1109"))
			{
				SingularName = "SalesOrderItemShipmentState",
				PluralName = "SalesOrderItemShipmentStates",
			};
        }

	}

    public partial class MetaSalesOrderItemPaymentState : MetaClass
	{
	    public static MetaSalesOrderItemPaymentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrderItemsWherePreviousSalesOrderItemPaymentState;
        public AssociationType SalesOrderItemsWhereLastSalesOrderItemPaymentState;
        public AssociationType SalesOrderItemsWhereSalesOrderItemPaymentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderItemPaymentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("94F8CC92-8937-4AC9-9787-BD00CBCCC458"))
			{
				SingularName = "SalesOrderItemPaymentState",
				PluralName = "SalesOrderItemPaymentStates",
			};
        }

	}

    public partial class MetaSalesOrderItemVersion : MetaClass
	{
	    public static MetaSalesOrderItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SalesOrderItemState;
        public RoleType InitialProfitMargin;
        public RoleType QuantityShortFalled;
        public RoleType OrderedWithFeatures;
        public RoleType MaintainedProfitMargin;
        public RoleType RequiredProfitMargin;
        public RoleType PreviousReservedFromInventoryItem;
        public RoleType QuantityShipNow;
        public RoleType RequiredMarkupPercentage;
        public RoleType QuantityShipped;
        public RoleType ShipToAddress;
        public RoleType QuantityPicked;
        public RoleType PreviousProduct;
        public RoleType UnitPurchasePrice;
        public RoleType ShipToParty;
        public RoleType AssignedShipToAddress;
        public RoleType QuantityReturned;
        public RoleType QuantityReserved;
        public RoleType SalesRep;
        public RoleType AssignedShipToParty;
        public RoleType QuantityPendingShipment;
        public RoleType MaintainedMarkupPercentage;
        public RoleType InitialMarkupPercentage;
        public RoleType ReservedFromInventoryItem;
        public RoleType Product;
        public RoleType ProductFeature;
        public RoleType QuantityRequestsShipping;

		// Inherited Roles
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType BudgetItem;
        public ConcreteRoleType PreviousQuantity;
        public ConcreteRoleType QuantityOrdered;
        public ConcreteRoleType Description;
        public ConcreteRoleType CorrespondingPurchaseOrder;
        public ConcreteRoleType TotalOrderAdjustmentCustomerCurrency;
        public ConcreteRoleType TotalOrderAdjustment;
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType AssignedDeliveryDate;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType ShippingInstruction;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Message;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType SalesOrderItemWhereCurrentVersion;
        public AssociationType SalesOrderItemWhereAllVersion;

		// Inherited Associations


		internal MetaSalesOrderItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("CD97F8F9-C0E8-4E5F-8516-3F9FE6A4F0FC"))
			{
				SingularName = "SalesOrderItemVersion",
				PluralName = "SalesOrderItemVersions",
			};
        }

	}

    public partial class MetaSalesOrderPaymentState : MetaClass
	{
	    public static MetaSalesOrderPaymentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrdersWherePreviousSalesOrderPaymentState;
        public AssociationType SalesOrdersWhereLastSalesOrderPaymentState;
        public AssociationType SalesOrdersWhereSalesOrderPaymentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderPaymentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4F20B041-0124-4264-8B12-37E67A38EA04"))
			{
				SingularName = "SalesOrderPaymentState",
				PluralName = "SalesOrderPaymentStates",
			};
        }

	}

    public partial class MetaSalesOrderShipmentState : MetaClass
	{
	    public static MetaSalesOrderShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrdersWherePreviousSalesShipmentOrderState;
        public AssociationType SalesOrdersWhereLastSalesOrderShipmentState;
        public AssociationType SalesOrdersWhereSalesOrderShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9FEE8DC0-E418-4E10-AD5A-2E2A33AFC0E9"))
			{
				SingularName = "SalesOrderShipmentState",
				PluralName = "SalesOrderShipmentStates",
			};
        }

	}

    public partial class MetaSalesOrderVersion : MetaClass
	{
	    public static MetaSalesOrderVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SalesOrderState;
        public RoleType TakenByContactMechanism;
        public RoleType ShipToCustomer;
        public RoleType BillToCustomer;
        public RoleType TotalPurchasePrice;
        public RoleType ShipmentMethod;
        public RoleType TotalListPriceCustomerCurrency;
        public RoleType MaintainedProfitMargin;
        public RoleType ShipToAddress;
        public RoleType PreviousShipToCustomer;
        public RoleType BillToContactMechanism;
        public RoleType SalesReps;
        public RoleType InitialProfitMargin;
        public RoleType TotalListPrice;
        public RoleType PartiallyShip;
        public RoleType Customers;
        public RoleType Store;
        public RoleType MaintainedMarkupPercentage;
        public RoleType BillFromContactMechanism;
        public RoleType PaymentMethod;
        public RoleType PlacingContactMechanism;
        public RoleType PreviousBillToCustomer;
        public RoleType SalesChannel;
        public RoleType PlacingCustomer;
        public RoleType ProformaInvoice;
        public RoleType SalesOrderItems;
        public RoleType InitialMarkupPercentage;
        public RoleType Quote;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType ValidOrderItems;
        public ConcreteRoleType OrderNumber;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType OrderDate;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType CapitalBudgetWhereCurrentVersion;
        public AssociationType CapitalBudgetWhereAllVersion;
        public AssociationType SalesOrderWhereCurrentVersion;
        public AssociationType SalesOrderWhereAllVersion;

		// Inherited Associations


		internal MetaSalesOrderVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("FDD29AA9-D2F5-4FA0-8F32-08AD09505577"))
			{
				SingularName = "SalesOrderVersion",
				PluralName = "SalesOrderVersions",
			};
        }

	}

    public partial class MetaSerialisedInventoryItemVersion : MetaClass
	{
	    public static MetaSerialisedInventoryItemVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SerialisedInventoryItemState;
        public RoleType SerialNumber;
        public RoleType Ownership;
        public RoleType Owner;
        public RoleType AcquisitionYear;
        public RoleType ManufacturingYear;
        public RoleType ReplacementValue;
        public RoleType LifeTime;
        public RoleType DepreciationYears;
        public RoleType PurchasePrice;
        public RoleType ExpectedSalesPrice;
        public RoleType RefurbishCost;
        public RoleType TransportCost;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType ProductCharacteristicValues;
        public ConcreteRoleType InventoryItemVariances;
        public ConcreteRoleType Part;
        public ConcreteRoleType Name;
        public ConcreteRoleType Lot;
        public ConcreteRoleType Sku;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType DerivedProductCategories;
        public ConcreteRoleType Good;
        public ConcreteRoleType ProductType;
        public ConcreteRoleType Facility;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType SerialisedInventoryItemWhereCurrentVersion;
        public AssociationType SerialisedInventoryItemWhereAllVersion;

		// Inherited Associations


		internal MetaSerialisedInventoryItemVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("F9111BDF-A0B6-40CB-B33A-0A856B357327"))
			{
				SingularName = "SerialisedInventoryItemVersion",
				PluralName = "SerialisedInventoryItemVersions",
			};
        }

	}

    public partial class MetaNote : MetaClass
	{
	    public static MetaNote Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaNote(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("587e017d-eb9a-412c-bd21-8ff91c42765b"))
			{
				SingularName = "Note",
				PluralName = "Notes",
			};
        }

	}

    public partial class MetaObligation : MetaClass
	{
	    public static MetaObligation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaObligation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a3fe34f9-7dfb-46fe-98ec-ed9a7d14ac19"))
			{
				SingularName = "Obligation",
				PluralName = "Obligations",
			};
        }

	}

    public partial class MetaOneTimeCharge : MetaClass
	{
	    public static MetaOneTimeCharge Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaOneTimeCharge(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5835aca6-214b-41cf-aefe-e361dda026d7"))
			{
				SingularName = "OneTimeCharge",
				PluralName = "OneTimeCharges",
			};
        }

	}

    public partial class MetaOperatingBudget : MetaClass
	{
	    public static MetaOperatingBudget Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Close;
        public MethodType Reopen;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType BudgetRevisions;
        public ConcreteRoleType BudgetNumber;
        public ConcreteRoleType BudgetReviews;
        public ConcreteRoleType BudgetItems;
        public ConcreteRoleType PreviousBudgetState;
        public ConcreteRoleType LastBudgetState;
        public ConcreteRoleType BudgetState;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOperatingBudget(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b5d151c7-0b18-4280-80d1-77b46162dba8"))
			{
				SingularName = "OperatingBudget",
				PluralName = "OperatingBudgets",
			};
        }

	}

    public partial class MetaOrderItemBilling : MetaClass
	{
	    public static MetaOrderItemBilling Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType OrderItem;
        public RoleType SalesInvoiceItem;
        public RoleType Amount;
        public RoleType Quantity;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaOrderItemBilling(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1f14fdb3-9e0f-4cea-b7c7-3ca2ab898f56"))
			{
				SingularName = "OrderItemBilling",
				PluralName = "OrderItemBillings",
			};
        }

	}

    public partial class MetaOrderKind : MetaClass
	{
	    public static MetaOrderKind Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType ScheduleManually;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType OrderVersionsWhereOrderKind;
        public AssociationType OrdersWhereOrderKind;
        public AssociationType PriceComponentsWhereOrderKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOrderKind(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7f13c77f-1ef1-446d-928d-1c96f9fc8b05"))
			{
				SingularName = "OrderKind",
				PluralName = "OrderKinds",
			};
        }

	}

    public partial class MetaOrderQuantityBreak : MetaClass
	{
	    public static MetaOrderQuantityBreak Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ThroughAmount;
        public RoleType FromAmount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWhereOrderQuantityBreak;

		// Inherited Associations


		internal MetaOrderQuantityBreak(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("aa5898e6-71d0-4dcb-9bbd-35ae5cb0e0ef"))
			{
				SingularName = "OrderQuantityBreak",
				PluralName = "OrderQuantityBreaks",
			};
        }

	}

    public partial class MetaOrderRequirementCommitment : MetaClass
	{
	    public static MetaOrderRequirementCommitment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Quantity;
        public RoleType OrderItem;
        public RoleType Requirement;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaOrderRequirementCommitment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2fcdaf95-c3ec-4da2-8e7e-09c55741082f"))
			{
				SingularName = "OrderRequirementCommitment",
				PluralName = "OrderRequirementCommitments",
			};
        }

	}

    public partial class MetaOrderShipment : MetaClass
	{
	    public static MetaOrderShipment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType SalesOrderItem;
        public RoleType Picked;
        public RoleType ShipmentItem;
        public RoleType Quantity;
        public RoleType PurchaseOrderItem;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaOrderShipment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("00be6409-1ca0-491e-b0a1-3d53e17005f6"))
			{
				SingularName = "OrderShipment",
				PluralName = "OrderShipments",
			};
        }

	}

    public partial class MetaOrderTerm : MetaClass
	{
	    public static MetaOrderTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TermValue;
        public RoleType TermType;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType OrderItemVersionsWhereOrderTerm;
        public AssociationType OrderVersionsWhereOrderTerm;
        public AssociationType OrderWhereOrderTerm;
        public AssociationType OrderItemWhereOrderTerm;

		// Inherited Associations


		internal MetaOrderTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("86cf6a28-baeb-479d-ac9e-fabc7fe1994d"))
			{
				SingularName = "OrderTerm",
				PluralName = "OrderTerms",
			};
        }

	}

    public partial class MetaOrderValue : MetaClass
	{
	    public static MetaOrderValue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ThroughAmount;
        public RoleType FromAmount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWhereOrderValue;

		// Inherited Associations


		internal MetaOrderValue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a3ca36e6-960d-4e3a-96d0-6ca1d71d05d7"))
			{
				SingularName = "OrderValue",
				PluralName = "OrderValues",
			};
        }

	}

    public partial class MetaOrdinal : MetaClass
	{
	    public static MetaOrdinal Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartBillOfMaterialSubstitutesWherePreference;
        public AssociationType PartSubstitutesWherePreference;
        public AssociationType SupplierOfferingsWherePreference;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOrdinal(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("385a2ae6-368c-4c3f-ad34-f8d69d8ca6cd"))
			{
				SingularName = "Ordinal",
				PluralName = "Ordinals",
			};
        }

	}

    public partial class MetaOrganisation : MetaClass
	{
	    public static MetaOrganisation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType OrganisationRoles;
        public RoleType ContactsSecurityToken;
        public RoleType ContactsAccessControl;
        public RoleType OwnerUserGroup;
        public RoleType LegalForm;
        public RoleType Name;
        public RoleType ContactsUserGroup;
        public RoleType LogoImage;
        public RoleType TaxNumber;
        public RoleType IndustryClassifications;
        public RoleType OrganisationClassifications;

		// Inherited Roles
        public ConcreteRoleType PartyName;
        public ConcreteRoleType GeneralCorrespondence;
        public ConcreteRoleType YTDRevenue;
        public ConcreteRoleType LastYearsRevenue;
        public ConcreteRoleType BillingInquiriesFax;
        public ConcreteRoleType Qualifications;
        public ConcreteRoleType HomeAddress;
        public ConcreteRoleType InactiveOrganisationContactRelationships;
        public ConcreteRoleType SalesOffice;
        public ConcreteRoleType InactiveContacts;
        public ConcreteRoleType InactivePartyContactMechanisms;
        public ConcreteRoleType OrderInquiriesFax;
        public ConcreteRoleType CurrentSalesReps;
        public ConcreteRoleType PartyContactMechanisms;
        public ConcreteRoleType ShippingInquiriesFax;
        public ConcreteRoleType ShippingInquiriesPhone;
        public ConcreteRoleType BillingAccounts;
        public ConcreteRoleType OrderInquiriesPhone;
        public ConcreteRoleType PartySkills;
        public ConcreteRoleType PartyClassifications;
        public ConcreteRoleType ExcludeFromDunning;
        public ConcreteRoleType BankAccounts;
        public ConcreteRoleType CurrentContacts;
        public ConcreteRoleType BillingAddress;
        public ConcreteRoleType GeneralEmail;
        public ConcreteRoleType DefaultShipmentMethod;
        public ConcreteRoleType Resumes;
        public ConcreteRoleType HeadQuarter;
        public ConcreteRoleType PersonalEmailAddress;
        public ConcreteRoleType CellPhoneNumber;
        public ConcreteRoleType BillingInquiriesPhone;
        public ConcreteRoleType OrderAddress;
        public ConcreteRoleType InternetAddress;
        public ConcreteRoleType Contents;
        public ConcreteRoleType CreditCards;
        public ConcreteRoleType ShippingAddress;
        public ConcreteRoleType CurrentOrganisationContactRelationships;
        public ConcreteRoleType OpenOrderAmount;
        public ConcreteRoleType GeneralFaxNumber;
        public ConcreteRoleType DefaultPaymentMethod;
        public ConcreteRoleType CurrentPartyContactMechanisms;
        public ConcreteRoleType GeneralPhoneNumber;
        public ConcreteRoleType PreferredCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType SimpleMovingAverage;
        public ConcreteRoleType AmountOverDue;
        public ConcreteRoleType DunningType;
        public ConcreteRoleType AmountDue;
        public ConcreteRoleType LastReminderDate;
        public ConcreteRoleType CreditLimit;
        public ConcreteRoleType SubAccountNumber;
        public ConcreteRoleType BlockedForDunning;
        public ConcreteRoleType Agreements;
        public ConcreteRoleType CommunicationEvents;
        public ConcreteRoleType Locale;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType Comment;

		// Defined Associations
        public AssociationType InternalOrganisationWhereCurrentSupplier;
        public AssociationType OrganisationContactRelationshipsWhereOrganisation;
        public AssociationType OrganisationRollUpsWhereParent;
        public AssociationType OrganisationRollUpsWhereChild;
        public AssociationType PositionsWhereOrganisation;
        public AssociationType ProfessionalServicesRelationshipsWhereProfessionalServicesProvider;
        public AssociationType ShipmentRouteSegmentsWhereCarrier;
        public AssociationType SupplierRelationshipsWhereSupplier;
        public AssociationType VatRatesWhereTaxAuthority;
        public AssociationType EstimatedProductCostsWhereOrganisation;

		// Inherited Associations
        public AssociationType CustomerRelationshipsWhereCustomer;
        public AssociationType EngagementsWhereBillToParty;
        public AssociationType EngagementsWherePlacingParty;
        public AssociationType FaceToFaceCommunicationsWhereParticipant;
        public AssociationType FaceToFaceCommunicationVersionsWhereParticipant;
        public AssociationType FaxCommunicationsWhereOriginator;
        public AssociationType FaxCommunicationsWhereReceiver;
        public AssociationType FaxCommunicationVersionsWhereOriginator;
        public AssociationType FaxCommunicationVersionsWhereReceiver;
        public AssociationType GoodsWhereManufacturedBy;
        public AssociationType GoodsWhereSuppliedBy;
        public AssociationType InternalOrganisationWhereCurrentCustomer;
        public AssociationType InternalOrganisationWhereActiveCustomer;
        public AssociationType InternalOrganisationWhereActiveSupplier;
        public AssociationType LetterCorrespondencesWhereOriginator;
        public AssociationType LetterCorrespondencesWhereReceiver;
        public AssociationType LetterCorrespondenceVersionsWhereOriginator;
        public AssociationType LetterCorrespondenceVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereReceiver;
        public AssociationType PhoneCommunicationVersionsWhereCaller;
        public AssociationType PickListVersionsWhereShipToParty;
        public AssociationType PurchaseInvoiceVersionsWhereBilledFromParty;
        public AssociationType PurchaseOrderVersionsWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrderVersionsWhereTakenViaSupplier;
        public AssociationType QuoteItemVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereAuthorizer;
        public AssociationType RequirementVersionsWhereNeededFor;
        public AssociationType RequirementVersionsWhereOriginator;
        public AssociationType RequirementVersionsWhereServicedBy;
        public AssociationType SalesInvoiceVersionsWherePreviousBillToCustomer;
        public AssociationType SalesInvoiceVersionsWherePreviousShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereBillToCustomer;
        public AssociationType SalesInvoiceVersionsWhereShipToCustomer;
        public AssociationType SalesInvoiceVersionsWhereCustomer;
        public AssociationType SalesOrderItemVersionsWhereShipToParty;
        public AssociationType SalesOrderItemVersionsWhereAssignedShipToParty;
        public AssociationType SalesOrderVersionsWhereShipToCustomer;
        public AssociationType SalesOrderVersionsWhereBillToCustomer;
        public AssociationType SalesOrderVersionsWherePreviousShipToCustomer;
        public AssociationType SalesOrderVersionsWhereCustomer;
        public AssociationType SalesOrderVersionsWherePreviousBillToCustomer;
        public AssociationType SalesOrderVersionsWherePlacingCustomer;
        public AssociationType OrganisationGlAccountsWhereParty;
        public AssociationType PartyFixedAssetAssignmentsWhereParty;
        public AssociationType PartyPackageRevenuesWhereParty;
        public AssociationType PartyProductCategoryRevenuesWhereParty;
        public AssociationType PartyProductRevenuesWhereParty;
        public AssociationType PartyRevenuesWhereParty;
        public AssociationType PhoneCommunicationsWhereReceiver;
        public AssociationType PhoneCommunicationsWhereCaller;
        public AssociationType PickListsWhereShipToParty;
        public AssociationType PurchaseInvoicesWhereBilledFromParty;
        public AssociationType PurchaseOrdersWherePreviousTakenViaSupplier;
        public AssociationType PurchaseOrdersWhereTakenViaSupplier;
        public AssociationType QuoteItemsWhereAuthorizer;
        public AssociationType RequirementsWhereAuthorizer;
        public AssociationType RequirementsWhereNeededFor;
        public AssociationType RequirementsWhereOriginator;
        public AssociationType RequirementsWhereServicedBy;
        public AssociationType RespondingPartiesWhereParty;
        public AssociationType SalesInvoicesWherePreviousBillToCustomer;
        public AssociationType SalesInvoicesWherePreviousShipToCustomer;
        public AssociationType SalesInvoicesWhereBillToCustomer;
        public AssociationType SalesInvoicesWhereShipToCustomer;
        public AssociationType SalesInvoicesWhereCustomer;
        public AssociationType SalesOrdersWhereShipToCustomer;
        public AssociationType SalesOrdersWhereBillToCustomer;
        public AssociationType SalesOrdersWherePreviousShipToCustomer;
        public AssociationType SalesOrdersWhereCustomer;
        public AssociationType SalesOrdersWherePreviousBillToCustomer;
        public AssociationType SalesOrdersWherePlacingCustomer;
        public AssociationType SalesOrderItemsWhereShipToParty;
        public AssociationType SalesOrderItemsWhereAssignedShipToParty;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereParty;
        public AssociationType SalesRepPartyRevenuesWhereParty;
        public AssociationType SalesRepRelationshipsWhereCustomer;
        public AssociationType SubContractorRelationshipsWhereContractor;
        public AssociationType SubContractorRelationshipsWhereSubContractor;
        public AssociationType SupplierOfferingsWhereSupplier;
        public AssociationType WebSiteCommunicationVersionsWhereOriginator;
        public AssociationType WebSiteCommunicationVersionsWhereReceiver;
        public AssociationType WebSiteCommunicationsWhereOriginator;
        public AssociationType WebSiteCommunicationsWhereReceiver;
        public AssociationType WorkEffortPartyAssignmentsWhereParty;
        public AssociationType CommunicationEventVersionsWhereToParty;
        public AssociationType CommunicationEventVersionsWhereInvolvedParty;
        public AssociationType CommunicationEventVersionsWhereFromParty;
        public AssociationType CommunicationEventsWhereToParty;
        public AssociationType CommunicationEventsWhereInvolvedParty;
        public AssociationType CommunicationEventsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereFromParty;
        public AssociationType ExternalAccountingTransactionsWhereToParty;
        public AssociationType PartyRelationshipsWhereParty;
        public AssociationType QuoteVersionsWhereReceiver;
        public AssociationType RequestVersionsWhereOriginator;
        public AssociationType PaymentsWhereSendingParty;
        public AssociationType PaymentsWhereReceivingParty;
        public AssociationType QuotesWhereReceiver;
        public AssociationType RequestsWhereOriginator;
        public AssociationType ShipmentVersionsWhereBillToParty;
        public AssociationType ShipmentVersionsWhereShipToParty;
        public AssociationType ShipmentVersionsWhereShipFromParty;
        public AssociationType ShipmentsWhereBillToParty;
        public AssociationType ShipmentsWhereShipToParty;
        public AssociationType ShipmentsWhereShipFromParty;
        public AssociationType NotificationsWhereTarget;


		internal MetaOrganisation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3a5dcec7-308f-48c7-afee-35d38415aa0b"))
			{
				SingularName = "Organisation",
				PluralName = "Organisations",
			};
        }

	}

    public partial class MetaOrganisationContactKind : MetaClass
	{
	    public static MetaOrganisationContactKind Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType OrganisationContactRelationshipsWhereContactKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOrganisationContactKind(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9570d60a-8baa-439c-99f4-472d10952165"))
			{
				SingularName = "OrganisationContactKind",
				PluralName = "OrganisationContactKinds",
			};
        }

	}

    public partial class MetaOrganisationContactRelationship : MetaClass
	{
	    public static MetaOrganisationContactRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Contact;
        public RoleType Organisation;
        public RoleType ContactKinds;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Parties;

		// Defined Associations
        public AssociationType EngagementsWhereTakenViaOrganisationContactRelationship;
        public AssociationType PartyVersionsWhereInactiveOrganisationContactRelationship;
        public AssociationType PartyVersionsWhereCurrentOrganisationContactRelationship;
        public AssociationType PartiesWhereInactiveOrganisationContactRelationship;
        public AssociationType PartiesWhereCurrentOrganisationContactRelationship;

		// Inherited Associations


		internal MetaOrganisationContactRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("956ecb86-097d-43d4-83b5-a7f45ea75448"))
			{
				SingularName = "OrganisationContactRelationship",
				PluralName = "OrganisationContactRelationships",
			};
        }

	}

    public partial class MetaOrganisationGlAccount : MetaClass
	{
	    public static MetaOrganisationGlAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Product;
        public RoleType Parent;
        public RoleType Party;
        public RoleType HasBankStatementTransactions;
        public RoleType ProductCategory;
        public RoleType GeneralLedgerAccount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations
        public AssociationType CostCentersWhereInternalTransferGlAccount;
        public AssociationType CostCentersWhereRedistributedCostsGlAccount;
        public AssociationType JournalsWhereGlPaymentInTransit;
        public AssociationType JournalWhereContraAccount;
        public AssociationType JournalsWherePreviousContraAccount;
        public AssociationType JournalEntryDetailsWhereGeneralLedgerAccount;
        public AssociationType OrganisationGlAccountsWhereParent;
        public AssociationType OrganisationGlAccountBalancesWhereOrganisationGlAccount;
        public AssociationType VatRateWhereVatPayableAccount;
        public AssociationType VatRatesWhereVatToPayAccount;
        public AssociationType VatRatesWhereVatToReceiveAccount;
        public AssociationType VatRateWhereVatReceivableAccount;
        public AssociationType VatRegimesWhereGeneralLedgerAccount;
        public AssociationType PaymentMethodsWhereGlPaymentInTransit;
        public AssociationType PaymentMethodWhereGeneralLedgerAccount;

		// Inherited Associations


		internal MetaOrganisationGlAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("59f3100c-da48-4b4c-a302-1a75e37216a6"))
			{
				SingularName = "OrganisationGlAccount",
				PluralName = "OrganisationGlAccounts",
			};
        }

	}

    public partial class MetaOrganisationGlAccountBalance : MetaClass
	{
	    public static MetaOrganisationGlAccountBalance Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType OrganisationGlAccount;
        public RoleType Amount;
        public RoleType AccountingPeriod;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType AccountingTransactionDetailsWhereOrganisationGlAccountBalance;

		// Inherited Associations


		internal MetaOrganisationGlAccountBalance(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("67a8352d-7fe0-4398-93c3-50ec8d3e8038"))
			{
				SingularName = "OrganisationGlAccountBalance",
				PluralName = "OrganisationGlAccountBalances",
			};
        }

	}

    public partial class MetaOrganisationRole : MetaClass
	{
	    public static MetaOrganisationRole Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType OrganisationVersionsWhereOrganisationRole;
        public AssociationType OrganisationsWhereOrganisationRole;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOrganisationRole(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1E373E12-5636-4D87-AEB6-941BBD10E0B8"))
			{
				SingularName = "OrganisationRole",
				PluralName = "OrganisationRoles",
			};
        }

	}

    public partial class MetaOrganisationRollUp : MetaClass
	{
	    public static MetaOrganisationRollUp Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Parent;
        public RoleType RollupKind;
        public RoleType Child;

		// Inherited Roles
        public ConcreteRoleType Parties;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaOrganisationRollUp(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("316fc0d3-2dce-43aa-9b38-a60f964d5395"))
			{
				SingularName = "OrganisationRollUp",
				PluralName = "OrganisationRollUps",
			};
        }

	}

    public partial class MetaOrganisationUnit : MetaClass
	{
	    public static MetaOrganisationUnit Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType OrganisationRollUpsWhereRollupKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaOrganisationUnit(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c0e14757-9825-4a86-95d9-b87c68efcb9c"))
			{
				SingularName = "OrganisationUnit",
				PluralName = "OrganisationUnits",
			};
        }

	}

    public partial class MetaOwnBankAccount : MetaClass
	{
	    public static MetaOwnBankAccount Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType BankAccount;

		// Inherited Roles
        public ConcreteRoleType BalanceLimit;
        public ConcreteRoleType CurrentBalance;
        public ConcreteRoleType Journal;
        public ConcreteRoleType Description;
        public ConcreteRoleType GlPaymentInTransit;
        public ConcreteRoleType Remarks;
        public ConcreteRoleType GeneralLedgerAccount;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FinancialAccountTransactions;

		// Defined Associations

		// Inherited Associations
        public AssociationType AmountDuesWherePaymentMethod;
        public AssociationType CustomerShipmentsWherePaymentMethod;
        public AssociationType CustomerShipmentVersionsWherePaymentMethod;
        public AssociationType InternalOrganisationWhereActivePaymentMethod;
        public AssociationType InternalOrganisationWhereDefaultPaymentMethod;
        public AssociationType SalesInvoiceVersionsWherePaymentMethod;
        public AssociationType SalesOrderVersionsWherePaymentMethod;
        public AssociationType PayrollPreferencesWherePaymentMethod;
        public AssociationType SalesInvoicesWherePaymentMethod;
        public AssociationType SalesOrdersWherePaymentMethod;
        public AssociationType StoresWhereDefaultPaymentMethod;
        public AssociationType StoresWherePaymentMethod;
        public AssociationType PartyVersionsWhereDefaultPaymentMethod;
        public AssociationType PartiesWhereDefaultPaymentMethod;
        public AssociationType PaymentsWherePaymentMethod;
        public AssociationType NotificationsWhereTarget;


		internal MetaOwnBankAccount(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ca008b8d-584e-4aa5-a759-895b634defc5"))
			{
				SingularName = "OwnBankAccount",
				PluralName = "OwnBankAccounts",
			};
        }

	}

    public partial class MetaOwnCreditCard : MetaClass
	{
	    public static MetaOwnCreditCard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Owner;
        public RoleType CreditCard;

		// Inherited Roles
        public ConcreteRoleType BalanceLimit;
        public ConcreteRoleType CurrentBalance;
        public ConcreteRoleType Journal;
        public ConcreteRoleType Description;
        public ConcreteRoleType GlPaymentInTransit;
        public ConcreteRoleType Remarks;
        public ConcreteRoleType GeneralLedgerAccount;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FinancialAccountTransactions;

		// Defined Associations

		// Inherited Associations
        public AssociationType AmountDuesWherePaymentMethod;
        public AssociationType CustomerShipmentsWherePaymentMethod;
        public AssociationType CustomerShipmentVersionsWherePaymentMethod;
        public AssociationType InternalOrganisationWhereActivePaymentMethod;
        public AssociationType InternalOrganisationWhereDefaultPaymentMethod;
        public AssociationType SalesInvoiceVersionsWherePaymentMethod;
        public AssociationType SalesOrderVersionsWherePaymentMethod;
        public AssociationType PayrollPreferencesWherePaymentMethod;
        public AssociationType SalesInvoicesWherePaymentMethod;
        public AssociationType SalesOrdersWherePaymentMethod;
        public AssociationType StoresWhereDefaultPaymentMethod;
        public AssociationType StoresWherePaymentMethod;
        public AssociationType PartyVersionsWhereDefaultPaymentMethod;
        public AssociationType PartiesWhereDefaultPaymentMethod;
        public AssociationType PaymentsWherePaymentMethod;
        public AssociationType NotificationsWhereTarget;


		internal MetaOwnCreditCard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("23848955-69ae-40ce-b973-0d416ae80c78"))
			{
				SingularName = "OwnCreditCard",
				PluralName = "OwnCreditCards",
			};
        }

	}

    public partial class MetaPackage : MetaClass
	{
	    public static MetaPackage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PackageRevenuesWherePackage;
        public AssociationType PartyPackageRevenuesWherePackage;
        public AssociationType ProductCategoriesWherePackage;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPackage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9371d5fc-748a-4ce4-95eb-6b21aa0ca841"))
			{
				SingularName = "Package",
				PluralName = "Packages",
			};
        }

	}

    public partial class MetaPackageQuantityBreak : MetaClass
	{
	    public static MetaPackageQuantityBreak Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType From;
        public RoleType Through;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWherePackageQuantityBreak;

		// Inherited Associations


		internal MetaPackageQuantityBreak(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d551887b-8520-478d-bf2c-b0f26e3bc356"))
			{
				SingularName = "PackageQuantityBreak",
				PluralName = "PackageQuantityBreaks",
			};
        }

	}

    public partial class MetaPackageRevenue : MetaClass
	{
	    public static MetaPackageRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revenue;
        public RoleType Year;
        public RoleType Month;
        public RoleType Currency;
        public RoleType PackageName;
        public RoleType Package;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPackageRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8bc2d0a0-a371-4292-9fd6-ecb1db838107"))
			{
				SingularName = "PackageRevenue",
				PluralName = "PackageRevenues",
			};
        }

	}

    public partial class MetaPackagingContent : MetaClass
	{
	    public static MetaPackagingContent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ShipmentItem;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ShipmentPackageWherePackagingContent;

		// Inherited Associations


		internal MetaPackagingContent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1c05a785-2de1-48fa-813f-6e740f6f7cec"))
			{
				SingularName = "PackagingContent",
				PluralName = "PackagingContents",
			};
        }

	}

    public partial class MetaPackagingSlip : MetaClass
	{
	    public static MetaPackagingSlip Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaPackagingSlip(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("66e7dcf3-90bc-4ac6-988f-54015f5bef11"))
			{
				SingularName = "PackagingSlip",
				PluralName = "PackagingSlips",
			};
        }

	}

    public partial class MetaPartBillOfMaterialSubstitute : MetaClass
	{
	    public static MetaPartBillOfMaterialSubstitute Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SubstitutionPartBillOfMaterial;
        public RoleType Preference;
        public RoleType Quantity;
        public RoleType PartBillOfMaterial;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaPartBillOfMaterialSubstitute(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5906f4cd-3950-43ee-a3ba-84124c4180f6"))
			{
				SingularName = "PartBillOfMaterialSubstitute",
				PluralName = "PartBillOfMaterialSubstitutes",
			};
        }

	}

    public partial class MetaPartRevision : MetaClass
	{
	    public static MetaPartRevision Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Reason;
        public RoleType SupersededByPart;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartRevision(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("22f87630-11dd-480e-a721-9836af7685b1"))
			{
				SingularName = "PartRevision",
				PluralName = "PartRevisions",
			};
        }

	}

    public partial class MetaPartSpecification : MetaClass
	{
	    public static MetaPartSpecification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Approve;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousPartSpecificationState;
        public RoleType LastPartSpecificationState;
        public RoleType PartSpecificationState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType DocumentationDate;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;

		// Defined Associations
        public AssociationType EngineeringChangesWherePartSpecification;
        public AssociationType PartWherePartSpecification;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPartSpecification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0091574c-edac-4376-8d03-c7e2c2d8132f"))
			{
				SingularName = "PartSpecification",
				PluralName = "PartSpecifications",
			};
        }

	}

    public partial class MetaPartSpecificationState : MetaClass
	{
	    public static MetaPartSpecificationState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartSpecificationVersionsWherePartSpecificationState;
        public AssociationType PartSpecificationsWherePreviousPartSpecificationState;
        public AssociationType PartSpecificationsWhereLastPartSpecificationState;
        public AssociationType PartSpecificationsWherePartSpecificationState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPartSpecificationState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("17b5b8ec-cb0e-4d81-b5e5-1a99a5afff2e"))
			{
				SingularName = "PartSpecificationState",
				PluralName = "PartSpecificationStates",
			};
        }

	}

    public partial class MetaPartSubstitute : MetaClass
	{
	    public static MetaPartSubstitute Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SubstitutionPart;
        public RoleType Preference;
        public RoleType FromDate;
        public RoleType Quantity;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartSubstitute(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c0ea51d6-e9f1-4cb3-80ea-36d8ac4f8a15"))
			{
				SingularName = "PartSubstitute",
				PluralName = "PartSubstitutes",
			};
        }

	}

    public partial class MetaPartyBenefit : MetaClass
	{
	    public static MetaPartyBenefit Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TimeFrequency;
        public RoleType Cost;
        public RoleType ActualEmployerPaidPercentage;
        public RoleType Benefit;
        public RoleType ActualAvailableTime;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyBenefit(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d520cf1a-8d3a-4380-8b88-85cd63a5ad05"))
			{
				SingularName = "PartyBenefit",
				PluralName = "PartyBenefits",
			};
        }

	}

    public partial class MetaPartyContactMechanism : MetaClass
	{
	    public static MetaPartyContactMechanism Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType ContactPurposes;
        public RoleType ContactMechanism;
        public RoleType UseAsDefault;
        public RoleType NonSolicitationIndicator;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType PartyVersionWhereInactivePartyContactMechanism;
        public AssociationType PartyVersionWherePartyContactMechanism;
        public AssociationType PartyVersionWhereCurrentPartyContactMechanism;
        public AssociationType PartyWhereInactivePartyContactMechanism;
        public AssociationType PartyWherePartyContactMechanism;
        public AssociationType PartyWhereCurrentPartyContactMechanism;

		// Inherited Associations


		internal MetaPartyContactMechanism(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ca633037-ba1e-4304-9f2c-3353c287474b"))
			{
				SingularName = "PartyContactMechanism",
				PluralName = "PartyContactMechanisms",
			};
        }

	}

    public partial class MetaPartyFixedAssetAssignment : MetaClass
	{
	    public static MetaPartyFixedAssetAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType FixedAsset;
        public RoleType Party;
        public RoleType AssetAssignmentStatus;
        public RoleType AllocatedCost;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyFixedAssetAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("40ee178e-7564-4dfa-ab6f-8bcd4e62b498"))
			{
				SingularName = "PartyFixedAssetAssignment",
				PluralName = "PartyFixedAssetAssignments",
			};
        }

	}

    public partial class MetaPartyPackageRevenue : MetaClass
	{
	    public static MetaPartyPackageRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Month;
        public RoleType Package;
        public RoleType Party;
        public RoleType Revenue;
        public RoleType Year;
        public RoleType PartyPackageName;
        public RoleType Currency;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyPackageRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("96fe3000-606e-4f88-ba04-87544ef176ca"))
			{
				SingularName = "PartyPackageRevenue",
				PluralName = "PartyPackageRevenues",
			};
        }

	}

    public partial class MetaPartyProductCategoryRevenue : MetaClass
	{
	    public static MetaPartyProductCategoryRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Party;
        public RoleType Revenue;
        public RoleType Month;
        public RoleType Currency;
        public RoleType Year;
        public RoleType PartyProductCategoryName;
        public RoleType ProductCategory;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyProductCategoryRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3f2c4c17-ec80-44ad-b452-76cf694f3d6a"))
			{
				SingularName = "PartyProductCategoryRevenue",
				PluralName = "PartyProductCategoryRevenues",
			};
        }

	}

    public partial class MetaPartyProductRevenue : MetaClass
	{
	    public static MetaPartyProductRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revenue;
        public RoleType Month;
        public RoleType Year;
        public RoleType PartyProductName;
        public RoleType Quantity;
        public RoleType Currency;
        public RoleType Party;
        public RoleType Product;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyProductRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3a0364f4-d872-4c47-a3ef-73d624128693"))
			{
				SingularName = "PartyProductRevenue",
				PluralName = "PartyProductRevenues",
			};
        }

	}

    public partial class MetaPartyRevenue : MetaClass
	{
	    public static MetaPartyRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Currency;
        public RoleType Month;
        public RoleType Party;
        public RoleType Year;
        public RoleType Revenue;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPartyRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6cf7d076-5c39-48b5-a27e-5e7752afee2d"))
			{
				SingularName = "PartyRevenue",
				PluralName = "PartyRevenues",
			};
        }

	}

    public partial class MetaPartySkill : MetaClass
	{
	    public static MetaPartySkill Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType YearsExperience;
        public RoleType StartedUsingDate;
        public RoleType SkillRating;
        public RoleType SkillLevel;
        public RoleType Skill;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PartyVersionsWherePartySkill;
        public AssociationType PartiesWherePartySkill;

		// Inherited Associations


		internal MetaPartySkill(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1d157965-59b5-4ead-b4e4-c722495d7658"))
			{
				SingularName = "PartySkill",
				PluralName = "PartySkills",
			};
        }

	}

    public partial class MetaPassport : MetaClass
	{
	    public static MetaPassport Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType IssueDate;
        public RoleType ExpiriationDate;
        public RoleType Number;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PersonWherePassport;
        public AssociationType CitizenshipWherePassport;
        public AssociationType PersonVersionWherePassport;

		// Inherited Associations


		internal MetaPassport(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("827bc38b-6570-41d7-8ae1-f1bbdf4409f9"))
			{
				SingularName = "Passport",
				PluralName = "Passports",
			};
        }

	}

    public partial class MetaPayCheck : MetaClass
	{
	    public static MetaPayCheck Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Deductions;

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType PaymentMethod;
        public ConcreteRoleType EffectiveDate;
        public ConcreteRoleType SendingParty;
        public ConcreteRoleType PaymentApplications;
        public ConcreteRoleType ReferenceNumber;
        public ConcreteRoleType ReceivingParty;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PerformanceReviewsWhereBonusPayCheck;

		// Inherited Associations
        public AssociationType PaymentBudgetAllocationsWherePayment;
        public AssociationType NotificationsWhereTarget;


		internal MetaPayCheck(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ef5fb351-2f0f-454a-b7b2-104af42b2c72"))
			{
				SingularName = "PayCheck",
				PluralName = "PayChecks",
			};
        }

	}

    public partial class MetaPayGrade : MetaClass
	{
	    public static MetaPayGrade Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType SalarySteps;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaPayGrade(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("028de4a4-12d4-422f-8d82-4f1edaa471ae"))
			{
				SingularName = "PayGrade",
				PluralName = "PayGrades",
			};
        }

	}

    public partial class MetaPayHistory : MetaClass
	{
	    public static MetaPayHistory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TimeFrequency;
        public RoleType SalaryStep;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations
        public AssociationType PerformanceReviewsWherePayHistory;

		// Inherited Associations


		internal MetaPayHistory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("208a5af6-8dd8-4a48-acb2-2ecb89e8d322"))
			{
				SingularName = "PayHistory",
				PluralName = "PayHistories",
			};
        }

	}

    public partial class MetaPaymentApplication : MetaClass
	{
	    public static MetaPaymentApplication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType AmountApplied;
        public RoleType InvoiceItem;
        public RoleType Invoice;
        public RoleType BillingAccount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PaymentWherePaymentApplication;

		// Inherited Associations


		internal MetaPaymentApplication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6fef08f0-d4cb-42f4-a10f-fb31787f65c3"))
			{
				SingularName = "PaymentApplication",
				PluralName = "PaymentApplications",
			};
        }

	}

    public partial class MetaPaymentBudgetAllocation : MetaClass
	{
	    public static MetaPaymentBudgetAllocation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Payment;
        public RoleType BudgetItem;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPaymentBudgetAllocation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2e588028-5de2-411c-ab43-b406ca735d5b"))
			{
				SingularName = "PaymentBudgetAllocation",
				PluralName = "PaymentBudgetAllocations",
			};
        }

	}

    public partial class MetaPayrollPreference : MetaClass
	{
	    public static MetaPayrollPreference Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Percentage;
        public RoleType AccountNumber;
        public RoleType PaymentMethod;
        public RoleType TimeFrequency;
        public RoleType DeductionType;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EmploymentWherePayrollPreference;

		// Inherited Associations


		internal MetaPayrollPreference(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("92f48c0c-31d9-4ed5-8f92-753de6af471a"))
			{
				SingularName = "PayrollPreference",
				PluralName = "PayrollPreferences",
			};
        }

	}

    public partial class MetaPerformanceNote : MetaClass
	{
	    public static MetaPerformanceNote Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType CommunicationDate;
        public RoleType GivenByManager;
        public RoleType Employee;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaPerformanceNote(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4629c7ed-e9a4-4f31-bb46-e3f2920bd768"))
			{
				SingularName = "PerformanceNote",
				PluralName = "PerformanceNotes",
			};
        }

	}

    public partial class MetaPerformanceReview : MetaClass
	{
	    public static MetaPerformanceReview Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Manager;
        public RoleType PayHistory;
        public RoleType BonusPayCheck;
        public RoleType PerformanceReviewItems;
        public RoleType Employee;
        public RoleType ResultingPosition;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaPerformanceReview(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("89c49578-bb5d-4589-b908-bf09c6495011"))
			{
				SingularName = "PerformanceReview",
				PluralName = "PerformanceReviews",
			};
        }

	}

    public partial class MetaPerformanceReviewItem : MetaClass
	{
	    public static MetaPerformanceReviewItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RatingType;
        public RoleType PerformanceReviewItemType;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PerformanceReviewWherePerformanceReviewItem;

		// Inherited Associations


		internal MetaPerformanceReviewItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("962e5149-546b-4b18-ab09-e4de59b709ff"))
			{
				SingularName = "PerformanceReviewItem",
				PluralName = "PerformanceReviewItems",
			};
        }

	}

    public partial class MetaPerformanceReviewItemType : MetaClass
	{
	    public static MetaPerformanceReviewItemType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PerformanceReviewItemsWherePerformanceReviewItemType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPerformanceReviewItemType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e80a9fe3-027b-4abd-acfb-99e3db9da70c"))
			{
				SingularName = "PerformanceReviewItemType",
				PluralName = "PerformanceReviewItemTypes",
			};
        }

	}

    public partial class MetaPersonalTitle : MetaClass
	{
	    public static MetaPersonalTitle Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PeopleWhereTitle;
        public AssociationType PersonVersionsWhereTitle;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPersonalTitle(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1a4166b3-9d9c-427b-a0d8-da53b0e601a2"))
			{
				SingularName = "PersonalTitle",
				PluralName = "PersonalTitles",
			};
        }

	}

    public partial class MetaPersonRole : MetaClass
	{
	    public static MetaPersonRole Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PeopleWherePersonRole;
        public AssociationType PersonVersionsWherePersonRole;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPersonRole(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("38D28E76-2525-4D15-8B58-5C903A855DB2"))
			{
				SingularName = "PersonRole",
				PluralName = "PersonRoles",
			};
        }

	}

    public partial class MetaPersonTraining : MetaClass
	{
	    public static MetaPersonTraining Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Training;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PeopleWherePersonTraining;
        public AssociationType PersonVersionsWherePersonTraining;

		// Inherited Associations


		internal MetaPersonTraining(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6674e32d-c139-4c99-97c5-92354d3ccc4c"))
			{
				SingularName = "PersonTraining",
				PluralName = "PersonTrainings",
			};
        }

	}

    public partial class MetaPhoneCommunication : MetaClass
	{
	    public static MetaPhoneCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType LeftVoiceMail;
        public RoleType IncomingCall;
        public RoleType Receivers;
        public RoleType Callers;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaPhoneCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fcdf4f00-d6f4-493f-a430-89789a3cdef6"))
			{
				SingularName = "PhoneCommunication",
				PluralName = "PhoneCommunications",
			};
        }

	}

    public partial class MetaPickList : MetaClass
	{
	    public static MetaPickList Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Hold;
        public MethodType Continue;
        public MethodType Cancel;
        public MethodType SetPicked;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousPickListState;
        public RoleType LastPickListState;
        public RoleType PickListState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType CustomerShipmentCorrection;
        public RoleType CreationDate;
        public RoleType PickListItems;
        public RoleType Picker;
        public RoleType ShipToParty;
        public RoleType Store;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;

		// Defined Associations

		// Inherited Associations


		internal MetaPickList(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("27b6630a-35d0-4352-9223-b5b6c8d7496b"))
			{
				SingularName = "PickList",
				PluralName = "PickLists",
			};
        }

	}

    public partial class MetaPickListItem : MetaClass
	{
	    public static MetaPickListItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType RequestedQuantity;
        public RoleType InventoryItem;
        public RoleType ActualQuantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ItemIssuancesWherePickListItem;
        public AssociationType PickListVersionWherePickListItem;
        public AssociationType PickListWherePickListItem;

		// Inherited Associations


		internal MetaPickListItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7fec090e-3d4a-4ec7-895f-4b30d01f59bb"))
			{
				SingularName = "PickListItem",
				PluralName = "PickListItems",
			};
        }

	}

    public partial class MetaPickListState : MetaClass
	{
	    public static MetaPickListState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PickListVersionsWherePickListState;
        public AssociationType PickListsWherePreviousPickListState;
        public AssociationType PickListsWhereLastPickListState;
        public AssociationType PickListsWherePickListState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPickListState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f7108ec0-3203-4e62-b323-2e3a6a527d66"))
			{
				SingularName = "PickListState",
				PluralName = "PickListStates",
			};
        }

	}

    public partial class MetaPosition : MetaClass
	{
	    public static MetaPosition Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Organisation;
        public RoleType Temporary;
        public RoleType EstimatedThroughDate;
        public RoleType EstimatedFromDate;
        public RoleType PositionType;
        public RoleType Fulltime;
        public RoleType Salary;
        public RoleType PositionStatus;
        public RoleType ApprovedBudgetItem;
        public RoleType ActualFromDate;
        public RoleType ActualThroughDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EmploymentApplicationsWherePosition;
        public AssociationType PerformanceReviewsWhereResultingPosition;
        public AssociationType PositionFulfillmentsWherePosition;
        public AssociationType PositionReportingStructuresWhereManagedByPosition;
        public AssociationType PositionReportingStructuresWherePosition;
        public AssociationType PositionResponsibilitiesWherePosition;

		// Inherited Associations


		internal MetaPosition(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("04540476-602f-456a-b300-54166b65c8b1"))
			{
				SingularName = "Position",
				PluralName = "Positions",
			};
        }

	}

    public partial class MetaPositionFulfillment : MetaClass
	{
	    public static MetaPositionFulfillment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Position;
        public RoleType Person;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPositionFulfillment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6a03924c-914b-4660-b7e8-5174caa0dff9"))
			{
				SingularName = "PositionFulfillment",
				PluralName = "PositionFulfillments",
			};
        }

	}

    public partial class MetaPositionReportingStructure : MetaClass
	{
	    public static MetaPositionReportingStructure Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Primary;
        public RoleType ManagedByPosition;
        public RoleType Position;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaPositionReportingStructure(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b50d0780-bcbf-4041-8576-164577d40c55"))
			{
				SingularName = "PositionReportingStructure",
				PluralName = "PositionReportingStructures",
			};
        }

	}

    public partial class MetaPositionResponsibility : MetaClass
	{
	    public static MetaPositionResponsibility Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Position;
        public RoleType Responsibility;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaPositionResponsibility(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b0a42c94-3d4e-47f1-86a2-cf45eeba5f0d"))
			{
				SingularName = "PositionResponsibility",
				PluralName = "PositionResponsibilities",
			};
        }

	}

    public partial class MetaPositionStatus : MetaClass
	{
	    public static MetaPositionStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PositionsWherePositionStatus;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPositionStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4250a005-4fec-4118-a5b4-725886c59269"))
			{
				SingularName = "PositionStatus",
				PluralName = "PositionStatuses",
			};
        }

	}

    public partial class MetaPositionType : MetaClass
	{
	    public static MetaPositionType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;
        public RoleType Responsibilities;
        public RoleType BenefitPercentage;
        public RoleType Title;
        public RoleType PositionTypeRate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PositionsWherePositionType;

		// Inherited Associations


		internal MetaPositionType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4d599ed2-c5e3-4c1d-8128-6ff61f9072c3"))
			{
				SingularName = "PositionType",
				PluralName = "PositionTypes",
			};
        }

	}

    public partial class MetaPositionTypeRate : MetaClass
	{
	    public static MetaPositionTypeRate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Rate;
        public RoleType RateType;
        public RoleType TimeFrequency;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PositionTypesWherePositionTypeRate;

		// Inherited Associations


		internal MetaPositionTypeRate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("17b9c8f1-ddf2-4db0-8358-ae66a02395ce"))
			{
				SingularName = "PositionTypeRate",
				PluralName = "PositionTypeRates",
			};
        }

	}

    public partial class MetaPostalAddress : MetaClass
	{
	    public static MetaPostalAddress Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType GeographicBoundaries;
        public RoleType Address1;
        public RoleType Address2;
        public RoleType Address3;
        public RoleType PostalBoundary;
        public RoleType PostalCode;
        public RoleType City;
        public RoleType Country;
        public RoleType Directions;

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType FollowTo;
        public ConcreteRoleType ContactMechanismType;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType InternalOrganisationsWhereShippingAddress;
        public AssociationType InternalOrganisationsWhereGeneralCorrespondence;
        public AssociationType LetterCorrespondencesWherePostalAddress;
        public AssociationType LetterCorrespondenceVersionsWherePostalAddress;
        public AssociationType PurchaseOrderVersionsWhereShipToAddress;
        public AssociationType SalesInvoiceVersionsWhereShipToAddress;
        public AssociationType SalesOrderItemVersionsWhereShipToAddress;
        public AssociationType SalesOrderItemVersionsWhereAssignedShipToAddress;
        public AssociationType SalesOrderVersionsWhereShipToAddress;
        public AssociationType PurchaseOrdersWhereShipToAddress;
        public AssociationType SalesInvoicesWhereShipToAddress;
        public AssociationType SalesOrdersWhereShipToAddress;
        public AssociationType SalesOrderItemsWhereShipToAddress;
        public AssociationType SalesOrderItemsWhereAssignedShipToAddress;
        public AssociationType PartyVersionsWhereGeneralCorrespondence;
        public AssociationType PartyVersionsWhereShippingAddress;
        public AssociationType PartiesWhereGeneralCorrespondence;
        public AssociationType PartiesWhereShippingAddress;
        public AssociationType ShipmentVersionsWhereShipToAddress;
        public AssociationType ShipmentVersionsWhereShipFromAddress;
        public AssociationType ShipmentsWhereShipToAddress;
        public AssociationType ShipmentsWhereShipFromAddress;

		// Inherited Associations
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;
        public AssociationType NotificationsWhereTarget;


		internal MetaPostalAddress(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d54b4bba-a84c-4826-85ba-7340714035c7"))
			{
				SingularName = "PostalAddress",
				PluralName = "PostalAddresses",
			};
        }

	}

    public partial class MetaPostalBoundary : MetaClass
	{
	    public static MetaPostalBoundary Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType PostalCode;
        public RoleType Locality;
        public RoleType Country;
        public RoleType Region;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PostalAddressWherePostalBoundary;

		// Inherited Associations


		internal MetaPostalBoundary(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e94bf9e1-373d-49e3-a0fe-f21a8b1525d4"))
			{
				SingularName = "PostalBoundary",
				PluralName = "PostalBoundaries",
			};
        }

	}

    public partial class MetaPostalCode : MetaClass
	{
	    public static MetaPostalCode Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Code;

		// Inherited Roles
        public ConcreteRoleType Country;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PostalAddressesWherePostalCode;

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaPostalCode(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9d0065b8-2760-4ec5-928a-9ebd128bbfdd"))
			{
				SingularName = "PostalCode",
				PluralName = "PostalCodes",
			};
        }

	}

    public partial class MetaPriority : MetaClass
	{
	    public static MetaPriority Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType CommunicationEventVersionsWherePriority;
        public AssociationType CommunicationEventsWherePriority;
        public AssociationType WorkEffortVersionsWherePriority;
        public AssociationType WorkEffortsWherePriority;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPriority(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("caa4814f-85a2-46a8-97a7-82220f8270cb"))
			{
				SingularName = "Priority",
				PluralName = "Priorities",
			};
        }

	}

    public partial class MetaProductCategory : MetaClass
	{
	    public static MetaProductCategory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Package;
        public RoleType Code;
        public RoleType Parents;
        public RoleType Children;
        public RoleType Description;
        public RoleType Name;
        public RoleType LocalisedNames;
        public RoleType LocalisedDescriptions;
        public RoleType CategoryImage;
        public RoleType SuperJacent;
        public RoleType CatScope;
        public RoleType AllProducts;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType BrandsWhereProductCategory;
        public AssociationType CatalogueWhereProductCategory;
        public AssociationType OrganisationGlAccountsWhereProductCategory;
        public AssociationType PartyProductCategoryRevenuesWhereProductCategory;
        public AssociationType ProductCategoriesWhereParent;
        public AssociationType ProductCategoriesWhereChild;
        public AssociationType ProductCategoriesWhereSuperJacent;
        public AssociationType ProductCategoryRevenuesWhereProductCategory;
        public AssociationType RevenueQuantityBreaksWhereProductCategory;
        public AssociationType RevenueValueBreaksWhereProductCategory;
        public AssociationType SalesRepPartyProductCategoryRevenuesWhereProductCategory;
        public AssociationType SalesRepProductCategoryRevenuesWhereProductCategory;
        public AssociationType SalesRepRelationshipsWhereProductCategory;
        public AssociationType InventoryItemVersionsWhereDerivedProductCategory;
        public AssociationType InventoryItemsWhereDerivedProductCategory;
        public AssociationType PriceComponentsWhereProductCategory;
        public AssociationType ProductsWherePrimaryProductCategory;
        public AssociationType ProductsWhereProductCategoriesExpanded;
        public AssociationType ProductsWhereProductCategory;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaProductCategory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ea83087e-05cc-458c-a6ba-3ce947644a0f"))
			{
				SingularName = "ProductCategory",
				PluralName = "ProductCategories",
			};
        }

	}

    public partial class MetaProductCategoryRevenue : MetaClass
	{
	    public static MetaProductCategoryRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType ProductCategoryName;
        public RoleType Month;
        public RoleType ProductCategory;
        public RoleType Revenue;
        public RoleType Currency;
        public RoleType Year;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaProductCategoryRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c8503ec-3796-4861-af47-b1aa4e911292"))
			{
				SingularName = "ProductCategoryRevenue",
				PluralName = "ProductCategoryRevenues",
			};
        }

	}

    public partial class MetaProductCharacteristic : MetaClass
	{
	    public static MetaProductCharacteristic Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ProductCharacteristicValuesWhereProductCharacteristic;
        public AssociationType ProductTypesWhereProductCharacteristic;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaProductCharacteristic(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5A0B6477-7B54-48FA-AF59-7B664587F197"))
			{
				SingularName = "ProductCharacteristic",
				PluralName = "ProductCharacteristics",
			};
        }

	}

    public partial class MetaProductCharacteristicValue : MetaClass
	{
	    public static MetaProductCharacteristicValue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType ProductCharacteristic;
        public RoleType Value;

		// Inherited Roles
        public ConcreteRoleType Locale;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InventoryItemVersionsWhereProductCharacteristicValue;
        public AssociationType InventoryItemsWhereProductCharacteristicValue;

		// Inherited Associations


		internal MetaProductCharacteristicValue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("C3A647C2-1073-4D8B-99EB-AE5293AADB6B"))
			{
				SingularName = "ProductCharacteristicValue",
				PluralName = "ProductCharacteristicValues",
			};
        }

	}

    public partial class MetaProductConfiguration : MetaClass
	{
	    public static MetaProductConfiguration Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ProductsUsedIn;
        public RoleType Product;
        public RoleType QuantityUsed;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaProductConfiguration(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("23503dae-02ff-4dae-950e-d699dcb12a3c"))
			{
				SingularName = "ProductConfiguration",
				PluralName = "ProductConfigurations",
			};
        }

	}

    public partial class MetaProductDeliverySkillRequirement : MetaClass
	{
	    public static MetaProductDeliverySkillRequirement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType StartedUsingDate;
        public RoleType Service;
        public RoleType Skill;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaProductDeliverySkillRequirement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fd342cb7-53d3-4377-acd8-ee586b924678"))
			{
				SingularName = "ProductDeliverySkillRequirement",
				PluralName = "ProductDeliverySkillRequirements",
			};
        }

	}

    public partial class MetaProductDrawing : MetaClass
	{
	    public static MetaProductDrawing Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaProductDrawing(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1fb8d537-a870-4793-95a1-7742749e16fc"))
			{
				SingularName = "ProductDrawing",
				PluralName = "ProductDrawings",
			};
        }

	}

    public partial class MetaProductFeatureApplicabilityRelationship : MetaClass
	{
	    public static MetaProductFeatureApplicabilityRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType AvailableFor;
        public RoleType UsedToDefine;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaProductFeatureApplicabilityRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("003433eb-a0c6-454d-8517-0c03e9be3e96"))
			{
				SingularName = "ProductFeatureApplicabilityRelationship",
				PluralName = "ProductFeatureApplicabilityRelationships",
			};
        }

	}

    public partial class MetaProductModel : MetaClass
	{
	    public static MetaProductModel Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaProductModel(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("99ea8125-7d86-4cb6-b453-27752c434fc7"))
			{
				SingularName = "ProductModel",
				PluralName = "ProductModels",
			};
        }

	}

    public partial class MetaProductPurchasePrice : MetaClass
	{
	    public static MetaProductPurchasePrice Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Price;
        public RoleType UnitOfMeasure;
        public RoleType Currency;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations
        public AssociationType SupplierOfferingWhereProductPurchasePrice;

		// Inherited Associations


		internal MetaProductPurchasePrice(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4e2d5dee-1dcf-4c14-8acc-d60fd47a3400"))
			{
				SingularName = "ProductPurchasePrice",
				PluralName = "ProductPurchasePrices",
			};
        }

	}

    public partial class MetaProductQuality : MetaClass
	{
	    public static MetaProductQuality Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaProductQuality(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d14fa0d2-8743-4d3c-8109-2ab9161cb310"))
			{
				SingularName = "ProductQuality",
				PluralName = "ProductQualities",
			};
        }

	}

    public partial class MetaProductQuote : MetaClass
	{
	    public static MetaProductQuote Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Order;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Approve;
        public MethodType Reject;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousQuoteState;
        public ConcreteRoleType LastQuoteState;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType Request;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType Comment;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;

		// Defined Associations
        public AssociationType SalesOrderVersionsWhereQuote;
        public AssociationType SalesOrderWhereQuote;

		// Inherited Associations


		internal MetaProductQuote(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c2214ff4-d592-4f0d-9215-e431b23dc9c2"))
			{
				SingularName = "ProductQuote",
				PluralName = "ProductQuotes",
			};
        }

	}

    public partial class MetaProductRevenue : MetaClass
	{
	    public static MetaProductRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revenue;
        public RoleType ProductName;
        public RoleType Currency;
        public RoleType Year;
        public RoleType Product;
        public RoleType Month;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaProductRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a34ca9ef-63e5-48c0-8a62-c8f43ad2d9d9"))
			{
				SingularName = "ProductRevenue",
				PluralName = "ProductRevenues",
			};
        }

	}

    public partial class MetaProductType : MetaClass
	{
	    public static MetaProductType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ProductCharacteristics;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InventoryItemVersionsWhereProductType;
        public AssociationType InventoryItemsWhereProductType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaProductType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6451E06E-747E-4F58-98F5-2F9DC5D787B5"))
			{
				SingularName = "ProductType",
				PluralName = "ProductTypes",
			};
        }

	}

    public partial class MetaProfessionalAssignment : MetaClass
	{
	    public static MetaProfessionalAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Professional;
        public RoleType EngagementItem;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaProfessionalAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9e679821-8eeb-4dce-b090-d8ade95cb47f"))
			{
				SingularName = "ProfessionalAssignment",
				PluralName = "ProfessionalAssignments",
			};
        }

	}

    public partial class MetaProfessionalPlacement : MetaClass
	{
	    public static MetaProfessionalPlacement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType Description;
        public ConcreteRoleType ExpectedStartDate;
        public ConcreteRoleType ExpectedEndDate;
        public ConcreteRoleType EngagementWorkFulfillment;
        public ConcreteRoleType EngagementRates;
        public ConcreteRoleType CurrentEngagementRate;
        public ConcreteRoleType OrderedWiths;
        public ConcreteRoleType CurrentAssignedProfessional;
        public ConcreteRoleType Product;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;


		internal MetaProfessionalPlacement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b83205c5-261f-4d9d-9789-55966ae8d61b"))
			{
				SingularName = "ProfessionalPlacement",
				PluralName = "ProfessionalPlacements",
			};
        }

	}

    public partial class MetaProfessionalServicesRelationship : MetaClass
	{
	    public static MetaProfessionalServicesRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Professional;
        public RoleType ProfessionalServicesProvider;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Parties;

		// Defined Associations

		// Inherited Associations


		internal MetaProfessionalServicesRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a6f772e6-8f2c-4180-bbf9-2e5ab0f0efc8"))
			{
				SingularName = "ProfessionalServicesRelationship",
				PluralName = "ProfessionalServicesRelationships",
			};
        }

	}

    public partial class MetaProperty : MetaClass
	{
	    public static MetaProperty Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType LastServiceDate;
        public ConcreteRoleType AcquiredDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType ProductionCapacity;
        public ConcreteRoleType NextServiceDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType DepreciationsWhereFixedAsset;
        public AssociationType PartyFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetStandardsWhereFixedAsset;
        public AssociationType WorkEffortTypesWhereFixedAssetToRepair;


		internal MetaProperty(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("dc54aafb-f0f2-4f72-8a81-d5b2fc792b86"))
			{
				SingularName = "Property",
				PluralName = "Properties",
			};
        }

	}

    public partial class MetaProposal : MetaClass
	{
	    public static MetaProposal Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Approve;
        public MethodType Reject;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousQuoteState;
        public ConcreteRoleType LastQuoteState;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType Request;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaProposal(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("360cf15d-c360-4d68-b693-7d1544388169"))
			{
				SingularName = "Proposal",
				PluralName = "Proposals",
			};
        }

	}

    public partial class MetaProvince : MetaClass
	{
	    public static MetaProvince Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType Cities;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Country;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaProvince(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ada24931-020a-48e8-8f8d-18ddb8f46cf7"))
			{
				SingularName = "Province",
				PluralName = "Provinces",
			};
        }

	}

    public partial class MetaPurchaseAgreement : MetaClass
	{
	    public static MetaPurchaseAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AgreementDate;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType Text;
        public ConcreteRoleType AgreementItems;
        public ConcreteRoleType AgreementNumber;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1032dc2f-72b7-4ba2-b47d-ba14d52a18c9"))
			{
				SingularName = "PurchaseAgreement",
				PluralName = "PurchaseAgreements",
			};
        }

	}

    public partial class MetaPurchaseInvoice : MetaClass
	{
	    public static MetaPurchaseInvoice Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Ready;
        public MethodType Approve;
        public MethodType Cancel;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousPurchaseInvoiceState;
        public RoleType LastPurchaseInvoiceState;
        public RoleType PurchaseInvoiceState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType PurchaseInvoiceItems;
        public RoleType BilledFromParty;
        public RoleType PurchaseInvoiceType;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType Description;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType BillingAccount;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType InvoiceDate;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType InvoiceNumber;
        public ConcreteRoleType Message;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Locale;
        public ConcreteRoleType Comment;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType PaymentApplicationsWhereInvoice;
        public AssociationType SalesAccountingTransactionWhereInvoice;


		internal MetaPurchaseInvoice(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7d7e4b6d-eebd-460c-b771-a93cd8d64bce"))
			{
				SingularName = "PurchaseInvoice",
				PluralName = "PurchaseInvoices",
			};
        }

	}

    public partial class MetaPurchaseInvoiceItem : MetaClass
	{
	    public static MetaPurchaseInvoiceItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousPurchaseInvoiceItemState;
        public RoleType LastPurchaseInvoiceItemState;
        public RoleType PurchaseInvoiceItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType PurchaseInvoiceItemType;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalInvoiceAdjustment;
        public ConcreteRoleType InvoiceVatRateItems;
        public ConcreteRoleType AdjustmentFor;
        public ConcreteRoleType SerializedInventoryItem;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalInvoiceAdjustmentCustomerCurrency;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType PurchaseInvoiceVersionsWherePurchaseInvoiceItem;
        public AssociationType PurchaseInvoiceWherePurchaseInvoiceItem;

		// Inherited Associations
        public AssociationType PaymentApplicationsWhereInvoiceItem;
        public AssociationType ServiceEntryBillingsWhereInvoiceItem;
        public AssociationType ShipmentItemWhereInvoiceItem;
        public AssociationType WorkEffortBillingsWhereInvoiceItem;
        public AssociationType InvoiceItemVersionsWhereAdjustmentFor;
        public AssociationType InvoiceItemsWhereAdjustmentFor;


		internal MetaPurchaseInvoiceItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1ee19062-e36d-4836-b0e6-928a3957bd57"))
			{
				SingularName = "PurchaseInvoiceItem",
				PluralName = "PurchaseInvoiceItems",
			};
        }

	}

    public partial class MetaPurchaseInvoiceItemState : MetaClass
	{
	    public static MetaPurchaseInvoiceItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePurchaseInvoiceItemState;
        public AssociationType PurchaseInvoiceItemsWherePreviousPurchaseInvoiceItemState;
        public AssociationType PurchaseInvoiceItemsWhereLastPurchaseInvoiceItemState;
        public AssociationType PurchaseInvoiceItemsWherePurchaseInvoiceItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseInvoiceItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a7d98869-b51e-45b4-9403-06094bb61fcf"))
			{
				SingularName = "PurchaseInvoiceItemState",
				PluralName = "PurchaseInvoiceItemStates",
			};
        }

	}

    public partial class MetaPurchaseInvoiceItemType : MetaClass
	{
	    public static MetaPurchaseInvoiceItemType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePurchaseInvoiceItemType;
        public AssociationType PurchaseInvoiceItemsWherePurchaseInvoiceItemType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseInvoiceItemType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("14f7d6d1-ade6-4a3a-a3ef-f614a375180e"))
			{
				SingularName = "PurchaseInvoiceItemType",
				PluralName = "PurchaseInvoiceItemTypes",
			};
        }

	}

    public partial class MetaPurchaseInvoiceState : MetaClass
	{
	    public static MetaPurchaseInvoiceState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseInvoiceVersionsWherePurchaseInvoiceState;
        public AssociationType PurchaseInvoicesWherePreviousPurchaseInvoiceState;
        public AssociationType PurchaseInvoicesWhereLastPurchaseInvoiceState;
        public AssociationType PurchaseInvoicesWherePurchaseInvoiceState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseInvoiceState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c485526-bf9e-42e0-b47e-84552a72589a"))
			{
				SingularName = "PurchaseInvoiceState",
				PluralName = "PurchaseInvoiceStates",
			};
        }

	}

    public partial class MetaPurchaseInvoiceType : MetaClass
	{
	    public static MetaPurchaseInvoiceType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseInvoiceVersionsWherePurchaseInvoiceType;
        public AssociationType PurchaseInvoicesWherePurchaseInvoiceType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseInvoiceType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("18cd7011-e0ed-4f45-a6a8-c28fbf80d95a"))
			{
				SingularName = "PurchaseInvoiceType",
				PluralName = "PurchaseInvoiceTypes",
			};
        }

	}

    public partial class MetaPurchaseOrder : MetaClass
	{
	    public static MetaPurchaseOrder Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Approve;
        public MethodType Reject;
        public MethodType Hold;
        public MethodType Continue;
        public MethodType Confirm;
        public MethodType Cancel;
        public MethodType Complete;

		// Defined Roles
        public RoleType PreviousPurchaseOrderState;
        public RoleType LastPurchaseOrderState;
        public RoleType PurchaseOrderState;
        public RoleType PreviousPurchaseOrderPaymentState;
        public RoleType LastPurchaseOrderPaymentState;
        public RoleType PurchaseOrderPaymentState;
        public RoleType PreviousPurchaseOrderShipmentOrderState;
        public RoleType LastPurchaseOrderShipmentState;
        public RoleType PurchaseOrderShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType PurchaseOrderItems;
        public RoleType PreviousTakenViaSupplier;
        public RoleType TakenViaSupplier;
        public RoleType TakenViaContactMechanism;
        public RoleType BillToContactMechanism;
        public RoleType Facility;
        public RoleType ShipToAddress;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType ValidOrderItems;
        public ConcreteRoleType OrderNumber;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType Message;
        public ConcreteRoleType Description;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType OrderDate;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType Locale;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType PurchaseShipmentVersionsWherePurchaseOrder;
        public AssociationType PurchaseShipmentsWherePurchaseOrder;
        public AssociationType OrderItemVersionsWhereCorrespondingPurchaseOrder;
        public AssociationType OrderItemsWhereCorrespondingPurchaseOrder;

		// Inherited Associations


		internal MetaPurchaseOrder(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("062bd939-9902-4747-a631-99ea10002156"))
			{
				SingularName = "PurchaseOrder",
				PluralName = "PurchaseOrders",
			};
        }

	}

    public partial class MetaPurchaseOrderItem : MetaClass
	{
	    public static MetaPurchaseOrderItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Complete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Confirm;
        public MethodType Approve;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousPurchaseOrderItemState;
        public RoleType LastPurchaseOrderItemState;
        public RoleType PurchaseOrderItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType QuantityReceived;
        public RoleType Product;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType BudgetItem;
        public ConcreteRoleType PreviousQuantity;
        public ConcreteRoleType QuantityOrdered;
        public ConcreteRoleType Description;
        public ConcreteRoleType CorrespondingPurchaseOrder;
        public ConcreteRoleType TotalOrderAdjustmentCustomerCurrency;
        public ConcreteRoleType TotalOrderAdjustment;
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType AssignedDeliveryDate;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType ShippingInstruction;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Message;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType PurchaseOrderVersionsWherePurchaseOrderItem;
        public AssociationType OrderShipmentsWherePurchaseOrderItem;
        public AssociationType PurchaseOrderWherePurchaseOrderItem;

		// Inherited Associations
        public AssociationType SalesOrderItemVersionsWhereOrderedWithFeature;
        public AssociationType OrderItemBillingsWhereOrderItem;
        public AssociationType OrderRequirementCommitmentsWhereOrderItem;
        public AssociationType SalesOrderItemWhereOrderedWithFeature;
        public AssociationType ShipmentReceiptsWhereOrderItem;
        public AssociationType OrderItemVersionsWhereAssociation;
        public AssociationType OrderVersionsWhereValidOrderItem;
        public AssociationType OrderWhereValidOrderItem;
        public AssociationType OrderItemsWhereAssociation;
        public AssociationType WorkEffortVersionsWhereOrderItemFulfillment;
        public AssociationType WorkEffortsWhereOrderItemFulfillment;


		internal MetaPurchaseOrderItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ab648bd0-6e31-4ab0-a9ee-cf4a6f02033d"))
			{
				SingularName = "PurchaseOrderItem",
				PluralName = "PurchaseOrderItems",
			};
        }

	}

    public partial class MetaPurchaseOrderItemState : MetaClass
	{
	    public static MetaPurchaseOrderItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseOrderItemVersionsWherePurchaseOrderItemState;
        public AssociationType PurchaseOrderItemsWherePreviousPurchaseOrderItemState;
        public AssociationType PurchaseOrderItemsWhereLastPurchaseOrderItemState;
        public AssociationType PurchaseOrderItemsWherePurchaseOrderItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseOrderItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ad76acee-eccc-42ce-9897-8c3f0252caf4"))
			{
				SingularName = "PurchaseOrderItemState",
				PluralName = "PurchaseOrderItemStates",
			};
        }

	}

    public partial class MetaPurchaseOrderState : MetaClass
	{
	    public static MetaPurchaseOrderState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseOrderVersionsWherePurchaseOrderState;
        public AssociationType PurchaseOrdersWherePreviousPurchaseOrderState;
        public AssociationType PurchaseOrdersWhereLastPurchaseOrderState;
        public AssociationType PurchaseOrdersWherePurchaseOrderState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseOrderState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("45e4f0da-9a6b-4077-bcc4-d49d9ec4cc97"))
			{
				SingularName = "PurchaseOrderState",
				PluralName = "PurchaseOrderStates",
			};
        }

	}

    public partial class MetaPurchaseReturn : MetaClass
	{
	    public static MetaPurchaseReturn Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousPurchaseReturnState;
        public RoleType LastPurchaseReturnState;
        public RoleType PurchaseReturnState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaPurchaseReturn(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a0cf565a-2dcf-4513-9110-8c34468d993f"))
			{
				SingularName = "PurchaseReturn",
				PluralName = "PurchaseReturns",
			};
        }

	}

    public partial class MetaPurchaseReturnState : MetaClass
	{
	    public static MetaPurchaseReturnState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseReturnVersionsWherePurchaseReturnState;
        public AssociationType PurchaseReturnsWherePreviousPurchaseReturnState;
        public AssociationType PurchaseReturnsWhereLastPurchaseReturnState;
        public AssociationType PurchaseReturnsWherePurchaseReturnState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseReturnState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("23162c0f-f5ec-45a5-a948-13a3355d99f2"))
			{
				SingularName = "PurchaseReturnState",
				PluralName = "PurchaseReturnStates",
			};
        }

	}

    public partial class MetaPurchaseShipment : MetaClass
	{
	    public static MetaPurchaseShipment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousPurchaseShipmentState;
        public RoleType LastPurchaseShipmentState;
        public RoleType PurchaseShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType Facility;
        public RoleType PurchaseOrder;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaPurchaseShipment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2bf859c6-de64-476f-a437-5eb57a778262"))
			{
				SingularName = "PurchaseShipment",
				PluralName = "PurchaseShipments",
			};
        }

	}

    public partial class MetaPurchaseShipmentState : MetaClass
	{
	    public static MetaPurchaseShipmentState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PurchaseShipmentVersionsWherePurchaseShipmentState;
        public AssociationType PurchaseShipmentsWherePreviousPurchaseShipmentState;
        public AssociationType PurchaseShipmentsWhereLastPurchaseShipmentState;
        public AssociationType PurchaseShipmentsWherePurchaseShipmentState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaPurchaseShipmentState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("21840af7-e7e7-4e8d-a720-3ea7ee5d2bfd"))
			{
				SingularName = "PurchaseShipmentState",
				PluralName = "PurchaseShipmentStates",
			};
        }

	}

    public partial class MetaQualification : MetaClass
	{
	    public static MetaQualification Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartyVersionsWhereQualification;
        public AssociationType PartiesWhereQualification;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaQualification(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c8077ff8-f443-44b5-93f5-15ad7f4a258d"))
			{
				SingularName = "Qualification",
				PluralName = "Qualifications",
			};
        }

	}

    public partial class MetaQuoteItem : MetaClass
	{
	    public static MetaQuoteItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Cancel;
        public MethodType Submit;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousQuoteItemState;
        public RoleType LastQuoteItemState;
        public RoleType QuoteItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType InternalComment;
        public RoleType Authorizer;
        public RoleType Deliverable;
        public RoleType Product;
        public RoleType EstimatedDeliveryDate;
        public RoleType RequiredByDate;
        public RoleType UnitOfMeasure;
        public RoleType ProductFeature;
        public RoleType UnitPrice;
        public RoleType Skill;
        public RoleType WorkEffort;
        public RoleType QuoteTerms;
        public RoleType Quantity;
        public RoleType RequestItem;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType EngagementItemsWhereQuoteItem;
        public AssociationType OrderItemVersionsWhereQuoteItem;
        public AssociationType QuoteVersionsWhereQuoteItem;
        public AssociationType OrderItemsWhereQuoteItem;
        public AssociationType QuoteWhereQuoteItem;

		// Inherited Associations


		internal MetaQuoteItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("01fc58a0-89b8-4dc0-97f9-5f628b9c9577"))
			{
				SingularName = "QuoteItem",
				PluralName = "QuoteItems",
			};
        }

	}

    public partial class MetaQuoteTerm : MetaClass
	{
	    public static MetaQuoteTerm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TermValue;
        public RoleType TermType;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType QuoteItemVersionWhereQuoteTerm;
        public AssociationType QuoteItemWhereQuoteTerm;
        public AssociationType TimeEntriesWhereQuoteTerm;
        public AssociationType QuoteVersionsWhereQuoteTerm;
        public AssociationType QuotesWhereQuoteTerm;

		// Inherited Associations


		internal MetaQuoteTerm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("cd60cf6d-65ba-4e31-b85d-16c19fc0978b"))
			{
				SingularName = "QuoteTerm",
				PluralName = "QuoteTerms",
			};
        }

	}

    public partial class MetaRateType : MetaClass
	{
	    public static MetaRateType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PositionTypeRatesWhereRateType;
        public AssociationType WorkEffortAssignmentRatesWhereRateType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaRateType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("096448e3-991d-481e-b323-39064387141c"))
			{
				SingularName = "RateType",
				PluralName = "RateTypes",
			};
        }

	}

    public partial class MetaRatingType : MetaClass
	{
	    public static MetaRatingType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType EngagementRatesWhereRatingType;
        public AssociationType PerformanceReviewItemsWhereRatingType;
        public AssociationType SupplierOfferingsWhereRating;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaRatingType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("17d7e31c-9b12-4e0b-a3a7-e687e3991e23"))
			{
				SingularName = "RatingType",
				PluralName = "RatingTypes",
			};
        }

	}

    public partial class MetaRawMaterial : MetaClass
	{
	    public static MetaRawMaterial Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType PartSpecifications;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType Documents;
        public ConcreteRoleType ManufacturerId;
        public ConcreteRoleType ReorderLevel;
        public ConcreteRoleType ReorderQuantity;
        public ConcreteRoleType PriceComponents;
        public ConcreteRoleType InventoryItemKind;
        public ConcreteRoleType Sku;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePart;
        public AssociationType PurchaseOrderItemVersionsWherePart;
        public AssociationType PartRevisionsWhereSupersededByPart;
        public AssociationType PartRevisionsWherePart;
        public AssociationType PartSubstitutesWhereSubstitutionPart;
        public AssociationType PartSubstitutesWherePart;
        public AssociationType PurchaseInvoiceItemsWherePart;
        public AssociationType PurchaseOrderItemsWherePart;
        public AssociationType ShipmentItemsWherePart;
        public AssociationType SupplierOfferingsWherePart;
        public AssociationType WorkEffortPartStandardsWherePart;
        public AssociationType InventoryItemVersionsWherePart;
        public AssociationType InventoryItemsWherePart;
        public AssociationType PartBillOfMaterialsWherePart;
        public AssociationType PartBillOfMaterialsWhereComponentPart;
        public AssociationType NotificationsWhereTarget;


		internal MetaRawMaterial(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9a484067-2003-42f1-b4c4-877e519bb8be"))
			{
				SingularName = "RawMaterial",
				PluralName = "RawMaterials",
			};
        }

	}

    public partial class MetaReceipt : MetaClass
	{
	    public static MetaReceipt Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType PaymentMethod;
        public ConcreteRoleType EffectiveDate;
        public ConcreteRoleType SendingParty;
        public ConcreteRoleType PaymentApplications;
        public ConcreteRoleType ReferenceNumber;
        public ConcreteRoleType ReceivingParty;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType DepositWhereReceipt;
        public AssociationType ReceiptAccountingTransactionWhereReceipt;

		// Inherited Associations
        public AssociationType PaymentBudgetAllocationsWherePayment;
        public AssociationType NotificationsWhereTarget;


		internal MetaReceipt(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("592260cc-365c-4769-b067-e95dd49609f5"))
			{
				SingularName = "Receipt",
				PluralName = "Receipts",
			};
        }

	}

    public partial class MetaReceiptAccountingTransaction : MetaClass
	{
	    public static MetaReceiptAccountingTransaction Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Receipt;

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaReceiptAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1a5195d6-8fff-4590-afe1-3f50c4fa0c67"))
			{
				SingularName = "ReceiptAccountingTransaction",
				PluralName = "ReceiptAccountingTransactions",
			};
        }

	}

    public partial class MetaRecurringCharge : MetaClass
	{
	    public static MetaRecurringCharge Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TimeFrequency;

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaRecurringCharge(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a71e670c-f089-4ec1-8295-dda8e7b62a19"))
			{
				SingularName = "RecurringCharge",
				PluralName = "RecurringCharges",
			};
        }

	}

    public partial class MetaRegion : MetaClass
	{
	    public static MetaRegion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType Associations;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaRegion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("62693ee8-1fd3-4b2b-85ce-8d88df3bba0c"))
			{
				SingularName = "Region",
				PluralName = "Regions",
			};
        }

	}

    public partial class MetaRequestForInformation : MetaClass
	{
	    public static MetaRequestForInformation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Submit;
        public MethodType Hold;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousRequestState;
        public ConcreteRoleType LastRequestState;
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PrintContent;

		// Defined Associations

		// Inherited Associations
        public AssociationType QuoteVersionsWhereRequest;
        public AssociationType QuoteWhereRequest;


		internal MetaRequestForInformation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("eab85f26-c3f4-4f47-97dc-8f9429856c00"))
			{
				SingularName = "RequestForInformation",
				PluralName = "RequestsForInformation",
			};
        }

	}

    public partial class MetaRequestForProposal : MetaClass
	{
	    public static MetaRequestForProposal Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType CreateProposal;

		// Inherited Methods
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Submit;
        public MethodType Hold;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousRequestState;
        public ConcreteRoleType LastRequestState;
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PrintContent;

		// Defined Associations

		// Inherited Associations
        public AssociationType QuoteVersionsWhereRequest;
        public AssociationType QuoteWhereRequest;


		internal MetaRequestForProposal(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0112ddd0-14de-43e2-97d3-981766dd957e"))
			{
				SingularName = "RequestForProposal",
				PluralName = "RequestsForProposal",
			};
        }

	}

    public partial class MetaRequestForQuote : MetaClass
	{
	    public static MetaRequestForQuote Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType CreateQuote;

		// Inherited Methods
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Submit;
        public MethodType Hold;
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousRequestState;
        public ConcreteRoleType LastRequestState;
        public ConcreteRoleType RequestState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType Description;
        public ConcreteRoleType RequestDate;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType RequestItems;
        public ConcreteRoleType RequestNumber;
        public ConcreteRoleType RespondingParties;
        public ConcreteRoleType Originator;
        public ConcreteRoleType Currency;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType EmailAddress;
        public ConcreteRoleType TelephoneNumber;
        public ConcreteRoleType TelephoneCountryCode;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PrintContent;

		// Defined Associations

		// Inherited Associations
        public AssociationType QuoteVersionsWhereRequest;
        public AssociationType QuoteWhereRequest;


		internal MetaRequestForQuote(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("874dfe70-2e50-4861-b26d-dc55bc8fa0d0"))
			{
				SingularName = "RequestForQuote",
				PluralName = "RequestsForQuote",
			};
        }

	}

    public partial class MetaRequestItem : MetaClass
	{
	    public static MetaRequestItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Cancel;
        public MethodType Submit;
        public MethodType Hold;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousRequestItemState;
        public RoleType LastRequestItemState;
        public RoleType RequestItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType InternalComment;
        public RoleType Description;
        public RoleType Quantity;
        public RoleType UnitOfMeasure;
        public RoleType Requirements;
        public RoleType Deliverable;
        public RoleType ProductFeature;
        public RoleType NeededSkill;
        public RoleType Product;
        public RoleType MaximumAllowedPrice;
        public RoleType RequiredByDate;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;

		// Defined Associations
        public AssociationType QuoteItemVersionsWhereRequestItem;
        public AssociationType QuoteItemsWhereRequestItem;
        public AssociationType RequestVersionWhereRequestItem;
        public AssociationType RequestWhereRequestItem;

		// Inherited Associations


		internal MetaRequestItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("daf83fcc-832e-4d5e-ba71-5a08f42355db"))
			{
				SingularName = "RequestItem",
				PluralName = "RequestItems",
			};
        }

	}

    public partial class MetaQuoteState : MetaClass
	{
	    public static MetaQuoteState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType QuoteVersionsWhereQuoteState;
        public AssociationType QuotesWherePreviousQuoteState;
        public AssociationType QuotesWhereLastQuoteState;
        public AssociationType QuotesWhereQuoteState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaQuoteState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("71CD083D-8928-4797-8332-0023A8855A66"))
			{
				SingularName = "QuoteState",
				PluralName = "QuoteStates",
			};
        }

	}

    public partial class MetaQuoteItemState : MetaClass
	{
	    public static MetaQuoteItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType QuoteItemVersionsWhereQuoteItemState;
        public AssociationType QuoteItemsWherePreviousQuoteItemState;
        public AssociationType QuoteItemsWhereLastQuoteItemState;
        public AssociationType QuoteItemsWhereQuoteItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaQuoteItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("{E60AA554-EAD1-43C8-999F-E6413E104192}"))
			{
				SingularName = "QuoteItemState",
				PluralName = "QuoteItemStates",
			};
        }

	}

    public partial class MetaRequirement : MetaClass
	{
	    public static MetaRequirement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Reopen;
        public MethodType Cancel;
        public MethodType Hold;
        public MethodType Close;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousRequirementState;
        public RoleType LastRequirementState;
        public RoleType RequirementState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType RequiredByDate;
        public RoleType RequirementType;
        public RoleType Authorizer;
        public RoleType Reason;
        public RoleType Children;
        public RoleType NeededFor;
        public RoleType Originator;
        public RoleType Facility;
        public RoleType ServicedBy;
        public RoleType EstimatedBudget;
        public RoleType Description;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType RequestItemVersionsWhereRequirement;
        public AssociationType RequirementVersionWhereChild;
        public AssociationType OrderRequirementCommitmentsWhereRequirement;
        public AssociationType RequestItemsWhereRequirement;
        public AssociationType RequirementWhereChild;
        public AssociationType RequirementBudgetAllocationsWhereRequirement;
        public AssociationType RequirementCommunicationsWhereRequirement;
        public AssociationType WorkEffortVersionsWhereRequirementFulfillment;
        public AssociationType WorkEffortsWhereRequirementFulfillment;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaRequirement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d3f90525-b7fe-4f81-bccd-adf4f57260bc"))
			{
				SingularName = "Requirement",
				PluralName = "Requirements",
			};
        }

	}

    public partial class MetaRequirementBudgetAllocation : MetaClass
	{
	    public static MetaRequirementBudgetAllocation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType BudgetItem;
        public RoleType Requirement;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaRequirementBudgetAllocation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5990c1d7-02d5-4e0d-8073-657b0dbfc5e1"))
			{
				SingularName = "RequirementBudgetAllocation",
				PluralName = "RequirementBudgetAllocations",
			};
        }

	}

    public partial class MetaRequirementCommunication : MetaClass
	{
	    public static MetaRequirementCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType CommunicationEvent;
        public RoleType Requirement;
        public RoleType AssociatedProfessional;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaRequirementCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("49cdc4a2-f7af-43c9-b160-4c7da9a0ca42"))
			{
				SingularName = "RequirementCommunication",
				PluralName = "RequirementCommunications",
			};
        }

	}

    public partial class MetaRequirementState : MetaClass
	{
	    public static MetaRequirementState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType RequirementVersionsWhereRequirementState;
        public AssociationType RequirementsWherePreviousRequirementState;
        public AssociationType RequirementsWhereLastRequirementState;
        public AssociationType RequirementsWhereRequirementState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaRequirementState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b1ee7191-544e-4cee-bbb1-d64364eb7137"))
			{
				SingularName = "RequirementState",
				PluralName = "RequirementStates",
			};
        }

	}

    public partial class MetaRespondingParty : MetaClass
	{
	    public static MetaRespondingParty Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SendingDate;
        public RoleType ContactMechanism;
        public RoleType Party;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType RequestVersionsWhereRespondingParty;
        public AssociationType RequestsWhereRespondingParty;

		// Inherited Associations


		internal MetaRespondingParty(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4b1e9776-8851-4a2a-a402-1b40211d1f3b"))
			{
				SingularName = "RespondingParty",
				PluralName = "RespondingParties",
			};
        }

	}

    public partial class MetaResponsibility : MetaClass
	{
	    public static MetaResponsibility Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PositionResponsibilitiesWhereResponsibility;
        public AssociationType PositionTypesWhereResponsibility;

		// Inherited Associations


		internal MetaResponsibility(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3aa7bf17-bd02-4587-9006-177845ae69df"))
			{
				SingularName = "Responsibility",
				PluralName = "Responsibilities",
			};
        }

	}

    public partial class MetaResume : MetaClass
	{
	    public static MetaResume Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ResumeDate;
        public RoleType ResumeText;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PartyVersionWhereResume;
        public AssociationType PartyWhereResume;

		// Inherited Associations


		internal MetaResume(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4f7703b0-7201-4f7a-a0b4-f177d64a2c31"))
			{
				SingularName = "Resume",
				PluralName = "Resumes",
			};
        }

	}

    public partial class MetaRevenueQuantityBreak : MetaClass
	{
	    public static MetaRevenueQuantityBreak Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ProductCategory;
        public RoleType Through;
        public RoleType From;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWhereRevenueQuantityBreak;

		// Inherited Associations


		internal MetaRevenueQuantityBreak(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ce394ad6-1229-4621-8506-5f0347cd8c92"))
			{
				SingularName = "RevenueQuantityBreak",
				PluralName = "RevenueQuantityBreaks",
			};
        }

	}

    public partial class MetaRevenueValueBreak : MetaClass
	{
	    public static MetaRevenueValueBreak Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ProductCategory;
        public RoleType ThroughAmount;
        public RoleType FromAmount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PriceComponentsWhereRevenueValueBreak;

		// Inherited Associations


		internal MetaRevenueValueBreak(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("32f8ea23-5ef9-4d2c-86d9-b6f67529c05d"))
			{
				SingularName = "RevenueValueBreak",
				PluralName = "RevenueValueBreaks",
			};
        }

	}

    public partial class MetaSalaryStep : MetaClass
	{
	    public static MetaSalaryStep Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ModifiedDate;
        public RoleType Amount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PayGradeWhereSalaryStep;
        public AssociationType PayHistoriesWhereSalaryStep;

		// Inherited Associations


		internal MetaSalaryStep(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6ebf4c66-dd19-494f-8081-67d7a10a16fc"))
			{
				SingularName = "SalaryStep",
				PluralName = "SalarySteps",
			};
        }

	}

    public partial class MetaSalesAccountingTransaction : MetaClass
	{
	    public static MetaSalesAccountingTransaction Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Invoice;

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesAccountingTransaction(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0aecacff-23d0-48ff-8934-a4e5f711c729"))
			{
				SingularName = "SalesAccountingTransaction",
				PluralName = "SalesAccountingTransactions",
			};
        }

	}

    public partial class MetaSalesAgreement : MetaClass
	{
	    public static MetaSalesAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AgreementDate;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType Text;
        public ConcreteRoleType AgreementItems;
        public ConcreteRoleType AgreementNumber;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7d620a47-475b-40de-a4a7-8be7994df18e"))
			{
				SingularName = "SalesAgreement",
				PluralName = "SalesAgreements",
			};
        }

	}

    public partial class MetaSalesChannel : MetaClass
	{
	    public static MetaSalesChannel Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereSalesChannel;
        public AssociationType SalesOrderVersionsWhereSalesChannel;
        public AssociationType SalesChannelRevenuesWhereSalesChannel;
        public AssociationType SalesInvoicesWhereSalesChannel;
        public AssociationType SalesOrdersWhereSalesChannel;
        public AssociationType PriceComponentsWhereSalesChannel;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesChannel(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("db1678af-6541-4a35-a3b9-cffd0f821bd2"))
			{
				SingularName = "SalesChannel",
				PluralName = "SalesChannels",
			};
        }

	}

    public partial class MetaSalesChannelRevenue : MetaClass
	{
	    public static MetaSalesChannelRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Year;
        public RoleType Month;
        public RoleType Currency;
        public RoleType SalesChannel;
        public RoleType SalesChannelName;
        public RoleType Revenue;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesChannelRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("354524c8-355e-4994-b07e-91fc6bcb06cf"))
			{
				SingularName = "SalesChannelRevenue",
				PluralName = "SalesChannelRevenues",
			};
        }

	}

    public partial class MetaSalesInvoice : MetaClass
	{
	    public static MetaSalesInvoice Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Send;
        public MethodType CancelInvoice;
        public MethodType WriteOff;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousSalesInvoiceState;
        public RoleType LastSalesInvoiceState;
        public RoleType SalesInvoiceState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType TotalListPrice;
        public RoleType BillToContactMechanism;
        public RoleType PreviousBillToCustomer;
        public RoleType SalesInvoiceType;
        public RoleType InitialProfitMargin;
        public RoleType PaymentMethod;
        public RoleType SalesOrder;
        public RoleType InitialMarkupPercentage;
        public RoleType MaintainedMarkupPercentage;
        public RoleType SalesReps;
        public RoleType Shipment;
        public RoleType MaintainedProfitMargin;
        public RoleType PreviousShipToCustomer;
        public RoleType BillToCustomer;
        public RoleType SalesInvoiceItems;
        public RoleType TotalListPriceCustomerCurrency;
        public RoleType ShipToCustomer;
        public RoleType BilledFromContactMechanism;
        public RoleType TotalPurchasePrice;
        public RoleType SalesChannel;
        public RoleType Customers;
        public RoleType ShipToAddress;
        public RoleType Store;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType Description;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType BillingAccount;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType InvoiceDate;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType InvoiceNumber;
        public ConcreteRoleType Message;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Locale;
        public ConcreteRoleType Comment;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType SalesOrderVersionsWhereProformaInvoice;
        public AssociationType SalesOrderWhereProformaInvoice;

		// Inherited Associations
        public AssociationType PaymentApplicationsWhereInvoice;
        public AssociationType SalesAccountingTransactionWhereInvoice;


		internal MetaSalesInvoice(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6173fc23-115f-4356-a0ce-867872c151ac"))
			{
				SingularName = "SalesInvoice",
				PluralName = "SalesInvoices",
			};
        }

	}

    public partial class MetaSalesInvoiceItem : MetaClass
	{
	    public static MetaSalesInvoiceItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousSalesInvoiceItemState;
        public RoleType LastSalesInvoiceItemState;
        public RoleType SalesInvoiceItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType ProductFeature;
        public RoleType RequiredProfitMargin;
        public RoleType InitialMarkupPercentage;
        public RoleType MaintainedMarkupPercentage;
        public RoleType Product;
        public RoleType UnitPurchasePrice;
        public RoleType SalesOrderItem;
        public RoleType SalesInvoiceItemType;
        public RoleType SalesRep;
        public RoleType InitialProfitMargin;
        public RoleType MaintainedProfitMargin;
        public RoleType TimeEntries;
        public RoleType RequiredMarkupPercentage;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType InvoiceTerms;
        public ConcreteRoleType TotalInvoiceAdjustment;
        public ConcreteRoleType InvoiceVatRateItems;
        public ConcreteRoleType AdjustmentFor;
        public ConcreteRoleType SerializedInventoryItem;
        public ConcreteRoleType Message;
        public ConcreteRoleType TotalInvoiceAdjustmentCustomerCurrency;
        public ConcreteRoleType AmountPaid;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereSalesInvoiceItem;
        public AssociationType OrderItemBillingsWhereSalesInvoiceItem;
        public AssociationType SalesInvoiceWhereSalesInvoiceItem;

		// Inherited Associations
        public AssociationType PaymentApplicationsWhereInvoiceItem;
        public AssociationType ServiceEntryBillingsWhereInvoiceItem;
        public AssociationType ShipmentItemWhereInvoiceItem;
        public AssociationType WorkEffortBillingsWhereInvoiceItem;
        public AssociationType InvoiceItemVersionsWhereAdjustmentFor;
        public AssociationType InvoiceItemsWhereAdjustmentFor;


		internal MetaSalesInvoiceItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a98f8aca-d711-47e8-ac9c-25b607cbaef1"))
			{
				SingularName = "SalesInvoiceItem",
				PluralName = "SalesInvoiceItems",
			};
        }

	}

    public partial class MetaSalesInvoiceItemState : MetaClass
	{
	    public static MetaSalesInvoiceItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesInvoiceItemVersionsWhereSalesInvoiceItemState;
        public AssociationType SalesInvoiceItemsWherePreviousSalesInvoiceItemState;
        public AssociationType SalesInvoiceItemsWhereLastSalesInvoiceItemState;
        public AssociationType SalesInvoiceItemsWhereSalesInvoiceItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesInvoiceItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4babdd0c-52dd-4fb8-bbf5-120aa58eff50"))
			{
				SingularName = "SalesInvoiceItemState",
				PluralName = "SalesInvoiceItemStates",
			};
        }

	}

    public partial class MetaSalesInvoiceItemType : MetaClass
	{
	    public static MetaSalesInvoiceItemType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesInvoiceItemVersionsWhereSalesInvoiceItemType;
        public AssociationType SalesInvoiceItemsWhereSalesInvoiceItemType;
        public AssociationType SalesOrderItemsWhereItemType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesInvoiceItemType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("26f60d84-0659-4874-9c00-d6f3db11f073"))
			{
				SingularName = "SalesInvoiceItemType",
				PluralName = "SalesInvoiceItemTypes",
			};
        }

	}

    public partial class MetaSalesInvoiceState : MetaClass
	{
	    public static MetaSalesInvoiceState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereSalesInvoiceState;
        public AssociationType SalesInvoicesWherePreviousSalesInvoiceState;
        public AssociationType SalesInvoicesWhereLastSalesInvoiceState;
        public AssociationType SalesInvoicesWhereSalesInvoiceState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesInvoiceState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a4092f59-2baf-4041-83e6-5d50c8338a5c"))
			{
				SingularName = "SalesInvoiceState",
				PluralName = "SalesInvoiceStates",
			};
        }

	}

    public partial class MetaSalesInvoiceType : MetaClass
	{
	    public static MetaSalesInvoiceType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereSalesInvoiceType;
        public AssociationType SalesInvoicesWhereSalesInvoiceType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesInvoiceType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("81c9eefa-9b8b-40c0-9f1e-e6ecc2fef119"))
			{
				SingularName = "SalesInvoiceType",
				PluralName = "SalesInvoiceTypes",
			};
        }

	}

    public partial class MetaSalesOrder : MetaClass
	{
	    public static MetaSalesOrder Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Approve;
        public MethodType Reject;
        public MethodType Hold;
        public MethodType Continue;
        public MethodType Confirm;
        public MethodType Cancel;
        public MethodType Complete;

		// Defined Roles
        public RoleType PreviousSalesOrderState;
        public RoleType LastSalesOrderState;
        public RoleType SalesOrderState;
        public RoleType PreviousSalesOrderPaymentState;
        public RoleType LastSalesOrderPaymentState;
        public RoleType SalesOrderPaymentState;
        public RoleType PreviousSalesShipmentOrderState;
        public RoleType LastSalesOrderShipmentState;
        public RoleType SalesOrderShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType TakenByContactMechanism;
        public RoleType ShipToCustomer;
        public RoleType BillToCustomer;
        public RoleType TotalPurchasePrice;
        public RoleType ShipmentMethod;
        public RoleType TotalListPriceCustomerCurrency;
        public RoleType MaintainedProfitMargin;
        public RoleType ShipToAddress;
        public RoleType PreviousShipToCustomer;
        public RoleType BillToContactMechanism;
        public RoleType SalesReps;
        public RoleType InitialProfitMargin;
        public RoleType TotalListPrice;
        public RoleType PartiallyShip;
        public RoleType Customers;
        public RoleType Store;
        public RoleType MaintainedMarkupPercentage;
        public RoleType BillFromContactMechanism;
        public RoleType PaymentMethod;
        public RoleType PlacingContactMechanism;
        public RoleType PreviousBillToCustomer;
        public RoleType SalesChannel;
        public RoleType PlacingCustomer;
        public RoleType ProformaInvoice;
        public RoleType SalesOrderItems;
        public RoleType InitialMarkupPercentage;
        public RoleType Quote;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType CustomerCurrency;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType CustomerReference;
        public ConcreteRoleType Fee;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType ValidOrderItems;
        public ConcreteRoleType OrderNumber;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType Message;
        public ConcreteRoleType Description;
        public ConcreteRoleType TotalShippingAndHandlingCustomerCurrency;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalFeeCustomerCurrency;
        public ConcreteRoleType TotalShippingAndHandling;
        public ConcreteRoleType ShippingAndHandlingCharge;
        public ConcreteRoleType OrderDate;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalFee;
        public ConcreteRoleType SurchargeAdjustment;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType Locale;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType SalesInvoiceVersionsWhereSalesOrder;
        public AssociationType SalesInvoicesWhereSalesOrder;

		// Inherited Associations


		internal MetaSalesOrder(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("716647bf-7589-4146-a45c-a6a3b1cee507"))
			{
				SingularName = "SalesOrder",
				PluralName = "SalesOrders",
			};
        }

	}

    public partial class MetaSalesOrderItem : MetaClass
	{
	    public static MetaSalesOrderItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Continue;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Reject;
        public MethodType Confirm;
        public MethodType Approve;
        public MethodType Delete;

		// Defined Roles
        public RoleType PreviousSalesOrderItemState;
        public RoleType LastSalesOrderItemState;
        public RoleType SalesOrderItemState;
        public RoleType PreviousSalesOrderItemPaymentState;
        public RoleType LastSalesOrderItemPaymentState;
        public RoleType SalesOrderItemPaymentState;
        public RoleType PreviousSalesOrderItemShipmentState;
        public RoleType LastSalesOrderItemShipmentState;
        public RoleType SalesOrderItemShipmentState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType InitialProfitMargin;
        public RoleType QuantityShortFalled;
        public RoleType OrderedWithFeatures;
        public RoleType MaintainedProfitMargin;
        public RoleType RequiredProfitMargin;
        public RoleType PreviousReservedFromInventoryItem;
        public RoleType QuantityShipNow;
        public RoleType RequiredMarkupPercentage;
        public RoleType QuantityShipped;
        public RoleType ShipToAddress;
        public RoleType QuantityPicked;
        public RoleType PreviousProduct;
        public RoleType UnitPurchasePrice;
        public RoleType ShipToParty;
        public RoleType AssignedShipToAddress;
        public RoleType QuantityReturned;
        public RoleType QuantityReserved;
        public RoleType SalesRep;
        public RoleType AssignedShipToParty;
        public RoleType QuantityPendingShipment;
        public RoleType MaintainedMarkupPercentage;
        public RoleType InitialMarkupPercentage;
        public RoleType ReservedFromInventoryItem;
        public RoleType Product;
        public RoleType ProductFeature;
        public RoleType QuantityRequestsShipping;
        public RoleType ItemType;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType BudgetItem;
        public ConcreteRoleType PreviousQuantity;
        public ConcreteRoleType QuantityOrdered;
        public ConcreteRoleType Description;
        public ConcreteRoleType CorrespondingPurchaseOrder;
        public ConcreteRoleType TotalOrderAdjustmentCustomerCurrency;
        public ConcreteRoleType TotalOrderAdjustment;
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType AssignedDeliveryDate;
        public ConcreteRoleType DeliveryDate;
        public ConcreteRoleType OrderTerms;
        public ConcreteRoleType ShippingInstruction;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Message;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType TotalDiscountAsPercentage;
        public ConcreteRoleType DiscountAdjustment;
        public ConcreteRoleType UnitVat;
        public ConcreteRoleType TotalVatCustomerCurrency;
        public ConcreteRoleType VatRegime;
        public ConcreteRoleType TotalVat;
        public ConcreteRoleType UnitSurcharge;
        public ConcreteRoleType UnitDiscount;
        public ConcreteRoleType TotalExVatCustomerCurrency;
        public ConcreteRoleType DerivedVatRate;
        public ConcreteRoleType ActualUnitPrice;
        public ConcreteRoleType TotalIncVatCustomerCurrency;
        public ConcreteRoleType UnitBasePrice;
        public ConcreteRoleType CalculatedUnitPrice;
        public ConcreteRoleType TotalSurchargeCustomerCurrency;
        public ConcreteRoleType TotalIncVat;
        public ConcreteRoleType TotalSurchargeAsPercentage;
        public ConcreteRoleType TotalDiscountCustomerCurrency;
        public ConcreteRoleType TotalDiscount;
        public ConcreteRoleType TotalSurcharge;
        public ConcreteRoleType AssignedVatRegime;
        public ConcreteRoleType TotalBasePrice;
        public ConcreteRoleType TotalExVat;
        public ConcreteRoleType TotalBasePriceCustomerCurrency;
        public ConcreteRoleType CurrentPriceComponents;
        public ConcreteRoleType SurchargeAdjustment;

		// Defined Associations
        public AssociationType SalesInvoiceItemVersionsWhereSalesOrderItem;
        public AssociationType SalesOrderVersionsWhereSalesOrderItem;
        public AssociationType OrderShipmentsWhereSalesOrderItem;
        public AssociationType SalesInvoiceItemsWhereSalesOrderItem;
        public AssociationType SalesOrderWhereSalesOrderItem;

		// Inherited Associations
        public AssociationType SalesOrderItemVersionsWhereOrderedWithFeature;
        public AssociationType OrderItemBillingsWhereOrderItem;
        public AssociationType OrderRequirementCommitmentsWhereOrderItem;
        public AssociationType SalesOrderItemWhereOrderedWithFeature;
        public AssociationType ShipmentReceiptsWhereOrderItem;
        public AssociationType OrderItemVersionsWhereAssociation;
        public AssociationType OrderVersionsWhereValidOrderItem;
        public AssociationType OrderWhereValidOrderItem;
        public AssociationType OrderItemsWhereAssociation;
        public AssociationType WorkEffortVersionsWhereOrderItemFulfillment;
        public AssociationType WorkEffortsWhereOrderItemFulfillment;


		internal MetaSalesOrderItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("80de925c-04cc-412c-83a5-60405b0e63e6"))
			{
				SingularName = "SalesOrderItem",
				PluralName = "SalesOrderItems",
			};
        }

	}

    public partial class MetaRequestItemState : MetaClass
	{
	    public static MetaRequestItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType RequestItemVersionsWhereRequestItemState;
        public AssociationType RequestItemsWherePreviousRequestItemState;
        public AssociationType RequestItemsWhereLastRequestItemState;
        public AssociationType RequestItemsWhereRequestItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaRequestItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("498BDEC2-FD25-40C1-B0E7-CE393E2F12D9"))
			{
				SingularName = "RequestItemState",
				PluralName = "RequestItemStates",
			};
        }

	}

    public partial class MetaSalesOrderItemState : MetaClass
	{
	    public static MetaSalesOrderItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrderItemVersionsWhereSalesOrderItemState;
        public AssociationType SalesOrderItemsWherePreviousSalesOrderItemState;
        public AssociationType SalesOrderItemsWhereLastSalesOrderItemState;
        public AssociationType SalesOrderItemsWhereSalesOrderItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("21f09e4c-7b3f-4152-8822-8c485011759c"))
			{
				SingularName = "SalesOrderItemState",
				PluralName = "SalesOrderItemStates",
			};
        }

	}

    public partial class MetaRequestState : MetaClass
	{
	    public static MetaRequestState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType RequestVersionWhereRequestState;
        public AssociationType RequestsWherePreviousRequestState;
        public AssociationType RequestsWhereLastRequestState;
        public AssociationType RequestsWhereRequestState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaRequestState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("A1982F12-70E7-4B6B-BA58-1507414BBDB2"))
			{
				SingularName = "RequestState",
				PluralName = "RequestStates",
			};
        }

	}

    public partial class MetaSalesOrderState : MetaClass
	{
	    public static MetaSalesOrderState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrderVersionsWhereSalesOrderState;
        public AssociationType SalesOrdersWherePreviousSalesOrderState;
        public AssociationType SalesOrdersWhereLastSalesOrderState;
        public AssociationType SalesOrdersWhereSalesOrderState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesOrderState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8c993e3f-59a0-42f0-a0ef-d49f9beb0af6"))
			{
				SingularName = "SalesOrderState",
				PluralName = "SalesOrderStates",
			};
        }

	}

    public partial class MetaSalesRepCommission : MetaClass
	{
	    public static MetaSalesRepCommission Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Commission;
        public RoleType SalesRepName;
        public RoleType Month;
        public RoleType Year;
        public RoleType Currency;
        public RoleType SalesRep;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepCommission(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("bb5e8196-f821-4fb8-98cb-f19416d1427c"))
			{
				SingularName = "SalesRepCommission",
				PluralName = "SalesRepCommissions",
			};
        }

	}

    public partial class MetaSalesRepPartyProductCategoryRevenue : MetaClass
	{
	    public static MetaSalesRepPartyProductCategoryRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Year;
        public RoleType SalesRep;
        public RoleType ProductCategory;
        public RoleType Month;
        public RoleType Party;
        public RoleType Revenue;
        public RoleType Currency;
        public RoleType SalesRepName;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepPartyProductCategoryRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("01fd14a1-c852-42c9-8d16-3243ff655b8f"))
			{
				SingularName = "SalesRepPartyProductCategoryRevenue",
				PluralName = "SalesRepPartyProductCategoryRevenues",
			};
        }

	}

    public partial class MetaSalesRepPartyRevenue : MetaClass
	{
	    public static MetaSalesRepPartyRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revenue;
        public RoleType Year;
        public RoleType SalesRep;
        public RoleType SalesRepName;
        public RoleType Party;
        public RoleType Currency;
        public RoleType Month;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepPartyRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7b0e5009-eef2-4043-8794-b94663397053"))
			{
				SingularName = "SalesRepPartyRevenue",
				PluralName = "SalesRepPartyRevenues",
			};
        }

	}

    public partial class MetaSalesRepProductCategoryRevenue : MetaClass
	{
	    public static MetaSalesRepProductCategoryRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Month;
        public RoleType SalesRepName;
        public RoleType ProductCategory;
        public RoleType Currency;
        public RoleType Revenue;
        public RoleType Year;
        public RoleType SalesRep;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepProductCategoryRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fd411b2a-0121-4f1f-b1db-86c187e8a089"))
			{
				SingularName = "SalesRepProductCategoryRevenue",
				PluralName = "SalesRepProductCategoryRevenues",
			};
        }

	}

    public partial class MetaSalesRepRelationship : MetaClass
	{
	    public static MetaSalesRepRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType SalesRepresentative;
        public RoleType LastYearsCommission;
        public RoleType ProductCategories;
        public RoleType YTDCommission;
        public RoleType Customer;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType Parties;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c28f40a-1826-4110-83c8-7eaefc797f1a"))
			{
				SingularName = "SalesRepRelationship",
				PluralName = "SalesRepRelationships",
			};
        }

	}

    public partial class MetaSalesRepRevenue : MetaClass
	{
	    public static MetaSalesRepRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Revenue;
        public RoleType Currency;
        public RoleType Month;
        public RoleType SalesRepName;
        public RoleType Year;
        public RoleType SalesRep;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSalesRepRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("749e2a92-b397-4d36-b965-6073d45a4135"))
			{
				SingularName = "SalesRepRevenue",
				PluralName = "SalesRepRevenues",
			};
        }

	}

    public partial class MetaSalesTerritory : MetaClass
	{
	    public static MetaSalesTerritory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaSalesTerritory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("62ea5285-b9d8-4a41-9c14-79c712fd3bf4"))
			{
				SingularName = "SalesTerritory",
				PluralName = "SalesTerritories",
			};
        }

	}

    public partial class MetaSalutation : MetaClass
	{
	    public static MetaSalutation Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PeopleWhereSalutation;
        public AssociationType PersonVersionsWhereSalutation;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSalutation(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("91d1ad08-2eae-4d9e-8a2e-223eeae138af"))
			{
				SingularName = "Salutation",
				PluralName = "Salutations",
			};
        }

	}

    public partial class MetaSerialisedInventoryItem : MetaClass
	{
	    public static MetaSerialisedInventoryItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousSerialisedInventoryItemState;
        public RoleType LastSerialisedInventoryItemState;
        public RoleType SerialisedInventoryItemState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;
        public RoleType SerialNumber;
        public RoleType Ownership;
        public RoleType Owner;
        public RoleType AcquisitionYear;
        public RoleType ManufacturingYear;
        public RoleType ReplacementValue;
        public RoleType LifeTime;
        public RoleType DepreciationYears;
        public RoleType PurchasePrice;
        public RoleType ExpectedSalesPrice;
        public RoleType RefurbishCost;
        public RoleType TransportCost;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType ProductCharacteristicValues;
        public ConcreteRoleType InventoryItemVariances;
        public ConcreteRoleType Part;
        public ConcreteRoleType Name;
        public ConcreteRoleType Lot;
        public ConcreteRoleType Sku;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType DerivedProductCategories;
        public ConcreteRoleType Good;
        public ConcreteRoleType ProductType;
        public ConcreteRoleType Facility;

		// Defined Associations
        public AssociationType DeploymentsWhereSerializedInventoryItem;
        public AssociationType InvoiceItemVersionsWhereSerializedInventoryItem;
        public AssociationType InvoiceItemsWhereSerializedInventoryItem;

		// Inherited Associations
        public AssociationType ItemIssuancesWhereInventoryItem;
        public AssociationType PickListItemsWhereInventoryItem;
        public AssociationType ShipmentItemsWhereInventoryItem;
        public AssociationType WorkEffortInventoryAssignmentsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereInventoryItem;
        public AssociationType InventoryItemConfigurationsWhereComponentInventoryItem;
        public AssociationType WorkEffortVersionsWhereInventoryItemsProduced;
        public AssociationType WorkEffortsWhereInventoryItemsProduced;
        public AssociationType NotificationsWhereTarget;


		internal MetaSerialisedInventoryItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("4a70cbb3-6e23-4118-a07d-d611de9297de"))
			{
				SingularName = "SerialisedInventoryItem",
				PluralName = "SerialisedInventoryItems",
			};
        }

	}

    public partial class MetaSerialisedInventoryItemState : MetaClass
	{
	    public static MetaSerialisedInventoryItemState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SerialisedInventoryItemVersionsWhereSerialisedInventoryItemState;
        public AssociationType SerialisedInventoryItemsWherePreviousSerialisedInventoryItemState;
        public AssociationType SerialisedInventoryItemsWhereLastSerialisedInventoryItemState;
        public AssociationType SerialisedInventoryItemsWhereSerialisedInventoryItemState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSerialisedInventoryItemState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d042eeae-5c17-4936-861b-aaa9dfaed254"))
			{
				SingularName = "SerialisedInventoryItemState",
				PluralName = "SerialisedInventoryItemStates",
			};
        }

	}

    public partial class MetaSerializedInventoryItemObjectState : MetaClass
	{
	    public static MetaSerializedInventoryItemObjectState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SerializedInventoryItemStatusesWhereSerializedInventoryItemObjectState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaSerializedInventoryItemObjectState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2A8FF5AC-F2F2-44F5-918B-365AA2BFD9F2"))
			{
				SingularName = "SerializedInventoryItemObjectState",
				PluralName = "SerializedInventoryItemObjectStates",
			};
        }

	}

    public partial class MetaSerializedInventoryItemStatus : MetaClass
	{
	    public static MetaSerializedInventoryItemStatus Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType StartDateTime;
        public RoleType SerializedInventoryItemObjectState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSerializedInventoryItemStatus(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1da3e549-47cb-4896-94ec-3f8a263bb559"))
			{
				SingularName = "SerializedInventoryItemStatus",
				PluralName = "SerializedInventoryItemStatuses",
			};
        }

	}

    public partial class MetaServiceConfiguration : MetaClass
	{
	    public static MetaServiceConfiguration Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType InventoryItem;
        public ConcreteRoleType Quantity;
        public ConcreteRoleType ComponentInventoryItem;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaServiceConfiguration(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5d4beea4-f480-460e-92ee-3e8d532ac7f9"))
			{
				SingularName = "ServiceConfiguration",
				PluralName = "ServiceConfigurations",
			};
        }

	}

    public partial class MetaServiceEntryBilling : MetaClass
	{
	    public static MetaServiceEntryBilling Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ServiceEntry;
        public RoleType InvoiceItem;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaServiceEntryBilling(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2be4075a-c7e3-4a38-a045-7910f85b3e46"))
			{
				SingularName = "ServiceEntryBilling",
				PluralName = "ServiceEntryBillings",
			};
        }

	}

    public partial class MetaServiceEntryHeader : MetaClass
	{
	    public static MetaServiceEntryHeader Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ServiceEntries;
        public RoleType SubmittedDate;
        public RoleType SubmittedBy;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaServiceEntryHeader(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("22e85314-cfdf-4ead-a816-18588294fa79"))
			{
				SingularName = "ServiceEntryHeader",
				PluralName = "ServiceEntryHeaders",
			};
        }

	}

    public partial class MetaServiceFeature : MetaClass
	{
	    public static MetaServiceFeature Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaServiceFeature(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("fdbea721-61f8-4e75-b1dd-e3880636ee78"))
			{
				SingularName = "ServiceFeature",
				PluralName = "ServiceFeatures",
			};
        }

	}

    public partial class MetaServiceTerritory : MetaClass
	{
	    public static MetaServiceTerritory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Associations;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaServiceTerritory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("987f8328-2bfa-47cd-9521-8b7bda78f90a"))
			{
				SingularName = "ServiceTerritory",
				PluralName = "ServiceTerritories",
			};
        }

	}

    public partial class MetaShipmentItem : MetaClass
	{
	    public static MetaShipmentItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Quantity;
        public RoleType Part;
        public RoleType ContentsDescription;
        public RoleType Documents;
        public RoleType QuantityShipped;
        public RoleType InResponseToShipmentItems;
        public RoleType InventoryItems;
        public RoleType ProductFeatures;
        public RoleType InvoiceItems;
        public RoleType Good;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ItemIssuancesWhereShipmentItem;
        public AssociationType OrderShipmentsWhereShipmentItem;
        public AssociationType PackagingContentsWhereShipmentItem;
        public AssociationType ShipmentItemWhereInResponseToShipmentItem;
        public AssociationType ShipmentReceiptWhereShipmentItem;
        public AssociationType ShipmentVersionWhereShipmentItem;
        public AssociationType ShipmentWhereShipmentItem;

		// Inherited Associations


		internal MetaShipmentItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d35c33c3-ca15-4b70-b20d-c51ed068626a"))
			{
				SingularName = "ShipmentItem",
				PluralName = "ShipmentItems",
			};
        }

	}

    public partial class MetaShipmentMethod : MetaClass
	{
	    public static MetaShipmentMethod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType SalesOrderVersionsWhereShipmentMethod;
        public AssociationType SalesOrdersWhereShipmentMethod;
        public AssociationType ShipmentRouteSegmentsWhereShipmentMethod;
        public AssociationType ShippingAndHandlingComponentsWhereShipmentMethod;
        public AssociationType StoresWhereDefaultShipmentMethod;
        public AssociationType PartyVersionsWhereDefaultShipmentMethod;
        public AssociationType PartiesWhereDefaultShipmentMethod;
        public AssociationType ShipmentVersionsWhereShipmentMethod;
        public AssociationType ShipmentsWhereShipmentMethod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaShipmentMethod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3a3e6acf-48f4-4a33-848c-0c77cb18693a"))
			{
				SingularName = "ShipmentMethod",
				PluralName = "ShipmentMethods",
			};
        }

	}

    public partial class MetaShipmentPackage : MetaClass
	{
	    public static MetaShipmentPackage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PackagingContents;
        public RoleType Documents;
        public RoleType CreationDate;
        public RoleType SequenceNumber;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType ShipmentVersionWhereShipmentPackage;
        public AssociationType ShipmentWhereShipmentPackage;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaShipmentPackage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("444e431b-f078-46e0-9c8e-694e15e807c7"))
			{
				SingularName = "ShipmentPackage",
				PluralName = "ShipmentPackages",
			};
        }

	}

    public partial class MetaShipmentReceipt : MetaClass
	{
	    public static MetaShipmentReceipt Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ItemDescription;
        public RoleType InventoryItem;
        public RoleType RejectionReason;
        public RoleType OrderItem;
        public RoleType QuantityRejected;
        public RoleType ShipmentItem;
        public RoleType ReceivedDateTime;
        public RoleType QuantityAccepted;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaShipmentReceipt(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("48d14522-5fa8-44a8-ba4c-e2ddfc18e069"))
			{
				SingularName = "ShipmentReceipt",
				PluralName = "ShipmentReceipts",
			};
        }

	}

    public partial class MetaShipmentRouteSegment : MetaClass
	{
	    public static MetaShipmentRouteSegment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType EndKilometers;
        public RoleType FromFacility;
        public RoleType StartKilometers;
        public RoleType ShipmentMethod;
        public RoleType EstimatedStartDateTime;
        public RoleType ToFacility;
        public RoleType EstimatedArrivalDateTime;
        public RoleType Vehicle;
        public RoleType ActualArrivalDateTime;
        public RoleType ActualStartDateTime;
        public RoleType Carrier;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ShipmentVersionWhereShipmentRouteSegment;
        public AssociationType ShipmentWhereShipmentRouteSegment;

		// Inherited Associations


		internal MetaShipmentRouteSegment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8e6eaa35-85da-4c80-848c-3f1ed6cd2f8a"))
			{
				SingularName = "ShipmentRouteSegment",
				PluralName = "ShipmentRouteSegments",
			};
        }

	}

    public partial class MetaShipmentValue : MetaClass
	{
	    public static MetaShipmentValue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ThroughAmount;
        public RoleType FromAmount;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ShippingAndHandlingComponentsWhereShipmentValue;

		// Inherited Associations


		internal MetaShipmentValue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("20ef8456-83f2-4722-b8a8-1d8ab3129843"))
			{
				SingularName = "ShipmentValue",
				PluralName = "ShipmentValues",
			};
        }

	}

    public partial class MetaShippingAndHandlingCharge : MetaClass
	{
	    public static MetaShippingAndHandlingCharge Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType Percentage;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InvoiceVersionsWhereShippingAndHandlingCharge;
        public AssociationType InvoiceWhereShippingAndHandlingCharge;
        public AssociationType OrderVersionsWhereShippingAndHandlingCharge;
        public AssociationType OrderWhereShippingAndHandlingCharge;

		// Inherited Associations


		internal MetaShippingAndHandlingCharge(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("e7625d17-2485-4894-ba1a-c565b8c6052c"))
			{
				SingularName = "ShippingAndHandlingCharge",
				PluralName = "ShippingAndHandlingCharges",
			};
        }

	}

    public partial class MetaShippingAndHandlingComponent : MetaClass
	{
	    public static MetaShippingAndHandlingComponent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Cost;
        public RoleType ShipmentMethod;
        public RoleType Carrier;
        public RoleType ShipmentValue;
        public RoleType Currency;
        public RoleType GeographicBoundary;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaShippingAndHandlingComponent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1a174f59-c8cd-49ad-b0f4-a561cdcdcfb2"))
			{
				SingularName = "ShippingAndHandlingComponent",
				PluralName = "ShippingAndHandlingComponents",
			};
        }

	}

    public partial class MetaSize : MetaClass
	{
	    public static MetaSize Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaSize(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("45f5a73c-34d8-4452-8f22-7a744bd6650b"))
			{
				SingularName = "Size",
				PluralName = "Sizes",
			};
        }

	}

    public partial class MetaSkill : MetaClass
	{
	    public static MetaSkill Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType NeededSkillsWhereSkill;
        public AssociationType QuoteItemVersionsWhereSkill;
        public AssociationType PartySkillsWhereSkill;
        public AssociationType ProductDeliverySkillRequirementsWhereSkill;
        public AssociationType QuoteItemsWhereSkill;
        public AssociationType WorkEffortSkillStandardsWhereSkill;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSkill(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("123bfcba-0548-4637-8dfc-267d6c0ac262"))
			{
				SingularName = "Skill",
				PluralName = "Skills",
			};
        }

	}

    public partial class MetaSkillLevel : MetaClass
	{
	    public static MetaSkillLevel Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType NeededSkillsWhereSkillLevel;
        public AssociationType PartySkillsWhereSkillLevel;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSkillLevel(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("555882ea-d25a-4da2-a8ea-330469c8cd41"))
			{
				SingularName = "SkillLevel",
				PluralName = "SkillLevels",
			};
        }

	}

    public partial class MetaSkillRating : MetaClass
	{
	    public static MetaSkillRating Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType PartySkillWhereSkillRating;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaSkillRating(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2b44d390-bdd5-43aa-91c4-25b1966c46fb"))
			{
				SingularName = "SkillRating",
				PluralName = "SkillRatings",
			};
        }

	}

    public partial class MetaSoftwareFeature : MetaClass
	{
	    public static MetaSoftwareFeature Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType Description;
        public ConcreteRoleType DependentFeatures;
        public ConcreteRoleType IncompatibleFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations

		// Inherited Associations
        public AssociationType DesiredProductFeaturesWhereProductFeature;
        public AssociationType QuoteItemVersionsWhereProductFeature;
        public AssociationType RequestItemVersionsWhereProductFeature;
        public AssociationType SalesInvoiceItemVersionsWhereProductFeature;
        public AssociationType SalesOrderItemVersionsWhereProductFeature;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereUsedToDefine;
        public AssociationType QuoteItemsWhereProductFeature;
        public AssociationType RequestItemsWhereProductFeature;
        public AssociationType SalesInvoiceItemsWhereProductFeature;
        public AssociationType SalesOrderItemsWhereProductFeature;
        public AssociationType ShipmentItemsWhereProductFeature;
        public AssociationType EngagementItemsWhereProductFeature;
        public AssociationType PriceComponentsWhereProductFeature;
        public AssociationType ProductsWhereOptionalFeature;
        public AssociationType ProductsWhereStandardFeature;
        public AssociationType ProductsWhereSelectableFeature;
        public AssociationType ProductFeaturesWhereDependentFeature;
        public AssociationType ProductFeaturesWhereIncompatibleFeature;
        public AssociationType NotificationsWhereTarget;


		internal MetaSoftwareFeature(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("34047b37-545d-420f-ae79-2e05123cd623"))
			{
				SingularName = "SoftwareFeature",
				PluralName = "SoftwareFeatures",
			};
        }

	}

    public partial class MetaStandardServiceOrderItem : MetaClass
	{
	    public static MetaStandardServiceOrderItem Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType QuoteItem;
        public ConcreteRoleType Description;
        public ConcreteRoleType ExpectedStartDate;
        public ConcreteRoleType ExpectedEndDate;
        public ConcreteRoleType EngagementWorkFulfillment;
        public ConcreteRoleType EngagementRates;
        public ConcreteRoleType CurrentEngagementRate;
        public ConcreteRoleType OrderedWiths;
        public ConcreteRoleType CurrentAssignedProfessional;
        public ConcreteRoleType Product;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementWhereEngagementItem;
        public AssociationType ProfessionalAssignmentsWhereEngagementItem;
        public AssociationType EngagementItemWhereOrderedWith;
        public AssociationType ServiceEntriesWhereEngagementItem;


		internal MetaStandardServiceOrderItem(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("622a0738-338e-454e-a8ca-4a8fa3e9d9a4"))
			{
				SingularName = "StandardServiceOrderItem",
				PluralName = "StandardServiceOrderItems",
			};
        }

	}

    public partial class MetaState : MetaClass
	{
	    public static MetaState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType Cities;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Country;

		// Defined Associations
        public AssociationType CitiesWhereState;
        public AssociationType CountiesWhereState;

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c37f7876-51af-4748-b083-4a6e42e99597"))
			{
				SingularName = "State",
				PluralName = "States",
			};
        }

	}

    public partial class MetaStatementOfWork : MetaClass
	{
	    public static MetaStatementOfWork Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Approve;
        public MethodType Reject;

		// Defined Roles
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType PreviousQuoteState;
        public ConcreteRoleType LastQuoteState;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType Request;
        public ConcreteRoleType ContactPerson;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaStatementOfWork(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5459f555-cf6a-49c1-8015-b43cad74da17"))
			{
				SingularName = "StatementOfWork",
				PluralName = "StatementsOfWork",
			};
        }

	}

    public partial class MetaStatementOfWorkVersion : MetaClass
	{
	    public static MetaStatementOfWorkVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType RequiredResponseDate;
        public ConcreteRoleType ValidFromDate;
        public ConcreteRoleType QuoteTerms;
        public ConcreteRoleType ValidThroughDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType Receiver;
        public ConcreteRoleType FullfillContactMechanism;
        public ConcreteRoleType Price;
        public ConcreteRoleType Currency;
        public ConcreteRoleType IssueDate;
        public ConcreteRoleType QuoteItems;
        public ConcreteRoleType QuoteNumber;
        public ConcreteRoleType QuoteState;
        public ConcreteRoleType Request;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType StatementOfWorkWhereCurrentVersion;
        public AssociationType StatementOfWorkWhereAllVersion;

		// Inherited Associations


		internal MetaStatementOfWorkVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9C5784FB-6217-4C5D-8B87-D458DD4A00EE"))
			{
				SingularName = "StatementOfWorkVersion",
				PluralName = "StatementOfWorkVersions",
			};
        }

	}

    public partial class MetaStore : MetaClass
	{
	    public static MetaStore Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Catalogues;
        public RoleType ShipmentThreshold;
        public RoleType SalesOrderCounter;
        public RoleType OutgoingShipmentNumberPrefix;
        public RoleType SalesInvoiceNumberPrefix;
        public RoleType PaymentGracePeriod;
        public RoleType LogoImage;
        public RoleType PaymentNetDays;
        public RoleType DefaultFacility;
        public RoleType Name;
        public RoleType CreditLimit;
        public RoleType DefaultShipmentMethod;
        public RoleType DefaultCarrier;
        public RoleType SalesInvoiceCounter;
        public RoleType OrderThreshold;
        public RoleType DefaultPaymentMethod;
        public RoleType FiscalYearInvoiceNumbers;
        public RoleType PaymentMethods;
        public RoleType OutgoingShipmentCounter;
        public RoleType SalesOrderNumberPrefix;
        public RoleType ProcessFlow;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PickListVersionsWhereStore;
        public AssociationType SalesInvoiceVersionsWhereStore;
        public AssociationType SalesOrderVersionsWhereStore;
        public AssociationType PickListsWhereStore;
        public AssociationType SalesInvoicesWhereStore;
        public AssociationType SalesOrdersWhereStore;
        public AssociationType StoreRevenuesWhereStore;
        public AssociationType ShipmentVersionsWhereStore;
        public AssociationType ShipmentsWhereStore;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaStore(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d8611e48-b0ba-4037-a992-09e3e26c6d5d"))
			{
				SingularName = "Store",
				PluralName = "Stores",
			};
        }

	}

    public partial class MetaStoreRevenue : MetaClass
	{
	    public static MetaStoreRevenue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Month;
        public RoleType Currency;
        public RoleType Store;
        public RoleType StoreName;
        public RoleType Revenue;
        public RoleType Year;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaStoreRevenue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("282e0f1a-fda0-4287-a043-65dcc1853d95"))
			{
				SingularName = "StoreRevenue",
				PluralName = "StoreRevenues",
			};
        }

	}

    public partial class MetaStringTemplate : MetaClass
	{
	    public static MetaStringTemplate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Body;
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Locale;

		// Defined Associations

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaStringTemplate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0c50c02a-cc9c-4617-8530-15a24d4ac969"))
			{
				SingularName = "StringTemplate",
				PluralName = "StringTemplates",
			};
        }

	}

    public partial class MetaSubAgreement : MetaClass
	{
	    public static MetaSubAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Text;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Children;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementItem;
        public AssociationType AgreementItemWhereChild;


		internal MetaSubAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("11e8fae8-3270-4789-a4eb-ca89cddd2859"))
			{
				SingularName = "SubAgreement",
				PluralName = "SubAgreements",
			};
        }

	}

    public partial class MetaSubAssembly : MetaClass
	{
	    public static MetaSubAssembly Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType PartSpecifications;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType Documents;
        public ConcreteRoleType ManufacturerId;
        public ConcreteRoleType ReorderLevel;
        public ConcreteRoleType ReorderQuantity;
        public ConcreteRoleType PriceComponents;
        public ConcreteRoleType InventoryItemKind;
        public ConcreteRoleType Sku;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations

		// Inherited Associations
        public AssociationType PurchaseInvoiceItemVersionsWherePart;
        public AssociationType PurchaseOrderItemVersionsWherePart;
        public AssociationType PartRevisionsWhereSupersededByPart;
        public AssociationType PartRevisionsWherePart;
        public AssociationType PartSubstitutesWhereSubstitutionPart;
        public AssociationType PartSubstitutesWherePart;
        public AssociationType PurchaseInvoiceItemsWherePart;
        public AssociationType PurchaseOrderItemsWherePart;
        public AssociationType ShipmentItemsWherePart;
        public AssociationType SupplierOfferingsWherePart;
        public AssociationType WorkEffortPartStandardsWherePart;
        public AssociationType InventoryItemVersionsWherePart;
        public AssociationType InventoryItemsWherePart;
        public AssociationType PartBillOfMaterialsWherePart;
        public AssociationType PartBillOfMaterialsWhereComponentPart;
        public AssociationType NotificationsWhereTarget;


		internal MetaSubAssembly(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("b1a10fe4-2d84-452b-b0cb-e96e55014856"))
			{
				SingularName = "SubAssembly",
				PluralName = "SubAssemblies",
			};
        }

	}

    public partial class MetaSubContractorAgreement : MetaClass
	{
	    public static MetaSubContractorAgreement Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType AgreementDate;
        public ConcreteRoleType Addenda;
        public ConcreteRoleType Description;
        public ConcreteRoleType AgreementTerms;
        public ConcreteRoleType Text;
        public ConcreteRoleType AgreementItems;
        public ConcreteRoleType AgreementNumber;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementsWhereAgreement;
        public AssociationType PartyVersionWhereAgreement;
        public AssociationType PartyWhereAgreement;
        public AssociationType NotificationsWhereTarget;


		internal MetaSubContractorAgreement(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1b2113f6-2c00-4ea7-9408-72bae667eaa3"))
			{
				SingularName = "SubContractorAgreement",
				PluralName = "SubContractorAgreements",
			};
        }

	}

    public partial class MetaSubContractorRelationship : MetaClass
	{
	    public static MetaSubContractorRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Contractor;
        public RoleType SubContractor;

		// Inherited Roles
        public ConcreteRoleType Parties;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSubContractorRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("d60cc44a-6491-4982-9b2d-99891e382a21"))
			{
				SingularName = "SubContractorRelationship",
				PluralName = "SubContractorRelationships",
			};
        }

	}

    public partial class MetaSupplierOffering : MetaClass
	{
	    public static MetaSupplierOffering Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Rating;
        public RoleType StandardLeadTime;
        public RoleType ProductPurchasePrices;
        public RoleType Preference;
        public RoleType MinimalOrderQuantity;
        public RoleType Product;
        public RoleType Supplier;
        public RoleType ReferenceNumber;
        public RoleType Part;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSupplierOffering(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0ae3caca-9b4b-407f-bd98-46db03b72a43"))
			{
				SingularName = "SupplierOffering",
				PluralName = "SupplierOfferings",
			};
        }

	}

    public partial class MetaSupplierRelationship : MetaClass
	{
	    public static MetaSupplierRelationship Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Supplier;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaSupplierRelationship(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2b162153-f74d-4f97-b97c-48f04749b216"))
			{
				SingularName = "SupplierRelationship",
				PluralName = "SupplierRelationships",
			};
        }

	}

    public partial class MetaSurchargeAdjustment : MetaClass
	{
	    public static MetaSurchargeAdjustment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Amount;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType Percentage;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType InvoiceVersionsWhereSurchargeAdjustment;
        public AssociationType InvoiceWhereSurchargeAdjustment;
        public AssociationType OrderVersionsWhereSurchargeAdjustment;
        public AssociationType PriceableVersionsWhereSurchargeAdjustment;
        public AssociationType OrderWhereSurchargeAdjustment;
        public AssociationType PriceableWhereSurchargeAdjustment;

		// Inherited Associations


		internal MetaSurchargeAdjustment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("70468d86-b8a0-4aff-881e-fca2386f64da"))
			{
				SingularName = "SurchargeAdjustment",
				PluralName = "SurchargeAdjustments",
			};
        }

	}

    public partial class MetaSurchargeComponent : MetaClass
	{
	    public static MetaSurchargeComponent Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Percentage;

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaSurchargeComponent(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a18de27f-54fe-4160-b149-475bebeaf716"))
			{
				SingularName = "SurchargeComponent",
				PluralName = "SurchargeComponents",
			};
        }

	}

    public partial class MetaRequirementType : MetaClass
	{
	    public static MetaRequirementType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType RequirementVersionsWhereRequirementType;
        public AssociationType RequirementsWhereRequirementType;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaRequirementType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2C2CE687-7DB1-4B8C-A582-EDE6E021EA03"))
			{
				SingularName = "RequirementType",
				PluralName = "RequirementTypes",
			};
        }

	}

    public partial class MetaInvoiceTermType : MetaClass
	{
	    public static MetaInvoiceTermType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations

		// Inherited Associations
        public AssociationType OrderTermsWhereTermType;
        public AssociationType QuoteTermsWhereTermType;
        public AssociationType AgreementTermsWhereTermType;
        public AssociationType NotificationsWhereTarget;


		internal MetaInvoiceTermType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2C934905-DAFF-4288-ACC4-2EBDF5CC79E4"))
			{
				SingularName = "InvoiceTermType",
				PluralName = "InvoiceTermTypes",
			};
        }

	}

    public partial class MetaTransferVersion : MetaClass
	{
	    public static MetaTransferVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType TransferState;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations
        public AssociationType TransferWhereCurrentVersion;
        public AssociationType TransferWhereAllVersion;

		// Inherited Associations


		internal MetaTransferVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("827E2A7D-2FEF-4511-9EA4-9D2A3BF76153"))
			{
				SingularName = "TransferVersion",
				PluralName = "TransferVersions",
			};
        }

	}

    public partial class MetaWebSiteCommunicationVersion : MetaClass
	{
	    public static MetaWebSiteCommunicationVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Originator;
        public RoleType Receiver;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType WebSiteCommunicationWhereCurrentVersion;
        public AssociationType WebSiteCommunicationWhereAllVersion;

		// Inherited Associations


		internal MetaWebSiteCommunicationVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("B1F9AAA7-188E-4C03-9AC1-EA4373DAA21A"))
			{
				SingularName = "WebSiteCommunicationVersion",
				PluralName = "WebSiteCommunicationVersions",
			};
        }

	}

    public partial class MetaWorkTask : MetaClass
	{
	    public static MetaWorkTask Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Confirm;
        public MethodType Finish;
        public MethodType Cancel;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType SendNotification;
        public RoleType SendReminder;
        public RoleType RemindAt;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Priority;
        public ConcreteRoleType WorkEffortPurposes;
        public ConcreteRoleType ActualCompletion;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ScheduledCompletion;
        public ConcreteRoleType ActualHours;
        public ConcreteRoleType EstimatedHours;
        public ConcreteRoleType Precendencies;
        public ConcreteRoleType Facility;
        public ConcreteRoleType DeliverablesProduced;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType InventoryItemsNeeded;
        public ConcreteRoleType Children;
        public ConcreteRoleType OrderItemFulfillment;
        public ConcreteRoleType WorkEffortType;
        public ConcreteRoleType InventoryItemsProduced;
        public ConcreteRoleType RequirementFulfillments;
        public ConcreteRoleType SpecialTerms;
        public ConcreteRoleType Concurrencies;
        public ConcreteRoleType PreviousWorkEffortState;
        public ConcreteRoleType LastWorkEffortState;
        public ConcreteRoleType WorkEffortState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType QuoteItemVersionsWhereWorkEffort;
        public AssociationType QuoteItemsWhereWorkEffort;
        public AssociationType WorkEffortAssignmentsWhereAssignment;
        public AssociationType WorkEffortBillingsWhereWorkEffort;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereAssignment;
        public AssociationType WorkEffortInventoryAssignmentsWhereAssignment;
        public AssociationType WorkEffortPartyAssignmentsWhereAssignment;
        public AssociationType CommunicationEventVersionsWhereWorkEffort;
        public AssociationType CommunicationEventsWhereWorkEffort;
        public AssociationType EngagementItemsWhereEngagementWorkFulfillment;
        public AssociationType ServiceEntriesWhereWorkEffort;
        public AssociationType WorkEffortVersionsWherePrecendency;
        public AssociationType WorkEffortVersionsWhereChild;
        public AssociationType WorkEffortVersionsWhereConcurrency;
        public AssociationType WorkEffortsWherePrecendency;
        public AssociationType WorkEffortsWhereChild;
        public AssociationType WorkEffortsWhereConcurrency;
        public AssociationType NotificationsWhereTarget;


		internal MetaWorkTask(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("76911215-A288-4B0D-BECE-83E7A617B847"))
			{
				SingularName = "WorkTask",
				PluralName = "WorkTasks",
			};
        }

	}

    public partial class MetaTaxDocument : MetaClass
	{
	    public static MetaTaxDocument Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Text;
        public ConcreteRoleType DocumentLocation;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ShipmentItemWhereDocument;
        public AssociationType ShipmentPackageWhereDocument;
        public AssociationType PartsWhereDocument;
        public AssociationType ProductsWhereDocument;
        public AssociationType ShipmentVersionWhereDocument;
        public AssociationType ShipmentWhereDocument;


		internal MetaTaxDocument(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0d03a71b-c58e-405d-a995-c467a0b25d5b"))
			{
				SingularName = "TaxDocument",
				PluralName = "TaxDocuments",
			};
        }

	}

    public partial class MetaTaxDue : MetaClass
	{
	    public static MetaTaxDue Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType FromParty;
        public ConcreteRoleType ToParty;
        public ConcreteRoleType AccountingTransactionDetails;
        public ConcreteRoleType Description;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DerivedTotalAmount;
        public ConcreteRoleType AccountingTransactionNumber;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaTaxDue(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("57b74174-1418-4307-96f7-e579638d7dd9"))
			{
				SingularName = "TaxDue",
				PluralName = "TaxDues",
			};
        }

	}

    public partial class MetaTelecommunicationsNumber : MetaClass
	{
	    public static MetaTelecommunicationsNumber Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType CountryCode;
        public RoleType AreaCode;
        public RoleType ContactNumber;

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType FollowTo;
        public ConcreteRoleType ContactMechanismType;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations
        public AssociationType FaxCommunicationsWhereOutgoingFaxNumber;
        public AssociationType FaxCommunicationVersionsWhereOutgoingFaxNumber;
        public AssociationType PartyVersionsWhereBillingInquiriesFax;
        public AssociationType PartyVersionsWhereOrderInquiriesFax;
        public AssociationType PartyVersionsWhereShippingInquiriesFax;
        public AssociationType PartyVersionsWhereShippingInquiriesPhone;
        public AssociationType PartyVersionsWhereOrderInquiriesPhone;
        public AssociationType PartyVersionsWhereCellPhoneNumber;
        public AssociationType PartyVersionsWhereBillingInquiriesPhone;
        public AssociationType PartyVersionsWhereGeneralFaxNumber;
        public AssociationType PartyVersionsWhereGeneralPhoneNumber;
        public AssociationType PartiesWhereBillingInquiriesFax;
        public AssociationType PartiesWhereOrderInquiriesFax;
        public AssociationType PartiesWhereShippingInquiriesFax;
        public AssociationType PartiesWhereShippingInquiriesPhone;
        public AssociationType PartiesWhereOrderInquiriesPhone;
        public AssociationType PartiesWhereCellPhoneNumber;
        public AssociationType PartiesWhereBillingInquiriesPhone;
        public AssociationType PartiesWhereGeneralFaxNumber;
        public AssociationType PartiesWhereGeneralPhoneNumber;

		// Inherited Associations
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;


		internal MetaTelecommunicationsNumber(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6c255f71-ce18-4d51-b0d9-e402ec0e570e"))
			{
				SingularName = "TelecommunicationsNumber",
				PluralName = "TelecommunicationsNumbers",
			};
        }

	}

    public partial class MetaTerritory : MetaClass
	{
	    public static MetaTerritory Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Cities;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType Latitude;
        public ConcreteRoleType Longitude;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Country;

		// Defined Associations

		// Inherited Associations
        public AssociationType PostalAddressesWhereGeographicBoundary;
        public AssociationType ShippingAndHandlingComponentsWhereGeographicBoundary;
        public AssociationType EstimatedProductCostsWhereGeographicBoundary;
        public AssociationType GeographicBoundaryCompositesWhereAssociation;
        public AssociationType PriceComponentsWhereGeographicBoundary;
        public AssociationType NotificationsWhereTarget;


		internal MetaTerritory(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7118e029-a8b3-415b-b9e9-d48ba4ea2823"))
			{
				SingularName = "Territory",
				PluralName = "Territories",
			};
        }

	}

    public partial class MetaThreshold : MetaClass
	{
	    public static MetaThreshold Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType TermValue;
        public ConcreteRoleType TermType;
        public ConcreteRoleType Description;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType AgreementWhereAgreementTerm;
        public AssociationType AgreementItemWhereAgreementTerm;
        public AssociationType InvoiceItemVersionsWhereInvoiceTerm;
        public AssociationType InvoiceItemWhereInvoiceTerm;


		internal MetaThreshold(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c7b56330-1fb6-46c7-a042-04a4cf671ec1"))
			{
				SingularName = "Threshold",
				PluralName = "Thresholds",
			};
        }

	}

    public partial class MetaTimeAndMaterialsService : MetaClass
	{
	    public static MetaTimeAndMaterialsService Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType InternalComment;
        public ConcreteRoleType PrimaryProductCategory;
        public ConcreteRoleType SupportDiscontinuationDate;
        public ConcreteRoleType SalesDiscontinuationDate;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType LocalisedDescriptions;
        public ConcreteRoleType LocalisedComments;
        public ConcreteRoleType Description;
        public ConcreteRoleType VirtualProductPriceComponents;
        public ConcreteRoleType IntrastatCode;
        public ConcreteRoleType ProductCategoriesExpanded;
        public ConcreteRoleType ProductComplement;
        public ConcreteRoleType OptionalFeatures;
        public ConcreteRoleType Variants;
        public ConcreteRoleType Name;
        public ConcreteRoleType IntroductionDate;
        public ConcreteRoleType Documents;
        public ConcreteRoleType StandardFeatures;
        public ConcreteRoleType UnitOfMeasure;
        public ConcreteRoleType EstimatedProductCosts;
        public ConcreteRoleType ProductObsolescences;
        public ConcreteRoleType SelectableFeatures;
        public ConcreteRoleType VatRate;
        public ConcreteRoleType BasePrices;
        public ConcreteRoleType ProductCategories;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType ProductDeliverySkillRequirementsWhereService;
        public AssociationType GeneralLedgerAccountsWhereDefaultCostUnit;
        public AssociationType GeneralLedgerAccountsWhereCostUnitsAllowed;
        public AssociationType GoodsWhereProductSubstitution;
        public AssociationType GoodsWhereProductIncompatibility;
        public AssociationType MarketingPackageWhereProductsUsedIn;
        public AssociationType MarketingPackagesWhereProduct;
        public AssociationType PurchaseOrderItemVersionsWhereProduct;
        public AssociationType QuoteItemVersionsWhereProduct;
        public AssociationType RequestItemVersionsWhereProduct;
        public AssociationType SalesInvoiceItemVersionsWhereProduct;
        public AssociationType SalesOrderItemVersionsWherePreviousProduct;
        public AssociationType SalesOrderItemVersionsWhereProduct;
        public AssociationType OrganisationGlAccountsWhereProduct;
        public AssociationType PartyProductRevenuesWhereProduct;
        public AssociationType ProductCategoriesWhereAllProduct;
        public AssociationType ProductConfigurationsWhereProductsUsedIn;
        public AssociationType ProductConfigurationsWhereProduct;
        public AssociationType ProductFeatureApplicabilityRelationshipsWhereAvailableFor;
        public AssociationType ProductRevenuesWhereProduct;
        public AssociationType PurchaseOrderItemsWhereProduct;
        public AssociationType QuoteItemsWhereProduct;
        public AssociationType RequestItemsWhereProduct;
        public AssociationType SalesInvoiceItemsWhereProduct;
        public AssociationType SalesOrderItemsWherePreviousProduct;
        public AssociationType SalesOrderItemsWhereProduct;
        public AssociationType SupplierOfferingsWhereProduct;
        public AssociationType WorkEffortTypesWhereProductToProduce;
        public AssociationType EngagementItemsWhereProduct;
        public AssociationType PriceComponentsWhereProduct;
        public AssociationType ProductsWhereProductComplement;
        public AssociationType ProductWhereVariant;
        public AssociationType ProductsWhereProductObsolescence;
        public AssociationType NotificationsWhereTarget;


		internal MetaTimeAndMaterialsService(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da504b46-2fd0-4500-ae23-61fa73151077"))
			{
				SingularName = "TimeAndMaterialsService",
				PluralName = "TimeAndMaterialsServices",
			};
        }

	}

    public partial class MetaTimeEntry : MetaClass
	{
	    public static MetaTimeEntry Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Cost;
        public RoleType GrossMargin;
        public RoleType QuoteTerm;
        public RoleType BillingRate;
        public RoleType UnitOfMeasure;
        public RoleType AmountOfTime;

		// Inherited Roles
        public ConcreteRoleType ThroughDateTime;
        public ConcreteRoleType EngagementItem;
        public ConcreteRoleType IsBillable;
        public ConcreteRoleType FromDateTime;
        public ConcreteRoleType Description;
        public ConcreteRoleType WorkEffort;
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType SalesInvoiceItemVersionsWhereTimeEntry;
        public AssociationType SalesInvoiceItemsWhereTimeEntry;

		// Inherited Associations
        public AssociationType ServiceEntryBillingsWhereServiceEntry;
        public AssociationType ServiceEntryHeaderWhereServiceEntry;


		internal MetaTimeEntry(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("6360b45d-3556-41c6-b183-f42a15b9424f"))
			{
				SingularName = "TimeEntry",
				PluralName = "TimeEntries",
			};
        }

	}

    public partial class MetaTimeFrequency : MetaClass
	{
	    public static MetaTimeFrequency Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType Description;
        public ConcreteRoleType UnitOfMeasureConversions;
        public ConcreteRoleType Abbreviation;

		// Defined Associations
        public AssociationType AccountingPeriodsWhereTimeFrequency;
        public AssociationType AccountingPeriodVersionsWhereTimeFrequency;
        public AssociationType PartyBenefitsWhereTimeFrequency;
        public AssociationType PayHistoriesWhereTimeFrequency;
        public AssociationType PayrollPreferencesWhereTimeFrequency;
        public AssociationType PositionTypeRatesWhereTimeFrequency;
        public AssociationType RecurringChargesWhereTimeFrequency;
        public AssociationType VatRatesWherePaymentFrequency;
        public AssociationType DeploymentUsagesWhereTimeFrequency;

		// Inherited Associations
        public AssociationType UnitOfMeasureConversionsWhereToUnitOfMeasure;
        public AssociationType NotificationsWhereTarget;


		internal MetaTimeFrequency(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("1aba0c3c-2a1c-414d-86df-5a9b8c672587"))
			{
				SingularName = "TimeFrequency",
				PluralName = "TimeFrequencies",
			};
        }

	}

    public partial class MetaTimePeriodUsage : MetaClass
	{
	    public static MetaTimePeriodUsage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType TimeFrequency;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DeploymentsWhereDeploymentUsage;


		internal MetaTimePeriodUsage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f7e69670-1824-44ea-b2cd-fdef02ef84a7"))
			{
				SingularName = "TimePeriodUsage",
				PluralName = "TimePeriodUsages",
			};
        }

	}

    public partial class MetaTraining : MetaClass
	{
	    public static MetaTraining Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType PersonTrainingsWhereTraining;

		// Inherited Associations


		internal MetaTraining(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0eaa8719-bbf4-408a-8226-851580556024"))
			{
				SingularName = "Training",
				PluralName = "Trainings",
			};
        }

	}

    public partial class MetaTransfer : MetaClass
	{
	    public static MetaTransfer Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType PreviousTransferState;
        public RoleType LastTransferState;
        public RoleType TransferState;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType PrintContent;
        public ConcreteRoleType Comment;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType ShipmentMethod;
        public ConcreteRoleType BillToContactMechanism;
        public ConcreteRoleType ShipmentPackages;
        public ConcreteRoleType ShipmentNumber;
        public ConcreteRoleType Documents;
        public ConcreteRoleType BillToParty;
        public ConcreteRoleType ShipToParty;
        public ConcreteRoleType ShipmentItems;
        public ConcreteRoleType ReceiverContactMechanism;
        public ConcreteRoleType ShipToAddress;
        public ConcreteRoleType EstimatedShipCost;
        public ConcreteRoleType EstimatedShipDate;
        public ConcreteRoleType LatestCancelDate;
        public ConcreteRoleType Carrier;
        public ConcreteRoleType InquireAboutContactMechanism;
        public ConcreteRoleType EstimatedReadyDate;
        public ConcreteRoleType ShipFromAddress;
        public ConcreteRoleType BillFromContactMechanism;
        public ConcreteRoleType HandlingInstruction;
        public ConcreteRoleType Store;
        public ConcreteRoleType ShipFromParty;
        public ConcreteRoleType ShipmentRouteSegments;
        public ConcreteRoleType EstimatedArrivalDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType SalesInvoiceVersionsWhereShipment;
        public AssociationType SalesInvoicesWhereShipment;


		internal MetaTransfer(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("cd66a79f-c4b8-4c33-b6ec-1928809b6b88"))
			{
				SingularName = "Transfer",
				PluralName = "Transfers",
			};
        }

	}

    public partial class MetaTransferState : MetaClass
	{
	    public static MetaTransferState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType TransferVersionsWhereTransferState;
        public AssociationType TransfersWherePreviousTransferState;
        public AssociationType TransfersWhereLastTransferState;
        public AssociationType TransfersWhereTransferState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaTransferState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9f3d9ae6-cbbf-4cfb-900d-bc66edccbf95"))
			{
				SingularName = "TransferState",
				PluralName = "TransferStates",
			};
        }

	}

    public partial class MetaTransition : MetaClass
	{
	    public static MetaTransition Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType FromStates;
        public RoleType ToState;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaTransition(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a7e490c0-ce29-4298-97c4-519904bb755a"))
			{
				SingularName = "Transition",
				PluralName = "Transitions",
			};
        }

	}

    public partial class MetaUnitOfMeasure : MetaClass
	{
	    public static MetaUnitOfMeasure Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType UnitOfMeasureConversions;
        public ConcreteRoleType Abbreviation;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;

		// Defined Associations
        public AssociationType ActivityUsagesWhereUnitOfMeasure;
        public AssociationType DimensionsWhereUnitOfMeasure;
        public AssociationType EngagementRatesWhereUnitOfMeasure;
        public AssociationType QuoteItemVersionsWhereUnitOfMeasure;
        public AssociationType RequestItemVersionsWhereUnitOfMeasure;
        public AssociationType ProductPurchasePricesWhereUnitOfMeasure;
        public AssociationType QuoteItemsWhereUnitOfMeasure;
        public AssociationType RequestItemsWhereUnitOfMeasure;
        public AssociationType TimeEntriesWhereUnitOfMeasure;
        public AssociationType UtilizationChargesWhereUnitOfMeasure;
        public AssociationType VolumeUsagesWhereUnitOfMeasure;
        public AssociationType InventoryItemVersionsWhereUnitOfMeasure;
        public AssociationType InventoryItemsWhereUnitOfMeasure;
        public AssociationType PartsWhereUnitOfMeasure;
        public AssociationType ProductsWhereUnitOfMeasure;

		// Inherited Associations
        public AssociationType UnitOfMeasureConversionsWhereToUnitOfMeasure;
        public AssociationType NotificationsWhereTarget;


		internal MetaUnitOfMeasure(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5cd7ea86-8bc6-4b72-a8f6-788e6453acdc"))
			{
				SingularName = "UnitOfMeasure",
				PluralName = "UnitsOfMeasure",
			};
        }

	}

    public partial class MetaUnitOfMeasureConversion : MetaClass
	{
	    public static MetaUnitOfMeasureConversion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType ToUnitOfMeasure;
        public RoleType StartDate;
        public RoleType ConversionFactor;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType IUnitOfMeasureWhereUnitOfMeasureConversion;

		// Inherited Associations


		internal MetaUnitOfMeasureConversion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2e216901-eab9-42e3-9e49-7fe8e88291d3"))
			{
				SingularName = "UnitOfMeasureConversion",
				PluralName = "UnitOfMeasureConversions",
			};
        }

	}

    public partial class MetaUtilizationCharge : MetaClass
	{
	    public static MetaUtilizationCharge Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Quantity;
        public RoleType UnitOfMeasure;

		// Inherited Roles
        public ConcreteRoleType GeographicBoundary;
        public ConcreteRoleType Rate;
        public ConcreteRoleType RevenueValueBreak;
        public ConcreteRoleType PartyClassification;
        public ConcreteRoleType OrderQuantityBreak;
        public ConcreteRoleType PackageQuantityBreak;
        public ConcreteRoleType Product;
        public ConcreteRoleType RevenueQuantityBreak;
        public ConcreteRoleType ProductFeature;
        public ConcreteRoleType AgreementPricingProgram;
        public ConcreteRoleType Description;
        public ConcreteRoleType Currency;
        public ConcreteRoleType OrderKind;
        public ConcreteRoleType OrderValue;
        public ConcreteRoleType Price;
        public ConcreteRoleType ProductCategory;
        public ConcreteRoleType SalesChannel;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations
        public AssociationType EngagementRateWhereGoverningPriceComponent;
        public AssociationType PriceableVersionsWhereCurrentPriceComponent;
        public AssociationType PartWherePriceComponent;
        public AssociationType PriceablesWhereCurrentPriceComponent;
        public AssociationType ProductWhereVirtualProductPriceComponent;
        public AssociationType ProductsWhereBasePrice;
        public AssociationType ProductFeaturesWhereBasePrice;


		internal MetaUtilizationCharge(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("96a64894-e444-4df4-9289-1b121842ac73"))
			{
				SingularName = "UtilizationCharge",
				PluralName = "UtilizationCharges",
			};
        }

	}

    public partial class MetaVarianceReason : MetaClass
	{
	    public static MetaVarianceReason Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InventoryItemVariancesWhereReason;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVarianceReason(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8ff46109-8ae7-4da5-a1f9-f19d4cf4e27e"))
			{
				SingularName = "VarianceReason",
				PluralName = "VarianceReasons",
			};
        }

	}

    public partial class MetaVatCalculationMethod : MetaClass
	{
	    public static MetaVatCalculationMethod Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType VatRatesWhereVatCalculationMethod;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVatCalculationMethod(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b73eea7-6455-4fe5-87c0-99c852f57e6b"))
			{
				SingularName = "VatCalculationMethod",
				PluralName = "VatCalculationMethods",
			};
        }

	}

    public partial class MetaVatForm : MetaClass
	{
	    public static MetaVatForm Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Name;
        public RoleType VatReturnBoxes;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType CountryWhereVatForm;

		// Inherited Associations


		internal MetaVatForm(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("eba70b57-05e3-487f-8cf1-45f14fcdc3b9"))
			{
				SingularName = "VatForm",
				PluralName = "VatForms",
			};
        }

	}

    public partial class MetaVatRate : MetaClass
	{
	    public static MetaVatRate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType VatCalculationMethod;
        public RoleType VatReturnBoxes;
        public RoleType Rate;
        public RoleType VatPayableAccount;
        public RoleType TaxAuthority;
        public RoleType VatRateUsage;
        public RoleType VatRatePurchaseKind;
        public RoleType VatTariff;
        public RoleType PaymentFrequency;
        public RoleType VatToPayAccount;
        public RoleType EuSalesListType;
        public RoleType VatToReceiveAccount;
        public RoleType VatReceivableAccount;
        public RoleType ReverseCharge;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType CountryWhereVatRate;
        public AssociationType InvoiceVatRateItemsWhereVatRate;
        public AssociationType VatRegimesWhereVatRate;
        public AssociationType OrderAdjustmentVersionsWhereVatRate;
        public AssociationType PriceableVersionsWhereDerivedVatRate;
        public AssociationType OrderAdjustmentsWhereVatRate;
        public AssociationType PriceablesWhereDerivedVatRate;
        public AssociationType ProductsWhereVatRate;
        public AssociationType ProductFeaturesWhereVatRate;

		// Inherited Associations


		internal MetaVatRate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a5e29ca1-80de-4de4-9085-b69f21550b5a"))
			{
				SingularName = "VatRate",
				PluralName = "VatRates",
			};
        }

	}

    public partial class MetaVatRatePurchaseKind : MetaClass
	{
	    public static MetaVatRatePurchaseKind Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType VatRatesWhereVatRatePurchaseKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVatRatePurchaseKind(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c758f77e-e3b3-4517-831a-af1bf0e1dceb"))
			{
				SingularName = "VatRatePurchaseKind",
				PluralName = "VatRatePurchaseKinds",
			};
        }

	}

    public partial class MetaVatRateUsage : MetaClass
	{
	    public static MetaVatRateUsage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType VatRatesWhereVatRateUsage;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVatRateUsage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("2e245d61-7739-4dfe-b108-7c9f0f4aed17"))
			{
				SingularName = "VatRateUsage",
				PluralName = "VatRateUsages",
			};
        }

	}

    public partial class MetaVatRegime : MetaClass
	{
	    public static MetaVatRegime Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType VatRate;
        public RoleType GeneralLedgerAccount;

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType InternalOrganisationWhereVatRegime;
        public AssociationType InvoiceVersionsWhereVatRegime;
        public AssociationType InvoicesWhereVatRegime;
        public AssociationType OrderVersionsWhereVatRegime;
        public AssociationType PartyVersionsWhereVatRegime;
        public AssociationType PriceableVersionsWhereVatRegime;
        public AssociationType PriceableVersionsWhereAssignedVatRegime;
        public AssociationType OrdersWhereVatRegime;
        public AssociationType PartiesWhereVatRegime;
        public AssociationType PriceablesWhereVatRegime;
        public AssociationType PriceablesWhereAssignedVatRegime;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVatRegime(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("69db99bc-97f7-4e2e-903c-74afb55992af"))
			{
				SingularName = "VatRegime",
				PluralName = "VatRegimes",
			};
        }

	}

    public partial class MetaVatReturnBox : MetaClass
	{
	    public static MetaVatReturnBox Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType HeaderNumber;
        public RoleType Description;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType VatFormWhereVatReturnBox;
        public AssociationType VatRatesWhereVatReturnBox;

		// Inherited Associations


		internal MetaVatReturnBox(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8dc67774-c15a-47dd-9b8a-ce4e7139e8a3"))
			{
				SingularName = "VatReturnBox",
				PluralName = "VatReturnBoxes",
			};
        }

	}

    public partial class MetaVatReturnBoxType : MetaClass
	{
	    public static MetaVatReturnBoxType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Type;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaVatReturnBoxType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b233161-d2a8-4d8f-a293-09d8a2bea3e2"))
			{
				SingularName = "VatReturnBoxType",
				PluralName = "VatReturnBoxTypes",
			};
        }

	}

    public partial class MetaVatTariff : MetaClass
	{
	    public static MetaVatTariff Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType VatRatesWhereVatTariff;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaVatTariff(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a3f63642-b397-4281-ba7e-8c77e9f30658"))
			{
				SingularName = "VatTariff",
				PluralName = "VatTariffs",
			};
        }

	}

    public partial class MetaVehicle : MetaClass
	{
	    public static MetaVehicle Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType Name;
        public ConcreteRoleType LastServiceDate;
        public ConcreteRoleType AcquiredDate;
        public ConcreteRoleType Description;
        public ConcreteRoleType ProductionCapacity;
        public ConcreteRoleType NextServiceDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType ShipmentRouteSegmentsWhereVehicle;

		// Inherited Associations
        public AssociationType DepreciationsWhereFixedAsset;
        public AssociationType PartyFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetAssignmentsWhereFixedAsset;
        public AssociationType WorkEffortFixedAssetStandardsWhereFixedAsset;
        public AssociationType WorkEffortTypesWhereFixedAssetToRepair;


		internal MetaVehicle(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0b476761-ad10-4e00-88bb-0e44b4574990"))
			{
				SingularName = "Vehicle",
				PluralName = "Vehicles",
			};
        }

	}

    public partial class MetaVolumeUsage : MetaClass
	{
	    public static MetaVolumeUsage Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Quantity;
        public RoleType UnitOfMeasure;

		// Inherited Roles
        public ConcreteRoleType TimeFrequency;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType DeploymentsWhereDeploymentUsage;


		internal MetaVolumeUsage(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("c219edcd-71dc-4f0b-afee-4b06f3d785be"))
			{
				SingularName = "VolumeUsage",
				PluralName = "VolumeUsages",
			};
        }

	}

    public partial class MetaWebAddress : MetaClass
	{
	    public static MetaWebAddress Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType ElectronicAddressString;
        public ConcreteRoleType Description;
        public ConcreteRoleType FollowTo;
        public ConcreteRoleType ContactMechanismType;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType PartyVersionsWhereGeneralEmail;
        public AssociationType PartyVersionWherePersonalEmailAddress;
        public AssociationType PartyVersionsWhereInternetAddress;
        public AssociationType PartiesWhereInternetAddress;
        public AssociationType BankAccountWhereContactMechanism;
        public AssociationType BillingAccountsWhereContactMechanism;
        public AssociationType EngagementsWherePlacingContactMechanism;
        public AssociationType EngagementsWhereBillToContactMechanism;
        public AssociationType EngagementsWhereTakenViaContactMechanism;
        public AssociationType FacilitiesWhereFacilityContactMechanism;
        public AssociationType InternalOrganisationsWhereBillingAddress;
        public AssociationType InternalOrganisationsWhereOrderAddress;
        public AssociationType InternalOrganisationsWhereBillingInquiriesFax;
        public AssociationType InternalOrganisationsWhereBillingInquiriesPhone;
        public AssociationType InternalOrganisationsWhereCellPhoneNumber;
        public AssociationType InternalOrganisationsWhereGeneralFaxNumber;
        public AssociationType InternalOrganisationsWhereGeneralPhoneNumber;
        public AssociationType InternalOrganisationsWhereHeadQuarter;
        public AssociationType InternalOrganisationsWhereInternetAddress;
        public AssociationType InternalOrganisationsWhereOrderInquiriesFax;
        public AssociationType InternalOrganisationsWhereOrderInquiriesPhone;
        public AssociationType InternalOrganisationsWhereGeneralEmailAddress;
        public AssociationType InternalOrganisationsWhereSalesOffice;
        public AssociationType InternalOrganisationsWhereShippingInquiriesFax;
        public AssociationType InternalOrganisationsWhereShippingInquiriesPhone;
        public AssociationType PurchaseOrderVersionsWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBillToContactMechanism;
        public AssociationType SalesInvoiceVersionsWhereBilledFromContactMechanism;
        public AssociationType SalesOrderVersionsWhereTakenByContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillToContactMechanism;
        public AssociationType SalesOrderVersionsWhereBillFromContactMechanism;
        public AssociationType SalesOrderVersionsWherePlacingContactMechanism;
        public AssociationType PartyContactMechanismsWhereContactMechanism;
        public AssociationType PurchaseOrdersWhereTakenViaContactMechanism;
        public AssociationType PurchaseOrdersWhereBillToContactMechanism;
        public AssociationType RespondingPartiesWhereContactMechanism;
        public AssociationType SalesInvoicesWhereBillToContactMechanism;
        public AssociationType SalesInvoicesWhereBilledFromContactMechanism;
        public AssociationType SalesOrdersWhereTakenByContactMechanism;
        public AssociationType SalesOrdersWhereBillToContactMechanism;
        public AssociationType SalesOrdersWhereBillFromContactMechanism;
        public AssociationType SalesOrdersWherePlacingContactMechanism;
        public AssociationType CommunicationEventVersionsWhereContactMechanism;
        public AssociationType CommunicationEventsWhereContactMechanism;
        public AssociationType ContactMechanismsWhereFollowTo;
        public AssociationType PartyVersionsWhereHomeAddress;
        public AssociationType PartyVersionsWhereSalesOffice;
        public AssociationType PartyVersionsWhereBillingAddress;
        public AssociationType PartyVersionsWhereHeadQuarter;
        public AssociationType PartyVersionsWhereOrderAddress;
        public AssociationType QuoteVersionsWhereFullfillContactMechanism;
        public AssociationType RequestVersionsWhereFullfillContactMechanism;
        public AssociationType PartiesWhereHomeAddress;
        public AssociationType PartiesWhereSalesOffice;
        public AssociationType PartiesWhereBillingAddress;
        public AssociationType PartiesWhereHeadQuarter;
        public AssociationType PartiesWhereOrderAddress;
        public AssociationType QuotesWhereFullfillContactMechanism;
        public AssociationType RequestsWhereFullfillContactMechanism;
        public AssociationType ShipmentVersionsWhereBillToContactMechanism;
        public AssociationType ShipmentVersionsWhereReceiverContactMechanism;
        public AssociationType ShipmentVersionsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentVersionsWhereBillFromContactMechanism;
        public AssociationType ShipmentsWhereBillToContactMechanism;
        public AssociationType ShipmentsWhereReceiverContactMechanism;
        public AssociationType ShipmentsWhereInquireAboutContactMechanism;
        public AssociationType ShipmentsWhereBillFromContactMechanism;


		internal MetaWebAddress(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("5138c0e3-1b28-4297-bf45-697624ee5c19"))
			{
				SingularName = "WebAddress",
				PluralName = "WebAddresses",
			};
        }

	}

    public partial class MetaWebSiteCommunication : MetaClass
	{
	    public static MetaWebSiteCommunication Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Cancel;
        public MethodType Close;
        public MethodType Reopen;
        public MethodType Delete;

		// Defined Roles
        public RoleType Originator;
        public RoleType Receiver;
        public RoleType CurrentVersion;
        public RoleType AllVersions;

		// Inherited Roles
        public ConcreteRoleType PreviousCommunicationEventState;
        public ConcreteRoleType LastCommunicationEventState;
        public ConcreteRoleType CommunicationEventState;
        public ConcreteRoleType PreviousObjectStates;
        public ConcreteRoleType LastObjectStates;
        public ConcreteRoleType ObjectStates;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ToParties;
        public ConcreteRoleType ContactMechanisms;
        public ConcreteRoleType InvolvedParties;
        public ConcreteRoleType InitialScheduledStart;
        public ConcreteRoleType EventPurposes;
        public ConcreteRoleType ScheduledEnd;
        public ConcreteRoleType ActualEnd;
        public ConcreteRoleType WorkEfforts;
        public ConcreteRoleType Description;
        public ConcreteRoleType InitialScheduledEnd;
        public ConcreteRoleType FromParties;
        public ConcreteRoleType Subject;
        public ConcreteRoleType Documents;
        public ConcreteRoleType Case;
        public ConcreteRoleType Priority;
        public ConcreteRoleType Owner;
        public ConcreteRoleType Note;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType SendNotification;
        public ConcreteRoleType SendReminder;
        public ConcreteRoleType RemindAt;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;
        public ConcreteRoleType UniqueId;
        public ConcreteRoleType CreatedBy;
        public ConcreteRoleType LastModifiedBy;
        public ConcreteRoleType CreationDate;
        public ConcreteRoleType LastModifiedDate;

		// Defined Associations

		// Inherited Associations
        public AssociationType RequirementCommunicationsWhereCommunicationEvent;
        public AssociationType PartyVersionsWhereCommunicationEvent;
        public AssociationType PartiesWhereCommunicationEvent;
        public AssociationType NotificationsWhereTarget;


		internal MetaWebSiteCommunication(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ecf2996a-7f8b-45d5-afac-56c88c62136a"))
			{
				SingularName = "WebSiteCommunication",
				PluralName = "WebSiteCommunications",
			};
        }

	}

    public partial class MetaWithdrawal : MetaClass
	{
	    public static MetaWithdrawal Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Disbursement;

		// Inherited Roles
        public ConcreteRoleType Description;
        public ConcreteRoleType EntryDate;
        public ConcreteRoleType TransactionDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations
        public AssociationType FinancialAccountWhereFinancialAccountTransaction;


		internal MetaWithdrawal(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("edf1788a-0c75-4635-904d-db9f9a6a7399"))
			{
				SingularName = "Withdrawal",
				PluralName = "Withdrawals",
			};
        }

	}

    public partial class MetaWorkEffortAssignment : MetaClass
	{
	    public static MetaWorkEffortAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;
        public MethodType Delete;

		// Defined Roles
        public RoleType Professional;
        public RoleType Assignment;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations

		// Inherited Associations


		internal MetaWorkEffortAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("33e9355b-b3db-43e0-a250-8ebc576e6221"))
			{
				SingularName = "WorkEffortAssignment",
				PluralName = "WorkEffortAssignments",
			};
        }

	}

    public partial class MetaWorkEffortAssignmentRate : MetaClass
	{
	    public static MetaWorkEffortAssignmentRate Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType RateType;
        public RoleType WorkEffortPartyAssignment;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations

		// Inherited Associations


		internal MetaWorkEffortAssignmentRate(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("ac18c87b-683c-4529-9171-d23e73c583d4"))
			{
				SingularName = "WorkEffortAssignmentRate",
				PluralName = "WorkEffortAssignmentRates",
			};
        }

	}

    public partial class MetaWorkEffortBilling : MetaClass
	{
	    public static MetaWorkEffortBilling Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType WorkEffort;
        public RoleType Percentage;
        public RoleType InvoiceItem;

		// Inherited Roles

		// Defined Associations

		// Inherited Associations


		internal MetaWorkEffortBilling(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("15c8c72b-f551-41b0-86c8-80f02424ec4c"))
			{
				SingularName = "WorkEffortBilling",
				PluralName = "WorkEffortBillings",
			};
        }

	}

    public partial class MetaWorkEffortFixedAssetAssignment : MetaClass
	{
	    public static MetaWorkEffortFixedAssetAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType AssetAssignmentStatus;
        public RoleType Assignment;
        public RoleType AllocatedCost;
        public RoleType FixedAsset;

		// Inherited Roles
        public ConcreteRoleType Comment;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;

		// Defined Associations

		// Inherited Associations


		internal MetaWorkEffortFixedAssetAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("3b43da7f-5252-4824-85fe-c85d6864838a"))
			{
				SingularName = "WorkEffortFixedAssetAssignment",
				PluralName = "WorkEffortFixedAssetAssignments",
			};
        }

	}

    public partial class MetaWorkEffortFixedAssetStandard : MetaClass
	{
	    public static MetaWorkEffortFixedAssetStandard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType EstimatedCost;
        public RoleType EstimatedDuration;
        public RoleType FixedAsset;
        public RoleType EstimatedQuantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortTypeWhereWorkEffortFixedAssetStandard;

		// Inherited Associations


		internal MetaWorkEffortFixedAssetStandard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("9b9f2a59-ae10-49df-b0b5-98b48ec99157"))
			{
				SingularName = "WorkEffortFixedAssetStandard",
				PluralName = "WorkEffortFixedAssetStandards",
			};
        }

	}

    public partial class MetaWorkEffortGoodStandard : MetaClass
	{
	    public static MetaWorkEffortGoodStandard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Good;
        public RoleType EstimatedCost;
        public RoleType EstimatedQuantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortTypeWhereWorkEffortGoodStandard;

		// Inherited Associations


		internal MetaWorkEffortGoodStandard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("81ddff76-9b82-4309-9c9f-f7f9dbd2db21"))
			{
				SingularName = "WorkEffortGoodStandard",
				PluralName = "WorkEffortGoodStandards",
			};
        }

	}

    public partial class MetaWorkEffortInventoryAssignment : MetaClass
	{
	    public static MetaWorkEffortInventoryAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Assignment;
        public RoleType InventoryItem;
        public RoleType Quantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortVersionsWhereInventoryItemsNeeded;
        public AssociationType WorkEffortWhereInventoryItemsNeeded;

		// Inherited Associations


		internal MetaWorkEffortInventoryAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f67e7755-5848-4601-ba70-4d1a39abfe4b"))
			{
				SingularName = "WorkEffortInventoryAssignment",
				PluralName = "WorkEffortInventoryAssignments",
			};
        }

	}

    public partial class MetaWorkEffortState : MetaClass
	{
	    public static MetaWorkEffortState Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType Name;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType WorkEffortVersionsWhereWorkEffortState;
        public AssociationType WorkEffortsWherePreviousWorkEffortState;
        public AssociationType WorkEffortsWhereLastWorkEffortState;
        public AssociationType WorkEffortsWhereWorkEffortState;

		// Inherited Associations
        public AssociationType TransitionsWhereFromState;
        public AssociationType TransitionsWhereToState;
        public AssociationType TransitionalsWherePreviousObjectState;
        public AssociationType TransitionalsWhereLastObjectState;
        public AssociationType TransitionalsWhereObjectState;
        public AssociationType TransitionalVersionsWherePreviousObjectState;
        public AssociationType TransitionalVersionsWhereLastObjectState;
        public AssociationType TransitionalVersionsWhereObjectState;
        public AssociationType NotificationsWhereTarget;


		internal MetaWorkEffortState(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("f7d24734-88d3-47e7-b718-8815914c9ad4"))
			{
				SingularName = "WorkEffortState",
				PluralName = "WorkEffortStates",
			};
        }

	}

    public partial class MetaWorkEffortPartStandard : MetaClass
	{
	    public static MetaWorkEffortPartStandard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Part;
        public RoleType EstimatedCost;
        public RoleType EstimatedQuantity;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortTypeWhereWorkEffortPartStandard;

		// Inherited Associations


		internal MetaWorkEffortPartStandard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("a12e5d28-e431-48d3-bbb1-8a2f5e3c4991"))
			{
				SingularName = "WorkEffortPartStandard",
				PluralName = "WorkEffortPartStandards",
			};
        }

	}

    public partial class MetaWorkEffortPartyAssignment : MetaClass
	{
	    public static MetaWorkEffortPartyAssignment Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Assignment;
        public RoleType Party;
        public RoleType Facility;

		// Inherited Roles
        public ConcreteRoleType FromDate;
        public ConcreteRoleType ThroughDate;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType Comment;

		// Defined Associations
        public AssociationType WorkEffortAssignmentRatesWhereWorkEffortPartyAssignment;

		// Inherited Associations


		internal MetaWorkEffortPartyAssignment(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("0bdfb093-35af-4c87-9c1c-05ed9dae6df6"))
			{
				SingularName = "WorkEffortPartyAssignment",
				PluralName = "WorkEffortPartyAssignments",
			};
        }

	}

    public partial class MetaWorkEffortPurpose : MetaClass
	{
	    public static MetaWorkEffortPurpose Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods
        public MethodType Delete;

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType WorkEffortVersionsWhereWorkEffortPurpose;
        public AssociationType WorkEffortsWhereWorkEffortPurpose;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaWorkEffortPurpose(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8f838542-cc98-4e95-8e60-fb3e774ba92e"))
			{
				SingularName = "WorkEffortPurpose",
				PluralName = "WorkEffortPurposes",
			};
        }

	}

    public partial class MetaWorkEffortSkillStandard : MetaClass
	{
	    public static MetaWorkEffortSkillStandard Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType Skill;
        public RoleType EstimatedNumberOfPeople;
        public RoleType EstimatedDuration;
        public RoleType EstimatedCost;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortTypeWhereWorkEffortSkillStandard;

		// Inherited Associations


		internal MetaWorkEffortSkillStandard(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("da16f5ee-0e04-41a7-afd7-b12e20414135"))
			{
				SingularName = "WorkEffortSkillStandard",
				PluralName = "WorkEffortSkillStandards",
			};
        }

	}

    public partial class MetaWorkEffortType : MetaClass
	{
	    public static MetaWorkEffortType Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType WorkEffortFixedAssetStandards;
        public RoleType WorkEffortGoodStandards;
        public RoleType Children;
        public RoleType FixedAssetToRepair;
        public RoleType Description;
        public RoleType Dependencies;
        public RoleType WorkEffortTypeKind;
        public RoleType WorkEffortPartStandards;
        public RoleType WorkEffortSkillStandards;
        public RoleType StandardWorkHours;
        public RoleType ProductToProduce;
        public RoleType DeliverableToProduce;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;

		// Defined Associations
        public AssociationType WorkEffortTypesWhereChild;
        public AssociationType WorkEffortTypesWhereDependency;
        public AssociationType WorkEffortVersionsWhereWorkEffortType;
        public AssociationType WorkEffortsWhereWorkEffortType;

		// Inherited Associations


		internal MetaWorkEffortType(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("7d2d9452-f250-47c3-81e0-4e1c0655cc86"))
			{
				SingularName = "WorkEffortType",
				PluralName = "WorkEffortTypes",
			};
        }

	}

    public partial class MetaWorkEffortTypeKind : MetaClass
	{
	    public static MetaWorkEffortTypeKind Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles

		// Inherited Roles
        public ConcreteRoleType LocalisedNames;
        public ConcreteRoleType Name;
        public ConcreteRoleType IsActive;
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType UniqueId;

		// Defined Associations
        public AssociationType WorkEffortTypesWhereWorkEffortTypeKind;

		// Inherited Associations
        public AssociationType NotificationsWhereTarget;


		internal MetaWorkEffortTypeKind(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("8551adf6-5a97-41fe-aff8-6bec08b09d08"))
			{
				SingularName = "WorkEffortTypeKind",
				PluralName = "WorkEffortTypeKinds",
			};
        }

	}

    public partial class MetaWorkTaskVersion : MetaClass
	{
	    public static MetaWorkTaskVersion Instance { get; internal set;}

		public override Class Class { get; }

		// Defined Methods

		// Inherited Methods
        public MethodType OnBuild;
        public MethodType OnPostBuild;
        public MethodType OnPreDerive;
        public MethodType OnDerive;
        public MethodType OnPostDerive;

		// Defined Roles
        public RoleType SendNotification;
        public RoleType SendReminder;
        public RoleType RemindAt;

		// Inherited Roles
        public ConcreteRoleType DeniedPermissions;
        public ConcreteRoleType SecurityTokens;
        public ConcreteRoleType WorkEffortState;
        public ConcreteRoleType OwnerSecurityToken;
        public ConcreteRoleType OwnerAccessControl;
        public ConcreteRoleType Name;
        public ConcreteRoleType Description;
        public ConcreteRoleType Priority;
        public ConcreteRoleType WorkEffortPurposes;
        public ConcreteRoleType ActualCompletion;
        public ConcreteRoleType ScheduledStart;
        public ConcreteRoleType ScheduledCompletion;
        public ConcreteRoleType ActualHours;
        public ConcreteRoleType EstimatedHours;
        public ConcreteRoleType Precendencies;
        public ConcreteRoleType Facility;
        public ConcreteRoleType DeliverablesProduced;
        public ConcreteRoleType ActualStart;
        public ConcreteRoleType InventoryItemsNeeded;
        public ConcreteRoleType Children;
        public ConcreteRoleType OrderItemFulfillment;
        public ConcreteRoleType WorkEffortType;
        public ConcreteRoleType InventoryItemsProduced;
        public ConcreteRoleType RequirementFulfillments;
        public ConcreteRoleType SpecialTerms;
        public ConcreteRoleType Concurrencies;
        public ConcreteRoleType DerivationId;
        public ConcreteRoleType DerivationTimeStamp;

		// Defined Associations
        public AssociationType WorkTaskWhereCurrentVersion;
        public AssociationType WorkTaskWhereAllVersion;

		// Inherited Associations


		internal MetaWorkTaskVersion(MetaPopulation metaPopulation)
        {
			this.Class = new Class(metaPopulation, new System.Guid("448FA98B-5683-4F0E-9745-AAA1093F5614"))
			{
				SingularName = "WorkTaskVersion",
				PluralName = "WorkTaskVersions",
			};
        }

	}
}