import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { GoodEditComponent } from "./edit.component";
export { GoodEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    GoodEditComponent,
  ],
  exports: [
    GoodEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class GoodEditModule {}
