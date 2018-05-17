import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';

import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import * as relations from '../allors/material/custom/relations';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthorizationService],
    children: [
      {
        path: 'dashboard', component: DashboardComponent,
      },
      {
        path: 'relations',
        children: [
          {
            children: [
              { path: '', component: relations.OrganisationsComponent },
              { path: 'add', component: relations.OrganisationComponent },
              { path: ':id/edit', component: relations.OrganisationComponent },
              { path: ':id/overview', component: relations.OrganisationOverviewComponent },
            ],
            path: 'organisations',
          }, {
            children: [
              { path: '', component: relations.PeopleComponent },
              { path: 'add', component: relations.PersonComponent },
              { path: ':id/edit', component: relations.PersonComponent },
              { path: ':id/overview', component: relations.PersonOverviewComponent },
            ],
            path: 'people',
          },
        ],
      },
    ],
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)],
})
export class AppRoutingModule { }
