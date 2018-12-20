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
        path: 'dashboard', component: DashboardComponent,
      },
      {
        path: 'contacts',
        children: [
          { path: 'organisations', component: relations.OrganisationsComponent },
          { path: 'organisation/:id', component: relations.OrganisationOverviewComponent },
          { path: 'addorganisation', component: relations.OrganisationComponent },
          { path: 'editorganisation/:id', component: relations.OrganisationComponent },

          { path: 'people', component: relations.PeopleComponent },
          { path: 'person/:id', component: relations.PersonOverviewComponent },
          { path: 'addperson', component: relations.PersonComponent },
          { path: 'editperson/:id', component: relations.PersonComponent },
        ],
      },
      {
        path: 'tests',
        children: [
          {
            path: 'form', component: tests.FormComponent
          },
        ],
      },
    ],
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
