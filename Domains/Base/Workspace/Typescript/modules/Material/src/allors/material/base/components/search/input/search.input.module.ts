import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatFormFieldModule, MatIconModule, MatInputModule, MatButtonModule } from '@angular/material';

import { AllorsMaterialSearchInputComponent } from './search.input.component';
export { AllorsMaterialSearchInputComponent } from './search.input.component';

@NgModule({
  declarations: [
    AllorsMaterialSearchInputComponent,
  ],
  exports: [
    AllorsMaterialSearchInputComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
  ],
})
export class AllorsMaterialSearchInputModule {
}
