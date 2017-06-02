import { Routes, RouterModule } from '@angular/router';

import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmailAddressAddComponent } from './contactMechanisms/contactMechanism/emailAddressAdd.component';
import { EmailAddressEditComponent } from './contactMechanisms/contactMechanism/emailAddressEdit.component';
import { OrganisationAddContactComponent } from './organisations/organisation/organisationAddContact.component';
import { OrganisationEditContactComponent } from './organisations/organisation/organisationEditContact.component';
import { OrganisationFormComponent } from './organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './organisations/organisation/organisation-overview.component';
import { OrganisationsComponent } from './organisations/organisations.component';
import { PeopleComponent } from './people/people.component';
import { PostalAddressAddComponent } from './contactMechanisms/contactMechanism/postalAddressAdd.component';
import { PostalAddressEditComponent } from './contactMechanisms/contactMechanism/postalAddressEdit.component';
import { TelecommunicationsNumberAddComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberAdd.component';
import { TelecommunicationsNumberEditComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberEdit.component';
import { WebAddressAddComponent } from './contactMechanisms/contactMechanism/webAddressAdd.component';
import { WebAddressEditComponent } from './contactMechanisms/contactMechanism/webAddressEdit.component';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './auth/login.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent},
  {
    canActivate: [ AuthenticationService ],
    path: '', component: MainComponent,
    children: [{
        path: '', component: DashboardComponent,
      }, { path: 'people', children: [
          {path: '', component: PeopleComponent},
        ]},
        { path: 'organisations', children: [
          {path: '', component: OrganisationsComponent},
          {path: 'add', component: OrganisationFormComponent},
          {path: ':id/edit', component: OrganisationFormComponent},
          {path: ':id/overview', component: OrganisationOverviewComponent},
          {path: ':id/addContact', component: OrganisationAddContactComponent},
          {path: ':id/contact/:contactId/edit', component: OrganisationEditContactComponent},
          {path: ':id/web/:contactMechanismId/edit', component: WebAddressEditComponent},
          {path: ':id/addWeb', component: WebAddressAddComponent},
          {path: ':id/email/:contactMechanismId/edit', component: EmailAddressEditComponent},
          {path: ':id/addEmail', component: EmailAddressAddComponent},
          {path: ':id/telecom/:contactMechanismId/edit', component: TelecommunicationsNumberEditComponent},
          {path: ':id/addTelecom', component: TelecommunicationsNumberAddComponent},
          {path: ':id/postal/:contactMechanismId/edit', component: PostalAddressEditComponent},
          {path: ':id/addPostal', component: PostalAddressAddComponent},
        ]},
    ]},
];

export const appRoutingProviders: any[] = [
];

export const appRoutes: any = RouterModule.forRoot(routes, { useHash: true });
