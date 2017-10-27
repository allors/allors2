import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";

import { MatIconModule, MatInputModule } from "@angular/material";
import { CovalentFileModule } from "@covalent/core";

import { MediaUploadComponent } from "./mediaupload.component";
export { MediaUploadComponent } from "./mediaupload.component";

@NgModule({
  declarations: [
    MediaUploadComponent,
  ],
  exports: [
    MediaUploadComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    MatInputModule,
    CovalentFileModule,
  ],
})
export class MediaUploadModule {}
