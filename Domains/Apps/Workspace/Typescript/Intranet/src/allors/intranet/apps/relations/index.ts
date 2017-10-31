export * from "./overview.module";
export * from "./dashboard/dashboard.module";
export * from "./people/person/person-overview.module";
export * from "./people/person/person.module";
export * from "./people/people-overview.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { PeopleOverviewModule } from "./people/people-overview.module";
import { PersonOverviewModule } from "./people/person/person-overview.module";
import { PersonModule } from "./people/person/person.module";

export const Modules = [
  OverviewModule,
  DashboardModule,
  PeopleOverviewModule, PersonOverviewModule, PersonModule,
];
