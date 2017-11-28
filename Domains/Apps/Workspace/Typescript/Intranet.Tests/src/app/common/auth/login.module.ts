import { NgModule } from "@angular/core";
import { SharedModule } from "../shared.module";

import { LoginComponent } from "./login.component";
export { LoginComponent } from "./login.component";

@NgModule({
  declarations: [
    LoginComponent,
  ],
  exports: [
    LoginComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class LoginModule {}
