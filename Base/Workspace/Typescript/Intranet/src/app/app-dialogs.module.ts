import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';

import { ids } from '../allors/meta/generated';

import { BasepriceEditComponent, BasepriceEditModule } from '../allors/material/base/objects/baseprice/edit/baseprice-edit.module';
import { CarrierEditComponent, CarrierEditModule } from '../allors/material/base/objects/carrier/edit/carrier-edit.module';
import { CatalogueEditComponent, CatalogueEditModule } from '../allors/material/base/objects/catalogue/edit/catalogue-edit.module';
import { CustomerRelationshipEditComponent, CustomerRelationshipEditModule } from '../allors/material/base/objects/customerrelationship/edit/customerrelationship-edit.module';
import { CustomerShipmentCreateComponent, CustomerShipmentCreateModule } from '../allors/material/base/objects/customershipment/create/customershipment-create.module';
import { DisbursementEditComponent, DisbursementEditModule } from '../allors/material/base/objects/disbursement/edit/disbursement-edit.module';
import { EmailAddressCreateComponent, EmailAddressCreateModule } from '../allors/material/base/objects/emailaddress/create/emailaddress-create.module';
import { EmailAddressEditComponent, EmailAddressEditModule } from '../allors/material/base/objects/emailaddress/edit/emailaddress-edit.module';
import { EmailCommunicationEditComponent, EmailCommunicationEditModule } from '../allors/material/base/objects/emailcommunication/edit/emailcommunication-edit.module';
import { EmploymentEditComponent, EmploymentEditModule } from '../allors/material/base/objects/employment/edit/employment-edit.module';
import { NonSerialisedInventoryItemEditComponent, NonSerialisedInventoryItemEditModule } from '../allors/material/base/objects/nonserialisedinventoryitem/edit/nonserialisedinventoryitem-edit.module';
import { NonUnifiedGoodCreateComponent, NonUnifiedGoodCreateModule } from '../allors/material/base/objects/nonunifiedgood/create/nonunifiedgood-create.module';
import { NonUnifiedPartCreateComponent, NonUnifiedPartCreateModule } from '../allors/material/base/objects/nonunifiedpart/create/nonunifiedpart-create.module';
import { InventoryItemTransactionEditComponent, InventoryItemTransactionEditModule } from '../allors/material/base/objects/inventoryitemtransaction/edit/inventoryitemtransaction-edit.module';
import { ProductIdentificationEditComponent, ProductIdentificationEditModule } from '../allors/material/base/objects/productidentification/edit/productIdentification-edit.module';
import { FaceToFaceCommunicationEditComponent, FaceToFaceCommunicationEditModule } from '../allors/material/base/objects/facetofacecommunication/edit/facetofacecommunication-edit.module';
import { LetterCorrespondenceEditComponent, LetterCorrespondenceEditModule } from '../allors/material/base/objects/lettercorrespondence/edit/lettercorrespondence-edit.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/base/objects/organisation/create/organisation-create.module';
import { OrganisationContactRelationshipEditComponent, OrganisationContactRelationshipEditModule } from '../allors/material/base/objects/organisationcontactrelationship/edit/organisationcontactrelationship-edit.module';
import { PartyContactmechanismEditComponent, PartyContactmechanismEditModule } from '../allors/material/base/objects/partycontactmechanism/edit/partycontactmechanism-edit.module';
import { PartyRateEditComponent, PartyRateEditModule } from '../allors/material/base/objects/partyrate/edit/partyrate-edit.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/base/objects/person/create/person-create.module';
import { PhoneCommunicationEditComponent, PhoneCommunicationEditModule } from '../allors/material/base/objects/phonecommunication/edit/phonecommunication-edit.module';
import { PositionTypeEditComponent, PositionTypeEditModule } from '../allors/material/base/objects/positiontype/edit/positiontype-edit.module';
import { PositionTypeRateEditComponent, PositionTypeRateEditModule } from '../allors/material/base/objects/positiontyperate/edit/positiontyperate-edit.module';
import { PostalAddressCreateComponent, PostalAddressCreateModule } from '../allors/material/base/objects/postaladdress/create/postaladdress-create.module';
import { PostalAddressEditComponent, PostalAddressEditModule } from '../allors/material/base/objects/postaladdress/edit/postaladdress-edit.module';
import { ProductCategoryEditComponent, ProductCategoryEditModule } from '../allors/material/base/objects/productcategory/edit/productcategory-edit.module';
import { ProductQuoteCreateComponent, ProductQuoteCreateModule } from '../allors/material/base/objects/productquote/create/productquote-create.module';
import { ProductTypeEditComponent, ProductTypeEditModule } from '../allors/material/base/objects/producttype/edit/producttype-edit.module';
import { ProductQuoteApprovalEditComponent, ProductQuoteApprovalEditModule } from '../allors/material/base/objects/productquoteapproval/edit/productquoteapproval-edit.module';
import { PurchaseInvoiceApprovalEditComponent, PurchaseInvoiceApprovalEditModule } from '../allors/material/base/objects/purchaseinvoiceapproval/edit/purchaseinvoiceapproval-edit.module';
import { PurchaseInvoiceCreateComponent, PurchaseInvoiceCreateModule } from '../allors/material/base/objects/purchaseinvoice/create/purchaseinvoice-create.module';
import { PurchaseInvoiceItemEditComponent, PurchaseInvoiceItemEditModule } from '../allors/material/base/objects/purchaseinvoiceitem/edit/purchaseinvoiceitem-edit.module';
import { PurchaseOrderApprovalLevel1EditComponent, PurchaseOrderApprovalLevel1EditModule } from '../allors/material/base/objects/purchaseorderapprovallevel1/edit/purchaseorderapprovallevel1-edit.module';
import { PurchaseOrderApprovalLevel2EditComponent, PurchaseOrderApprovalLevel2EditModule } from '../allors/material/base/objects/purchaseorderapprovallevel2/edit/purchaseorderapprovallevel2-edit.module';
import { PurchaseOrderCreateComponent, PurchaseOrderCreateModule } from '../allors/material/base/objects/purchaseorder/create/purchaseorder-create.module';
import { PurchaseOrderItemEditComponent, PurchaseOrderItemEditModule } from '../allors/material/base/objects/purchaseorderitem/edit/purchaseorderitem-edit.module';
import { PurchaseReturnCreateComponent, PurchaseReturnCreateModule } from '../allors/material/base/objects/purchasereturn/create/purchasereturn-create.module';
import { PurchaseShipmentCreateComponent, PurchaseShipmentCreateModule } from '../allors/material/base/objects/purchaseshipment/create/purchaseshipment-create.module';
import { QuoteItemEditComponent, QuoteItemEditModule } from '../allors/material/base/objects/quoteitem/edit/quoteitem-edit.module';
import { ReceiptEditComponent, ReceiptEditModule } from '../allors/material/base/objects/receipt/edit/receipt-edit.module';
import { RepeatingPurchaseInvoiceEditComponent, RepeatingPurchaseInvoiceEditModule } from '../allors/material/base/objects/repeatingpurchaseinvoice/edit/repeatingpurchaseinvoice-edit.module';
import { RepeatingSalesInvoiceEditComponent, RepeatingSalesInvoiceEditModule } from '../allors/material/base/objects/repeatingsalesinvoice/edit/repeatingsalesinvoice-edit.module';
import { RequestItemEditComponent, RequestItemEditModule } from '../allors/material/base/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from '../allors/material/base/objects/requestforquote/create/requestforquote-create.module';
import { SalesInvoiceCreateComponent, SalesInvoiceCreateModule } from '../allors/material/base/objects/salesinvoice/create/salesinvoice-create.module';
import { SalesInvoiceItemEditComponent, SalesInvoiceItemEditModule } from '../allors/material/base/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import { SalesOrderCreateComponent, SalesOrderCreateModule } from '../allors/material/base/objects/salesorder/create/salesorder-create.module';
import { SalesOrderItemEditComponent, SalesOrderItemEditModule } from '../allors/material/base/objects/salesorderitem/edit/salesorderitem-edit.module';
import { SalesTermEditComponent, SalesTermEditModule } from '../allors/material/base/objects/salesterm/edit/salesterm-edit.module';
import { SerialisedItemCharacteristicEditComponent, SerialisedItemCharacteristicEditModule } from '../allors/material/base/objects/serialiseditemcharacteristictype/edit/serialiseditemcharacteristic-edit.module';
import { SerialisedItemCreateComponent, SerialisedItemCreateModule } from '../allors/material/base/objects/serialiseditem/create/serialiseditem-create.module';
import { ShipmentItemEditComponent, ShipmentItemEditModule } from '../allors/material/base/objects/shipmentitem/edit/shipmentitem-edit.module';
import { SubContractorRelationshipEditComponent, SubContractorRelationshipEditModule } from '../allors/material/base/objects/subcontractorrelationship/edit/subcontractorrelationship-edit.module';
import { SupplierOfferingEditComponent, SupplierOfferingEditModule } from '../allors/material/base/objects/supplieroffering/edit/supplieroffering-edit.module';
import { SupplierRelationshipEditComponent, SupplierRelationshipEditModule } from '../allors/material/base/objects/supplierrelationship/edit/supplierrelationship-edit.module';
import { TelecommunicationsNumberCreateComponent, TelecommunicationsNumberCreateModule } from '../allors/material/base/objects/telecommunicationsnumber/create/telecommunicationsnumber-create.module';
import { TelecommunicationsNumberEditComponent, TelecommunicationsNumberEditModule } from '../allors/material/base/objects/telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import { TimeEntryEditComponent, TimeEntryEditModule } from '../allors/material/base/objects/timeentry/edit/timeentry-edit.module';
import { UnifiedGoodCreateComponent, UnifiedGoodCreateModule } from '../allors/material/base/objects/unifiedgood/create/unifiedgood-create.module';
import { UserProfileEditComponent, UserProfileEditModule } from '../allors/material/base/objects/userprofile/edit/userprofile-edit.module';
import { WebAddressCreateComponent, WebAddressCreateModule } from '../allors/material/base/objects/webaddress/create/webaddress-create.module';
import { WebAddressEditComponent, WebAddressEditModule } from '../allors/material/base/objects/webaddress/edit/webaddress-edit.module';
import { WorkEffortAssignmentRateEditComponent, WorkEffortAssignmentRateEditModule } from '../allors/material/base/objects/workeffortassignmentrate/edit/workeffortassignmentrate-edit.module';
import { WorkEffortFixedAssetAssignmentEditComponent, WorkEffortFixedAssetAssignmentEditModule } from '../allors/material/base/objects/workeffortfixedassetassignment/edit/workeffortfixedassetassignment-edit.module';
import { WorkEffortInventoryAssignmentEditComponent, WorkEffortInventoryAssignmentEditModule } from '../allors/material/base/objects/workeffortinventoryassignment/edit/workeffortinventoryassignment-edit.module';
import { WorkEffortPartyAssignmentEditComponent, WorkEffortPartyAssignmentEditModule } from '../allors/material/base/objects/workeffortpartyassignment/edit/workeffortpartyassignment-edit.module';
// import { WorkEffortPurchaseOrderItemAssignmentEditComponent, WorkEffortPurchaseOrderItemAssignmentEditModule } from '../allors/material/base/objects/workeffortpurchaseorderitemassignment/edit/workeffortpurchaseorderitemassignment-edit.module';
import { WorkTaskCreateModule, WorkTaskCreateComponent } from '../allors/material/base/objects/worktask/create/worktask-create.module';

import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from '../allors/material/core/services/object';
import { NonSerialisedInventoryItemComponent } from 'src/allors/material/base/objects/nonserialisedinventoryitem/overview/panel/nonserialisedinventoryitem-overview-panel.module';

export const create = {
  [ids.BasePrice]: BasepriceEditComponent,
  [ids.Carrier]: CarrierEditComponent,
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
  [ids.CustomerShipment]: CustomerShipmentCreateComponent,
  [ids.Disbursement]: DisbursementEditComponent,
  [ids.EanIdentification]: ProductIdentificationEditComponent,
  [ids.EmailAddress]: EmailAddressCreateComponent,
  [ids.EmailCommunication]: EmailCommunicationEditComponent,
  [ids.Employment]: EmploymentEditComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationEditComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InventoryItemTransaction]: InventoryItemTransactionEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.IsbnIdentification]: ProductIdentificationEditComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceEditComponent,
  [ids.ManufacturerIdentification]: ProductIdentificationEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.Organisation]: OrganisationCreateComponent,
  [ids.OrganisationContactRelationship]: OrganisationContactRelationshipEditComponent,
  [ids.NonSerialisedInventoryItem]: NonSerialisedInventoryItemEditComponent,
  [ids.NonUnifiedGood]: NonUnifiedGoodCreateComponent,
  [ids.NonUnifiedPart]: NonUnifiedPartCreateComponent,
  [ids.PartNumber]: ProductIdentificationEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.PartyRate]: PartyRateEditComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PositionType]: PositionTypeEditComponent,
  [ids.PositionTypeRate]: PositionTypeRateEditComponent,
  [ids.PostalAddress]: PostalAddressCreateComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.ProductNumber]: ProductIdentificationEditComponent,
  [ids.ProductQuote]: ProductQuoteCreateComponent,
  [ids.ProductType]: ProductTypeEditComponent,
  [ids.PurchaseInvoice]: PurchaseInvoiceCreateComponent,
  [ids.PurchaseInvoiceItem]: PurchaseInvoiceItemEditComponent,
  [ids.PurchaseOrder]: PurchaseOrderCreateComponent,
  [ids.PurchaseOrderItem]: PurchaseOrderItemEditComponent,
  [ids.PurchaseReturn]: PurchaseReturnCreateComponent,
  [ids.PurchaseShipment]: PurchaseShipmentCreateComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.Receipt]: ReceiptEditComponent,
  [ids.RepeatingPurchaseInvoice]: RepeatingPurchaseInvoiceEditComponent,
  [ids.RepeatingSalesInvoice]: RepeatingSalesInvoiceEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.RequestForQuote]: RequestForQuoteCreateComponent,
  [ids.SalesInvoice]: SalesInvoiceCreateComponent,
  [ids.SalesInvoiceItem]: SalesInvoiceItemEditComponent,
  [ids.SalesOrder]: SalesOrderCreateComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItem]: SerialisedItemCreateComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.ShipmentItem]: ShipmentItemEditComponent,
  [ids.SkuIdentification]: ProductIdentificationEditComponent,
  [ids.SubContractorRelationship]: SubContractorRelationshipEditComponent,
  [ids.SupplierOffering]: SupplierOfferingEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberCreateComponent,
  [ids.TimeEntry]: TimeEntryEditComponent,
  [ids.UnifiedGood]: UnifiedGoodCreateComponent,
  [ids.UpcaIdentification]: ProductIdentificationEditComponent,
  [ids.UpceIdentification]: ProductIdentificationEditComponent,
  [ids.WebAddress]: WebAddressCreateComponent,
  [ids.WorkEffortAssignmentRate]: WorkEffortAssignmentRateEditComponent,
  [ids.WorkEffortFixedAssetAssignment]: WorkEffortFixedAssetAssignmentEditComponent,
  [ids.WorkEffortInventoryAssignment]: WorkEffortInventoryAssignmentEditComponent,
  [ids.WorkEffortPartyAssignment]: WorkEffortPartyAssignmentEditComponent,
  // [ids.WorkEffortPurchaseOrderItemAssignment]: WorkEffortPurchaseOrderItemAssignmentEditComponent,
  [ids.WorkTask]: WorkTaskCreateComponent,
};

export const edit = {
  [ids.BasePrice]: BasepriceEditComponent,
  [ids.Carrier]: CarrierEditComponent,
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
  [ids.Disbursement]: DisbursementEditComponent,
  [ids.EanIdentification]: ProductIdentificationEditComponent,
  [ids.EmailAddress]: EmailAddressEditComponent,
  [ids.EmailCommunication]: EmailCommunicationEditComponent,
  [ids.Employment]: EmploymentEditComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationEditComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InventoryItemTransaction]: InventoryItemTransactionEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.IsbnIdentification]: ProductIdentificationEditComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceEditComponent,
  [ids.ManufacturerIdentification]: ProductIdentificationEditComponent,
  [ids.NonSerialisedInventoryItem]: NonSerialisedInventoryItemEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.OrganisationContactRelationship]: OrganisationContactRelationshipEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.PartyRate]: PartyRateEditComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PositionType]: PositionTypeEditComponent,
  [ids.PositionTypeRate]: PositionTypeRateEditComponent,
  [ids.PostalAddress]: PostalAddressEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.PartNumber]: ProductIdentificationEditComponent,
  [ids.ProductNumber]: ProductIdentificationEditComponent,
  [ids.ProductType]: ProductTypeEditComponent,
  [ids.ProductQuoteApproval]: ProductQuoteApprovalEditComponent,
  [ids.PurchaseInvoiceApproval]: PurchaseInvoiceApprovalEditComponent,
  [ids.PurchaseInvoiceItem]: PurchaseInvoiceItemEditComponent,
  [ids.PurchaseOrderApprovalLevel1]: PurchaseOrderApprovalLevel1EditComponent,
  [ids.PurchaseOrderApprovalLevel2]: PurchaseOrderApprovalLevel2EditComponent,
  [ids.PurchaseOrderItem]: PurchaseOrderItemEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.Receipt]: ReceiptEditComponent,
  [ids.RepeatingPurchaseInvoice]: RepeatingPurchaseInvoiceEditComponent,
  [ids.RepeatingSalesInvoice]: RepeatingSalesInvoiceEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.SalesInvoiceItem]: SalesInvoiceItemEditComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.ShipmentItem]: ShipmentItemEditComponent,
  [ids.SkuIdentification]: ProductIdentificationEditComponent,
  [ids.SupplierOffering]: SupplierOfferingEditComponent,
  [ids.SubContractorRelationship]: SubContractorRelationshipEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberEditComponent,
  [ids.TimeEntry]: TimeEntryEditComponent,
  [ids.UpcaIdentification]: ProductIdentificationEditComponent,
  [ids.UpceIdentification]: ProductIdentificationEditComponent,
  [ids.UserProfile]: UserProfileEditComponent,
  [ids.WebAddress]: WebAddressEditComponent,
  [ids.WorkEffortAssignmentRate]: WorkEffortAssignmentRateEditComponent,
  [ids.WorkEffortFixedAssetAssignment]: WorkEffortFixedAssetAssignmentEditComponent,
  [ids.WorkEffortInventoryAssignment]: WorkEffortInventoryAssignmentEditComponent,
  // [ids.WorkEffortPurchaseOrderItemAssignment]: WorkEffortPurchaseOrderItemAssignmentEditComponent,
  [ids.WorkEffortPartyAssignment]: WorkEffortPartyAssignmentEditComponent,
};

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,

    BasepriceEditModule,
    CarrierEditModule,
    CatalogueEditModule,
    CustomerRelationshipEditModule,
    CustomerShipmentCreateModule,
    DisbursementEditModule,
    EmailAddressCreateModule,
    EmailAddressEditModule,
    EmailCommunicationEditModule,
    EmploymentEditModule,
    FaceToFaceCommunicationEditModule,
    NonUnifiedGoodCreateModule,
    ProductIdentificationEditModule,
    InventoryItemTransactionEditModule,
    LetterCorrespondenceEditModule,
    OrganisationCreateModule,
    OrganisationContactRelationshipEditModule,
    NonSerialisedInventoryItemEditModule,
    NonUnifiedPartCreateModule,
    PartyContactmechanismEditModule,
    PartyRateEditModule,
    PersonCreateModule,
    PhoneCommunicationEditModule,
    PositionTypeEditModule,
    PositionTypeRateEditModule,
    PostalAddressCreateModule,
    PostalAddressEditModule,
    ProductCategoryEditModule,
    ProductQuoteApprovalEditModule,
    ProductQuoteCreateModule,
    ProductTypeEditModule,
    PurchaseInvoiceApprovalEditModule,
    PurchaseInvoiceCreateModule,
    PurchaseInvoiceItemEditModule,
    PurchaseOrderApprovalLevel1EditModule,
    PurchaseOrderApprovalLevel2EditModule,
    PurchaseOrderCreateModule,
    PurchaseOrderItemEditModule,
    PurchaseReturnCreateModule,
    PurchaseShipmentCreateModule,
    QuoteItemEditModule,
    ReceiptEditModule,
    RepeatingPurchaseInvoiceEditModule,
    RepeatingSalesInvoiceEditModule,
    RequestItemEditModule,
    RequestForQuoteCreateModule,
    SalesInvoiceCreateModule,
    SalesInvoiceItemEditModule,
    SalesOrderCreateModule,
    SalesOrderItemEditModule,
    SalesTermEditModule,
    SerialisedItemCreateModule,
    SerialisedItemCharacteristicEditModule,
    ShipmentItemEditModule,
    SubContractorRelationshipEditModule,
    SupplierOfferingEditModule,
    SupplierRelationshipEditModule,
    TelecommunicationsNumberCreateModule,
    TelecommunicationsNumberEditModule,
    TimeEntryEditModule,
    UnifiedGoodCreateModule,
    UserProfileEditModule,
    WebAddressCreateModule,
    WebAddressEditModule,
    WorkEffortAssignmentRateEditModule,
    WorkEffortFixedAssetAssignmentEditModule,
    WorkEffortPartyAssignmentEditModule,
    // WorkEffortPurchaseOrderItemAssignmentEditModule,
    WorkEffortInventoryAssignmentEditModule,
    WorkTaskCreateModule,
  ],
  entryComponents: [
    BasepriceEditComponent,
    CarrierEditComponent,
    CatalogueEditComponent,
    CustomerRelationshipEditComponent,
    CustomerShipmentCreateComponent,
    DisbursementEditComponent,
    EmailAddressCreateComponent,
    EmailAddressEditComponent,
    EmailCommunicationEditComponent,
    EmploymentEditComponent,
    FaceToFaceCommunicationEditComponent,
    NonSerialisedInventoryItemComponent,
    NonUnifiedGoodCreateComponent,
    NonUnifiedPartCreateComponent,
    InventoryItemTransactionEditComponent,
    LetterCorrespondenceEditComponent,
    OrganisationCreateComponent,
    OrganisationContactRelationshipEditComponent,
    PersonCreateComponent,
    PartyContactmechanismEditComponent,
    PartyRateEditComponent,
    PhoneCommunicationEditComponent,
    PositionTypeEditComponent,
    PositionTypeRateEditComponent,
    PostalAddressCreateComponent,
    PostalAddressEditComponent,
    ProductCategoryEditComponent,
    ProductIdentificationEditComponent,
    ProductQuoteApprovalEditComponent,
    ProductQuoteCreateComponent,
    ProductTypeEditComponent,
    PurchaseInvoiceApprovalEditComponent,
    PurchaseInvoiceCreateComponent,
    PurchaseInvoiceItemEditComponent,
    PurchaseOrderCreateComponent,
    PurchaseOrderApprovalLevel1EditComponent,
    PurchaseOrderApprovalLevel2EditComponent,
    PurchaseOrderItemEditComponent,
    PurchaseReturnCreateComponent,
    PurchaseShipmentCreateComponent,
    ReceiptEditComponent,
    RepeatingPurchaseInvoiceEditComponent,
    RepeatingSalesInvoiceEditComponent,
    QuoteItemEditComponent,
    RequestItemEditComponent,
    RequestForQuoteCreateComponent,
    SalesInvoiceCreateComponent,
    SalesInvoiceItemEditComponent,
    SalesOrderCreateComponent,
    SalesOrderItemEditComponent,
    SalesTermEditComponent,
    SerialisedItemCreateComponent,
    SerialisedItemCharacteristicEditComponent,
    ShipmentItemEditComponent,
    SubContractorRelationshipEditComponent,
    SupplierOfferingEditComponent,
    SupplierRelationshipEditComponent,
    TelecommunicationsNumberCreateComponent,
    TelecommunicationsNumberEditComponent,
    TimeEntryEditComponent,
    UnifiedGoodCreateComponent,
    UserProfileEditComponent,
    WebAddressCreateComponent,
    WebAddressEditComponent,
    WorkEffortAssignmentRateEditComponent,
    WorkEffortFixedAssetAssignmentEditComponent,
    WorkEffortInventoryAssignmentEditComponent,
    WorkEffortPartyAssignmentEditComponent,
    // WorkEffortPurchaseOrderItemAssignmentEditComponent,
    WorkTaskCreateComponent,
  ],
  providers: [
    ObjectService,
    { provide: OBJECT_CREATE_TOKEN, useValue: create },
    { provide: OBJECT_EDIT_TOKEN, useValue: edit },
  ]
})
export class AppDialogModule {

  constructor(@Optional() @SkipSelf() core: AppDialogModule) {
    if (core) {
      throw new Error('Use FactoryModule from AppModule');
    }
  }
}
