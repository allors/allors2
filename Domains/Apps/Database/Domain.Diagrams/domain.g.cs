namespace Allors.Domain
{
		public interface AccessControlledObject  : Object 
		{
						Permission DeniedPermissions {set;}

						SecurityToken SecurityTokens {set;}

		}
		public interface AsyncDerivable  : Object 
		{
		}
		public interface Deletable  : Object 
		{
		}
		public interface Enumeration  : AccessControlledObject, UniquelyIdentifiable 
		{
						LocalisedText LocalisedNames {set;}

						global::System.String Name {set;}

						global::System.Boolean IsActive {set;}

		}
		public interface Localised  : Object 
		{
						Locale Locale {set;}

		}
		public interface Object 
		{
		}
		public interface ObjectState  : UniquelyIdentifiable 
		{
						Permission DeniedPermissions {set;}

						global::System.String Name {set;}

		}
		public interface SecurityTokenOwner  : Object 
		{
						SecurityToken OwnerSecurityToken {set;}

						AccessControl OwnerAccessControl {set;}

		}
		public interface Transitional  : AccessControlledObject 
		{
						ObjectState PreviousObjectState {set;}

						ObjectState LastObjectState {set;}

		}
		public interface UniquelyIdentifiable  : Object 
		{
						global::System.Guid UniqueId {set;}

		}
		public interface User  : SecurityTokenOwner, AccessControlledObject, Localised 
		{
						global::System.Boolean? UserEmailConfirmed {set;}

						global::System.String UserName {set;}

						global::System.String UserEmail {set;}

						global::System.String UserPasswordHash {set;}

		}
		public interface AccountingTransaction  : AccessControlledObject 
		{
						AccountingTransactionDetail AccountingTransactionDetails {set;}

						global::System.String Description {set;}

						global::System.DateTime TransactionDate {set;}

						global::System.Decimal DerivedTotalAmount {set;}

						AccountingTransactionNumber AccountingTransactionNumber {set;}

						global::System.DateTime EntryDate {set;}

		}
		public interface Agreement  : AccessControlledObject, UniquelyIdentifiable, Period 
		{
						global::System.DateTime? AgreementDate {set;}

						Addendum Addenda {set;}

						global::System.String Description {set;}

						AgreementTerm AgreementTerms {set;}

						global::System.String Text {set;}

						AgreementItem AgreementItems {set;}

						global::System.String AgreementNumber {set;}

		}
		public interface AgreementItem  : AccessControlledObject 
		{
						global::System.String Text {set;}

						Addendum Addenda {set;}

						AgreementItem Children {set;}

						global::System.String Description {set;}

						AgreementTerm AgreementTerms {set;}

		}
		public interface AgreementTerm  : AccessControlledObject 
		{
						global::System.String TermValue {set;}

						TermType TermType {set;}

						global::System.String Description {set;}

		}
		public interface Auditable  : AccessControlledObject 
		{
						User CreatedBy {set;}

						User LastModifiedBy {set;}

						global::System.DateTime? CreationDate {set;}

						global::System.DateTime? LastModifiedDate {set;}

		}
		public interface Budget  : Period, Commentable, UniquelyIdentifiable, Transitional 
		{
						global::System.String Description {set;}

						BudgetRevision BudgetRevisions {set;}

						BudgetStatus BudgetStatuses {set;}

						global::System.String BudgetNumber {set;}

						BudgetObjectState CurrentObjectState {set;}

						BudgetReview BudgetReviews {set;}

						BudgetStatus CurrentBudgetStatus {set;}

						BudgetItem BudgetItems {set;}

		}
		public interface CityBound  : AccessControlledObject 
		{
						City Cities {set;}

		}
		public interface Commentable  : Object 
		{
						global::System.String Comment {set;}

		}
		public interface CommunicationAttachment  : AccessControlledObject 
		{
		}
		public interface CommunicationEvent  : Transitional, Deletable, Commentable, UniquelyIdentifiable, Auditable 
		{
						SecurityToken OwnerSecurityToken {set;}

						AccessControl OwnerAccessControl {set;}

						global::System.DateTime? ScheduledStart {set;}

						Party ToParties {set;}

						ContactMechanism ToContactMechanisms {set;}

						CommunicationEventStatus CommunicationEventStatuses {set;}

						Party InvolvedParties {set;}

						global::System.DateTime? InitialScheduledStart {set;}

						CommunicationEventObjectState CurrentObjectState {set;}

						CommunicationEventPurpose EventPurposes {set;}

						global::System.DateTime? ScheduledEnd {set;}

						global::System.DateTime? ActualEnd {set;}

						WorkEffort WorkEfforts {set;}

						global::System.String Description {set;}

						global::System.DateTime? InitialScheduledEnd {set;}

						Party FromParties {set;}

						global::System.String Subject {set;}

						Media Documents {set;}

						Case Case {set;}

						Priority Priority {set;}

						ContactMechanism FromContactMechanisms {set;}

						Person Owner {set;}

						CommunicationEventStatus CurrentCommunicationEventStatus {set;}

						global::System.String Note {set;}

						global::System.DateTime? ActualStart {set;}

		}
		public interface ContactMechanism  : Auditable, Deletable 
		{
						global::System.String Description {set;}

						ContactMechanism FollowTo {set;}

		}
		public interface Container  : AccessControlledObject 
		{
						Facility Facility {set;}

						global::System.String ContainerDescription {set;}

		}
		public interface CountryBound  : AccessControlledObject 
		{
						Country Country {set;}

		}
		public interface DeploymentUsage  : AccessControlledObject, Commentable, Period 
		{
						TimeFrequency TimeFrequency {set;}

		}
		public interface Document  : Printable, Commentable 
		{
						global::System.String Name {set;}

						global::System.String Description {set;}

						global::System.String Text {set;}

						global::System.String DocumentLocation {set;}

		}
		public interface ElectronicAddress  : ContactMechanism 
		{
						global::System.String ElectronicAddressString {set;}

		}
		public interface EngagementItem  : AccessControlledObject 
		{
						QuoteItem QuoteItem {set;}

						global::System.String Description {set;}

						global::System.DateTime? ExpectedStartDate {set;}

						global::System.DateTime? ExpectedEndDate {set;}

						WorkEffort EngagementWorkFulfillment {set;}

						EngagementRate EngagementRates {set;}

						EngagementRate CurrentEngagementRate {set;}

						EngagementItem OrderedWiths {set;}

						Person CurrentAssignedProfessional {set;}

						Product Product {set;}

						ProductFeature ProductFeature {set;}

		}
		public interface EstimatedProductCost  : Period, AccessControlledObject 
		{
						global::System.Decimal Cost {set;}

						Currency Currency {set;}

						Organisation Organisation {set;}

						global::System.String Description {set;}

						GeographicBoundary GeographicBoundary {set;}

		}
		public interface ExternalAccountingTransaction  : AccountingTransaction 
		{
						Party FromParty {set;}

						Party ToParty {set;}

		}
		public interface Facility  : GeoLocatable 
		{
						Facility MadeUpOf {set;}

						global::System.Decimal? SquareFootage {set;}

						global::System.String Description {set;}

						ContactMechanism FacilityContactMechanisms {set;}

						global::System.String Name {set;}

						InternalOrganisation Owner {set;}

		}
		public interface FinancialAccount  : AccessControlledObject 
		{
						FinancialAccountTransaction FinancialAccountTransactions {set;}

		}
		public interface FinancialAccountTransaction  : AccessControlledObject 
		{
						global::System.String Description {set;}

						global::System.DateTime? EntryDate {set;}

						global::System.DateTime TransactionDate {set;}

		}
		public interface FixedAsset  : AccessControlledObject 
		{
						global::System.String Name {set;}

						global::System.DateTime? LastServiceDate {set;}

						global::System.DateTime? AcquiredDate {set;}

						global::System.String Description {set;}

						global::System.Decimal? ProductionCapacity {set;}

						global::System.DateTime? NextServiceDate {set;}

		}
		public interface GeographicBoundary  : GeoLocatable 
		{
						global::System.String Abbreviation {set;}

		}
		public interface GeographicBoundaryComposite  : GeographicBoundary 
		{
						GeographicBoundary Associations {set;}

		}
		public interface GeoLocatable  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.Decimal Latitude {set;}

						global::System.Decimal Longitude {set;}

		}
		public interface InternalAccountingTransaction  : AccountingTransaction 
		{
						InternalOrganisation InternalOrganisation {set;}

		}
		public interface InventoryItem  : Transitional, UniquelyIdentifiable 
		{
						InventoryItemVariance InventoryItemVariances {set;}

						Part Part {set;}

						Container Container {set;}

						global::System.String Name {set;}

						Lot Lot {set;}

						global::System.String Sku {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						ProductCategory DerivedProductCategories {set;}

						Good Good {set;}

						Facility Facility {set;}

		}
		public interface InventoryItemConfiguration  : Commentable, AccessControlledObject 
		{
						InventoryItem InventoryItem {set;}

						global::System.Int32 Quantity {set;}

						InventoryItem ComponentInventoryItem {set;}

		}
		public interface Invoice  : Localised, Transitional, Commentable, Printable, Auditable 
		{
						global::System.Decimal TotalShippingAndHandlingCustomerCurrency {set;}

						Currency CustomerCurrency {set;}

						global::System.String Description {set;}

						ShippingAndHandlingCharge ShippingAndHandlingCharge {set;}

						global::System.Decimal TotalFeeCustomerCurrency {set;}

						Fee Fee {set;}

						global::System.Decimal TotalExVatCustomerCurrency {set;}

						global::System.String CustomerReference {set;}

						DiscountAdjustment DiscountAdjustment {set;}

						global::System.Decimal AmountPaid {set;}

						global::System.Decimal TotalDiscount {set;}

						BillingAccount BillingAccount {set;}

						global::System.Decimal TotalIncVat {set;}

						global::System.Decimal TotalSurcharge {set;}

						global::System.Decimal TotalBasePrice {set;}

						global::System.Decimal TotalVatCustomerCurrency {set;}

						global::System.DateTime InvoiceDate {set;}

						global::System.DateTime EntryDate {set;}

						global::System.Decimal TotalIncVatCustomerCurrency {set;}

						global::System.Decimal TotalShippingAndHandling {set;}

						global::System.Decimal TotalBasePriceCustomerCurrency {set;}

						SurchargeAdjustment SurchargeAdjustment {set;}

						global::System.Decimal TotalExVat {set;}

						InvoiceTerm InvoiceTerms {set;}

						global::System.Decimal TotalSurchargeCustomerCurrency {set;}

						global::System.String InvoiceNumber {set;}

						global::System.String Message {set;}

						VatRegime VatRegime {set;}

						global::System.Decimal TotalDiscountCustomerCurrency {set;}

						global::System.Decimal TotalVat {set;}

						global::System.Decimal TotalFee {set;}

		}
		public interface InvoiceItem  : Priceable 
		{
						AgreementTerm InvoiceTerms {set;}

						global::System.Decimal TotalInvoiceAdjustment {set;}

						InvoiceVatRateItem InvoiceVatRateItems {set;}

						InvoiceItem AdjustmentFor {set;}

						SerializedInventoryItem SerializedInventoryItem {set;}

						global::System.String Message {set;}

						global::System.Decimal TotalInvoiceAdjustmentCustomerCurrency {set;}

						global::System.Decimal AmountPaid {set;}

						global::System.Decimal Quantity {set;}

						global::System.String Description {set;}

		}
		public interface IUnitOfMeasure  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.String Description {set;}

						UnitOfMeasureConversion UnitOfMeasureConversions {set;}

						global::System.String Abbreviation {set;}

		}
		public interface Order  : Printable, Transitional, Commentable, Localised, Auditable 
		{
						Currency CustomerCurrency {set;}

						global::System.Decimal TotalBasePriceCustomerCurrency {set;}

						global::System.Decimal TotalIncVatCustomerCurrency {set;}

						global::System.Decimal TotalDiscountCustomerCurrency {set;}

						global::System.String CustomerReference {set;}

						Fee Fee {set;}

						global::System.Decimal TotalExVat {set;}

						OrderTerm OrderTerms {set;}

						global::System.Decimal TotalVat {set;}

						global::System.Decimal TotalSurcharge {set;}

						OrderItem ValidOrderItems {set;}

						global::System.String OrderNumber {set;}

						global::System.Decimal TotalVatCustomerCurrency {set;}

						global::System.Decimal TotalDiscount {set;}

						global::System.String Message {set;}

						global::System.Decimal TotalShippingAndHandlingCustomerCurrency {set;}

						global::System.DateTime EntryDate {set;}

						DiscountAdjustment DiscountAdjustment {set;}

						OrderKind OrderKind {set;}

						global::System.Decimal TotalIncVat {set;}

						global::System.Decimal TotalSurchargeCustomerCurrency {set;}

						VatRegime VatRegime {set;}

						global::System.Decimal TotalFeeCustomerCurrency {set;}

						global::System.Decimal TotalShippingAndHandling {set;}

						ShippingAndHandlingCharge ShippingAndHandlingCharge {set;}

						global::System.DateTime OrderDate {set;}

						global::System.Decimal TotalExVatCustomerCurrency {set;}

						global::System.DateTime? DeliveryDate {set;}

						global::System.Decimal TotalBasePrice {set;}

						global::System.Decimal TotalFee {set;}

						SurchargeAdjustment SurchargeAdjustment {set;}

		}
		public interface OrderAdjustment  : AccessControlledObject 
		{
						global::System.Decimal? Amount {set;}

						VatRate VatRate {set;}

						global::System.Decimal? Percentage {set;}

		}
		public interface OrderItem  : Priceable 
		{
						BudgetItem BudgetItem {set;}

						global::System.Decimal PreviousQuantity {set;}

						global::System.Decimal QuantityOrdered {set;}

						global::System.String Description {set;}

						PurchaseOrder CorrespondingPurchaseOrder {set;}

						global::System.Decimal TotalOrderAdjustmentCustomerCurrency {set;}

						global::System.Decimal TotalOrderAdjustment {set;}

						QuoteItem QuoteItem {set;}

						global::System.DateTime? AssignedDeliveryDate {set;}

						global::System.DateTime? DeliveryDate {set;}

						OrderTerm OrderTerms {set;}

						global::System.String ShippingInstruction {set;}

						OrderItem Associations {set;}

						global::System.String Message {set;}

		}
		public interface OrganisationClassification  : PartyClassification 
		{
		}
		public interface Part  : AccessControlledObject, UniquelyIdentifiable 
		{
						InternalOrganisation OwnedByParty {set;}

						global::System.String Name {set;}

						PartSpecification PartSpecifications {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						Document Documents {set;}

						global::System.String ManufacturerId {set;}

						global::System.Int32? ReorderLevel {set;}

						global::System.Int32? ReorderQuantity {set;}

						PriceComponent PriceComponents {set;}

						InventoryItemKind InventoryItemKind {set;}

						global::System.String Sku {set;}

		}
		public interface PartBillOfMaterial  : AccessControlledObject, Commentable, Period 
		{
						Part Part {set;}

						global::System.String Instruction {set;}

						global::System.Int32 QuantityUsed {set;}

						Part ComponentPart {set;}

		}
		public interface PartSpecification  : UniquelyIdentifiable, Commentable, Transitional 
		{
						PartSpecificationStatus PartSpecificationStatuses {set;}

						PartSpecificationObjectState CurrentObjectState {set;}

						global::System.DateTime? DocumentationDate {set;}

						PartSpecificationStatus CurrentPartSpecificationStatus {set;}

						global::System.String Description {set;}

		}
		public interface Party  : Localised, Auditable, UniquelyIdentifiable 
		{
						PostalAddress GeneralCorrespondence {set;}

						global::System.Decimal YTDRevenue {set;}

						global::System.Decimal LastYearsRevenue {set;}

						TelecommunicationsNumber BillingInquiriesFax {set;}

						Qualification Qualifications {set;}

						ContactMechanism HomeAddress {set;}

						OrganisationContactRelationship InactiveOrganisationContactRelationships {set;}

						ContactMechanism SalesOffice {set;}

						Person InactiveContacts {set;}

						PartyContactMechanism InactivePartyContactMechanisms {set;}

						TelecommunicationsNumber OrderInquiriesFax {set;}

						Person CurrentSalesReps {set;}

						PartyContactMechanism PartyContactMechanisms {set;}

						TelecommunicationsNumber ShippingInquiriesFax {set;}

						TelecommunicationsNumber ShippingInquiriesPhone {set;}

						BillingAccount BillingAccounts {set;}

						TelecommunicationsNumber OrderInquiriesPhone {set;}

						PartySkill PartySkills {set;}

						PartyClassification PartyClassifications {set;}

						global::System.Boolean? ExcludeFromDunning {set;}

						BankAccount BankAccounts {set;}

						Person CurrentContacts {set;}

						ContactMechanism BillingAddress {set;}

						ElectronicAddress GeneralEmail {set;}

						ShipmentMethod DefaultShipmentMethod {set;}

						Resume Resumes {set;}

						ContactMechanism HeadQuarter {set;}

						ElectronicAddress PersonalEmailAddress {set;}

						TelecommunicationsNumber CellPhoneNumber {set;}

						TelecommunicationsNumber BillingInquiriesPhone {set;}

						global::System.String PartyName {set;}

						ContactMechanism OrderAddress {set;}

						ElectronicAddress InternetAddress {set;}

						Media Contents {set;}

						CreditCard CreditCards {set;}

						PostalAddress ShippingAddress {set;}

						OrganisationContactRelationship CurrentOrganisationContactRelationships {set;}

						global::System.Decimal OpenOrderAmount {set;}

						TelecommunicationsNumber GeneralFaxNumber {set;}

						PaymentMethod DefaultPaymentMethod {set;}

						PartyContactMechanism CurrentPartyContactMechanisms {set;}

						TelecommunicationsNumber GeneralPhoneNumber {set;}

						Currency PreferredCurrency {set;}

						VatRegime VatRegime {set;}

		}
		public interface PartyClassification  : AccessControlledObject 
		{
						global::System.String Name {set;}

		}
		public interface PartyRelationship  : Period, Commentable, AccessControlledObject, Deletable 
		{
						PartyRelationshipStatus PartyRelationshipStatus {set;}

						Agreement RelationshipAgreements {set;}

						Priority PartyRelationshipPriority {set;}

						global::System.Decimal? SimpleMovingAverage {set;}

						CommunicationEvent CommunicationEvents {set;}

						Party Parties {set;}

		}
		public interface Payment  : AccessControlledObject, Commentable, UniquelyIdentifiable 
		{
						global::System.Decimal Amount {set;}

						PaymentMethod PaymentMethod {set;}

						global::System.DateTime EffectiveDate {set;}

						Party SendingParty {set;}

						PaymentApplication PaymentApplications {set;}

						global::System.String ReferenceNumber {set;}

						Party ReceivingParty {set;}

		}
		public interface PaymentMethod  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.Decimal? BalanceLimit {set;}

						global::System.Decimal CurrentBalance {set;}

						Journal Journal {set;}

						global::System.String Description {set;}

						OrganisationGlAccount GlPaymentInTransit {set;}

						global::System.String Remarks {set;}

						OrganisationGlAccount GeneralLedgerAccount {set;}

						SupplierRelationship Creditor {set;}

						global::System.Boolean IsActive {set;}

		}
		public interface Period  : Object 
		{
						global::System.DateTime FromDate {set;}

						global::System.DateTime? ThroughDate {set;}

		}
		public interface PersonClassification  : PartyClassification 
		{
		}
		public interface Priceable  : Commentable, Transitional 
		{
						global::System.Decimal? TotalDiscountAsPercentage {set;}

						DiscountAdjustment DiscountAdjustment {set;}

						global::System.Decimal UnitVat {set;}

						global::System.Decimal TotalVatCustomerCurrency {set;}

						VatRegime VatRegime {set;}

						global::System.Decimal TotalVat {set;}

						global::System.Decimal UnitSurcharge {set;}

						global::System.Decimal UnitDiscount {set;}

						global::System.Decimal TotalExVatCustomerCurrency {set;}

						VatRate DerivedVatRate {set;}

						global::System.Decimal? ActualUnitPrice {set;}

						global::System.Decimal TotalIncVatCustomerCurrency {set;}

						global::System.Decimal UnitBasePrice {set;}

						global::System.Decimal CalculatedUnitPrice {set;}

						global::System.Decimal TotalSurchargeCustomerCurrency {set;}

						global::System.Decimal TotalIncVat {set;}

						global::System.Decimal? TotalSurchargeAsPercentage {set;}

						global::System.Decimal TotalDiscountCustomerCurrency {set;}

						global::System.Decimal TotalDiscount {set;}

						global::System.Decimal TotalSurcharge {set;}

						VatRegime AssignedVatRegime {set;}

						global::System.Decimal TotalBasePrice {set;}

						global::System.Decimal TotalExVat {set;}

						global::System.Decimal TotalBasePriceCustomerCurrency {set;}

						PriceComponent CurrentPriceComponents {set;}

						SurchargeAdjustment SurchargeAdjustment {set;}

		}
		public interface PriceComponent  : Period, AccessControlledObject, Commentable 
		{
						GeographicBoundary GeographicBoundary {set;}

						global::System.Decimal? Rate {set;}

						RevenueValueBreak RevenueValueBreak {set;}

						PartyClassification PartyClassification {set;}

						OrderQuantityBreak OrderQuantityBreak {set;}

						PackageQuantityBreak PackageQuantityBreak {set;}

						Product Product {set;}

						RevenueQuantityBreak RevenueQuantityBreak {set;}

						Party SpecifiedFor {set;}

						ProductFeature ProductFeature {set;}

						AgreementPricingProgram AgreementPricingProgram {set;}

						global::System.String Description {set;}

						Currency Currency {set;}

						OrderKind OrderKind {set;}

						OrderValue OrderValue {set;}

						global::System.Decimal? Price {set;}

						ProductCategory ProductCategory {set;}

						SalesChannel SalesChannel {set;}

		}
		public interface Printable  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.String PrintContent {set;}

		}
		public interface Product  : UniquelyIdentifiable, AccessControlledObject 
		{
						ProductCharacteristicValue ProductCharacteristicValues {set;}

						ProductCategory PrimaryProductCategory {set;}

						global::System.DateTime? SupportDiscontinuationDate {set;}

						global::System.DateTime? SalesDiscontinuationDate {set;}

						LocalisedText LocalisedNames {set;}

						LocalisedText LocalisedDescriptions {set;}

						LocalisedText LocalisedComments {set;}

						global::System.String InternalComment {set;}

						global::System.String Description {set;}

						PriceComponent VirtualProductPriceComponents {set;}

						global::System.String IntrastatCode {set;}

						ProductCategory ProductCategoriesExpanded {set;}

						Product ProductComplement {set;}

						ProductFeature OptionalFeatures {set;}

						Party ManufacturedBy {set;}

						Product Variants {set;}

						global::System.String Name {set;}

						global::System.DateTime? IntroductionDate {set;}

						Document Documents {set;}

						ProductFeature StandardFeatures {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						EstimatedProductCost EstimatedProductCosts {set;}

						Product ProductObsolescences {set;}

						ProductFeature SelectableFeatures {set;}

						VatRate VatRate {set;}

						PriceComponent BasePrices {set;}

						ProductCategory ProductCategories {set;}

						InternalOrganisation SoldBy {set;}

		}
		public interface ProductAssociation  : Commentable, AccessControlledObject, Period 
		{
		}
		public interface ProductFeature  : UniquelyIdentifiable, AccessControlledObject 
		{
						EstimatedProductCost EstimatedProductCosts {set;}

						PriceComponent BasePrices {set;}

						global::System.String Description {set;}

						ProductFeature DependentFeatures {set;}

						ProductFeature IncompatibleFeatures {set;}

						VatRate VatRate {set;}

		}
		public interface Quote  : Transitional, Auditable 
		{
						global::System.DateTime? ValidFromDate {set;}

						QuoteTerm QuoteTerms {set;}

						Party Issuer {set;}

						global::System.DateTime? ValidThroughDate {set;}

						global::System.String Description {set;}

						Party Receiver {set;}

						global::System.Decimal? Amount {set;}

						global::System.DateTime? IssueDate {set;}

						QuoteItem QuoteItems {set;}

						global::System.String QuoteNumber {set;}

		}
		public interface Request  : Commentable, Auditable 
		{
						global::System.String Description {set;}

						global::System.DateTime? RequiredResponseDate {set;}

						RequestItem RequestItems {set;}

						global::System.String RequestNumber {set;}

						RespondingParty RespondingParties {set;}

						Party Originator {set;}

		}
		public interface Requirement  : Transitional, UniquelyIdentifiable 
		{
						global::System.DateTime? RequiredByDate {set;}

						Party Authorizer {set;}

						RequirementStatus RequirementStatuses {set;}

						global::System.String Reason {set;}

						Requirement Children {set;}

						Party NeededFor {set;}

						Party Originator {set;}

						RequirementObjectState CurrentObjectState {set;}

						RequirementStatus CurrentRequirementStatus {set;}

						Facility Facility {set;}

						Party ServicedBy {set;}

						global::System.Decimal? EstimatedBudget {set;}

						global::System.String Description {set;}

						global::System.Int32? Quantity {set;}

		}
		public interface Service  : Product 
		{
		}
		public interface ServiceEntry  : Commentable, AccessControlledObject 
		{
						global::System.DateTime? ThroughDateTime {set;}

						EngagementItem EngagementItem {set;}

						global::System.Boolean? IsBillable {set;}

						global::System.DateTime? FromDateTime {set;}

						global::System.String Description {set;}

						WorkEffort WorkEffort {set;}

		}
		public interface Shipment  : Printable, Transitional 
		{
						ShipmentMethod ShipmentMethod {set;}

						ContactMechanism BillToContactMechanism {set;}

						ShipmentPackage ShipmentPackages {set;}

						global::System.String ShipmentNumber {set;}

						Document Documents {set;}

						Party BillToParty {set;}

						Party ShipToParty {set;}

						ShipmentItem ShipmentItems {set;}

						InternalOrganisation BillFromInternalOrganisation {set;}

						ContactMechanism ReceiverContactMechanism {set;}

						PostalAddress ShipToAddress {set;}

						global::System.Decimal? EstimatedShipCost {set;}

						global::System.DateTime? EstimatedShipDate {set;}

						global::System.DateTime? LatestCancelDate {set;}

						Carrier Carrier {set;}

						ContactMechanism InquireAboutContactMechanism {set;}

						global::System.DateTime? EstimatedReadyDate {set;}

						PostalAddress ShipFromAddress {set;}

						ContactMechanism BillFromContactMechanism {set;}

						global::System.String HandlingInstruction {set;}

						Store Store {set;}

						Party ShipFromParty {set;}

						ShipmentRouteSegment ShipmentRouteSegments {set;}

						global::System.DateTime? EstimatedArrivalDate {set;}

		}
		public interface WorkEffort  : Transitional, UniquelyIdentifiable, Deletable, Auditable 
		{
						WorkEffortStatus CurrentWorkEffortStatus {set;}

						WorkEffort Precendencies {set;}

						Facility Facility {set;}

						Deliverable DeliverablesProduced {set;}

						global::System.DateTime? ActualStart {set;}

						WorkEffortInventoryAssignment InventoryItemsNeeded {set;}

						WorkEffort Children {set;}

						global::System.DateTime? ActualCompletion {set;}

						OrderItem OrderItemFulfillment {set;}

						WorkEffortStatus WorkEffortStatuses {set;}

						WorkEffortType WorkEffortType {set;}

						InventoryItem InventoryItemsProduced {set;}

						WorkEffortPurpose WorkEffortPurposes {set;}

						Priority Priority {set;}

						global::System.String Name {set;}

						Requirement RequirementFulfillments {set;}

						global::System.String SpecialTerms {set;}

						WorkEffort Concurrencies {set;}

						global::System.DateTime? ScheduledStart {set;}

						global::System.Decimal? ActualHours {set;}

						global::System.String Description {set;}

						WorkEffortObjectState CurrentObjectState {set;}

						global::System.DateTime? ScheduledCompletion {set;}

						global::System.Decimal? EstimatedHours {set;}

		}
		public interface AccessControl  : Deletable, AccessControlledObject 
		{
						UserGroup SubjectGroups {set;}

						User Subjects {set;}

						Role Role {set;}

						Permission EffectivePermissions {set;}

						User EffectiveUsers {set;}

		}
		public interface AsyncDerivation  : Deletable 
		{
						AsyncDerivable AsyncDerivable {set;}

		}
		public interface Counter  : UniquelyIdentifiable 
		{
						global::System.Int32 Value {set;}

		}
		public interface Country  : GeographicBoundary, CityBound 
		{
						Currency Currency {set;}

						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						LocalisedText LocalisedNames {set;}

						VatRate VatRates {set;}

						global::System.Int32? IbanLength {set;}

						global::System.Boolean? EuMemberState {set;}

						global::System.String TelephoneCode {set;}

						global::System.String IbanRegex {set;}

						VatForm VatForm {set;}

						global::System.String UriExtension {set;}

		}
		public interface Currency  : IUnitOfMeasure 
		{
						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						LocalisedText LocalisedNames {set;}

		}
		public interface Language  : AccessControlledObject 
		{
						global::System.String IsoCode {set;}

						global::System.String Name {set;}

						global::System.String NativeName {set;}

						LocalisedText LocalisedNames {set;}

		}
		public interface Locale  : AccessControlledObject 
		{
						global::System.String Name {set;}

						Language Language {set;}

						Country Country {set;}

		}
		public interface LocalisedText  : AccessControlledObject, Localised 
		{
						global::System.String Text {set;}

		}
		public interface Login  : Deletable 
		{
						global::System.String Key {set;}

						global::System.String Provider {set;}

						User User {set;}

		}
		public interface Media  : UniquelyIdentifiable, AccessControlledObject, Deletable 
		{
						global::System.Guid? Revision {set;}

						MediaContent MediaContent {set;}

						global::System.Byte[] InData {set;}

						global::System.String InDataUri {set;}

		}
		public interface MediaContent  : AccessControlledObject, Deletable 
		{
						global::System.String Type {set;}

						global::System.Byte[] Data {set;}

		}
		public interface Permission  : Deletable, AccessControlledObject 
		{
						global::System.Guid OperandTypePointer {set;}

						global::System.Guid ConcreteClassPointer {set;}

						global::System.Int32 OperationEnum {set;}

		}
		public interface Person  : User, Party, Deletable 
		{
						global::System.String FirstName {set;}

						global::System.String LastName {set;}

						global::System.String MiddleName {set;}

						Salutation Salutation {set;}

						global::System.Decimal? YTDCommission {set;}

						PersonClassification PersonClassifications {set;}

						Citizenship Citizenship {set;}

						Employment CurrentEmployment {set;}

						global::System.Decimal? LastYearsCommission {set;}

						global::System.String GivenName {set;}

						PersonalTitle Titles {set;}

						global::System.String MothersMaidenName {set;}

						global::System.DateTime? BirthDate {set;}

						global::System.Decimal? Height {set;}

						PersonTraining PersonTrainings {set;}

						GenderType Gender {set;}

						global::System.Int32? Weight {set;}

						Hobby Hobbies {set;}

						global::System.Int32? TotalYearsWorkExperience {set;}

						Passport Passports {set;}

						MaritalStatus MaritalStatus {set;}

						Image Picture {set;}

						global::System.String SocialSecurityNumber {set;}

						global::System.DateTime? DeceasedDate {set;}

		}
		public interface Role  : AccessControlledObject, UniquelyIdentifiable 
		{
						Permission Permissions {set;}

						global::System.String Name {set;}

		}
		public interface SecurityToken  : Deletable 
		{
						AccessControl AccessControls {set;}

		}
		public interface Singleton  : AccessControlledObject 
		{
						Locale DefaultLocale {set;}

						Locale Locales {set;}

						User Guest {set;}

						SecurityToken InitialSecurityToken {set;}

						SecurityToken DefaultSecurityToken {set;}

						AccessControl CreatorsAccessControl {set;}

						AccessControl GuestAccessControl {set;}

						AccessControl AdministratorsAccessControl {set;}

						PrintQueue DefaultPrintQueue {set;}

						SecurityToken AdministratorSecurityToken {set;}

						Currency DefaultCurrency {set;}

						Media NoImageAvailableImage {set;}

						InternalOrganisation DefaultInternalOrganisation {set;}

		}
		public interface UserGroup  : UniquelyIdentifiable, AccessControlledObject 
		{
						User Members {set;}

						global::System.String Name {set;}

		}
		public interface AccountAdjustment  : FinancialAccountTransaction 
		{
		}
		public interface AccountingPeriod  : Budget 
		{
						AccountingPeriod Parent {set;}

						global::System.Boolean Active {set;}

						global::System.Int32 PeriodNumber {set;}

						TimeFrequency TimeFrequency {set;}

		}
		public interface AccountingTransactionDetail  : AccessControlledObject 
		{
						AccountingTransactionDetail AssociatedWith {set;}

						OrganisationGlAccountBalance OrganisationGlAccountBalance {set;}

						global::System.Decimal Amount {set;}

						global::System.Boolean Debit {set;}

		}
		public interface AccountingTransactionNumber  : AccessControlledObject 
		{
						global::System.Int32? Number {set;}

						global::System.Int32? Year {set;}

						AccountingTransactionType AccountingTransactionType {set;}

		}
		public interface AccountingTransactionType  : Enumeration 
		{
		}
		public interface Activity  : WorkEffort 
		{
		}
		public interface ActivityUsage  : DeploymentUsage 
		{
						global::System.Decimal Quantity {set;}

						UnitOfMeasure UnitOfMeasure {set;}

		}
		public interface Addendum  : AccessControlledObject 
		{
						global::System.String Text {set;}

						global::System.DateTime? EffictiveDate {set;}

						global::System.String Description {set;}

						global::System.DateTime CreationDate {set;}

		}
		public interface AgreementExhibit  : AgreementItem 
		{
		}
		public interface AgreementPricingProgram  : AgreementItem 
		{
		}
		public interface AgreementSection  : AgreementItem 
		{
		}
		public interface Amortization  : InternalAccountingTransaction 
		{
		}
		public interface AmountDue  : AccessControlledObject 
		{
						global::System.Decimal? Amount {set;}

						PaymentMethod PaymentMethod {set;}

						global::System.DateTime? TransactionDate {set;}

						global::System.DateTime? BlockedForDunning {set;}

						global::System.Decimal? AmountVat {set;}

						BankAccount BankAccount {set;}

						global::System.DateTime? ReconciliationDate {set;}

						global::System.String InvoiceNumber {set;}

						global::System.Int32? DunningStep {set;}

						global::System.Int32? SubAccountNumber {set;}

						global::System.String TransactionNumber {set;}

						DebitCreditConstant Side {set;}

						Currency Currency {set;}

						global::System.Boolean? BlockedForPayment {set;}

						global::System.DateTime? DateLastReminder {set;}

						global::System.String YourReference {set;}

						global::System.String OurReference {set;}

						global::System.String ReconciliationNumber {set;}

						global::System.DateTime? DueDate {set;}

		}
		public interface AssetAssignmentStatus  : Enumeration 
		{
		}
		public interface AutomatedAgent  : User, Party 
		{
						global::System.String Name {set;}

						global::System.String Description {set;}

		}
		public interface Bank  : AccessControlledObject 
		{
						Media Logo {set;}

						global::System.String Bic {set;}

						global::System.String SwiftCode {set;}

						Country Country {set;}

						global::System.String Name {set;}

		}
		public interface BankAccount  : FinancialAccount 
		{
						Bank Bank {set;}

						global::System.String NameOnAccount {set;}

						ContactMechanism ContactMechanisms {set;}

						Currency Currency {set;}

						global::System.String Iban {set;}

						global::System.String Branch {set;}

						Person ContactPersons {set;}

		}
		public interface Barrel  : Container 
		{
		}
		public interface BasePrice  : Deletable, PriceComponent 
		{
		}
		public interface Benefit  : AccessControlledObject 
		{
						global::System.Decimal? EmployerPaidPercentage {set;}

						global::System.String Description {set;}

						global::System.String Name {set;}

						global::System.Decimal? AvailableTime {set;}

		}
		public interface BillingAccount  : AccessControlledObject 
		{
						global::System.String Description {set;}

						ContactMechanism ContactMechanism {set;}

		}
		public interface BillOfLading  : Document 
		{
		}
		public interface Bin  : Container 
		{
		}
		public interface Brand  : AccessControlledObject 
		{
						global::System.String Name {set;}

						ProductCategory ProductCategories {set;}

		}
		public interface BudgetItem  : AccessControlledObject 
		{
						global::System.String Purpose {set;}

						global::System.String Justification {set;}

						BudgetItem Children {set;}

						global::System.Decimal Amount {set;}

		}
		public interface BudgetObjectState  : ObjectState 
		{
		}
		public interface BudgetReview  : Commentable, AccessControlledObject 
		{
						global::System.DateTime ReviewDate {set;}

						global::System.String Description {set;}

		}
		public interface BudgetRevision  : AccessControlledObject 
		{
						global::System.DateTime RevisionDate {set;}

		}
		public interface BudgetRevisionImpact  : AccessControlledObject 
		{
						BudgetItem BudgetItem {set;}

						global::System.String Reason {set;}

						global::System.Boolean? Deleted {set;}

						global::System.Boolean? Added {set;}

						global::System.Decimal? RevisedAmount {set;}

						BudgetRevision BudgetRevision {set;}

		}
		public interface BudgetStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						BudgetObjectState BudgetObjectState {set;}

		}
		public interface Building  : Facility 
		{
		}
		public interface CapitalBudget  : Budget 
		{
		}
		public interface Capitalization  : InternalAccountingTransaction 
		{
		}
		public interface Carrier  : UniquelyIdentifiable, AccessControlledObject 
		{
						global::System.String Name {set;}

		}
		public interface Case  : Transitional, UniquelyIdentifiable 
		{
						CaseStatus CurrentCaseStatus {set;}

						CaseStatus CaseStatuses {set;}

						global::System.DateTime? StartDate {set;}

						CaseObjectState CurrentObjectState {set;}

						global::System.String Description {set;}

		}
		public interface CaseObjectState  : ObjectState 
		{
		}
		public interface CaseStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						CaseObjectState CaseObjectState {set;}

		}
		public interface Cash  : PaymentMethod 
		{
						Person PersonResponsible {set;}

		}
		public interface ChartOfAccounts  : UniquelyIdentifiable, AccessControlledObject 
		{
						global::System.String Name {set;}

						GeneralLedgerAccount GeneralLedgerAccounts {set;}

		}
		public interface Citizenship  : AccessControlledObject 
		{
						Passport Passports {set;}

						Country Country {set;}

		}
		public interface City  : GeographicBoundary, CountryBound 
		{
						global::System.String Name {set;}

						State State {set;}

		}
		public interface ClientAgreement  : Agreement 
		{
		}
		public interface ClientRelationship  : PartyRelationship 
		{
						Party Client {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface Colour  : Enumeration, ProductFeature 
		{
		}
		public interface CommunicationEventObjectState  : ObjectState 
		{
		}
		public interface CommunicationEventPurpose  : Enumeration 
		{
		}
		public interface CommunicationEventStatus  : Deletable, AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						CommunicationEventObjectState CommunicationEventObjectState {set;}

		}
		public interface ConstraintSpecification  : PartSpecification 
		{
		}
		public interface ContactMechanismPurpose  : Enumeration 
		{
		}
		public interface CostCenter  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.String Description {set;}

						OrganisationGlAccount InternalTransferGlAccount {set;}

						CostCenterCategory CostCenterCategories {set;}

						OrganisationGlAccount RedistributedCostsGlAccount {set;}

						global::System.String Name {set;}

						global::System.Boolean? Active {set;}

						global::System.Boolean? UseGlAccountOfBooking {set;}

		}
		public interface CostCenterCategory  : UniquelyIdentifiable 
		{
						CostCenterCategory Parent {set;}

						CostCenterCategory Ancestors {set;}

						CostCenterCategory Children {set;}

						global::System.String Description {set;}

		}
		public interface CostCenterSplitMethod  : Enumeration 
		{
		}
		public interface CostOfGoodsSoldMethod  : Enumeration 
		{
		}
		public interface County  : GeographicBoundary, CityBound 
		{
						global::System.String Name {set;}

						State State {set;}

		}
		public interface CreditCard  : FinancialAccount 
		{
						global::System.String NameOnCard {set;}

						CreditCardCompany CreditCardCompany {set;}

						global::System.Int32 ExpirationYear {set;}

						global::System.Int32 ExpirationMonth {set;}

						global::System.String CardNumber {set;}

		}
		public interface CreditCardCompany  : AccessControlledObject 
		{
						global::System.String Name {set;}

		}
		public interface CreditLine  : ExternalAccountingTransaction 
		{
		}
		public interface CustomEngagementItem  : EngagementItem 
		{
						global::System.String DescriptionOfWork {set;}

						global::System.Decimal? AgreedUponPrice {set;}

		}
		public interface CustomerRelationship  : PartyRelationship 
		{
						global::System.Boolean? BlockedForDunning {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal AmountOverDue {set;}

						Party Customer {set;}

						DunningType DunningType {set;}

						global::System.Decimal AmountDue {set;}

						global::System.Decimal YTDRevenue {set;}

						global::System.DateTime? LastReminderDate {set;}

						global::System.Decimal? CreditLimit {set;}

						global::System.Int32 SubAccountNumber {set;}

						global::System.Decimal LastYearsRevenue {set;}

		}
		public interface CustomerRequirement  : Requirement 
		{
		}
		public interface CustomerReturn  : Shipment 
		{
						CustomerReturnStatus CurrentShipmentStatus {set;}

						CustomerReturnStatus ShipmentStatuses {set;}

						CustomerReturnObjectState CurrentObjectState {set;}

		}
		public interface CustomerReturnObjectState  : ObjectState 
		{
		}
		public interface CustomerReturnStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						ShipmentReceipt ShipmentReceipt {set;}

						CustomerReturnObjectState CustomerReturnObjectState {set;}

		}
		public interface CustomerShipment  : Deletable, Shipment 
		{
						CustomerShipmentStatus CurrentShipmentStatus {set;}

						global::System.Boolean ReleasedManually {set;}

						CustomerShipmentObjectState CurrentObjectState {set;}

						CustomerShipmentStatus ShipmentStatuses {set;}

						PaymentMethod PaymentMethod {set;}

						global::System.Boolean WithoutCharges {set;}

						global::System.Boolean HeldManually {set;}

						global::System.Decimal ShipmentValue {set;}

		}
		public interface CustomerShipmentObjectState  : ObjectState 
		{
		}
		public interface CustomerShipmentStatus  : AccessControlledObject 
		{
						CustomerShipmentObjectState CustomerShipmentObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface DebitCreditConstant  : Enumeration 
		{
		}
		public interface Deduction  : AccessControlledObject 
		{
						DeductionType DeductionType {set;}

						global::System.Decimal Amount {set;}

		}
		public interface DeductionType  : Enumeration 
		{
		}
		public interface Deliverable  : AccessControlledObject 
		{
						global::System.String Name {set;}

						DeliverableType DeliverableType {set;}

		}
		public interface DeliverableBasedService  : Service 
		{
		}
		public interface DeliverableOrderItem  : EngagementItem 
		{
						global::System.Decimal? AgreedUponPrice {set;}

		}
		public interface DeliverableTurnover  : ServiceEntry 
		{
						global::System.Decimal Amount {set;}

		}
		public interface DeliverableType  : Enumeration 
		{
		}
		public interface Deployment  : AccessControlledObject, Period 
		{
						Good ProductOffering {set;}

						DeploymentUsage DeploymentUsage {set;}

						SerializedInventoryItem SerializedInventoryItem {set;}

		}
		public interface Deposit  : FinancialAccountTransaction 
		{
						Receipt Receipts {set;}

		}
		public interface Depreciation  : InternalAccountingTransaction 
		{
						FixedAsset FixedAsset {set;}

		}
		public interface DepreciationMethod  : AccessControlledObject 
		{
						global::System.String Formula {set;}

						global::System.String Description {set;}

		}
		public interface DesiredProductFeature  : AccessControlledObject 
		{
						global::System.Boolean Required {set;}

						ProductFeature ProductFeature {set;}

		}
		public interface Dimension  : ProductFeature 
		{
						global::System.Decimal? Unit {set;}

						UnitOfMeasure UnitOfMeasure {set;}

		}
		public interface Disbursement  : Payment 
		{
		}
		public interface DisbursementAccountingTransaction  : ExternalAccountingTransaction 
		{
						Disbursement Disbursement {set;}

		}
		public interface DiscountAdjustment  : OrderAdjustment 
		{
		}
		public interface DiscountComponent  : PriceComponent 
		{
						global::System.Decimal? Percentage {set;}

		}
		public interface DistributionChannelRelationship  : PartyRelationship 
		{
						InternalOrganisation InternalOrganisation {set;}

						Organisation Distributor {set;}

		}
		public interface DropShipment  : Shipment 
		{
						DropShipmentStatus ShipmentStatuses {set;}

						DropShipmentStatus CurrentShipmentStatus {set;}

						DropShipmentObjectState CurrentObjectState {set;}

		}
		public interface DropShipmentObjectState  : ObjectState 
		{
		}
		public interface DropShipmentStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						DropShipmentObjectState DropShipmentObjectState {set;}

		}
		public interface DunningType  : Enumeration 
		{
		}
		public interface EmailAddress  : ElectronicAddress 
		{
		}
		public interface EmailCommunication  : CommunicationEvent 
		{
						EmailAddress Originator {set;}

						EmailAddress Addressees {set;}

						EmailAddress CarbonCopies {set;}

						EmailAddress BlindCopies {set;}

						EmailTemplate EmailTemplate {set;}

		}
		public interface EmailTemplate  : AccessControlledObject 
		{
						global::System.String Description {set;}

						global::System.String BodyTemplate {set;}

						global::System.String SubjectTemplate {set;}

		}
		public interface Employment  : PartyRelationship 
		{
						InternalOrganisation Employer {set;}

						Person Employee {set;}

						PayrollPreference PayrollPreferences {set;}

						EmploymentTerminationReason EmploymentTerminationReason {set;}

						EmploymentTermination EmploymentTermination {set;}

		}
		public interface EmploymentAgreement  : Agreement 
		{
		}
		public interface EmploymentApplication  : AccessControlledObject 
		{
						global::System.DateTime ApplicationDate {set;}

						Position Position {set;}

						EmploymentApplicationStatus EmploymentApplicationStatus {set;}

						Person Person {set;}

						EmploymentApplicationSource EmploymentApplicationSource {set;}

		}
		public interface EmploymentApplicationSource  : Enumeration 
		{
		}
		public interface EmploymentApplicationStatus  : Enumeration 
		{
		}
		public interface EmploymentTermination  : Enumeration 
		{
		}
		public interface EmploymentTerminationReason  : Enumeration 
		{
		}
		public interface Engagement  : AccessControlledObject 
		{
						Agreement Agreement {set;}

						ContactMechanism PlacingContactMechanism {set;}

						global::System.Decimal? MaximumAmount {set;}

						ContactMechanism BillToContactMechanism {set;}

						global::System.String Description {set;}

						Party BillToParty {set;}

						Party PlacingParty {set;}

						InternalOrganisation TakenViaInternalOrganisation {set;}

						global::System.DateTime? StartDate {set;}

						ContactMechanism TakenViaContactMechanism {set;}

						global::System.Decimal? EstimatedAmount {set;}

						global::System.DateTime? EndDate {set;}

						global::System.DateTime? ContractDate {set;}

						EngagementItem EngagementItems {set;}

						global::System.String ClientPurchaseOrderNumber {set;}

						OrganisationContactRelationship TakenViaOrganisationContactRelationship {set;}

		}
		public interface EngagementRate  : Period, AccessControlledObject 
		{
						global::System.Decimal BillingRate {set;}

						RatingType RatingType {set;}

						global::System.Decimal? Cost {set;}

						PriceComponent GoverningPriceComponents {set;}

						global::System.String ChangeReason {set;}

						UnitOfMeasure UnitOfMeasure {set;}

		}
		public interface EngineeringBom  : PartBillOfMaterial 
		{
		}
		public interface EngineeringChange  : Transitional 
		{
						Person Requestor {set;}

						Person Authorizer {set;}

						global::System.String Description {set;}

						Person Designer {set;}

						PartSpecification PartSpecifications {set;}

						PartBillOfMaterial PartBillOfMaterials {set;}

						EngineeringChangeObjectState CurrentObjectState {set;}

						EngineeringChangeStatus EngineeringChangeStatuses {set;}

						Person Tester {set;}

						EngineeringChangeStatus CurrentEngineeringChangeStatus {set;}

		}
		public interface EngineeringChangeObjectState  : ObjectState 
		{
		}
		public interface EngineeringChangeStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						EngineeringChangeObjectState EngineeringChangeObjectState {set;}

		}
		public interface EngineeringDocument  : Document 
		{
		}
		public interface Equipment  : FixedAsset 
		{
		}
		public interface EstimatedLaborCost  : EstimatedProductCost 
		{
		}
		public interface EstimatedMaterialCost  : EstimatedProductCost 
		{
		}
		public interface EstimatedOtherCost  : EstimatedProductCost 
		{
		}
		public interface EuSalesListType  : Enumeration 
		{
		}
		public interface Event  : Object 
		{
						global::System.Boolean? RegistrationRequired {set;}

						global::System.String Link {set;}

						global::System.String Location {set;}

						global::System.String Text {set;}

						global::System.String AnnouncementText {set;}

						global::System.DateTime? From {set;}

						Locale Locale {set;}

						global::System.String Title {set;}

						Media Photo {set;}

						global::System.Boolean? Announce {set;}

						global::System.DateTime? To {set;}

		}
		public interface EventRegistration  : Object 
		{
						Person Person {set;}

						Event Event {set;}

						global::System.DateTime? AllorsDateTime {set;}

		}
		public interface ExpenseEntry  : ServiceEntry 
		{
						global::System.Decimal Amount {set;}

		}
		public interface ExportDocument  : Document 
		{
		}
		public interface FaceToFaceCommunication  : CommunicationEvent 
		{
						Party Participants {set;}

						global::System.String Location {set;}

		}
		public interface FaxCommunication  : CommunicationEvent 
		{
						Party Originator {set;}

						Party Receiver {set;}

						TelecommunicationsNumber OutgoingFaxNumber {set;}

		}
		public interface Fee  : OrderAdjustment 
		{
		}
		public interface FinancialAccountAdjustment  : FinancialAccountTransaction 
		{
		}
		public interface FinancialTerm  : AgreementTerm 
		{
		}
		public interface FinishedGood  : Part 
		{
		}
		public interface FiscalYearInvoiceNumber  : Object 
		{
						global::System.Int32 NextSalesInvoiceNumber {set;}

						global::System.Int32 FiscalYear {set;}

		}
		public interface Floor  : Facility 
		{
		}
		public interface GenderType  : Enumeration 
		{
		}
		public interface GeneralLedgerAccount  : UniquelyIdentifiable, AccessControlledObject 
		{
						Product DefaultCostUnit {set;}

						CostCenter DefaultCostCenter {set;}

						global::System.String Description {set;}

						GeneralLedgerAccountType GeneralLedgerAccountType {set;}

						global::System.Boolean CashAccount {set;}

						global::System.Boolean CostCenterAccount {set;}

						DebitCreditConstant Side {set;}

						global::System.Boolean BalanceSheetAccount {set;}

						global::System.Boolean ReconciliationAccount {set;}

						global::System.String Name {set;}

						global::System.Boolean CostCenterRequired {set;}

						global::System.Boolean CostUnitRequired {set;}

						GeneralLedgerAccountGroup GeneralLedgerAccountGroup {set;}

						CostCenter CostCentersAllowed {set;}

						global::System.Boolean CostUnitAccount {set;}

						global::System.String AccountNumber {set;}

						Product CostUnitsAllowed {set;}

						global::System.Boolean Protected {set;}

		}
		public interface GeneralLedgerAccountGroup  : AccessControlledObject 
		{
						GeneralLedgerAccountGroup Parent {set;}

						global::System.String Description {set;}

		}
		public interface GeneralLedgerAccountType  : AccessControlledObject 
		{
						global::System.String Description {set;}

		}
		public interface GlBudgetAllocation  : AccessControlledObject 
		{
						GeneralLedgerAccount GeneralLedgerAccount {set;}

						BudgetItem BudgetItem {set;}

						global::System.Decimal AllocationPercentage {set;}

		}
		public interface Good  : Product, Deletable 
		{
						ProductType ProductType {set;}

						global::System.Decimal AvailableToPromise {set;}

						Media Thumbnail {set;}

						InventoryItemKind InventoryItemKind {set;}

						global::System.String BarCode {set;}

						FinishedGood FinishedGood {set;}

						global::System.String Sku {set;}

						global::System.String ArticleNumber {set;}

						global::System.String ManufacturerId {set;}

						Product ProductSubstitutions {set;}

						Product ProductIncompatibilities {set;}

						Media Photo {set;}

		}
		public interface GoodOrderItem  : EngagementItem 
		{
						global::System.Decimal? Price {set;}

						global::System.Int32? Quantity {set;}

		}
		public interface HazardousMaterialsDocument  : Document 
		{
		}
		public interface Hobby  : Enumeration 
		{
		}
		public interface Image  : Deletable 
		{
						Media Original {set;}

						Media Responsive {set;}

						global::System.String OriginalFilename {set;}

						Media Thumbnail {set;}

		}
		public interface Incentive  : AgreementTerm 
		{
		}
		public interface IndustryClassification  : OrganisationClassification 
		{
		}
		public interface InternalOrganisation  : Party 
		{
						global::System.String PurchaseOrderNumberPrefix {set;}

						global::System.String TransactionReferenceNumber {set;}

						JournalEntryNumber JournalEntryNumbers {set;}

						Country EuListingState {set;}

						Counter PurchaseInvoiceCounter {set;}

						AccountingPeriod ActualAccountingPeriod {set;}

						InvoiceSequence InvoiceSequence {set;}

						PaymentMethod ActivePaymentMethods {set;}

						global::System.Decimal? MaximumAllowedPaymentDifference {set;}

						Image LogoImage {set;}

						CostCenterSplitMethod CostCenterSplitMethod {set;}

						Counter PurchaseOrderCounter {set;}

						LegalForm LegalForm {set;}

						AccountingPeriod AccountingPeriods {set;}

						GeneralLedgerAccount SalesPaymentDifferencesAccount {set;}

						global::System.String Name {set;}

						global::System.String PurchaseTransactionReferenceNumber {set;}

						global::System.Int32 FiscalYearStartMonth {set;}

						CostOfGoodsSoldMethod CostOfGoodsSoldMethod {set;}

						Role EmployeeRoles {set;}

						global::System.Boolean? VatDeactivated {set;}

						global::System.Int32 FiscalYearStartDay {set;}

						GeneralLedgerAccount GeneralLedgerAccounts {set;}

						Counter AccountingTransactionCounter {set;}

						Counter IncomingShipmentCounter {set;}

						GeneralLedgerAccount RetainedEarningsAccount {set;}

						Party Customers {set;}

						global::System.String PurchaseInvoiceNumberPrefix {set;}

						GeneralLedgerAccount SalesPaymentDiscountDifferencesAccount {set;}

						Counter SubAccountCounter {set;}

						AccountingTransactionNumber AccountingTransactionNumbers {set;}

						global::System.String TransactionReferenceNumberPrefix {set;}

						Counter QuoteCounter {set;}

						Currency PreviousCurrency {set;}

						Person Employees {set;}

						GeneralLedgerAccount PurchasePaymentDifferencesAccount {set;}

						GeneralLedgerAccount SuspenceAccount {set;}

						GeneralLedgerAccount NetIncomeAccount {set;}

						global::System.Boolean DoAccounting {set;}

						Facility DefaultFacility {set;}

						GeneralLedgerAccount PurchasePaymentDiscountDifferencesAccount {set;}

						Party Suppliers {set;}

						global::System.String QuoteNumberPrefix {set;}

						global::System.String PurchaseTransactionReferenceNumberPrefix {set;}

						global::System.String TaxNumber {set;}

						GeneralLedgerAccount CalculationDifferencesAccount {set;}

						PaymentMethod PaymentMethods {set;}

						global::System.String IncomingShipmentNumberPrefix {set;}

		}
		public interface InternalOrganisationAccountingPreference  : AccessControlledObject 
		{
						GeneralLedgerAccount GeneralLedgerAccount {set;}

						InventoryItemKind InventoryItemKind {set;}

						PaymentMethod PaymentMethod {set;}

						Receipt Receipt {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface InternalOrganisationRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Int32 Month {set;}

						global::System.Int32 Year {set;}

						global::System.Decimal Revenue {set;}

						Currency Currency {set;}

						global::System.String PartyName {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface InternalOrganisationRevenueHistory  : AccessControlledObject 
		{
						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal? AllorsDecimal {set;}

						Currency Currency {set;}

						global::System.Decimal? Revenue {set;}

		}
		public interface InternalRequirement  : Requirement 
		{
		}
		public interface InventoryItemKind  : Enumeration 
		{
		}
		public interface InventoryItemVariance  : AccessControlledObject, Commentable 
		{
						global::System.Int32 Quantity {set;}

						ItemVarianceAccountingTransaction ItemVarianceAccountingTransaction {set;}

						global::System.DateTime? InventoryDate {set;}

						VarianceReason Reason {set;}

		}
		public interface InvestmentAccount  : FinancialAccount 
		{
						global::System.String Name {set;}

		}
		public interface InvoiceSequence  : Enumeration 
		{
		}
		public interface InvoiceTerm  : AgreementTerm 
		{
		}
		public interface InvoiceVatRateItem  : AccessControlledObject 
		{
						global::System.Decimal? BaseAmount {set;}

						VatRate VatRates {set;}

						global::System.Decimal? VatAmount {set;}

		}
		public interface ItemIssuance  : Deletable, AccessControlledObject 
		{
						global::System.DateTime? IssuanceDateTime {set;}

						InventoryItem InventoryItem {set;}

						global::System.Decimal Quantity {set;}

						ShipmentItem ShipmentItem {set;}

						PickListItem PickListItem {set;}

		}
		public interface ItemVarianceAccountingTransaction  : AccountingTransaction 
		{
		}
		public interface Journal  : AccessControlledObject 
		{
						global::System.Boolean UseAsDefault {set;}

						OrganisationGlAccount GlPaymentInTransit {set;}

						JournalType JournalType {set;}

						global::System.String Description {set;}

						global::System.Boolean BlockUnpaidTransactions {set;}

						OrganisationGlAccount ContraAccount {set;}

						InternalOrganisation InternalOrganisation {set;}

						JournalType PreviousJournalType {set;}

						OrganisationGlAccount PreviousContraAccount {set;}

						JournalEntry JournalEntries {set;}

						global::System.Boolean CloseWhenInBalance {set;}

		}
		public interface JournalEntry  : Transitional 
		{
						global::System.String Description {set;}

						global::System.Int32? EntryNumber {set;}

						global::System.DateTime? EntryDate {set;}

						global::System.DateTime? JournalDate {set;}

						JournalEntryDetail JournalEntryDetails {set;}

		}
		public interface JournalEntryDetail  : AccessControlledObject 
		{
						OrganisationGlAccount GeneralLedgerAccount {set;}

						global::System.Decimal? Amount {set;}

						global::System.Boolean? Debit {set;}

		}
		public interface JournalEntryNumber  : AccessControlledObject 
		{
						JournalType JournalType {set;}

						global::System.Int32? Number {set;}

						global::System.Int32? Year {set;}

		}
		public interface JournalType  : Enumeration 
		{
		}
		public interface LegalForm  : AccessControlledObject 
		{
						global::System.String Description {set;}

		}
		public interface LegalTerm  : AgreementTerm 
		{
		}
		public interface LetterCorrespondence  : CommunicationEvent 
		{
						PostalAddress PostalAddresses {set;}

						Party Originator {set;}

						Party Receivers {set;}

		}
		public interface Lot  : AccessControlledObject 
		{
						global::System.DateTime? ExpirationDate {set;}

						global::System.Int32? Quantity {set;}

						global::System.String LotNumber {set;}

		}
		public interface Maintenance  : WorkEffort 
		{
		}
		public interface Manifest  : Document 
		{
		}
		public interface ManufacturerSuggestedRetailPrice  : PriceComponent 
		{
		}
		public interface ManufacturingBom  : PartBillOfMaterial 
		{
		}
		public interface ManufacturingConfiguration  : InventoryItemConfiguration 
		{
		}
		public interface MaritalStatus  : Enumeration 
		{
		}
		public interface MarketingMaterial  : Document 
		{
		}
		public interface MarketingPackage  : ProductAssociation 
		{
						global::System.String Instruction {set;}

						Product ProductsUsedIn {set;}

						Product Product {set;}

						global::System.String Description {set;}

						global::System.Int32? QuantityUsed {set;}

		}
		public interface MaterialsUsage  : ServiceEntry 
		{
						global::System.Decimal Amount {set;}

		}
		public interface Model  : ProductFeature, Enumeration 
		{
		}
		public interface NeededSkill  : AccessControlledObject 
		{
						SkillLevel SkillLevel {set;}

						global::System.Decimal? YearsExperience {set;}

						Skill Skill {set;}

		}
		public interface NewsItem  : AccessControlledObject 
		{
						global::System.Boolean? IsPublished {set;}

						global::System.String Title {set;}

						global::System.Int32? DisplayOrder {set;}

						Locale Locale {set;}

						global::System.String LongText {set;}

						global::System.String Text {set;}

						global::System.DateTime? Date {set;}

						global::System.Boolean? Announcement {set;}

		}
		public interface NonSerializedInventoryItem  : InventoryItem 
		{
						NonSerializedInventoryItemObjectState CurrentObjectState {set;}

						global::System.Decimal QuantityCommittedOut {set;}

						NonSerializedInventoryItemStatus NonSerializedInventoryItemStatuses {set;}

						NonSerializedInventoryItemStatus CurrentInventoryItemStatus {set;}

						global::System.Decimal QuantityOnHand {set;}

						global::System.Decimal PreviousQuantityOnHand {set;}

						global::System.Decimal AvailableToPromise {set;}

						global::System.Decimal QuantityExpectedIn {set;}

		}
		public interface NonSerializedInventoryItemObjectState  : ObjectState 
		{
		}
		public interface NonSerializedInventoryItemStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						NonSerializedInventoryItemObjectState NonSerializedInventoryItemObjectState {set;}

		}
		public interface Note  : ExternalAccountingTransaction 
		{
		}
		public interface Obligation  : ExternalAccountingTransaction 
		{
		}
		public interface Office  : Facility 
		{
		}
		public interface OneTimeCharge  : PriceComponent 
		{
		}
		public interface OperatingBudget  : Budget 
		{
		}
		public interface OperatingCondition  : PartSpecification 
		{
		}
		public interface OrderItemBilling  : Object 
		{
						OrderItem OrderItem {set;}

						SalesInvoiceItem SalesInvoiceItem {set;}

						global::System.Decimal Amount {set;}

						global::System.Decimal? Quantity {set;}

		}
		public interface OrderKind  : UniquelyIdentifiable, AccessControlledObject 
		{
						global::System.String Description {set;}

						global::System.Boolean ScheduleManually {set;}

		}
		public interface OrderQuantityBreak  : AccessControlledObject 
		{
						global::System.Decimal? ThroughAmount {set;}

						global::System.Decimal? FromAmount {set;}

		}
		public interface OrderRequirementCommitment  : AccessControlledObject 
		{
						global::System.Int32 Quantity {set;}

						OrderItem OrderItem {set;}

						Requirement Requirement {set;}

		}
		public interface OrderShipment  : Deletable 
		{
						SalesOrderItem SalesOrderItem {set;}

						global::System.Boolean Picked {set;}

						ShipmentItem ShipmentItem {set;}

						global::System.Decimal Quantity {set;}

						PurchaseOrderItem PurchaseOrderItem {set;}

		}
		public interface OrderTerm  : AccessControlledObject 
		{
						global::System.String TermValue {set;}

						TermType TermType {set;}

		}
		public interface OrderValue  : AccessControlledObject 
		{
						global::System.Decimal? ThroughAmount {set;}

						global::System.Decimal? FromAmount {set;}

		}
		public interface Ordinal  : Enumeration 
		{
		}
		public interface Organisation  : Party, Deletable 
		{
						SecurityToken ContactsSecurityToken {set;}

						AccessControl ContactsAccessControl {set;}

						UserGroup OwnerUserGroup {set;}

						LegalForm LegalForm {set;}

						global::System.String Name {set;}

						UserGroup ContactsUserGroup {set;}

						Image LogoImage {set;}

						global::System.String TaxNumber {set;}

						IndustryClassification IndustryClassification {set;}

						OrganisationClassification OrganisationClassifications {set;}

		}
		public interface OrganisationContactKind  : AccessControlledObject, UniquelyIdentifiable 
		{
						global::System.String Description {set;}

		}
		public interface OrganisationContactRelationship  : PartyRelationship 
		{
						Person Contact {set;}

						Organisation Organisation {set;}

						OrganisationContactKind ContactKinds {set;}

		}
		public interface OrganisationGlAccount  : AccessControlledObject, Period 
		{
						Product Product {set;}

						OrganisationGlAccount Parent {set;}

						Party Party {set;}

						global::System.Boolean HasBankStatementTransactions {set;}

						ProductCategory ProductCategory {set;}

						InternalOrganisation InternalOrganisation {set;}

						GeneralLedgerAccount GeneralLedgerAccount {set;}

		}
		public interface OrganisationGlAccountBalance  : AccessControlledObject 
		{
						OrganisationGlAccount OrganisationGlAccount {set;}

						global::System.Decimal Amount {set;}

						AccountingPeriod AccountingPeriod {set;}

		}
		public interface OrganisationRollUp  : PartyRelationship 
		{
						Organisation Parent {set;}

						OrganisationUnit RollupKind {set;}

						Organisation Child {set;}

		}
		public interface OrganisationUnit  : Enumeration 
		{
		}
		public interface OwnBankAccount  : PaymentMethod, FinancialAccount 
		{
						BankAccount BankAccount {set;}

		}
		public interface OwnCreditCard  : PaymentMethod, FinancialAccount 
		{
						Person Owner {set;}

						CreditCard CreditCard {set;}

		}
		public interface Package  : UniquelyIdentifiable, AccessControlledObject 
		{
						global::System.String Name {set;}

		}
		public interface PackageQuantityBreak  : AccessControlledObject 
		{
						global::System.Int32? From {set;}

						global::System.Int32? Through {set;}

		}
		public interface PackageRevenue  : Deletable, AccessControlledObject 
		{
						global::System.Decimal Revenue {set;}

						global::System.Int32 Year {set;}

						global::System.Int32 Month {set;}

						Currency Currency {set;}

						global::System.String PackageName {set;}

						InternalOrganisation InternalOrganisation {set;}

						Package Package {set;}

		}
		public interface PackageRevenueHistory  : AccessControlledObject 
		{
						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal? Revenue {set;}

						Package Package {set;}

						Currency Currency {set;}

		}
		public interface PackagingContent  : AccessControlledObject 
		{
						ShipmentItem ShipmentItem {set;}

						global::System.Decimal Quantity {set;}

		}
		public interface PackagingSlip  : Document 
		{
		}
		public interface PartBillOfMaterialSubstitute  : Period, AccessControlledObject, Commentable 
		{
						PartBillOfMaterial SubstitutionPartBillOfMaterial {set;}

						Ordinal Preference {set;}

						global::System.Int32? Quantity {set;}

						PartBillOfMaterial PartBillOfMaterial {set;}

		}
		public interface Partnership  : PartyRelationship 
		{
						InternalOrganisation InternalOrganisation {set;}

						Organisation Partner {set;}

		}
		public interface PartRevision  : Period, AccessControlledObject 
		{
						global::System.String Reason {set;}

						Part SupersededByPart {set;}

						Part Part {set;}

		}
		public interface PartSpecificationObjectState  : ObjectState 
		{
		}
		public interface PartSpecificationStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						PartSpecificationObjectState PartSpecificationObjectState {set;}

		}
		public interface PartSubstitute  : Commentable, AccessControlledObject 
		{
						Part SubstitutionPart {set;}

						Ordinal Preference {set;}

						global::System.DateTime? FromDate {set;}

						global::System.Int32 Quantity {set;}

						Part Part {set;}

		}
		public interface PartyBenefit  : AccessControlledObject 
		{
						TimeFrequency TimeFrequency {set;}

						global::System.Decimal? Cost {set;}

						global::System.Decimal? ActualEmployerPaidPercentage {set;}

						Benefit Benefit {set;}

						global::System.Decimal? ActualAvailableTime {set;}

						Employment Employment {set;}

		}
		public interface PartyContactMechanism  : Commentable, Auditable, Period, Deletable 
		{
						ContactMechanismPurpose ContactPurpose {set;}

						ContactMechanism ContactMechanism {set;}

						global::System.Boolean UseAsDefault {set;}

						global::System.Boolean? NonSolicitationIndicator {set;}

		}
		public interface PartyFixedAssetAssignment  : AccessControlledObject, Period, Commentable 
		{
						FixedAsset FixedAsset {set;}

						Party Party {set;}

						AssetAssignmentStatus AssetAssignmentStatus {set;}

						global::System.Decimal? AllocatedCost {set;}

		}
		public interface PartyPackageRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Int32 Month {set;}

						Package Package {set;}

						Party Party {set;}

						global::System.Decimal Revenue {set;}

						global::System.Int32 Year {set;}

						global::System.String PartyPackageName {set;}

						Currency Currency {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface PartyPackageRevenueHistory  : AccessControlledObject 
		{
						Package Package {set;}

						InternalOrganisation InternalOrganisation {set;}

						Currency Currency {set;}

						Party Party {set;}

						global::System.Decimal? Revenue {set;}

		}
		public interface PartyProductCategoryRevenue  : AccessControlledObject, Deletable 
		{
						Party Party {set;}

						global::System.Decimal Revenue {set;}

						global::System.Int32 Month {set;}

						Currency Currency {set;}

						global::System.Int32 Year {set;}

						global::System.String PartyProductCategoryName {set;}

						InternalOrganisation InternalOrganisation {set;}

						ProductCategory ProductCategory {set;}

						global::System.Decimal Quantity {set;}

		}
		public interface PartyProductCategoryRevenueHistory  : AccessControlledObject 
		{
						ProductCategory ProductCategory {set;}

						Party Party {set;}

						global::System.Decimal Quantity {set;}

						global::System.Decimal Revenue {set;}

						InternalOrganisation InternalOrganisation {set;}

						Currency Currency {set;}

		}
		public interface PartyProductRevenue  : Deletable, AccessControlledObject 
		{
						global::System.Decimal Revenue {set;}

						global::System.Int32 Month {set;}

						global::System.Int32 Year {set;}

						global::System.String PartyProductName {set;}

						global::System.Decimal Quantity {set;}

						Currency Currency {set;}

						Party Party {set;}

						InternalOrganisation InternalOrganisation {set;}

						Product Product {set;}

		}
		public interface PartyProductRevenueHistory  : AccessControlledObject 
		{
						global::System.Decimal? Revenue {set;}

						Party Party {set;}

						Product Product {set;}

						global::System.Decimal? Quantity {set;}

						InternalOrganisation InternalOrganisation {set;}

						Currency Currency {set;}

		}
		public interface PartyRelationshipStatus  : Enumeration 
		{
		}
		public interface PartyRevenue  : AccessControlledObject, Deletable 
		{
						Currency Currency {set;}

						global::System.String PartyName {set;}

						global::System.Int32 Month {set;}

						Party Party {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.Int32 Year {set;}

						global::System.Decimal Revenue {set;}

		}
		public interface PartyRevenueHistory  : AccessControlledObject 
		{
						Currency Currency {set;}

						global::System.Decimal Revenue {set;}

						Party Party {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface PartySkill  : AccessControlledObject 
		{
						global::System.Decimal? YearsExperience {set;}

						global::System.DateTime? StartedUsingDate {set;}

						SkillRating SkillRating {set;}

						SkillLevel SkillLevel {set;}

						Skill Skill {set;}

		}
		public interface Passport  : AccessControlledObject 
		{
						global::System.DateTime? IssueDate {set;}

						global::System.DateTime? ExpiriationDate {set;}

						global::System.String Number {set;}

		}
		public interface PayCheck  : Payment 
		{
						Deduction Deductions {set;}

						Employment Employment {set;}

		}
		public interface PayGrade  : AccessControlledObject, Commentable 
		{
						global::System.String Name {set;}

						SalaryStep SalarySteps {set;}

		}
		public interface PayHistory  : AccessControlledObject, Period 
		{
						Employment Employment {set;}

						TimeFrequency TimeFrequency {set;}

						SalaryStep SalaryStep {set;}

						global::System.Decimal? Amount {set;}

		}
		public interface PaymentApplication  : AccessControlledObject 
		{
						global::System.Decimal AmountApplied {set;}

						InvoiceItem InvoiceItem {set;}

						Invoice Invoice {set;}

						BillingAccount BillingAccount {set;}

		}
		public interface PaymentBudgetAllocation  : AccessControlledObject 
		{
						Payment Payment {set;}

						BudgetItem BudgetItem {set;}

						global::System.Decimal Amount {set;}

		}
		public interface PayrollPreference  : AccessControlledObject 
		{
						global::System.Decimal? Percentage {set;}

						global::System.String AccountNumber {set;}

						PaymentMethod PaymentMethod {set;}

						TimeFrequency TimeFrequency {set;}

						DeductionType DeductionType {set;}

						global::System.Decimal? Amount {set;}

		}
		public interface PerformanceNote  : AccessControlledObject, Commentable 
		{
						global::System.String Description {set;}

						global::System.DateTime? CommunicationDate {set;}

						Person GivenByManager {set;}

						Person Employee {set;}

		}
		public interface PerformanceReview  : AccessControlledObject, Commentable, Period 
		{
						Person Manager {set;}

						PayHistory PayHistory {set;}

						PayCheck BonusPayCheck {set;}

						PerformanceReviewItem PerformanceReviewItems {set;}

						Person Employee {set;}

						Position ResultingPosition {set;}

		}
		public interface PerformanceReviewItem  : Commentable, AccessControlledObject 
		{
						RatingType RatingType {set;}

						PerformanceReviewItemType PerformanceReviewItemType {set;}

		}
		public interface PerformanceReviewItemType  : Enumeration 
		{
		}
		public interface PerformanceSpecification  : PartSpecification 
		{
		}
		public interface PersonalTitle  : Enumeration 
		{
		}
		public interface PersonTraining  : Period, AccessControlledObject 
		{
						Training Training {set;}

		}
		public interface Phase  : WorkEffort 
		{
		}
		public interface PhoneCommunication  : CommunicationEvent 
		{
						global::System.Boolean? LeftVoiceMail {set;}

						global::System.Boolean IncomingCall {set;}

						Party Receivers {set;}

						Party Callers {set;}

		}
		public interface PickList  : Printable, Transitional 
		{
						CustomerShipment CustomerShipmentCorrection {set;}

						global::System.DateTime CreationDate {set;}

						PickListItem PickListItems {set;}

						PickListObjectState CurrentObjectState {set;}

						PickListStatus CurrentPickListStatus {set;}

						Person Picker {set;}

						PickListStatus PickListStatuses {set;}

						Party ShipToParty {set;}

						Store Store {set;}

		}
		public interface PickListItem  : AccessControlledObject, Deletable 
		{
						global::System.Decimal RequestedQuantity {set;}

						InventoryItem InventoryItem {set;}

						global::System.Decimal? ActualQuantity {set;}

		}
		public interface PickListObjectState  : ObjectState 
		{
		}
		public interface PickListStatus  : AccessControlledObject 
		{
						PickListObjectState PickListObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface Plant  : Facility 
		{
		}
		public interface Position  : AccessControlledObject 
		{
						Organisation Organisation {set;}

						global::System.Boolean? Temporary {set;}

						global::System.DateTime? EstimatedThroughDate {set;}

						global::System.DateTime? EstimatedFromDate {set;}

						PositionType PositionType {set;}

						global::System.Boolean? Fulltime {set;}

						global::System.Boolean? Salary {set;}

						PositionStatus PositionStatus {set;}

						BudgetItem ApprovedBudgetItem {set;}

						global::System.DateTime ActualFromDate {set;}

						global::System.DateTime? ActualThroughDate {set;}

		}
		public interface PositionFulfillment  : Commentable, Period, AccessControlledObject 
		{
						Position Position {set;}

						Person Person {set;}

		}
		public interface PositionReportingStructure  : AccessControlledObject, Commentable 
		{
						global::System.Boolean? Primary {set;}

						Position ManagedByPosition {set;}

						Position Position {set;}

		}
		public interface PositionResponsibility  : Commentable, AccessControlledObject 
		{
						Position Position {set;}

						Responsibility Responsibility {set;}

		}
		public interface PositionStatus  : Enumeration 
		{
		}
		public interface PositionType  : AccessControlledObject 
		{
						global::System.String Description {set;}

						Responsibility Responsibilities {set;}

						global::System.Decimal? BenefitPercentage {set;}

						global::System.String Title {set;}

						PositionTypeRate PositionTypeRate {set;}

		}
		public interface PositionTypeRate  : AccessControlledObject 
		{
						global::System.Decimal Rate {set;}

						RateType RateType {set;}

						TimeFrequency TimeFrequency {set;}

		}
		public interface PostalAddress  : ContactMechanism, GeoLocatable 
		{
						GeographicBoundary GeographicBoundaries {set;}

						global::System.String Address3 {set;}

						Country Country {set;}

						global::System.String Address2 {set;}

						City City {set;}

						global::System.String Address1 {set;}

						PostalBoundary PostalBoundary {set;}

						PostalCode PostalCode {set;}

						global::System.String Directions {set;}

						global::System.String FormattedFullAddress {set;}

		}
		public interface PostalBoundary  : AccessControlledObject, Deletable 
		{
						global::System.String PostalCode {set;}

						global::System.String Locality {set;}

						Country Country {set;}

						global::System.String Region {set;}

		}
		public interface PostalCode  : CountryBound, GeographicBoundary 
		{
						global::System.String Code {set;}

		}
		public interface PrintQueue  : AccessControlledObject, UniquelyIdentifiable 
		{
						Printable Printables {set;}

						global::System.String Name {set;}

		}
		public interface Priority  : Enumeration 
		{
		}
		public interface Catalogue  : AccessControlledObject, UniquelyIdentifiable 
		{
						Media NoImageAvailableImage {set;}

						global::System.String Name {set;}

						global::System.String Description {set;}

						LocalisedText LocalisedNames {set;}

						LocalisedText LocalisedDescriptions {set;}

						Media CatalogueImage {set;}

						ProductCategory ProductCategories {set;}

		}
		public interface ProductCategory  : AccessControlledObject, UniquelyIdentifiable 
		{
						Package Package {set;}

						global::System.String Code {set;}

						Media NoImageAvailableImage {set;}

						ProductCategory Parents {set;}

						ProductCategory Children {set;}

						global::System.String Description {set;}

						global::System.String Name {set;}

						LocalisedText LocalisedNames {set;}

						LocalisedText LocalisedDescriptions {set;}

						Media CategoryImage {set;}

						ProductCategory Ancestors {set;}

		}
		public interface ProductCategoryRevenue  : Deletable, AccessControlledObject 
		{
						global::System.String ProductCategoryName {set;}

						global::System.Int32 Month {set;}

						InternalOrganisation InternalOrganisation {set;}

						ProductCategory ProductCategory {set;}

						global::System.Decimal Revenue {set;}

						Currency Currency {set;}

						global::System.Int32 Year {set;}

		}
		public interface ProductCategoryRevenueHistory  : AccessControlledObject 
		{
						Currency Currency {set;}

						global::System.Decimal? Revenue {set;}

						InternalOrganisation InternalOrganisation {set;}

						ProductCategory ProductCategory {set;}

		}
		public interface ProductCharacteristic  : Enumeration 
		{
		}
		public interface ProductCharacteristicValue  : Localised 
		{
						ProductCharacteristic ProductCharacteristic {set;}

						global::System.String Value {set;}

		}
		public interface ProductConfiguration  : ProductAssociation 
		{
						Product ProductsUsedIn {set;}

						Product Product {set;}

						global::System.Decimal? QuantityUsed {set;}

						global::System.String Description {set;}

		}
		public interface ProductDeliverySkillRequirement  : AccessControlledObject 
		{
						global::System.DateTime? StartedUsingDate {set;}

						Service Service {set;}

						Skill Skill {set;}

		}
		public interface ProductDrawing  : Document 
		{
		}
		public interface ProductFeatureApplicabilityRelationship  : AccessControlledObject 
		{
						Product AvailableFor {set;}

						ProductFeature UsedToDefine {set;}

		}
		public interface ProductionRun  : WorkEffort 
		{
						global::System.Int32? QuantityProduced {set;}

						global::System.Int32? QuantityRejected {set;}

						global::System.Int32? QuantityToProduce {set;}

		}
		public interface ProductModel  : Document 
		{
		}
		public interface ProductPurchasePrice  : AccessControlledObject, Period 
		{
						global::System.Decimal Price {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						Currency Currency {set;}

		}
		public interface ProductQuality  : ProductFeature, Enumeration 
		{
		}
		public interface ProductQuote  : Quote 
		{
						ProductQuoteObjectState CurrentObjectState {set;}

		}
		public interface ProductQuoteObjectState  : ObjectState 
		{
		}
		public interface ProductRequirement  : Requirement 
		{
						Product Product {set;}

						DesiredProductFeature DesiredProductFeatures {set;}

		}
		public interface ProductRevenue  : Deletable, AccessControlledObject 
		{
						global::System.Decimal Revenue {set;}

						global::System.String ProductName {set;}

						Currency Currency {set;}

						global::System.Int32 Year {set;}

						Product Product {set;}

						global::System.Int32 Month {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface ProductRevenueHistory  : AccessControlledObject 
		{
						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal? Revenue {set;}

						Currency Currency {set;}

						Product Product {set;}

		}
		public interface ProductType  : UniquelyIdentifiable, AccessControlledObject 
		{
						ProductCharacteristic ProductCharacteristics {set;}

						global::System.String Name {set;}

		}
		public interface ProfessionalAssignment  : AccessControlledObject, Period 
		{
						Person Professional {set;}

						EngagementItem EngagementItem {set;}

		}
		public interface ProfessionalPlacement  : EngagementItem 
		{
		}
		public interface ProfessionalServicesRelationship  : PartyRelationship 
		{
						Person Professional {set;}

						Organisation ProfessionalServicesProvider {set;}

		}
		public interface Program  : WorkEffort 
		{
		}
		public interface Project  : WorkEffort 
		{
		}
		public interface ProjectRequirement  : Requirement 
		{
						Deliverable NeededDeliverables {set;}

		}
		public interface Property  : FixedAsset 
		{
		}
		public interface Proposal  : Quote 
		{
						ProposalObjectState CurrentObjectState {set;}

		}
		public interface ProposalObjectState  : ObjectState 
		{
		}
		public interface ProspectRelationship  : PartyRelationship 
		{
						InternalOrganisation InternalOrganisation {set;}

						Party Prospect {set;}

		}
		public interface Province  : CityBound, GeographicBoundary, CountryBound 
		{
						global::System.String Name {set;}

		}
		public interface PurchaseAgreement  : Agreement 
		{
		}
		public interface PurchaseInvoice  : Invoice 
		{
						PurchaseInvoiceItem PurchaseInvoiceItems {set;}

						InternalOrganisation BilledToInternalOrganisation {set;}

						PurchaseInvoiceStatus CurrentInvoiceStatus {set;}

						PurchaseInvoiceObjectState CurrentObjectState {set;}

						Party BilledFromParty {set;}

						PurchaseInvoiceType PurchaseInvoiceType {set;}

						PurchaseInvoiceStatus InvoiceStatuses {set;}

		}
		public interface PurchaseInvoiceItem  : InvoiceItem 
		{
						PurchaseInvoiceItemType PurchaseInvoiceItemType {set;}

						Part Part {set;}

						PurchaseInvoiceItemStatus CurrentInvoiceItemStatus {set;}

						PurchaseInvoiceItemStatus InvoiceItemStatuses {set;}

						PurchaseInvoiceItemObjectState CurrentObjectState {set;}

		}
		public interface PurchaseInvoiceItemObjectState  : ObjectState 
		{
		}
		public interface PurchaseInvoiceItemStatus  : AccessControlledObject 
		{
						PurchaseInvoiceItemObjectState PurchaseInvoiceItemObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface PurchaseInvoiceItemType  : Enumeration 
		{
		}
		public interface PurchaseInvoiceObjectState  : ObjectState 
		{
		}
		public interface PurchaseInvoiceStatus  : AccessControlledObject 
		{
						global::System.DateTime? StartDateTime {set;}

						PurchaseInvoiceObjectState PurchaseInvoiceObjectState {set;}

		}
		public interface PurchaseInvoiceType  : Enumeration 
		{
		}
		public interface PurchaseOrder  : Order 
		{
						PurchaseOrderItem PurchaseOrderItems {set;}

						Party PreviousTakenViaSupplier {set;}

						PurchaseOrderStatus PaymentStatuses {set;}

						PurchaseOrderStatus CurrentPaymentStatus {set;}

						Party TakenViaSupplier {set;}

						PurchaseOrderObjectState CurrentObjectState {set;}

						PurchaseOrderStatus CurrentShipmentStatus {set;}

						ContactMechanism TakenViaContactMechanism {set;}

						PurchaseOrderStatus OrderStatuses {set;}

						ContactMechanism BillToContactMechanism {set;}

						PurchaseOrderStatus ShipmentStatuses {set;}

						InternalOrganisation ShipToBuyer {set;}

						PurchaseOrderStatus CurrentOrderStatus {set;}

						Facility Facility {set;}

						PostalAddress ShipToAddress {set;}

						InternalOrganisation BillToPurchaser {set;}

		}
		public interface PurchaseOrderItem  : OrderItem 
		{
						PurchaseOrderItemStatus OrderItemStatuses {set;}

						PurchaseOrderItemObjectState CurrentObjectState {set;}

						PurchaseOrderItemStatus ShipmentStatuses {set;}

						PurchaseOrderItemStatus PaymentStatuses {set;}

						global::System.Decimal QuantityReceived {set;}

						PurchaseOrderItemStatus CurrentShipmentStatus {set;}

						Product Product {set;}

						PurchaseOrderItemStatus CurrentOrderItemStatus {set;}

						PurchaseOrderItemStatus CurrentPaymentStatus {set;}

						Part Part {set;}

		}
		public interface PurchaseOrderItemObjectState  : ObjectState 
		{
		}
		public interface PurchaseOrderItemStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						PurchaseOrderItemObjectState PurchaseOrderItemObjectState {set;}

		}
		public interface PurchaseOrderObjectState  : ObjectState 
		{
		}
		public interface PurchaseOrderStatus  : AccessControlledObject 
		{
						PurchaseOrderObjectState PurchaseOrderObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface PurchaseReturn  : Shipment 
		{
						PurchaseReturnStatus CurrentShipmentStatus {set;}

						PurchaseReturnObjectState CurrentObjectState {set;}

						PurchaseReturnStatus ShipmentStatuses {set;}

		}
		public interface PurchaseReturnObjectState  : ObjectState 
		{
		}
		public interface PurchaseReturnStatus  : AccessControlledObject 
		{
						PurchaseReturnObjectState PurchaseReturnObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface PurchaseShipment  : Shipment 
		{
						PurchaseShipmentObjectState CurrentObjectState {set;}

						Facility Facility {set;}

						PurchaseShipmentStatus ShipmentStatuses {set;}

						PurchaseShipmentStatus CurrentShipmentStatus {set;}

						PurchaseOrder PurchaseOrder {set;}

		}
		public interface PurchaseShipmentObjectState  : ObjectState 
		{
		}
		public interface PurchaseShipmentStatus  : AccessControlledObject 
		{
						PurchaseShipmentObjectState PurchaseShipmentObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface Qualification  : Enumeration 
		{
		}
		public interface QuoteItem  : Commentable, AccessControlledObject 
		{
						Party Authorizer {set;}

						Deliverable Deliverable {set;}

						Product Product {set;}

						global::System.DateTime? EstimatedDeliveryDate {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						ProductFeature ProductFeature {set;}

						global::System.Decimal? UnitPrice {set;}

						Skill Skill {set;}

						WorkEffort WorkEffort {set;}

						QuoteTerm QuoteTerms {set;}

						global::System.Int32? Quantity {set;}

						RequestItem RequestItem {set;}

		}
		public interface QuoteTerm  : AccessControlledObject 
		{
						global::System.String TermValue {set;}

						TermType TermType {set;}

		}
		public interface RateType  : Enumeration 
		{
		}
		public interface RatingType  : Enumeration 
		{
		}
		public interface RawMaterial  : Part 
		{
		}
		public interface Receipt  : Payment 
		{
		}
		public interface ReceiptAccountingTransaction  : ExternalAccountingTransaction 
		{
						Receipt Receipt {set;}

		}
		public interface RecurringCharge  : PriceComponent 
		{
						TimeFrequency TimeFrequency {set;}

		}
		public interface Region  : GeographicBoundaryComposite 
		{
						global::System.String Name {set;}

		}
		public interface RequestForInformation  : Request 
		{
		}
		public interface RequestForProposal  : Request 
		{
		}
		public interface RequestForQuote  : Request 
		{
		}
		public interface RequestItem  : AccessControlledObject, Commentable 
		{
						global::System.String Description {set;}

						global::System.Int32? Quantity {set;}

						Requirement Requirements {set;}

						Deliverable Deliverable {set;}

						ProductFeature ProductFeature {set;}

						NeededSkill NeededSkill {set;}

						Product Product {set;}

						global::System.Decimal? MaximumAllowedPrice {set;}

						global::System.DateTime? RequiredByDate {set;}

		}
		public interface RequirementBudgetAllocation  : AccessControlledObject 
		{
						BudgetItem BudgetItem {set;}

						Requirement Requirement {set;}

						global::System.Decimal Amount {set;}

		}
		public interface RequirementCommunication  : AccessControlledObject 
		{
						CommunicationEvent CommunicationEvent {set;}

						Requirement Requirement {set;}

						Person AssociatedProfessional {set;}

		}
		public interface RequirementObjectState  : ObjectState 
		{
		}
		public interface RequirementStatus  : AccessControlledObject 
		{
						RequirementObjectState RequirementObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface Research  : WorkEffort 
		{
		}
		public interface ResourceRequirement  : Requirement 
		{
						global::System.String Duties {set;}

						global::System.Decimal? NumberOfPositions {set;}

						global::System.DateTime? RequiredStartDate {set;}

						NeededSkill NeededSkills {set;}

						global::System.DateTime? RequiredEndDate {set;}

		}
		public interface RespondingParty  : AccessControlledObject 
		{
						global::System.DateTime? SendingDate {set;}

						ContactMechanism ContactMechanism {set;}

						Party Party {set;}

		}
		public interface Responsibility  : AccessControlledObject 
		{
						global::System.String Description {set;}

		}
		public interface Resume  : AccessControlledObject 
		{
						global::System.DateTime ResumeDate {set;}

						global::System.String ResumeText {set;}

		}
		public interface RevenueQuantityBreak  : AccessControlledObject 
		{
						ProductCategory ProductCategory {set;}

						global::System.Decimal? Through {set;}

						global::System.Decimal? From {set;}

		}
		public interface RevenueValueBreak  : AccessControlledObject 
		{
						ProductCategory ProductCategory {set;}

						global::System.Decimal? ThroughAmount {set;}

						global::System.Decimal? FromAmount {set;}

		}
		public interface Room  : Facility, Container 
		{
		}
		public interface SalaryStep  : AccessControlledObject 
		{
						global::System.DateTime ModifiedDate {set;}

						global::System.Decimal Amount {set;}

		}
		public interface SalesAccountingTransaction  : ExternalAccountingTransaction 
		{
						Invoice Invoice {set;}

		}
		public interface SalesAgreement  : Agreement 
		{
		}
		public interface SalesChannel  : Enumeration 
		{
		}
		public interface SalesChannelRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Int32 Year {set;}

						global::System.Int32 Month {set;}

						Currency Currency {set;}

						SalesChannel SalesChannel {set;}

						global::System.String SalesChannelName {set;}

						global::System.Decimal Revenue {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface SalesChannelRevenueHistory  : AccessControlledObject 
		{
						SalesChannel SalesChannel {set;}

						Currency Currency {set;}

						global::System.Decimal? Revenue {set;}

						InternalOrganisation InternalOrganisation {set;}

		}
		public interface SalesInvoice  : Invoice 
		{
						SalesInvoiceObjectState CurrentObjectState {set;}

						global::System.Decimal? TotalListPrice {set;}

						InternalOrganisation BilledFromInternalOrganisation {set;}

						ContactMechanism BillToContactMechanism {set;}

						Party PreviousBillToCustomer {set;}

						SalesInvoiceType SalesInvoiceType {set;}

						global::System.Decimal InitialProfitMargin {set;}

						PaymentMethod PaymentMethod {set;}

						SalesOrder SalesOrder {set;}

						global::System.Decimal InitialMarkupPercentage {set;}

						global::System.Decimal MaintainedMarkupPercentage {set;}

						Person SalesReps {set;}

						Shipment Shipment {set;}

						global::System.Decimal MaintainedProfitMargin {set;}

						SalesInvoiceStatus InvoiceStatuses {set;}

						Party PreviousShipToCustomer {set;}

						Party BillToCustomer {set;}

						SalesInvoiceStatus CurrentInvoiceStatus {set;}

						SalesInvoiceItem SalesInvoiceItems {set;}

						global::System.Decimal TotalListPriceCustomerCurrency {set;}

						Party ShipToCustomer {set;}

						ContactMechanism BilledFromContactMechanism {set;}

						global::System.Decimal? TotalPurchasePrice {set;}

						SalesChannel SalesChannel {set;}

						Party Customers {set;}

						PostalAddress ShipToAddress {set;}

						Store Store {set;}

		}
		public interface SalesInvoiceItem  : InvoiceItem 
		{
						ProductFeature ProductFeature {set;}

						SalesInvoiceItemObjectState CurrentObjectState {set;}

						global::System.Decimal? RequiredProfitMargin {set;}

						global::System.Decimal InitialMarkupPercentage {set;}

						global::System.Decimal MaintainedMarkupPercentage {set;}

						Product Product {set;}

						global::System.Decimal UnitPurchasePrice {set;}

						SalesInvoiceItemStatus InvoiceItemStatuses {set;}

						SalesOrderItem SalesOrderItem {set;}

						SalesInvoiceItemType SalesInvoiceItemType {set;}

						Person SalesRep {set;}

						global::System.Decimal InitialProfitMargin {set;}

						SalesInvoiceItemStatus CurrentInvoiceItemStatus {set;}

						global::System.Decimal MaintainedProfitMargin {set;}

						TimeEntry TimeEntries {set;}

						global::System.Decimal? RequiredMarkupPercentage {set;}

		}
		public interface SalesInvoiceItemObjectState  : ObjectState 
		{
		}
		public interface SalesInvoiceItemStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						SalesInvoiceItemObjectState SalesInvoiceItemObjectState {set;}

		}
		public interface SalesInvoiceItemType  : Enumeration 
		{
		}
		public interface SalesInvoiceObjectState  : ObjectState 
		{
		}
		public interface SalesInvoiceStatus  : AccessControlledObject 
		{
						SalesInvoiceObjectState SalesInvoiceObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface SalesInvoiceType  : Enumeration 
		{
		}
		public interface SalesOrder  : Order 
		{
						ContactMechanism TakenByContactMechanism {set;}

						SalesOrderStatus ShipmentStatuses {set;}

						SalesOrderStatus CurrentShipmentStatus {set;}

						SalesOrderStatus CurrentPaymentStatus {set;}

						Party ShipToCustomer {set;}

						Party BillToCustomer {set;}

						global::System.Decimal TotalPurchasePrice {set;}

						ShipmentMethod ShipmentMethod {set;}

						global::System.Decimal TotalListPriceCustomerCurrency {set;}

						global::System.Decimal MaintainedProfitMargin {set;}

						PostalAddress ShipToAddress {set;}

						Party PreviousShipToCustomer {set;}

						ContactMechanism BillToContactMechanism {set;}

						Person SalesReps {set;}

						global::System.Decimal InitialProfitMargin {set;}

						global::System.Decimal TotalListPrice {set;}

						global::System.Boolean PartiallyShip {set;}

						SalesOrderStatus PaymentStatuses {set;}

						Party Customers {set;}

						Store Store {set;}

						global::System.Decimal MaintainedMarkupPercentage {set;}

						ContactMechanism BillFromContactMechanism {set;}

						PaymentMethod PaymentMethod {set;}

						ContactMechanism PlacingContactMechanism {set;}

						SalesOrderStatus CurrentOrderStatus {set;}

						Party PreviousBillToCustomer {set;}

						SalesChannel SalesChannel {set;}

						Party PlacingCustomer {set;}

						SalesOrderStatus OrderStatuses {set;}

						SalesInvoice ProformaInvoice {set;}

						SalesOrderItem SalesOrderItems {set;}

						SalesOrderObjectState CurrentObjectState {set;}

						global::System.Decimal InitialMarkupPercentage {set;}

						InternalOrganisation TakenByInternalOrganisation {set;}

		}
		public interface SalesOrderItem  : OrderItem 
		{
						global::System.Decimal InitialProfitMargin {set;}

						SalesOrderItemStatus CurrentPaymentStatus {set;}

						global::System.Decimal QuantityShortFalled {set;}

						OrderItem OrderedWithFeatures {set;}

						global::System.Decimal MaintainedProfitMargin {set;}

						global::System.Decimal? RequiredProfitMargin {set;}

						SalesOrderItemStatus OrderItemStatuses {set;}

						SalesOrderItemStatus CurrentShipmentStatus {set;}

						NonSerializedInventoryItem PreviousReservedFromInventoryItem {set;}

						global::System.Decimal? QuantityShipNow {set;}

						global::System.Decimal? RequiredMarkupPercentage {set;}

						global::System.Decimal QuantityShipped {set;}

						SalesOrderItemStatus CurrentOrderItemStatus {set;}

						PostalAddress ShipToAddress {set;}

						global::System.Decimal QuantityPicked {set;}

						Product PreviousProduct {set;}

						SalesOrderItemObjectState CurrentObjectState {set;}

						global::System.Decimal UnitPurchasePrice {set;}

						Party ShipToParty {set;}

						PostalAddress AssignedShipToAddress {set;}

						global::System.Decimal QuantityReturned {set;}

						global::System.Decimal QuantityReserved {set;}

						Person SalesRep {set;}

						SalesOrderItemStatus ShipmentStatuses {set;}

						Party AssignedShipToParty {set;}

						global::System.Decimal QuantityPendingShipment {set;}

						global::System.Decimal MaintainedMarkupPercentage {set;}

						global::System.Decimal InitialMarkupPercentage {set;}

						NonSerializedInventoryItem ReservedFromInventoryItem {set;}

						Product Product {set;}

						ProductFeature ProductFeature {set;}

						global::System.Decimal QuantityRequestsShipping {set;}

						SalesOrderItemStatus PaymentStatuses {set;}

		}
		public interface SalesOrderItemObjectState  : ObjectState 
		{
		}
		public interface SalesOrderItemStatus  : AccessControlledObject 
		{
						SalesOrderItemObjectState SalesOrderItemObjectState {set;}

						global::System.DateTime StartDateTime {set;}

		}
		public interface SalesOrderObjectState  : ObjectState 
		{
		}
		public interface SalesOrderStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						SalesOrderObjectState SalesOrderObjectState {set;}

		}
		public interface SalesRepCommission  : AccessControlledObject 
		{
						global::System.Decimal? Commission {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.String SalesRepName {set;}

						global::System.Int32? Month {set;}

						global::System.Int32 Year {set;}

						Currency Currency {set;}

						Person SalesRep {set;}

		}
		public interface SalesRepPartyProductCategoryRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Int32 Year {set;}

						Person SalesRep {set;}

						ProductCategory ProductCategory {set;}

						global::System.Int32 Month {set;}

						Party Party {set;}

						global::System.Decimal Revenue {set;}

						Currency Currency {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.String SalesRepName {set;}

		}
		public interface SalesRepPartyRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Decimal Revenue {set;}

						global::System.Int32 Year {set;}

						Person SalesRep {set;}

						global::System.String SalesRepName {set;}

						InternalOrganisation InternalOrganisation {set;}

						Party Party {set;}

						Currency Currency {set;}

						global::System.Int32 Month {set;}

		}
		public interface SalesRepProductCategoryRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Int32 Month {set;}

						global::System.String SalesRepName {set;}

						InternalOrganisation InternalOrganisation {set;}

						ProductCategory ProductCategory {set;}

						Currency Currency {set;}

						global::System.Decimal Revenue {set;}

						global::System.Int32 Year {set;}

						Person SalesRep {set;}

		}
		public interface SalesRepRelationship  : PartyRelationship 
		{
						Person SalesRepresentative {set;}

						global::System.Decimal LastYearsCommission {set;}

						ProductCategory ProductCategories {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal YTDCommission {set;}

						Party Customer {set;}

		}
		public interface SalesRepRevenue  : AccessControlledObject, Deletable 
		{
						global::System.Decimal Revenue {set;}

						Currency Currency {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.Int32 Month {set;}

						global::System.String SalesRepName {set;}

						global::System.Int32 Year {set;}

						Person SalesRep {set;}

		}
		public interface SalesRepRevenueHistory  : AccessControlledObject 
		{
						Currency Currency {set;}

						Person SalesRep {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.Decimal? Revenue {set;}

		}
		public interface SalesTerritory  : GeographicBoundaryComposite 
		{
						global::System.String Name {set;}

		}
		public interface Salutation  : Enumeration 
		{
		}
		public interface SerializedInventoryItem  : InventoryItem 
		{
						SerializedInventoryItemStatus InventoryItemStatuses {set;}

						global::System.String SerialNumber {set;}

						SerializedInventoryItemObjectState CurrentObjectState {set;}

						SerializedInventoryItemStatus CurrentInventoryItemStatus {set;}

		}
		public interface SerializedInventoryItemObjectState  : ObjectState 
		{
		}
		public interface SerializedInventoryItemStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						SerializedInventoryItemObjectState SerializedInventoryItemObjectState {set;}

		}
		public interface ServiceConfiguration  : InventoryItemConfiguration 
		{
		}
		public interface ServiceEntryBilling  : Object 
		{
						ServiceEntry ServiceEntry {set;}

						InvoiceItem InvoiceItem {set;}

		}
		public interface ServiceEntryHeader  : Period, AccessControlledObject 
		{
						ServiceEntry ServiceEntries {set;}

						global::System.DateTime SubmittedDate {set;}

						Person SubmittedBy {set;}

		}
		public interface ServiceFeature  : Enumeration, ProductFeature 
		{
		}
		public interface ServiceTerritory  : GeographicBoundaryComposite 
		{
						global::System.String Name {set;}

		}
		public interface Shelf  : Container 
		{
		}
		public interface ShipmentItem  : Deletable, AccessControlledObject 
		{
						global::System.Decimal Quantity {set;}

						Part Part {set;}

						global::System.String ContentsDescription {set;}

						Document Documents {set;}

						global::System.Decimal QuantityShipped {set;}

						ShipmentItem InResponseToShipmentItems {set;}

						InventoryItem InventoryItems {set;}

						ProductFeature ProductFeatures {set;}

						InvoiceItem InvoiceItems {set;}

						Good Good {set;}

		}
		public interface ShipmentMethod  : Enumeration 
		{
		}
		public interface ShipmentPackage  : AccessControlledObject, UniquelyIdentifiable 
		{
						PackagingContent PackagingContents {set;}

						Document Documents {set;}

						global::System.DateTime CreationDate {set;}

						global::System.Int32 SequenceNumber {set;}

		}
		public interface ShipmentReceipt  : AccessControlledObject 
		{
						global::System.String ItemDescription {set;}

						NonSerializedInventoryItem InventoryItem {set;}

						global::System.String RejectionReason {set;}

						OrderItem OrderItem {set;}

						global::System.Decimal QuantityRejected {set;}

						ShipmentItem ShipmentItem {set;}

						global::System.DateTime ReceivedDateTime {set;}

						global::System.Decimal QuantityAccepted {set;}

		}
		public interface ShipmentRouteSegment  : AccessControlledObject 
		{
						global::System.Decimal? EndKilometers {set;}

						Facility FromFacility {set;}

						global::System.Decimal? StartKilometers {set;}

						ShipmentMethod ShipmentMethod {set;}

						global::System.DateTime? EstimatedStartDateTime {set;}

						Facility ToFacility {set;}

						global::System.DateTime? EstimatedArrivalDateTime {set;}

						Vehicle Vehicle {set;}

						global::System.DateTime? ActualArrivalDateTime {set;}

						global::System.DateTime? ActualStartDateTime {set;}

						Organisation Carrier {set;}

		}
		public interface ShipmentValue  : AccessControlledObject 
		{
						global::System.Decimal? ThroughAmount {set;}

						global::System.Decimal? FromAmount {set;}

		}
		public interface ShippingAndHandlingCharge  : OrderAdjustment 
		{
		}
		public interface ShippingAndHandlingComponent  : AccessControlledObject, Period 
		{
						global::System.Decimal? Cost {set;}

						ShipmentMethod ShipmentMethod {set;}

						Carrier Carrier {set;}

						ShipmentValue ShipmentValue {set;}

						Currency Currency {set;}

						InternalOrganisation SpecifiedFor {set;}

						GeographicBoundary GeographicBoundary {set;}

		}
		public interface Size  : Enumeration, ProductFeature 
		{
		}
		public interface Skill  : Enumeration 
		{
		}
		public interface SkillLevel  : Enumeration 
		{
		}
		public interface SkillRating  : Enumeration 
		{
		}
		public interface SoftwareFeature  : ProductFeature, Enumeration 
		{
		}
		public interface StandardServiceOrderItem  : EngagementItem 
		{
		}
		public interface State  : CityBound, GeographicBoundary, CountryBound 
		{
						global::System.String Name {set;}

		}
		public interface StatementOfWork  : Quote 
		{
						StatementOfWorkObjectState CurrentObjectState {set;}

		}
		public interface StatementOfWorkObjectState  : ObjectState 
		{
		}
		public interface Store  : UniquelyIdentifiable, AccessControlledObject 
		{
						Catalogue Catalogues {set;}

						global::System.Decimal ShipmentThreshold {set;}

						Counter SalesOrderCounter {set;}

						global::System.String QuoteNumberPrefix {set;}

						global::System.String OutgoingShipmentNumberPrefix {set;}

						global::System.String SalesInvoiceNumberPrefix {set;}

						global::System.Int32 PaymentGracePeriod {set;}

						Media LogoImage {set;}

						global::System.Int32 PaymentNetDays {set;}

						Facility DefaultFacility {set;}

						global::System.String Name {set;}

						global::System.Decimal CreditLimit {set;}

						ShipmentMethod DefaultShipmentMethod {set;}

						Carrier DefaultCarrier {set;}

						Counter SalesInvoiceCounter {set;}

						global::System.Decimal OrderThreshold {set;}

						PaymentMethod DefaultPaymentMethod {set;}

						InternalOrganisation Owner {set;}

						FiscalYearInvoiceNumber FiscalYearInvoiceNumbers {set;}

						PaymentMethod PaymentMethods {set;}

						Counter OutgoingShipmentCounter {set;}

						global::System.String SalesOrderNumberPrefix {set;}

		}
		public interface StoreRevenue  : AccessControlledObject, Deletable 
		{
						InternalOrganisation InternalOrganisation {set;}

						global::System.Int32 Month {set;}

						Currency Currency {set;}

						Store Store {set;}

						global::System.String StoreName {set;}

						global::System.Decimal Revenue {set;}

						global::System.Int32 Year {set;}

		}
		public interface StoreRevenueHistory  : AccessControlledObject 
		{
						InternalOrganisation InternalOrganisation {set;}

						Currency Currency {set;}

						Store Store {set;}

						global::System.Decimal? Revenue {set;}

		}
		public interface StringTemplate  : UniquelyIdentifiable, Localised 
		{
						global::System.String Body {set;}

						global::System.String Name {set;}

		}
		public interface SubAgreement  : AgreementItem 
		{
		}
		public interface SubAssembly  : Part 
		{
		}
		public interface SubContractorAgreement  : Agreement 
		{
		}
		public interface SubContractorRelationship  : PartyRelationship 
		{
						Party Contractor {set;}

						Party SubContractor {set;}

		}
		public interface SupplierOffering  : Commentable, Period, AccessControlledObject 
		{
						RatingType Rating {set;}

						global::System.Int32? StandardLeadTime {set;}

						ProductPurchasePrice ProductPurchasePrices {set;}

						Ordinal Preference {set;}

						global::System.Decimal? MinimalOrderQuantity {set;}

						Product Product {set;}

						Party Supplier {set;}

						global::System.String ReferenceNumber {set;}

						Part Part {set;}

		}
		public interface SupplierRelationship  : PartyRelationship 
		{
						Organisation Supplier {set;}

						global::System.Int32 SubAccountNumber {set;}

						global::System.DateTime? LastReminderDate {set;}

						DunningType DunningType {set;}

						InternalOrganisation InternalOrganisation {set;}

						global::System.DateTime? BlockedForDunning {set;}

		}
		public interface SurchargeAdjustment  : OrderAdjustment 
		{
		}
		public interface SurchargeComponent  : PriceComponent 
		{
						global::System.Decimal? Percentage {set;}

		}
		public interface Task  : WorkEffort 
		{
		}
		public interface TaxDocument  : Document 
		{
		}
		public interface TaxDue  : ExternalAccountingTransaction 
		{
		}
		public interface TelecommunicationsNumber  : ContactMechanism 
		{
						global::System.String AreaCode {set;}

						global::System.String CountryCode {set;}

						global::System.String ContactNumber {set;}

		}
		public interface TermType  : Enumeration 
		{
		}
		public interface Territory  : CityBound, GeographicBoundary, CountryBound 
		{
						global::System.String Name {set;}

		}
		public interface TestingRequirement  : PartSpecification 
		{
		}
		public interface Threshold  : AgreementTerm 
		{
		}
		public interface TimeAndMaterialsService  : Service 
		{
		}
		public interface TimeEntry  : ServiceEntry 
		{
						global::System.Decimal Cost {set;}

						global::System.Decimal GrossMargin {set;}

						QuoteTerm QuoteTerm {set;}

						global::System.Decimal? BillingRate {set;}

						UnitOfMeasure UnitOfMeasure {set;}

						global::System.Decimal? AmountOfTime {set;}

		}
		public interface TimeFrequency  : Enumeration, IUnitOfMeasure 
		{
		}
		public interface TimePeriodUsage  : DeploymentUsage 
		{
		}
		public interface Tolerance  : PartSpecification 
		{
		}
		public interface Training  : AccessControlledObject 
		{
						global::System.String Description {set;}

		}
		public interface Transfer  : Shipment 
		{
						TransferObjectState CurrentObjectState {set;}

						TransferStatus CurrentShipmentStatus {set;}

						TransferStatus ShipmentStatuses {set;}

		}
		public interface TransferObjectState  : ObjectState 
		{
		}
		public interface TransferStatus  : AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						TransferObjectState TransferObjectState {set;}

		}
		public interface Transition  : Object 
		{
						ObjectState FromStates {set;}

						ObjectState ToState {set;}

		}
		public interface UnitOfMeasure  : IUnitOfMeasure, Enumeration 
		{
		}
		public interface UnitOfMeasureConversion  : AccessControlledObject 
		{
						IUnitOfMeasure ToUnitOfMeasure {set;}

						global::System.DateTime? StartDate {set;}

						global::System.Decimal ConversionFactor {set;}

		}
		public interface UtilizationCharge  : PriceComponent 
		{
						global::System.Decimal? Quantity {set;}

						UnitOfMeasure UnitOfMeasure {set;}

		}
		public interface VarianceReason  : Enumeration 
		{
		}
		public interface VatCalculationMethod  : Enumeration 
		{
		}
		public interface VatForm  : AccessControlledObject 
		{
						global::System.String Name {set;}

						VatReturnBox VatReturnBoxes {set;}

		}
		public interface VatRate  : AccessControlledObject 
		{
						VatCalculationMethod VatCalculationMethod {set;}

						VatReturnBox VatReturnBoxes {set;}

						global::System.Decimal Rate {set;}

						OrganisationGlAccount VatPayableAccount {set;}

						Organisation TaxAuthority {set;}

						VatRateUsage VatRateUsage {set;}

						VatRatePurchaseKind VatRatePurchaseKind {set;}

						VatTariff VatTariff {set;}

						TimeFrequency PaymentFrequency {set;}

						OrganisationGlAccount VatToPayAccount {set;}

						EuSalesListType EuSalesListType {set;}

						OrganisationGlAccount VatToReceiveAccount {set;}

						OrganisationGlAccount VatReceivableAccount {set;}

						global::System.Boolean? ReverseCharge {set;}

		}
		public interface VatRatePurchaseKind  : Enumeration 
		{
		}
		public interface VatRateUsage  : Enumeration 
		{
		}
		public interface VatRegime  : Enumeration 
		{
						VatRate VatRate {set;}

						OrganisationGlAccount GeneralLedgerAccount {set;}

		}
		public interface VatReturnBox  : AccessControlledObject 
		{
						global::System.String HeaderNumber {set;}

						global::System.String Description {set;}

		}
		public interface VatReturnBoxType  : AccessControlledObject 
		{
						global::System.String Type {set;}

		}
		public interface VatTariff  : Enumeration 
		{
		}
		public interface Vehicle  : FixedAsset 
		{
		}
		public interface VolumeUsage  : DeploymentUsage 
		{
						global::System.Decimal Quantity {set;}

						UnitOfMeasure UnitOfMeasure {set;}

		}
		public interface Warehouse  : Facility 
		{
		}
		public interface WebAddress  : ElectronicAddress 
		{
		}
		public interface WebSiteCommunication  : CommunicationEvent 
		{
						Party Originator {set;}

						Party Receiver {set;}

		}
		public interface Withdrawal  : FinancialAccountTransaction 
		{
						Disbursement Disbursement {set;}

		}
		public interface WorkEffortAssignment  : Period, AccessControlledObject, Commentable, Deletable 
		{
						Person Professional {set;}

						WorkEffort Assignment {set;}

		}
		public interface WorkEffortAssignmentRate  : AccessControlledObject 
		{
						RateType RateType {set;}

						WorkEffortPartyAssignment WorkEffortPartyAssignment {set;}

		}
		public interface WorkEffortBilling  : Object 
		{
						WorkEffort WorkEffort {set;}

						global::System.Decimal? Percentage {set;}

						InvoiceItem InvoiceItem {set;}

		}
		public interface WorkEffortFixedAssetAssignment  : Commentable, AccessControlledObject, Period 
		{
						AssetAssignmentStatus AssetAssignmentStatus {set;}

						WorkEffort Assignment {set;}

						global::System.Decimal? AllocatedCost {set;}

						FixedAsset FixedAsset {set;}

		}
		public interface WorkEffortFixedAssetStandard  : AccessControlledObject 
		{
						global::System.Decimal? EstimatedCost {set;}

						global::System.Decimal? EstimatedDuration {set;}

						FixedAsset FixedAsset {set;}

						global::System.Int32? EstimatedQuantity {set;}

		}
		public interface WorkEffortGoodStandard  : AccessControlledObject 
		{
						Good Good {set;}

						global::System.Decimal? EstimatedCost {set;}

						global::System.Int32? EstimatedQuantity {set;}

		}
		public interface WorkEffortInventoryAssignment  : AccessControlledObject 
		{
						WorkEffort Assignment {set;}

						InventoryItem InventoryItem {set;}

						global::System.Int32? Quantity {set;}

		}
		public interface WorkEffortObjectState  : ObjectState 
		{
		}
		public interface WorkEffortPartStandard  : AccessControlledObject 
		{
						Part Part {set;}

						global::System.Decimal? EstimatedCost {set;}

						global::System.Int32? EstimatedQuantity {set;}

		}
		public interface WorkEffortPartyAssignment  : Period, AccessControlledObject, Commentable 
		{
						WorkEffort Assignment {set;}

						Party Party {set;}

						Facility Facility {set;}

		}
		public interface WorkEffortPurpose  : Enumeration 
		{
		}
		public interface WorkEffortSkillStandard  : AccessControlledObject 
		{
						Skill Skill {set;}

						global::System.Int32? EstimatedNumberOfPeople {set;}

						global::System.Decimal? EstimatedDuration {set;}

						global::System.Decimal? EstimatedCost {set;}

		}
		public interface WorkEffortStatus  : Deletable, AccessControlledObject 
		{
						global::System.DateTime StartDateTime {set;}

						WorkEffortObjectState WorkEffortObjectState {set;}

		}
		public interface WorkEffortType  : AccessControlledObject 
		{
						WorkEffortFixedAssetStandard WorkEffortFixedAssetStandards {set;}

						WorkEffortGoodStandard WorkEffortGoodStandards {set;}

						WorkEffortType Children {set;}

						FixedAsset FixedAssetToRepair {set;}

						global::System.String Description {set;}

						WorkEffortType Dependencies {set;}

						WorkEffortTypeKind WorkEffortTypeKind {set;}

						WorkEffortPartStandard WorkEffortPartStandards {set;}

						WorkEffortSkillStandard WorkEffortSkillStandards {set;}

						global::System.Decimal? StandardWorkHours {set;}

						Product ProductToProduce {set;}

						Deliverable DeliverableToProduce {set;}

		}
		public interface WorkEffortTypeKind  : Enumeration 
		{
		}
		public interface WorkFlow  : WorkEffort 
		{
		}
		public interface WorkRequirement  : Requirement 
		{
						FixedAsset FixedAsset {set;}

						Deliverable Deliverable {set;}

						Product Product {set;}

		}
}