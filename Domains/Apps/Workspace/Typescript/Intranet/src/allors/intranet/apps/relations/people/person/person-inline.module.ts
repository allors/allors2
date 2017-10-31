import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { PersonInlineComponent } from "./person-inline.component";
export { PersonInlineComponent } from "./person-inline.component";

@NgModule({
  declarations: [
    PersonInlineComponent,
  ],
  exports: [
    PersonInlineComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PersonInlineModule {}
