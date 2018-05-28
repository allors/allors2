import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';

import { AllorsMaterialAvatarModule } from  '../../../../../../base/components/avatar';
import { AllorsMaterialFileModule } from '../../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../../base/components/select';
import { AllorsMaterialSlideToggleModule } from '../../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../../base/components/textarea';

import { PartyContactMechanismTelecommunicationsNumberInlineComponent } from './party-contactmechanism-telecommunicationsnumber-inline.component';
export { PartyContactMechanismTelecommunicationsNumberInlineComponent } from './party-contactmechanism-telecommunicationsnumber-inline.component';

@NgModule({
  declarations: [
    PartyContactMechanismTelecommunicationsNumberInlineComponent,
  ],
  exports: [
    PartyContactMechanismTelecommunicationsNumberInlineComponent,
  ],
  imports: [
    AllorsMaterialAvatarModule,
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
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyContactMechanismTelecommunicationsNumberInlineModule {}
