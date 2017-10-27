import { NgModule } from "@angular/core";
import { MatInputModule } from "@angular/material";
import { CovalentChipsModule } from "@covalent/core";

import { MediaUploadComponent } from "./mediaUpload";

@NgModule({
  declarations: [
    MediaUploadComponent,
  ],
  exports: [
    MediaUploadComponent,
  ],
  imports: [
    CovalentChipsModule,
  ],
})
export class ChipsModule {}
