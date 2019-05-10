import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { FetchComponent } from './fetch.component';
export { FetchComponent } from './fetch.component';

@NgModule({
  declarations: [
    FetchComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
})
export class FetchModule {
}
