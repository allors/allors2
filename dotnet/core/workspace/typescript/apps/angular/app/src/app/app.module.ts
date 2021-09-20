// Meta extensions
import '@allors/meta/core';
import '@allors/angular/core';

import { NgModule, APP_INITIALIZER } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { enGB } from 'date-fns/locale';

import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { data } from '@allors/meta/generated';

// Allors Angular Services Core
import {
  WorkspaceService,
  DatabaseService,
  DatabaseConfig,
  ContextService,
  AuthenticationService,
  DateService,
  AllorsFocusService,
  RefreshService,
  AllorsBarcodeService,
  NavigationService,
  MediaService,
} from '@allors/angular/services/core';

// Allors Angular Core
import {
  DateConfig,
  MediaConfig,
  AuthenticationConfig,
  AuthenticationInterceptor,
  AllorsFocusDirective,
  AllorsBarcodeDirective,
  AuthenticationServiceCore,
  DateServiceCore,
  MediaServiceCore,
  AllorsBarcodeServiceCore,
  AllorsFocusServiceCore,
  NavigationServiceCore,
  RefreshServiceCore,
} from '@allors/angular/core';

// App
import { environment } from '../environments/environment';
import { AuthorizationService } from './auth/authorization.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login.component';
import { FetchComponent } from './fetch/fetch.component';
import { FormComponent } from './form/form.component';
import { HomeComponent } from './home/home.component';
import { QueryComponent } from './query/query.component';

import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/core';
import { configure } from './configure';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    canActivate: [AuthorizationService],
    children: [
      {
        component: HomeComponent,
        path: '',
      },
      {
        component: FormComponent,
        path: 'form',
      },
      {
        component: QueryComponent,
        path: 'query',
      },
      {
        component: FetchComponent,
        path: 'fetch/:id',
      },
    ],
  },
];

export function appInitFactory(workspaceService: WorkspaceService) {
  return () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);

    // Domain extensions
    extendDomain(workspace);
    extendAngular(workspace);

    // Configuration
    configure(metaPopulation);

    workspaceService.workspace = workspace;
  };
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    // Allors Angular Core
    AllorsBarcodeDirective,
    AllorsFocusDirective,
    // App
    AppComponent,
    LoginComponent,
    FetchComponent,
    FormComponent,
    HomeComponent,
    QueryComponent,
  ],
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' }),
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitFactory,
      deps: [WorkspaceService],
      multi: true,
    },
    DatabaseService,
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    WorkspaceService,
    ContextService,
    { provide: AuthenticationService, useClass: AuthenticationServiceCore },
    {
      provide: AuthenticationConfig,
      useValue: {
        url: environment.url + environment.authenticationUrl,
      },
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true,
    },
    { provide: AllorsBarcodeService, useClass: AllorsBarcodeServiceCore },
    { provide: DateService, useClass: DateServiceCore },
    {
      provide: DateConfig,
      useValue: {
        locale: enGB,
      },
    },
    { provide: AllorsFocusService, useClass: AllorsFocusServiceCore },
    { provide: MediaService, useClass: MediaServiceCore },
    { provide: MediaConfig, useValue: { url: environment.url } },
    { provide: NavigationService, useClass: NavigationServiceCore },
    { provide: RefreshService, useClass: RefreshServiceCore },
  ],
})
export class AppModule {}
