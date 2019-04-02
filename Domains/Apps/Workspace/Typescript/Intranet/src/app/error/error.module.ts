import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ErrorComponent } from './error.component';
import { ErrorHandlerAdapter } from './ErrorHandlerAdapter';

@NgModule({
  declarations: [
    ErrorComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
  ],
  providers: [
    { provide: ErrorHandler, useClass: ErrorHandlerAdapter },  ]
})
export class ErrorModule {
}
