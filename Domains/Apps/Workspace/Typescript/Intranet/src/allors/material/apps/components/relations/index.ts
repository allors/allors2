export * from './person/overview/person-overview.module';
export * from './person/edit/person-edit.module';
export * from './person/export/person-export.module';
export * from './person/list/person-list.module';

export * from './organisation/list/organisation-list.module';
export * from './organisation/overview/organisation-overview.module';
export * from './organisation/edit/organisation-edit.module';

export * from './emailaddress/edit/emailaddress-edit.module';
export * from './postaladdress/edit/postaladdress-edit.module';
export * from './telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';

export * from './webaddress/edit/webaddress-edit.module';

export * from './emailcommunication/edit/emailcommunication.module';
export * from './facetofacecommunication/edit/facetofacecommunication.module';
export * from './lettercorrespondence/edit/lettercorrespondence.module';
export * from './phonecommunication/edit/phonecommunication.module';

export * from './communicationevent/overview/communicationevent-overview.module';
export * from './communicationevent/list/communicationevent-list.module';
export * from './communicationevent/worktask/communicationevent-worktask.module';

import { PeopleExportModule } from './person/export/person-export.module';
import { PeopleOverviewModule } from './person/list/person-list.module';
import { PersonOverviewModule } from './person/overview/person-overview.module';
import { PersonModule } from './person/edit/person-edit.module';

import { OrganisationOverviewModule } from './organisation/overview/organisation-overview.module';
import { OrganisationModule } from './organisation/edit/organisation-edit.module';
import { OrganisationsOverviewModule } from './organisation/list/organisation-list.module';

import { EmailAddressdModule } from './emailaddress/edit/emailaddress-edit.module';
import { PostalAddressModule } from './postaladdress/edit/postaladdress-edit.module';
import { TelecommunicationsNumberModule } from './telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import { EditWebAddressModule } from './webaddress/edit/webaddress-edit.module';

import { EmailCommunicationModule } from './emailcommunication/edit/emailcommunication.module';
import { FaceToFaceCommunicationModule } from './facetofacecommunication/edit/facetofacecommunication.module';
import { LetterCorrespondenceModule } from './/lettercorrespondence/edit/lettercorrespondence.module';
import { PhoneCommunicationModule } from './phonecommunication/edit/phonecommunication.module';

import { CommunicationEventOverviewModule } from './communicationevent/overview/communicationevent-overview.module';
import { CommunicationEventsOverviewModule } from './communicationevent/list/communicationevent-list.module';
import { CommunicationEventWorkTaskModule } from './communicationevent/worktask/communicationevent-worktask.module';

export const Modules = [
  PeopleExportModule, PeopleOverviewModule, PersonOverviewModule, PersonModule,
  OrganisationOverviewModule, OrganisationModule, OrganisationsOverviewModule,
  EmailAddressdModule, PostalAddressModule, TelecommunicationsNumberModule, EditWebAddressModule,
  EmailCommunicationModule, FaceToFaceCommunicationModule, LetterCorrespondenceModule, PhoneCommunicationModule,
  CommunicationEventWorkTaskModule, CommunicationEventsOverviewModule, CommunicationEventOverviewModule,
];
