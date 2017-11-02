import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppComponent } from "./app.component";

import { AuthenticationInterceptor, AuthenticationService } from "@baseAngular/authentication";
import { AllorsService, ENVIRONMENT, ErrorService } from "@baseAngular/core";
import { DefaultErrorService } from "@baseCovalent/errors";

import { environment } from "../environments/environment";

import { MenuService } from "@allors";

import { DefaultAllorsService } from "./allors.service";

import { RouterModule, Routes } from "@angular/router";
import { routes } from "./routes";

import * as ar from "../allors/intranet/apps/ar";
import * as catalogues from "../allors/intranet/apps/catalogues";
import * as orders from "../allors/intranet/apps/orders";
import * as relations from "../allors/intranet/apps/relations";
import * as workefforts from "../allors/intranet/apps/workefforts";
import * as common from "./common";

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,

    common.Modules,
    relations.Modules,
    orders.Modules,
    catalogues.Modules,
    ar.modules,
    workefforts.Modules,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: ErrorService, useClass: DefaultErrorService },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    Title,
    MenuService,
  ],
})
export class AppModule { }
