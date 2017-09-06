// Overview
import { PartyCommunicationEventOverviewComponent } from "./overview/communicationevent/communicationeventoverview.component";
import { CommunicationEventsOverviewComponent } from "./overview/communicationevents/communicationeventsoverview.component";
import { OrganisationOverviewComponent } from "./overview/organisation/organisationOverview.component";
import { OrganisationsOverviewComponent } from "./overview/organisations/organisationsOverview.component";
import { OverviewComponent } from "./overview/overview.component";
import { PeopleOverviewComponent } from "./overview/people/peopleOverview.component";
import { PersonOverviewComponent } from "./overview/person/personOverview.component";
import { RelationsOverviewComponent } from "./overview/relations/relationsOverview.component";

// Party
import { PartyContactMechanismAddEmailAddressComponent } from "./party/contactMechanism/emailaddress/add.component";
import { PartyContactMechanismEditEmailAddressComponent } from "./party/contactMechanism/emailaddress/edit.component";
import { PartyContactMechanismInlineEmailAddressComponent } from "./party/contactMechanism/emailaddress/inline.component";
import { PartyContactMechanismAddPostalAddressComponent } from "./party/contactMechanism/postaladdress/add.component";
import { PartyContactMechanismEditPostalAddressComponent } from "./party/contactMechanism/postaladdress/edit.component";
import { PartyContactMechanismInlinePostalAddressComponent } from "./party/contactMechanism/postaladdress/inline.component";
import { PartyContactMechanismAddTelecommunicationsNumberComponent } from "./party/contactMechanism/telecommunicationsnumber/add.component";
import { PartyContactMechanismEditTelecommunicationsNumberComponent } from "./party/contactMechanism/telecommunicationsnumber/edit.component";
import { PartyContactMechanismInlineTelecommunicationsNumberComponent } from "./party/contactMechanism/telecommunicationsnumber/inline.component";
import { PartyContactMechanismAddWebAddressComponent } from "./party/contactMechanism/webAddress/add.component";
import { PartyContactMechanismEditWebAddressComponent } from "./party/contactMechanism/webAddress/edit.component";

import { PartyCommunicationEventEditEmailCommunicationComponent } from "./party/communicationevent/emailCommunication/edit.component";
import { PartyCommunicationEventEditFaceToFaceCommunicationComponent } from "./party/communicationevent/faceToFaceCommunication/edit.component";
import { PartyCommunicationEventEditLetterCorrespondenceComponent } from "./party/communicationevent/letterCorrespondence/edit.component";
import { PartyCommunicationEventEditPhoneCommunicationComponent } from "./party/communicationevent/phoneCommunication/edit.component";
import { PartyCommunicationEventAddWorkTaskComponent } from "./party/communicationevent/worktask/add.component";

// Person
import { PersonEditComponent } from "./person/edit.component";
import { PersonInlineComponent } from "./person/inline.component";

// Organisation
import { OrganisationContactrelationshipAddComponent } from "./organisation/contactrelationship/add.component";
import { OrganisationContactrelationshipEditComponent } from "./organisation/contactrelationship/edit.component";
import { OrganisationEditComponent } from "./organisation/edit.component";

// Export
import { ExportPeopleComponent } from "./export/people/export.people.component";

export const RELATIONS: any[] = [
];

export const RELATIONS_ROUTING: any[] = [
  // Overview
  CommunicationEventsOverviewComponent,
  OverviewComponent,
  RelationsOverviewComponent,
  PeopleOverviewComponent,
  PersonOverviewComponent,
  OrganisationsOverviewComponent,
  OrganisationOverviewComponent,

  // Party
  PartyCommunicationEventAddWorkTaskComponent,
  PartyContactMechanismAddEmailAddressComponent,
  PartyContactMechanismEditEmailAddressComponent,
  PartyContactMechanismInlineEmailAddressComponent,
  PartyCommunicationEventEditEmailCommunicationComponent,
  PartyCommunicationEventEditFaceToFaceCommunicationComponent,
  PartyCommunicationEventEditLetterCorrespondenceComponent,
  PartyCommunicationEventEditPhoneCommunicationComponent,
  PartyCommunicationEventOverviewComponent,
  PartyContactMechanismAddPostalAddressComponent,
  PartyContactMechanismEditPostalAddressComponent,
  PartyContactMechanismInlinePostalAddressComponent,
  PartyContactMechanismInlineTelecommunicationsNumberComponent,
  PartyContactMechanismAddTelecommunicationsNumberComponent,
  PartyContactMechanismEditTelecommunicationsNumberComponent,
  PartyContactMechanismAddWebAddressComponent,
  PartyContactMechanismEditWebAddressComponent,

  // Person
  PersonEditComponent,
  PersonInlineComponent,

  // Organisation
  OrganisationEditComponent,
  OrganisationContactrelationshipAddComponent,
  OrganisationContactrelationshipEditComponent,

  // Export
  ExportPeopleComponent,
];

export {
  // Overview
  CommunicationEventsOverviewComponent,
  OverviewComponent,
  RelationsOverviewComponent,
  PeopleOverviewComponent,
  PersonOverviewComponent,
  OrganisationsOverviewComponent,
  OrganisationOverviewComponent,

  // Party
  PartyCommunicationEventAddWorkTaskComponent,
  PartyContactMechanismAddEmailAddressComponent,
  PartyContactMechanismEditEmailAddressComponent,
  PartyContactMechanismInlineEmailAddressComponent,
  PartyCommunicationEventEditEmailCommunicationComponent,
  PartyCommunicationEventEditFaceToFaceCommunicationComponent,
  PartyCommunicationEventEditLetterCorrespondenceComponent,
  PartyCommunicationEventEditPhoneCommunicationComponent,
  PartyCommunicationEventOverviewComponent,
  PartyContactMechanismAddPostalAddressComponent,
  PartyContactMechanismEditPostalAddressComponent,
  PartyContactMechanismInlinePostalAddressComponent,
  PartyContactMechanismInlineTelecommunicationsNumberComponent,
  PartyContactMechanismAddTelecommunicationsNumberComponent,
  PartyContactMechanismEditTelecommunicationsNumberComponent,
  PartyContactMechanismAddWebAddressComponent,
  PartyContactMechanismEditWebAddressComponent,

  // Person
  PersonEditComponent,
  PersonInlineComponent,

  // Organisation
  OrganisationEditComponent,
  OrganisationContactrelationshipAddComponent,
  OrganisationContactrelationshipEditComponent,

  // Export
  ExportPeopleComponent,
};
