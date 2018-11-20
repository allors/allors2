import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { MainModule } from './main/main.module';

import * as relations from '../allors/material/custom/relations';
import * as tests from '../allors/material/custom/tests';

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
})
export class AppModule { }
