import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';

import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import * as relations from '../allors/material/custom/relations';
import * as tests from '../allors/material/custom/tests';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthorizationService],
    children: [
      {
        path: 'dashboard', component: DashboardComponent, data: { type: 'module', title: 'Home', icon: 'home' },
      },
      {
        path: 'relations', data: { type: 'module', title: 'Relations', icon: 'business' },
        children: [
          {
            children: [
              { path: '', component: relations.OrganisationsComponent, data: { type: 'page', title: 'Organisations' } },
              { path: 'add', component: relations.OrganisationComponent },
              { path: ':id/edit', component: relations.OrganisationComponent },
              { path: ':id/overview', component: relations.OrganisationOverviewComponent },
            ],
            path: 'organisations',
          }, {
            children: [
              { path: '', component: relations.PeopleComponent, data: { type: 'page', title: 'People' }, },
              { path: 'add', component: relations.PersonComponent },
              { path: ':id/edit', component: relations.PersonComponent },
              { path: ':id/overview', component: relations.PersonOverviewComponent },
            ],
            path: 'people',
          },
        ],
      },
      {
        path: 'tests', data: { type: 'module', title: 'Tests', icon: 'build' },
        children: [
          {
            path: 'form', component: tests.FormComponent, data: { type: 'page', title: 'Form' }
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
