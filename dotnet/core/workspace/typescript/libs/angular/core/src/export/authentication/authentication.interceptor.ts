import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthenticationService } from '@allors/angular/services/core';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  constructor(private injector: Injector) {}

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Lazy inject AuthenticationService to prevent cyclic dependency on HttpClient
    const authenticationService = this.injector.get(AuthenticationService);
    const token = authenticationService.token;

    if (token) {
      const authReq: any = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + token),
      });

      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
