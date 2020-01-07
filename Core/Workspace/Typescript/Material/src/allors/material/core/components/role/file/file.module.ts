import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { AllorsFocusModule } from '../../../../../angular';
import { AllorsMaterialMediaModule } from '../../../../../material/core/components/media';

import { AllorsMaterialFileComponent } from './file.component';
export { AllorsMaterialFileComponent } from './file.component';

@NgModule({
  declarations: [
    AllorsMaterialFileComponent,
  ],
  exports: [
    AllorsMaterialFileComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatInputModule,
    AllorsFocusModule,
    AllorsMaterialMediaModule
  ],
})
export class AllorsMaterialFileModule { }
