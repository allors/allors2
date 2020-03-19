import { NgModule, ErrorHandler } from '@angular/core';
import { AllorsErrorHandler } from './AllorsErrorHandler';
import { ErrorComponent } from './error.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    ErrorComponent,
  ],
  exports: [
    ErrorComponent,
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: AllorsErrorHandler },  ]
})
export class ErrorModule {
}
