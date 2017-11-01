import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { WorkTaskEditComponent } from "./edit.component";
export { WorkTaskEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    WorkTaskEditComponent,
  ],
  exports: [
    WorkTaskEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class WorkTaskEditModule {}
