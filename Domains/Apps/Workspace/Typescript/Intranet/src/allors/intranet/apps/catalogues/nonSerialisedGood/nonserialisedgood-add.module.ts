import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { NonSerialisedGoodAddComponent } from "./nonserialisedgood-add.component";
export { NonSerialisedGoodAddComponent } from "./nonserialisedgood-add.component";

@NgModule({
  declarations: [
    NonSerialisedGoodAddComponent,
  ],
  exports: [
    NonSerialisedGoodAddComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class NonSerialisedGoodAddModule {}
