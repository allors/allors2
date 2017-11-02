import { NgModule } from "@angular/core";

import { SharedModule } from "../../../shared.module";

import { WorkTaskInlineComponent } from "./worktask-inline.component";
export { WorkTaskInlineComponent } from "./worktask-inline.component";

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
