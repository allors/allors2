import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../../inline.module";
import { SharedModule } from "../../../../../shared.module";

import { OrganisationContactrelationshipAddComponent } from "./organisation-contactrelationship-add.component";
export { OrganisationContactrelationshipAddComponent } from "./organisation-contactrelationship-add.component";

@NgModule({
  declarations: [
    OrganisationContactrelationshipAddComponent,
  ],
  exports: [
    OrganisationContactrelationshipAddComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class OrganisationContactrelationshipAddModule {}
