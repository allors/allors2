import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatFormFieldModule, MatIconModule, MatInputModule, MatButtonModule, MatSelectModule } from '@angular/material';

import { AllorsMaterialSearchSelectComponent } from './search.select.component';
export { AllorsMaterialSearchSelectComponent } from './search.select.component';

@NgModule({
  declarations: [
    AllorsMaterialSearchSelectComponent,
  ],
  exports: [
    AllorsMaterialSearchSelectComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
  ],
})
export class AllorsMaterialSearchSelectModule {
}
