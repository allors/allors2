import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { OrganisationComponent } from "./organisation.component";
export { OrganisationComponent } from "./organisation.component";

@NgModule({
  declarations: [
    OrganisationComponent,
  ],
  exports: [
    OrganisationComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class OrganisationModule {}
