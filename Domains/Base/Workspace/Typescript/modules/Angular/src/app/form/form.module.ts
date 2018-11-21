import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormComponent } from './form.component';
export { FormComponent } from './form.component';

@NgModule({
  declarations: [
    FormComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
  ],
})
export class FormModule {
}
