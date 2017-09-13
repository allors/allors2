import { NgModule, Type } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {FlexLayoutModule} from '@angular/flex-layout';

import { ErrorService } from '../allors/angular';
import { DefaultErrorService } from '../allors/covalent';

import { AppComponent } from './app.component';
import { routedComponents, AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';

import { ENVIRONMENT, AllorsService, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';

import { DefaultAllorsService } from './allors.service';

import { MATERIAL } from '../allors/material';
import { COVALENT } from '../allors/covalent';

import { RELATIONS } from '../allors/covalent/custom/relations';

@NgModule({
  declarations: [
    MATERIAL,
    COVALENT,
    RELATIONS,
    AppComponent,
    routedComponents,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    SharedModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: ErrorService, useClass: DefaultErrorService },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    Title,
  ],
  entryComponents: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
