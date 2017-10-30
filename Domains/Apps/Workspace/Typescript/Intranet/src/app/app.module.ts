import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule, Type } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule, routedComponents } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SharedModule } from "./shared/shared.module";

import { AuthenticationInterceptor, AuthenticationService } from "@baseAngular/authentication";
import { AllorsService, ENVIRONMENT, ErrorService } from "@baseAngular/core";
import { DefaultErrorService } from "@baseCovalent/errors";

import { environment } from "../environments/environment";

import { MenuService } from "@allors";

import { DefaultAllorsService } from "./allors.service";

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
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
