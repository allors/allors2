export * from './person/person-overview.module';
export * from './person/person.module';
export * from './person/people-export.module';
export * from './person/people-overview.module';

export * from './organisation/organisations-overview.module';
export * from './organisation/organisation-overview.module';
export * from './organisation/organisation.module';

export * from './party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module';
export * from './party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module';
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

import { PeopleExportModule } from './person/people-export.module';
import { PeopleOverviewModule } from './person/people-overview.module';
import { PersonOverviewModule } from './person/person-overview.module';
import { PersonModule } from './person/person.module';

import { OrganisationOverviewModule } from './organisation/organisation-overview.module';
import { OrganisationModule } from './organisation/organisation.module';
import { OrganisationsOverviewModule } from './organisation/organisations-overview.module';

import { PartyContactMechanismEmailAddressAddModule } from './party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module';
import { PartyContactMechanismEmailAddressEditModule } from './party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module';
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
  PartyContactMechanismEmailAddressAddModule, PartyContactMechanismEmailAddressEditModule, PartyContactMechanismAddPostalAddressModule, PartyContactMechanismPostalAddressEditModule, PartyContactMechanismTelecommunicationsNumberAddModule, PartyContactMechanismTelecommunicationsNumberEditModule, PartyContactMechanismAddWebAddressModule, PartyContactMechanismEditWebAddressModule,
  PartyCommunicationEventEmailCommunicationModule, PartyCommunicationEventFaceToFaceCommunicationModule, PartyCommunicationEventLetterCorrespondenceModule, PartyCommunicationEventPhoneCommunicationModule, 
  CommunicationEventWorkTaskModule, CommunicationEventsOverviewModule, CommunicationEventOverviewModule,
];
