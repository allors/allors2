import { NgModule, Type } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CovalentHttpModule, IHttpInterceptor } from '@covalent/http';
import { CovalentHighlightModule } from '@covalent/highlight';
import { CovalentMarkdownModule } from '@covalent/markdown';

import { AppComponent } from './app.component';

import { routedComponents, AppRoutingModule } from './app-routing.module';

import { SharedModule } from './shared/shared.module';

import { ENVIRONMENT, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';
import { AllorsService } from './allors.service';

import { CommunicationEventsTableComponent } from './relations/communicationEvents/communicationEvents.table.component';
import { OrganisationsTableComponent } from './relations/organisations/organisations.table.component';

const components = [CommunicationEventsTableComponent, OrganisationsTableComponent];

@NgModule({
  declarations: [
    AppComponent,
    routedComponents,
    components
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    CovalentHighlightModule,
    CovalentMarkdownModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
    Title
  ],
  entryComponents: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
