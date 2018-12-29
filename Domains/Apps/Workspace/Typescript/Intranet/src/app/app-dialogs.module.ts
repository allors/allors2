import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { CatalogueEditComponent, CatalogueEditModule } from '../allors/material/apps/objects/catalogue/edit/catalogue-edit.module';
import { CustomerRelationshipEditComponent, CustomerRelationshipEditModule } from '../allors/material/apps/objects/customerrelationship/edit/customerrelationship-edit.module';
import { EmailAddressEditComponent, EmailAddressEditModule } from '../allors/material/apps/objects/emailaddress/edit/emailaddress-edit.module';
import { EmailCommunicationEditComponent, EmailCommunicationEditModule } from '../allors/material/apps/objects/emailcommunication/edit/emailcommunication-edit.module';
import { EmploymentEditComponent, EmploymentEditModule } from '../allors/material/apps/objects/employment/edit/employment-edit.module';
import { GoodCreateComponent, GoodCreateModule } from '../allors/material/apps/objects/good/create/good-create.module';
import { IGoodIdentificationEditComponent, IGoodIdentificationEditModule } from '../allors/material/apps/objects/igoodidentification/edit/igoodIdentification.module';
import { FaceToFaceCommunicationEditComponent, FaceToFaceCommunicationEditModule } from '../allors/material/apps/objects/facetofacecommunication/edit/facetofacecommunication-edit.module';
import { LetterCorrespondenceEditComponent, LetterCorrespondenceEditModule } from '../allors/material/apps/objects/lettercorrespondence/edit/lettercorrespondence-edit.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/apps/objects/organisation/create/organisation-create.module';
import { PartyContactmechanismEditComponent, PartyContactmechanismEditModule } from '../allors/material/apps/objects/partycontactmechanism/edit/partycontactmechanism-edit.module';
import { PartCreateComponent, PartCreateModule } from '../allors/material/apps/objects/part/create/part-create.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/apps/objects/person/create/person-create.module';
import { PhoneCommunicationEditComponent, PhoneCommunicationEditModule } from '../allors/material/apps/objects/phonecommunication/edit/phonecommunication-edit.module';
import { PostalAddressEditComponent, PostalAddressEditModule } from '../allors/material/apps/objects/postaladdress/edit/postaladdress-edit.module';
import { ProductCategoryEditComponent, ProductCategoryEditModule } from '../allors/material/apps/objects/productcategory/edit/productcategory-edit.module';
import { RequestItemEditComponent, RequestItemEditModule } from '../allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from '../allors/material/apps/objects/requestforquote/create/requestforquote-create.module';
import { ProductQuoteCreateComponent, ProductQuoteCreateModule } from '../allors/material/apps/objects/productquote/create/productquote-create.module';
import { QuoteItemEditComponent, QuoteItemEditModule } from '../allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import { SalesOrderCreateComponent, SalesOrderCreateModule } from '../allors/material/apps/objects/salesorder/create/salesorder-create.module';
import { SalesOrderItemEditComponent, SalesOrderItemEditModule } from '../allors/material/apps/objects/salesorderitem/edit/salesorderitem-edit.module';
import { SalesTermEditComponent, SalesTermEditModule } from '../allors/material/apps/objects/salesterm/edit/salesterm-edit.module';
import { SupplierRelationshipEditComponent, SupplierRelationshipEditModule } from '../allors/material/apps/objects/supplierraltionship/edit/supplierrelationship-edit.module';
import { TelecommunicationsNumberEditComponent, TelecommunicationsNumberEditModule } from '../allors/material/apps/objects/telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import { WebAddressEditComponent, WebAddressEditModule } from '../allors/material/apps/objects/webaddress/edit/webaddress-edit.module';
import { WorkTaskCreateModule, WorkTaskCreateComponent } from '../allors/material/apps/objects/worktask/create/worktask-create.module';

import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from '../allors/material/base/services/object';
import { SerialisedItemCharacteristicEditComponent, SerialisedItemCharacteristicEditModule } from '../allors/material/apps/objects/serialiseditemcharacteristictype/edit/serialiseditemcharacteristic-edit.module';
import { ProductTypeEditComponent, ProductTypeEditModule } from '../allors/material/apps/objects/producttype/edit/producttype-edit.module';
import { PurchaseInvoiceItemEditComponent, PurchaseInvoiceItemEditModule } from '../allors/material/apps/objects/purchaseinvoiceItem/edit/purchaseinvoiceitem-edit.module';
import { PurchaseInvoiceCreateComponent, PurchaseInvoiceCreateModule } from '../allors/material/apps/objects/purchaseinvoice/create/purchaseinvoice-create.module';
import { SalesInvoiceItemEditComponent, SalesInvoiceItemEditModule } from '../allors/material/apps/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import { SalesInvoiceCreateComponent, SalesInvoiceCreateModule } from '../allors/material/apps/objects/salesinvoice/create/salesinvoice-create.module';

export const create = {
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
  [ids.EanIdentification]: IGoodIdentificationEditComponent,
  [ids.EmailAddress]: EmailAddressEditComponent,
  [ids.EmailCommunication]: EmailCommunicationEditComponent,
  [ids.Employment]: EmploymentEditComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationEditComponent,
  [ids.Good]: GoodCreateComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.IsbnIdentification]: IGoodIdentificationEditComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceEditComponent,
  [ids.ManufacturerIdentification]: IGoodIdentificationEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.Organisation]: OrganisationCreateComponent,
  [ids.Part]: PartCreateComponent,
  [ids.PartNumber]: IGoodIdentificationEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PostalAddress]: PostalAddressEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.ProductNumber]: IGoodIdentificationEditComponent,
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
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.SkuIdentification]: IGoodIdentificationEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberEditComponent,
  [ids.UpcaIdentification]: IGoodIdentificationEditComponent,
  [ids.UpceIdentification]: IGoodIdentificationEditComponent,
  [ids.WebAddress]: WebAddressEditComponent,
  [ids.WorkTask]: WorkTaskCreateComponent,
};

export const edit = {
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.CustomerRelationship]: CustomerRelationshipEditComponent,
  [ids.EanIdentification]: IGoodIdentificationEditComponent,
  [ids.EmailAddress]: EmailAddressEditComponent,
  [ids.EmailCommunication]: EmailCommunicationEditComponent,
  [ids.Employment]: EmploymentEditComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationEditComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.IsbnIdentification]: IGoodIdentificationEditComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceEditComponent,
  [ids.ManufacturerIdentification]: IGoodIdentificationEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.PartyContactMechanism]: PartyContactmechanismEditComponent,
  [ids.PhoneCommunication]: PhoneCommunicationEditComponent,
  [ids.PostalAddress]: PostalAddressEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.PartNumber]: IGoodIdentificationEditComponent,
  [ids.ProductNumber]: IGoodIdentificationEditComponent,
  [ids.ProductType]: ProductTypeEditComponent,
  [ids.PurchaseInvoiceItem]: PurchaseInvoiceItemEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.SalesInvoiceItem]: SalesInvoiceItemEditComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.SkuIdentification]: IGoodIdentificationEditComponent,
  [ids.SupplierRelationship]: SupplierRelationshipEditComponent,
  [ids.TelecommunicationsNumber]: TelecommunicationsNumberEditComponent,
  [ids.UpcaIdentification]: IGoodIdentificationEditComponent,
  [ids.UpceIdentification]: IGoodIdentificationEditComponent,
  [ids.WebAddress]: WebAddressEditComponent,
};

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,

    CatalogueEditModule,
    CustomerRelationshipEditModule,
    EmailAddressEditModule,
    EmailCommunicationEditModule,
    EmploymentEditModule,
    FaceToFaceCommunicationEditModule,
    GoodCreateModule,
    IGoodIdentificationEditModule,
    LetterCorrespondenceEditModule,
    OrganisationCreateModule,
    PartCreateModule,
    PartyContactmechanismEditModule,
    PersonCreateModule,
    PhoneCommunicationEditModule,
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
    SerialisedItemCharacteristicEditModule,
    SupplierRelationshipEditModule,
    TelecommunicationsNumberEditModule,
    WebAddressEditModule,
    WorkTaskCreateModule,
  ],
  entryComponents: [
    CatalogueEditComponent,
    CustomerRelationshipEditComponent,
    EmailAddressEditComponent,
    EmailCommunicationEditComponent,
    EmploymentEditComponent,
    FaceToFaceCommunicationEditComponent,
    GoodCreateComponent,
    IGoodIdentificationEditComponent,
    LetterCorrespondenceEditComponent,
    OrganisationCreateComponent,
    PersonCreateComponent,
    PartCreateComponent,
    PartyContactmechanismEditComponent,
    PhoneCommunicationEditComponent,
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
    SerialisedItemCharacteristicEditComponent,
    SupplierRelationshipEditComponent,
    TelecommunicationsNumberEditComponent,
    WebAddressEditComponent,
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
