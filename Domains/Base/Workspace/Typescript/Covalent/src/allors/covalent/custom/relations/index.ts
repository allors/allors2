import { RelationsHeaderComponent } from './relations-header.component';
import { RelationsToolbarComponent } from './relations-toolbar.component';
import { RelationsNavigationComponent } from './relations-navigation.component';

import { RelationsComponent } from './relations.component';
import { OrganisationsComponent } from './organisations/organisations.component';
import { OrganisationComponent } from './organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './organisations/organisation/organisation-overview.component';
import { PeopleComponent } from './people/people.component';
import { PersonComponent } from './people/person/person.component';
import { PersonOverviewComponent } from './people/person/person-overview.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const RELATIONS: any[] = [
  RelationsHeaderComponent,
  RelationsToolbarComponent,
  RelationsNavigationComponent,
];

export const RELATIONS_ROUTING: any[] = [
  RelationsComponent, DashboardComponent,
  OrganisationsComponent, OrganisationComponent, OrganisationOverviewComponent,
  PeopleComponent, PersonComponent, PersonOverviewComponent,
];

export {
  RelationsNavigationComponent,
  RelationsToolbarComponent,

  RelationsComponent,
  DashboardComponent,
  OrganisationsComponent,
  OrganisationComponent,
  OrganisationOverviewComponent,
  PeopleComponent,
  PersonComponent,
  PersonOverviewComponent,
};
