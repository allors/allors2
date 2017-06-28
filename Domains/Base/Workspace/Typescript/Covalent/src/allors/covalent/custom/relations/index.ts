import { LayoutComponent } from './layout.component';
import { PeopleComponent } from './people/people.component';
import { PersonComponent } from './people/person/person.component';
import { PersonOverviewComponent } from './people/person/person-overview.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const RELATIONS_ROUTING: any[] = [
  LayoutComponent, DashboardComponent,
  PeopleComponent, PersonComponent, PersonOverviewComponent,
];

export {
  LayoutComponent,
  DashboardComponent,
  PeopleComponent,
  PersonComponent,
  PersonOverviewComponent,
};
