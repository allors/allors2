import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './common/auth/login.component';
import { MainComponent } from './common/main/main.component';
import { DashboardComponent } from './common/dashboard/dashboard.component';

import * as relations from '../allors/material/apps/relations';
import * as catalogues from '../allors/material/apps/catalogues';

import { RELATIONS_ROUTING } from '../allors/material/apps/relations';
import { CATALOGUES_ROUTING } from '../allors/material/apps/catalogues';

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
        path: 'relations', component: relations.RelationComponent,
        children: [
          {
            path: '', component: relations.RelationDashboardComponent,
          }, {
            path: 'people',
            children: [
              { path: '', component: relations.PeopleComponent },
              { path: 'add', component: relations.PersonFormComponent },
              { path: ':id/edit', component: relations.PersonFormComponent },
              { path: ':id/overview', component: relations.PersonOverviewComponent },
              { path: ':id/web/:partyContactMechanismId/edit', component: relations.WebAddressEditComponent },
              { path: ':id/addWeb', component: relations.WebAddressAddComponent },
              { path: ':id/email/:partyContactMechanismId/edit', component: relations.EmailAddressEditComponent },
              { path: ':id/addEmail', component: relations.EmailAddressAddComponent },
              { path: ':id/telecom/:partyContactMechanismId/edit', component: relations.TelecommunicationsNumberEditComponent },
              { path: ':id/addTelecom', component: relations.TelecommunicationsNumberAddComponent },
              { path: ':id/postal/:partyContactMechanismId/edit', component: relations.PostalAddressEditComponent },
              { path: ':id/addPostal', component: relations.PostalAddressAddComponent },
            ],
          },
          {
            path: 'organisations',
            children: [
              { path: '', component: relations.OrganisationsComponent },
              { path: 'add', component: relations.OrganisationFormComponent },
              { path: ':id/edit', component: relations.OrganisationFormComponent },
              { path: ':id/overview', component: relations.OrganisationOverviewComponent },
              { path: ':id/addContact', component: relations.OrganisationAddContactComponent },
              { path: ':id/contact/:contactRelationshipId/edit', component: relations.OrganisationEditContactComponent },
              { path: ':id/web/:partyContactMechanismId/edit', component: relations.WebAddressEditComponent },
              { path: ':id/addWeb', component: relations.WebAddressAddComponent },
              { path: ':id/email/:partyContactMechanismId/edit', component: relations.EmailAddressEditComponent },
              { path: ':id/addEmail', component: relations.EmailAddressAddComponent },
              { path: ':id/telecom/:partyContactMechanismId/edit', component: relations.TelecommunicationsNumberEditComponent },
              { path: ':id/addTelecom', component: relations.TelecommunicationsNumberAddComponent },
              { path: ':id/postal/:partyContactMechanismId/edit', component: relations.PostalAddressEditComponent },
              { path: ':id/addPostal', component: relations.PostalAddressAddComponent },
            ],
          },
        ],
      },
      {
        path: 'catalogues', component: catalogues.CatalogueComponent,
        children: [
          {
            path: '', component: catalogues.CatalogueDashboardComponent,
          }, {
            path: 'catalogues',
            children: [
              { path: '', component: catalogues.CataloguesComponent },
              { path: 'add', component: catalogues.CatalogueFormComponent },
              { path: ':id/edit', component: catalogues.CatalogueFormComponent },
            ],
          }, {
            path: 'categories',
            children: [
              { path: '', component: catalogues.CategoriesComponent },
              { path: 'add', component: catalogues.CategoryFormComponent },
              { path: ':id/edit', component: catalogues.CategoryFormComponent },
            ],
          }, {
            path: 'goods',
            children: [
              { path: '', component: catalogues.GoodsComponent },
              { path: 'add', component: catalogues.GoodFormComponent },
              { path: ':id/edit', component: catalogues.GoodFormComponent },
            ],
          }, {
            path: 'productTypes',
            children: [
              { path: '', component: catalogues.ProductTypesComponent },
              { path: 'add', component: catalogues.ProductTypeFormComponent },
              { path: ':id/edit', component: catalogues.ProductTypeFormComponent },
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
  RELATIONS_ROUTING, CATALOGUES_ROUTING,
];
