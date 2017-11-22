import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule, Type } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule, routedComponents } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SharedModule } from "./shared/shared.module";

import { ENVIRONMENT, ErrorService } from "@allors/base-angular";
import { AuthenticationInterceptor, AuthenticationService } from "@allors/base-angular";
import { DefaultErrorService } from "@allors/base-covalent";

import { environment } from "../environments/environment";

import { AllorsService } from "./allors.service";

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
    { provide: AllorsService, useClass: AllorsService },
    AuthenticationService,
    Title,
  ],
})
export class AppModule { }
