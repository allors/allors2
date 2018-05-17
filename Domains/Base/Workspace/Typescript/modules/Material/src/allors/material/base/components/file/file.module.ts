import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from '@angular/material';

import { FileComponent } from './file.component';
export { FileComponent } from './file.component';

@NgModule({
  declarations: [
    FileComponent,
  ],
  exports: [
    FileComponent,
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
export class FileModule {}
