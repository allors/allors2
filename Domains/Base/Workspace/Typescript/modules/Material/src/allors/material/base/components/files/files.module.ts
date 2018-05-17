import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from '@angular/material';

import { FilesComponent } from './files.component';
export { FilesComponent } from './files.component';

@NgModule({
  declarations: [
    FilesComponent,
  ],
  exports: [
    FilesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
  ],
})
export class FilesModule {}
