import { LayoutComponent } from './layout.component';

// Routing
import { EmailAddressAddComponent } from './contactMechanisms/contactMechanism/emailAddressAdd.component';
import { EmailAddressEditComponent } from './contactMechanisms/contactMechanism/emailAddressEdit.component';
import { EmailAddressInlineComponent } from './contactMechanisms/contactMechanism/emailAddress-inline.component';
import { EmailCommunicationFormComponent } from './communicationevents/communicationevent/emailCommunication.component';
import { FaceToFaceCommunicationFormComponent } from './communicationevents/communicationevent/faceToFaceCommunication.component';
import { LetterCorrespondenceFormComponent } from './communicationevents/communicationevent/letterCorrespondence.component';
import { OrganisationAddContactComponent } from './organisations/organisation/organisationAddContact.component';
import { OrganisationEditContactComponent } from './organisations/organisation/organisationEditContact.component';
import { OrganisationFormComponent } from './organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './organisations/organisation/organisation-overview.component';
import { OrganisationsComponent } from './organisations/organisations.component';
import { PeopleComponent } from './people/people.component';
import { PersonFormComponent } from './people/person/person.component';
import { PersonInlineComponent } from './people/person/person-inline.component';
import { PersonOverviewComponent } from './people/person/person-overview.component';
import { PhoneCommunicationFormComponent } from './communicationevents/communicationevent/phoneCommunication.component';
import { PostalAddressAddComponent } from './contactMechanisms/contactMechanism/postalAddressAdd.component';
import { PostalAddressEditComponent } from './contactMechanisms/contactMechanism/postalAddressEdit.component';
import { PostalAddressInlineComponent } from './contactMechanisms/contactMechanism/postaladdress-inline.component';
import { RelationDashboardComponent } from './dashboard/relation-dashboard.component';
import { TelecommunicationsNumberInlineComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumber-inline.component';
import { TelecommunicationsNumberAddComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberAdd.component';
import { TelecommunicationsNumberEditComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberEdit.component';
import { WebAddressAddComponent } from './contactMechanisms/contactMechanism/webAddressAdd.component';
import { WebAddressEditComponent } from './contactMechanisms/contactMechanism/webAddressEdit.component';

export const RELATIONS: any[] = [
  LayoutComponent,
];

export const RELATIONS_ROUTING: any[] = [
  EmailAddressAddComponent, EmailAddressEditComponent, EmailAddressInlineComponent, EmailCommunicationFormComponent, FaceToFaceCommunicationFormComponent, LetterCorrespondenceFormComponent,
  OrganisationAddContactComponent, OrganisationEditContactComponent, OrganisationFormComponent, OrganisationOverviewComponent, OrganisationsComponent,
  PeopleComponent, PersonFormComponent, PersonInlineComponent, PersonOverviewComponent, PhoneCommunicationFormComponent,
  PostalAddressAddComponent, PostalAddressEditComponent, PostalAddressInlineComponent,
  RelationDashboardComponent,
  TelecommunicationsNumberInlineComponent, TelecommunicationsNumberAddComponent, TelecommunicationsNumberEditComponent,
  WebAddressAddComponent, WebAddressEditComponent,
];

export {
  // Routing
  EmailAddressAddComponent,
  EmailAddressEditComponent,
  EmailCommunicationFormComponent,
  FaceToFaceCommunicationFormComponent,
  LetterCorrespondenceFormComponent,
  OrganisationAddContactComponent,
  OrganisationEditContactComponent,
  OrganisationFormComponent,
  OrganisationOverviewComponent,
  OrganisationsComponent,
  PeopleComponent,
  PersonFormComponent,
  PersonOverviewComponent,
  PhoneCommunicationFormComponent,
  PostalAddressAddComponent,
  PostalAddressEditComponent,
  PostalAddressInlineComponent,
  RelationDashboardComponent,
  TelecommunicationsNumberAddComponent,
  TelecommunicationsNumberEditComponent,
  WebAddressAddComponent,
  WebAddressEditComponent,
};
