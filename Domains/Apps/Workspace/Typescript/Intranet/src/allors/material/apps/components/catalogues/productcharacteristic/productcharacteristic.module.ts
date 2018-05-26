import { NgModule } from '@angular/core';




import { ProductCharacteristicComponent } from './productcharacteristic.component';
import { FormsModule } from '@angular/forms';
export { ProductCharacteristicComponent } from './productcharacteristic.component';

@NgModule({
  declarations: [
    ProductCharacteristicComponent,
  ],
  exports: [
    ProductCharacteristicComponent,

    
    
  ],
  imports: [
    FormsModule
    
  ],
})
export class ProductCharacteristicModule {}
