import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { OrganisationsOverviewComponent } from "./organisations-overview.component";
export { OrganisationsOverviewComponent } from "./organisations-overview.component";

@NgModule({
  declarations: [
    OrganisationsOverviewComponent,
  ],
  exports: [
    OrganisationsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class OrganisationsOverviewModule {}
