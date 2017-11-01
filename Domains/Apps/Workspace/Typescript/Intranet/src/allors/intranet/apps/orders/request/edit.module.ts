import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { RequestEditComponent } from "./edit.component";
export { RequestEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    RequestEditComponent,
  ],
  exports: [
    RequestEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class RequestEditModule {}
