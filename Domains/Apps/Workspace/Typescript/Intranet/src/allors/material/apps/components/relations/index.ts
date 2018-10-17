export * from './person/overview/person-overview.module';
export * from './person/edit/person-edit.module';
export * from './person/export/person-export.module';
export * from './person/list/person-list.module';

export * from './organisation/list/organisation-list.module';
export * from './organisation/overview/organisation-overview.module';
export * from './organisation/edit/organisation-edit.module';

export * from './emailaddress/edit/emailaddress-edit.module';

export * from './party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module';
export * from './party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module';
export * from './party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module';
export * from './party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module';
export * from './party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module';
export * from './party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module';

export * from './party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module';
export * from './party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module';
export * from './party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module';
export * from './party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module';

export * from './communicationevent/communicationevent-overview.module';
export * from './communicationevent/communicationevents-overview.module';
export * from './communicationevent/worktask/communicationevent-worktask.module';

import { PeopleExportModule } from './person/export/person-export.module';
import { PeopleOverviewModule } from './person/list/person-list.module';
import { PersonOverviewModule } from './person/overview/person-overview.module';
import { PersonModule } from './person/edit/person-edit.module';

import { OrganisationOverviewModule } from './organisation/overview/organisation-overview.module';
import { OrganisationModule } from './organisation/edit/organisation-edit.module';
import { OrganisationsOverviewModule } from './organisation/list/organisation-list.module';

import { PartyContactMechanismEmailAddressAddModule } from './emailaddress/edit/emailaddress-edit.module';

import { PartyContactMechanismAddPostalAddressModule } from './party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module';
import { PartyContactMechanismPostalAddressEditModule } from './party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module';
import { PartyContactMechanismTelecommunicationsNumberAddModule } from './party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module';
import { PartyContactMechanismTelecommunicationsNumberEditModule } from './party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module';
import { PartyContactMechanismAddWebAddressModule } from './party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module';
import { PartyContactMechanismEditWebAddressModule } from './party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module';

import { PartyCommunicationEventEmailCommunicationModule } from './party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module';
import { PartyCommunicationEventFaceToFaceCommunicationModule } from './party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module';
import { PartyCommunicationEventLetterCorrespondenceModule } from './party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module';
import { PartyCommunicationEventPhoneCommunicationModule } from './party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module';

import { CommunicationEventOverviewModule } from './communicationevent/communicationevent-overview.module';
import { CommunicationEventsOverviewModule } from './communicationevent/communicationevents-overview.module';
import { CommunicationEventWorkTaskModule } from './communicationevent/worktask/communicationevent-worktask.module';

export const Modules = [
  PeopleExportModule, PeopleOverviewModule, PersonOverviewModule, PersonModule,
  OrganisationOverviewModule, OrganisationModule, OrganisationsOverviewModule,
  PartyContactMechanismEmailAddressAddModule, PartyContactMechanismAddPostalAddressModule, PartyContactMechanismPostalAddressEditModule, PartyContactMechanismTelecommunicationsNumberAddModule, PartyContactMechanismTelecommunicationsNumberEditModule, PartyContactMechanismAddWebAddressModule, PartyContactMechanismEditWebAddressModule,
  PartyCommunicationEventEmailCommunicationModule, PartyCommunicationEventFaceToFaceCommunicationModule, PartyCommunicationEventLetterCorrespondenceModule, PartyCommunicationEventPhoneCommunicationModule,
  CommunicationEventWorkTaskModule, CommunicationEventsOverviewModule, CommunicationEventOverviewModule,
];
