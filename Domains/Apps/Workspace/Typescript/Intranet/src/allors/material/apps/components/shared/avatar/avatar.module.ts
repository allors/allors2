import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AllorsMaterialAvatarComponent } from './avatar.component';
export { AllorsMaterialAvatarComponent } from './avatar.component';

@NgModule({
  declarations: [
    AllorsMaterialAvatarComponent,
  ],
  exports: [
    AllorsMaterialAvatarComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class AllorsMaterialAvatarModule {}
