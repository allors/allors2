import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router, Route,
         CanActivate, CanActivateChild, CanLoad } from '@angular/router';

import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {

    constructor(private authService: AuthService,
                private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
        return this.checkLoggedIn(state.url);
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
        return this.checkLoggedIn(state.url);
    }

    canLoad(route: Route): Promise<boolean> {
        return this.checkLoggedIn(route.path);
    }

    checkLoggedIn(url: string): Promise<boolean> {
        return this.authService.checkLoggedIn()
            .then( loggedIn => {
                if (!loggedIn ) {
                    this.authService.redirectUrl = url;
                    this.router.navigateByUrl('/login');
                }

                return loggedIn;
            });
    }
}
