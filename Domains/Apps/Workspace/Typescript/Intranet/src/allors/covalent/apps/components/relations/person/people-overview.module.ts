import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { PeopleOverviewComponent } from "./people-overview.component";
export { PeopleOverviewComponent } from "./people-overview.component";

@NgModule({
  declarations: [
    PeopleOverviewComponent,
  ],
  exports: [
    PeopleOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PeopleOverviewModule {}
