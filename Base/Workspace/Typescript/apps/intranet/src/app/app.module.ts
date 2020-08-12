// Meta extensions
import '@allors/meta/core';
import '@allors/angular/module';
import '@allors/angular/material/module';

import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';
import { enGB } from 'date-fns/locale';

import { MetaPopulation } from '@allors/meta/system';
import { data } from '@allors/meta/generated';
import { Workspace } from '@allors/domain/system';
import { WorkspaceService } from '@allors/angular/core';
import { AllorsAngularModule } from '@allors/angular/module';
import { AllorsMaterialModule } from '@allors/angular/material/module';
import { AllorsDateAdapter } from '@allors/angular/material/core';

import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/module';
import { configure as configureMaterial } from '@allors/angular/material/module';

import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

export function appInitFactory(workspaceService: WorkspaceService) {
  return () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);

    // Domain extensions
    extendDomain(workspace);
    extendAngular(workspace);

    // Configuration
    configureMaterial(metaPopulation);

    workspaceService.workspace = workspace;
  };
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    AllorsAngularModule,

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
    AllorsMaterialModule,
    AppRoutingModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitFactory,
      deps: [WorkspaceService],
      multi: true,
    },
    {
      provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS,
      useValue: { autoActiveFirstOption: true },
    },
    { provide: DateAdapter, useClass: AllorsDateAdapter },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: {
          dateInput: 'dd/MM/yyyy',
        },
        display: {
          dateInput: 'dd/MM/yyyy',
          monthYearLabel: 'LLL y',
          dateA11yLabel: 'MMMM d, y',
          monthYearA11yLabel: 'MMMM y',
        },
      },
    },
  ],
})
export class AppModule {}
