import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule, MatFormFieldModule, MatInputModule, MatButtonModule } from '@angular/material';

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
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule
  ],
})
export class AllorsMaterialSearchModule {
}
