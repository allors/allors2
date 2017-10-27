import { NgModule } from "@angular/core";
import { CovalentChipsModule } from "@covalent/core";

import { ChipsComponent } from "./chips";

@NgModule({
  declarations: [
    ChipsComponent,
  ],
  exports: [
    ChipsComponent,
  ],
  imports: [
    CovalentChipsModule,
  ],
})
export class ChipsModule {}
