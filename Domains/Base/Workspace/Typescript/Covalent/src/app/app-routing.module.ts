import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './common/auth/login.component';
import { MainComponent } from './common/main/main.component';

import * as relations from '../allors/covalent/custom/relations';

import { RELATIONS_ROUTING } from '../allors/covalent/custom/relations';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/relations', pathMatch: 'full', },
  {
    canActivate: [AuthenticationService],
    path: '', component: MainComponent,
    children: [
      {
        path: 'relations', component: relations.RelationsComponent,
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
  LoginComponent,
  MainComponent,
  RELATIONS_ROUTING,
];
