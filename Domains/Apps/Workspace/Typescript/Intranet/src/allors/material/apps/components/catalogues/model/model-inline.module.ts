import { NgModule } from '@angular/core';


import { InlineModelComponent } from './model-inline.component';
import { FormsModule } from '@angular/forms';
export { InlineModelComponent } from './model-inline.component';

@NgModule({
  declarations: [
    InlineModelComponent,
  ],
  exports: [
    InlineModelComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class ModelInlineModule {}
