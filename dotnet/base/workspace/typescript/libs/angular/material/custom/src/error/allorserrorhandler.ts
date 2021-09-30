import { ErrorHandler, Injectable, Injector, Type, NgZone } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JL } from 'jsnlog';

@Injectable({
  providedIn: 'root',
})
export class AllorsErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any) {
    if (error instanceof SyntaxError) {
      return;
    }

    const message: string = error && error.message;
    if (message.startsWith('ExpressionChangedAfterItHasBeenCheckedError')) {
      return;
    }

    JL().fatalException('Uncaught Exception', error);

    const router = this.injector.get(ActivatedRoute);

    location.href = '/error';
  }
}
