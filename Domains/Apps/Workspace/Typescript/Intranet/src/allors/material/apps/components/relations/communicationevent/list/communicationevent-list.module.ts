import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../../../../material';

import { CommunicationEventListComponent } from './communicationevent-list.component';
export { CommunicationEventListComponent } from './communicationevent-list.component';

@NgModule({
  declarations: [
    CommunicationEventListComponent,
  ],
  exports: [
    CommunicationEventListComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class CommunicationEventsOverviewModule { }
