import { NgModule, APP_INITIALIZER } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { enGB } from 'date-fns/locale';

import { WorkspaceService } from '@allors/angular/core';
import { AllorsAngularModule } from '@allors/angular/module';

import { AppRoutingModule } from './app-routing.module';
import { environment } from '../environments/environment';
import { appMeta } from './app.meta';

import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login.component';
import { FetchComponent } from './fetch/fetch.component';
import { FormComponent } from './form/form.component';
import { HomeComponent } from './home/home.component';
import { QueryComponent } from './query/query.component';
import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { data } from '@allors/meta/generated';

import '@allors/meta/core';
import '@allors/angular/core';
import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/core';
export function appInitFactory(workspaceService: WorkspaceService) {
  return () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);
    extendDomain(workspace);
    extendAngular(workspace);
    appMeta(metaPopulation);

    workspaceService.workspace = workspace;
  };
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent, LoginComponent, FetchComponent, FormComponent, HomeComponent, QueryComponent],
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    // Allors
    AllorsAngularModule.forRoot({
      databaseConfig: { url: environment.url },
      authenticationConfig: {
        url: environment.url + environment.authenticationUrl,
      },
      dateConfig: {
        locale: enGB,
      },
      mediaConfig: { url: environment.url },
    }),
    AppRoutingModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitFactory,
      deps: [WorkspaceService],
      multi: true,
    },
  ],
})
export class AppModule {}
