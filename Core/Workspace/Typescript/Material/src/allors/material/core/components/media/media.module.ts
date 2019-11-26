import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialMediaComponent } from './media.component';
export { AllorsMaterialMediaComponent } from './media.component';

import { AllorMediaPreviewComponent } from './preview/media-preview.component';

@NgModule({
  declarations: [
    AllorsMaterialMediaComponent,
    AllorMediaPreviewComponent,
  ],
  entryComponents: [
    AllorMediaPreviewComponent,
  ],
  exports: [
    AllorsMaterialMediaComponent,
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
export class AllorsMaterialMediaModule { }
