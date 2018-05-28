import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatListModule, MatIconModule } from '@angular/material';

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
    CommonModule,
    FormsModule,
    MatIconModule,
    MatListModule
  ],
})
export class AllorsMaterialAvatarModule {}
