import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../inline.module";
import { SharedModule } from "../../../../shared.module";

import { ProductCharacteristicComponent } from "./productcharacteristic.component";
export { ProductCharacteristicComponent } from "./productcharacteristic.component";

@NgModule({
  declarations: [
    ProductCharacteristicComponent,
  ],
  exports: [
    ProductCharacteristicComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class ProductCharacteristicModule {}
