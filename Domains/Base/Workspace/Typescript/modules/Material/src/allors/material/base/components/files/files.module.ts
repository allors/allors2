import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from '@angular/material';

import { FilesComponent } from './files.component';
import { FlexLayoutModule } from '@angular/flex-layout';
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
    FlexLayoutModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
  ],
})
export class FilesModule {}
