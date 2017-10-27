import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule, Type } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { ErrorService, MenuService } from "@allors";
import { DefaultErrorService } from "@allors";

import { AppRoutingModule, routedComponents } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SharedModule } from "./shared/shared.module";

import { AllorsService, AuthenticationInterceptor, AuthenticationService, ENVIRONMENT } from "@allors";
import { environment } from "../environments/environment";

import { DefaultAllorsService } from "./allors.service";

import { MATERIAL } from "@allors";

import { CATALOGUES } from "@appsIntranet";
import { ORDERS } from "@appsIntranet";
import { RELATIONS } from "@appsIntranet";
import { WORKEFFORTS } from "@appsIntranet";


@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    MATERIAL,

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
    HttpClientModule,
    SharedModule,
  ],
  providers: [
    Title,
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: ErrorService, useClass: DefaultErrorService },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    MenuService,
  ],
})
export class AppModule { }
