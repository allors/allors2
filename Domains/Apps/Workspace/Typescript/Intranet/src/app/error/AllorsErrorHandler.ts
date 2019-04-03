import { ErrorHandler, Injectable, Injector, Type, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { JL } from 'jsnlog';
import { ErrorService } from './error.service';

@Injectable()
export class AllorsErrorHandler implements ErrorHandler {
  constructor(
    private injector: Injector,
  ) { }

  handleError(error: any) {

    JL().fatalException('Uncaught Exception', error);

    const router = this.injector.get<Router>(Router as Type<Router>);
    const errorService = this.injector.get<ErrorService>(ErrorService as Type<ErrorService>);

    errorService.error = error;
    router.navigate(['/error']);
  }
}
