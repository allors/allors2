import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { CatalogueEditComponent, CatalogueEditModule } from '../allors/material/apps/objects/catalogue/edit/catalogue-edit.module';
import { EmailCommunicationCreateComponent, EmailCommunicationCreateModule } from '../allors/material/apps/objects/emailcommunication/create/emailcommunication-create.module';
import { GoodCreateComponent, GoodCreateModule } from '../allors/material/apps/objects/good/create/good-create.module';
import { IGoodIdentificationEditComponent, IGoodIdentificationEditModule } from '../allors/material/apps/objects/igoodidentification/edit/igoodIdentification.module';
import { FaceToFaceCommunicationCreateComponent, FaceToFaceCommunicationCreateModule } from '../allors/material/apps/objects/facetofacecommunication/create/facetofacecommunication-create.module';
import { LetterCorrespondenceCreateComponent, LetterCorrespondenceCreateModule } from '../allors/material/apps/objects/lettercorrespondence/create/lettercorrespondence-create.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/apps/objects/organisation/create/organisation-create.module';
import { PartCreateComponent, PartCreateModule } from '../allors/material/apps/objects/part/create/part-create.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/apps/objects/person/create/person-create.module';
import { PhoneCommunicationCreateComponent, PhoneCommunicationCreateModule } from '../allors/material/apps/objects/phonecommunication/create/phonecommunication-create.module';
import { ProductCategoryEditComponent, ProductCategoryEditModule } from '../allors/material/apps/objects/productcategory/edit/productcategory-edit.module';
import { RequestItemEditComponent, RequestItemEditModule } from '../allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from '../allors/material/apps/objects/requestforquote/create/requestforquote-create.module';
import { ProductQuoteCreateComponent, ProductQuoteCreateModule } from '../allors/material/apps/objects/productquote/create/productquote-create.module';
import { QuoteItemEditComponent, QuoteItemEditModule } from '../allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import { SalesOrderCreateComponent, SalesOrderCreateModule } from '../allors/material/apps/objects/salesorder/create/salesorder-create.module';
import { SalesOrderItemEditComponent, SalesOrderItemEditModule } from '../allors/material/apps/objects/salesorderitem/edit/salesorderitem-edit.module';
import { SalesTermEditComponent, SalesTermEditModule } from '../allors/material/apps/objects/salesterm/edit/salesterm-edit.module';
import { WorkTaskCreateModule, WorkTaskCreateComponent } from '../allors/material/apps/objects/worktask/create/worktask-create.module';

import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from '../allors/material/base/services/object';
import { SerialisedItemCharacteristicEditComponent, SerialisedItemCharacteristicEditModule } from 'src/allors/material/apps/objects/serialiseditemcharacteristictype/edit/serialiseditemcharacteristic-edit.module';

const create = {
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.EmailCommunication]: EmailCommunicationCreateComponent,
  [ids.Good]: GoodCreateComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationCreateComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceCreateComponent,
  [ids.Organisation]: OrganisationCreateComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationCreateComponent,
  [ids.ProductQuote]: ProductQuoteCreateComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.RequestForQuote]: RequestForQuoteCreateComponent,
  [ids.SalesOrder]: SalesOrderCreateComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.WorkTask]: WorkTaskCreateComponent,
  [ids.IsbnIdentification]: IGoodIdentificationEditComponent,
  [ids.ManufacturerIdentification]: IGoodIdentificationEditComponent,
  [ids.PartNumber]: IGoodIdentificationEditComponent,
  [ids.EanIdentification]: IGoodIdentificationEditComponent,
  [ids.ProductNumber]: IGoodIdentificationEditComponent,
  [ids.SkuIdentification]: IGoodIdentificationEditComponent,
  [ids.UpcaIdentification]: IGoodIdentificationEditComponent,
  [ids.UpceIdentification]: IGoodIdentificationEditComponent,
  [ids.Part]: PartCreateComponent,
};

const edit = {
  [ids.Catalogue]: CatalogueEditComponent,
  [ids.IncoTerm]: SalesTermEditComponent,
  [ids.InvoiceTerm]: SalesTermEditComponent,
  [ids.OrderTerm]: SalesTermEditComponent,
  [ids.ProductCategory]: ProductCategoryEditComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.SalesOrderItem]: SalesOrderItemEditComponent,
  [ids.IsbnIdentification]: IGoodIdentificationEditComponent,
  [ids.ManufacturerIdentification]: IGoodIdentificationEditComponent,
  [ids.PartNumber]: IGoodIdentificationEditComponent,
  [ids.EanIdentification]: IGoodIdentificationEditComponent,
  [ids.ProductNumber]: IGoodIdentificationEditComponent,
  [ids.SerialisedItemCharacteristicType]: SerialisedItemCharacteristicEditComponent,
  [ids.SkuIdentification]: IGoodIdentificationEditComponent,
  [ids.UpcaIdentification]: IGoodIdentificationEditComponent,
  [ids.UpceIdentification]: IGoodIdentificationEditComponent,
};

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,

    CatalogueEditModule,
    EmailCommunicationCreateModule,
    FaceToFaceCommunicationCreateModule,
    GoodCreateModule,
    IGoodIdentificationEditModule,
    LetterCorrespondenceCreateModule,
    OrganisationCreateModule,
    PartCreateModule,
    PersonCreateModule,
    PhoneCommunicationCreateModule,
    ProductCategoryEditModule,
    ProductQuoteCreateModule,
    QuoteItemEditModule,
    RequestItemEditModule,
    RequestForQuoteCreateModule,
    SalesOrderCreateModule,
    SalesOrderItemEditModule,
    SalesTermEditModule,
    SerialisedItemCharacteristicEditModule,
    WorkTaskCreateModule,
  ],
  entryComponents: [
    CatalogueEditComponent,
    EmailCommunicationCreateComponent,
    FaceToFaceCommunicationCreateComponent,
    GoodCreateComponent,
    IGoodIdentificationEditComponent,
    LetterCorrespondenceCreateComponent,
    OrganisationCreateComponent,
    PersonCreateComponent,
    PartCreateComponent,
    PhoneCommunicationCreateComponent,
    ProductCategoryEditComponent,
    ProductQuoteCreateComponent,
    QuoteItemEditComponent,
    RequestItemEditComponent,
    RequestForQuoteCreateComponent,
    SalesOrderCreateComponent,
    SalesOrderItemEditComponent,
    SalesTermEditComponent,
    SerialisedItemCharacteristicEditComponent,
    WorkTaskCreateComponent,
  ],
  providers: [
    ObjectService,
    { provide: OBJECT_CREATE_TOKEN, useValue: create },
    { provide: OBJECT_EDIT_TOKEN, useValue: edit },
  ]
})
export class ObjectModule {

  constructor(@Optional() @SkipSelf() core: ObjectModule) {
    if (core) {
      throw new Error('Use FactoryModule from AppModule');
    }
  }

}
