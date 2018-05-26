import { NgModule } from '@angular/core';
import { PeopleExportComponent } from './people-export.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatCardModule, MatListModule, MatFormFieldModule } from '@angular/material';
export { PeopleExportComponent } from './people-export.component';

@NgModule({
  declarations: [
    PeopleExportComponent,
  ],
  exports: [
    PeopleExportComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatListModule
  ],
})
export class PeopleExportModule {}
