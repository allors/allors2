import { NgModule } from '@angular/core';


import { InlineBrandComponent } from './brand-inline.component';
import { FormsModule } from '@angular/forms';
export { InlineBrandComponent } from './brand-inline.component';

@NgModule({
  declarations: [
    InlineBrandComponent,
  ],
  exports: [
    InlineBrandComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class BrandInlineModule {}
