import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './common/auth/login.component';
import { MainComponent } from './common/main/main.component';
import { DashboardComponent } from './common/dashboard/dashboard.component';

import * as relations from '../allors/covalent/custom/relations';

import { RELATIONS_ROUTING } from '../allors/covalent/custom/relations';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    canActivate: [AuthenticationService],
    path: '', component: MainComponent,
    children: [
      {
        path: '', component: DashboardComponent,
      },
      {
        path: 'relations',
        children: [
          {
            path: '', component: relations.DashboardComponent,
          }, {
            path: 'organisations',
            children: [
              { path: '', component: relations.OrganisationsComponent },
              { path: 'add', component: relations.OrganisationComponent },
              { path: ':id/edit', component: relations.OrganisationComponent },
              { path: ':id/overview', component: relations.OrganisationOverviewComponent },
            ],
          }, {
            path: 'people',
            children: [
              { path: '', component: relations.PeopleComponent },
              { path: 'add', component: relations.PersonComponent },
              { path: ':id/edit', component: relations.PersonComponent },
              { path: ':id/overview', component: relations.PersonOverviewComponent },
            ],
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
  ],
  exports: [
    RouterModule,
  ],
})
export class AppRoutingModule { }
export const routedComponents: any[] = [
  MainComponent, LoginComponent, DashboardComponent,
  RELATIONS_ROUTING,
];
