import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { BasepriceEditComponent, BasepriceEditModule } from '../allors/material/apps/objects/baseprice/edit/baseprice-edit.module';
import { CatalogueEditComponent, CatalogueEditModule } from '../allors/material/apps/objects/catalogue/edit/catalogue-edit.module';
import { CustomerRelationshipEditComponent, CustomerRelationshipEditModule } from '../allors/material/apps/objects/customerrelationship/edit/customerrelationship-edit.module';
import { EmailAddressCreateComponent, EmailAddressCreateModule } from '../allors/material/apps/objects/emailaddress/create/emailaddress-create.module';
import { EmailAddressEditComponent, EmailAddressEditModule } from '../allors/material/apps/objects/emailaddress/edit/emailaddress-edit.module';
import { EmailCommunicationEditComponent, EmailCommunicationEditModule } from '../allors/material/apps/objects/emailcommunication/edit/emailcommunication-edit.module';
import { EmploymentEditComponent, EmploymentEditModule } from '../allors/material/apps/objects/employment/edit/employment-edit.module';
import { NonUnifiedGoodCreateComponent, NonUnifiedGoodCreateModule } from '../allors/material/apps/objects/nonunifiedgood/create/nonunifiedgood-create.module';
import { InventoryItemTransactionEditComponent, InventoryItemTransactionEditModule } from '../allors/material/apps/objects/inventoryitemtransaction/edit/inventoryitemtransaction-edit.module';
import { ProductIdentificationEditComponent, ProductIdentificationEditModule } from '../allors/material/apps/objects/productidentification/edit/productIdentification.module';
import { FaceToFaceCommunicationEditComponent, FaceToFaceCommunicationEditModule } from '../allors/material/apps/objects/facetofacecommunication/edit/facetofacecommunication-edit.module';
import { LetterCorrespondenceEditComponent, LetterCorrespondenceEditModule } from '../allors/material/apps/objects/lettercorrespondence/edit/lettercorrespondence-edit.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/apps/objects/organisation/create/organisation-create.module';
import { OrganisationContactRelationshipEditComponent, OrganisationContactRelationshipEditModule } from '../allors/material/apps/objects/organisationcontactrelationship/edit/organisationcontactrelationship-edit.module';
import { PartyContactmechanismEditComponent, PartyContactmechanismEditModule } from '../allors/material/apps/objects/partycontactmechanism/edit/partycontactmechanism-edit.module';
import { NonUnifiedPartCreateComponent, NonUnifiedPartCreateModule } from '../allors/material/apps/objects/nonunifiedpart/create/nonunifiedpart-create.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/apps/objects/person/create/person-create.module';
import { PhoneCommunicationEditComponent, PhoneCommunicationEditModule } from '../allors/material/apps/objects/phonecommunication/edit/phonecommunication-edit.module';
import { PostalAddressCreateComponent, PostalAddressCreateModule } from '../allors/material/apps/objects/postaladdress/create/postaladdress-create.module';
import { PostalAddressEditComponent, PostalAddressEditModule } from '../allors/material/apps/objects/postaladdress/edit/postaladdress-edit.module';
import { ProductCategoryEditComponent, ProductCategoryEditModule } from '../allors/material/apps/objects/productcategory/edit/productcategory-edit.module';
import { ProductQuoteCreateComponent, ProductQuoteCreateModule } from '../allors/material/apps/objects/productquote/create/productquote-create.module';
import { ProductTypeEditComponent, ProductTypeEditModule } from '../allors/material/apps/objects/producttype/edit/producttype-edit.module';
import { RequestItemEditComponent, RequestItemEditModule } from '../allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from '../allors/material/apps/objects/requestforquote/create/requestforquote-create.module';
import { QuoteItemEditComponent, QuoteItemEditModule } from '../allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import { PurchaseInvoiceCreateComponent, PurchaseInvoiceCreateModule } from '../allors/material/apps/objects/purchaseinvoice/create/purchaseinvoice-create.module';
import { PurchaseInvoiceItemEditComponent, PurchaseInvoiceItemEditModule } from '../allors/material/apps/objects/purchaseinvoiceItem/edit/purchaseinvoiceitem-edit.module';
import { SalesInvoiceCreateComponent, SalesInvoiceCreateModule } from '../allors/material/apps/objects/salesinvoice/create/salesinvoice-create.module';
import { SalesInvoiceItemEditComponent, SalesInvoiceItemEditModule } from '../allors/material/apps/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import { SalesOrderCreateComponent, SalesOrderCreateModule } from '../allors/material/apps/objects/salesorder/create/salesorder-create.module';
import { SalesOrderItemEditComponent, SalesOrderItemEditModule } from '../allors/material/apps/objects/salesorderitem/edit/salesorderitem-edit.module';
import { SalesTermEditComponent, SalesTermEditModule } from '../allors/material/apps/objects/salesterm/edit/salesterm-edit.module';
import { SerialisedItemCharacteristicEditComponent, SerialisedItemCharacteristicEditModule } from '../allors/material/apps/objects/serialiseditemcharacteristictype/edit/serialiseditemcharacteristic-edit.module';
import { SerialisedItemCreateComponent, SerialisedItemCreateModule } from '../allors/material/apps/objects/serialiseditem/create/serialiseditem-create.module';
import { SupplierOfferingEditComponent, SupplierOfferingEditModule } from '../allors/material/apps/objects/supplieroffering/edit/supplieroffering-edit.module';
import { SupplierRelationshipEditComponent, SupplierRelationshipEditModule } from '../allors/material/apps/objects/supplierrelationship/edit/supplierrelationship-edit.module';
import { TelecommunicationsNumberCreateComponent, TelecommunicationsNumberCreateModule } from '../allors/material/apps/objects/telecommunicationsnumber/create/telecommunicationsnumber-create.module';
import { TelecommunicationsNumberEditComponent, TelecommunicationsNumberEditModule } from '../allors/material/apps/objects/telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import { UnifiedGoodCreateComponent, UnifiedGoodCreateModule } from '../allors/material/apps/objects/unifiedgood/create/unifiedgood-create.module';
import { WebAddressCreateComponent, WebAddressCreateModule } from '../allors/material/apps/objects/webaddress/create/webaddress-create.module';
import { WebAddressEditComponent, WebAddressEditModule } from '../allors/material/apps/objects/webaddress/edit/webaddress-edit.module';
import { WorkEffortFixedAssetAssignmentEditComponent, WorkEffortFixedAssetAssignmentEditModule } from '../allors/material/apps/objects/workeffortfixedassetassignment/edit/workeffortfixedassetassignment-edit.module';
import { WorkEffortPartyAssignmentEditComponent, WorkEffortPartyAssignmentEditModule } from '../allors/material/apps/objects/workeffortpartyassignment/edit/workeffortpartyassignment-edit.module';
import { WorkTaskCreateModule, WorkTaskCreateComponent } from '../allors/material/apps/objects/worktask/create/worktask-create.module';

import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from '../allors/material/base/services/object';

export const create = {
  [ids.BasePrice]: BasepriceEditComponent,
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
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
  [ids.NonUnifiedGood]: NonUnifiedGoodCreateComponent,
  [ids.NonUnifiedPart]: NonUnifiedPartCreateComponent,
  [ids.PartNumber]: ProductIdentificationEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PostalAddress]: PostalAddressCreateComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.ProductNumber]: ProductIdentificationEditComponent,
  [ids.ProductQuote]: ProductQuoteCreateComponent,
  [ids.ProductType]: ProductTypeEditComponent,
  [ids.PurchaseInvoice]: PurchaseInvoiceCreateComponent,
  [ids.PurchaseInvoiceItem]: PurchaseInvoiceItemEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.RequestForQuote]: RequestForQuoteCreateComponent,
  [ids.SalesInvoice]: SalesInvoiceCreateComponent,
  [ids.SalesInvoiceItem]: SalesInvoiceItemEditComponent,
  [ids.SalesOrder]: SalesOrderCreateComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItem]: SerialisedItemCreateComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.SkuIdentification]: ProductIdentificationEditComponent,
  [ids.SupplierOffering]: SupplierOfferingEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberCreateComponent,
  [ids.UnifiedGood]: UnifiedGoodCreateComponent,
  [ids.UpcaIdentification]: ProductIdentificationEditComponent,
  [ids.UpceIdentification]: ProductIdentificationEditComponent,
  [ids.WebAddress]: WebAddressCreateComponent,
  [ids.WorkEffortFixedAssetAssignment]: WorkEffortFixedAssetAssignmentEditComponent,
  [ids.WorkEffortPartyAssignment]: WorkEffortPartyAssignmentEditComponent,
  [ids.WorkTask]: WorkTaskCreateComponent,
};

export const edit = {
  [ids.BasePrice]: BasepriceEditComponent,
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
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
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.OrganisationContactRelationship]: OrganisationContactRelationshipEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PostalAddress]: PostalAddressEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.PartNumber]: ProductIdentificationEditComponent,
  [ids.ProductNumber]: ProductIdentificationEditComponent,
  [ids.ProductType]: ProductTypeEditComponent,
  [ids.PurchaseInvoiceItem]: PurchaseInvoiceItemEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.SalesInvoiceItem]: SalesInvoiceItemEditComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.SkuIdentification]: ProductIdentificationEditComponent,
  [ids.SupplierOffering]: SupplierOfferingEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberEditComponent,
  [ids.UpcaIdentification]: ProductIdentificationEditComponent,
  [ids.UpceIdentification]: ProductIdentificationEditComponent,
  [ids.WebAddress]: WebAddressEditComponent,
  [ids.WorkEffortFixedAssetAssignment]: WorkEffortFixedAssetAssignmentEditComponent,
  [ids.WorkEffortPartyAssignment]: WorkEffortPartyAssignmentEditComponent,
};

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,

    BasepriceEditModule,
    CatalogueEditModule,
    CustomerRelationshipEditModule,
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
    NonUnifiedPartCreateModule,
    PartyContactmechanismEditModule,
    PersonCreateModule,
    PhoneCommunicationEditModule,
    PostalAddressCreateModule,
    PostalAddressEditModule,
    ProductCategoryEditModule,
    ProductQuoteCreateModule,
    ProductTypeEditModule,
    PurchaseInvoiceCreateModule,
    PurchaseInvoiceItemEditModule,
    QuoteItemEditModule,
    RequestItemEditModule,
    RequestForQuoteCreateModule,
    SalesInvoiceCreateModule,
    SalesInvoiceItemEditModule,
    SalesOrderCreateModule,
    SalesOrderItemEditModule,
    SalesTermEditModule,
    SerialisedItemCreateModule,
    SerialisedItemCharacteristicEditModule,
    SupplierOfferingEditModule,
    SupplierRelationshipEditModule,
    TelecommunicationsNumberCreateModule,
    TelecommunicationsNumberEditModule,
    UnifiedGoodCreateModule,
    WebAddressCreateModule,
    WebAddressEditModule,
    WorkEffortFixedAssetAssignmentEditModule,
    WorkEffortPartyAssignmentEditModule,
    WorkTaskCreateModule,
  ],
  entryComponents: [
    BasepriceEditComponent,
    CatalogueEditComponent,
    CustomerRelationshipEditComponent,
    EmailAddressCreateComponent,
    EmailAddressEditComponent,
    EmailCommunicationEditComponent,
    EmploymentEditComponent,
    FaceToFaceCommunicationEditComponent,
    NonUnifiedGoodCreateComponent,
    ProductIdentificationEditComponent,
    InventoryItemTransactionEditComponent,
    LetterCorrespondenceEditComponent,
    OrganisationCreateComponent,
    OrganisationContactRelationshipEditComponent,
    PersonCreateComponent,
    NonUnifiedPartCreateComponent,
    PartyContactmechanismEditComponent,
    PhoneCommunicationEditComponent,
    PostalAddressCreateComponent,
    PostalAddressEditComponent,
    ProductCategoryEditComponent,
    ProductQuoteCreateComponent,
    ProductTypeEditComponent,
    PurchaseInvoiceCreateComponent,
    PurchaseInvoiceItemEditComponent,
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
    SupplierOfferingEditComponent,
    SupplierRelationshipEditComponent,
    TelecommunicationsNumberCreateComponent,
    TelecommunicationsNumberEditComponent,
    UnifiedGoodCreateComponent,
    WebAddressCreateComponent,
    WebAddressEditComponent,
    WorkEffortFixedAssetAssignmentEditComponent,
    WorkEffortPartyAssignmentEditComponent,
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
