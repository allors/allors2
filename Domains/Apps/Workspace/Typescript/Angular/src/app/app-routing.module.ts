import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { RelationComponent } from './relations/relation.component';
import { RelationDashboardComponent } from './relations/dashboard/relation-dashboard.component';
import { EmailAddressAddComponent } from './relations/contactMechanisms/contactMechanism/emailAddressAdd.component';
import { EmailAddressEditComponent } from './relations/contactMechanisms/contactMechanism/emailAddressEdit.component';
import { OrganisationAddContactComponent } from './relations/organisations/organisation/organisationAddContact.component';
import { OrganisationEditContactComponent } from './relations/organisations/organisation/organisationEditContact.component';
import { OrganisationFormComponent } from './relations/organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './relations/organisations/organisation/organisation-overview.component';
import { OrganisationsComponent } from './relations/organisations/organisations.component';
import { PeopleComponent } from './relations/people/people.component';
import { PostalAddressAddComponent } from './relations/contactMechanisms/contactMechanism/postalAddressAdd.component';
import { PostalAddressEditComponent } from './relations/contactMechanisms/contactMechanism/postalAddressEdit.component';
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
            ]
          },
          {
            path: 'organisations', children: [
              { path: '', component: OrganisationsComponent },
              { path: 'add', component: OrganisationFormComponent },
              { path: ':id/edit', component: OrganisationFormComponent },
              { path: ':id/overview', component: OrganisationOverviewComponent },
              { path: ':id/addContact', component: OrganisationAddContactComponent },
              { path: ':id/contact/:contactId/edit', component: OrganisationEditContactComponent },
              { path: ':id/web/:contactMechanismId/edit', component: WebAddressEditComponent },
              { path: ':id/addWeb', component: WebAddressAddComponent },
              { path: ':id/email/:contactMechanismId/edit', component: EmailAddressEditComponent },
              { path: ':id/addEmail', component: EmailAddressAddComponent },
              { path: ':id/telecom/:contactMechanismId/edit', component: TelecommunicationsNumberEditComponent },
              { path: ':id/addTelecom', component: TelecommunicationsNumberAddComponent },
              { path: ':id/postal/:contactMechanismId/edit', component: PostalAddressEditComponent },
              { path: ':id/addPostal', component: PostalAddressAddComponent },
            ]
          },
        ]
      }]
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
  ],
  exports: [
    RouterModule,
  ]
})
export class AppRoutingModule { }
export const routedComponents: any[] = [
  MainComponent, LoginComponent, DashboardComponent,

  RelationComponent, RelationDashboardComponent,
  EmailAddressAddComponent, EmailAddressEditComponent,
  OrganisationAddContactComponent, OrganisationEditContactComponent, OrganisationFormComponent, OrganisationOverviewComponent, OrganisationsComponent,
  PeopleComponent,
  PostalAddressAddComponent, PostalAddressEditComponent,
  TelecommunicationsNumberAddComponent, TelecommunicationsNumberEditComponent,
  WebAddressAddComponent, WebAddressEditComponent
];
