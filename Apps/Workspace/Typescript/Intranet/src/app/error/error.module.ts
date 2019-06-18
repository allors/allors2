import { NgModule, ErrorHandler } from '@angular/core';
import { AllorsErrorHandler } from './AllorsErrorHandler';
import { ErrorComponent } from './error.component';
import { MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule } from '@angular/material';
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
