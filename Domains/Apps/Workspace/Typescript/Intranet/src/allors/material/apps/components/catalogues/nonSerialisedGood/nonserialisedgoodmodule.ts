import { NgModule } from '@angular/core';




import { NonSerialisedGoodComponent } from './nonserialisedgood.component';
import { FormsModule } from '@angular/forms';
export { NonSerialisedGoodComponent } from './nonserialisedgood.component';

@NgModule({
  declarations: [
    NonSerialisedGoodComponent,
  ],
  exports: [
    NonSerialisedGoodComponent,

    
    
  ],
  imports: [
    FormsModule
    
  ],
})
export class NonSerialisedGoodModule {}
