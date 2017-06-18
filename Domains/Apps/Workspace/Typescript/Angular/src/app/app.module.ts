import { NgModule, Type } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';

import { routedComponents, AppRoutingModule } from './app-routing.module';

import { SharedModule } from './shared/shared.module';

import { ENVIRONMENT, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';
import { AllorsService } from './allors.service';

import { CataloguesTableComponent } from './catalogues/catalogues/catalogues.table.component';
import { CommunicationEventsTableComponent } from './relations/communicationEvents/communicationEvents.table.component';
import { OrganisationsTableComponent } from './relations/organisations/organisations.table.component';

import { MATERIAL } from '../allors/material';
import { COVALENT } from '../allors/covalent';

const components: any[] = [CataloguesTableComponent, CommunicationEventsTableComponent, OrganisationsTableComponent];

@NgModule({
  declarations: [
    MATERIAL,
    COVALENT,
    AppComponent,
    routedComponents,
    components,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
    Title,
  ],
  entryComponents: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
