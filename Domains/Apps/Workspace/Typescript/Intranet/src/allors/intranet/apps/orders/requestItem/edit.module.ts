import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { RequestItemEditComponent } from "./edit.component";
export { RequestItemEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    RequestItemEditComponent,
  ],
  exports: [
    RequestItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class RequestItemEditModule {}
