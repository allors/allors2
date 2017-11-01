import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { ArOverviewComponent } from "./arOverview.component";
export { ArOverviewComponent } from "./arOverview.component";

@NgModule({
  declarations: [
    ArOverviewComponent,
  ],
  exports: [
    ArOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ArOverviewModule {}
