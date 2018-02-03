import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { PeopleExportComponent } from "./people-export.component";
export { PeopleExportComponent } from "./people-export.component";

@NgModule({
  declarations: [
    PeopleExportComponent,
  ],
  exports: [
    PeopleExportComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PeopleExportModule {}
