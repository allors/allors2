import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { MainModule } from './main/main.module';

import { WorkspaceService } from '../allors/angular';

import * as relations from '../allors/material/custom/relations';
import * as tests from '../allors/material/custom/tests';

import { appInit } from './app.init';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,

    // App Components
    AuthModule.forRoot(),
    MainModule,
    DashboardModule,

    relations.Modules,
    tests.Modules,

    AppRoutingModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: (workspaceService: WorkspaceService) => () => {
        appInit(workspaceService);
      },
      deps: [WorkspaceService],
      multi: true
    },
  ]
})
export class AppModule { }
