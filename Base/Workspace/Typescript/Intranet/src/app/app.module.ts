import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { MainModule } from './main/main.module';

import { InternalOrganisationSelectModule } from '../allors/material/base/objects/internalorganisation/state/internalorganisation-select.module';
import { AppDialogModule } from './app-dialogs.module';
import { WorkspaceService } from '../allors/angular';
import { appMeta } from './app.meta';

export function appInitFactory(workspaceService: WorkspaceService) {
  return () => (appMeta(workspaceService.metaPopulation));
}

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,
    AppRoutingModule,
    AppDialogModule,

    // App Components
    AuthModule.forRoot(),
    MainModule,
    DashboardModule,

    InternalOrganisationSelectModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitFactory,
      deps: [WorkspaceService],
      multi: true
    },
  ]
})
export class AppModule { }
