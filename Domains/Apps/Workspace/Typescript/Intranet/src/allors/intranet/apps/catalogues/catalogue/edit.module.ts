import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { CatalogueEditComponent } from "./edit.component";
export { CatalogueEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    CatalogueEditComponent,
  ],
  exports: [
    CatalogueEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class CatalogueEditModule {}
