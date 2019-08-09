import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatBadgeModule } from '@angular/material/badge';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';

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
