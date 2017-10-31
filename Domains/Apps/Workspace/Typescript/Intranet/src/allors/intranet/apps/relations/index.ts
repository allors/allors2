export * from "./overview.module";
export * from "./dashboard/dashboard.module";

export * from "./people/person/person-overview.module";
export * from "./people/person/person.module";
export * from "./people/people-overview.module";

export * from "./organisations/organisations-overview.module";
export * from "./organisations/organisation/organisation-overview.module";
export * from "./organisations/organisation/organisation.module";
export * from "./organisations/organisation/contactrelationship/organisation-contactrelationship-add.module";
export * from "./organisations/organisation/contactrelationship/organisation-contactrelationship-edit.module";

export * from "./communicationevents/communicationevents-overview.module";
export * from "./communicationevents/communicationevent/communicationevents-overview.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { PeopleOverviewModule } from "./people/people-overview.module";
import { PersonOverviewModule } from "./people/person/person-overview.module";
import { PersonModule } from "./people/person/person.module";

import { OrganisationContactrelationshipAddModule } from "./organisations/organisation/contactrelationship/organisation-contactrelationship-add.module";
import { OrganisationContactrelationshipEditModule } from "./organisations/organisation/contactrelationship/organisation-contactrelationship-edit.module";
import { OrganisationOverviewModule } from "./organisations/organisation/organisation-overview.module";
import { OrganisationModule } from "./organisations/organisation/organisation.module";
import { OrganisationsOverviewModule } from "./organisations/organisations-overview.module";

import { CommunicationEventOverviewModule } from "./communicationevents/communicationevent/communicationevents-overview.module";
import { CommunicationEventsOverviewModule } from "./communicationevents/communicationevents-overview.module";

export const Modules = [
  OverviewModule,
  DashboardModule,
  PeopleOverviewModule, PersonOverviewModule, PersonModule,
  OrganisationContactrelationshipAddModule , OrganisationContactrelationshipEditModule, OrganisationOverviewModule, OrganisationModule, OrganisationsOverviewModule,
  CommunicationEventsOverviewModule,
  CommunicationEventOverviewModule,
];
