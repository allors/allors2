import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../../..';

import { UserProfileLinkComponent } from './userprofile-link.component';
export { UserProfileLinkComponent } from './userprofile-link.component';

@NgModule({
  declarations: [
    UserProfileLinkComponent,
  ],
  exports: [
    UserProfileLinkComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatBadgeModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatTooltipModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class UserProfileLinkModule { }
