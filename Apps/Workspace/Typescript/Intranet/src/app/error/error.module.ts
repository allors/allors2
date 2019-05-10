import { NgModule, ErrorHandler } from '@angular/core';
import { AllorsErrorHandler } from './AllorsErrorHandler';

@NgModule({
  providers: [
    { provide: ErrorHandler, useClass: AllorsErrorHandler },  ]
})
export class ErrorModule {
}
