import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from "@angular/material";
import { CovalentFileModule } from "@covalent/core";

import { ImageComponent } from "./image.component";
export { ImageComponent } from "./image.component";

@NgModule({
  declarations: [
    ImageComponent,
  ],
  exports: [
    ImageComponent,
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
export class MediaImageModule {}
