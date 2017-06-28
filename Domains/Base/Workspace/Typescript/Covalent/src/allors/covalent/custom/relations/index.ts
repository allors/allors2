import { PeopleComponent } from './people/people.component';
import { PersonFormComponent } from './people/person/person.component';
import { PersonOverviewComponent } from './people/person/person-overview.component';
import { RelationComponent } from './relation.component';
import { RelationDashboardComponent } from './dashboard/relation-dashboard.component';

export const RELATIONS_ROUTING: any[] = [
  PeopleComponent, PersonFormComponent, PersonOverviewComponent,
  RelationComponent, RelationDashboardComponent,
];

export {
  PeopleComponent,
  PersonFormComponent,
  PersonOverviewComponent,
  RelationComponent,
  RelationDashboardComponent,
};
