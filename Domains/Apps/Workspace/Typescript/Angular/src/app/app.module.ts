import 'rxjs/Rx';

import { NgModule, Type } from '@angular/core';
import { BrowserModule, Title }  from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { MdButtonModule,
         MdListModule,
         MdIconModule,
         MdCardModule,
         MdCoreModule,
         MdMenuModule,
         MdInputModule,
         MdSlideToggleModule,
         MdSnackBarModule } from '@angular/material';

import { CovalentCommonModule,
         CovalentChipsModule,
         CovalentDataTableModule,
         CovalentDialogsModule,
         CovalentExpansionPanelModule,
         CovalentFileModule,
         CovalentLayoutModule,
         CovalentLoadingModule,
         CovalentMediaModule,
         CovalentMenuModule,
         CovalentMessageModule,
         CovalentNotificationsModule,
         CovalentPagingModule,
         CovalentSearchModule,
         CovalentStepsModule} from '@covalent/core';
import { CovalentHighlightModule } from '@covalent/highlight';
import { CovalentHttpModule } from '@covalent/http';
import { CovalentMarkdownModule } from '@covalent/markdown';

import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { CommunicationEventsTableComponent } from './communicationEvents/communicationEvents.table.component';
import { EmailAddressAddComponent } from './contactMechanisms/contactMechanism/emailAddressAdd.component';
import { EmailAddressEditComponent } from './contactMechanisms/contactMechanism/emailAddressEdit.component';
import { OrganisationAddContactComponent } from './organisations/organisation/organisationAddContact.component';
import { OrganisationEditContactComponent } from './organisations/organisation/organisationEditContact.component';
import { OrganisationFormComponent } from './organisations/organisation/organisation.component';
import { OrganisationOverviewComponent } from './organisations/organisation/organisation-overview.component';
import { OrganisationsComponent } from './organisations/organisations.component';
import { OrganisationsTableComponent } from './organisations/organisations.table.component';
import { PeopleComponent } from './people/people.component';
import { PostalAddressAddComponent } from './contactMechanisms/contactMechanism/postalAddressAdd.component';
import { PostalAddressEditComponent } from './contactMechanisms/contactMechanism/postalAddressEdit.component';
import { TelecommunicationsNumberAddComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberAdd.component';
import { TelecommunicationsNumberEditComponent } from './contactMechanisms/contactMechanism/telecommunicationsNumberEdit.component';
import { WebAddressAddComponent } from './contactMechanisms/contactMechanism/webAddressAdd.component';
import { WebAddressEditComponent } from './contactMechanisms/contactMechanism/webAddressEdit.component';

import { appRoutes, appRoutingProviders } from './app.routes';

import { NgxChartsModule } from '@swimlane/ngx-charts';

import { ENVIRONMENT, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';
import { AllorsService } from './allors.service';
import { LoginComponent } from './auth/login.component';

@NgModule({
  declarations: [
    LoginComponent,
    AppComponent,
    MainComponent,
    CommunicationEventsTableComponent,
    DashboardComponent,
    EmailAddressAddComponent,
    EmailAddressEditComponent,
    OrganisationAddContactComponent,
    OrganisationEditContactComponent,
    OrganisationFormComponent,
    OrganisationOverviewComponent,
    OrganisationsComponent,
    OrganisationsTableComponent,
    PeopleComponent,
    PostalAddressAddComponent,
    PostalAddressEditComponent,
    TelecommunicationsNumberAddComponent,
    TelecommunicationsNumberEditComponent,
    WebAddressAddComponent,
    WebAddressEditComponent
  ],
  imports: [
    appRoutes,
    ReactiveFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    /** Material Modules */
    MdCoreModule,
    MdButtonModule,
    MdListModule,
    MdIconModule,
    MdCardModule,
    MdMenuModule,
    MdInputModule,
    MdSlideToggleModule,
    MdSnackBarModule,
    /** Covalent Modules */
    CovalentCommonModule,
    CovalentChipsModule,
    CovalentDataTableModule,
    CovalentDialogsModule,
    CovalentExpansionPanelModule,
    CovalentFileModule,
    CovalentLayoutModule,
    CovalentLoadingModule,
    CovalentMediaModule,
    CovalentMenuModule,
    CovalentMessageModule,
    CovalentNotificationsModule,
    CovalentPagingModule,
    CovalentSearchModule,
    CovalentStepsModule,
    CovalentHighlightModule,
    CovalentHttpModule,
    CovalentMarkdownModule,
    NgxChartsModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
    appRoutingProviders,
    Title,
  ],
  entryComponents: [ ],
  bootstrap: [ AppComponent ],
})
export class AppModule {}
