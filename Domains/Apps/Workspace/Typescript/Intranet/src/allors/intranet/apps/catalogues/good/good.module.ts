import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { GoodComponent } from "./good.component";
export { GoodComponent } from "./good.component";

@NgModule({
  declarations: [
    GoodComponent,
  ],
  exports: [
    GoodComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class GoodModule {}
