import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorComponent } from './error.component';
import { AllorsErrorHandler } from './AllorsErrorHandler';
import { ErrorService } from './error.service';
import { MatCardModule, MatButtonModule } from '@angular/material';

@NgModule({
  declarations: [
    ErrorComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule
  ],
  providers: [
    ErrorService,
    { provide: ErrorHandler, useClass: AllorsErrorHandler },  ]
})
export class ErrorModule {
}
