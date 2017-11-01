import { NgModule } from "@angular/core";

import { SharedModule } from "../../../shared.module";

import { WorkTaskInlineComponent } from "./inline.component";
export { WorkTaskInlineComponent } from "./inline.component";

@NgModule({
  declarations: [
    WorkTaskInlineComponent,
  ],
  exports: [
    WorkTaskInlineComponent,

    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkTaskInlineModule {}
