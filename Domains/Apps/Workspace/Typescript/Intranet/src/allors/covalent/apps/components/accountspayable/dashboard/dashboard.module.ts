import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { DashboardComponent } from "./dashboard.component";
export { DashboardComponent } from "./dashboard.component";

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  exports: [
    DashboardComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class DashboardModule {}
