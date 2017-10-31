import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { OrganisationOverviewComponent } from "./organisation-overview.component";
export { OrganisationOverviewComponent } from "./organisation-overview.component";

@NgModule({
  declarations: [
    OrganisationOverviewComponent,
  ],
  exports: [
    OrganisationOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class OrganisationOverviewModule {}
