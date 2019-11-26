import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialImageComponent } from './image.component';
export { AllorsMaterialImageComponent } from './image.component';

import { ImagePreviewComponent } from './preview/image-preview.component';

@NgModule({
  declarations: [
    AllorsMaterialImageComponent,
    ImagePreviewComponent,
  ],
  entryComponents: [
    ImagePreviewComponent,
  ],
  exports: [
    AllorsMaterialImageComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialImageModule { }
