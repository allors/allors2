import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule, MatGridListModule, MatBadgeModule } from '@angular/material';

import { AllorsMaterialLauncherComponent } from './launcher.component';
export { AllorsMaterialLauncherComponent } from './launcher.component';

@NgModule({
  declarations: [
    AllorsMaterialLauncherComponent,
  ],
  exports: [
    AllorsMaterialLauncherComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatBadgeModule,
    MatIconModule,
    MatGridListModule,
  ],
})
export class AllorsMaterialLauncherModule { }
