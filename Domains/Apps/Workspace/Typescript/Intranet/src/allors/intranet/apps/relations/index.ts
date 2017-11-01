export * from "./overview.module";
export * from "./dashboard/dashboard.module";

export * from "./people/person/person-overview.module";
export * from "./people/person/person.module";
export * from "./people/people-export.module";
export * from "./people/people-overview.module";

export * from "./organisations/organisations-overview.module";
export * from "./organisations/organisation/organisation-overview.module";
export * from "./organisations/organisation/organisation.module";
export * from "./organisations/organisation/contactrelationship/organisation-contactrelationship-add.module";
export * from "./organisations/organisation/contactrelationship/organisation-contactrelationship-edit.module";

export * from "./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module";
export * from "./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module";
export * from "./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module";
export * from "./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module";
export * from "./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module";
export * from "./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module";
export * from "./party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module";
export * from "./party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module";

export * from "./party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module";
export * from "./party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module";
export * from "./party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module";
export * from "./party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module";
export * from "./party/communicationevent/worktask/party-communicationevent-worktask.module";

export * from "./communicationevents/communicationevents-overview.module";
export * from "./communicationevents/communicationevent/communicationevents-overview.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { PeopleExportModule } from "./people/people-export.module";
import { PeopleOverviewModule } from "./people/people-overview.module";
import { PersonOverviewModule } from "./people/person/person-overview.module";
import { PersonModule } from "./people/person/person.module";

import { OrganisationContactrelationshipAddModule } from "./organisations/organisation/contactrelationship/organisation-contactrelationship-add.module";
import { OrganisationContactrelationshipEditModule } from "./organisations/organisation/contactrelationship/organisation-contactrelationship-edit.module";
import { OrganisationOverviewModule } from "./organisations/organisation/organisation-overview.module";
import { OrganisationModule } from "./organisations/organisation/organisation.module";
import { OrganisationsOverviewModule } from "./organisations/organisations-overview.module";

import { PartyContactMechanismEmailAddressAddModule } from "./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-add.module";
import { PartyContactMechanismEmailAddressEditModule } from "./party/contactmechanism/emailaddress/party-contactmechanism-emailaddress-edit.module";
import { PartyContactMechanismAddPostalAddressModule } from "./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-add.module";
import { PartyContactMechanismPostalAddressEditModule } from "./party/contactmechanism/postaladdress/party-contactmechanism-postaladdress-edit.module";
import { PartyContactMechanismTelecommunicationsNumberAddModule } from "./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-add.module";
import { PartyContactMechanismTelecommunicationsNumberEditModule } from "./party/contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-edit.module";
import { PartyContactMechanismAddWebAddressModule } from "./party/contactmechanism/webaddress/party-contactmechanism-webaddress-add.module";
import { PartyContactMechanismEditWebAddressModule } from "./party/contactmechanism/webaddress/party-contactmechanism-webaddress-edit.module";

import { PartyCommunicationEventEmailCommunicationModule } from "./party/communicationevent/emailcommunication/party-communicationevent-emailcommunication.module";
import { PartyCommunicationEventFaceToFaceCommunicationModule } from "./party/communicationevent/facetofacecommunication/party-communicationevent-facetofacecommunication.module";
import { PartyCommunicationEventLetterCorrespondenceModule } from "./party/communicationevent/lettercorrespondence/party-communicationevent-lettercorrespondence.module";
import { PartyCommunicationEventPhoneCommunicationModule } from "./party/communicationevent/phonecommunication/party-communicationevent-phonecommunication.module";
import { PartyCommunicationEventWorkTaskModule } from "./party/communicationevent/worktask/party-communicationevent-worktask.module";

import { CommunicationEventOverviewModule } from "./communicationevents/communicationevent/communicationevents-overview.module";
import { CommunicationEventsOverviewModule } from "./communicationevents/communicationevents-overview.module";

export const Modules = [
  OverviewModule,
  DashboardModule,
  PeopleExportModule, PeopleOverviewModule, PersonOverviewModule, PersonModule,
  OrganisationContactrelationshipAddModule , OrganisationContactrelationshipEditModule, OrganisationOverviewModule, OrganisationModule, OrganisationsOverviewModule,
  PartyContactMechanismEmailAddressAddModule, PartyContactMechanismEmailAddressEditModule, PartyContactMechanismAddPostalAddressModule, PartyContactMechanismPostalAddressEditModule, PartyContactMechanismTelecommunicationsNumberAddModule, PartyContactMechanismTelecommunicationsNumberEditModule, PartyContactMechanismAddWebAddressModule, PartyContactMechanismEditWebAddressModule,
  PartyCommunicationEventEmailCommunicationModule, PartyCommunicationEventFaceToFaceCommunicationModule, PartyCommunicationEventLetterCorrespondenceModule, PartyCommunicationEventPhoneCommunicationModule, PartyCommunicationEventWorkTaskModule,
  CommunicationEventsOverviewModule,
  CommunicationEventOverviewModule,
];
