import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { CatalogueComponent } from "./catalogue.component";
export { CatalogueComponent } from "./catalogue.component";

@NgModule({
  declarations: [
    CatalogueComponent,
  ],
  exports: [
    CatalogueComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class CatalogueModule {}
