import { NgModule } from "@angular/core";
import { CovalentChipsModule, CovalentLayoutModule } from "@covalent/core";

import { MatCardModule, MatIconModule, MatToolbarModule } from "@angular/material";

import { DashboardComponent } from "./dashboard.component";
export { DashboardComponent } from "./dashboard.component";

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  exports: [
    DashboardComponent,
  ],
  imports: [
    MatCardModule,
    MatIconModule,
    MatToolbarModule,
    CovalentChipsModule,
    CovalentLayoutModule,
  ],
})
export class DashboardModule {}
