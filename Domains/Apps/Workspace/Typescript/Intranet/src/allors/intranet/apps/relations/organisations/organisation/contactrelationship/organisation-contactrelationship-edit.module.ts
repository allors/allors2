import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../../inline.module";
import { SharedModule } from "../../../../../shared.module";

import { OrganisationContactrelationshipEditComponent } from "./organisation-contactrelationship-edit.component";
export { OrganisationContactrelationshipEditComponent } from "./organisation-contactrelationship-edit.component";

@NgModule({
  declarations: [
    OrganisationContactrelationshipEditComponent,
  ],
  exports: [
    OrganisationContactrelationshipEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class OrganisationContactrelationshipEditModule {}
