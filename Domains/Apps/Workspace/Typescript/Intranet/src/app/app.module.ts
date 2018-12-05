import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { MainModule } from './main/main.module';

import { InternalOrganisationSelectModule } from '../allors/material/apps/objects/internalorganisation/state/internalorganisation-select.module';
import { FactoryModule } from './factory.module';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,
    AppRoutingModule,
    FactoryModule,

    // App Components
    AuthModule.forRoot(),
    MainModule,
    DashboardModule,

    InternalOrganisationSelectModule,
  ],
})
export class AppModule { }
