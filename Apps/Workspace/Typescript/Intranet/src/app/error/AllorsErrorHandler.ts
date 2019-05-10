import { ErrorHandler, Injectable, Injector, Type, NgZone } from '@angular/core';
import { JL } from 'jsnlog';

@Injectable()
export class AllorsErrorHandler implements ErrorHandler {
  constructor(
    private injector: Injector,
  ) { }

  handleError(error: any) {

    const message: string = error && error.message;
    if (message.startsWith('ExpressionChangedAfterItHasBeenCheckedError')) {
      return;
    }

    JL().fatalException('Uncaught Exception', error);

    alert('🛑 An error occurred and was logged, the application will restart.');
    location.href = '/';
  }
}
