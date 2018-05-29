import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';


import { AllorsMaterialChipsModule } from '../../../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../../base/components/select';
import { AllorsMaterialSlideToggleModule } from '../../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../../base/components/textarea';

import { PersonInlineModule } from '../../../person/person-inline.module';

import { PartyCommunicationEventWorkTaskComponent } from './party-communicationevent-worktask.component';
export { PartyCommunicationEventWorkTaskComponent } from './party-communicationevent-worktask.component';

@NgModule({
  declarations: [
    PartyCommunicationEventWorkTaskComponent,
  ],
  exports: [
    PartyCommunicationEventWorkTaskComponent,
  ],
  imports: [
    
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,  
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    PersonInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyCommunicationEventWorkTaskModule {}
