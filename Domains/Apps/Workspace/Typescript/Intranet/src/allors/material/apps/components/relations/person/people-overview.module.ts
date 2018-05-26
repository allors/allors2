import { NgModule } from '@angular/core';
import { PeopleOverviewComponent } from './people-overview.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatToolbarModule, MatTooltipModule, MatButtonModule, MatFormFieldModule, MatIconModule, MatDividerModule, MatMenuModule, MatCardModule, MatListModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
export { PeopleOverviewComponent } from './people-overview.component';

@NgModule({
  declarations: [
    PeopleOverviewComponent,
  ],
  exports: [
    PeopleOverviewComponent,
    
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatToolbarModule,
    MatTooltipModule,
    MatButtonModule,
    MatFormFieldModule,  
    MatIconModule,
    MatListModule,
    MatDividerModule,
    MatMenuModule,
    RouterModule,
  ],
})
export class PeopleOverviewModule {}
