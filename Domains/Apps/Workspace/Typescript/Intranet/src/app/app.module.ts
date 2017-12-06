import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { RouterModule, Routes } from "@angular/router";

import { AuthenticationConfig, AuthenticationInterceptor, AuthenticationService, DatabaseConfig, DatabaseService, WorkspaceService } from "../allors/angular";
import { ErrorService, MenuService } from "../allors/angular";
import { DefaultErrorService } from "../allors/covalent";

import * as ar from "../allors/covalent/apps/ar";
import * as catalogues from "../allors/covalent/apps/catalogues";
import * as orders from "../allors/covalent/apps/orders";
import * as relations from "../allors/covalent/apps/relations";
import * as workefforts from "../allors/covalent/apps/workefforts";

import { environment } from "../environments/environment";
import { AppComponent } from "./app.component";
import { AuthorizationService } from "./common/auth/authorization.service";
import { routes } from "./routes";

import * as common from "./common";

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    RouterModule.forRoot(routes, { useHash: false }),
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
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl} },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: ErrorService, useClass: DefaultErrorService },
    DatabaseService,
    WorkspaceService,
    AuthenticationService,
    AuthorizationService,
    Title,
    MenuService,
  ],
})
export class AppModule { }
