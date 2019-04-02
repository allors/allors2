import { ErrorHandler, Injectable, Injector, Type, NgZone } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class ErrorHandlerAdapter implements ErrorHandler {
  constructor(
    private injector: Injector,
  ) { }

  handleError(error: any) {
    const router = this.injector.get<Router>(Router as Type<Router>);
    if (router) {
      router.navigate(['/error']);
    } else {
      throw error;
    }
  }
}
