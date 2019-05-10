export * from './organisations/organisation/organisation-overview.module';
export * from './organisations/organisation/organisation.module';
export * from './organisations/organisations.module';
export * from './people/person/person-overview.module';
export * from './people/person/person.module';
export * from './people/people.module';

import { OrganisationOverviewModule } from './organisations/organisation/organisation-overview.module';
import { OrganisationModule } from './organisations/organisation/organisation.module';
import { OrganisationsModule } from './organisations/organisations.module';
import { PersonOverviewModule } from './people/person/person-overview.module';
import { PersonModule } from './people/person/person.module';
import { PeopleModule } from './people/people.module';

export const Modules = [
    OrganisationOverviewModule,
    OrganisationModule,
    OrganisationsModule,
    PersonOverviewModule,
    PersonModule,
    PeopleModule,
];
