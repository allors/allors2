import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatButtonModule, MatCardModule, MatIconModule, MatInputModule } from '@angular/material';

import { AllorsMaterialFileComponent } from './file.component';
import { FlexLayoutModule } from '@angular/flex-layout';
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
    FlexLayoutModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatInputModule,
  ],
})
export class AllorsMaterialFileModule {}
