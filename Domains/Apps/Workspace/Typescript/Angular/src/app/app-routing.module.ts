import { NgModule, ChangeDetectorRef } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthenticationService } from '../allors/angular';
import { LoginComponent } from './common/auth/login.component';
import { MainComponent } from './common/main/main.component';
import { DashboardComponent } from './common/dashboard/dashboard.component';

import * as relations from '../allors/material/apps/relations';
import * as workefforts from '../allors/material/apps/workefforts';
import * as catalogues from '../allors/material/apps/catalogues';
import * as orders from '../allors/material/apps/orders';

import { RELATIONS_ROUTING } from '../allors/material/apps/relations';
import { WORKEFFORTS_ROUTING } from '../allors/material/apps/workefforts';
import { CATALOGUES_ROUTING } from '../allors/material/apps/catalogues';
import { ORDERS_ROUTING } from '../allors/material/apps/orders';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    canActivate: [AuthenticationService],
    path: '', component: MainComponent,
    children: [
      {
        path: '', component: DashboardComponent, data: { type: 'module', title: 'Home', icon: 'home' },
      },
      {
        path: 'relations', component: relations.OverviewComponent, data: { type: 'module', title: 'Relations', icon: 'dashboard' },
        children: [
          { path: '', component: relations.RelationsOverviewComponent },
          { path: 'communicationevents', component: relations.CommunicationEventsOverviewComponent, data: { type: 'page', title: 'Communications', icon: 'share' } },
          { path: 'organisations', component: relations.OrganisationsOverviewComponent, data: { type: 'page', title: 'Organisations', icon: 'business' } },
          { path: 'organisation/:id', component: relations.OrganisationOverviewComponent },
          { path: 'people', component: relations.PeopleOverviewComponent, data: { type: 'page', title: 'People', icon: 'people' } },
          { path: 'person/:id', component: relations.PersonOverviewComponent },
          { path: 'party/:id/communicationevent/:roleId', component: relations.PartyCommunicationEventOverviewComponent },
        ],
      },
      {
        path: 'organisation',
        children: [
          { path: '', component: relations.OrganisationEditComponent },
          { path: ':id', component: relations.OrganisationEditComponent },
          { path: ':id/contact', component: relations.OrganisationContactrelationshipAddComponent },
          { path: ':id/contact/:roleId', component: relations.OrganisationContactrelationshipEditComponent },
        ],
      },
      {
        path: 'person',
        children: [
          { path: '', component: relations.PersonEditComponent },
          { path: ':id', component: relations.PersonEditComponent },
        ],
      },
      {
        path: 'communicationevent',
        children: [
          { path: ':id/worktask', component: relations.PartyCommunicationEventAddWorkTaskComponent },
          { path: ':id/worktask/:roleId', component: relations.PartyCommunicationEventAddWorkTaskComponent },
        ],
      },
      {
        path: 'party',
        children: [
          { path: ':id/partycontactmechanism/emailaddress', component: relations.PartyContactMechanismAddEmailAddressComponent },
          { path: ':id/partycontactmechanism/emailaddress/:roleId', component: relations.PartyContactMechanismEditEmailAddressComponent },
          { path: ':id/partycontactmechanism/postaladdress', component: relations.PartyContactMechanismAddPostalAddressComponent },
          { path: ':id/partycontactmechanism/postaladdress/:roleId', component: relations.PartyContactMechanismEditPostalAddressComponent },
          { path: ':id/partycontactmechanism/telecommunicationsnumber', component: relations.PartyContactMechanismAddTelecommunicationsNumberComponent },
          { path: ':id/partycontactmechanism/telecommunicationsnumber/:roleId', component: relations.PartyContactMechanismEditTelecommunicationsNumberComponent },
          { path: ':id/partycontactmechanism/webaddress', component: relations.PartyContactMechanismAddWebAddressComponent },
          { path: ':id/partycontactmechanism/webaddres/:roleId', component: relations.PartyContactMechanismEditWebAddressComponent },

          { path: ':id/communicationevent/emailcommunication/:roleId', component: relations.PartyCommunicationEventEditEmailCommunicationComponent },
          { path: ':id/communicationevent/emailcommunication', component: relations.PartyCommunicationEventEditEmailCommunicationComponent },
          { path: ':id/communicationevent/facetofacecommunication/:roleId', component: relations.PartyCommunicationEventEditFaceToFaceCommunicationComponent },
          { path: ':id/communicationevent/facetofacecommunication', component: relations.PartyCommunicationEventEditFaceToFaceCommunicationComponent },
          { path: ':id/communicationevent/lettercorrespondence/:roleId', component: relations.PartyCommunicationEventEditLetterCorrespondenceComponent },
          { path: ':id/communicationevent/lettercorrespondence', component: relations.PartyCommunicationEventEditLetterCorrespondenceComponent },
          { path: ':id/communicationevent/phonecommunication/:roleId', component: relations.PartyCommunicationEventEditPhoneCommunicationComponent },
          { path: ':id/communicationevent/phonecommunication', component: relations.PartyCommunicationEventEditPhoneCommunicationComponent },
        ],
      },
      {
        path: 'workefforts', component: workefforts.OverviewComponent, data: { type: 'module', title: 'Work Efforts', icon: 'work' },
        children: [
          { path: '', component: workefforts.WorkEffortsOverviewComponent },
          { path: 'worktasks', component: workefforts.WorkTasksOverviewComponent, data: { type: 'page', title: 'Tasks', icon: 'timer' } },
          { path: 'worktask/:id', component: workefforts.WorkTaskOverviewComponent },
        ],
      },
      {
        path: 'worktask',
        children: [
          { path: '', component: workefforts.WorkTaskEditComponent },
          { path: ':id', component: workefforts.WorkTaskEditComponent },
        ],
      },
      {
        path: 'export',
        children: [
          { path: 'people', component: relations.ExportPeopleComponent },
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
  DashboardComponent,
  RELATIONS_ROUTING,
  WORKEFFORTS_ROUTING,
  ORDERS_ROUTING,
  CATALOGUES_ROUTING,
];
