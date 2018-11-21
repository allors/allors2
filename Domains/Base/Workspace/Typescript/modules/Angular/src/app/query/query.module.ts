import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { QueryComponent } from './query.component';
export { QueryComponent } from './query.component';

@NgModule({
  declarations: [
    QueryComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
})
export class QueryModule {
}
