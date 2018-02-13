import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { OrderTermEditComponent } from "./orderterm.component";
export { OrderTermEditComponent } from "./orderterm.component";

@NgModule({
  declarations: [
    OrderTermEditComponent,
  ],
  exports: [
    OrderTermEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class OrderTermEditModule {}
