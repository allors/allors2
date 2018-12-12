import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { ObjectService, OBJECT_CREATE_TOKEN, OBJECT_EDIT_TOKEN } from '../allors/angular/base/object';

import { EmailCommunicationCreateComponent, EmailCommunicationCreateModule } from '../allors/material/apps/objects/emailcommunication/create/emailcommunication-create.module';
import { FaceToFaceCommunicationCreateComponent, FaceToFaceCommunicationCreateModule } from '../allors/material/apps/objects/facetofacecommunication/create/facetofacecommunication-create.module';
import { LetterCorrespondenceCreateComponent, LetterCorrespondenceCreateModule } from '../allors/material/apps/objects/lettercorrespondence/create/lettercorrespondence-create.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/apps/objects/organisation/create/organisation-create.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/apps/objects/person/create/person-create.module';
import { PhoneCommunicationCreateComponent } from '../allors/material/apps/objects/phonecommunication/create/phonecommunication-create.component';
import { PhoneCommunicationCreateModule } from '../allors/material/apps/objects/phonecommunication/create/phonecommunication-create.module';
import { RequestItemEditComponent, RequestItemEditModule } from '../allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from '../allors/material/apps/objects/requestforquote/create/requestforquote-create.module';
import { ProductQuoteCreateComponent, ProductQuoteCreateModule } from '../allors/material/apps/objects/productquote/create/productquote-create.module';
import { QuoteItemEditComponent, QuoteItemEditModule } from '../allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';

const create = {
  [ids.EmailCommunication]: EmailCommunicationCreateComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationCreateComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceCreateComponent,
  [ids.Organisation]: OrganisationCreateComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationCreateComponent,
  [ids.ProductQuote]: ProductQuoteCreateComponent,
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.RequestForQuote]: RequestForQuoteCreateComponent
};

const edit = {
  [ids.QuoteItem]: QuoteItemEditComponent,
  [ids.RequestItem]: RequestItemEditComponent,
};

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,

    EmailCommunicationCreateModule,
    FaceToFaceCommunicationCreateModule,
    LetterCorrespondenceCreateModule,
    OrganisationCreateModule,
    PersonCreateModule,
    PhoneCommunicationCreateModule,
    ProductQuoteCreateModule,
    QuoteItemEditModule,
    RequestItemEditModule,
    RequestForQuoteCreateModule,
  ],
  entryComponents: [
    EmailCommunicationCreateComponent,
    FaceToFaceCommunicationCreateComponent,
    LetterCorrespondenceCreateComponent,
    OrganisationCreateComponent,
    PersonCreateComponent,
    PhoneCommunicationCreateComponent,
    ProductQuoteCreateComponent,
    QuoteItemEditComponent,
    RequestItemEditComponent,
    RequestForQuoteCreateComponent,
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
