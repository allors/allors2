// Overview
import { OverviewComponent } from './overview/overview.component';
import { RelationsOverviewComponent } from './overview/relations/relationsOverview.component';
import { PeopleOverviewComponent } from './overview/people/peopleOverview.component';
import { PersonOverviewComponent } from './overview/person/personOverview.component';
import { OrganisationsOverviewComponent } from './overview/organisations/organisationsOverview.component';
import { OrganisationOverviewComponent } from './overview/organisation/organisationOverview.component';

// Party
import { PartyContactMechanismAddEmailAddressComponent } from './party/contactMechanism/emailaddress/add.component';
import { PartyContactMechanismEditEmailAddressComponent } from './party/contactMechanism/emailaddress/edit.component';
import { PartyContactMechanismInlineEmailAddressComponent } from './party/contactMechanism/emailaddress/inline.component';
import { PartyContactMechanismAddPostalAddressComponent } from './party/contactMechanism/postaladdress/add.component';
import { PartyContactMechanismEditPostalAddressComponent } from './party/contactMechanism/postaladdress/edit.component';
import { PartyContactMechanismInlinePostalAddressComponent } from './party/contactMechanism/postaladdress/inline.component';
import { PartyContactMechanismAddTelecommunicationsNumberComponent } from './party/contactMechanism/telecommunicationsnumber/add.component';
import { PartyContactMechanismEditTelecommunicationsNumberComponent } from './party/contactMechanism/telecommunicationsnumber/edit.component';
import { PartyContactMechanismInlineTelecommunicationsNumberComponent } from './party/contactMechanism/telecommunicationsnumber/inline.component';
import { PartyContactMechanismAddWebAddressComponent } from './party/contactMechanism/webAddress/add.component';
import { PartyContactMechanismEditWebAddressComponent } from './party/contactMechanism/webAddress/edit.component';

import { PartycommunicationEventEditEmailCommunicationComponent } from './party/communicationevent/emailCommunication/edit.component';
import { PartycommunicationEventEditFaceToFaceCommunicationComponent } from './party/communicationevent/faceToFaceCommunication/edit.component';
import { PartycommunicationEventEditLetterCorrespondenceComponent } from './party/communicationevent/letterCorrespondence/edit.component';
import { PartycommunicationEventEditPhoneCommunicationComponent } from './party/communicationevent/phoneCommunication/edit.component';

// Person
import { PersonEditComponent } from './person/edit.component';
import { PersonInlineComponent } from './person/inline.component';

// Organisation
import { OrganisationEditComponent } from './organisation/edit.component';
import { OrganisationContactrelationshipAddComponent } from './organisation/contactrelationship/add.component';
import { OrganisationContactrelationshipEditComponent } from './organisation/contactrelationship/edit.component';

export const RELATIONS: any[] = [
];

export const RELATIONS_ROUTING: any[] = [
  // Overview
  OverviewComponent,
  RelationsOverviewComponent,
  PeopleOverviewComponent,
  PersonOverviewComponent,
  OrganisationsOverviewComponent,
  OrganisationOverviewComponent,

  // Party
  PartyContactMechanismAddEmailAddressComponent,
  PartyContactMechanismEditEmailAddressComponent,
  PartyContactMechanismInlineEmailAddressComponent,
  PartycommunicationEventEditEmailCommunicationComponent,
  PartycommunicationEventEditFaceToFaceCommunicationComponent,
  PartycommunicationEventEditLetterCorrespondenceComponent,
  PartycommunicationEventEditPhoneCommunicationComponent,
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
];

export {
   // Overview
  OverviewComponent,
  RelationsOverviewComponent,
  PeopleOverviewComponent,
  PersonOverviewComponent,
  OrganisationsOverviewComponent,
  OrganisationOverviewComponent,

  // Party
  PartyContactMechanismAddEmailAddressComponent,
  PartyContactMechanismEditEmailAddressComponent,
  PartyContactMechanismInlineEmailAddressComponent,
  PartycommunicationEventEditEmailCommunicationComponent,
  PartycommunicationEventEditFaceToFaceCommunicationComponent,
  PartycommunicationEventEditLetterCorrespondenceComponent,
  PartycommunicationEventEditPhoneCommunicationComponent,
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
};
