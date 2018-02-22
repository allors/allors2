import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule, Type } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule, routedComponents } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SharedModule } from "./shared/shared.module";

import { AuthenticationConfig, AuthenticationInterceptor, AuthenticationService, DatabaseConfig, DatabaseService, ErrorService, WorkspaceService } from "../allors/angular";
import { DefaultErrorService } from "../allors/covalent";

import { environment } from "../environments/environment";
import { AuthorizationService } from "./common/auth/authorization.service";

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
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl} },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: ErrorService, useClass: DefaultErrorService },
    DatabaseService,
    WorkspaceService,
    AuthenticationService,
    AuthorizationService,
    Title,
  ],
})
export class AppModule { }
