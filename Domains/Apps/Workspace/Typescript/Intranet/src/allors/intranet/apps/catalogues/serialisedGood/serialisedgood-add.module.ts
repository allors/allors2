import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { SerialisedGoodAddComponent } from "./serialisedgood-add.component";
export { SerialisedGoodAddComponent } from "./serialisedgood-add.component";

@NgModule({
  declarations: [
    SerialisedGoodAddComponent,
  ],
  exports: [
    SerialisedGoodAddComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SerialisedGoodAddModule {}
