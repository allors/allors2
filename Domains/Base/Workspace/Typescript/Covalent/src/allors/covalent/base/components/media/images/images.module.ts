import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from "@angular/material";
import { CovalentFileModule } from "@covalent/core";

import { ImagesComponent } from "./images.component";
export { ImagesComponent } from "./images.component";

@NgModule({
  declarations: [
    ImagesComponent,
  ],
  exports: [
    ImagesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
    CovalentFileModule,
  ],
})
export class MediaImagesModule {}
