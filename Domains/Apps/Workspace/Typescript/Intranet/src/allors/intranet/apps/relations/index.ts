export * from "./relations.module";
export * from "./dashboard/dashboard.module";
export * from "./people/person/person-overview.module";
export * from "./people/person/person.module";
export * from "./people/people.module";

import { RelationsModule } from "./relations.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { PeopleModule } from "./people/people.module";
import { PersonOverviewModule } from "./people/person/person-overview.module";
import { PersonModule } from "./people/person/person.module";

export const Modules = [
  RelationsModule,
  DashboardModule,
  PeopleModule, PersonOverviewModule, PersonModule,
];
