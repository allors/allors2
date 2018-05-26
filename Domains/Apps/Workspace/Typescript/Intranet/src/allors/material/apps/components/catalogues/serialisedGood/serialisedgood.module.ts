import { NgModule } from '@angular/core';




import { SerialisedGoodComponent } from './serialisedgood.component';
import { FormsModule } from '@angular/forms';
export { SerialisedGoodComponent } from './serialisedgood.component';

@NgModule({
  declarations: [
    SerialisedGoodComponent,
  ],
  exports: [
    SerialisedGoodComponent,

    
    
  ],
  imports: [
    FormsModule
    
  ],
})
export class SerialisedGoodModule {}
