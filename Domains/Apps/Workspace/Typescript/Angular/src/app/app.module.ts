import { NgModule, Type } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ErrorService, MenuService } from '../allors/angular';
import { DefaultErrorService } from '../allors/covalent';

import { AppComponent } from './app.component';
import { routedComponents, AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';

import { ENVIRONMENT, AllorsService, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';

import { DefaultAllorsService } from './allors.service';

import { MATERIAL } from '../allors/material';
import { COVALENT } from '../allors/covalent';
import { RELATIONS } from '../allors/material/apps/relations';
import { WORKEFFORTS } from '../allors/material/apps/workefforts';
import { CATALOGUES } from '../allors/material/apps/catalogues';
import { ORDERS } from '../allors/material/apps/orders';

@NgModule({
  declarations: [
    MATERIAL,
    COVALENT,
    RELATIONS,
    WORKEFFORTS,
    CATALOGUES,
    ORDERS,
    AppComponent,
    routedComponents,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
  ],
  providers: [
    Title,
    { provide: ENVIRONMENT, useValue: environment },
    { provide: ErrorService, useClass: DefaultErrorService },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    MenuService,
  ],
  entryComponents: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
