import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material';

import { AllorsMaterialSearchComponent } from './search.component';
export { AllorsMaterialSearchComponent } from './search.component';

@NgModule({
  declarations: [
    AllorsMaterialSearchComponent,
  ],
  exports: [
    AllorsMaterialSearchComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
  ],
})
export class AllorsMaterialSearchModule {
}
