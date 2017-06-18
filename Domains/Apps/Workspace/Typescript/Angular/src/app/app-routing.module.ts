import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './common/auth/login.component';
import { MainComponent } from './common/main/main.component';
import { DashboardComponent } from './common/dashboard/dashboard.component';

import { CatalogueComponent } from './catalogues/catalogue.component';
import { CatalogueDashboardComponent } from './catalogues/dashboard/catalogue-dashboard.component';
import { CatalogueFormComponent } from './catalogues/catalogues/catalogue/catalogue.component';
import { CataloguesComponent } from './catalogues/catalogues/catalogues.component';
import { CategoryFormComponent } from './catalogues/categories/category/category.component';
import { CategoriesComponent } from './catalogues/categories/categories.component';
import { GoodFormComponent } from './catalogues/goods/good/good.component';
import { GoodsComponent } from './catalogues/goods/goods.component';
import { EmailAddressAddComponent } from './relations/contactMechanisms/contactMechanism/emailAddressAdd.component';
import { EmailAddressEditComponent } from './relations/contactMechanisms/contactMechanism/emailAddressEdit.component';
import { OrganisationAddContactComponent } from './relations/organisations/organisation/organisationAddContact.component';
import { OrganisationEditContactComponent } from './relations/organisations/organisation/organisationEditContact.component';
import { OrganisationFormComponent } from './relations/organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './relations/organisations/organisation/organisation-overview.component';
import { OrganisationsComponent } from './relations/organisations/organisations.component';
import { PeopleComponent } from './relations/people/people.component';
import { PersonFormComponent } from './relations/people/person/person.component';
import { PersonOverviewComponent } from './relations/people/person/person-overview.component';
import { PostalAddressAddComponent } from './relations/contactMechanisms/contactMechanism/postalAddressAdd.component';
import { PostalAddressEditComponent } from './relations/contactMechanisms/contactMechanism/postalAddressEdit.component';
import { ProductTypeFormComponent } from './catalogues/producttypes/producttype/producttype.component';
import { ProductTypesComponent } from './catalogues/producttypes/producttypes.component';
import { RelationComponent } from './relations/relation.component';
import { RelationDashboardComponent } from './relations/dashboard/relation-dashboard.component';
import { TelecommunicationsNumberAddComponent } from './relations/contactMechanisms/contactMechanism/telecommunicationsNumberAdd.component';
import { TelecommunicationsNumberEditComponent } from './relations/contactMechanisms/contactMechanism/telecommunicationsNumberEdit.component';
import { WebAddressAddComponent } from './relations/contactMechanisms/contactMechanism/webAddressAdd.component';
import { WebAddressEditComponent } from './relations/contactMechanisms/contactMechanism/webAddressEdit.component';

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
        path: 'relations', component: RelationComponent,
        children: [
          {
            path: '', component: RelationDashboardComponent,
          }, {
            path: 'people',
            children: [
              { path: '', component: PeopleComponent },
              { path: 'add', component: PersonFormComponent },
              { path: ':id/edit', component: PersonFormComponent },
              { path: ':id/overview', component: PersonOverviewComponent },
              { path: ':id/web/:partyContactMechanismId/edit', component: WebAddressEditComponent },
              { path: ':id/addWeb', component: WebAddressAddComponent },
              { path: ':id/email/:partyContactMechanismId/edit', component: EmailAddressEditComponent },
              { path: ':id/addEmail', component: EmailAddressAddComponent },
              { path: ':id/telecom/:partyContactMechanismId/edit', component: TelecommunicationsNumberEditComponent },
              { path: ':id/addTelecom', component: TelecommunicationsNumberAddComponent },
              { path: ':id/postal/:partyContactMechanismId/edit', component: PostalAddressEditComponent },
              { path: ':id/addPostal', component: PostalAddressAddComponent },
            ],
          },
          {
            path: 'organisations',
            children: [
              { path: '', component: OrganisationsComponent },
              { path: 'add', component: OrganisationFormComponent },
              { path: ':id/edit', component: OrganisationFormComponent },
              { path: ':id/overview', component: OrganisationOverviewComponent },
              { path: ':id/addContact', component: OrganisationAddContactComponent },
              { path: ':id/contact/:contactRelationshipId/edit', component: OrganisationEditContactComponent },
              { path: ':id/web/:partyContactMechanismId/edit', component: WebAddressEditComponent },
              { path: ':id/addWeb', component: WebAddressAddComponent },
              { path: ':id/email/:partyContactMechanismId/edit', component: EmailAddressEditComponent },
              { path: ':id/addEmail', component: EmailAddressAddComponent },
              { path: ':id/telecom/:partyContactMechanismId/edit', component: TelecommunicationsNumberEditComponent },
              { path: ':id/addTelecom', component: TelecommunicationsNumberAddComponent },
              { path: ':id/postal/:partyContactMechanismId/edit', component: PostalAddressEditComponent },
              { path: ':id/addPostal', component: PostalAddressAddComponent },
            ],
          },
        ],
      },
      {
        path: 'catalogues', component: CatalogueComponent,
        children: [
          {
            path: '', component: CatalogueDashboardComponent,
          }, {
            path: 'catalogues',
            children: [
              { path: '', component: CataloguesComponent },
              { path: 'add', component: CatalogueFormComponent },
              { path: ':id/edit', component: CatalogueFormComponent },
            ],
          }, {
            path: 'categories',
            children: [
              { path: '', component: CategoriesComponent },
              { path: 'add', component: CategoryFormComponent },
              { path: ':id/edit', component: CategoryFormComponent },
            ],
          }, {
            path: 'goods',
            children: [
              { path: '', component: GoodsComponent },
              { path: 'add', component: GoodFormComponent },
              { path: ':id/edit', component: GoodFormComponent },
            ],
          }, {
            path: 'productTypes',
            children: [
              { path: '', component: ProductTypesComponent },
              { path: 'add', component: ProductTypeFormComponent },
              { path: ':id/edit', component: ProductTypeFormComponent },
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

  CatalogueComponent, CatalogueDashboardComponent, CatalogueFormComponent, CataloguesComponent, CategoriesComponent, CategoryFormComponent,
  GoodFormComponent, GoodsComponent,
  EmailAddressAddComponent, EmailAddressEditComponent,
  OrganisationAddContactComponent, OrganisationEditContactComponent, OrganisationFormComponent, OrganisationOverviewComponent, OrganisationsComponent,
  PeopleComponent, PersonFormComponent, PersonOverviewComponent,
  PostalAddressAddComponent, PostalAddressEditComponent, ProductTypeFormComponent, ProductTypesComponent,
  RelationComponent, RelationDashboardComponent,
  TelecommunicationsNumberAddComponent, TelecommunicationsNumberEditComponent,
  WebAddressAddComponent, WebAddressEditComponent,
];
