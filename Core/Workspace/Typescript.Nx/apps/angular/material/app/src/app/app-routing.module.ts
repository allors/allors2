import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  LoginComponent,
  MainComponent,
  DashboardComponent,
  OrganisationsComponent,
  OrganisationOverviewComponent,
  OrganisationComponent,
  PeopleComponent,
  PersonOverviewComponent,
  PersonComponent,
  FormComponent,
} from '../allors/material/module';

import { AuthorizationService } from '../allors/custom/material/auth';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthorizationService],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'contacts',
        children: [
          { path: 'organisations', component: OrganisationsComponent },
          { path: 'organisation/:id', component: OrganisationOverviewComponent },
          { path: 'addorganisation', component: OrganisationComponent },
          { path: 'editorganisation/:id', component: OrganisationComponent },

          { path: 'people', component: PeopleComponent },
          { path: 'person/:id', component: PersonOverviewComponent },
          { path: 'addperson', component: PersonComponent },
          { path: 'editperson/:id', component: PersonComponent },
        ],
      },
      {
        path: 'tests',
        children: [
          {
            path: 'form',
            component: FormComponent,
          },
        ],
      },
    ],
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
