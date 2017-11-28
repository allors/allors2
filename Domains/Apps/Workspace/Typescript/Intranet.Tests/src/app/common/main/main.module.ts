import { NgModule } from "@angular/core";
import { SharedModule } from "../shared.module";

import { MainComponent } from "./main.component";
export { MainComponent } from "./main.component";

@NgModule({
  declarations: [
    MainComponent,
  ],
  exports: [
    MainComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class MainModule {}
