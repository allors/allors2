import { RelationsComponent } from './relations.component';
import { OrganisationsComponent } from './organisations/organisations.component';
import { OrganisationComponent } from './organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './organisations/organisation/organisation-overview.component';
import { PeopleComponent } from './people/people.component';
import { PersonComponent } from './people/person/person.component';
import { PersonOverviewComponent } from './people/person/person-overview.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const RELATIONS: any[] = [
  RelationsComponent,
];

export const RELATIONS_ROUTING: any[] = [
  DashboardComponent,
  OrganisationsComponent, OrganisationComponent, OrganisationOverviewComponent,
  PeopleComponent, PersonComponent, PersonOverviewComponent,
];

export {
  RelationsComponent,

  DashboardComponent,
  OrganisationsComponent,
  OrganisationComponent,
  OrganisationOverviewComponent,
  PeopleComponent,
  PersonComponent,
  PersonOverviewComponent,
};
