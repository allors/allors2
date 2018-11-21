import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { MainModule } from './main/main.module';
import { InternalOrganisationSelectModule } from '../allors/material/apps/components/common/internalorganisation/internalorganisation-select.module';

import * as ap from '../allors/material/apps/components/accountspayable';
import * as ar from '../allors/material/apps/components/accountsreceivable';
import * as catalogues from '../allors/material/apps/components/catalogues';
import * as orders from '../allors/material/apps/components/orders';
import * as relations from '../allors/material/apps/components/relations';
import * as workefforts from '../allors/material/apps/components/workefforts';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,
    AppRoutingModule,

    // App Components
    AuthModule.forRoot(),
    MainModule,
    DashboardModule,
    InternalOrganisationSelectModule,

    relations.Modules,
    orders.Modules,
    catalogues.Modules,
    ap.modules,
    ar.modules,
    workefforts.Modules,
  ],
})
export class AppModule { }
