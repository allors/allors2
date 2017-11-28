import { ObjectTyped, RoleType, AssociationType, Data } from '@allors/framework';
export interface MetaDomain {
    Deletable: MetaDeletable;
    Enumeration: MetaEnumeration;
    UniquelyIdentifiable: MetaUniquelyIdentifiable;
    Version: MetaVersion;
    Localised: MetaLocalised;
    ObjectState: MetaObjectState;
    Task: MetaTask;
    User: MetaUser;
    WorkItem: MetaWorkItem;
    Agreement: MetaAgreement;
    AgreementTerm: MetaAgreementTerm;
    Auditable: MetaAuditable;
    Budget: MetaBudget;
    BudgetVersion: MetaBudgetVersion;
    Commentable: MetaCommentable;
    CommunicationEventVersion: MetaCommunicationEventVersion;
    CommunicationEvent: MetaCommunicationEvent;
    ContactMechanism: MetaContactMechanism;
    DeploymentUsage: MetaDeploymentUsage;
    Document: MetaDocument;
    ElectronicAddress: MetaElectronicAddress;
    EstimatedProductCost: MetaEstimatedProductCost;
    GeographicBoundary: MetaGeographicBoundary;
    GeographicBoundaryComposite: MetaGeographicBoundaryComposite;
    GeoLocatable: MetaGeoLocatable;
    InventoryItemVersion: MetaInventoryItemVersion;
    InventoryItem: MetaInventoryItem;
    InventoryItemConfiguration: MetaInventoryItemConfiguration;
    InvoiceItemVersion: MetaInvoiceItemVersion;
    InvoiceVersion: MetaInvoiceVersion;
    Invoice: MetaInvoice;
    InvoiceItem: MetaInvoiceItem;
    IUnitOfMeasure: MetaIUnitOfMeasure;
    OrderAdjustmentVersion: MetaOrderAdjustmentVersion;
    OrderItemVersion: MetaOrderItemVersion;
    OrderVersion: MetaOrderVersion;
    PartyVersion: MetaPartyVersion;
    PartyRelationship: MetaPartyRelationship;
    PriceableVersion: MetaPriceableVersion;
    QuoteVersion: MetaQuoteVersion;
    RequestVersion: MetaRequestVersion;
    Order: MetaOrder;
    OrderAdjustment: MetaOrderAdjustment;
    OrderItem: MetaOrderItem;
    OrganisationClassification: MetaOrganisationClassification;
    Part: MetaPart;
    PartBillOfMaterial: MetaPartBillOfMaterial;
    Party: MetaParty;
    PartyClassification: MetaPartyClassification;
    Payment: MetaPayment;
    PaymentMethod: MetaPaymentMethod;
    Period: MetaPeriod;
    PersonClassification: MetaPersonClassification;
    Priceable: MetaPriceable;
    PriceComponent: MetaPriceComponent;
    Printable: MetaPrintable;
    Product: MetaProduct;
    ProductAssociation: MetaProductAssociation;
    ProductFeature: MetaProductFeature;
    Quote: MetaQuote;
    Request: MetaRequest;
    Service: MetaService;
    ServiceEntry: MetaServiceEntry;
    ShipmentVersion: MetaShipmentVersion;
    Shipment: MetaShipment;
    WorkEffortVersion: MetaWorkEffortVersion;
    TermType: MetaTermType;
    WorkEffort: MetaWorkEffort;
    Counter: MetaCounter;
    Singleton: MetaSingleton;
    Media: MetaMedia;
    MediaContent: MetaMediaContent;
    Country: MetaCountry;
    Currency: MetaCurrency;
    Language: MetaLanguage;
    Locale: MetaLocale;
    LocalisedText: MetaLocalisedText;
    AccessControl: MetaAccessControl;
    Login: MetaLogin;
    Permission: MetaPermission;
    Role: MetaRole;
    SecurityToken: MetaSecurityToken;
    AutomatedAgent: MetaAutomatedAgent;
    Notification: MetaNotification;
    NotificationList: MetaNotificationList;
    Person: MetaPerson;
    TaskAssignment: MetaTaskAssignment;
    TaskList: MetaTaskList;
    UserGroup: MetaUserGroup;
    AccountingPeriod: MetaAccountingPeriod;
    AccountingPeriodVersion: MetaAccountingPeriodVersion;
    AccountingTransactionNumber: MetaAccountingTransactionNumber;
    AccountingTransactionType: MetaAccountingTransactionType;
    ActivityUsage: MetaActivityUsage;
    AssetAssignmentStatus: MetaAssetAssignmentStatus;
    AutomatedAgentVersion: MetaAutomatedAgentVersion;
    BankAccount: MetaBankAccount;
    BasePrice: MetaBasePrice;
    BillingAccount: MetaBillingAccount;
    BillOfLading: MetaBillOfLading;
    Brand: MetaBrand;
    BudgetItem: MetaBudgetItem;
    BudgetState: MetaBudgetState;
    BudgetReview: MetaBudgetReview;
    BudgetRevision: MetaBudgetRevision;
    CapitalBudget: MetaCapitalBudget;
    CapitalBudgetVersion: MetaCapitalBudgetVersion;
    Carrier: MetaCarrier;
    CaseVersion: MetaCaseVersion;
    Case: MetaCase;
    CaseState: MetaCaseState;
    Cash: MetaCash;
    Catalogue: MetaCatalogue;
    CatScope: MetaCatScope;
    ChartOfAccounts: MetaChartOfAccounts;
    Citizenship: MetaCitizenship;
    City: MetaCity;
    ClientAgreement: MetaClientAgreement;
    Colour: MetaColour;
    CommunicationEventState: MetaCommunicationEventState;
    CommunicationEventPurpose: MetaCommunicationEventPurpose;
    ContactMechanismPurpose: MetaContactMechanismPurpose;
    ContactMechanismType: MetaContactMechanismType;
    CostCenter: MetaCostCenter;
    CostCenterCategory: MetaCostCenterCategory;
    CostCenterSplitMethod: MetaCostCenterSplitMethod;
    CostOfGoodsSoldMethod: MetaCostOfGoodsSoldMethod;
    County: MetaCounty;
    CreditCard: MetaCreditCard;
    CustomerRelationship: MetaCustomerRelationship;
    CustomerReturn: MetaCustomerReturn;
    CustomerReturnState: MetaCustomerReturnState;
    CustomerReturnVersion: MetaCustomerReturnVersion;
    CustomerShipment: MetaCustomerShipment;
    CustomerShipmentState: MetaCustomerShipmentState;
    CustomerShipmentVersion: MetaCustomerShipmentVersion;
    CustomOrganisationClassification: MetaCustomOrganisationClassification;
    DebitCreditConstant: MetaDebitCreditConstant;
    DeductionType: MetaDeductionType;
    Deliverable: MetaDeliverable;
    DeliverableBasedService: MetaDeliverableBasedService;
    DeliverableTurnover: MetaDeliverableTurnover;
    DeliverableType: MetaDeliverableType;
    Deployment: MetaDeployment;
    Dimension: MetaDimension;
    Disbursement: MetaDisbursement;
    DiscountAdjustmentVersion: MetaDiscountAdjustmentVersion;
    DiscountAdjustment: MetaDiscountAdjustment;
    DiscountComponent: MetaDiscountComponent;
    DropShipment: MetaDropShipment;
    DropShipmentState: MetaDropShipmentState;
    DropShipmentVersion: MetaDropShipmentVersion;
    DunningType: MetaDunningType;
    EmailAddress: MetaEmailAddress;
    EmailCommunication: MetaEmailCommunication;
    EmailCommunicationVersion: MetaEmailCommunicationVersion;
    EmailTemplate: MetaEmailTemplate;
    Employment: MetaEmployment;
    EmploymentAgreement: MetaEmploymentAgreement;
    EmploymentApplicationSource: MetaEmploymentApplicationSource;
    EmploymentApplicationStatus: MetaEmploymentApplicationStatus;
    EmploymentTermination: MetaEmploymentTermination;
    EmploymentTerminationReason: MetaEmploymentTerminationReason;
    EngagementRate: MetaEngagementRate;
    EngineeringBom: MetaEngineeringBom;
    EngineeringChangeObjectState: MetaEngineeringChangeObjectState;
    EngineeringDocument: MetaEngineeringDocument;
    EstimatedLaborCost: MetaEstimatedLaborCost;
    EstimatedMaterialCost: MetaEstimatedMaterialCost;
    EstimatedOtherCost: MetaEstimatedOtherCost;
    EuSalesListType: MetaEuSalesListType;
    ExpenseEntry: MetaExpenseEntry;
    ExportDocument: MetaExportDocument;
    FaceToFaceCommunication: MetaFaceToFaceCommunication;
    FaceToFaceCommunicationVersion: MetaFaceToFaceCommunicationVersion;
    Facility: MetaFacility;
    FaxCommunication: MetaFaxCommunication;
    FaxCommunicationVersion: MetaFaxCommunicationVersion;
    Fee: MetaFee;
    FinancialTerm: MetaFinancialTerm;
    FinishedGood: MetaFinishedGood;
    FacilityType: MetaFacilityType;
    GenderType: MetaGenderType;
    GeneralLedgerAccount: MetaGeneralLedgerAccount;
    GeneralLedgerAccountGroup: MetaGeneralLedgerAccountGroup;
    GeneralLedgerAccountType: MetaGeneralLedgerAccountType;
    Good: MetaGood;
    HazardousMaterialsDocument: MetaHazardousMaterialsDocument;
    Hobby: MetaHobby;
    Incentive: MetaIncentive;
    IndustryClassification: MetaIndustryClassification;
    InternalOrganisation: MetaInternalOrganisation;
    InternalOrganisationRevenue: MetaInternalOrganisationRevenue;
    InventoryItemKind: MetaInventoryItemKind;
    InventoryItemVariance: MetaInventoryItemVariance;
    IncoTerm: MetaIncoTerm;
    IncoTermType: MetaIncoTermType;
    InvoiceSequence: MetaInvoiceSequence;
    InvoiceTerm: MetaInvoiceTerm;
    InvoiceVatRateItem: MetaInvoiceVatRateItem;
    ItemIssuance: MetaItemIssuance;
    JournalEntryNumber: MetaJournalEntryNumber;
    JournalType: MetaJournalType;
    LegalForm: MetaLegalForm;
    LegalTerm: MetaLegalTerm;
    LetterCorrespondence: MetaLetterCorrespondence;
    LetterCorrespondenceVersion: MetaLetterCorrespondenceVersion;
    Lot: MetaLot;
    Manifest: MetaManifest;
    ManufacturerSuggestedRetailPrice: MetaManufacturerSuggestedRetailPrice;
    ManufacturingBom: MetaManufacturingBom;
    ManufacturingConfiguration: MetaManufacturingConfiguration;
    MaritalStatus: MetaMaritalStatus;
    MarketingMaterial: MetaMarketingMaterial;
    MarketingPackage: MetaMarketingPackage;
    MaterialsUsage: MetaMaterialsUsage;
    Model: MetaModel;
    NeededSkill: MetaNeededSkill;
    NonSerialisedInventoryItem: MetaNonSerialisedInventoryItem;
    NonSerialisedInventoryItemState: MetaNonSerialisedInventoryItemState;
    NonSerialisedInventoryItemVersion: MetaNonSerialisedInventoryItemVersion;
    NonSerializedInventoryItemObjectState: MetaNonSerializedInventoryItemObjectState;
    OperatingBudgetVersion: MetaOperatingBudgetVersion;
    OrganisationVersion: MetaOrganisationVersion;
    PartSpecificationVersion: MetaPartSpecificationVersion;
    PartSpecificationType: MetaPartSpecificationType;
    Ownership: MetaOwnership;
    PersonVersion: MetaPersonVersion;
    PhoneCommunicationVersion: MetaPhoneCommunicationVersion;
    PickListVersion: MetaPickListVersion;
    ProcessFlow: MetaProcessFlow;
    ProductQuoteVersion: MetaProductQuoteVersion;
    ProposalVersion: MetaProposalVersion;
    PurchaseInvoiceItemVersion: MetaPurchaseInvoiceItemVersion;
    PurchaseInvoiceVersion: MetaPurchaseInvoiceVersion;
    PurchaseOrderItemVersion: MetaPurchaseOrderItemVersion;
    PurchaseOrderPaymentState: MetaPurchaseOrderPaymentState;
    PurchaseOrderShipmentState: MetaPurchaseOrderShipmentState;
    PurchaseOrderVersion: MetaPurchaseOrderVersion;
    PurchaseReturnVersion: MetaPurchaseReturnVersion;
    PurchaseShipmentVersion: MetaPurchaseShipmentVersion;
    QuoteItemVersion: MetaQuoteItemVersion;
    RequestForInformationVersion: MetaRequestForInformationVersion;
    RequestForProposalVersion: MetaRequestForProposalVersion;
    RequestForQuoteVersion: MetaRequestForQuoteVersion;
    RequestItemVersion: MetaRequestItemVersion;
    RequirementVersion: MetaRequirementVersion;
    SalesInvoiceItemVersion: MetaSalesInvoiceItemVersion;
    SalesInvoiceVersion: MetaSalesInvoiceVersion;
    SalesOrderItemShipmentState: MetaSalesOrderItemShipmentState;
    SalesOrderItemPaymentState: MetaSalesOrderItemPaymentState;
    SalesOrderItemVersion: MetaSalesOrderItemVersion;
    SalesOrderPaymentState: MetaSalesOrderPaymentState;
    SalesOrderShipmentState: MetaSalesOrderShipmentState;
    SalesOrderVersion: MetaSalesOrderVersion;
    SerialisedInventoryItemVersion: MetaSerialisedInventoryItemVersion;
    OneTimeCharge: MetaOneTimeCharge;
    OperatingBudget: MetaOperatingBudget;
    OrderKind: MetaOrderKind;
    OrderShipment: MetaOrderShipment;
    OrderTerm: MetaOrderTerm;
    Ordinal: MetaOrdinal;
    Organisation: MetaOrganisation;
    OrganisationContactKind: MetaOrganisationContactKind;
    OrganisationContactRelationship: MetaOrganisationContactRelationship;
    OrganisationGlAccount: MetaOrganisationGlAccount;
    OrganisationRole: MetaOrganisationRole;
    OrganisationRollUp: MetaOrganisationRollUp;
    OrganisationUnit: MetaOrganisationUnit;
    OwnBankAccount: MetaOwnBankAccount;
    OwnCreditCard: MetaOwnCreditCard;
    Package: MetaPackage;
    PackageRevenue: MetaPackageRevenue;
    PackagingSlip: MetaPackagingSlip;
    PartBillOfMaterialSubstitute: MetaPartBillOfMaterialSubstitute;
    PartRevision: MetaPartRevision;
    PartSpecification: MetaPartSpecification;
    PartSpecificationState: MetaPartSpecificationState;
    PartSubstitute: MetaPartSubstitute;
    PartyContactMechanism: MetaPartyContactMechanism;
    PartyFixedAssetAssignment: MetaPartyFixedAssetAssignment;
    PartyPackageRevenue: MetaPartyPackageRevenue;
    PartyProductCategoryRevenue: MetaPartyProductCategoryRevenue;
    PartyProductRevenue: MetaPartyProductRevenue;
    PartyRevenue: MetaPartyRevenue;
    PartySkill: MetaPartySkill;
    Passport: MetaPassport;
    PayCheck: MetaPayCheck;
    PayGrade: MetaPayGrade;
    PayHistory: MetaPayHistory;
    PayrollPreference: MetaPayrollPreference;
    PerformanceNote: MetaPerformanceNote;
    PerformanceReview: MetaPerformanceReview;
    PerformanceReviewItem: MetaPerformanceReviewItem;
    PerformanceReviewItemType: MetaPerformanceReviewItemType;
    PersonalTitle: MetaPersonalTitle;
    PersonRole: MetaPersonRole;
    PersonTraining: MetaPersonTraining;
    PhoneCommunication: MetaPhoneCommunication;
    PickList: MetaPickList;
    PickListItem: MetaPickListItem;
    PickListState: MetaPickListState;
    PositionFulfillment: MetaPositionFulfillment;
    PositionReportingStructure: MetaPositionReportingStructure;
    PositionResponsibility: MetaPositionResponsibility;
    PositionStatus: MetaPositionStatus;
    PostalAddress: MetaPostalAddress;
    PostalBoundary: MetaPostalBoundary;
    PostalCode: MetaPostalCode;
    Priority: MetaPriority;
    ProductCategory: MetaProductCategory;
    ProductCategoryRevenue: MetaProductCategoryRevenue;
    ProductCharacteristic: MetaProductCharacteristic;
    ProductCharacteristicValue: MetaProductCharacteristicValue;
    ProductConfiguration: MetaProductConfiguration;
    ProductDrawing: MetaProductDrawing;
    ProductModel: MetaProductModel;
    ProductPurchasePrice: MetaProductPurchasePrice;
    ProductQuality: MetaProductQuality;
    ProductQuote: MetaProductQuote;
    ProductRevenue: MetaProductRevenue;
    ProductType: MetaProductType;
    ProfessionalAssignment: MetaProfessionalAssignment;
    ProfessionalServicesRelationship: MetaProfessionalServicesRelationship;
    Proposal: MetaProposal;
    Province: MetaProvince;
    PurchaseAgreement: MetaPurchaseAgreement;
    PurchaseInvoice: MetaPurchaseInvoice;
    PurchaseInvoiceItem: MetaPurchaseInvoiceItem;
    PurchaseInvoiceItemState: MetaPurchaseInvoiceItemState;
    PurchaseInvoiceItemType: MetaPurchaseInvoiceItemType;
    PurchaseInvoiceState: MetaPurchaseInvoiceState;
    PurchaseInvoiceType: MetaPurchaseInvoiceType;
    PurchaseOrder: MetaPurchaseOrder;
    PurchaseOrderItem: MetaPurchaseOrderItem;
    PurchaseOrderItemState: MetaPurchaseOrderItemState;
    PurchaseOrderState: MetaPurchaseOrderState;
    PurchaseReturn: MetaPurchaseReturn;
    PurchaseReturnState: MetaPurchaseReturnState;
    PurchaseShipment: MetaPurchaseShipment;
    PurchaseShipmentState: MetaPurchaseShipmentState;
    Qualification: MetaQualification;
    QuoteItem: MetaQuoteItem;
    QuoteTerm: MetaQuoteTerm;
    RateType: MetaRateType;
    RatingType: MetaRatingType;
    RawMaterial: MetaRawMaterial;
    Receipt: MetaReceipt;
    RecurringCharge: MetaRecurringCharge;
    Region: MetaRegion;
    RequestForInformation: MetaRequestForInformation;
    RequestForProposal: MetaRequestForProposal;
    RequestForQuote: MetaRequestForQuote;
    RequestItem: MetaRequestItem;
    QuoteState: MetaQuoteState;
    QuoteItemState: MetaQuoteItemState;
    Requirement: MetaRequirement;
    RequirementState: MetaRequirementState;
    RespondingParty: MetaRespondingParty;
    Resume: MetaResume;
    SalesAgreement: MetaSalesAgreement;
    SalesChannel: MetaSalesChannel;
    SalesChannelRevenue: MetaSalesChannelRevenue;
    SalesInvoice: MetaSalesInvoice;
    SalesInvoiceItem: MetaSalesInvoiceItem;
    SalesInvoiceItemState: MetaSalesInvoiceItemState;
    SalesInvoiceItemType: MetaSalesInvoiceItemType;
    SalesInvoiceState: MetaSalesInvoiceState;
    SalesInvoiceType: MetaSalesInvoiceType;
    SalesOrder: MetaSalesOrder;
    SalesOrderItem: MetaSalesOrderItem;
    RequestItemState: MetaRequestItemState;
    SalesOrderItemState: MetaSalesOrderItemState;
    RequestState: MetaRequestState;
    SalesOrderState: MetaSalesOrderState;
    SalesRepPartyProductCategoryRevenue: MetaSalesRepPartyProductCategoryRevenue;
    SalesRepPartyRevenue: MetaSalesRepPartyRevenue;
    SalesRepProductCategoryRevenue: MetaSalesRepProductCategoryRevenue;
    SalesRepRelationship: MetaSalesRepRelationship;
    SalesRepRevenue: MetaSalesRepRevenue;
    SalesTerritory: MetaSalesTerritory;
    Salutation: MetaSalutation;
    SerialisedInventoryItem: MetaSerialisedInventoryItem;
    SerialisedInventoryItemState: MetaSerialisedInventoryItemState;
    SerializedInventoryItemObjectState: MetaSerializedInventoryItemObjectState;
    ServiceConfiguration: MetaServiceConfiguration;
    ServiceEntryHeader: MetaServiceEntryHeader;
    ServiceFeature: MetaServiceFeature;
    ServiceTerritory: MetaServiceTerritory;
    ShipmentItem: MetaShipmentItem;
    ShipmentMethod: MetaShipmentMethod;
    ShipmentPackage: MetaShipmentPackage;
    ShipmentRouteSegment: MetaShipmentRouteSegment;
    ShippingAndHandlingCharge: MetaShippingAndHandlingCharge;
    ShippingAndHandlingComponent: MetaShippingAndHandlingComponent;
    Size: MetaSize;
    Skill: MetaSkill;
    SkillLevel: MetaSkillLevel;
    SkillRating: MetaSkillRating;
    SoftwareFeature: MetaSoftwareFeature;
    State: MetaState;
    StatementOfWork: MetaStatementOfWork;
    StatementOfWorkVersion: MetaStatementOfWorkVersion;
    Store: MetaStore;
    StoreRevenue: MetaStoreRevenue;
    StringTemplate: MetaStringTemplate;
    SubAssembly: MetaSubAssembly;
    SubContractorAgreement: MetaSubContractorAgreement;
    SubContractorRelationship: MetaSubContractorRelationship;
    SupplierOffering: MetaSupplierOffering;
    SupplierRelationship: MetaSupplierRelationship;
    SurchargeAdjustment: MetaSurchargeAdjustment;
    SurchargeComponent: MetaSurchargeComponent;
    RequirementType: MetaRequirementType;
    InvoiceTermType: MetaInvoiceTermType;
    TransferVersion: MetaTransferVersion;
    WebSiteCommunicationVersion: MetaWebSiteCommunicationVersion;
    WorkTask: MetaWorkTask;
    TaxDocument: MetaTaxDocument;
    TelecommunicationsNumber: MetaTelecommunicationsNumber;
    Territory: MetaTerritory;
    Threshold: MetaThreshold;
    TimeAndMaterialsService: MetaTimeAndMaterialsService;
    TimeEntry: MetaTimeEntry;
    TimeFrequency: MetaTimeFrequency;
    TimePeriodUsage: MetaTimePeriodUsage;
    Transfer: MetaTransfer;
    TransferState: MetaTransferState;
    UnitOfMeasure: MetaUnitOfMeasure;
    UtilizationCharge: MetaUtilizationCharge;
    VarianceReason: MetaVarianceReason;
    VatCalculationMethod: MetaVatCalculationMethod;
    VatForm: MetaVatForm;
    VatRate: MetaVatRate;
    VatRatePurchaseKind: MetaVatRatePurchaseKind;
    VatRateUsage: MetaVatRateUsage;
    VatRegime: MetaVatRegime;
    VatReturnBox: MetaVatReturnBox;
    VatTariff: MetaVatTariff;
    VolumeUsage: MetaVolumeUsage;
    WebAddress: MetaWebAddress;
    WebSiteCommunication: MetaWebSiteCommunication;
    WorkEffortAssignment: MetaWorkEffortAssignment;
    WorkEffortFixedAssetAssignment: MetaWorkEffortFixedAssetAssignment;
    WorkEffortInventoryAssignment: MetaWorkEffortInventoryAssignment;
    WorkEffortState: MetaWorkEffortState;
    WorkEffortPartyAssignment: MetaWorkEffortPartyAssignment;
    WorkEffortPurpose: MetaWorkEffortPurpose;
    WorkEffortType: MetaWorkEffortType;
    WorkEffortTypeKind: MetaWorkEffortTypeKind;
    WorkTaskVersion: MetaWorkTaskVersion;
}
export interface MetaDeletable extends ObjectTyped {
}
export interface MetaEnumeration extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaUniquelyIdentifiable extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
}
export interface MetaLocalised extends ObjectTyped {
    Locale: RoleType;
}
export interface MetaObjectState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaTask extends ObjectTyped {
    WorkItem: RoleType;
    DateCreated: RoleType;
    DateClosed: RoleType;
    Participants: RoleType;
    Performer: RoleType;
    UniqueId: RoleType;
    TaskAssignmentsWhereTask: AssociationType;
}
export interface MetaUser extends ObjectTyped {
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
    AuditablesWhereCreatedBy: AssociationType;
    AuditablesWhereLastModifiedBy: AssociationType;
    CommunicationEventVersionsWhereCreatedBy: AssociationType;
    CommunicationEventVersionsWhereLastModifiedBy: AssociationType;
    InvoiceVersionsWhereCreatedBy: AssociationType;
    InvoiceVersionsWhereLastModifiedBy: AssociationType;
    OrderVersionsWhereCreatedBy: AssociationType;
    OrderVersionsWhereLastModifiedBy: AssociationType;
    PartyVersionsWhereCreatedBy: AssociationType;
    PartyVersionsWhereLastModifiedBy: AssociationType;
}
export interface MetaWorkItem extends ObjectTyped {
    WorkItemDescription: RoleType;
    TasksWhereWorkItem: AssociationType;
}
export interface MetaAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaAgreementTerm extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaAuditable extends ObjectTyped {
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaBudget extends ObjectTyped {
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaBudgetVersion extends ObjectTyped {
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    Description: RoleType;
    BudgetRevisions: RoleType;
    BudgetNumber: RoleType;
    BudgetReviews: RoleType;
    BudgetItems: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaCommentable extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaCommunicationEventVersion extends ObjectTyped {
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaCommunicationEvent extends ObjectTyped {
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaContactMechanism extends ObjectTyped {
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaDeploymentUsage extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaDocument extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaElectronicAddress extends ObjectTyped {
    ElectronicAddressString: RoleType;
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyVersionsWhereGeneralEmail: AssociationType;
    PartyVersionWherePersonalEmailAddress: AssociationType;
    PartyVersionsWhereInternetAddress: AssociationType;
    PartiesWhereInternetAddress: AssociationType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaEstimatedProductCost extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    ProductWhereEstimatedProductCost: AssociationType;
    ProductFeatureWhereEstimatedProductCost: AssociationType;
}
export interface MetaGeographicBoundary extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaGeographicBoundaryComposite extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaGeoLocatable extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaInventoryItemVersion extends ObjectTyped {
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaInventoryItem extends ObjectTyped {
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    UniqueId: RoleType;
    WorkEffortVersionsWhereInventoryItemsProduced: AssociationType;
    WorkEffortsWhereInventoryItemsProduced: AssociationType;
}
export interface MetaInventoryItemConfiguration extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaInvoiceItemVersion extends ObjectTyped {
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaInvoiceVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaInvoice extends ObjectTyped {
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    ContactPerson: RoleType;
    Locale: RoleType;
    Comment: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaInvoiceItem extends ObjectTyped {
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    InvoiceItemVersionsWhereAdjustmentFor: AssociationType;
    InvoiceItemsWhereAdjustmentFor: AssociationType;
}
export interface MetaIUnitOfMeasure extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaOrderAdjustmentVersion extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaOrderItemVersion extends ObjectTyped {
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaOrderVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaPartyVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaPartyRelationship extends ObjectTyped {
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPriceableVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
}
export interface MetaQuoteVersion extends ObjectTyped {
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaRequestVersion extends ObjectTyped {
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaOrder extends ObjectTyped {
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    Description: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    Locale: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaOrderAdjustment extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
}
export interface MetaOrderItem extends ObjectTyped {
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    SalesOrderItemVersionsWhereOrderedWithFeature: AssociationType;
    SalesOrderItemWhereOrderedWithFeature: AssociationType;
    OrderItemVersionsWhereAssociation: AssociationType;
    OrderVersionsWhereValidOrderItem: AssociationType;
    OrderWhereValidOrderItem: AssociationType;
    OrderItemsWhereAssociation: AssociationType;
}
export interface MetaOrganisationClassification extends ObjectTyped {
    Name: RoleType;
    OrganisationVersionsWhereOrganisationClassification: AssociationType;
    OrganisationsWhereOrganisationClassification: AssociationType;
    PartyVersionsWherePartyClassification: AssociationType;
    PartiesWherePartyClassification: AssociationType;
}
export interface MetaPart extends ObjectTyped {
    UniqueId: RoleType;
    InventoryItemVersionsWherePart: AssociationType;
    InventoryItemsWherePart: AssociationType;
}
export interface MetaPartBillOfMaterial extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaParty extends ObjectTyped {
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    Locale: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    UniqueId: RoleType;
    Comment: RoleType;
    CustomerRelationshipsWhereCustomer: AssociationType;
    FaceToFaceCommunicationsWhereParticipant: AssociationType;
    FaceToFaceCommunicationVersionsWhereParticipant: AssociationType;
    FaxCommunicationsWhereOriginator: AssociationType;
    FaxCommunicationsWhereReceiver: AssociationType;
    FaxCommunicationVersionsWhereOriginator: AssociationType;
    FaxCommunicationVersionsWhereReceiver: AssociationType;
    GoodsWhereManufacturedBy: AssociationType;
    GoodsWhereSuppliedBy: AssociationType;
    InternalOrganisationWhereCurrentCustomer: AssociationType;
    InternalOrganisationWhereActiveCustomer: AssociationType;
    InternalOrganisationWhereActiveSupplier: AssociationType;
    LetterCorrespondencesWhereOriginator: AssociationType;
    LetterCorrespondencesWhereReceiver: AssociationType;
    LetterCorrespondenceVersionsWhereOriginator: AssociationType;
    LetterCorrespondenceVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereCaller: AssociationType;
    PickListVersionsWhereShipToParty: AssociationType;
    QuoteItemVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereNeededFor: AssociationType;
    RequirementVersionsWhereOriginator: AssociationType;
    RequirementVersionsWhereServicedBy: AssociationType;
    SalesInvoiceVersionsWhereBillToCustomer: AssociationType;
    SalesInvoiceVersionsWhereShipToCustomer: AssociationType;
    SalesInvoiceVersionsWhereCustomer: AssociationType;
    SalesOrderItemVersionsWhereShipToParty: AssociationType;
    SalesOrderItemVersionsWhereAssignedShipToParty: AssociationType;
    SalesOrderVersionsWhereShipToCustomer: AssociationType;
    SalesOrderVersionsWhereBillToCustomer: AssociationType;
    SalesOrderVersionsWherePlacingCustomer: AssociationType;
    OrganisationGlAccountsWhereParty: AssociationType;
    PhoneCommunicationsWhereReceiver: AssociationType;
    PhoneCommunicationsWhereCaller: AssociationType;
    PickListsWhereShipToParty: AssociationType;
    QuoteItemsWhereAuthorizer: AssociationType;
    RequirementsWhereAuthorizer: AssociationType;
    RequirementsWhereNeededFor: AssociationType;
    RequirementsWhereOriginator: AssociationType;
    RequirementsWhereServicedBy: AssociationType;
    SalesInvoicesWhereBillToCustomer: AssociationType;
    SalesInvoicesWhereShipToCustomer: AssociationType;
    SalesInvoicesWhereCustomer: AssociationType;
    SalesOrdersWhereShipToCustomer: AssociationType;
    SalesOrdersWhereBillToCustomer: AssociationType;
    SalesOrdersWherePlacingCustomer: AssociationType;
    SalesOrderItemsWhereShipToParty: AssociationType;
    SalesOrderItemsWhereAssignedShipToParty: AssociationType;
    SalesRepRelationshipsWhereCustomer: AssociationType;
    SubContractorRelationshipsWhereContractor: AssociationType;
    SubContractorRelationshipsWhereSubContractor: AssociationType;
    WebSiteCommunicationVersionsWhereOriginator: AssociationType;
    WebSiteCommunicationVersionsWhereReceiver: AssociationType;
    WebSiteCommunicationsWhereOriginator: AssociationType;
    WebSiteCommunicationsWhereReceiver: AssociationType;
    CommunicationEventVersionsWhereToParty: AssociationType;
    CommunicationEventVersionsWhereInvolvedParty: AssociationType;
    CommunicationEventVersionsWhereFromParty: AssociationType;
    CommunicationEventsWhereToParty: AssociationType;
    CommunicationEventsWhereInvolvedParty: AssociationType;
    CommunicationEventsWhereFromParty: AssociationType;
    PartyRelationshipsWhereParty: AssociationType;
    QuoteVersionsWhereReceiver: AssociationType;
    RequestVersionsWhereOriginator: AssociationType;
    QuotesWhereReceiver: AssociationType;
    RequestsWhereOriginator: AssociationType;
    ShipmentVersionsWhereBillToParty: AssociationType;
    ShipmentVersionsWhereShipToParty: AssociationType;
    ShipmentVersionsWhereShipFromParty: AssociationType;
}
export interface MetaPartyClassification extends ObjectTyped {
    Name: RoleType;
    PartyVersionsWherePartyClassification: AssociationType;
    PartiesWherePartyClassification: AssociationType;
}
export interface MetaPayment extends ObjectTyped {
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaPaymentMethod extends ObjectTyped {
    UniqueId: RoleType;
    CustomerShipmentVersionsWherePaymentMethod: AssociationType;
    InternalOrganisationWhereActivePaymentMethod: AssociationType;
    InternalOrganisationWhereDefaultPaymentMethod: AssociationType;
    SalesInvoiceVersionsWherePaymentMethod: AssociationType;
    SalesOrderVersionsWherePaymentMethod: AssociationType;
    SalesInvoicesWherePaymentMethod: AssociationType;
    SalesOrdersWherePaymentMethod: AssociationType;
    StoresWhereDefaultPaymentMethod: AssociationType;
    StoresWherePaymentMethod: AssociationType;
    PartyVersionsWhereDefaultPaymentMethod: AssociationType;
    PartiesWhereDefaultPaymentMethod: AssociationType;
}
export interface MetaPeriod extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPersonClassification extends ObjectTyped {
    Name: RoleType;
    PeopleWherePersonClassification: AssociationType;
    PersonVersionsWherePersonClassification: AssociationType;
    PartyVersionsWherePartyClassification: AssociationType;
    PartiesWherePartyClassification: AssociationType;
}
export interface MetaPriceable extends ObjectTyped {
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
}
export interface MetaPriceComponent extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaPrintable extends ObjectTyped {
    PrintContent: RoleType;
}
export interface MetaProduct extends ObjectTyped {
    InternalComment: RoleType;
    PrimaryProductCategory: RoleType;
    SupportDiscontinuationDate: RoleType;
    SalesDiscontinuationDate: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    LocalisedComments: RoleType;
    Description: RoleType;
    ProductComplement: RoleType;
    OptionalFeatures: RoleType;
    Variants: RoleType;
    Name: RoleType;
    IntroductionDate: RoleType;
    Documents: RoleType;
    StandardFeatures: RoleType;
    UnitOfMeasure: RoleType;
    EstimatedProductCosts: RoleType;
    ProductObsolescences: RoleType;
    SelectableFeatures: RoleType;
    VatRate: RoleType;
    BasePrices: RoleType;
    ProductCategories: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereDefaultCostUnit: AssociationType;
    GeneralLedgerAccountsWhereCostUnitsAllowed: AssociationType;
    GoodsWhereProductSubstitution: AssociationType;
    GoodsWhereProductIncompatibility: AssociationType;
    QuoteItemVersionsWhereProduct: AssociationType;
    RequestItemVersionsWhereProduct: AssociationType;
    SalesInvoiceItemVersionsWhereProduct: AssociationType;
    SalesOrderItemVersionsWhereProduct: AssociationType;
    OrganisationGlAccountsWhereProduct: AssociationType;
    ProductCategoriesWhereAllProduct: AssociationType;
    QuoteItemsWhereProduct: AssociationType;
    RequestItemsWhereProduct: AssociationType;
    SalesInvoiceItemsWhereProduct: AssociationType;
    SalesOrderItemsWhereProduct: AssociationType;
    ProductsWhereProductComplement: AssociationType;
    ProductWhereVariant: AssociationType;
    ProductsWhereProductObsolescence: AssociationType;
}
export interface MetaProductAssociation extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaProductFeature extends ObjectTyped {
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaQuote extends ObjectTyped {
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    Comment: RoleType;
}
export interface MetaRequest extends ObjectTyped {
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    ContactPerson: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PrintContent: RoleType;
    QuoteVersionsWhereRequest: AssociationType;
    QuoteWhereRequest: AssociationType;
}
export interface MetaService extends ObjectTyped {
    InternalComment: RoleType;
    PrimaryProductCategory: RoleType;
    SupportDiscontinuationDate: RoleType;
    SalesDiscontinuationDate: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    LocalisedComments: RoleType;
    Description: RoleType;
    ProductComplement: RoleType;
    OptionalFeatures: RoleType;
    Variants: RoleType;
    Name: RoleType;
    IntroductionDate: RoleType;
    Documents: RoleType;
    StandardFeatures: RoleType;
    UnitOfMeasure: RoleType;
    EstimatedProductCosts: RoleType;
    ProductObsolescences: RoleType;
    SelectableFeatures: RoleType;
    VatRate: RoleType;
    BasePrices: RoleType;
    ProductCategories: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereDefaultCostUnit: AssociationType;
    GeneralLedgerAccountsWhereCostUnitsAllowed: AssociationType;
    GoodsWhereProductSubstitution: AssociationType;
    GoodsWhereProductIncompatibility: AssociationType;
    QuoteItemVersionsWhereProduct: AssociationType;
    RequestItemVersionsWhereProduct: AssociationType;
    SalesInvoiceItemVersionsWhereProduct: AssociationType;
    SalesOrderItemVersionsWhereProduct: AssociationType;
    OrganisationGlAccountsWhereProduct: AssociationType;
    ProductCategoriesWhereAllProduct: AssociationType;
    QuoteItemsWhereProduct: AssociationType;
    RequestItemsWhereProduct: AssociationType;
    SalesInvoiceItemsWhereProduct: AssociationType;
    SalesOrderItemsWhereProduct: AssociationType;
    ProductsWhereProductComplement: AssociationType;
    ProductWhereVariant: AssociationType;
    ProductsWhereProductObsolescence: AssociationType;
}
export interface MetaServiceEntry extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaShipmentVersion extends ObjectTyped {
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaShipment extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaWorkEffortVersion extends ObjectTyped {
    Name: RoleType;
    Description: RoleType;
    Priority: RoleType;
    WorkEffortPurposes: RoleType;
    ActualCompletion: RoleType;
    ScheduledStart: RoleType;
    ScheduledCompletion: RoleType;
    ActualHours: RoleType;
    EstimatedHours: RoleType;
    Precendencies: RoleType;
    Facility: RoleType;
    DeliverablesProduced: RoleType;
    ActualStart: RoleType;
    InventoryItemsNeeded: RoleType;
    Children: RoleType;
    WorkEffortType: RoleType;
    InventoryItemsProduced: RoleType;
    RequirementFulfillments: RoleType;
    SpecialTerms: RoleType;
    Concurrencies: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaTermType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    OrderTermsWhereTermType: AssociationType;
    AgreementTermsWhereTermType: AssociationType;
}
export interface MetaWorkEffort extends ObjectTyped {
    WorkEffortState: RoleType;
    Owner: RoleType;
    Name: RoleType;
    Description: RoleType;
    Priority: RoleType;
    WorkEffortPurposes: RoleType;
    ActualCompletion: RoleType;
    ScheduledStart: RoleType;
    ScheduledCompletion: RoleType;
    ActualHours: RoleType;
    EstimatedHours: RoleType;
    Precendencies: RoleType;
    Facility: RoleType;
    DeliverablesProduced: RoleType;
    ActualStart: RoleType;
    InventoryItemsNeeded: RoleType;
    Children: RoleType;
    WorkEffortType: RoleType;
    InventoryItemsProduced: RoleType;
    RequirementFulfillments: RoleType;
    SpecialTerms: RoleType;
    Concurrencies: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    QuoteItemVersionsWhereWorkEffort: AssociationType;
    QuoteItemsWhereWorkEffort: AssociationType;
    WorkEffortAssignmentsWhereAssignment: AssociationType;
    CommunicationEventVersionsWhereWorkEffort: AssociationType;
    CommunicationEventsWhereWorkEffort: AssociationType;
    WorkEffortVersionsWherePrecendency: AssociationType;
    WorkEffortVersionsWhereChild: AssociationType;
    WorkEffortVersionsWhereConcurrency: AssociationType;
    WorkEffortsWherePrecendency: AssociationType;
    WorkEffortsWhereChild: AssociationType;
    WorkEffortsWhereConcurrency: AssociationType;
}
export interface MetaCounter extends ObjectTyped {
    UniqueId: RoleType;
    InternalOrganisationsWherePurchaseInvoiceCounter: AssociationType;
    InternalOrganisationsWherePurchaseOrderCounter: AssociationType;
    InternalOrganisationsWhereAccountingTransactionCounter: AssociationType;
    InternalOrganisationsWhereIncomingShipmentCounter: AssociationType;
    InternalOrganisationsWhereSubAccountCounter: AssociationType;
    InternalOrganisationsWhereQuoteCounter: AssociationType;
    InternalOrganisationsWhereRequestCounter: AssociationType;
}
export interface MetaSingleton extends ObjectTyped {
    DefaultLocale: RoleType;
    Locales: RoleType;
    Guest: RoleType;
    PreferredCurrency: RoleType;
    NoImageAvailableImage: RoleType;
    InternalOrganisation: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaMedia extends ObjectTyped {
    Revision: RoleType;
    MediaContent: RoleType;
    InData: RoleType;
    InDataUri: RoleType;
    FileName: RoleType;
    Type: RoleType;
    UniqueId: RoleType;
    SingletonsWhereNoImageAvailableImage: AssociationType;
    PersonWherePicture: AssociationType;
    CataloguesWhereCatalogueImage: AssociationType;
    GoodWherePrimaryPhoto: AssociationType;
    GoodsWherePhoto: AssociationType;
    InternalOrganisationsWhereLogoImage: AssociationType;
    OrganisationVersionsWhereLogoImage: AssociationType;
    PersonVersionWherePicture: AssociationType;
    OrganisationsWhereLogoImage: AssociationType;
    ProductCategoriesWhereCategoryImage: AssociationType;
    StoresWhereLogoImage: AssociationType;
    CommunicationEventVersionsWhereDocument: AssociationType;
    CommunicationEventWhereDocument: AssociationType;
    PartyVersionWhereContent: AssociationType;
    PartyWhereContent: AssociationType;
}
export interface MetaMediaContent extends ObjectTyped {
    Type: RoleType;
    Data: RoleType;
    MediaWhereMediaContent: AssociationType;
}
export interface MetaCountry extends ObjectTyped {
    Currency: RoleType;
    IsoCode: RoleType;
    Name: RoleType;
    LocalisedNames: RoleType;
    VatRates: RoleType;
    IbanLength: RoleType;
    EuMemberState: RoleType;
    TelephoneCode: RoleType;
    IbanRegex: RoleType;
    VatForm: RoleType;
    UriExtension: RoleType;
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
    LocalesWhereCountry: AssociationType;
    InternalOrganisationsWhereEuListingState: AssociationType;
    IncoTermsWhereIncoTermCountry: AssociationType;
    PostalAddressesWhereCountry: AssociationType;
    PostalBoundariesWhereCountry: AssociationType;
}
export interface MetaCurrency extends ObjectTyped {
    IsoCode: RoleType;
    Name: RoleType;
    LocalisedNames: RoleType;
    UniqueId: RoleType;
    SingletonsWherePreferredCurrency: AssociationType;
    CountriesWhereCurrency: AssociationType;
    InvoiceVersionsWhereCustomerCurrency: AssociationType;
    InvoicesWhereCustomerCurrency: AssociationType;
    OrderVersionsWhereCustomerCurrency: AssociationType;
    PartyVersionsWherePreferredCurrency: AssociationType;
    QuoteVersionsWhereCurrency: AssociationType;
    RequestVersionsWhereCurrency: AssociationType;
    OrdersWhereCustomerCurrency: AssociationType;
    PartiesWherePreferredCurrency: AssociationType;
    QuotesWhereCurrency: AssociationType;
    RequestsWhereCurrency: AssociationType;
}
export interface MetaLanguage extends ObjectTyped {
    IsoCode: RoleType;
    Name: RoleType;
    NativeName: RoleType;
    LocalisedNames: RoleType;
    LocalesWhereLanguage: AssociationType;
}
export interface MetaLocale extends ObjectTyped {
    Name: RoleType;
    Language: RoleType;
    Country: RoleType;
    SingletonsWhereDefaultLocale: AssociationType;
    SingletonWhereLocale: AssociationType;
    LocalisedsWhereLocale: AssociationType;
}
export interface MetaLocalisedText extends ObjectTyped {
    Text: RoleType;
    Locale: RoleType;
    CountryWhereLocalisedName: AssociationType;
    CurrencyWhereLocalisedName: AssociationType;
    LanguageWhereLocalisedName: AssociationType;
    CatalogueWhereLocalisedName: AssociationType;
    CatalogueWhereLocalisedDescription: AssociationType;
    ProductCategoryWhereLocalisedName: AssociationType;
    ProductCategoryWhereLocalisedDescription: AssociationType;
    EnumerationWhereLocalisedName: AssociationType;
    ProductWhereLocalisedName: AssociationType;
    ProductWhereLocalisedDescription: AssociationType;
    ProductWhereLocalisedComment: AssociationType;
}
export interface MetaAccessControl extends ObjectTyped {
}
export interface MetaLogin extends ObjectTyped {
}
export interface MetaPermission extends ObjectTyped {
}
export interface MetaRole extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaSecurityToken extends ObjectTyped {
}
export interface MetaAutomatedAgent extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    UniqueId: RoleType;
    Comment: RoleType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
    AuditablesWhereCreatedBy: AssociationType;
    AuditablesWhereLastModifiedBy: AssociationType;
    CommunicationEventVersionsWhereCreatedBy: AssociationType;
    CommunicationEventVersionsWhereLastModifiedBy: AssociationType;
    InvoiceVersionsWhereCreatedBy: AssociationType;
    InvoiceVersionsWhereLastModifiedBy: AssociationType;
    OrderVersionsWhereCreatedBy: AssociationType;
    OrderVersionsWhereLastModifiedBy: AssociationType;
    PartyVersionsWhereCreatedBy: AssociationType;
    PartyVersionsWhereLastModifiedBy: AssociationType;
    CustomerRelationshipsWhereCustomer: AssociationType;
    FaceToFaceCommunicationsWhereParticipant: AssociationType;
    FaceToFaceCommunicationVersionsWhereParticipant: AssociationType;
    FaxCommunicationsWhereOriginator: AssociationType;
    FaxCommunicationsWhereReceiver: AssociationType;
    FaxCommunicationVersionsWhereOriginator: AssociationType;
    FaxCommunicationVersionsWhereReceiver: AssociationType;
    GoodsWhereManufacturedBy: AssociationType;
    GoodsWhereSuppliedBy: AssociationType;
    InternalOrganisationWhereCurrentCustomer: AssociationType;
    InternalOrganisationWhereActiveCustomer: AssociationType;
    InternalOrganisationWhereActiveSupplier: AssociationType;
    LetterCorrespondencesWhereOriginator: AssociationType;
    LetterCorrespondencesWhereReceiver: AssociationType;
    LetterCorrespondenceVersionsWhereOriginator: AssociationType;
    LetterCorrespondenceVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereCaller: AssociationType;
    PickListVersionsWhereShipToParty: AssociationType;
    QuoteItemVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereNeededFor: AssociationType;
    RequirementVersionsWhereOriginator: AssociationType;
    RequirementVersionsWhereServicedBy: AssociationType;
    SalesInvoiceVersionsWhereBillToCustomer: AssociationType;
    SalesInvoiceVersionsWhereShipToCustomer: AssociationType;
    SalesInvoiceVersionsWhereCustomer: AssociationType;
    SalesOrderItemVersionsWhereShipToParty: AssociationType;
    SalesOrderItemVersionsWhereAssignedShipToParty: AssociationType;
    SalesOrderVersionsWhereShipToCustomer: AssociationType;
    SalesOrderVersionsWhereBillToCustomer: AssociationType;
    SalesOrderVersionsWherePlacingCustomer: AssociationType;
    OrganisationGlAccountsWhereParty: AssociationType;
    PhoneCommunicationsWhereReceiver: AssociationType;
    PhoneCommunicationsWhereCaller: AssociationType;
    PickListsWhereShipToParty: AssociationType;
    QuoteItemsWhereAuthorizer: AssociationType;
    RequirementsWhereAuthorizer: AssociationType;
    RequirementsWhereNeededFor: AssociationType;
    RequirementsWhereOriginator: AssociationType;
    RequirementsWhereServicedBy: AssociationType;
    SalesInvoicesWhereBillToCustomer: AssociationType;
    SalesInvoicesWhereShipToCustomer: AssociationType;
    SalesInvoicesWhereCustomer: AssociationType;
    SalesOrdersWhereShipToCustomer: AssociationType;
    SalesOrdersWhereBillToCustomer: AssociationType;
    SalesOrdersWherePlacingCustomer: AssociationType;
    SalesOrderItemsWhereShipToParty: AssociationType;
    SalesOrderItemsWhereAssignedShipToParty: AssociationType;
    SalesRepRelationshipsWhereCustomer: AssociationType;
    SubContractorRelationshipsWhereContractor: AssociationType;
    SubContractorRelationshipsWhereSubContractor: AssociationType;
    WebSiteCommunicationVersionsWhereOriginator: AssociationType;
    WebSiteCommunicationVersionsWhereReceiver: AssociationType;
    WebSiteCommunicationsWhereOriginator: AssociationType;
    WebSiteCommunicationsWhereReceiver: AssociationType;
    CommunicationEventVersionsWhereToParty: AssociationType;
    CommunicationEventVersionsWhereInvolvedParty: AssociationType;
    CommunicationEventVersionsWhereFromParty: AssociationType;
    CommunicationEventsWhereToParty: AssociationType;
    CommunicationEventsWhereInvolvedParty: AssociationType;
    CommunicationEventsWhereFromParty: AssociationType;
    PartyRelationshipsWhereParty: AssociationType;
    QuoteVersionsWhereReceiver: AssociationType;
    RequestVersionsWhereOriginator: AssociationType;
    QuotesWhereReceiver: AssociationType;
    RequestsWhereOriginator: AssociationType;
    ShipmentVersionsWhereBillToParty: AssociationType;
    ShipmentVersionsWhereShipToParty: AssociationType;
    ShipmentVersionsWhereShipFromParty: AssociationType;
}
export interface MetaNotification extends ObjectTyped {
    Confirmed: RoleType;
    Title: RoleType;
    Description: RoleType;
    DateCreated: RoleType;
    NotificationListWhereUnconfirmedNotification: AssociationType;
}
export interface MetaNotificationList extends ObjectTyped {
    UnconfirmedNotifications: RoleType;
    UserWhereNotificationList: AssociationType;
}
export interface MetaPerson extends ObjectTyped {
    FirstName: RoleType;
    LastName: RoleType;
    MiddleName: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PersonRoles: RoleType;
    Salutation: RoleType;
    YTDCommission: RoleType;
    PersonClassifications: RoleType;
    Citizenship: RoleType;
    LastYearsCommission: RoleType;
    GivenName: RoleType;
    Titles: RoleType;
    MothersMaidenName: RoleType;
    BirthDate: RoleType;
    Height: RoleType;
    PersonTrainings: RoleType;
    Gender: RoleType;
    Weight: RoleType;
    Hobbies: RoleType;
    TotalYearsWorkExperience: RoleType;
    Passports: RoleType;
    MaritalStatus: RoleType;
    Picture: RoleType;
    SocialSecurityNumber: RoleType;
    DeceasedDate: RoleType;
    Function: RoleType;
    UserName: RoleType;
    NormalizedUserName: RoleType;
    UserEmail: RoleType;
    UserEmailConfirmed: RoleType;
    TaskList: RoleType;
    NotificationList: RoleType;
    Locale: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    UniqueId: RoleType;
    Comment: RoleType;
    EmploymentsWhereEmployee: AssociationType;
    InternalOrganisationWhereCurrentSalesRep: AssociationType;
    InternalOrganisationWhereSalesRep: AssociationType;
    InternalOrganisationWhereActiveEmployee: AssociationType;
    PickListVersionsWherePicker: AssociationType;
    SalesInvoiceItemVersionsWhereSalesRep: AssociationType;
    SalesInvoiceVersionsWhereSalesRep: AssociationType;
    SalesOrderItemVersionsWhereSalesRep: AssociationType;
    SalesOrderVersionsWhereSalesRep: AssociationType;
    OrganisationContactRelationshipsWhereContact: AssociationType;
    PickListsWherePicker: AssociationType;
    ProfessionalServicesRelationshipsWhereProfessional: AssociationType;
    SalesInvoicesWhereSalesRep: AssociationType;
    SalesInvoiceItemsWhereSalesRep: AssociationType;
    SalesOrdersWhereSalesRep: AssociationType;
    SalesOrderItemsWhereSalesRep: AssociationType;
    SalesRepRelationshipsWhereSalesRepresentative: AssociationType;
    WorkEffortAssignmentsWhereProfessional: AssociationType;
    TasksWhereParticipant: AssociationType;
    TasksWherePerformer: AssociationType;
    CommunicationEventVersionsWhereOwner: AssociationType;
    CommunicationEventsWhereOwner: AssociationType;
    InvoicesWhereContactPerson: AssociationType;
    PartyVersionsWhereInactiveContact: AssociationType;
    PartyVersionsWhereCurrentSalesRep: AssociationType;
    PartyVersionsWhereCurrentContact: AssociationType;
    OrdersWhereContactPerson: AssociationType;
    PartiesWhereInactiveContact: AssociationType;
    PartiesWhereCurrentSalesRep: AssociationType;
    PartiesWhereCurrentContact: AssociationType;
    QuotesWhereContactPerson: AssociationType;
    RequestsWhereContactPerson: AssociationType;
    WorkEffortsWhereOwner: AssociationType;
    SingletonWhereGuest: AssociationType;
    TaskAssignmentsWhereUser: AssociationType;
    AuditablesWhereCreatedBy: AssociationType;
    AuditablesWhereLastModifiedBy: AssociationType;
    CommunicationEventVersionsWhereCreatedBy: AssociationType;
    CommunicationEventVersionsWhereLastModifiedBy: AssociationType;
    InvoiceVersionsWhereCreatedBy: AssociationType;
    InvoiceVersionsWhereLastModifiedBy: AssociationType;
    OrderVersionsWhereCreatedBy: AssociationType;
    OrderVersionsWhereLastModifiedBy: AssociationType;
    PartyVersionsWhereCreatedBy: AssociationType;
    PartyVersionsWhereLastModifiedBy: AssociationType;
    CustomerRelationshipsWhereCustomer: AssociationType;
    FaceToFaceCommunicationsWhereParticipant: AssociationType;
    FaceToFaceCommunicationVersionsWhereParticipant: AssociationType;
    FaxCommunicationsWhereOriginator: AssociationType;
    FaxCommunicationsWhereReceiver: AssociationType;
    FaxCommunicationVersionsWhereOriginator: AssociationType;
    FaxCommunicationVersionsWhereReceiver: AssociationType;
    GoodsWhereManufacturedBy: AssociationType;
    GoodsWhereSuppliedBy: AssociationType;
    InternalOrganisationWhereCurrentCustomer: AssociationType;
    InternalOrganisationWhereActiveCustomer: AssociationType;
    InternalOrganisationWhereActiveSupplier: AssociationType;
    LetterCorrespondencesWhereOriginator: AssociationType;
    LetterCorrespondencesWhereReceiver: AssociationType;
    LetterCorrespondenceVersionsWhereOriginator: AssociationType;
    LetterCorrespondenceVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereCaller: AssociationType;
    PickListVersionsWhereShipToParty: AssociationType;
    QuoteItemVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereNeededFor: AssociationType;
    RequirementVersionsWhereOriginator: AssociationType;
    RequirementVersionsWhereServicedBy: AssociationType;
    SalesInvoiceVersionsWhereBillToCustomer: AssociationType;
    SalesInvoiceVersionsWhereShipToCustomer: AssociationType;
    SalesInvoiceVersionsWhereCustomer: AssociationType;
    SalesOrderItemVersionsWhereShipToParty: AssociationType;
    SalesOrderItemVersionsWhereAssignedShipToParty: AssociationType;
    SalesOrderVersionsWhereShipToCustomer: AssociationType;
    SalesOrderVersionsWhereBillToCustomer: AssociationType;
    SalesOrderVersionsWherePlacingCustomer: AssociationType;
    OrganisationGlAccountsWhereParty: AssociationType;
    PhoneCommunicationsWhereReceiver: AssociationType;
    PhoneCommunicationsWhereCaller: AssociationType;
    PickListsWhereShipToParty: AssociationType;
    QuoteItemsWhereAuthorizer: AssociationType;
    RequirementsWhereAuthorizer: AssociationType;
    RequirementsWhereNeededFor: AssociationType;
    RequirementsWhereOriginator: AssociationType;
    RequirementsWhereServicedBy: AssociationType;
    SalesInvoicesWhereBillToCustomer: AssociationType;
    SalesInvoicesWhereShipToCustomer: AssociationType;
    SalesInvoicesWhereCustomer: AssociationType;
    SalesOrdersWhereShipToCustomer: AssociationType;
    SalesOrdersWhereBillToCustomer: AssociationType;
    SalesOrdersWherePlacingCustomer: AssociationType;
    SalesOrderItemsWhereShipToParty: AssociationType;
    SalesOrderItemsWhereAssignedShipToParty: AssociationType;
    SalesRepRelationshipsWhereCustomer: AssociationType;
    SubContractorRelationshipsWhereContractor: AssociationType;
    SubContractorRelationshipsWhereSubContractor: AssociationType;
    WebSiteCommunicationVersionsWhereOriginator: AssociationType;
    WebSiteCommunicationVersionsWhereReceiver: AssociationType;
    WebSiteCommunicationsWhereOriginator: AssociationType;
    WebSiteCommunicationsWhereReceiver: AssociationType;
    CommunicationEventVersionsWhereToParty: AssociationType;
    CommunicationEventVersionsWhereInvolvedParty: AssociationType;
    CommunicationEventVersionsWhereFromParty: AssociationType;
    CommunicationEventsWhereToParty: AssociationType;
    CommunicationEventsWhereInvolvedParty: AssociationType;
    CommunicationEventsWhereFromParty: AssociationType;
    PartyRelationshipsWhereParty: AssociationType;
    QuoteVersionsWhereReceiver: AssociationType;
    RequestVersionsWhereOriginator: AssociationType;
    QuotesWhereReceiver: AssociationType;
    RequestsWhereOriginator: AssociationType;
    ShipmentVersionsWhereBillToParty: AssociationType;
    ShipmentVersionsWhereShipToParty: AssociationType;
    ShipmentVersionsWhereShipFromParty: AssociationType;
}
export interface MetaTaskAssignment extends ObjectTyped {
    User: RoleType;
    Task: RoleType;
    TaskListWhereTaskAssignment: AssociationType;
    TaskListWhereOpenTaskAssignment: AssociationType;
}
export interface MetaTaskList extends ObjectTyped {
    TaskAssignments: RoleType;
    OpenTaskAssignments: RoleType;
    Count: RoleType;
    UserWhereTaskList: AssociationType;
}
export interface MetaUserGroup extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaAccountingPeriod extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    AccountingPeriodVersionWhereParent: AssociationType;
    InternalOrganisationWhereActualAccountingPeriod: AssociationType;
}
export interface MetaAccountingPeriodVersion extends ObjectTyped {
    Parent: RoleType;
    Active: RoleType;
    PeriodNumber: RoleType;
    TimeFrequency: RoleType;
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    Description: RoleType;
    BudgetRevisions: RoleType;
    BudgetNumber: RoleType;
    BudgetReviews: RoleType;
    BudgetItems: RoleType;
    DerivationTimeStamp: RoleType;
    AccountingPeriodWhereCurrentVersion: AssociationType;
    AccountingPeriodWhereAllVersion: AssociationType;
}
export interface MetaAccountingTransactionNumber extends ObjectTyped {
    InternalOrganisationWhereAccountingTransactionNumber: AssociationType;
}
export interface MetaAccountingTransactionType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaActivityUsage extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaAssetAssignmentStatus extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaAutomatedAgentVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    DerivationTimeStamp: RoleType;
    AutomatedAgentWhereCurrentVersion: AssociationType;
    AutomatedAgentWhereAllVersion: AssociationType;
}
export interface MetaBankAccount extends ObjectTyped {
    InternalOrganisationWhereBankAccount: AssociationType;
    PartyVersionWhereBankAccount: AssociationType;
    PartyWhereBankAccount: AssociationType;
}
export interface MetaBasePrice extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaBillingAccount extends ObjectTyped {
    InvoiceVersionsWhereBillingAccount: AssociationType;
    InvoicesWhereBillingAccount: AssociationType;
    PartyVersionWhereBillingAccount: AssociationType;
    PartyWhereBillingAccount: AssociationType;
}
export interface MetaBillOfLading extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaBrand extends ObjectTyped {
    Name: RoleType;
    ProductCategories: RoleType;
    Models: RoleType;
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaBudgetItem extends ObjectTyped {
    BudgetVersionWhereBudgetItem: AssociationType;
}
export interface MetaBudgetState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    BudgetsWhereBudgetState: AssociationType;
    BudgetVersionsWhereBudgetState: AssociationType;
}
export interface MetaBudgetReview extends ObjectTyped {
    Comment: RoleType;
    BudgetVersionWhereBudgetReview: AssociationType;
}
export interface MetaBudgetRevision extends ObjectTyped {
    BudgetVersionWhereBudgetRevision: AssociationType;
}
export interface MetaCapitalBudget extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaCapitalBudgetVersion extends ObjectTyped {
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    Description: RoleType;
    BudgetRevisions: RoleType;
    BudgetNumber: RoleType;
    BudgetReviews: RoleType;
    BudgetItems: RoleType;
    DerivationTimeStamp: RoleType;
}
export interface MetaCarrier extends ObjectTyped {
    UniqueId: RoleType;
    StoresWhereDefaultCarrier: AssociationType;
    ShipmentVersionsWhereCarrier: AssociationType;
}
export interface MetaCaseVersion extends ObjectTyped {
    CaseState: RoleType;
    DerivationTimeStamp: RoleType;
    CaseWhereCurrentVersion: AssociationType;
    CaseWhereAllVersion: AssociationType;
}
export interface MetaCase extends ObjectTyped {
    CaseState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    UniqueId: RoleType;
    CommunicationEventVersionsWhereCase: AssociationType;
    CommunicationEventsWhereCase: AssociationType;
}
export interface MetaCaseState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    CaseVersionsWhereCaseState: AssociationType;
    CasesWhereCaseState: AssociationType;
}
export interface MetaCash extends ObjectTyped {
    UniqueId: RoleType;
    CustomerShipmentVersionsWherePaymentMethod: AssociationType;
    InternalOrganisationWhereActivePaymentMethod: AssociationType;
    InternalOrganisationWhereDefaultPaymentMethod: AssociationType;
    SalesInvoiceVersionsWherePaymentMethod: AssociationType;
    SalesOrderVersionsWherePaymentMethod: AssociationType;
    SalesInvoicesWherePaymentMethod: AssociationType;
    SalesOrdersWherePaymentMethod: AssociationType;
    StoresWhereDefaultPaymentMethod: AssociationType;
    StoresWherePaymentMethod: AssociationType;
    PartyVersionsWhereDefaultPaymentMethod: AssociationType;
    PartiesWhereDefaultPaymentMethod: AssociationType;
}
export interface MetaCatalogue extends ObjectTyped {
    Name: RoleType;
    Description: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    CatalogueImage: RoleType;
    ProductCategories: RoleType;
    CatScope: RoleType;
    UniqueId: RoleType;
    StoreWhereCatalogue: AssociationType;
}
export interface MetaCatScope extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    CataloguesWhereCatScope: AssociationType;
    ProductCategoriesWhereCatScope: AssociationType;
}
export interface MetaChartOfAccounts extends ObjectTyped {
    UniqueId: RoleType;
}
export interface MetaCitizenship extends ObjectTyped {
    PersonWhereCitizenship: AssociationType;
    PersonVersionWhereCitizenship: AssociationType;
}
export interface MetaCity extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
    PostalAddressesWhereCity: AssociationType;
}
export interface MetaClientAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaColour extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaCommunicationEventState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    CommunicationEventVersionsWhereCommunicationEventState: AssociationType;
    CommunicationEventsWhereCommunicationEventState: AssociationType;
}
export interface MetaCommunicationEventPurpose extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    CommunicationEventVersionsWhereEventPurpose: AssociationType;
    CommunicationEventsWhereEventPurpose: AssociationType;
}
export interface MetaContactMechanismPurpose extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PartyContactMechanismsWhereContactPurpose: AssociationType;
}
export interface MetaContactMechanismType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    ContactMechanismsWhereContactMechanismType: AssociationType;
}
export interface MetaCostCenter extends ObjectTyped {
    Description: RoleType;
    InternalTransferGlAccount: RoleType;
    CostCenterCategories: RoleType;
    RedistributedCostsGlAccount: RoleType;
    Name: RoleType;
    Active: RoleType;
    UseGlAccountOfBooking: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereDefaultCostCenter: AssociationType;
    GeneralLedgerAccountsWhereCostCentersAllowed: AssociationType;
}
export interface MetaCostCenterCategory extends ObjectTyped {
    Parent: RoleType;
    Ancestors: RoleType;
    Children: RoleType;
    Description: RoleType;
    UniqueId: RoleType;
    CostCentersWhereCostCenterCategory: AssociationType;
    CostCenterCategoriesWhereParent: AssociationType;
    CostCenterCategoriesWhereAncestor: AssociationType;
    CostCenterCategoriesWhereChild: AssociationType;
}
export interface MetaCostCenterSplitMethod extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    InternalOrganisationsWhereCostCenterSplitMethod: AssociationType;
}
export interface MetaCostOfGoodsSoldMethod extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    InternalOrganisationsWhereCostOfGoodsSoldMethod: AssociationType;
}
export interface MetaCounty extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaCreditCard extends ObjectTyped {
    PartyVersionWhereCreditCard: AssociationType;
    PartyWhereCreditCard: AssociationType;
}
export interface MetaCustomerRelationship extends ObjectTyped {
    Customer: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaCustomerReturn extends ObjectTyped {
    CustomerReturnState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaCustomerReturnState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    CustomerReturnsWhereCustomerReturnState: AssociationType;
    CustomerReturnVersionsWhereCustomerReturnState: AssociationType;
}
export interface MetaCustomerReturnVersion extends ObjectTyped {
    CustomerReturnState: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    CustomerReturnWhereCurrentVersion: AssociationType;
    CustomerReturnWhereAllVersion: AssociationType;
}
export interface MetaCustomerShipment extends ObjectTyped {
    CustomerShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PickListVersionWhereCustomerShipmentCorrection: AssociationType;
    PickListWhereCustomerShipmentCorrection: AssociationType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaCustomerShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    CustomerShipmentsWhereCustomerShipmentState: AssociationType;
    CustomerShipmentVersionsWhereCustomerShipmentState: AssociationType;
}
export interface MetaCustomerShipmentVersion extends ObjectTyped {
    CustomerShipmentState: RoleType;
    ReleasedManually: RoleType;
    PaymentMethod: RoleType;
    WithoutCharges: RoleType;
    HeldManually: RoleType;
    ShipmentValue: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    CustomerShipmentWhereCurrentVersion: AssociationType;
    CustomerShipmentWhereAllVersion: AssociationType;
}
export interface MetaCustomOrganisationClassification extends ObjectTyped {
    Name: RoleType;
    OrganisationVersionsWhereOrganisationClassification: AssociationType;
    OrganisationsWhereOrganisationClassification: AssociationType;
    PartyVersionsWherePartyClassification: AssociationType;
    PartiesWherePartyClassification: AssociationType;
}
export interface MetaDebitCreditConstant extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereSide: AssociationType;
}
export interface MetaDeductionType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaDeliverable extends ObjectTyped {
    QuoteItemVersionsWhereDeliverable: AssociationType;
    RequestItemVersionsWhereDeliverable: AssociationType;
    QuoteItemsWhereDeliverable: AssociationType;
    RequestItemsWhereDeliverable: AssociationType;
    WorkEffortVersionsWhereDeliverablesProduced: AssociationType;
    WorkEffortWhereDeliverablesProduced: AssociationType;
}
export interface MetaDeliverableBasedService extends ObjectTyped {
    InternalComment: RoleType;
    PrimaryProductCategory: RoleType;
    SupportDiscontinuationDate: RoleType;
    SalesDiscontinuationDate: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    LocalisedComments: RoleType;
    Description: RoleType;
    ProductComplement: RoleType;
    OptionalFeatures: RoleType;
    Variants: RoleType;
    Name: RoleType;
    IntroductionDate: RoleType;
    Documents: RoleType;
    StandardFeatures: RoleType;
    UnitOfMeasure: RoleType;
    EstimatedProductCosts: RoleType;
    ProductObsolescences: RoleType;
    SelectableFeatures: RoleType;
    VatRate: RoleType;
    BasePrices: RoleType;
    ProductCategories: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereDefaultCostUnit: AssociationType;
    GeneralLedgerAccountsWhereCostUnitsAllowed: AssociationType;
    GoodsWhereProductSubstitution: AssociationType;
    GoodsWhereProductIncompatibility: AssociationType;
    QuoteItemVersionsWhereProduct: AssociationType;
    RequestItemVersionsWhereProduct: AssociationType;
    SalesInvoiceItemVersionsWhereProduct: AssociationType;
    SalesOrderItemVersionsWhereProduct: AssociationType;
    OrganisationGlAccountsWhereProduct: AssociationType;
    ProductCategoriesWhereAllProduct: AssociationType;
    QuoteItemsWhereProduct: AssociationType;
    RequestItemsWhereProduct: AssociationType;
    SalesInvoiceItemsWhereProduct: AssociationType;
    SalesOrderItemsWhereProduct: AssociationType;
    ProductsWhereProductComplement: AssociationType;
    ProductWhereVariant: AssociationType;
    ProductsWhereProductObsolescence: AssociationType;
}
export interface MetaDeliverableTurnover extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaDeliverableType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaDeployment extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaDimension extends ObjectTyped {
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaDisbursement extends ObjectTyped {
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaDiscountAdjustmentVersion extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    DerivationTimeStamp: RoleType;
    DiscountAdjustmentWhereCurrentVersion: AssociationType;
    DiscountAdjustmentWhereAllVersion: AssociationType;
}
export interface MetaDiscountAdjustment extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    InvoiceVersionsWhereDiscountAdjustment: AssociationType;
    InvoiceWhereDiscountAdjustment: AssociationType;
    OrderVersionsWhereDiscountAdjustment: AssociationType;
    OrderWhereDiscountAdjustment: AssociationType;
    PriceableWhereDiscountAdjustment: AssociationType;
}
export interface MetaDiscountComponent extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaDropShipment extends ObjectTyped {
    DropShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaDropShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    DropShipmentsWhereDropShipmentState: AssociationType;
    DropShipmentVersionsWhereDropShipmentState: AssociationType;
}
export interface MetaDropShipmentVersion extends ObjectTyped {
    DropShipmentState: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    DropShipmentWhereCurrentVersion: AssociationType;
    DropShipmentWhereAllVersion: AssociationType;
}
export interface MetaDunningType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PartyVersionsWhereDunningType: AssociationType;
    PartiesWhereDunningType: AssociationType;
}
export interface MetaEmailAddress extends ObjectTyped {
    ElectronicAddressString: RoleType;
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    EmailCommunicationsWhereOriginator: AssociationType;
    EmailCommunicationsWhereAddressee: AssociationType;
    EmailCommunicationsWhereCarbonCopy: AssociationType;
    EmailCommunicationsWhereBlindCopy: AssociationType;
    EmailCommunicationVersionsWhereOriginator: AssociationType;
    EmailCommunicationVersionsWhereAddressee: AssociationType;
    EmailCommunicationVersionsWhereCarbonCopy: AssociationType;
    EmailCommunicationVersionsWhereBlindCopy: AssociationType;
    PartiesWhereGeneralEmail: AssociationType;
    PartyWherePersonalEmailAddress: AssociationType;
    PartyVersionsWhereGeneralEmail: AssociationType;
    PartyVersionWherePersonalEmailAddress: AssociationType;
    PartyVersionsWhereInternetAddress: AssociationType;
    PartiesWhereInternetAddress: AssociationType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaEmailCommunication extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    Originator: RoleType;
    Addressees: RoleType;
    CarbonCopies: RoleType;
    BlindCopies: RoleType;
    EmailTemplate: RoleType;
    IncomingMail: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaEmailCommunicationVersion extends ObjectTyped {
    Originator: RoleType;
    Addressees: RoleType;
    CarbonCopies: RoleType;
    BlindCopies: RoleType;
    EmailTemplate: RoleType;
    IncomingMail: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    EmailCommunicationWhereCurrentVersion: AssociationType;
    EmailCommunicationWhereAllVersion: AssociationType;
}
export interface MetaEmailTemplate extends ObjectTyped {
    Description: RoleType;
    BodyTemplate: RoleType;
    SubjectTemplate: RoleType;
    EmailCommunicationsWhereEmailTemplate: AssociationType;
    EmailCommunicationVersionsWhereEmailTemplate: AssociationType;
}
export interface MetaEmployment extends ObjectTyped {
    Employee: RoleType;
    PayrollPreferences: RoleType;
    EmploymentTerminationReason: RoleType;
    EmploymentTermination: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaEmploymentAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaEmploymentApplicationSource extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaEmploymentApplicationStatus extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaEmploymentTermination extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    EmploymentsWhereEmploymentTermination: AssociationType;
}
export interface MetaEmploymentTerminationReason extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    EmploymentsWhereEmploymentTerminationReason: AssociationType;
}
export interface MetaEngagementRate extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaEngineeringBom extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaEngineeringChangeObjectState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaEngineeringDocument extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaEstimatedLaborCost extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    ProductWhereEstimatedProductCost: AssociationType;
    ProductFeatureWhereEstimatedProductCost: AssociationType;
}
export interface MetaEstimatedMaterialCost extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    ProductWhereEstimatedProductCost: AssociationType;
    ProductFeatureWhereEstimatedProductCost: AssociationType;
}
export interface MetaEstimatedOtherCost extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    ProductWhereEstimatedProductCost: AssociationType;
    ProductFeatureWhereEstimatedProductCost: AssociationType;
}
export interface MetaEuSalesListType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    VatRatesWhereEuSalesListType: AssociationType;
}
export interface MetaExpenseEntry extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaExportDocument extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaFaceToFaceCommunication extends ObjectTyped {
    Participants: RoleType;
    Location: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaFaceToFaceCommunicationVersion extends ObjectTyped {
    Participants: RoleType;
    Location: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    FaceToFaceCommunicationWhereCurrentVersion: AssociationType;
    FaceToFaceCommunicationWhereAllVersion: AssociationType;
}
export interface MetaFacility extends ObjectTyped {
    FacilityType: RoleType;
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
    InternalOrganisationsWhereDefaultFacility: AssociationType;
    PurchaseShipmentVersionsWhereFacility: AssociationType;
    RequirementVersionsWhereFacility: AssociationType;
    PurchaseShipmentsWhereFacility: AssociationType;
    RequirementsWhereFacility: AssociationType;
    StoresWhereDefaultFacility: AssociationType;
    InventoryItemVersionsWhereFacility: AssociationType;
    InventoryItemsWhereFacility: AssociationType;
    WorkEffortVersionsWhereFacility: AssociationType;
    WorkEffortsWhereFacility: AssociationType;
}
export interface MetaFaxCommunication extends ObjectTyped {
    Originator: RoleType;
    Receiver: RoleType;
    OutgoingFaxNumber: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaFaxCommunicationVersion extends ObjectTyped {
    Originator: RoleType;
    Receiver: RoleType;
    OutgoingFaxNumber: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    FaxCommunicationWhereCurrentVersion: AssociationType;
    FaxCommunicationWhereAllVersion: AssociationType;
}
export interface MetaFee extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    InvoiceVersionsWhereFee: AssociationType;
    InvoiceWhereFee: AssociationType;
    OrderVersionsWhereFee: AssociationType;
    OrderWhereFee: AssociationType;
}
export interface MetaFinancialTerm extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaFinishedGood extends ObjectTyped {
    UniqueId: RoleType;
    GoodsWhereFinishedGood: AssociationType;
    InventoryItemVersionsWherePart: AssociationType;
    InventoryItemsWherePart: AssociationType;
}
export interface MetaFacilityType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    FacilitiesWhereFacilityType: AssociationType;
}
export interface MetaGenderType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PeopleWhereGender: AssociationType;
    PersonVersionsWhereGender: AssociationType;
}
export interface MetaGeneralLedgerAccount extends ObjectTyped {
    DefaultCostUnit: RoleType;
    DefaultCostCenter: RoleType;
    Description: RoleType;
    GeneralLedgerAccountType: RoleType;
    CashAccount: RoleType;
    CostCenterAccount: RoleType;
    Side: RoleType;
    BalanceSheetAccount: RoleType;
    ReconciliationAccount: RoleType;
    Name: RoleType;
    CostCenterRequired: RoleType;
    CostUnitRequired: RoleType;
    CostCentersAllowed: RoleType;
    CostUnitAccount: RoleType;
    AccountNumber: RoleType;
    CostUnitsAllowed: RoleType;
    Protected: RoleType;
    UniqueId: RoleType;
    InternalOrganisationWhereSalesPaymentDifferencesAccount: AssociationType;
    InternalOrganisationWhereGeneralLedgerAccount: AssociationType;
    InternalOrganisationsWhereRetainedEarningsAccount: AssociationType;
    InternalOrganisationsWhereSalesPaymentDiscountDifferencesAccount: AssociationType;
    InternalOrganisationsWherePurchasePaymentDifferencesAccount: AssociationType;
    InternalOrganisationsWhereSuspenceAccount: AssociationType;
    InternalOrganisationsWhereNetIncomeAccount: AssociationType;
    InternalOrganisationsWherePurchasePaymentDiscountDifferencesAccount: AssociationType;
    InternalOrganisationsWhereCalculationDifferencesAccount: AssociationType;
    OrganisationGlAccountsWhereGeneralLedgerAccount: AssociationType;
}
export interface MetaGeneralLedgerAccountGroup extends ObjectTyped {
    Parent: RoleType;
    Description: RoleType;
    GeneralLedgerAccountGroupsWhereParent: AssociationType;
}
export interface MetaGeneralLedgerAccountType extends ObjectTyped {
    Description: RoleType;
    GeneralLedgerAccountsWhereGeneralLedgerAccountType: AssociationType;
}
export interface MetaGood extends ObjectTyped {
    QuantityOnHand: RoleType;
    AvailableToPromise: RoleType;
    InventoryItemKind: RoleType;
    BarCode: RoleType;
    FinishedGood: RoleType;
    Sku: RoleType;
    ArticleNumber: RoleType;
    ManufacturedBy: RoleType;
    ManufacturerId: RoleType;
    SuppliedBy: RoleType;
    ProductSubstitutions: RoleType;
    ProductIncompatibilities: RoleType;
    PrimaryPhoto: RoleType;
    Photos: RoleType;
    Keywords: RoleType;
    InternalComment: RoleType;
    PrimaryProductCategory: RoleType;
    SupportDiscontinuationDate: RoleType;
    SalesDiscontinuationDate: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    LocalisedComments: RoleType;
    Description: RoleType;
    ProductComplement: RoleType;
    OptionalFeatures: RoleType;
    Variants: RoleType;
    Name: RoleType;
    IntroductionDate: RoleType;
    Documents: RoleType;
    StandardFeatures: RoleType;
    UnitOfMeasure: RoleType;
    EstimatedProductCosts: RoleType;
    ProductObsolescences: RoleType;
    SelectableFeatures: RoleType;
    VatRate: RoleType;
    BasePrices: RoleType;
    ProductCategories: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    InventoryItemVersionsWhereGood: AssociationType;
    InventoryItemsWhereGood: AssociationType;
    GeneralLedgerAccountsWhereDefaultCostUnit: AssociationType;
    GeneralLedgerAccountsWhereCostUnitsAllowed: AssociationType;
    GoodsWhereProductSubstitution: AssociationType;
    GoodsWhereProductIncompatibility: AssociationType;
    QuoteItemVersionsWhereProduct: AssociationType;
    RequestItemVersionsWhereProduct: AssociationType;
    SalesInvoiceItemVersionsWhereProduct: AssociationType;
    SalesOrderItemVersionsWhereProduct: AssociationType;
    OrganisationGlAccountsWhereProduct: AssociationType;
    ProductCategoriesWhereAllProduct: AssociationType;
    QuoteItemsWhereProduct: AssociationType;
    RequestItemsWhereProduct: AssociationType;
    SalesInvoiceItemsWhereProduct: AssociationType;
    SalesOrderItemsWhereProduct: AssociationType;
    ProductsWhereProductComplement: AssociationType;
    ProductWhereVariant: AssociationType;
    ProductsWhereProductObsolescence: AssociationType;
}
export interface MetaHazardousMaterialsDocument extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaHobby extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PeopleWhereHobby: AssociationType;
    PersonVersionsWhereHobby: AssociationType;
}
export interface MetaIncentive extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaIndustryClassification extends ObjectTyped {
    Name: RoleType;
    OrganisationVersionsWhereIndustryClassification: AssociationType;
    OrganisationsWhereIndustryClassification: AssociationType;
    OrganisationVersionsWhereOrganisationClassification: AssociationType;
    OrganisationsWhereOrganisationClassification: AssociationType;
    PartyVersionsWherePartyClassification: AssociationType;
    PartiesWherePartyClassification: AssociationType;
}
export interface MetaInternalOrganisation extends ObjectTyped {
    PurchaseOrderNumberPrefix: RoleType;
    TransactionReferenceNumber: RoleType;
    JournalEntryNumbers: RoleType;
    EuListingState: RoleType;
    PurchaseInvoiceCounter: RoleType;
    ActualAccountingPeriod: RoleType;
    InvoiceSequence: RoleType;
    ActivePaymentMethods: RoleType;
    MaximumAllowedPaymentDifference: RoleType;
    LogoImage: RoleType;
    CostCenterSplitMethod: RoleType;
    PurchaseOrderCounter: RoleType;
    LegalForm: RoleType;
    SalesPaymentDifferencesAccount: RoleType;
    Name: RoleType;
    PurchaseTransactionReferenceNumber: RoleType;
    FiscalYearStartMonth: RoleType;
    CostOfGoodsSoldMethod: RoleType;
    VatDeactivated: RoleType;
    FiscalYearStartDay: RoleType;
    GeneralLedgerAccounts: RoleType;
    AccountingTransactionCounter: RoleType;
    IncomingShipmentCounter: RoleType;
    RetainedEarningsAccount: RoleType;
    PurchaseInvoiceNumberPrefix: RoleType;
    SalesPaymentDiscountDifferencesAccount: RoleType;
    SubAccountCounter: RoleType;
    AccountingTransactionNumbers: RoleType;
    TransactionReferenceNumberPrefix: RoleType;
    QuoteCounter: RoleType;
    RequestCounter: RoleType;
    PurchasePaymentDifferencesAccount: RoleType;
    SuspenceAccount: RoleType;
    NetIncomeAccount: RoleType;
    DoAccounting: RoleType;
    DefaultFacility: RoleType;
    PurchasePaymentDiscountDifferencesAccount: RoleType;
    QuoteNumberPrefix: RoleType;
    PurchaseTransactionReferenceNumberPrefix: RoleType;
    TaxNumber: RoleType;
    CalculationDifferencesAccount: RoleType;
    IncomingShipmentNumberPrefix: RoleType;
    RequestNumberPrefix: RoleType;
    CurrentSalesReps: RoleType;
    CurrentCustomers: RoleType;
    CurrentSuppliers: RoleType;
    BankAccounts: RoleType;
    DefaultPaymentMethod: RoleType;
    VatRegime: RoleType;
    SalesReps: RoleType;
    BillingAddress: RoleType;
    OrderAddress: RoleType;
    ShippingAddress: RoleType;
    ActiveCustomers: RoleType;
    ActiveEmployees: RoleType;
    ActiveSuppliers: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SingletonsWhereInternalOrganisation: AssociationType;
}
export interface MetaInternalOrganisationRevenue extends ObjectTyped {
}
export interface MetaInventoryItemKind extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    GoodsWhereInventoryItemKind: AssociationType;
}
export interface MetaInventoryItemVariance extends ObjectTyped {
    Quantity: RoleType;
    InventoryDate: RoleType;
    Reason: RoleType;
    Comment: RoleType;
    InventoryItemVersionsWhereInventoryItemVariance: AssociationType;
    InventoryItemWhereInventoryItemVariance: AssociationType;
}
export interface MetaIncoTerm extends ObjectTyped {
    incoTermCity: RoleType;
    IncoTermCountry: RoleType;
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaIncoTermType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    OrderTermsWhereTermType: AssociationType;
    AgreementTermsWhereTermType: AssociationType;
}
export interface MetaInvoiceSequence extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    InternalOrganisationsWhereInvoiceSequence: AssociationType;
}
export interface MetaInvoiceTerm extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceVersionsWhereInvoiceTerm: AssociationType;
    InvoiceWhereInvoiceTerm: AssociationType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaInvoiceVatRateItem extends ObjectTyped {
    BaseAmount: RoleType;
    VatRates: RoleType;
    VatAmount: RoleType;
    InvoiceItemVersionsWhereInvoiceVatRateItem: AssociationType;
    InvoiceItemWhereInvoiceVatRateItem: AssociationType;
}
export interface MetaItemIssuance extends ObjectTyped {
}
export interface MetaJournalEntryNumber extends ObjectTyped {
    InternalOrganisationWhereJournalEntryNumber: AssociationType;
}
export interface MetaJournalType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaLegalForm extends ObjectTyped {
    InternalOrganisationsWhereLegalForm: AssociationType;
    OrganisationVersionsWhereLegalForm: AssociationType;
    OrganisationsWhereLegalForm: AssociationType;
}
export interface MetaLegalTerm extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaLetterCorrespondence extends ObjectTyped {
    PostalAddresses: RoleType;
    Originators: RoleType;
    Receivers: RoleType;
    IncomingLetter: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaLetterCorrespondenceVersion extends ObjectTyped {
    PostalAddresses: RoleType;
    Originators: RoleType;
    Receivers: RoleType;
    IncomingLetter: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    LetterCorrespondenceWhereCurrentVersion: AssociationType;
    LetterCorrespondenceWhereAllVersion: AssociationType;
}
export interface MetaLot extends ObjectTyped {
    InventoryItemVersionsWhereLot: AssociationType;
    InventoryItemsWhereLot: AssociationType;
}
export interface MetaManifest extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaManufacturerSuggestedRetailPrice extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaManufacturingBom extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaManufacturingConfiguration extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaMaritalStatus extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PersonWhereMaritalStatus: AssociationType;
    PersonVersionWhereMaritalStatus: AssociationType;
}
export interface MetaMarketingMaterial extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaMarketingPackage extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaMaterialsUsage extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaModel extends ObjectTyped {
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    BrandWhereModel: AssociationType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaNeededSkill extends ObjectTyped {
    RequestItemVersionsWhereNeededSkill: AssociationType;
    RequestItemsWhereNeededSkill: AssociationType;
}
export interface MetaNonSerialisedInventoryItem extends ObjectTyped {
    NonSerialisedInventoryItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    QuantityCommittedOut: RoleType;
    QuantityOnHand: RoleType;
    PreviousQuantityOnHand: RoleType;
    AvailableToPromise: RoleType;
    QuantityExpectedIn: RoleType;
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    UniqueId: RoleType;
    SalesOrderItemVersionsWhereReservedFromInventoryItem: AssociationType;
    SalesOrderItemsWhereReservedFromInventoryItem: AssociationType;
    WorkEffortVersionsWhereInventoryItemsProduced: AssociationType;
    WorkEffortsWhereInventoryItemsProduced: AssociationType;
}
export interface MetaNonSerialisedInventoryItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    NonSerialisedInventoryItemsWhereNonSerialisedInventoryItemState: AssociationType;
    NonSerialisedInventoryItemVersionsWhereNonSerialisedInventoryItemState: AssociationType;
}
export interface MetaNonSerialisedInventoryItemVersion extends ObjectTyped {
    NonSerialisedInventoryItemState: RoleType;
    QuantityCommittedOut: RoleType;
    QuantityOnHand: RoleType;
    PreviousQuantityOnHand: RoleType;
    AvailableToPromise: RoleType;
    QuantityExpectedIn: RoleType;
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    DerivationTimeStamp: RoleType;
    NonSerialisedInventoryItemWhereCurrentVersion: AssociationType;
    NonSerialisedInventoryItemWhereAllVersion: AssociationType;
}
export interface MetaNonSerializedInventoryItemObjectState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaOperatingBudgetVersion extends ObjectTyped {
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    Description: RoleType;
    BudgetRevisions: RoleType;
    BudgetNumber: RoleType;
    BudgetReviews: RoleType;
    BudgetItems: RoleType;
    DerivationTimeStamp: RoleType;
    OperatingBudgetWhereCurrentVersion: AssociationType;
    OperatingBudgetWhereAllVersion: AssociationType;
}
export interface MetaOrganisationVersion extends ObjectTyped {
    OrganisationRoles: RoleType;
    LegalForm: RoleType;
    Name: RoleType;
    LogoImage: RoleType;
    TaxNumber: RoleType;
    IndustryClassifications: RoleType;
    OrganisationClassifications: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    DerivationTimeStamp: RoleType;
    OrganisationWhereCurrentVersion: AssociationType;
    OrganisationWhereAllVersion: AssociationType;
}
export interface MetaPartSpecificationVersion extends ObjectTyped {
    DerivationTimeStamp: RoleType;
    PartSpecificationWhereCurrentVersion: AssociationType;
    PartSpecificationWhereAllVersion: AssociationType;
}
export interface MetaPartSpecificationType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaOwnership extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    SerialisedInventoryItemVersionsWhereOwnership: AssociationType;
    SerialisedInventoryItemsWhereOwnership: AssociationType;
}
export interface MetaPersonVersion extends ObjectTyped {
    PersonRoles: RoleType;
    Salutation: RoleType;
    YTDCommission: RoleType;
    PersonClassifications: RoleType;
    Citizenship: RoleType;
    LastYearsCommission: RoleType;
    GivenName: RoleType;
    Titles: RoleType;
    MothersMaidenName: RoleType;
    BirthDate: RoleType;
    Height: RoleType;
    PersonTrainings: RoleType;
    Gender: RoleType;
    Weight: RoleType;
    Hobbies: RoleType;
    TotalYearsWorkExperience: RoleType;
    Passports: RoleType;
    MaritalStatus: RoleType;
    Picture: RoleType;
    SocialSecurityNumber: RoleType;
    DeceasedDate: RoleType;
    Function: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    DerivationTimeStamp: RoleType;
    PersonWhereCurrentVersion: AssociationType;
    PersonWhereAllVersion: AssociationType;
}
export interface MetaPhoneCommunicationVersion extends ObjectTyped {
    LeftVoiceMail: RoleType;
    IncomingCall: RoleType;
    Receivers: RoleType;
    Callers: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    PhoneCommunicationWhereCurrentVersion: AssociationType;
    PhoneCommunicationWhereAllVersion: AssociationType;
}
export interface MetaPickListVersion extends ObjectTyped {
    PickListState: RoleType;
    CustomerShipmentCorrection: RoleType;
    CreationDate: RoleType;
    PickListItems: RoleType;
    Picker: RoleType;
    ShipToParty: RoleType;
    Store: RoleType;
    DerivationTimeStamp: RoleType;
    PickListWhereCurrentVersion: AssociationType;
    PickListWhereAllVersion: AssociationType;
}
export interface MetaProcessFlow extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    StoresWhereProcessFlow: AssociationType;
}
export interface MetaProductQuoteVersion extends ObjectTyped {
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    DerivationTimeStamp: RoleType;
    ProductQuoteWhereCurrentVersion: AssociationType;
    ProductQuoteWhereAllVersion: AssociationType;
}
export interface MetaProposalVersion extends ObjectTyped {
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    DerivationTimeStamp: RoleType;
    ProposalWhereCurrentVersion: AssociationType;
    ProposalWhereAllVersion: AssociationType;
}
export interface MetaPurchaseInvoiceItemVersion extends ObjectTyped {
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseInvoiceItemWhereCurrentVersion: AssociationType;
    PurchaseInvoiceItemWhereAllVersion: AssociationType;
}
export interface MetaPurchaseInvoiceVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseInvoiceWhereCurrentVersion: AssociationType;
    PurchaseInvoiceWhereAllVersion: AssociationType;
}
export interface MetaPurchaseOrderItemVersion extends ObjectTyped {
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseOrderItemWhereCurrentVersion: AssociationType;
    PurchaseOrderItemWhereAllVersion: AssociationType;
}
export interface MetaPurchaseOrderPaymentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseOrdersWherePurchaseOrderPaymentState: AssociationType;
}
export interface MetaPurchaseOrderShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseOrdersWherePurchaseOrderShipmentState: AssociationType;
}
export interface MetaPurchaseOrderVersion extends ObjectTyped {
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseOrderWhereCurrentVersion: AssociationType;
    PurchaseOrderWhereAllVersion: AssociationType;
}
export interface MetaPurchaseReturnVersion extends ObjectTyped {
    PurchaseReturnState: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseReturnWhereCurrentVersion: AssociationType;
    PurchaseReturnWhereAllVersion: AssociationType;
}
export interface MetaPurchaseShipmentVersion extends ObjectTyped {
    PurchaseShipmentState: RoleType;
    Facility: RoleType;
    PurchaseOrder: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    PurchaseShipmentWhereCurrentVersion: AssociationType;
    PurchaseShipmentWhereAllVersion: AssociationType;
}
export interface MetaQuoteItemVersion extends ObjectTyped {
    QuoteItemState: RoleType;
    InternalComment: RoleType;
    Authorizer: RoleType;
    Deliverable: RoleType;
    Product: RoleType;
    EstimatedDeliveryDate: RoleType;
    RequiredByDate: RoleType;
    UnitOfMeasure: RoleType;
    ProductFeature: RoleType;
    UnitPrice: RoleType;
    Skill: RoleType;
    WorkEffort: RoleType;
    QuoteTerms: RoleType;
    Quantity: RoleType;
    RequestItem: RoleType;
    DerivationTimeStamp: RoleType;
    QuoteItemWhereCurrentVersion: AssociationType;
    QuoteItemWhereAllVersion: AssociationType;
}
export interface MetaRequestForInformationVersion extends ObjectTyped {
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    DerivationTimeStamp: RoleType;
    RequestForInformationWhereCurrentVersion: AssociationType;
    RequestForInformationWhereAllVersion: AssociationType;
}
export interface MetaRequestForProposalVersion extends ObjectTyped {
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    DerivationTimeStamp: RoleType;
    RequestForProposalWhereCurrentVersion: AssociationType;
    RequestForProposalWhereAllVersion: AssociationType;
}
export interface MetaRequestForQuoteVersion extends ObjectTyped {
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    DerivationTimeStamp: RoleType;
    RequestForQuoteWhereCurrentVersion: AssociationType;
    RequestForQuoteWhereAllVersion: AssociationType;
}
export interface MetaRequestItemVersion extends ObjectTyped {
    RequestItemState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    Quantity: RoleType;
    UnitOfMeasure: RoleType;
    Requirements: RoleType;
    Deliverable: RoleType;
    ProductFeature: RoleType;
    NeededSkill: RoleType;
    Product: RoleType;
    MaximumAllowedPrice: RoleType;
    RequiredByDate: RoleType;
    DerivationTimeStamp: RoleType;
    RequestItemWhereCurrentVersion: AssociationType;
    RequestItemWhereAllVersion: AssociationType;
}
export interface MetaRequirementVersion extends ObjectTyped {
    RequirementState: RoleType;
    RequiredByDate: RoleType;
    RequirementType: RoleType;
    Authorizer: RoleType;
    Reason: RoleType;
    Children: RoleType;
    NeededFor: RoleType;
    Originator: RoleType;
    Facility: RoleType;
    ServicedBy: RoleType;
    EstimatedBudget: RoleType;
    Description: RoleType;
    Quantity: RoleType;
    DerivationTimeStamp: RoleType;
    RequirementWhereCurrentVersion: AssociationType;
    RequirementWhereAllVersion: AssociationType;
}
export interface MetaSalesInvoiceItemVersion extends ObjectTyped {
    SalesInvoiceItemState: RoleType;
    ProductFeature: RoleType;
    RequiredProfitMargin: RoleType;
    InitialMarkupPercentage: RoleType;
    MaintainedMarkupPercentage: RoleType;
    Product: RoleType;
    UnitPurchasePrice: RoleType;
    SalesOrderItem: RoleType;
    SalesInvoiceItemType: RoleType;
    SalesRep: RoleType;
    InitialProfitMargin: RoleType;
    MaintainedProfitMargin: RoleType;
    RequiredMarkupPercentage: RoleType;
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    DerivationTimeStamp: RoleType;
    SalesInvoiceItemWhereCurrentVersion: AssociationType;
    SalesInvoiceItemWhereAllVersion: AssociationType;
}
export interface MetaSalesInvoiceVersion extends ObjectTyped {
    SalesInvoiceState: RoleType;
    TotalListPrice: RoleType;
    BillToContactMechanism: RoleType;
    SalesInvoiceType: RoleType;
    InitialProfitMargin: RoleType;
    PaymentMethod: RoleType;
    SalesOrder: RoleType;
    InitialMarkupPercentage: RoleType;
    MaintainedMarkupPercentage: RoleType;
    SalesReps: RoleType;
    Shipment: RoleType;
    MaintainedProfitMargin: RoleType;
    BillToCustomer: RoleType;
    SalesInvoiceItems: RoleType;
    TotalListPriceCustomerCurrency: RoleType;
    ShipToCustomer: RoleType;
    BilledFromContactMechanism: RoleType;
    TotalPurchasePrice: RoleType;
    SalesChannel: RoleType;
    Customers: RoleType;
    ShipToAddress: RoleType;
    Store: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    DerivationTimeStamp: RoleType;
    SalesInvoiceWhereCurrentVersion: AssociationType;
    SalesInvoiceWhereAllVersion: AssociationType;
}
export interface MetaSalesOrderItemShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrderItemsWhereSalesOrderItemShipmentState: AssociationType;
}
export interface MetaSalesOrderItemPaymentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrderItemsWhereSalesOrderItemPaymentState: AssociationType;
}
export interface MetaSalesOrderItemVersion extends ObjectTyped {
    SalesOrderItemState: RoleType;
    InitialProfitMargin: RoleType;
    QuantityShortFalled: RoleType;
    OrderedWithFeatures: RoleType;
    MaintainedProfitMargin: RoleType;
    RequiredProfitMargin: RoleType;
    QuantityShipNow: RoleType;
    RequiredMarkupPercentage: RoleType;
    QuantityShipped: RoleType;
    ShipToAddress: RoleType;
    QuantityPicked: RoleType;
    UnitPurchasePrice: RoleType;
    ShipToParty: RoleType;
    AssignedShipToAddress: RoleType;
    QuantityReturned: RoleType;
    QuantityReserved: RoleType;
    SalesRep: RoleType;
    AssignedShipToParty: RoleType;
    QuantityPendingShipment: RoleType;
    MaintainedMarkupPercentage: RoleType;
    InitialMarkupPercentage: RoleType;
    ReservedFromInventoryItem: RoleType;
    Product: RoleType;
    ProductFeature: RoleType;
    QuantityRequestsShipping: RoleType;
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    DerivationTimeStamp: RoleType;
    SalesOrderItemWhereCurrentVersion: AssociationType;
    SalesOrderItemWhereAllVersion: AssociationType;
}
export interface MetaSalesOrderPaymentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrdersWhereSalesOrderPaymentState: AssociationType;
}
export interface MetaSalesOrderShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrdersWhereSalesOrderShipmentState: AssociationType;
}
export interface MetaSalesOrderVersion extends ObjectTyped {
    SalesOrderState: RoleType;
    TakenByContactMechanism: RoleType;
    ShipToCustomer: RoleType;
    BillToCustomer: RoleType;
    TotalPurchasePrice: RoleType;
    ShipmentMethod: RoleType;
    TotalListPriceCustomerCurrency: RoleType;
    MaintainedProfitMargin: RoleType;
    ShipToAddress: RoleType;
    BillToContactMechanism: RoleType;
    SalesReps: RoleType;
    InitialProfitMargin: RoleType;
    TotalListPrice: RoleType;
    MaintainedMarkupPercentage: RoleType;
    BillFromContactMechanism: RoleType;
    PaymentMethod: RoleType;
    PlacingContactMechanism: RoleType;
    SalesChannel: RoleType;
    PlacingCustomer: RoleType;
    SalesOrderItems: RoleType;
    InitialMarkupPercentage: RoleType;
    Quote: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    DerivationTimeStamp: RoleType;
    CapitalBudgetWhereCurrentVersion: AssociationType;
    CapitalBudgetWhereAllVersion: AssociationType;
    SalesOrderWhereCurrentVersion: AssociationType;
    SalesOrderWhereAllVersion: AssociationType;
}
export interface MetaSerialisedInventoryItemVersion extends ObjectTyped {
    SerialisedInventoryItemState: RoleType;
    SerialNumber: RoleType;
    Ownership: RoleType;
    Owner: RoleType;
    AcquisitionYear: RoleType;
    ManufacturingYear: RoleType;
    ReplacementValue: RoleType;
    LifeTime: RoleType;
    DepreciationYears: RoleType;
    PurchasePrice: RoleType;
    ExpectedSalesPrice: RoleType;
    RefurbishCost: RoleType;
    TransportCost: RoleType;
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    DerivationTimeStamp: RoleType;
    SerialisedInventoryItemWhereCurrentVersion: AssociationType;
    SerialisedInventoryItemWhereAllVersion: AssociationType;
}
export interface MetaOneTimeCharge extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaOperatingBudget extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    BudgetState: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaOrderKind extends ObjectTyped {
    UniqueId: RoleType;
    OrderVersionsWhereOrderKind: AssociationType;
    OrdersWhereOrderKind: AssociationType;
}
export interface MetaOrderShipment extends ObjectTyped {
}
export interface MetaOrderTerm extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    OrderItemVersionsWhereOrderTerm: AssociationType;
    OrderVersionsWhereOrderTerm: AssociationType;
    OrderWhereOrderTerm: AssociationType;
    OrderItemWhereOrderTerm: AssociationType;
}
export interface MetaOrdinal extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaOrganisation extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    OrganisationRoles: RoleType;
    LegalForm: RoleType;
    Name: RoleType;
    LogoImage: RoleType;
    TaxNumber: RoleType;
    IndustryClassifications: RoleType;
    OrganisationClassifications: RoleType;
    PartyName: RoleType;
    GeneralCorrespondence: RoleType;
    YTDRevenue: RoleType;
    LastYearsRevenue: RoleType;
    BillingInquiriesFax: RoleType;
    Qualifications: RoleType;
    HomeAddress: RoleType;
    InactiveOrganisationContactRelationships: RoleType;
    SalesOffice: RoleType;
    InactiveContacts: RoleType;
    InactivePartyContactMechanisms: RoleType;
    OrderInquiriesFax: RoleType;
    CurrentSalesReps: RoleType;
    PartyContactMechanisms: RoleType;
    ShippingInquiriesFax: RoleType;
    ShippingInquiriesPhone: RoleType;
    BillingAccounts: RoleType;
    OrderInquiriesPhone: RoleType;
    PartySkills: RoleType;
    PartyClassifications: RoleType;
    ExcludeFromDunning: RoleType;
    BankAccounts: RoleType;
    CurrentContacts: RoleType;
    BillingAddress: RoleType;
    GeneralEmail: RoleType;
    DefaultShipmentMethod: RoleType;
    Resumes: RoleType;
    HeadQuarter: RoleType;
    PersonalEmailAddress: RoleType;
    CellPhoneNumber: RoleType;
    BillingInquiriesPhone: RoleType;
    OrderAddress: RoleType;
    InternetAddress: RoleType;
    Contents: RoleType;
    CreditCards: RoleType;
    ShippingAddress: RoleType;
    CurrentOrganisationContactRelationships: RoleType;
    OpenOrderAmount: RoleType;
    GeneralFaxNumber: RoleType;
    DefaultPaymentMethod: RoleType;
    CurrentPartyContactMechanisms: RoleType;
    GeneralPhoneNumber: RoleType;
    PreferredCurrency: RoleType;
    VatRegime: RoleType;
    AmountOverDue: RoleType;
    DunningType: RoleType;
    AmountDue: RoleType;
    LastReminderDate: RoleType;
    CreditLimit: RoleType;
    SubAccountNumber: RoleType;
    BlockedForDunning: RoleType;
    Locale: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    UniqueId: RoleType;
    Comment: RoleType;
    InternalOrganisationWhereCurrentSupplier: AssociationType;
    OrganisationContactRelationshipsWhereOrganisation: AssociationType;
    OrganisationRollUpsWhereParent: AssociationType;
    OrganisationRollUpsWhereChild: AssociationType;
    ProfessionalServicesRelationshipsWhereProfessionalServicesProvider: AssociationType;
    SupplierRelationshipsWhereSupplier: AssociationType;
    VatRatesWhereTaxAuthority: AssociationType;
    CustomerRelationshipsWhereCustomer: AssociationType;
    FaceToFaceCommunicationsWhereParticipant: AssociationType;
    FaceToFaceCommunicationVersionsWhereParticipant: AssociationType;
    FaxCommunicationsWhereOriginator: AssociationType;
    FaxCommunicationsWhereReceiver: AssociationType;
    FaxCommunicationVersionsWhereOriginator: AssociationType;
    FaxCommunicationVersionsWhereReceiver: AssociationType;
    GoodsWhereManufacturedBy: AssociationType;
    GoodsWhereSuppliedBy: AssociationType;
    InternalOrganisationWhereCurrentCustomer: AssociationType;
    InternalOrganisationWhereActiveCustomer: AssociationType;
    InternalOrganisationWhereActiveSupplier: AssociationType;
    LetterCorrespondencesWhereOriginator: AssociationType;
    LetterCorrespondencesWhereReceiver: AssociationType;
    LetterCorrespondenceVersionsWhereOriginator: AssociationType;
    LetterCorrespondenceVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereReceiver: AssociationType;
    PhoneCommunicationVersionsWhereCaller: AssociationType;
    PickListVersionsWhereShipToParty: AssociationType;
    QuoteItemVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereAuthorizer: AssociationType;
    RequirementVersionsWhereNeededFor: AssociationType;
    RequirementVersionsWhereOriginator: AssociationType;
    RequirementVersionsWhereServicedBy: AssociationType;
    SalesInvoiceVersionsWhereBillToCustomer: AssociationType;
    SalesInvoiceVersionsWhereShipToCustomer: AssociationType;
    SalesInvoiceVersionsWhereCustomer: AssociationType;
    SalesOrderItemVersionsWhereShipToParty: AssociationType;
    SalesOrderItemVersionsWhereAssignedShipToParty: AssociationType;
    SalesOrderVersionsWhereShipToCustomer: AssociationType;
    SalesOrderVersionsWhereBillToCustomer: AssociationType;
    SalesOrderVersionsWherePlacingCustomer: AssociationType;
    OrganisationGlAccountsWhereParty: AssociationType;
    PhoneCommunicationsWhereReceiver: AssociationType;
    PhoneCommunicationsWhereCaller: AssociationType;
    PickListsWhereShipToParty: AssociationType;
    QuoteItemsWhereAuthorizer: AssociationType;
    RequirementsWhereAuthorizer: AssociationType;
    RequirementsWhereNeededFor: AssociationType;
    RequirementsWhereOriginator: AssociationType;
    RequirementsWhereServicedBy: AssociationType;
    SalesInvoicesWhereBillToCustomer: AssociationType;
    SalesInvoicesWhereShipToCustomer: AssociationType;
    SalesInvoicesWhereCustomer: AssociationType;
    SalesOrdersWhereShipToCustomer: AssociationType;
    SalesOrdersWhereBillToCustomer: AssociationType;
    SalesOrdersWherePlacingCustomer: AssociationType;
    SalesOrderItemsWhereShipToParty: AssociationType;
    SalesOrderItemsWhereAssignedShipToParty: AssociationType;
    SalesRepRelationshipsWhereCustomer: AssociationType;
    SubContractorRelationshipsWhereContractor: AssociationType;
    SubContractorRelationshipsWhereSubContractor: AssociationType;
    WebSiteCommunicationVersionsWhereOriginator: AssociationType;
    WebSiteCommunicationVersionsWhereReceiver: AssociationType;
    WebSiteCommunicationsWhereOriginator: AssociationType;
    WebSiteCommunicationsWhereReceiver: AssociationType;
    CommunicationEventVersionsWhereToParty: AssociationType;
    CommunicationEventVersionsWhereInvolvedParty: AssociationType;
    CommunicationEventVersionsWhereFromParty: AssociationType;
    CommunicationEventsWhereToParty: AssociationType;
    CommunicationEventsWhereInvolvedParty: AssociationType;
    CommunicationEventsWhereFromParty: AssociationType;
    PartyRelationshipsWhereParty: AssociationType;
    QuoteVersionsWhereReceiver: AssociationType;
    RequestVersionsWhereOriginator: AssociationType;
    QuotesWhereReceiver: AssociationType;
    RequestsWhereOriginator: AssociationType;
    ShipmentVersionsWhereBillToParty: AssociationType;
    ShipmentVersionsWhereShipToParty: AssociationType;
    ShipmentVersionsWhereShipFromParty: AssociationType;
}
export interface MetaOrganisationContactKind extends ObjectTyped {
    Description: RoleType;
    UniqueId: RoleType;
    OrganisationContactRelationshipsWhereContactKind: AssociationType;
}
export interface MetaOrganisationContactRelationship extends ObjectTyped {
    Contact: RoleType;
    Organisation: RoleType;
    ContactKinds: RoleType;
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    PartyVersionsWhereInactiveOrganisationContactRelationship: AssociationType;
    PartyVersionsWhereCurrentOrganisationContactRelationship: AssociationType;
    PartiesWhereInactiveOrganisationContactRelationship: AssociationType;
    PartiesWhereCurrentOrganisationContactRelationship: AssociationType;
}
export interface MetaOrganisationGlAccount extends ObjectTyped {
    Product: RoleType;
    Parent: RoleType;
    Party: RoleType;
    HasBankStatementTransactions: RoleType;
    ProductCategory: RoleType;
    GeneralLedgerAccount: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    CostCentersWhereInternalTransferGlAccount: AssociationType;
    CostCentersWhereRedistributedCostsGlAccount: AssociationType;
    OrganisationGlAccountsWhereParent: AssociationType;
    VatRateWhereVatPayableAccount: AssociationType;
    VatRatesWhereVatToPayAccount: AssociationType;
    VatRatesWhereVatToReceiveAccount: AssociationType;
    VatRateWhereVatReceivableAccount: AssociationType;
    VatRegimesWhereGeneralLedgerAccount: AssociationType;
}
export interface MetaOrganisationRole extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    OrganisationVersionsWhereOrganisationRole: AssociationType;
    OrganisationsWhereOrganisationRole: AssociationType;
}
export interface MetaOrganisationRollUp extends ObjectTyped {
    Parent: RoleType;
    RollupKind: RoleType;
    Child: RoleType;
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaOrganisationUnit extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    OrganisationRollUpsWhereRollupKind: AssociationType;
}
export interface MetaOwnBankAccount extends ObjectTyped {
    UniqueId: RoleType;
    CustomerShipmentVersionsWherePaymentMethod: AssociationType;
    InternalOrganisationWhereActivePaymentMethod: AssociationType;
    InternalOrganisationWhereDefaultPaymentMethod: AssociationType;
    SalesInvoiceVersionsWherePaymentMethod: AssociationType;
    SalesOrderVersionsWherePaymentMethod: AssociationType;
    SalesInvoicesWherePaymentMethod: AssociationType;
    SalesOrdersWherePaymentMethod: AssociationType;
    StoresWhereDefaultPaymentMethod: AssociationType;
    StoresWherePaymentMethod: AssociationType;
    PartyVersionsWhereDefaultPaymentMethod: AssociationType;
    PartiesWhereDefaultPaymentMethod: AssociationType;
}
export interface MetaOwnCreditCard extends ObjectTyped {
    UniqueId: RoleType;
    CustomerShipmentVersionsWherePaymentMethod: AssociationType;
    InternalOrganisationWhereActivePaymentMethod: AssociationType;
    InternalOrganisationWhereDefaultPaymentMethod: AssociationType;
    SalesInvoiceVersionsWherePaymentMethod: AssociationType;
    SalesOrderVersionsWherePaymentMethod: AssociationType;
    SalesInvoicesWherePaymentMethod: AssociationType;
    SalesOrdersWherePaymentMethod: AssociationType;
    StoresWhereDefaultPaymentMethod: AssociationType;
    StoresWherePaymentMethod: AssociationType;
    PartyVersionsWhereDefaultPaymentMethod: AssociationType;
    PartiesWhereDefaultPaymentMethod: AssociationType;
}
export interface MetaPackage extends ObjectTyped {
    UniqueId: RoleType;
    ProductCategoriesWherePackage: AssociationType;
}
export interface MetaPackageRevenue extends ObjectTyped {
}
export interface MetaPackagingSlip extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaPartBillOfMaterialSubstitute extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
}
export interface MetaPartRevision extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPartSpecification extends ObjectTyped {
    PartSpecificationState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    UniqueId: RoleType;
    Comment: RoleType;
}
export interface MetaPartSpecificationState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PartSpecificationsWherePartSpecificationState: AssociationType;
}
export interface MetaPartSubstitute extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPartyContactMechanism extends ObjectTyped {
    ContactPurposes: RoleType;
    ContactMechanism: RoleType;
    UseAsDefault: RoleType;
    NonSolicitationIndicator: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    PartyVersionWhereInactivePartyContactMechanism: AssociationType;
    PartyVersionWherePartyContactMechanism: AssociationType;
    PartyVersionWhereCurrentPartyContactMechanism: AssociationType;
    PartyWhereInactivePartyContactMechanism: AssociationType;
    PartyWherePartyContactMechanism: AssociationType;
    PartyWhereCurrentPartyContactMechanism: AssociationType;
}
export interface MetaPartyFixedAssetAssignment extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
}
export interface MetaPartyPackageRevenue extends ObjectTyped {
}
export interface MetaPartyProductCategoryRevenue extends ObjectTyped {
}
export interface MetaPartyProductRevenue extends ObjectTyped {
}
export interface MetaPartyRevenue extends ObjectTyped {
}
export interface MetaPartySkill extends ObjectTyped {
    PartyVersionsWherePartySkill: AssociationType;
    PartiesWherePartySkill: AssociationType;
}
export interface MetaPassport extends ObjectTyped {
    PersonWherePassport: AssociationType;
    PersonVersionWherePassport: AssociationType;
}
export interface MetaPayCheck extends ObjectTyped {
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaPayGrade extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPayHistory extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPayrollPreference extends ObjectTyped {
    EmploymentWherePayrollPreference: AssociationType;
}
export interface MetaPerformanceNote extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPerformanceReview extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPerformanceReviewItem extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPerformanceReviewItemType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaPersonalTitle extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PeopleWhereTitle: AssociationType;
    PersonVersionsWhereTitle: AssociationType;
}
export interface MetaPersonRole extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PeopleWherePersonRole: AssociationType;
    PersonVersionsWherePersonRole: AssociationType;
}
export interface MetaPersonTraining extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    PeopleWherePersonTraining: AssociationType;
    PersonVersionsWherePersonTraining: AssociationType;
}
export interface MetaPhoneCommunication extends ObjectTyped {
    LeftVoiceMail: RoleType;
    IncomingCall: RoleType;
    Receivers: RoleType;
    Callers: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaPickList extends ObjectTyped {
    PickListState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CustomerShipmentCorrection: RoleType;
    CreationDate: RoleType;
    PickListItems: RoleType;
    Picker: RoleType;
    ShipToParty: RoleType;
    Store: RoleType;
    PrintContent: RoleType;
}
export interface MetaPickListItem extends ObjectTyped {
    PickListVersionWherePickListItem: AssociationType;
    PickListWherePickListItem: AssociationType;
}
export interface MetaPickListState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PickListVersionsWherePickListState: AssociationType;
    PickListsWherePickListState: AssociationType;
}
export interface MetaPositionFulfillment extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPositionReportingStructure extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPositionResponsibility extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaPositionStatus extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaPostalAddress extends ObjectTyped {
    Address1: RoleType;
    Address2: RoleType;
    Address3: RoleType;
    PostalBoundary: RoleType;
    PostalCode: RoleType;
    City: RoleType;
    Country: RoleType;
    Directions: RoleType;
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
    InternalOrganisationsWhereShippingAddress: AssociationType;
    LetterCorrespondencesWherePostalAddress: AssociationType;
    LetterCorrespondenceVersionsWherePostalAddress: AssociationType;
    SalesInvoiceVersionsWhereShipToAddress: AssociationType;
    SalesOrderItemVersionsWhereShipToAddress: AssociationType;
    SalesOrderItemVersionsWhereAssignedShipToAddress: AssociationType;
    SalesOrderVersionsWhereShipToAddress: AssociationType;
    SalesInvoicesWhereShipToAddress: AssociationType;
    SalesOrdersWhereShipToAddress: AssociationType;
    SalesOrderItemsWhereShipToAddress: AssociationType;
    SalesOrderItemsWhereAssignedShipToAddress: AssociationType;
    PartyVersionsWhereGeneralCorrespondence: AssociationType;
    PartyVersionsWhereShippingAddress: AssociationType;
    PartiesWhereGeneralCorrespondence: AssociationType;
    PartiesWhereShippingAddress: AssociationType;
    ShipmentVersionsWhereShipToAddress: AssociationType;
    ShipmentVersionsWhereShipFromAddress: AssociationType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaPostalBoundary extends ObjectTyped {
    PostalCode: RoleType;
    Locality: RoleType;
    Country: RoleType;
    Region: RoleType;
    PostalAddressWherePostalBoundary: AssociationType;
}
export interface MetaPostalCode extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
    PostalAddressesWherePostalCode: AssociationType;
}
export interface MetaPriority extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    CommunicationEventVersionsWherePriority: AssociationType;
    CommunicationEventsWherePriority: AssociationType;
    WorkEffortVersionsWherePriority: AssociationType;
    WorkEffortsWherePriority: AssociationType;
}
export interface MetaProductCategory extends ObjectTyped {
    Package: RoleType;
    Code: RoleType;
    Parents: RoleType;
    Children: RoleType;
    Description: RoleType;
    Name: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    CategoryImage: RoleType;
    SuperJacent: RoleType;
    CatScope: RoleType;
    AllProducts: RoleType;
    UniqueId: RoleType;
    BrandsWhereProductCategory: AssociationType;
    CatalogueWhereProductCategory: AssociationType;
    OrganisationGlAccountsWhereProductCategory: AssociationType;
    ProductCategoriesWhereParent: AssociationType;
    ProductCategoriesWhereChild: AssociationType;
    ProductCategoriesWhereSuperJacent: AssociationType;
    SalesRepRelationshipsWhereProductCategory: AssociationType;
    ProductsWherePrimaryProductCategory: AssociationType;
    ProductsWhereProductCategory: AssociationType;
}
export interface MetaProductCategoryRevenue extends ObjectTyped {
}
export interface MetaProductCharacteristic extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    ProductCharacteristicValuesWhereProductCharacteristic: AssociationType;
    ProductTypesWhereProductCharacteristic: AssociationType;
}
export interface MetaProductCharacteristicValue extends ObjectTyped {
    ProductCharacteristic: RoleType;
    Value: RoleType;
    Locale: RoleType;
    InventoryItemVersionsWhereProductCharacteristicValue: AssociationType;
    InventoryItemsWhereProductCharacteristicValue: AssociationType;
}
export interface MetaProductConfiguration extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaProductDrawing extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaProductModel extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaProductPurchasePrice extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaProductQuality extends ObjectTyped {
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaProductQuote extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    Comment: RoleType;
    SalesOrderVersionsWhereQuote: AssociationType;
    SalesOrderWhereQuote: AssociationType;
}
export interface MetaProductRevenue extends ObjectTyped {
}
export interface MetaProductType extends ObjectTyped {
    ProductCharacteristics: RoleType;
    Name: RoleType;
    UniqueId: RoleType;
    InventoryItemVersionsWhereProductType: AssociationType;
    InventoryItemsWhereProductType: AssociationType;
}
export interface MetaProfessionalAssignment extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaProfessionalServicesRelationship extends ObjectTyped {
    Professional: RoleType;
    ProfessionalServicesProvider: RoleType;
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaProposal extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    Comment: RoleType;
}
export interface MetaProvince extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaPurchaseAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaPurchaseInvoice extends ObjectTyped {
    PurchaseInvoiceState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    ContactPerson: RoleType;
    Locale: RoleType;
    Comment: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaPurchaseInvoiceItem extends ObjectTyped {
    PurchaseInvoiceItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    InvoiceItemVersionsWhereAdjustmentFor: AssociationType;
    InvoiceItemsWhereAdjustmentFor: AssociationType;
}
export interface MetaPurchaseInvoiceItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseInvoiceItemsWherePurchaseInvoiceItemState: AssociationType;
}
export interface MetaPurchaseInvoiceItemType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaPurchaseInvoiceState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseInvoicesWherePurchaseInvoiceState: AssociationType;
}
export interface MetaPurchaseInvoiceType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaPurchaseOrder extends ObjectTyped {
    PurchaseOrderState: RoleType;
    PurchaseOrderPaymentState: RoleType;
    PurchaseOrderShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    Description: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    Locale: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PurchaseShipmentVersionsWherePurchaseOrder: AssociationType;
    PurchaseShipmentsWherePurchaseOrder: AssociationType;
    OrderItemVersionsWhereCorrespondingPurchaseOrder: AssociationType;
    OrderItemsWhereCorrespondingPurchaseOrder: AssociationType;
}
export interface MetaPurchaseOrderItem extends ObjectTyped {
    PurchaseOrderItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    SalesOrderItemVersionsWhereOrderedWithFeature: AssociationType;
    SalesOrderItemWhereOrderedWithFeature: AssociationType;
    OrderItemVersionsWhereAssociation: AssociationType;
    OrderVersionsWhereValidOrderItem: AssociationType;
    OrderWhereValidOrderItem: AssociationType;
    OrderItemsWhereAssociation: AssociationType;
}
export interface MetaPurchaseOrderItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseOrderItemsWherePurchaseOrderItemState: AssociationType;
}
export interface MetaPurchaseOrderState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseOrdersWherePurchaseOrderState: AssociationType;
}
export interface MetaPurchaseReturn extends ObjectTyped {
    PurchaseReturnState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaPurchaseReturnState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseReturnVersionsWherePurchaseReturnState: AssociationType;
    PurchaseReturnsWherePurchaseReturnState: AssociationType;
}
export interface MetaPurchaseShipment extends ObjectTyped {
    PurchaseShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    Facility: RoleType;
    PurchaseOrder: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaPurchaseShipmentState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    PurchaseShipmentVersionsWherePurchaseShipmentState: AssociationType;
    PurchaseShipmentsWherePurchaseShipmentState: AssociationType;
}
export interface MetaQualification extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PartyVersionsWhereQualification: AssociationType;
    PartiesWhereQualification: AssociationType;
}
export interface MetaQuoteItem extends ObjectTyped {
    QuoteItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    Authorizer: RoleType;
    Deliverable: RoleType;
    Product: RoleType;
    EstimatedDeliveryDate: RoleType;
    RequiredByDate: RoleType;
    UnitOfMeasure: RoleType;
    ProductFeature: RoleType;
    UnitPrice: RoleType;
    Skill: RoleType;
    WorkEffort: RoleType;
    QuoteTerms: RoleType;
    Quantity: RoleType;
    RequestItem: RoleType;
    Comment: RoleType;
    OrderItemVersionsWhereQuoteItem: AssociationType;
    QuoteVersionsWhereQuoteItem: AssociationType;
    OrderItemsWhereQuoteItem: AssociationType;
    QuoteWhereQuoteItem: AssociationType;
}
export interface MetaQuoteTerm extends ObjectTyped {
    QuoteItemVersionWhereQuoteTerm: AssociationType;
    QuoteItemWhereQuoteTerm: AssociationType;
    QuoteVersionsWhereQuoteTerm: AssociationType;
    QuotesWhereQuoteTerm: AssociationType;
}
export interface MetaRateType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaRatingType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaRawMaterial extends ObjectTyped {
    UniqueId: RoleType;
    InventoryItemVersionsWherePart: AssociationType;
    InventoryItemsWherePart: AssociationType;
}
export interface MetaReceipt extends ObjectTyped {
    Comment: RoleType;
    UniqueId: RoleType;
}
export interface MetaRecurringCharge extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaRegion extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaRequestForInformation extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    ContactPerson: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PrintContent: RoleType;
    QuoteVersionsWhereRequest: AssociationType;
    QuoteWhereRequest: AssociationType;
}
export interface MetaRequestForProposal extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    ContactPerson: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PrintContent: RoleType;
    QuoteVersionsWhereRequest: AssociationType;
    QuoteWhereRequest: AssociationType;
}
export interface MetaRequestForQuote extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    RequestState: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    RequestDate: RoleType;
    RequiredResponseDate: RoleType;
    RequestItems: RoleType;
    RequestNumber: RoleType;
    RespondingParties: RoleType;
    Originator: RoleType;
    Currency: RoleType;
    FullfillContactMechanism: RoleType;
    EmailAddress: RoleType;
    TelephoneNumber: RoleType;
    TelephoneCountryCode: RoleType;
    ContactPerson: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PrintContent: RoleType;
    QuoteVersionsWhereRequest: AssociationType;
    QuoteWhereRequest: AssociationType;
}
export interface MetaRequestItem extends ObjectTyped {
    RequestItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InternalComment: RoleType;
    Description: RoleType;
    Quantity: RoleType;
    UnitOfMeasure: RoleType;
    Requirements: RoleType;
    Deliverable: RoleType;
    ProductFeature: RoleType;
    NeededSkill: RoleType;
    Product: RoleType;
    MaximumAllowedPrice: RoleType;
    RequiredByDate: RoleType;
    Comment: RoleType;
    QuoteItemVersionsWhereRequestItem: AssociationType;
    QuoteItemsWhereRequestItem: AssociationType;
    RequestVersionWhereRequestItem: AssociationType;
    RequestWhereRequestItem: AssociationType;
}
export interface MetaQuoteState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    QuoteVersionsWhereQuoteState: AssociationType;
    QuotesWhereQuoteState: AssociationType;
}
export interface MetaQuoteItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    QuoteItemVersionsWhereQuoteItemState: AssociationType;
    QuoteItemsWhereQuoteItemState: AssociationType;
}
export interface MetaRequirement extends ObjectTyped {
    RequirementState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    RequiredByDate: RoleType;
    RequirementType: RoleType;
    Authorizer: RoleType;
    Reason: RoleType;
    Children: RoleType;
    NeededFor: RoleType;
    Originator: RoleType;
    Facility: RoleType;
    ServicedBy: RoleType;
    EstimatedBudget: RoleType;
    Description: RoleType;
    Quantity: RoleType;
    UniqueId: RoleType;
    RequestItemVersionsWhereRequirement: AssociationType;
    RequirementVersionWhereChild: AssociationType;
    RequestItemsWhereRequirement: AssociationType;
    RequirementWhereChild: AssociationType;
    WorkEffortVersionsWhereRequirementFulfillment: AssociationType;
    WorkEffortsWhereRequirementFulfillment: AssociationType;
}
export interface MetaRequirementState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    RequirementVersionsWhereRequirementState: AssociationType;
    RequirementsWhereRequirementState: AssociationType;
}
export interface MetaRespondingParty extends ObjectTyped {
    RequestVersionsWhereRespondingParty: AssociationType;
    RequestsWhereRespondingParty: AssociationType;
}
export interface MetaResume extends ObjectTyped {
    PartyVersionWhereResume: AssociationType;
    PartyWhereResume: AssociationType;
}
export interface MetaSalesAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSalesChannel extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    SalesInvoiceVersionsWhereSalesChannel: AssociationType;
    SalesOrderVersionsWhereSalesChannel: AssociationType;
    SalesInvoicesWhereSalesChannel: AssociationType;
    SalesOrdersWhereSalesChannel: AssociationType;
}
export interface MetaSalesChannelRevenue extends ObjectTyped {
}
export interface MetaSalesInvoice extends ObjectTyped {
    SalesInvoiceState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    TotalListPrice: RoleType;
    BillToContactMechanism: RoleType;
    SalesInvoiceType: RoleType;
    InitialProfitMargin: RoleType;
    PaymentMethod: RoleType;
    SalesOrder: RoleType;
    InitialMarkupPercentage: RoleType;
    MaintainedMarkupPercentage: RoleType;
    SalesReps: RoleType;
    Shipment: RoleType;
    MaintainedProfitMargin: RoleType;
    BillToCustomer: RoleType;
    SalesInvoiceItems: RoleType;
    TotalListPriceCustomerCurrency: RoleType;
    ShipToCustomer: RoleType;
    BilledFromContactMechanism: RoleType;
    TotalPurchasePrice: RoleType;
    SalesChannel: RoleType;
    Customers: RoleType;
    ShipToAddress: RoleType;
    Store: RoleType;
    InternalComment: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    CustomerCurrency: RoleType;
    Description: RoleType;
    ShippingAndHandlingCharge: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    Fee: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    DiscountAdjustment: RoleType;
    AmountPaid: RoleType;
    TotalDiscount: RoleType;
    BillingAccount: RoleType;
    TotalIncVat: RoleType;
    TotalSurcharge: RoleType;
    TotalBasePrice: RoleType;
    TotalVatCustomerCurrency: RoleType;
    InvoiceDate: RoleType;
    EntryDate: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    TotalExVat: RoleType;
    InvoiceTerms: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    InvoiceNumber: RoleType;
    Message: RoleType;
    VatRegime: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalVat: RoleType;
    TotalFee: RoleType;
    ContactPerson: RoleType;
    Locale: RoleType;
    Comment: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaSalesInvoiceItem extends ObjectTyped {
    SalesInvoiceItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    ProductFeature: RoleType;
    RequiredProfitMargin: RoleType;
    InitialMarkupPercentage: RoleType;
    MaintainedMarkupPercentage: RoleType;
    Product: RoleType;
    UnitPurchasePrice: RoleType;
    SalesOrderItem: RoleType;
    SalesInvoiceItemType: RoleType;
    SalesRep: RoleType;
    InitialProfitMargin: RoleType;
    MaintainedProfitMargin: RoleType;
    RequiredMarkupPercentage: RoleType;
    InternalComment: RoleType;
    InvoiceTerms: RoleType;
    TotalInvoiceAdjustment: RoleType;
    InvoiceVatRateItems: RoleType;
    AdjustmentFor: RoleType;
    Message: RoleType;
    TotalInvoiceAdjustmentCustomerCurrency: RoleType;
    AmountPaid: RoleType;
    Quantity: RoleType;
    Description: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    SalesInvoiceVersionsWhereSalesInvoiceItem: AssociationType;
    SalesInvoiceWhereSalesInvoiceItem: AssociationType;
    InvoiceItemVersionsWhereAdjustmentFor: AssociationType;
    InvoiceItemsWhereAdjustmentFor: AssociationType;
}
export interface MetaSalesInvoiceItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesInvoiceItemVersionsWhereSalesInvoiceItemState: AssociationType;
    SalesInvoiceItemsWhereSalesInvoiceItemState: AssociationType;
}
export interface MetaSalesInvoiceItemType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    SalesInvoiceItemVersionsWhereSalesInvoiceItemType: AssociationType;
    SalesInvoiceItemsWhereSalesInvoiceItemType: AssociationType;
    SalesOrderItemsWhereItemType: AssociationType;
}
export interface MetaSalesInvoiceState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesInvoiceVersionsWhereSalesInvoiceState: AssociationType;
    SalesInvoicesWhereSalesInvoiceState: AssociationType;
}
export interface MetaSalesInvoiceType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    SalesInvoiceVersionsWhereSalesInvoiceType: AssociationType;
    SalesInvoicesWhereSalesInvoiceType: AssociationType;
}
export interface MetaSalesOrder extends ObjectTyped {
    SalesOrderState: RoleType;
    SalesOrderPaymentState: RoleType;
    SalesOrderShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    TakenByContactMechanism: RoleType;
    ShipToCustomer: RoleType;
    BillToCustomer: RoleType;
    TotalPurchasePrice: RoleType;
    ShipmentMethod: RoleType;
    TotalListPriceCustomerCurrency: RoleType;
    MaintainedProfitMargin: RoleType;
    ShipToAddress: RoleType;
    BillToContactMechanism: RoleType;
    SalesReps: RoleType;
    InitialProfitMargin: RoleType;
    TotalListPrice: RoleType;
    Store: RoleType;
    MaintainedMarkupPercentage: RoleType;
    BillFromContactMechanism: RoleType;
    PaymentMethod: RoleType;
    PlacingContactMechanism: RoleType;
    SalesChannel: RoleType;
    PlacingCustomer: RoleType;
    SalesOrderItems: RoleType;
    InitialMarkupPercentage: RoleType;
    Quote: RoleType;
    InternalComment: RoleType;
    CustomerCurrency: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    CustomerReference: RoleType;
    Fee: RoleType;
    TotalExVat: RoleType;
    OrderTerms: RoleType;
    TotalVat: RoleType;
    TotalSurcharge: RoleType;
    ValidOrderItems: RoleType;
    OrderNumber: RoleType;
    TotalVatCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    Message: RoleType;
    Description: RoleType;
    TotalShippingAndHandlingCustomerCurrency: RoleType;
    EntryDate: RoleType;
    DiscountAdjustment: RoleType;
    OrderKind: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalFeeCustomerCurrency: RoleType;
    TotalShippingAndHandling: RoleType;
    ShippingAndHandlingCharge: RoleType;
    OrderDate: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DeliveryDate: RoleType;
    TotalBasePrice: RoleType;
    TotalFee: RoleType;
    SurchargeAdjustment: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    Locale: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereSalesOrder: AssociationType;
    SalesInvoicesWhereSalesOrder: AssociationType;
}
export interface MetaSalesOrderItem extends ObjectTyped {
    SalesOrderItemState: RoleType;
    SalesOrderItemPaymentState: RoleType;
    SalesOrderItemShipmentState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    InitialProfitMargin: RoleType;
    QuantityShortFalled: RoleType;
    OrderedWithFeatures: RoleType;
    MaintainedProfitMargin: RoleType;
    RequiredProfitMargin: RoleType;
    QuantityShipNow: RoleType;
    RequiredMarkupPercentage: RoleType;
    QuantityShipped: RoleType;
    ShipToAddress: RoleType;
    QuantityPicked: RoleType;
    UnitPurchasePrice: RoleType;
    ShipToParty: RoleType;
    AssignedShipToAddress: RoleType;
    QuantityReturned: RoleType;
    QuantityReserved: RoleType;
    SalesRep: RoleType;
    AssignedShipToParty: RoleType;
    QuantityPendingShipment: RoleType;
    MaintainedMarkupPercentage: RoleType;
    InitialMarkupPercentage: RoleType;
    ReservedFromInventoryItem: RoleType;
    Product: RoleType;
    ProductFeature: RoleType;
    QuantityRequestsShipping: RoleType;
    ItemType: RoleType;
    InternalComment: RoleType;
    QuantityOrdered: RoleType;
    Description: RoleType;
    CorrespondingPurchaseOrder: RoleType;
    TotalOrderAdjustmentCustomerCurrency: RoleType;
    TotalOrderAdjustment: RoleType;
    QuoteItem: RoleType;
    AssignedDeliveryDate: RoleType;
    DeliveryDate: RoleType;
    OrderTerms: RoleType;
    ShippingInstruction: RoleType;
    Associations: RoleType;
    Message: RoleType;
    TotalDiscountAsPercentage: RoleType;
    DiscountAdjustment: RoleType;
    UnitVat: RoleType;
    TotalVatCustomerCurrency: RoleType;
    VatRegime: RoleType;
    TotalVat: RoleType;
    UnitSurcharge: RoleType;
    UnitDiscount: RoleType;
    TotalExVatCustomerCurrency: RoleType;
    DerivedVatRate: RoleType;
    ActualUnitPrice: RoleType;
    TotalIncVatCustomerCurrency: RoleType;
    UnitBasePrice: RoleType;
    CalculatedUnitPrice: RoleType;
    TotalSurchargeCustomerCurrency: RoleType;
    TotalIncVat: RoleType;
    TotalSurchargeAsPercentage: RoleType;
    TotalDiscountCustomerCurrency: RoleType;
    TotalDiscount: RoleType;
    TotalSurcharge: RoleType;
    AssignedVatRegime: RoleType;
    TotalBasePrice: RoleType;
    TotalExVat: RoleType;
    TotalBasePriceCustomerCurrency: RoleType;
    SurchargeAdjustment: RoleType;
    Comment: RoleType;
    SalesInvoiceItemVersionsWhereSalesOrderItem: AssociationType;
    SalesOrderVersionsWhereSalesOrderItem: AssociationType;
    SalesInvoiceItemsWhereSalesOrderItem: AssociationType;
    SalesOrderWhereSalesOrderItem: AssociationType;
    SalesOrderItemVersionsWhereOrderedWithFeature: AssociationType;
    SalesOrderItemWhereOrderedWithFeature: AssociationType;
    OrderItemVersionsWhereAssociation: AssociationType;
    OrderVersionsWhereValidOrderItem: AssociationType;
    OrderWhereValidOrderItem: AssociationType;
    OrderItemsWhereAssociation: AssociationType;
}
export interface MetaRequestItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    RequestItemVersionsWhereRequestItemState: AssociationType;
    RequestItemsWhereRequestItemState: AssociationType;
}
export interface MetaSalesOrderItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrderItemVersionsWhereSalesOrderItemState: AssociationType;
    SalesOrderItemsWhereSalesOrderItemState: AssociationType;
}
export interface MetaRequestState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    RequestVersionWhereRequestState: AssociationType;
    RequestsWhereRequestState: AssociationType;
}
export interface MetaSalesOrderState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SalesOrderVersionsWhereSalesOrderState: AssociationType;
    SalesOrdersWhereSalesOrderState: AssociationType;
}
export interface MetaSalesRepPartyProductCategoryRevenue extends ObjectTyped {
}
export interface MetaSalesRepPartyRevenue extends ObjectTyped {
}
export interface MetaSalesRepProductCategoryRevenue extends ObjectTyped {
}
export interface MetaSalesRepRelationship extends ObjectTyped {
    SalesRepresentative: RoleType;
    LastYearsCommission: RoleType;
    ProductCategories: RoleType;
    YTDCommission: RoleType;
    Customer: RoleType;
    Comment: RoleType;
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSalesRepRevenue extends ObjectTyped {
}
export interface MetaSalesTerritory extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaSalutation extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    PeopleWhereSalutation: AssociationType;
    PersonVersionsWhereSalutation: AssociationType;
}
export interface MetaSerialisedInventoryItem extends ObjectTyped {
    SerialisedInventoryItemState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    SerialNumber: RoleType;
    Ownership: RoleType;
    Owner: RoleType;
    AcquisitionYear: RoleType;
    ManufacturingYear: RoleType;
    ReplacementValue: RoleType;
    LifeTime: RoleType;
    DepreciationYears: RoleType;
    PurchasePrice: RoleType;
    ExpectedSalesPrice: RoleType;
    RefurbishCost: RoleType;
    TransportCost: RoleType;
    ProductCharacteristicValues: RoleType;
    InventoryItemVariances: RoleType;
    Part: RoleType;
    Name: RoleType;
    Lot: RoleType;
    Sku: RoleType;
    UnitOfMeasure: RoleType;
    Good: RoleType;
    ProductType: RoleType;
    Facility: RoleType;
    UniqueId: RoleType;
    WorkEffortVersionsWhereInventoryItemsProduced: AssociationType;
    WorkEffortsWhereInventoryItemsProduced: AssociationType;
}
export interface MetaSerialisedInventoryItemState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    SerialisedInventoryItemVersionsWhereSerialisedInventoryItemState: AssociationType;
    SerialisedInventoryItemsWhereSerialisedInventoryItemState: AssociationType;
}
export interface MetaSerializedInventoryItemObjectState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
}
export interface MetaServiceConfiguration extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaServiceEntryHeader extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaServiceFeature extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaServiceTerritory extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaShipmentItem extends ObjectTyped {
    ShipmentVersionWhereShipmentItem: AssociationType;
}
export interface MetaShipmentMethod extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    SalesOrderVersionsWhereShipmentMethod: AssociationType;
    SalesOrdersWhereShipmentMethod: AssociationType;
    StoresWhereDefaultShipmentMethod: AssociationType;
    PartyVersionsWhereDefaultShipmentMethod: AssociationType;
    PartiesWhereDefaultShipmentMethod: AssociationType;
    ShipmentVersionsWhereShipmentMethod: AssociationType;
}
export interface MetaShipmentPackage extends ObjectTyped {
    UniqueId: RoleType;
    ShipmentVersionWhereShipmentPackage: AssociationType;
}
export interface MetaShipmentRouteSegment extends ObjectTyped {
    ShipmentVersionWhereShipmentRouteSegment: AssociationType;
}
export interface MetaShippingAndHandlingCharge extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    InvoiceVersionsWhereShippingAndHandlingCharge: AssociationType;
    InvoiceWhereShippingAndHandlingCharge: AssociationType;
    OrderVersionsWhereShippingAndHandlingCharge: AssociationType;
    OrderWhereShippingAndHandlingCharge: AssociationType;
}
export interface MetaShippingAndHandlingComponent extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSize extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaSkill extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    QuoteItemVersionsWhereSkill: AssociationType;
    QuoteItemsWhereSkill: AssociationType;
}
export interface MetaSkillLevel extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaSkillRating extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaSoftwareFeature extends ObjectTyped {
    EstimatedProductCosts: RoleType;
    BasePrices: RoleType;
    Description: RoleType;
    DependentFeatures: RoleType;
    IncompatibleFeatures: RoleType;
    VatRate: RoleType;
    UniqueId: RoleType;
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    QuoteItemVersionsWhereProductFeature: AssociationType;
    RequestItemVersionsWhereProductFeature: AssociationType;
    SalesInvoiceItemVersionsWhereProductFeature: AssociationType;
    SalesOrderItemVersionsWhereProductFeature: AssociationType;
    QuoteItemsWhereProductFeature: AssociationType;
    RequestItemsWhereProductFeature: AssociationType;
    SalesInvoiceItemsWhereProductFeature: AssociationType;
    SalesOrderItemsWhereProductFeature: AssociationType;
    ProductsWhereOptionalFeature: AssociationType;
    ProductsWhereStandardFeature: AssociationType;
    ProductsWhereSelectableFeature: AssociationType;
    ProductFeaturesWhereDependentFeature: AssociationType;
    ProductFeaturesWhereIncompatibleFeature: AssociationType;
}
export interface MetaState extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaStatementOfWork extends ObjectTyped {
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    ContactPerson: RoleType;
    PrintContent: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    Comment: RoleType;
}
export interface MetaStatementOfWorkVersion extends ObjectTyped {
    QuoteState: RoleType;
    InternalComment: RoleType;
    RequiredResponseDate: RoleType;
    ValidFromDate: RoleType;
    QuoteTerms: RoleType;
    ValidThroughDate: RoleType;
    Description: RoleType;
    Receiver: RoleType;
    FullfillContactMechanism: RoleType;
    Price: RoleType;
    Currency: RoleType;
    IssueDate: RoleType;
    QuoteItems: RoleType;
    QuoteNumber: RoleType;
    Request: RoleType;
    DerivationTimeStamp: RoleType;
    StatementOfWorkWhereCurrentVersion: AssociationType;
    StatementOfWorkWhereAllVersion: AssociationType;
}
export interface MetaStore extends ObjectTyped {
    Catalogues: RoleType;
    OutgoingShipmentNumberPrefix: RoleType;
    SalesInvoiceNumberPrefix: RoleType;
    PaymentGracePeriod: RoleType;
    LogoImage: RoleType;
    PaymentNetDays: RoleType;
    DefaultFacility: RoleType;
    Name: RoleType;
    CreditLimit: RoleType;
    DefaultShipmentMethod: RoleType;
    DefaultCarrier: RoleType;
    DefaultPaymentMethod: RoleType;
    PaymentMethods: RoleType;
    SalesOrderNumberPrefix: RoleType;
    ProcessFlow: RoleType;
    UniqueId: RoleType;
    PickListVersionsWhereStore: AssociationType;
    SalesInvoiceVersionsWhereStore: AssociationType;
    PickListsWhereStore: AssociationType;
    SalesInvoicesWhereStore: AssociationType;
    SalesOrdersWhereStore: AssociationType;
    ShipmentVersionsWhereStore: AssociationType;
}
export interface MetaStoreRevenue extends ObjectTyped {
}
export interface MetaStringTemplate extends ObjectTyped {
    UniqueId: RoleType;
    Locale: RoleType;
}
export interface MetaSubAssembly extends ObjectTyped {
    UniqueId: RoleType;
    InventoryItemVersionsWherePart: AssociationType;
    InventoryItemsWherePart: AssociationType;
}
export interface MetaSubContractorAgreement extends ObjectTyped {
    UniqueId: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSubContractorRelationship extends ObjectTyped {
    Contractor: RoleType;
    SubContractor: RoleType;
    Parties: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSupplierOffering extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSupplierRelationship extends ObjectTyped {
    Supplier: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaSurchargeAdjustment extends ObjectTyped {
    Amount: RoleType;
    VatRate: RoleType;
    Percentage: RoleType;
    InvoiceVersionsWhereSurchargeAdjustment: AssociationType;
    InvoiceWhereSurchargeAdjustment: AssociationType;
    OrderVersionsWhereSurchargeAdjustment: AssociationType;
    OrderWhereSurchargeAdjustment: AssociationType;
    PriceableWhereSurchargeAdjustment: AssociationType;
}
export interface MetaSurchargeComponent extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaRequirementType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    RequirementVersionsWhereRequirementType: AssociationType;
    RequirementsWhereRequirementType: AssociationType;
}
export interface MetaInvoiceTermType extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    OrderTermsWhereTermType: AssociationType;
    AgreementTermsWhereTermType: AssociationType;
}
export interface MetaTransferVersion extends ObjectTyped {
    TransferState: RoleType;
    ShipmentMethod: RoleType;
    BillToContactMechanism: RoleType;
    ShipmentPackages: RoleType;
    ShipmentNumber: RoleType;
    Documents: RoleType;
    BillToParty: RoleType;
    ShipToParty: RoleType;
    ShipmentItems: RoleType;
    ReceiverContactMechanism: RoleType;
    ShipToAddress: RoleType;
    EstimatedShipCost: RoleType;
    EstimatedShipDate: RoleType;
    LatestCancelDate: RoleType;
    Carrier: RoleType;
    InquireAboutContactMechanism: RoleType;
    EstimatedReadyDate: RoleType;
    ShipFromAddress: RoleType;
    BillFromContactMechanism: RoleType;
    HandlingInstruction: RoleType;
    Store: RoleType;
    ShipFromParty: RoleType;
    ShipmentRouteSegments: RoleType;
    EstimatedArrivalDate: RoleType;
    DerivationTimeStamp: RoleType;
    TransferWhereCurrentVersion: AssociationType;
    TransferWhereAllVersion: AssociationType;
}
export interface MetaWebSiteCommunicationVersion extends ObjectTyped {
    Originator: RoleType;
    Receiver: RoleType;
    CommunicationEventState: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    DerivationTimeStamp: RoleType;
    WebSiteCommunicationWhereCurrentVersion: AssociationType;
    WebSiteCommunicationWhereAllVersion: AssociationType;
}
export interface MetaWorkTask extends ObjectTyped {
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    WorkEffortState: RoleType;
    Owner: RoleType;
    Name: RoleType;
    Description: RoleType;
    Priority: RoleType;
    WorkEffortPurposes: RoleType;
    ActualCompletion: RoleType;
    ScheduledStart: RoleType;
    ScheduledCompletion: RoleType;
    ActualHours: RoleType;
    EstimatedHours: RoleType;
    Precendencies: RoleType;
    Facility: RoleType;
    DeliverablesProduced: RoleType;
    ActualStart: RoleType;
    InventoryItemsNeeded: RoleType;
    Children: RoleType;
    WorkEffortType: RoleType;
    InventoryItemsProduced: RoleType;
    RequirementFulfillments: RoleType;
    SpecialTerms: RoleType;
    Concurrencies: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    QuoteItemVersionsWhereWorkEffort: AssociationType;
    QuoteItemsWhereWorkEffort: AssociationType;
    WorkEffortAssignmentsWhereAssignment: AssociationType;
    CommunicationEventVersionsWhereWorkEffort: AssociationType;
    CommunicationEventsWhereWorkEffort: AssociationType;
    WorkEffortVersionsWherePrecendency: AssociationType;
    WorkEffortVersionsWhereChild: AssociationType;
    WorkEffortVersionsWhereConcurrency: AssociationType;
    WorkEffortsWherePrecendency: AssociationType;
    WorkEffortsWhereChild: AssociationType;
    WorkEffortsWhereConcurrency: AssociationType;
}
export interface MetaTaxDocument extends ObjectTyped {
    PrintContent: RoleType;
    Comment: RoleType;
    ProductsWhereDocument: AssociationType;
    ShipmentVersionWhereDocument: AssociationType;
}
export interface MetaTelecommunicationsNumber extends ObjectTyped {
    CountryCode: RoleType;
    AreaCode: RoleType;
    ContactNumber: RoleType;
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    FaxCommunicationsWhereOutgoingFaxNumber: AssociationType;
    FaxCommunicationVersionsWhereOutgoingFaxNumber: AssociationType;
    PartyVersionsWhereBillingInquiriesFax: AssociationType;
    PartyVersionsWhereOrderInquiriesFax: AssociationType;
    PartyVersionsWhereShippingInquiriesFax: AssociationType;
    PartyVersionsWhereShippingInquiriesPhone: AssociationType;
    PartyVersionsWhereOrderInquiriesPhone: AssociationType;
    PartyVersionsWhereCellPhoneNumber: AssociationType;
    PartyVersionsWhereBillingInquiriesPhone: AssociationType;
    PartyVersionsWhereGeneralFaxNumber: AssociationType;
    PartyVersionsWhereGeneralPhoneNumber: AssociationType;
    PartiesWhereBillingInquiriesFax: AssociationType;
    PartiesWhereOrderInquiriesFax: AssociationType;
    PartiesWhereShippingInquiriesFax: AssociationType;
    PartiesWhereShippingInquiriesPhone: AssociationType;
    PartiesWhereOrderInquiriesPhone: AssociationType;
    PartiesWhereCellPhoneNumber: AssociationType;
    PartiesWhereBillingInquiriesPhone: AssociationType;
    PartiesWhereGeneralFaxNumber: AssociationType;
    PartiesWhereGeneralPhoneNumber: AssociationType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaTerritory extends ObjectTyped {
    Latitude: RoleType;
    Longitude: RoleType;
    UniqueId: RoleType;
}
export interface MetaThreshold extends ObjectTyped {
    TermValue: RoleType;
    TermType: RoleType;
    Description: RoleType;
    InvoiceItemVersionsWhereInvoiceTerm: AssociationType;
    InvoiceItemWhereInvoiceTerm: AssociationType;
}
export interface MetaTimeAndMaterialsService extends ObjectTyped {
    InternalComment: RoleType;
    PrimaryProductCategory: RoleType;
    SupportDiscontinuationDate: RoleType;
    SalesDiscontinuationDate: RoleType;
    LocalisedNames: RoleType;
    LocalisedDescriptions: RoleType;
    LocalisedComments: RoleType;
    Description: RoleType;
    ProductComplement: RoleType;
    OptionalFeatures: RoleType;
    Variants: RoleType;
    Name: RoleType;
    IntroductionDate: RoleType;
    Documents: RoleType;
    StandardFeatures: RoleType;
    UnitOfMeasure: RoleType;
    EstimatedProductCosts: RoleType;
    ProductObsolescences: RoleType;
    SelectableFeatures: RoleType;
    VatRate: RoleType;
    BasePrices: RoleType;
    ProductCategories: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    GeneralLedgerAccountsWhereDefaultCostUnit: AssociationType;
    GeneralLedgerAccountsWhereCostUnitsAllowed: AssociationType;
    GoodsWhereProductSubstitution: AssociationType;
    GoodsWhereProductIncompatibility: AssociationType;
    QuoteItemVersionsWhereProduct: AssociationType;
    RequestItemVersionsWhereProduct: AssociationType;
    SalesInvoiceItemVersionsWhereProduct: AssociationType;
    SalesOrderItemVersionsWhereProduct: AssociationType;
    OrganisationGlAccountsWhereProduct: AssociationType;
    ProductCategoriesWhereAllProduct: AssociationType;
    QuoteItemsWhereProduct: AssociationType;
    RequestItemsWhereProduct: AssociationType;
    SalesInvoiceItemsWhereProduct: AssociationType;
    SalesOrderItemsWhereProduct: AssociationType;
    ProductsWhereProductComplement: AssociationType;
    ProductWhereVariant: AssociationType;
    ProductsWhereProductObsolescence: AssociationType;
}
export interface MetaTimeEntry extends ObjectTyped {
    Comment: RoleType;
}
export interface MetaTimeFrequency extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    AccountingPeriodVersionsWhereTimeFrequency: AssociationType;
    VatRatesWherePaymentFrequency: AssociationType;
}
export interface MetaTimePeriodUsage extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaTransfer extends ObjectTyped {
    TransferState: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    PrintContent: RoleType;
    Comment: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    SalesInvoiceVersionsWhereShipment: AssociationType;
    SalesInvoicesWhereShipment: AssociationType;
}
export interface MetaTransferState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    TransferVersionsWhereTransferState: AssociationType;
    TransfersWhereTransferState: AssociationType;
}
export interface MetaUnitOfMeasure extends ObjectTyped {
    UniqueId: RoleType;
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    QuoteItemVersionsWhereUnitOfMeasure: AssociationType;
    RequestItemVersionsWhereUnitOfMeasure: AssociationType;
    QuoteItemsWhereUnitOfMeasure: AssociationType;
    RequestItemsWhereUnitOfMeasure: AssociationType;
    InventoryItemVersionsWhereUnitOfMeasure: AssociationType;
    InventoryItemsWhereUnitOfMeasure: AssociationType;
    ProductsWhereUnitOfMeasure: AssociationType;
}
export interface MetaUtilizationCharge extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
    ProductsWhereBasePrice: AssociationType;
    ProductFeaturesWhereBasePrice: AssociationType;
}
export interface MetaVarianceReason extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    InventoryItemVariancesWhereReason: AssociationType;
}
export interface MetaVatCalculationMethod extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    VatRatesWhereVatCalculationMethod: AssociationType;
}
export interface MetaVatForm extends ObjectTyped {
    CountryWhereVatForm: AssociationType;
}
export interface MetaVatRate extends ObjectTyped {
    VatCalculationMethod: RoleType;
    VatReturnBoxes: RoleType;
    Rate: RoleType;
    VatPayableAccount: RoleType;
    TaxAuthority: RoleType;
    VatRateUsage: RoleType;
    VatRatePurchaseKind: RoleType;
    VatTariff: RoleType;
    PaymentFrequency: RoleType;
    VatToPayAccount: RoleType;
    EuSalesListType: RoleType;
    VatToReceiveAccount: RoleType;
    VatReceivableAccount: RoleType;
    ReverseCharge: RoleType;
    CountryWhereVatRate: AssociationType;
    InvoiceVatRateItemsWhereVatRate: AssociationType;
    VatRegimesWhereVatRate: AssociationType;
    OrderAdjustmentVersionsWhereVatRate: AssociationType;
    OrderAdjustmentsWhereVatRate: AssociationType;
    PriceablesWhereDerivedVatRate: AssociationType;
    ProductsWhereVatRate: AssociationType;
    ProductFeaturesWhereVatRate: AssociationType;
}
export interface MetaVatRatePurchaseKind extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    VatRatesWhereVatRatePurchaseKind: AssociationType;
}
export interface MetaVatRateUsage extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    VatRatesWhereVatRateUsage: AssociationType;
}
export interface MetaVatRegime extends ObjectTyped {
    VatRate: RoleType;
    GeneralLedgerAccount: RoleType;
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    InternalOrganisationWhereVatRegime: AssociationType;
    InvoiceVersionsWhereVatRegime: AssociationType;
    InvoicesWhereVatRegime: AssociationType;
    OrderVersionsWhereVatRegime: AssociationType;
    PartyVersionsWhereVatRegime: AssociationType;
    OrdersWhereVatRegime: AssociationType;
    PartiesWhereVatRegime: AssociationType;
    PriceablesWhereVatRegime: AssociationType;
    PriceablesWhereAssignedVatRegime: AssociationType;
}
export interface MetaVatReturnBox extends ObjectTyped {
    VatRatesWhereVatReturnBox: AssociationType;
}
export interface MetaVatTariff extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    VatRatesWhereVatTariff: AssociationType;
}
export interface MetaVolumeUsage extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaWebAddress extends ObjectTyped {
    ElectronicAddressString: RoleType;
    Description: RoleType;
    ContactMechanismType: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
    PartyVersionsWhereGeneralEmail: AssociationType;
    PartyVersionWherePersonalEmailAddress: AssociationType;
    PartyVersionsWhereInternetAddress: AssociationType;
    PartiesWhereInternetAddress: AssociationType;
    InternalOrganisationsWhereBillingAddress: AssociationType;
    InternalOrganisationsWhereOrderAddress: AssociationType;
    SalesInvoiceVersionsWhereBillToContactMechanism: AssociationType;
    SalesInvoiceVersionsWhereBilledFromContactMechanism: AssociationType;
    SalesOrderVersionsWhereTakenByContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillToContactMechanism: AssociationType;
    SalesOrderVersionsWhereBillFromContactMechanism: AssociationType;
    SalesOrderVersionsWherePlacingContactMechanism: AssociationType;
    PartyContactMechanismsWhereContactMechanism: AssociationType;
    SalesInvoicesWhereBillToContactMechanism: AssociationType;
    SalesInvoicesWhereBilledFromContactMechanism: AssociationType;
    SalesOrdersWhereTakenByContactMechanism: AssociationType;
    SalesOrdersWhereBillToContactMechanism: AssociationType;
    SalesOrdersWhereBillFromContactMechanism: AssociationType;
    SalesOrdersWherePlacingContactMechanism: AssociationType;
    CommunicationEventVersionsWhereContactMechanism: AssociationType;
    CommunicationEventsWhereContactMechanism: AssociationType;
    PartyVersionsWhereHomeAddress: AssociationType;
    PartyVersionsWhereSalesOffice: AssociationType;
    PartyVersionsWhereBillingAddress: AssociationType;
    PartyVersionsWhereHeadQuarter: AssociationType;
    PartyVersionsWhereOrderAddress: AssociationType;
    QuoteVersionsWhereFullfillContactMechanism: AssociationType;
    RequestVersionsWhereFullfillContactMechanism: AssociationType;
    PartiesWhereHomeAddress: AssociationType;
    PartiesWhereSalesOffice: AssociationType;
    PartiesWhereBillingAddress: AssociationType;
    PartiesWhereHeadQuarter: AssociationType;
    PartiesWhereOrderAddress: AssociationType;
    QuotesWhereFullfillContactMechanism: AssociationType;
    RequestsWhereFullfillContactMechanism: AssociationType;
    ShipmentVersionsWhereBillToContactMechanism: AssociationType;
    ShipmentVersionsWhereReceiverContactMechanism: AssociationType;
    ShipmentVersionsWhereInquireAboutContactMechanism: AssociationType;
    ShipmentVersionsWhereBillFromContactMechanism: AssociationType;
}
export interface MetaWebSiteCommunication extends ObjectTyped {
    Originator: RoleType;
    Receiver: RoleType;
    CurrentVersion: RoleType;
    AllVersions: RoleType;
    CommunicationEventState: RoleType;
    ScheduledStart: RoleType;
    ToParties: RoleType;
    ContactMechanisms: RoleType;
    InvolvedParties: RoleType;
    InitialScheduledStart: RoleType;
    EventPurposes: RoleType;
    ScheduledEnd: RoleType;
    ActualEnd: RoleType;
    WorkEfforts: RoleType;
    Description: RoleType;
    InitialScheduledEnd: RoleType;
    FromParties: RoleType;
    Subject: RoleType;
    Documents: RoleType;
    Case: RoleType;
    Priority: RoleType;
    Owner: RoleType;
    Note: RoleType;
    ActualStart: RoleType;
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Comment: RoleType;
    UniqueId: RoleType;
    CreatedBy: RoleType;
    LastModifiedBy: RoleType;
    CreationDate: RoleType;
    LastModifiedDate: RoleType;
}
export interface MetaWorkEffortAssignment extends ObjectTyped {
    Professional: RoleType;
    Assignment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
}
export interface MetaWorkEffortFixedAssetAssignment extends ObjectTyped {
    Comment: RoleType;
    FromDate: RoleType;
    ThroughDate: RoleType;
}
export interface MetaWorkEffortInventoryAssignment extends ObjectTyped {
    WorkEffortVersionsWhereInventoryItemsNeeded: AssociationType;
    WorkEffortWhereInventoryItemsNeeded: AssociationType;
}
export interface MetaWorkEffortState extends ObjectTyped {
    Name: RoleType;
    UniqueId: RoleType;
    WorkEffortsWhereWorkEffortState: AssociationType;
}
export interface MetaWorkEffortPartyAssignment extends ObjectTyped {
    FromDate: RoleType;
    ThroughDate: RoleType;
    Comment: RoleType;
}
export interface MetaWorkEffortPurpose extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
    WorkEffortVersionsWhereWorkEffortPurpose: AssociationType;
    WorkEffortsWhereWorkEffortPurpose: AssociationType;
}
export interface MetaWorkEffortType extends ObjectTyped {
    WorkEffortVersionsWhereWorkEffortType: AssociationType;
    WorkEffortsWhereWorkEffortType: AssociationType;
}
export interface MetaWorkEffortTypeKind extends ObjectTyped {
    LocalisedNames: RoleType;
    Name: RoleType;
    IsActive: RoleType;
    UniqueId: RoleType;
}
export interface MetaWorkTaskVersion extends ObjectTyped {
    SendNotification: RoleType;
    SendReminder: RoleType;
    RemindAt: RoleType;
    Name: RoleType;
    Description: RoleType;
    Priority: RoleType;
    WorkEffortPurposes: RoleType;
    ActualCompletion: RoleType;
    ScheduledStart: RoleType;
    ScheduledCompletion: RoleType;
    ActualHours: RoleType;
    EstimatedHours: RoleType;
    Precendencies: RoleType;
    Facility: RoleType;
    DeliverablesProduced: RoleType;
    ActualStart: RoleType;
    InventoryItemsNeeded: RoleType;
    Children: RoleType;
    WorkEffortType: RoleType;
    InventoryItemsProduced: RoleType;
    RequirementFulfillments: RoleType;
    SpecialTerms: RoleType;
    Concurrencies: RoleType;
    DerivationTimeStamp: RoleType;
    WorkTaskWhereCurrentVersion: AssociationType;
    WorkTaskWhereAllVersion: AssociationType;
}
export declare let data: Data;
