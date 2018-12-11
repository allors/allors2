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
import { PhoneCommunicationCreateModule } from 'src/allors/material/apps/objects/phonecommunication/create/phonecommunication-create.module';
import { RequestItemEditComponent, RequestItemEditModule } from 'src/allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import { RequestForQuoteCreateComponent, RequestForQuoteCreateModule } from 'src/allors/material/apps/objects/requestforquote/create/requestforquote-create.module';

const create = {
  [ids.EmailCommunication]: EmailCommunicationCreateComponent,
  [ids.FaceToFaceCommunication]: FaceToFaceCommunicationCreateComponent,
  [ids.LetterCorrespondence]: LetterCorrespondenceCreateComponent,
  [ids.Organisation]: OrganisationCreateComponent,
  [ids.Person]: PersonCreateComponent,
  [ids.PhoneCommunication]: PhoneCommunicationCreateComponent,
  [ids.RequestItem]: RequestItemEditComponent,
  [ids.RequestForQuote]: RequestForQuoteCreateComponent
};

const edit = {
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
