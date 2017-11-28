import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { PersonOverviewComponent } from "./person-overview.component";
export { PersonOverviewComponent } from "./person-overview.component";

@NgModule({
  declarations: [
    PersonOverviewComponent,
  ],
  exports: [
    PersonOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PersonOverviewModule {}
