import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { ids } from '../allors/meta/generated';

import { FactoryService, FactoryConfig } from '../allors/angular/base/factory';

import { EmailCommunicationCreateComponent, EmailCommunicationCreateModule } from '../allors/material/apps/objects/emailcommunication/create/emailcommunication-create.module';
import { FaceToFaceCommunicationCreateComponent, FaceToFaceCommunicationCreateModule } from '../allors/material/apps/objects/facetofacecommunication/create/facetofacecommunication-create.module';
import { LetterCorrespondenceCreateComponent, LetterCorrespondenceCreateModule } from '../allors/material/apps/objects/lettercorrespondence/create/lettercorrespondence-create.module';
import { OrganisationCreateModule, OrganisationCreateComponent } from '../allors/material/apps/objects/organisation/create/organisation-create.module';
import { PersonCreateModule, PersonCreateComponent } from '../allors/material/apps/objects/person/create/person-create.module';
import { PhoneCommunicationCreateComponent } from '../allors/material/apps/objects/phonecommunication/create/phonecommunication-create.component';
import { PhoneCommunicationCreateModule } from 'src/allors/material/apps/objects/phonecommunication/create/phonecommunication-create.module';

const factoryConfig: FactoryConfig = new FactoryConfig({
  items:
    [
      { id: ids.EmailCommunication, component: EmailCommunicationCreateComponent },
      { id: ids.FaceToFaceCommunication, component: FaceToFaceCommunicationCreateComponent },
      { id: ids.LetterCorrespondence, component: LetterCorrespondenceCreateComponent },
      { id: ids.Organisation, component: OrganisationCreateComponent },
      { id: ids.Person, component: PersonCreateComponent },
      { id: ids.PhoneCommunication, component: PhoneCommunicationCreateComponent },
    ]
});

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
  ],
  entryComponents: [
    EmailCommunicationCreateComponent,
    FaceToFaceCommunicationCreateComponent,
    LetterCorrespondenceCreateComponent,
    OrganisationCreateComponent,
    PersonCreateComponent,
    PhoneCommunicationCreateComponent
  ],
  providers: [
    FactoryService,
    { provide: FactoryConfig, useValue: factoryConfig },
  ]
})
export class FactoryModule {

  constructor(@Optional() @SkipSelf() core: FactoryModule) {
    if (core) {
      throw new Error('Use FactoryModule from AppModule');
    }
  }

}
