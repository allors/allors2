import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';

import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import * as relations from '../allors/material/custom/relations';
import * as tests from '../allors/material/custom/tests';
import { moduleData, listData, overviewData, editData, addData } from 'src/allors/angular';
import { ids } from 'src/allors/meta';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthorizationService],
    children: [
      {
        path: 'dashboard', component: DashboardComponent, data: moduleData({ title: 'Home', icon: 'home' }),
      },
      {
        path: 'relations', data: moduleData({ title: 'Relations', icon: 'business' }),
        children: [
          {
            children: [
              { path: '', component: relations.OrganisationsComponent, data: listData({ id: ids.Organisation, title: 'Organisations', icon: 'business' }) },
              { path: 'add', component: relations.OrganisationComponent },
              { path: ':id/edit', component: relations.OrganisationComponent },
              { path: ':id/overview', component: relations.OrganisationOverviewComponent, data: overviewData({ id: ids.Organisation }) },
            ],
            path: 'organisations',
          }, {
            children: [
              { path: '', component: relations.PeopleComponent, data: listData({ id: ids.Person, title: 'People', icon: 'people' }) },
              { path: 'add', data: addData({ id: ids.Person }), component: relations.PersonComponent },
              { path: ':id/edit', data: editData({ id: ids.Person }), component: relations.PersonComponent },
              { path: ':id/overview', data: overviewData({ id: ids.Person }), component: relations.PersonOverviewComponent },
            ],
            path: 'people',
          },
        ],
      },
      {
        path: 'tests', data: moduleData({ title: 'Tests', icon: 'build' }),
        children: [
          {
            path: 'form', component: tests.FormComponent, data: { menuType: 'page', title: 'Form' }
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
