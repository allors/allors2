import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { ErrorHandler, NgModule } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { RouterModule, Routes } from "@angular/router";

import { AuthenticationConfig, AuthenticationInterceptor, AuthenticationService, DatabaseConfig, DatabaseService, WorkspaceService, LoggingService } from "../allors/angular";
import { ErrorService, MediaService, MenuService } from "../allors/angular";
import { DefaultErrorService } from "../allors/covalent";

import { DefaultStateService } from "../allors/covalent/apps/services/DefaultStateService";
import { StateService } from "../allors/covalent/apps/services/StateService";

import * as ap from "../allors/covalent/apps/components/accountspayable";
import * as ar from "../allors/covalent/apps/components/accountsreceivable";
import * as catalogues from "../allors/covalent/apps/components/catalogues";
import * as orders from "../allors/covalent/apps/components/orders";
import * as relations from "../allors/covalent/apps/components/relations";
import * as workefforts from "../allors/covalent/apps/components/workefforts";

import { environment } from "../environments/environment";
import { AppComponent } from "./app.component";
import { AuthorizationService } from "./common/auth/authorization.service";
import { routes } from "./routes";

import { ConfigService } from "./app.config.service";
import { DefaultErrorHandler } from "./app.error.handler";
import { DefaultLoggingService } from "./app.logging.service";

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
    ap.modules,
    ar.modules,
    workefforts.Modules,
  ],
  providers: [
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl} },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: StateService, useClass: DefaultStateService },
    { provide: LoggingService, useClass: DefaultLoggingService },
    { provide: ErrorHandler, useClass: DefaultErrorHandler },
    { provide: ErrorService, useClass: DefaultErrorService },
    ConfigService,
    DatabaseService,
    WorkspaceService,
    AuthenticationService,
    AuthorizationService,
    Title,
    MenuService,
    MediaService,
  ],
})
export class AppModule { }
