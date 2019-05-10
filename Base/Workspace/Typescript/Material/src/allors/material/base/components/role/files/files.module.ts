import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialFilesComponent } from './files.component';
export { AllorsMaterialFilesComponent } from './files.component';

@NgModule({
  declarations: [
    AllorsMaterialFilesComponent,
  ],
  exports: [
    AllorsMaterialFilesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialFilesModule {}
