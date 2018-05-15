export * from './dashboard/dashboard.module';
export * from './organisations/organisation/organisation-overview.module';
export * from './organisations/organisation/organisation.module';
export * from './organisations/organisations.module';
export * from './people/person/person-overview.module';
export * from './people/person/person.module';
export * from './people/people.module';
export * from './relations.module';


import { DashboardModule } from './dashboard/dashboard.module';
import { OrganisationOverviewModule } from './organisations/organisation/organisation-overview.module';
import { OrganisationModule } from './organisations/organisation/organisation.module';
import { OrganisationsModule } from './organisations/organisations.module';
import { PersonOverviewModule } from './people/person/person-overview.module';
import { PersonModule } from './people/person/person.module';
import { PeopleModule } from './people/people.module';
import { RelationsModule } from './relations.module';

export const Modules = [
    DashboardModule,
    OrganisationOverviewModule,
    OrganisationModule,
    OrganisationsModule,
    PersonOverviewModule,
    PersonModule,
    PeopleModule,
    RelationsModule,
];
