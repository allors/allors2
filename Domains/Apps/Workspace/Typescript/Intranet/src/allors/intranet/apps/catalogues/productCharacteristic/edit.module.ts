import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { ProductCharacteristicEditComponent } from "./edit.component";
export { ProductCharacteristicEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    ProductCharacteristicEditComponent,
  ],
  exports: [
    ProductCharacteristicEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class ProductCharacteristicEditModule {}
