import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { enGB } from 'date-fns/locale';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { CoreModule } from './core.module';
import { AppRoutingModule } from './app-routing.module';

import { AllorsModule } from '../allors/angular/core/framework';
import { AllorsAngularModule } from '../allors/angular';
import { AllorsMaterialModule } from '../allors/material';

import { WorkspaceService } from '../allors/angular';
import { appMeta } from './app.meta';
import { environment } from '../environments/environment';

import { AppComponent } from './app.component';
import { LoginComponent } from './auth';
import { DashboardComponent } from './dashboard';
import { MainComponent } from './main';

export function appInitFactory(workspaceService: WorkspaceService) {
  return () => appMeta(workspaceService.metaPopulation);
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [AppComponent, LoginComponent, DashboardComponent, MainComponent],
  imports: [
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatCardModule,
    MatFormFieldModule,
    MatSidenavModule,
    MatToolbarModule,

    AllorsModule.forRoot({ url: environment.url }),
    AllorsAngularModule.forRoot({
      dateConfig: {
        locale: enGB,
      },
      mediaConfig: { url: environment.url },
      authenticationConfig: {
        url: environment.url + environment.authenticationUrl,
      },
    }),
    AllorsMaterialModule,

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
