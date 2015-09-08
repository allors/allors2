namespace Allors.Domain
{
	public interface AccessControlledObject  : Object 
	{
					Permission DeniedPermissions {set;}

					SecurityToken SecurityTokens {set;}

	}
	public interface Commentable  : Object 
	{
					global::System.String Comment {set;}

	}
	public interface Deletable  : Object 
	{
	}
	public interface Enumeration  : Object, AccessControlledObject, UniquelyIdentifiable 
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
	public interface ObjectState  : Object, UniquelyIdentifiable 
	{
					Permission DeniedPermissions {set;}

					global::System.String Name {set;}

	}
	public interface Period  : Object 
	{
					global::System.DateTime FromDate {set;}

					global::System.DateTime? ThroughDate {set;}

	}
	public interface Printable  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.String PrintContent {set;}

	}
	public interface SecurityTokenOwner  : Object 
	{
					SecurityToken OwnerSecurityToken {set;}

	}
	public interface Transitional  : Object, AccessControlledObject 
	{
					ObjectState PreviousObjectState {set;}

					ObjectState LastObjectState {set;}

	}
	public interface UniquelyIdentifiable  : Object 
	{
					global::System.Guid UniqueId {set;}

	}
	public interface User  : Object, SecurityTokenOwner, Localised, AccessControlledObject 
	{
					global::System.Boolean? UserEmailConfirmed {set;}

					global::System.String UserName {set;}

					global::System.String UserEmail {set;}

					global::System.String UserPasswordHash {set;}

	}
	public interface AgreementTerm  : Object, AccessControlledObject 
	{
					global::System.String TermValue {set;}

					TermType TermType {set;}

					global::System.String Description {set;}

	}
	public interface CityBound  : Object, AccessControlledObject 
	{
					City Cities {set;}

	}
	public interface CommunicationAttachment  : Object, AccessControlledObject 
	{
	}
	public interface ContactMechanism  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

					ContactMechanism FollowTo {set;}

	}
	public interface CountryBound  : Object, AccessControlledObject 
	{
					Country Country {set;}

	}
	public interface FinancialAccount  : Object, AccessControlledObject 
	{
					FinancialAccountTransaction FinancialAccountTransactions {set;}

	}
	public interface FixedAsset  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					global::System.DateTime? LastServiceDate {set;}

					global::System.DateTime? AcquiredDate {set;}

					global::System.String Description {set;}

					global::System.Decimal? ProductionCapacity {set;}

					global::System.DateTime? NextServiceDate {set;}

	}
	public interface GeographicBoundary  : Object, AccessControlledObject, GeoLocatable 
	{
					global::System.String Abbreviation {set;}

	}
	public interface GeographicBoundaryComposite  : Object, GeographicBoundary 
	{
					GeographicBoundary Associations {set;}

	}
	public interface GeoLocatable  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					global::System.Decimal Latitude {set;}

					global::System.Decimal Longitude {set;}

	}
	public interface InternalAccountingTransaction  : Object, AccountingTransaction 
	{
					InternalOrganisation InternalOrganisation {set;}

	}
	public interface InventoryItem  : Object, Transitional, AccessControlledObject, UniquelyIdentifiable 
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
	public interface InventoryItemConfiguration  : Object, AccessControlledObject, Commentable 
	{
					InventoryItem InventoryItem {set;}

					global::System.Int32 Quantity {set;}

					InventoryItem ComponentInventoryItem {set;}

	}
	public interface Invoice  : Object, Printable, AccessControlledObject, Commentable, UniquelyIdentifiable, Localised, Transitional 
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
	public interface InvoiceItem  : Object, AccessControlledObject, Transitional 
	{
					global::System.Decimal TotalIncVatCustomerCurrency {set;}

					AgreementTerm InvoiceTerms {set;}

					global::System.Decimal TotalVatCustomerCurrency {set;}

					global::System.Decimal TotalBasePrice {set;}

					global::System.Decimal TotalSurcharge {set;}

					global::System.Decimal TotalInvoiceAdjustment {set;}

					global::System.Decimal TotalExVatCustomerCurrency {set;}

					global::System.Decimal TotalDiscount {set;}

					InvoiceVatRateItem InvoiceVatRateItems {set;}

					global::System.Decimal? TotalDiscountAsPercentage {set;}

					global::System.Decimal CalculatedUnitPrice {set;}

					global::System.Decimal UnitDiscount {set;}

					VatRegime AssignedVatRegime {set;}

					global::System.Decimal TotalIncVat {set;}

					InvoiceItem AdjustmentFor {set;}

					global::System.Decimal UnitBasePrice {set;}

					global::System.Decimal TotalSurchargeCustomerCurrency {set;}

					SerializedInventoryItem SerializedInventoryItem {set;}

					PriceComponent CurrentPriceComponents {set;}

					DiscountAdjustment DiscountAdjustment {set;}

					global::System.Decimal? ActualUnitPrice {set;}

					global::System.String Message {set;}

					global::System.Decimal TotalInvoiceAdjustmentCustomerCurrency {set;}

					global::System.Decimal AmountPaid {set;}

					VatRate DerivedVatRate {set;}

					global::System.Decimal TotalDiscountCustomerCurrency {set;}

					global::System.Decimal UnitSurcharge {set;}

					global::System.Decimal TotalExVat {set;}

					global::System.Decimal Quantity {set;}

					global::System.Decimal? TotalSurchargeAsPercentage {set;}

					VatRegime VatRegime {set;}

					global::System.Decimal TotalBasePriceCustomerCurrency {set;}

					global::System.Decimal TotalVat {set;}

					SurchargeAdjustment SurchargeAdjustment {set;}

					global::System.Decimal UnitVat {set;}

					global::System.String Description {set;}

	}
	public interface IUnitOfMeasure  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					global::System.String Description {set;}

					UnitOfMeasureConversion UnitOfMeasureConversions {set;}

					global::System.String Abbreviation {set;}

	}
	public interface OrderAdjustment  : Object, AccessControlledObject 
	{
					global::System.Decimal? Amount {set;}

					VatRate VatRate {set;}

					global::System.Decimal? Percentage {set;}

	}
	public interface OrderItem  : Object, Commentable, Transitional, AccessControlledObject 
	{
					global::System.Decimal TotalDiscountAsPercentage {set;}

					DiscountAdjustment DiscountAdjustment {set;}

					global::System.Decimal UnitVat {set;}

					global::System.Decimal TotalVatCustomerCurrency {set;}

					VatRegime VatRegime {set;}

					BudgetItem BudgetItem {set;}

					global::System.Decimal TotalVat {set;}

					global::System.Decimal UnitSurcharge {set;}

					global::System.Decimal UnitDiscount {set;}

					global::System.Decimal PreviousQuantity {set;}

					global::System.Decimal QuantityOrdered {set;}

					global::System.Decimal TotalExVatCustomerCurrency {set;}

					VatRate DerivedVatRate {set;}

					global::System.Decimal? ActualUnitPrice {set;}

					global::System.Decimal TotalIncVatCustomerCurrency {set;}

					global::System.String Description {set;}

					global::System.Decimal UnitBasePrice {set;}

					PurchaseOrder CorrespondingPurchaseOrder {set;}

					global::System.Decimal CalculatedUnitPrice {set;}

					global::System.Decimal TotalOrderAdjustmentCustomerCurrency {set;}

					global::System.Decimal TotalOrderAdjustment {set;}

					global::System.Decimal TotalSurchargeCustomerCurrency {set;}

					QuoteItem QuoteItem {set;}

					global::System.DateTime? AssignedDeliveryDate {set;}

					global::System.DateTime? DeliveryDate {set;}

					global::System.Decimal TotalIncVat {set;}

					global::System.Decimal TotalSurchargeAsPercentage {set;}

					global::System.Decimal TotalDiscountCustomerCurrency {set;}

					global::System.Decimal TotalDiscount {set;}

					global::System.Decimal TotalSurcharge {set;}

					OrderTerm OrderTerms {set;}

					VatRegime AssignedVatRegime {set;}

					global::System.String ShippingInstruction {set;}

					global::System.Decimal TotalBasePrice {set;}

					OrderItem Associations {set;}

					global::System.Decimal TotalExVat {set;}

					global::System.Decimal TotalBasePriceCustomerCurrency {set;}

					PriceComponent CurrentPriceComponents {set;}

					SurchargeAdjustment SurchargeAdjustment {set;}

					global::System.String Message {set;}

	}
	public interface OrganisationClassification  : Object, PartyClassification 
	{
	}
	public interface Part  : Object, AccessControlledObject, UniquelyIdentifiable 
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

	}
	public interface PartBillOfMaterial  : Object, AccessControlledObject, Commentable, Period 
	{
					Part Part {set;}

					global::System.String Instruction {set;}

					global::System.Int32 QuantityUsed {set;}

					Part ComponentPart {set;}

	}
	public interface PartSpecification  : Object, UniquelyIdentifiable, Transitional, AccessControlledObject, Commentable 
	{
					PartSpecificationStatus PartSpecificationStatuses {set;}

					PartSpecificationObjectState CurrentObjectState {set;}

					global::System.DateTime? DocumentationDate {set;}

					PartSpecificationStatus CurrentPartSpecificationStatus {set;}

					global::System.String Description {set;}

	}
	public interface Party  : Object, AccessControlledObject, Localised, SecurityTokenOwner, UniquelyIdentifiable 
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
	public interface PartyClassification  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

	}
	public interface PartyRelationship  : Object, Period, Commentable, AccessControlledObject 
	{
					PartyRelationshipStatus PartyRelationshipStatus {set;}

					Agreement RelationshipAgreements {set;}

					Priority PartyRelationshipPriority {set;}

					global::System.Decimal? SimpleMovingAverage {set;}

					CommunicationEvent CommunicationEvents {set;}

	}
	public interface Payment  : Object, UniquelyIdentifiable, AccessControlledObject, Commentable 
	{
					global::System.Decimal Amount {set;}

					PaymentMethod PaymentMethod {set;}

					global::System.DateTime EffectiveDate {set;}

					Party SendingParty {set;}

					PaymentApplication PaymentApplications {set;}

					global::System.String ReferenceNumber {set;}

					Party ReceivingParty {set;}

	}
	public interface PaymentMethod  : Object, AccessControlledObject, UniquelyIdentifiable 
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
	public interface PersonClassification  : Object, PartyClassification 
	{
	}
	public interface Product  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					ProductCategory PrimaryProductCategory {set;}

					global::System.DateTime? SupportDiscontinuationDate {set;}

					global::System.DateTime? SalesDiscontinuationDate {set;}

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
	public interface ProductAssociation  : Object, AccessControlledObject, Commentable, Period 
	{
	}
	public interface ProductFeature  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					EstimatedProductCost EstimatedProductCosts {set;}

					PriceComponent BasePrices {set;}

					global::System.String Description {set;}

					ProductFeature DependentFeatures {set;}

					ProductFeature IncompatibleFeatures {set;}

					VatRate VatRate {set;}

	}
	public interface Quote  : Object, AccessControlledObject 
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
	public interface Request  : Object, AccessControlledObject, Commentable 
	{
					global::System.String Description {set;}

					global::System.DateTime? RequiredResponseDate {set;}

					RequestItem RequestItems {set;}

					global::System.String RequestNumber {set;}

					RespondingParty RespondingParties {set;}

					Party Originator {set;}

	}
	public interface Requirement  : Object, Transitional, UniquelyIdentifiable, AccessControlledObject 
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
	public interface Service  : Object, Product 
	{
	}
	public interface ServiceEntry  : Object, AccessControlledObject, Commentable 
	{
					global::System.DateTime? ThroughDateTime {set;}

					EngagementItem EngagementItem {set;}

					global::System.Boolean? IsBillable {set;}

					global::System.DateTime? FromDateTime {set;}

					global::System.String Description {set;}

					WorkEffort WorkEffort {set;}

	}
	public interface Shipment  : Object, UniquelyIdentifiable, Transitional, Printable, AccessControlledObject 
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
	public interface WorkEffort  : Object, Deletable, AccessControlledObject, Transitional, UniquelyIdentifiable 
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
	public interface AccountingTransaction  : Object, AccessControlledObject 
	{
					AccountingTransactionDetail AccountingTransactionDetails {set;}

					global::System.String Description {set;}

					global::System.DateTime TransactionDate {set;}

					global::System.Decimal DerivedTotalAmount {set;}

					AccountingTransactionNumber AccountingTransactionNumber {set;}

					global::System.DateTime EntryDate {set;}

	}
	public interface Agreement  : Object, UniquelyIdentifiable, Period, AccessControlledObject 
	{
					global::System.DateTime? AgreementDate {set;}

					Addendum Addenda {set;}

					global::System.String Description {set;}

					AgreementTerm AgreementTerms {set;}

					global::System.String Text {set;}

					AgreementItem AgreementItems {set;}

					global::System.String AgreementNumber {set;}

	}
	public interface AgreementItem  : Object, AccessControlledObject 
	{
					global::System.String Text {set;}

					Addendum Addenda {set;}

					AgreementItem Children {set;}

					global::System.String Description {set;}

					AgreementTerm AgreementTerms {set;}

	}
	public interface Budget  : Object, Commentable, UniquelyIdentifiable, AccessControlledObject, Period, Transitional 
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
	public interface CommunicationEvent  : Object, AccessControlledObject, UniquelyIdentifiable, Deletable, Commentable, Transitional 
	{
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
	public interface Container  : Object, AccessControlledObject 
	{
					Facility Facility {set;}

					global::System.String ContainerDescription {set;}

	}
	public interface DeploymentUsage  : Object, Period, AccessControlledObject, Commentable 
	{
					TimeFrequency TimeFrequency {set;}

	}
	public interface Document  : Object, Commentable, Printable, AccessControlledObject 
	{
					global::System.String Name {set;}

					global::System.String Description {set;}

					global::System.String Text {set;}

					global::System.String DocumentLocation {set;}

	}
	public interface ElectronicAddress  : Object, ContactMechanism 
	{
					global::System.String ElectronicAddressString {set;}

	}
	public interface EngagementItem  : Object, AccessControlledObject 
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
	public interface EstimatedProductCost  : Object, AccessControlledObject, Period 
	{
					global::System.Decimal Cost {set;}

					Currency Currency {set;}

					Organisation Organisation {set;}

					global::System.String Description {set;}

					GeographicBoundary GeographicBoundary {set;}

	}
	public interface ExternalAccountingTransaction  : Object, AccountingTransaction 
	{
					Party FromParty {set;}

					Party ToParty {set;}

	}
	public interface Facility  : Object, GeoLocatable, AccessControlledObject 
	{
					Facility MadeUpOf {set;}

					global::System.Decimal? SquareFootage {set;}

					global::System.String Description {set;}

					ContactMechanism FacilityContactMechanisms {set;}

					global::System.String Name {set;}

					InternalOrganisation Owner {set;}

	}
	public interface FinancialAccountTransaction  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

					global::System.DateTime? EntryDate {set;}

					global::System.DateTime TransactionDate {set;}

	}
	public interface Order  : Object, Localised, AccessControlledObject, Printable, UniquelyIdentifiable, Transitional, Commentable 
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
	public interface PriceComponent  : Object, Commentable, Period, AccessControlledObject 
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
	public interface AccessControl  : Object, Deletable, AccessControlledObject 
	{
					UserGroup SubjectGroups {set;}

					User Subjects {set;}

					SecurityToken Objects {set;}

					Role Role {set;}

					global::System.Guid CacheId {set;}

	}
	public interface Counter  : Object, UniquelyIdentifiable 
	{
					global::System.Int32 Value {set;}

	}
	public interface Country  : Object, CityBound, AccessControlledObject, GeographicBoundary 
	{
					Currency Currency {set;}

					global::System.String Name {set;}

					LocalisedText LocalisedNames {set;}

					global::System.String IsoCode {set;}

					VatRate VatRates {set;}

					global::System.Int32? IbanLength {set;}

					global::System.Boolean? EuMemberState {set;}

					global::System.String TelephoneCode {set;}

					global::System.String IbanRegex {set;}

					VatForm VatForm {set;}

					global::System.String UriExtension {set;}

	}
	public interface Currency  : Object, AccessControlledObject, IUnitOfMeasure 
	{
					global::System.String IsoCode {set;}

					global::System.String Name {set;}

					global::System.String Symbol {set;}

					LocalisedText LocalisedNames {set;}

	}
	public interface Image  : Object, Deletable 
	{
					Media Original {set;}

					Media Responsive {set;}

					global::System.String OriginalFilename {set;}

					Media Thumbnail {set;}

	}
	public interface Language  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					global::System.String IsoCode {set;}

					LocalisedText LocalisedNames {set;}

	}
	public interface Locale  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					Language Language {set;}

					Country Country {set;}

	}
	public interface LocalisedText  : Object, Localised, AccessControlledObject 
	{
					global::System.String Text {set;}

	}
	public interface Login  : Object, Deletable 
	{
					global::System.String Key {set;}

					global::System.String Provider {set;}

					User User {set;}

	}
	public interface Media  : Object, Deletable, AccessControlledObject, UniquelyIdentifiable 
	{
					MediaType MediaType {set;}

					MediaContent MediaContent {set;}

	}
	public interface MediaContent  : Object, Deletable 
	{
					global::System.Byte[] Value {set;}

					global::System.String Hash {set;}

	}
	public interface MediaType  : Object, AccessControlledObject 
	{
					global::System.String DefaultFileExtension {set;}

					global::System.String Name {set;}

	}
	public interface Permission  : Object, AccessControlledObject, Deletable 
	{
					global::System.Guid OperandTypePointer {set;}

					global::System.Guid ConcreteClassPointer {set;}

					global::System.Int32 OperationEnum {set;}

	}
	public interface Person  : Object, UniquelyIdentifiable, Deletable, Party, User, AccessControlledObject 
	{
					global::System.String LastName {set;}

					global::System.String MiddleName {set;}

					global::System.String FirstName {set;}

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

					Media Picture {set;}

					global::System.String SocialSecurityNumber {set;}

					global::System.DateTime? DeceasedDate {set;}

	}
	public interface PrintQueue  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					Printable Printables {set;}

					global::System.String Name {set;}

	}
	public interface Role  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					Permission Permissions {set;}

					global::System.String Name {set;}

	}
	public interface SecurityToken  : Object, Deletable 
	{
	}
	public interface Singleton  : Object, AccessControlledObject 
	{
					PrintQueue DefaultPrintQueue {set;}

					Locale DefaultLocale {set;}

					Locale Locales {set;}

					SecurityToken AdministratorSecurityToken {set;}

					User Guest {set;}

					SecurityToken DefaultSecurityToken {set;}

					Currency DefaultCurrency {set;}

					Media NoImageAvailableImage {set;}

					InternalOrganisation DefaultInternalOrganisation {set;}

	}
	public interface StringTemplate  : Object, Localised, UniquelyIdentifiable 
	{
					global::System.String Body {set;}

					global::System.String Name {set;}

					TemplatePurpose TemplatePurpose {set;}

	}
	public interface Transition  : Object 
	{
					ObjectState FromStates {set;}

					ObjectState ToState {set;}

	}
	public interface UserGroup  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					Role Role {set;}

					User Members {set;}

					UserGroup Parent {set;}

					global::System.String Name {set;}

					Party Party {set;}

	}
	public interface AccountAdjustment  : Object, FinancialAccountTransaction 
	{
	}
	public interface AccountingTransactionNumber  : Object, AccessControlledObject 
	{
					global::System.Int32? Number {set;}

					global::System.Int32? Year {set;}

					AccountingTransactionType AccountingTransactionType {set;}

	}
	public interface AccountingTransactionType  : Object, Enumeration 
	{
	}
	public interface Activity  : Object, WorkEffort 
	{
	}
	public interface AgreementExhibit  : Object, AgreementItem 
	{
	}
	public interface AgreementPricingProgram  : Object, AgreementItem 
	{
	}
	public interface AgreementSection  : Object, AgreementItem 
	{
	}
	public interface Amortization  : Object, InternalAccountingTransaction 
	{
	}
	public interface AmountDue  : Object, AccessControlledObject 
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
	public interface AssetAssignmentStatus  : Object, Enumeration 
	{
	}
	public interface Barrel  : Object, Container 
	{
	}
	public interface BillOfLading  : Object, Document 
	{
	}
	public interface Bin  : Object, Container 
	{
	}
	public interface BudgetObjectState  : Object, ObjectState 
	{
	}
	public interface Building  : Object, Facility 
	{
	}
	public interface Capitalization  : Object, InternalAccountingTransaction 
	{
	}
	public interface CaseObjectState  : Object, ObjectState 
	{
	}
	public interface Cash  : Object, PaymentMethod 
	{
					Person PersonResponsible {set;}

	}
	public interface ClientAgreement  : Object, Agreement 
	{
	}
	public interface Colour  : Object, ProductFeature, Enumeration 
	{
	}
	public interface CommunicationEventObjectState  : Object, ObjectState 
	{
	}
	public interface CommunicationEventPurpose  : Object, Enumeration 
	{
	}
	public interface ConstraintSpecification  : Object, PartSpecification 
	{
	}
	public interface ContactMechanismPurpose  : Object, Enumeration 
	{
	}
	public interface CostCenterSplitMethod  : Object, Enumeration 
	{
	}
	public interface CostOfGoodsSoldMethod  : Object, Enumeration 
	{
	}
	public interface CreditLine  : Object, ExternalAccountingTransaction 
	{
	}
	public interface CustomEngagementItem  : Object, EngagementItem 
	{
					global::System.String DescriptionOfWork {set;}

					global::System.Decimal? AgreedUponPrice {set;}

	}
	public interface CustomerRequirement  : Object, Requirement 
	{
	}
	public interface CustomerReturnObjectState  : Object, ObjectState 
	{
	}
	public interface CustomerShipmentObjectState  : Object, ObjectState 
	{
	}
	public interface DebitCreditConstant  : Object, UniquelyIdentifiable, Enumeration 
	{
	}
	public interface DeductionType  : Object, Enumeration 
	{
	}
	public interface DeliverableBasedService  : Object, Service 
	{
	}
	public interface DeliverableOrderItem  : Object, EngagementItem 
	{
					global::System.Decimal? AgreedUponPrice {set;}

	}
	public interface DeliverableType  : Object, Enumeration 
	{
	}
	public interface Deployment  : Object, AccessControlledObject, Period 
	{
					Good ProductOffering {set;}

					DeploymentUsage DeploymentUsage {set;}

					SerializedInventoryItem SerializedInventoryItem {set;}

	}
	public interface Deposit  : Object, FinancialAccountTransaction 
	{
					Receipt Receipts {set;}

	}
	public interface Disbursement  : Object, Payment 
	{
	}
	public interface DiscountAdjustment  : Object, OrderAdjustment 
	{
	}
	public interface DiscountComponent  : Object, PriceComponent 
	{
					global::System.Decimal? Percentage {set;}

	}
	public interface DropShipmentObjectState  : Object, ObjectState 
	{
	}
	public interface DunningType  : Object, Enumeration 
	{
	}
	public interface EmailAddress  : Object, ElectronicAddress 
	{
	}
	public interface EmploymentAgreement  : Object, Agreement 
	{
	}
	public interface EmploymentApplicationSource  : Object, Enumeration 
	{
	}
	public interface EmploymentApplicationStatus  : Object, Enumeration 
	{
	}
	public interface EmploymentTermination  : Object, Enumeration 
	{
	}
	public interface EmploymentTerminationReason  : Object, Enumeration 
	{
	}
	public interface EngineeringBom  : Object, PartBillOfMaterial 
	{
	}
	public interface EngineeringChangeObjectState  : Object, ObjectState 
	{
	}
	public interface EngineeringDocument  : Object, Document 
	{
	}
	public interface Equipment  : Object, FixedAsset 
	{
	}
	public interface EstimatedLaborCost  : Object, EstimatedProductCost 
	{
	}
	public interface EstimatedMaterialCost  : Object, EstimatedProductCost 
	{
	}
	public interface EstimatedOtherCost  : Object, EstimatedProductCost 
	{
	}
	public interface EuSalesListType  : Object, Enumeration 
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
	public interface ExportDocument  : Object, Document 
	{
	}
	public interface Fee  : Object, OrderAdjustment 
	{
	}
	public interface FinancialAccountAdjustment  : Object, FinancialAccountTransaction 
	{
	}
	public interface FinancialTerm  : Object, AgreementTerm 
	{
	}
	public interface FinishedGood  : Object, Part 
	{
	}
	public interface FiscalYearInvoiceNumber  : Object 
	{
					global::System.Int32 NextSalesInvoiceNumber {set;}

					global::System.Int32 FiscalYear {set;}

	}
	public interface Floor  : Object, Facility 
	{
	}
	public interface GenderType  : Object, Enumeration 
	{
	}
	public interface GeneralLedgerAccount  : Object, AccessControlledObject, UniquelyIdentifiable 
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
	public interface GlBudgetAllocation  : Object, AccessControlledObject 
	{
					GeneralLedgerAccount GeneralLedgerAccount {set;}

					BudgetItem BudgetItem {set;}

					global::System.Decimal AllocationPercentage {set;}

	}
	public interface GoodOrderItem  : Object, EngagementItem 
	{
					global::System.Decimal? Price {set;}

					global::System.Int32? Quantity {set;}

	}
	public interface HazardousMaterialsDocument  : Object, Document 
	{
	}
	public interface Hobby  : Object, Enumeration 
	{
	}
	public interface Incentive  : Object, AgreementTerm 
	{
	}
	public interface IndustryClassification  : Object, OrganisationClassification 
	{
	}
	public interface InternalOrganisationAccountingPreference  : Object, AccessControlledObject 
	{
					GeneralLedgerAccount GeneralLedgerAccount {set;}

					InventoryItemKind InventoryItemKind {set;}

					PaymentMethod PaymentMethod {set;}

					Receipt Receipt {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface InternalOrganisationRevenue  : Object, AccessControlledObject, Deletable 
	{
					global::System.Int32 Month {set;}

					global::System.Int32 Year {set;}

					global::System.Decimal Revenue {set;}

					Currency Currency {set;}

					global::System.String PartyName {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface InternalOrganisationRevenueHistory  : Object, AccessControlledObject 
	{
					InternalOrganisation InternalOrganisation {set;}

					global::System.Decimal? AllorsDecimal {set;}

					Currency Currency {set;}

					global::System.Decimal? Revenue {set;}

	}
	public interface InternalRequirement  : Object, Requirement 
	{
	}
	public interface InventoryItemKind  : Object, Enumeration 
	{
	}
	public interface InventoryItemVariance  : Object, Commentable, AccessControlledObject 
	{
					global::System.Int32 Quantity {set;}

					ItemVarianceAccountingTransaction ItemVarianceAccountingTransaction {set;}

					global::System.DateTime? InventoryDate {set;}

					VarianceReason Reason {set;}

	}
	public interface InvestmentAccount  : Object, FinancialAccount 
	{
					global::System.String Name {set;}

	}
	public interface InvoiceSequence  : Object, Enumeration 
	{
	}
	public interface InvoiceVatRateItem  : Object, AccessControlledObject 
	{
					global::System.Decimal? BaseAmount {set;}

					VatRate VatRates {set;}

					global::System.Decimal? VatAmount {set;}

	}
	public interface ItemIssuance  : Object, Deletable, AccessControlledObject 
	{
					global::System.DateTime? IssuanceDateTime {set;}

					InventoryItem InventoryItem {set;}

					global::System.Decimal Quantity {set;}

					ShipmentItem ShipmentItem {set;}

					PickListItem PickListItem {set;}

	}
	public interface ItemVarianceAccountingTransaction  : Object, AccountingTransaction 
	{
	}
	public interface Journal  : Object, AccessControlledObject 
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
	public interface JournalEntry  : Object, Transitional, AccessControlledObject 
	{
					global::System.String Description {set;}

					global::System.Int32? EntryNumber {set;}

					global::System.DateTime? EntryDate {set;}

					global::System.DateTime? JournalDate {set;}

					JournalEntryDetail JournalEntryDetails {set;}

	}
	public interface JournalEntryDetail  : Object, AccessControlledObject 
	{
					OrganisationGlAccount GeneralLedgerAccount {set;}

					global::System.Decimal? Amount {set;}

					global::System.Boolean? Debit {set;}

	}
	public interface JournalEntryNumber  : Object, AccessControlledObject 
	{
					JournalType JournalType {set;}

					global::System.Int32? Number {set;}

					global::System.Int32? Year {set;}

	}
	public interface JournalType  : Object, Enumeration 
	{
	}
	public interface LegalForm  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

	}
	public interface LegalTerm  : Object, AgreementTerm 
	{
	}
	public interface LetterCorrespondence  : Object, CommunicationEvent 
	{
					PostalAddress PostalAddresses {set;}

					Party Originator {set;}

					Party Receivers {set;}

	}
	public interface Lot  : Object, AccessControlledObject 
	{
					global::System.DateTime? ExpirationDate {set;}

					global::System.Int32? Quantity {set;}

					global::System.String LotNumber {set;}

	}
	public interface Maintenance  : Object, WorkEffort 
	{
	}
	public interface Manifest  : Object, Document 
	{
	}
	public interface ManufacturingBom  : Object, PartBillOfMaterial 
	{
	}
	public interface ManufacturingConfiguration  : Object, InventoryItemConfiguration 
	{
	}
	public interface MaritalStatus  : Object, Enumeration 
	{
	}
	public interface MarketingMaterial  : Object, Document 
	{
	}
	public interface MarketingPackage  : Object, ProductAssociation 
	{
					global::System.String Instruction {set;}

					Product ProductsUsedIn {set;}

					Product Product {set;}

					global::System.String Description {set;}

					global::System.Int32? QuantityUsed {set;}

	}
	public interface Model  : Object, ProductFeature, Enumeration 
	{
	}
	public interface NeededSkill  : Object, AccessControlledObject 
	{
					SkillLevel SkillLevel {set;}

					global::System.Decimal? YearsExperience {set;}

					Skill Skill {set;}

	}
	public interface NewsItem  : Object, AccessControlledObject 
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
	public interface NonSerializedInventoryItem  : Object, InventoryItem 
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
	public interface NonSerializedInventoryItemObjectState  : Object, ObjectState 
	{
	}
	public interface NonSerializedInventoryItemStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					NonSerializedInventoryItemObjectState NonSerializedInventoryItemObjectState {set;}

	}
	public interface Note  : Object, ExternalAccountingTransaction 
	{
	}
	public interface Obligation  : Object, ExternalAccountingTransaction 
	{
	}
	public interface Office  : Object, Facility 
	{
	}
	public interface OperatingCondition  : Object, PartSpecification 
	{
	}
	public interface OrderItemBilling  : Object 
	{
					OrderItem OrderItem {set;}

					SalesInvoiceItem SalesInvoiceItem {set;}

					global::System.Decimal Amount {set;}

					global::System.Decimal? Quantity {set;}

	}
	public interface OrderKind  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.String Description {set;}

					global::System.Boolean ScheduleManually {set;}

	}
	public interface OrderQuantityBreak  : Object, AccessControlledObject 
	{
					global::System.Decimal? ThroughAmount {set;}

					global::System.Decimal? FromAmount {set;}

	}
	public interface OrderRequirementCommitment  : Object, AccessControlledObject 
	{
					global::System.Int32 Quantity {set;}

					OrderItem OrderItem {set;}

					Requirement Requirement {set;}

	}
	public interface OrderShipment  : Object, Deletable 
	{
					SalesOrderItem SalesOrderItem {set;}

					global::System.Boolean Picked {set;}

					ShipmentItem ShipmentItem {set;}

					global::System.Decimal Quantity {set;}

					PurchaseOrderItem PurchaseOrderItem {set;}

	}
	public interface OrderTerm  : Object, AccessControlledObject 
	{
					global::System.String TermValue {set;}

					TermType TermType {set;}

	}
	public interface OrderValue  : Object, AccessControlledObject 
	{
					global::System.Decimal? ThroughAmount {set;}

					global::System.Decimal? FromAmount {set;}

	}
	public interface Ordinal  : Object, Enumeration 
	{
	}
	public interface OrganisationContactKind  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					global::System.String Description {set;}

	}
	public interface OrganisationContactRelationship  : Object, PartyRelationship 
	{
					Person Contact {set;}

					Organisation Organisation {set;}

					OrganisationContactKind ContactKinds {set;}

	}
	public interface OrganisationGlAccount  : Object, Period, AccessControlledObject 
	{
					Product Product {set;}

					OrganisationGlAccount Parent {set;}

					Party Party {set;}

					global::System.Boolean HasBankStatementTransactions {set;}

					ProductCategory ProductCategory {set;}

					InternalOrganisation InternalOrganisation {set;}

					GeneralLedgerAccount GeneralLedgerAccount {set;}

	}
	public interface OrganisationGlAccountBalance  : Object, AccessControlledObject 
	{
					OrganisationGlAccount OrganisationGlAccount {set;}

					global::System.Decimal Amount {set;}

					AccountingPeriod AccountingPeriod {set;}

	}
	public interface OrganisationRollUp  : Object, PartyRelationship 
	{
					Organisation Parent {set;}

					OrganisationUnit RollupKind {set;}

					Organisation Child {set;}

	}
	public interface OrganisationUnit  : Object, Enumeration 
	{
	}
	public interface OwnBankAccount  : Object, PaymentMethod, FinancialAccount 
	{
					BankAccount BankAccount {set;}

	}
	public interface OwnCreditCard  : Object, PaymentMethod, FinancialAccount 
	{
					Person Owner {set;}

					CreditCard CreditCard {set;}

	}
	public interface Package  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					global::System.String Name {set;}

	}
	public interface PackageQuantityBreak  : Object, AccessControlledObject 
	{
					global::System.Int32? From {set;}

					global::System.Int32? Through {set;}

	}
	public interface PackageRevenue  : Object, AccessControlledObject, Deletable 
	{
					global::System.Decimal Revenue {set;}

					global::System.Int32 Year {set;}

					global::System.Int32 Month {set;}

					Currency Currency {set;}

					global::System.String PackageName {set;}

					InternalOrganisation InternalOrganisation {set;}

					Package Package {set;}

	}
	public interface PackageRevenueHistory  : Object, AccessControlledObject 
	{
					InternalOrganisation InternalOrganisation {set;}

					global::System.Decimal? Revenue {set;}

					Package Package {set;}

					Currency Currency {set;}

	}
	public interface PackagingContent  : Object, AccessControlledObject 
	{
					ShipmentItem ShipmentItem {set;}

					global::System.Decimal Quantity {set;}

	}
	public interface PackagingSlip  : Object, Document 
	{
	}
	public interface PartBillOfMaterialSubstitute  : Object, AccessControlledObject, Commentable, Period 
	{
					PartBillOfMaterial SubstitutionPartBillOfMaterial {set;}

					Ordinal Preference {set;}

					global::System.Int32? Quantity {set;}

					PartBillOfMaterial PartBillOfMaterial {set;}

	}
	public interface Partnership  : Object, PartyRelationship 
	{
					InternalOrganisation InternalOrganisation {set;}

					Organisation Partner {set;}

	}
	public interface PartRevision  : Object, AccessControlledObject, Period 
	{
					global::System.String Reason {set;}

					Part SupersededByPart {set;}

					Part Part {set;}

	}
	public interface PartSpecificationObjectState  : Object, ObjectState 
	{
	}
	public interface PartSpecificationStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					PartSpecificationObjectState PartSpecificationObjectState {set;}

	}
	public interface PartSubstitute  : Object, Commentable, AccessControlledObject 
	{
					Part SubstitutionPart {set;}

					Ordinal Preference {set;}

					global::System.DateTime? FromDate {set;}

					global::System.Int32 Quantity {set;}

					Part Part {set;}

	}
	public interface PartyBenefit  : Object, AccessControlledObject 
	{
					TimeFrequency TimeFrequency {set;}

					global::System.Decimal? Cost {set;}

					global::System.Decimal? ActualEmployerPaidPercentage {set;}

					Benefit Benefit {set;}

					global::System.Decimal? ActualAvailableTime {set;}

					Employment Employment {set;}

	}
	public interface PartyContactMechanism  : Object, Period, Commentable, AccessControlledObject 
	{
					ContactMechanismPurpose ContactPurpose {set;}

					ContactMechanism ContactMechanism {set;}

					global::System.Boolean UseAsDefault {set;}

					global::System.Boolean? NonSolicitationIndicator {set;}

	}
	public interface PartyFixedAssetAssignment  : Object, Period, AccessControlledObject, Commentable 
	{
					FixedAsset FixedAsset {set;}

					Party Party {set;}

					AssetAssignmentStatus AssetAssignmentStatus {set;}

					global::System.Decimal? AllocatedCost {set;}

	}
	public interface PartyPackageRevenue  : Object, Deletable, AccessControlledObject 
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
	public interface PartyPackageRevenueHistory  : Object, AccessControlledObject 
	{
					Package Package {set;}

					InternalOrganisation InternalOrganisation {set;}

					Currency Currency {set;}

					Party Party {set;}

					global::System.Decimal? Revenue {set;}

	}
	public interface PartyProductCategoryRevenue  : Object, AccessControlledObject, Deletable 
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
	public interface PartyProductCategoryRevenueHistory  : Object, AccessControlledObject 
	{
					ProductCategory ProductCategory {set;}

					Party Party {set;}

					global::System.Decimal Quantity {set;}

					global::System.Decimal Revenue {set;}

					InternalOrganisation InternalOrganisation {set;}

					Currency Currency {set;}

	}
	public interface PartyProductRevenue  : Object, Deletable, AccessControlledObject 
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
	public interface PartyProductRevenueHistory  : Object, AccessControlledObject 
	{
					global::System.Decimal? Revenue {set;}

					Party Party {set;}

					Product Product {set;}

					global::System.Decimal? Quantity {set;}

					InternalOrganisation InternalOrganisation {set;}

					Currency Currency {set;}

	}
	public interface PartyRelationshipStatus  : Object, Enumeration 
	{
	}
	public interface PartyRevenue  : Object, AccessControlledObject, Deletable 
	{
					Currency Currency {set;}

					global::System.String PartyName {set;}

					global::System.Int32 Month {set;}

					Party Party {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.Int32 Year {set;}

					global::System.Decimal Revenue {set;}

	}
	public interface PartyRevenueHistory  : Object, AccessControlledObject 
	{
					Currency Currency {set;}

					global::System.Decimal Revenue {set;}

					Party Party {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface PartySkill  : Object, AccessControlledObject 
	{
					global::System.Decimal? YearsExperience {set;}

					global::System.DateTime? StartedUsingDate {set;}

					SkillRating SkillRating {set;}

					SkillLevel SkillLevel {set;}

					Skill Skill {set;}

	}
	public interface Passport  : Object, AccessControlledObject 
	{
					global::System.DateTime? IssueDate {set;}

					global::System.DateTime? ExpiriationDate {set;}

					global::System.String Number {set;}

	}
	public interface PayCheck  : Object, Payment 
	{
					Deduction Deductions {set;}

					Employment Employment {set;}

	}
	public interface PayGrade  : Object, AccessControlledObject, Commentable 
	{
					global::System.String Name {set;}

					SalaryStep SalarySteps {set;}

	}
	public interface PayHistory  : Object, AccessControlledObject, Period 
	{
					Employment Employment {set;}

					TimeFrequency TimeFrequency {set;}

					SalaryStep SalaryStep {set;}

					global::System.Decimal? Amount {set;}

	}
	public interface PaymentApplication  : Object, AccessControlledObject 
	{
					global::System.Decimal AmountApplied {set;}

					InvoiceItem InvoiceItem {set;}

					Invoice Invoice {set;}

					BillingAccount BillingAccount {set;}

	}
	public interface PaymentBudgetAllocation  : Object, AccessControlledObject 
	{
					Payment Payment {set;}

					BudgetItem BudgetItem {set;}

					global::System.Decimal Amount {set;}

	}
	public interface PayrollPreference  : Object, AccessControlledObject 
	{
					global::System.Decimal? Percentage {set;}

					global::System.String AccountNumber {set;}

					PaymentMethod PaymentMethod {set;}

					TimeFrequency TimeFrequency {set;}

					DeductionType DeductionType {set;}

					global::System.Decimal? Amount {set;}

	}
	public interface PerformanceNote  : Object, Commentable, AccessControlledObject 
	{
					global::System.String Description {set;}

					global::System.DateTime? CommunicationDate {set;}

					Person GivenByManager {set;}

					Person Employee {set;}

	}
	public interface PerformanceReview  : Object, AccessControlledObject, Commentable, Period 
	{
					Person Manager {set;}

					PayHistory PayHistory {set;}

					PayCheck BonusPayCheck {set;}

					PerformanceReviewItem PerformanceReviewItems {set;}

					Person Employee {set;}

					Position ResultingPosition {set;}

	}
	public interface PerformanceReviewItem  : Object, AccessControlledObject, Commentable 
	{
					RatingType RatingType {set;}

					PerformanceReviewItemType PerformanceReviewItemType {set;}

	}
	public interface PerformanceReviewItemType  : Object, Enumeration 
	{
	}
	public interface PerformanceSpecification  : Object, PartSpecification 
	{
	}
	public interface PersonalTitle  : Object, Enumeration 
	{
	}
	public interface Phase  : Object, WorkEffort 
	{
	}
	public interface PhoneCommunication  : Object, CommunicationEvent 
	{
					global::System.Boolean? LeftVoiceMail {set;}

					global::System.Boolean IncomingCall {set;}

					Party Receivers {set;}

					Party Callers {set;}

	}
	public interface PickListItem  : Object, Deletable, AccessControlledObject 
	{
					global::System.Decimal RequestedQuantity {set;}

					InventoryItem InventoryItem {set;}

					global::System.Decimal? ActualQuantity {set;}

	}
	public interface PickListObjectState  : Object, ObjectState 
	{
	}
	public interface PickListStatus  : Object, AccessControlledObject 
	{
					PickListObjectState PickListObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface Plant  : Object, Facility 
	{
	}
	public interface Position  : Object, AccessControlledObject 
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
	public interface PositionFulfillment  : Object, Period, Commentable, AccessControlledObject 
	{
					Position Position {set;}

					Person Person {set;}

	}
	public interface PositionReportingStructure  : Object, Commentable, AccessControlledObject 
	{
					global::System.Boolean? Primary {set;}

					Position ManagedByPosition {set;}

					Position Position {set;}

	}
	public interface PositionResponsibility  : Object, AccessControlledObject, Commentable 
	{
					Position Position {set;}

					Responsibility Responsibility {set;}

	}
	public interface PositionStatus  : Object, Enumeration 
	{
	}
	public interface PositionType  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

					Responsibility Responsibilities {set;}

					global::System.Decimal? BenefitPercentage {set;}

					global::System.String Title {set;}

					PositionTypeRate PositionTypeRate {set;}

	}
	public interface PositionTypeRate  : Object, AccessControlledObject 
	{
					global::System.Decimal Rate {set;}

					RateType RateType {set;}

					TimeFrequency TimeFrequency {set;}

	}
	public interface PostalAddress  : Object, ContactMechanism, GeoLocatable, AccessControlledObject 
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
	public interface PostalBoundary  : Object, AccessControlledObject 
	{
					global::System.String PostalCode {set;}

					global::System.String Locality {set;}

					Country Country {set;}

					global::System.String Region {set;}

	}
	public interface PostalCode  : Object, AccessControlledObject, CountryBound, GeographicBoundary 
	{
					global::System.String Code {set;}

	}
	public interface Priority  : Object, Enumeration 
	{
	}
	public interface ProductCategory  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					Package Package {set;}

					global::System.String Code {set;}

					Media NoImageAvailableImage {set;}

					ProductCategory Parents {set;}

					ProductCategory Children {set;}

					global::System.String Description {set;}

					global::System.String Name {set;}

					Media CategoryImage {set;}

					ProductCategory Ancestors {set;}

	}
	public interface ProductCategoryRevenue  : Object, Deletable, AccessControlledObject 
	{
					global::System.String ProductCategoryName {set;}

					global::System.Int32 Month {set;}

					InternalOrganisation InternalOrganisation {set;}

					ProductCategory ProductCategory {set;}

					global::System.Decimal Revenue {set;}

					Currency Currency {set;}

					global::System.Int32 Year {set;}

	}
	public interface ProductCategoryRevenueHistory  : Object, AccessControlledObject 
	{
					Currency Currency {set;}

					global::System.Decimal? Revenue {set;}

					InternalOrganisation InternalOrganisation {set;}

					ProductCategory ProductCategory {set;}

	}
	public interface ProductConfiguration  : Object, ProductAssociation 
	{
					Product ProductsUsedIn {set;}

					Product Product {set;}

					global::System.Decimal? QuantityUsed {set;}

					global::System.String Description {set;}

	}
	public interface ProductDeliverySkillRequirement  : Object, AccessControlledObject 
	{
					global::System.DateTime? StartedUsingDate {set;}

					Service Service {set;}

					Skill Skill {set;}

	}
	public interface ProductDrawing  : Object, Document 
	{
	}
	public interface ProductFeatureApplicabilityRelationship  : Object, AccessControlledObject 
	{
					Product AvailableFor {set;}

					ProductFeature UsedToDefine {set;}

	}
	public interface ProductionRun  : Object, WorkEffort 
	{
					global::System.Int32? QuantityProduced {set;}

					global::System.Int32? QuantityRejected {set;}

					global::System.Int32? QuantityToProduce {set;}

	}
	public interface ProductModel  : Object, Document 
	{
	}
	public interface ProductPurchasePrice  : Object, Period, AccessControlledObject 
	{
					global::System.Decimal Price {set;}

					UnitOfMeasure UnitOfMeasure {set;}

					Currency Currency {set;}

	}
	public interface ProductQuality  : Object, ProductFeature, Enumeration 
	{
	}
	public interface ProductQuote  : Object, Quote 
	{
	}
	public interface ProductRequirement  : Object, Requirement 
	{
					Product Product {set;}

					DesiredProductFeature DesiredProductFeatures {set;}

	}
	public interface ProductRevenue  : Object, AccessControlledObject, Deletable 
	{
					global::System.Decimal Revenue {set;}

					global::System.String ProductName {set;}

					Currency Currency {set;}

					global::System.Int32 Year {set;}

					Product Product {set;}

					global::System.Int32 Month {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface ProductRevenueHistory  : Object, AccessControlledObject 
	{
					InternalOrganisation InternalOrganisation {set;}

					global::System.Decimal? Revenue {set;}

					Currency Currency {set;}

					Product Product {set;}

	}
	public interface ProfessionalAssignment  : Object, AccessControlledObject, Period 
	{
					Person Professional {set;}

					EngagementItem EngagementItem {set;}

	}
	public interface ProfessionalPlacement  : Object, EngagementItem 
	{
	}
	public interface ProfessionalServicesRelationship  : Object, PartyRelationship 
	{
					Person Professional {set;}

					Organisation ProfessionalServicesProvider {set;}

	}
	public interface Program  : Object, WorkEffort 
	{
	}
	public interface Project  : Object, WorkEffort 
	{
	}
	public interface ProjectRequirement  : Object, Requirement 
	{
					Deliverable NeededDeliverables {set;}

	}
	public interface Property  : Object, FixedAsset 
	{
	}
	public interface Proposal  : Object, Quote 
	{
	}
	public interface ProspectRelationship  : Object, PartyRelationship 
	{
					InternalOrganisation InternalOrganisation {set;}

					Party Prospect {set;}

	}
	public interface Province  : Object, CountryBound, CityBound, GeographicBoundary, AccessControlledObject 
	{
					global::System.String Name {set;}

	}
	public interface PurchaseAgreement  : Object, Agreement 
	{
	}
	public interface PurchaseInvoiceItemObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseInvoiceItemStatus  : Object, AccessControlledObject 
	{
					PurchaseInvoiceItemObjectState PurchaseInvoiceItemObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface PurchaseInvoiceItemType  : Object, Enumeration 
	{
	}
	public interface PurchaseInvoiceObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseInvoiceStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime? StartDateTime {set;}

					PurchaseInvoiceObjectState PurchaseInvoiceObjectState {set;}

	}
	public interface PurchaseInvoiceType  : Object, Enumeration 
	{
	}
	public interface PurchaseOrder  : Object, Order 
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
	public interface PurchaseOrderItemObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseOrderItemStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					PurchaseOrderItemObjectState PurchaseOrderItemObjectState {set;}

	}
	public interface PurchaseOrderObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseOrderStatus  : Object, AccessControlledObject 
	{
					PurchaseOrderObjectState PurchaseOrderObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface PurchaseReturnObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseReturnStatus  : Object, AccessControlledObject 
	{
					PurchaseReturnObjectState PurchaseReturnObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface PurchaseShipment  : Object, Shipment 
	{
					PurchaseShipmentObjectState CurrentObjectState {set;}

					Facility Facility {set;}

					PurchaseShipmentStatus ShipmentStatuses {set;}

					PurchaseShipmentStatus CurrentShipmentStatus {set;}

					PurchaseOrder PurchaseOrder {set;}

	}
	public interface PurchaseShipmentObjectState  : Object, ObjectState 
	{
	}
	public interface PurchaseShipmentStatus  : Object, AccessControlledObject 
	{
					PurchaseShipmentObjectState PurchaseShipmentObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface Qualification  : Object, Enumeration 
	{
	}
	public interface QuoteItem  : Object, Commentable, AccessControlledObject 
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
	public interface QuoteTerm  : Object, AccessControlledObject 
	{
					global::System.String TermValue {set;}

					TermType TermType {set;}

	}
	public interface RateType  : Object, Enumeration 
	{
	}
	public interface RatingType  : Object, Enumeration 
	{
	}
	public interface RawMaterial  : Object, Part 
	{
	}
	public interface ReceiptAccountingTransaction  : Object, ExternalAccountingTransaction 
	{
					Receipt Receipt {set;}

	}
	public interface Region  : Object, GeographicBoundaryComposite, AccessControlledObject 
	{
					global::System.String Name {set;}

	}
	public interface RequestForInformation  : Object, Request 
	{
	}
	public interface RequestForProposal  : Object, Request 
	{
	}
	public interface RequestForQuote  : Object, Request 
	{
	}
	public interface RequestItem  : Object, Commentable, AccessControlledObject 
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
	public interface RequirementBudgetAllocation  : Object, AccessControlledObject 
	{
					BudgetItem BudgetItem {set;}

					Requirement Requirement {set;}

					global::System.Decimal Amount {set;}

	}
	public interface RequirementCommunication  : Object, AccessControlledObject 
	{
					CommunicationEvent CommunicationEvent {set;}

					Requirement Requirement {set;}

					Person AssociatedProfessional {set;}

	}
	public interface RequirementObjectState  : Object, ObjectState 
	{
	}
	public interface RequirementStatus  : Object, AccessControlledObject 
	{
					RequirementObjectState RequirementObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface Research  : Object, WorkEffort 
	{
	}
	public interface ResourceRequirement  : Object, Requirement 
	{
					global::System.String Duties {set;}

					global::System.Decimal? NumberOfPositions {set;}

					global::System.DateTime? RequiredStartDate {set;}

					NeededSkill NeededSkills {set;}

					global::System.DateTime? RequiredEndDate {set;}

	}
	public interface RespondingParty  : Object, AccessControlledObject 
	{
					global::System.DateTime? SendingDate {set;}

					ContactMechanism ContactMechanism {set;}

					Party Party {set;}

	}
	public interface Responsibility  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

	}
	public interface Resume  : Object, AccessControlledObject 
	{
					global::System.DateTime ResumeDate {set;}

					global::System.String ResumeText {set;}

	}
	public interface RevenueQuantityBreak  : Object, AccessControlledObject 
	{
					ProductCategory ProductCategory {set;}

					global::System.Decimal? Through {set;}

					global::System.Decimal? From {set;}

	}
	public interface RevenueValueBreak  : Object, AccessControlledObject 
	{
					ProductCategory ProductCategory {set;}

					global::System.Decimal? ThroughAmount {set;}

					global::System.Decimal? FromAmount {set;}

	}
	public interface Room  : Object, Facility, Container 
	{
	}
	public interface SalaryStep  : Object, AccessControlledObject 
	{
					global::System.DateTime ModifiedDate {set;}

					global::System.Decimal Amount {set;}

	}
	public interface SalesAccountingTransaction  : Object, ExternalAccountingTransaction 
	{
					Invoice Invoice {set;}

	}
	public interface SalesAgreement  : Object, Agreement 
	{
	}
	public interface SalesChannel  : Object, Enumeration 
	{
	}
	public interface SalesChannelRevenue  : Object, AccessControlledObject, Deletable 
	{
					global::System.Int32 Year {set;}

					global::System.Int32 Month {set;}

					Currency Currency {set;}

					SalesChannel SalesChannel {set;}

					global::System.String SalesChannelName {set;}

					global::System.Decimal Revenue {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface SalesChannelRevenueHistory  : Object, AccessControlledObject 
	{
					SalesChannel SalesChannel {set;}

					Currency Currency {set;}

					global::System.Decimal? Revenue {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface SalesInvoiceItem  : Object, InvoiceItem 
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
	public interface SalesInvoiceItemObjectState  : Object, ObjectState 
	{
	}
	public interface SalesInvoiceItemStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					SalesInvoiceItemObjectState SalesInvoiceItemObjectState {set;}

	}
	public interface SalesInvoiceItemType  : Object, Enumeration 
	{
	}
	public interface SalesInvoiceObjectState  : Object, ObjectState 
	{
	}
	public interface SalesInvoiceStatus  : Object, AccessControlledObject 
	{
					SalesInvoiceObjectState SalesInvoiceObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface SalesInvoiceType  : Object, Enumeration 
	{
	}
	public interface SalesOrderItemObjectState  : Object, ObjectState 
	{
	}
	public interface SalesOrderItemStatus  : Object, AccessControlledObject 
	{
					SalesOrderItemObjectState SalesOrderItemObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface SalesOrderObjectState  : Object, ObjectState 
	{
	}
	public interface SalesOrderStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					SalesOrderObjectState SalesOrderObjectState {set;}

	}
	public interface SalesRepCommission  : Object, AccessControlledObject 
	{
					global::System.Decimal? Commission {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.String SalesRepName {set;}

					global::System.Int32? Month {set;}

					global::System.Int32 Year {set;}

					Currency Currency {set;}

					Person SalesRep {set;}

	}
	public interface SalesRepPartyProductCategoryRevenue  : Object, Deletable, AccessControlledObject 
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
	public interface SalesRepPartyRevenue  : Object, AccessControlledObject, Deletable 
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
	public interface SalesRepProductCategoryRevenue  : Object, Deletable, AccessControlledObject 
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
	public interface SalesRepRelationship  : Object, Commentable, Period, PartyRelationship, AccessControlledObject 
	{
					Person SalesRepresentative {set;}

					global::System.Decimal LastYearsCommission {set;}

					ProductCategory ProductCategories {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.Decimal YTDCommission {set;}

					Party Customer {set;}

	}
	public interface SalesRepRevenue  : Object, Deletable, AccessControlledObject 
	{
					global::System.Decimal Revenue {set;}

					Currency Currency {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.Int32 Month {set;}

					global::System.String SalesRepName {set;}

					global::System.Int32 Year {set;}

					Person SalesRep {set;}

	}
	public interface SalesRepRevenueHistory  : Object, AccessControlledObject 
	{
					Currency Currency {set;}

					Person SalesRep {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.Decimal? Revenue {set;}

	}
	public interface SalesTerritory  : Object, AccessControlledObject, GeographicBoundaryComposite 
	{
					global::System.String Name {set;}

	}
	public interface Salutation  : Object, AccessControlledObject, Enumeration, UniquelyIdentifiable 
	{
	}
	public interface SerializedInventoryItem  : Object, InventoryItem 
	{
					SerializedInventoryItemStatus InventoryItemStatuses {set;}

					global::System.String SerialNumber {set;}

					SerializedInventoryItemObjectState CurrentObjectState {set;}

					SerializedInventoryItemStatus CurrentInventoryItemStatus {set;}

	}
	public interface SerializedInventoryItemObjectState  : Object, ObjectState 
	{
	}
	public interface SerializedInventoryItemStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					SerializedInventoryItemObjectState SerializedInventoryItemObjectState {set;}

	}
	public interface ServiceConfiguration  : Object, InventoryItemConfiguration 
	{
	}
	public interface ServiceEntryBilling  : Object 
	{
					ServiceEntry ServiceEntry {set;}

					InvoiceItem InvoiceItem {set;}

	}
	public interface ServiceEntryHeader  : Object, Period, AccessControlledObject 
	{
					ServiceEntry ServiceEntries {set;}

					global::System.DateTime SubmittedDate {set;}

					Person SubmittedBy {set;}

	}
	public interface ServiceFeature  : Object, ProductFeature, Enumeration 
	{
	}
	public interface ServiceTerritory  : Object, AccessControlledObject, GeographicBoundaryComposite 
	{
					global::System.String Name {set;}

	}
	public interface Shelf  : Object, Container 
	{
	}
	public interface ShipmentItem  : Object, AccessControlledObject, Deletable 
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
	public interface ShipmentMethod  : Object, Enumeration 
	{
	}
	public interface ShipmentPackage  : Object, AccessControlledObject, UniquelyIdentifiable 
	{
					PackagingContent PackagingContents {set;}

					Document Documents {set;}

					global::System.DateTime CreationDate {set;}

					global::System.Int32 SequenceNumber {set;}

	}
	public interface ShipmentReceipt  : Object, AccessControlledObject 
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
	public interface ShipmentRouteSegment  : Object, AccessControlledObject 
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
	public interface ShipmentValue  : Object, AccessControlledObject 
	{
					global::System.Decimal? ThroughAmount {set;}

					global::System.Decimal? FromAmount {set;}

	}
	public interface ShippingAndHandlingCharge  : Object, OrderAdjustment 
	{
	}
	public interface ShippingAndHandlingComponent  : Object, AccessControlledObject, Period 
	{
					global::System.Decimal? Cost {set;}

					ShipmentMethod ShipmentMethod {set;}

					Carrier Carrier {set;}

					ShipmentValue ShipmentValue {set;}

					Currency Currency {set;}

					InternalOrganisation SpecifiedFor {set;}

					GeographicBoundary GeographicBoundary {set;}

	}
	public interface Size  : Object, ProductFeature, Enumeration 
	{
	}
	public interface Skill  : Object, Enumeration 
	{
	}
	public interface SkillLevel  : Object, Enumeration 
	{
	}
	public interface SkillRating  : Object, Enumeration 
	{
	}
	public interface SoftwareFeature  : Object, ProductFeature, Enumeration 
	{
	}
	public interface StandardServiceOrderItem  : Object, EngagementItem 
	{
	}
	public interface State  : Object, CityBound, GeographicBoundary, AccessControlledObject, CountryBound 
	{
					global::System.String Name {set;}

	}
	public interface StatementOfWork  : Object, Quote 
	{
	}
	public interface Store  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.Decimal ShipmentThreshold {set;}

					Counter SalesOrderCounter {set;}

					StringTemplate SalesInvoiceTemplates {set;}

					global::System.String OutgoingShipmentNumberPrefix {set;}

					global::System.String SalesInvoiceNumberPrefix {set;}

					global::System.Int32 PaymentGracePeriod {set;}

					Media LogoImage {set;}

					global::System.Int32 PaymentNetDays {set;}

					Facility DefaultFacility {set;}

					global::System.String Name {set;}

					StringTemplate SalesOrderTemplates {set;}

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

					StringTemplate CustomerShipmentTemplates {set;}

	}
	public interface StoreRevenue  : Object, AccessControlledObject, Deletable 
	{
					InternalOrganisation InternalOrganisation {set;}

					global::System.Int32 Month {set;}

					Currency Currency {set;}

					Store Store {set;}

					global::System.String StoreName {set;}

					global::System.Decimal Revenue {set;}

					global::System.Int32 Year {set;}

	}
	public interface StoreRevenueHistory  : Object, AccessControlledObject 
	{
					InternalOrganisation InternalOrganisation {set;}

					Currency Currency {set;}

					Store Store {set;}

					global::System.Decimal? Revenue {set;}

	}
	public interface SubAgreement  : Object, AgreementItem 
	{
	}
	public interface SubAssembly  : Object, Part 
	{
	}
	public interface SubContractorAgreement  : Object, Agreement 
	{
	}
	public interface SubContractorRelationship  : Object, PartyRelationship 
	{
					Party Contractor {set;}

					Party SubContractor {set;}

	}
	public interface SupplierOffering  : Object, Commentable, Period, AccessControlledObject 
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
	public interface SupplierRelationship  : Object, PartyRelationship 
	{
					Organisation Supplier {set;}

					global::System.Int32 SubAccountNumber {set;}

					global::System.DateTime? LastReminderDate {set;}

					DunningType DunningType {set;}

					InternalOrganisation InternalOrganisation {set;}

					global::System.DateTime? BlockedForDunning {set;}

	}
	public interface SurchargeAdjustment  : Object, OrderAdjustment 
	{
	}
	public interface SurchargeComponent  : Object, PriceComponent 
	{
					global::System.Decimal? Percentage {set;}

	}
	public interface Task  : Object, WorkEffort 
	{
	}
	public interface TaxDocument  : Object, Document 
	{
	}
	public interface TaxDue  : Object, ExternalAccountingTransaction 
	{
	}
	public interface TelecommunicationsNumber  : Object, ContactMechanism 
	{
					global::System.String AreaCode {set;}

					global::System.String CountryCode {set;}

					global::System.String ContactNumber {set;}

	}
	public interface TemplatePurpose  : Object, Enumeration 
	{
	}
	public interface TermType  : Object, Enumeration 
	{
	}
	public interface Territory  : Object, AccessControlledObject, CityBound, GeographicBoundary, CountryBound 
	{
					global::System.String Name {set;}

	}
	public interface TestingRequirement  : Object, PartSpecification 
	{
	}
	public interface Threshold  : Object, AgreementTerm 
	{
	}
	public interface TimeAndMaterialsService  : Object, Service 
	{
	}
	public interface TimeEntry  : Object, ServiceEntry 
	{
					global::System.Decimal Cost {set;}

					global::System.Decimal GrossMargin {set;}

					QuoteTerm QuoteTerm {set;}

					global::System.Decimal? BillingRate {set;}

					UnitOfMeasure UnitOfMeasure {set;}

					global::System.Decimal? AmountOfTime {set;}

	}
	public interface TimeFrequency  : Object, Enumeration, IUnitOfMeasure 
	{
	}
	public interface TimePeriodUsage  : Object, DeploymentUsage 
	{
	}
	public interface Tolerance  : Object, PartSpecification 
	{
	}
	public interface Training  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

	}
	public interface TransferObjectState  : Object, ObjectState 
	{
	}
	public interface TransferStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					TransferObjectState TransferObjectState {set;}

	}
	public interface UnitOfMeasure  : Object, IUnitOfMeasure, AccessControlledObject, UniquelyIdentifiable, Enumeration 
	{
	}
	public interface UnitOfMeasureConversion  : Object, AccessControlledObject 
	{
					IUnitOfMeasure ToUnitOfMeasure {set;}

					global::System.DateTime? StartDate {set;}

					global::System.Decimal ConversionFactor {set;}

	}
	public interface VarianceReason  : Object, Enumeration 
	{
	}
	public interface VatCalculationMethod  : Object, Enumeration 
	{
	}
	public interface VatForm  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					VatReturnBox VatReturnBoxes {set;}

	}
	public interface VatRate  : Object, AccessControlledObject 
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
	public interface VatRatePurchaseKind  : Object, Enumeration 
	{
	}
	public interface VatRateUsage  : Object, Enumeration 
	{
	}
	public interface VatRegime  : Object, Enumeration 
	{
					VatRate VatRate {set;}

					OrganisationGlAccount GeneralLedgerAccount {set;}

	}
	public interface VatReturnBox  : Object, AccessControlledObject 
	{
					global::System.String HeaderNumber {set;}

					global::System.String Description {set;}

	}
	public interface VatReturnBoxType  : Object, AccessControlledObject 
	{
					global::System.String Type {set;}

	}
	public interface VatTariff  : Object, Enumeration 
	{
	}
	public interface Vehicle  : Object, FixedAsset 
	{
	}
	public interface VolumeUsage  : Object, DeploymentUsage 
	{
					global::System.Decimal Quantity {set;}

					UnitOfMeasure UnitOfMeasure {set;}

	}
	public interface WebAddress  : Object, ElectronicAddress 
	{
	}
	public interface WebSiteCommunication  : Object, CommunicationEvent 
	{
					Party Originator {set;}

					Party Receiver {set;}

	}
	public interface Withdrawal  : Object, FinancialAccountTransaction 
	{
					Disbursement Disbursement {set;}

	}
	public interface WorkEffortAssignment  : Object, Period, AccessControlledObject, Commentable, Deletable 
	{
					Person Professional {set;}

					WorkEffort Assignment {set;}

	}
	public interface WorkEffortAssignmentRate  : Object, AccessControlledObject 
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
	public interface WorkEffortFixedAssetAssignment  : Object, AccessControlledObject, Period, Commentable 
	{
					AssetAssignmentStatus AssetAssignmentStatus {set;}

					WorkEffort Assignment {set;}

					global::System.Decimal? AllocatedCost {set;}

					FixedAsset FixedAsset {set;}

	}
	public interface WorkEffortFixedAssetStandard  : Object, AccessControlledObject 
	{
					global::System.Decimal? EstimatedCost {set;}

					global::System.Decimal? EstimatedDuration {set;}

					FixedAsset FixedAsset {set;}

					global::System.Int32? EstimatedQuantity {set;}

	}
	public interface WorkEffortGoodStandard  : Object, AccessControlledObject 
	{
					Good Good {set;}

					global::System.Decimal? EstimatedCost {set;}

					global::System.Int32? EstimatedQuantity {set;}

	}
	public interface WorkEffortInventoryAssignment  : Object, AccessControlledObject 
	{
					WorkEffort Assignment {set;}

					InventoryItem InventoryItem {set;}

					global::System.Int32? Quantity {set;}

	}
	public interface WorkEffortObjectState  : Object, ObjectState 
	{
	}
	public interface WorkEffortPartStandard  : Object, AccessControlledObject 
	{
					Part Part {set;}

					global::System.Decimal? EstimatedCost {set;}

					global::System.Int32? EstimatedQuantity {set;}

	}
	public interface WorkEffortPurpose  : Object, Enumeration 
	{
	}
	public interface WorkEffortSkillStandard  : Object, AccessControlledObject 
	{
					Skill Skill {set;}

					global::System.Int32? EstimatedNumberOfPeople {set;}

					global::System.Decimal? EstimatedDuration {set;}

					global::System.Decimal? EstimatedCost {set;}

	}
	public interface WorkEffortTypeKind  : Object, Enumeration 
	{
	}
	public interface WorkFlow  : Object, WorkEffort 
	{
	}
	public interface WorkRequirement  : Object, Requirement 
	{
					FixedAsset FixedAsset {set;}

					Deliverable Deliverable {set;}

					Product Product {set;}

	}
	public interface AccountingPeriod  : Object, Budget, AccessControlledObject 
	{
					AccountingPeriod Parent {set;}

					global::System.Boolean Active {set;}

					global::System.Int32 PeriodNumber {set;}

					TimeFrequency TimeFrequency {set;}

	}
	public interface AccountingTransactionDetail  : Object, AccessControlledObject 
	{
					AccountingTransactionDetail AssociatedWith {set;}

					OrganisationGlAccountBalance OrganisationGlAccountBalance {set;}

					global::System.Decimal Amount {set;}

					global::System.Boolean Debit {set;}

	}
	public interface ActivityUsage  : Object, DeploymentUsage 
	{
					global::System.Decimal Quantity {set;}

					UnitOfMeasure UnitOfMeasure {set;}

	}
	public interface Addendum  : Object, AccessControlledObject 
	{
					global::System.String Text {set;}

					global::System.DateTime? EffictiveDate {set;}

					global::System.String Description {set;}

					global::System.DateTime CreationDate {set;}

	}
	public interface AutomatedAgent  : Object, User, Party 
	{
					global::System.String Name {set;}

					global::System.String Description {set;}

	}
	public interface Bank  : Object, AccessControlledObject 
	{
					Media Logo {set;}

					global::System.String Bic {set;}

					global::System.String SwiftCode {set;}

					Country Country {set;}

					global::System.String Name {set;}

	}
	public interface BankAccount  : Object, FinancialAccount 
	{
					Bank Bank {set;}

					global::System.String NameOnAccount {set;}

					ContactMechanism ContactMechanisms {set;}

					Currency Currency {set;}

					global::System.String Iban {set;}

					global::System.String Branch {set;}

					Person ContactPersons {set;}

	}
	public interface BasePrice  : Object, PriceComponent, Deletable 
	{
	}
	public interface Benefit  : Object, AccessControlledObject 
	{
					global::System.Decimal? EmployerPaidPercentage {set;}

					global::System.String Description {set;}

					global::System.String Name {set;}

					global::System.Decimal? AvailableTime {set;}

	}
	public interface BillingAccount  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

					ContactMechanism ContactMechanism {set;}

	}
	public interface Brand  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					ProductCategory ProductCategories {set;}

	}
	public interface BudgetItem  : Object, AccessControlledObject 
	{
					global::System.String Purpose {set;}

					global::System.String Justification {set;}

					BudgetItem Children {set;}

					global::System.Decimal Amount {set;}

	}
	public interface BudgetReview  : Object, Commentable, AccessControlledObject 
	{
					global::System.DateTime ReviewDate {set;}

					global::System.String Description {set;}

	}
	public interface BudgetRevision  : Object, AccessControlledObject 
	{
					global::System.DateTime RevisionDate {set;}

	}
	public interface BudgetRevisionImpact  : Object, AccessControlledObject 
	{
					BudgetItem BudgetItem {set;}

					global::System.String Reason {set;}

					global::System.Boolean? Deleted {set;}

					global::System.Boolean? Added {set;}

					global::System.Decimal? RevisedAmount {set;}

					BudgetRevision BudgetRevision {set;}

	}
	public interface BudgetStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					BudgetObjectState BudgetObjectState {set;}

	}
	public interface CapitalBudget  : Object, Budget 
	{
	}
	public interface Carrier  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.String Name {set;}

	}
	public interface Case  : Object, AccessControlledObject, Transitional, UniquelyIdentifiable 
	{
					CaseStatus CurrentCaseStatus {set;}

					CaseStatus CaseStatuses {set;}

					global::System.DateTime? StartDate {set;}

					CaseObjectState CurrentObjectState {set;}

					global::System.String Description {set;}

	}
	public interface CaseStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					CaseObjectState CaseObjectState {set;}

	}
	public interface ChartOfAccounts  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.String Name {set;}

					GeneralLedgerAccount GeneralLedgerAccounts {set;}

	}
	public interface Citizenship  : Object, AccessControlledObject 
	{
					Passport Passports {set;}

					Country Country {set;}

	}
	public interface City  : Object, GeographicBoundary, AccessControlledObject, CountryBound 
	{
					global::System.String Name {set;}

					State State {set;}

	}
	public interface ClientRelationship  : Object, PartyRelationship 
	{
					Party Client {set;}

					InternalOrganisation InternalOrganisation {set;}

	}
	public interface CommunicationEventStatus  : Object, Deletable, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					CommunicationEventObjectState CommunicationEventObjectState {set;}

	}
	public interface CostCenter  : Object, UniquelyIdentifiable, AccessControlledObject 
	{
					global::System.String Description {set;}

					OrganisationGlAccount InternalTransferGlAccount {set;}

					CostCenterCategory CostCenterCategories {set;}

					OrganisationGlAccount RedistributedCostsGlAccount {set;}

					global::System.String Name {set;}

					global::System.Boolean? Active {set;}

					global::System.Boolean? UseGlAccountOfBooking {set;}

	}
	public interface CostCenterCategory  : Object, UniquelyIdentifiable 
	{
					CostCenterCategory Parent {set;}

					CostCenterCategory Ancestors {set;}

					CostCenterCategory Children {set;}

					global::System.String Description {set;}

	}
	public interface County  : Object, GeographicBoundary, AccessControlledObject, CityBound 
	{
					global::System.String Name {set;}

					State State {set;}

	}
	public interface CreditCard  : Object, FinancialAccount, AccessControlledObject 
	{
					global::System.String NameOnCard {set;}

					CreditCardCompany CreditCardCompany {set;}

					global::System.Int32 ExpirationYear {set;}

					global::System.Int32 ExpirationMonth {set;}

					global::System.String CardNumber {set;}

	}
	public interface CreditCardCompany  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

	}
	public interface CustomerRelationship  : Object, PartyRelationship 
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
	public interface CustomerReturn  : Object, Shipment 
	{
					CustomerReturnStatus CurrentShipmentStatus {set;}

					CustomerReturnStatus ShipmentStatuses {set;}

					CustomerReturnObjectState CurrentObjectState {set;}

	}
	public interface CustomerReturnStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					ShipmentReceipt ShipmentReceipt {set;}

					CustomerReturnObjectState CustomerReturnObjectState {set;}

	}
	public interface CustomerShipment  : Object, Shipment, Deletable 
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
	public interface CustomerShipmentStatus  : Object, AccessControlledObject 
	{
					CustomerShipmentObjectState CustomerShipmentObjectState {set;}

					global::System.DateTime StartDateTime {set;}

	}
	public interface Deduction  : Object, AccessControlledObject 
	{
					DeductionType DeductionType {set;}

					global::System.Decimal Amount {set;}

	}
	public interface Deliverable  : Object, AccessControlledObject 
	{
					global::System.String Name {set;}

					DeliverableType DeliverableType {set;}

	}
	public interface DeliverableTurnover  : Object, ServiceEntry 
	{
					global::System.Decimal Amount {set;}

	}
	public interface Depreciation  : Object, InternalAccountingTransaction 
	{
					FixedAsset FixedAsset {set;}

	}
	public interface DepreciationMethod  : Object, AccessControlledObject 
	{
					global::System.String Formula {set;}

					global::System.String Description {set;}

	}
	public interface DesiredProductFeature  : Object, AccessControlledObject 
	{
					global::System.Boolean Required {set;}

					ProductFeature ProductFeature {set;}

	}
	public interface Dimension  : Object, ProductFeature 
	{
					global::System.Decimal? Unit {set;}

					UnitOfMeasure UnitOfMeasure {set;}

	}
	public interface DisbursementAccountingTransaction  : Object, ExternalAccountingTransaction 
	{
					Disbursement Disbursement {set;}

	}
	public interface DistributionChannelRelationship  : Object, PartyRelationship 
	{
					InternalOrganisation InternalOrganisation {set;}

					Organisation Distributor {set;}

	}
	public interface DropShipment  : Object, Shipment 
	{
					DropShipmentStatus ShipmentStatuses {set;}

					DropShipmentStatus CurrentShipmentStatus {set;}

					DropShipmentObjectState CurrentObjectState {set;}

	}
	public interface DropShipmentStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					DropShipmentObjectState DropShipmentObjectState {set;}

	}
	public interface EmailCommunication  : Object, CommunicationEvent 
	{
					EmailAddress Originator {set;}

					EmailAddress Addressees {set;}

					EmailAddress CarbonCopies {set;}

					EmailAddress BlindCopies {set;}

					EmailTemplate EmailTemplate {set;}

	}
	public interface EmailTemplate  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

					global::System.String BodyTemplate {set;}

					global::System.String SubjectTemplate {set;}

	}
	public interface Employment  : Object, PartyRelationship, Deletable 
	{
					InternalOrganisation Employer {set;}

					Person Employee {set;}

					PayrollPreference PayrollPreferences {set;}

					EmploymentTerminationReason EmploymentTerminationReason {set;}

					EmploymentTermination EmploymentTermination {set;}

	}
	public interface EmploymentApplication  : Object, AccessControlledObject 
	{
					global::System.DateTime ApplicationDate {set;}

					Position Position {set;}

					EmploymentApplicationStatus EmploymentApplicationStatus {set;}

					Person Person {set;}

					EmploymentApplicationSource EmploymentApplicationSource {set;}

	}
	public interface Engagement  : Object, AccessControlledObject 
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
	public interface EngagementRate  : Object, Period, AccessControlledObject 
	{
					global::System.Decimal BillingRate {set;}

					RatingType RatingType {set;}

					global::System.Decimal? Cost {set;}

					PriceComponent GoverningPriceComponents {set;}

					global::System.String ChangeReason {set;}

					UnitOfMeasure UnitOfMeasure {set;}

	}
	public interface EngineeringChange  : Object, Transitional, AccessControlledObject 
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
	public interface EngineeringChangeStatus  : Object, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					EngineeringChangeObjectState EngineeringChangeObjectState {set;}

	}
	public interface ExpenseEntry  : Object, ServiceEntry 
	{
					global::System.Decimal Amount {set;}

	}
	public interface FaceToFaceCommunication  : Object, CommunicationEvent 
	{
					Party Participants {set;}

					global::System.String Location {set;}

	}
	public interface FaxCommunication  : Object, CommunicationEvent 
	{
					Party Originator {set;}

					Party Receiver {set;}

					TelecommunicationsNumber OutgoingFaxNumber {set;}

	}
	public interface GeneralLedgerAccountGroup  : Object, AccessControlledObject 
	{
					GeneralLedgerAccountGroup Parent {set;}

					global::System.String Description {set;}

	}
	public interface GeneralLedgerAccountType  : Object, AccessControlledObject 
	{
					global::System.String Description {set;}

	}
	public interface Good  : Object, Deletable, Product 
	{
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
	public interface InternalOrganisation  : Object, Party 
	{
					global::System.String PurchaseOrderNumberPrefix {set;}

					global::System.String TransactionReferenceNumber {set;}

					JournalEntryNumber JournalEntryNumbers {set;}

					Country EuListingState {set;}

					Counter PurchaseInvoiceCounter {set;}

					AccountingPeriod ActualAccountingPeriod {set;}

					InvoiceSequence InvoiceSequence {set;}

					PaymentMethod ActivePaymentMethods {set;}

					StringTemplate PurchaseShipmentTemplates {set;}

					global::System.Decimal? MaximumAllowedPaymentDifference {set;}

					Media LogoImage {set;}

					CostCenterSplitMethod CostCenterSplitMethod {set;}

					Counter PurchaseOrderCounter {set;}

					LegalForm LegalForm {set;}

					AccountingPeriod AccountingPeriods {set;}

					GeneralLedgerAccount SalesPaymentDifferencesAccount {set;}

					global::System.String Name {set;}

					global::System.String PurchaseTransactionReferenceNumber {set;}

					global::System.Int32 FiscalYearStartMonth {set;}

					StringTemplate PurchaseOrderTemplates {set;}

					CostOfGoodsSoldMethod CostOfGoodsSoldMethod {set;}

					Role EmployeeRoles {set;}

					global::System.Boolean? VatDeactivated {set;}

					global::System.Int32 FiscalYearStartDay {set;}

					GeneralLedgerAccount GeneralLedgerAccounts {set;}

					Counter AccountingTransactionCounter {set;}

					Counter IncomingShipmentCounter {set;}

					GeneralLedgerAccount RetainedEarningsAccount {set;}

					StringTemplate PackagingSlipTemplates {set;}

					Party Customers {set;}

					global::System.String PurchaseInvoiceNumberPrefix {set;}

					StringTemplate PickListTemplates {set;}

					GeneralLedgerAccount SalesPaymentDiscountDifferencesAccount {set;}

					Counter SubAccountCounter {set;}

					AccountingTransactionNumber AccountingTransactionNumbers {set;}

					StringTemplate QuoteTemplates {set;}

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
	public interface InvoiceTerm  : Object, AgreementTerm, AccessControlledObject 
	{
	}
	public interface ManufacturerSuggestedRetailPrice  : Object, PriceComponent 
	{
	}
	public interface MaterialsUsage  : Object, ServiceEntry 
	{
					global::System.Decimal Amount {set;}

	}
	public interface OneTimeCharge  : Object, PriceComponent 
	{
	}
	public interface OperatingBudget  : Object, Budget 
	{
	}
	public interface Organisation  : Object, Party 
	{
					LegalForm LegalForm {set;}

					global::System.String Name {set;}

					UserGroup CustomerContactUserGroup {set;}

					Image LogoImage {set;}

					UserGroup PartnerContactUserGroup {set;}

					global::System.String TaxNumber {set;}

					IndustryClassification IndustryClassification {set;}

					UserGroup SupplierContactUserGroup {set;}

					OrganisationClassification OrganisationClassifications {set;}

	}
	public interface PersonTraining  : Object, AccessControlledObject, Period 
	{
					Training Training {set;}

	}
	public interface PickList  : Object, UniquelyIdentifiable, AccessControlledObject, Printable, Transitional 
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
	public interface PurchaseInvoice  : Object, Invoice 
	{
					PurchaseInvoiceItem PurchaseInvoiceItems {set;}

					InternalOrganisation BilledToInternalOrganisation {set;}

					PurchaseInvoiceStatus CurrentInvoiceStatus {set;}

					PurchaseInvoiceObjectState CurrentObjectState {set;}

					Party BilledFromParty {set;}

					PurchaseInvoiceType PurchaseInvoiceType {set;}

					PurchaseInvoiceStatus InvoiceStatuses {set;}

	}
	public interface PurchaseInvoiceItem  : Object, InvoiceItem 
	{
					PurchaseInvoiceItemType PurchaseInvoiceItemType {set;}

					Part Part {set;}

					PurchaseInvoiceItemStatus CurrentInvoiceItemStatus {set;}

					PurchaseInvoiceItemStatus InvoiceItemStatuses {set;}

					PurchaseInvoiceItemObjectState CurrentObjectState {set;}

	}
	public interface PurchaseOrderItem  : Object, OrderItem 
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
	public interface PurchaseReturn  : Object, Shipment 
	{
					PurchaseReturnStatus CurrentShipmentStatus {set;}

					PurchaseReturnObjectState CurrentObjectState {set;}

					PurchaseReturnStatus ShipmentStatuses {set;}

	}
	public interface Receipt  : Object, Payment 
	{
	}
	public interface RecurringCharge  : Object, PriceComponent 
	{
					TimeFrequency TimeFrequency {set;}

	}
	public interface SalesInvoice  : Object, Invoice 
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
	public interface SalesOrder  : Object, Order 
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
	public interface SalesOrderItem  : Object, OrderItem 
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
	public interface Transfer  : Object, Shipment 
	{
					TransferObjectState CurrentObjectState {set;}

					TransferStatus CurrentShipmentStatus {set;}

					TransferStatus ShipmentStatuses {set;}

	}
	public interface UtilizationCharge  : Object, PriceComponent 
	{
					global::System.Decimal? Quantity {set;}

					UnitOfMeasure UnitOfMeasure {set;}

	}
	public interface Warehouse  : Object, Facility 
	{
	}
	public interface WorkEffortPartyAssignment  : Object, Period, AccessControlledObject, Commentable 
	{
					WorkEffort Assignment {set;}

					Party Party {set;}

					Facility Facility {set;}

	}
	public interface WorkEffortStatus  : Object, Deletable, AccessControlledObject 
	{
					global::System.DateTime StartDateTime {set;}

					WorkEffortObjectState WorkEffortObjectState {set;}

	}
	public interface WorkEffortType  : Object, AccessControlledObject 
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
}