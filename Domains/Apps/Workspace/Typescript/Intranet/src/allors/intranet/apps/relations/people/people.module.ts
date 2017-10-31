import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { PeopleComponent } from "./people.component";
export { PeopleComponent } from "./people.component";

@NgModule({
  declarations: [
    PeopleComponent,
  ],
  exports: [
    PeopleComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PeopleModule {}
