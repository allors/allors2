import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { RelationsComponent } from "./relations.component";
export { RelationsComponent } from "./relations.component";

@NgModule({
  declarations: [
    RelationsComponent,
  ],
  exports: [
    RelationsComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class RelationsModule {}
