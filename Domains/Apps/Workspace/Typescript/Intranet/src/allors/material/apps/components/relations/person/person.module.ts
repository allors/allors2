import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { PersonComponent } from './person.component';
export { PersonComponent } from './person.component';

@NgModule({
  declarations: [
    PersonComponent,
  ],
  exports: [
    PersonComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PersonModule {}
