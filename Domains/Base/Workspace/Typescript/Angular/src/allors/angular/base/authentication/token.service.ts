import { Injectable } from '@angular/core';

@Injectable()
export class TokenService {
    private static IS_PERSISTENT_KEY = 'allors.token.IS_PERSISTENT_KEY';

    get isPersistent(): boolean {
        const isPersistent = localStorage.getItem[TokenService.IS_PERSISTENT_KEY];
        return isPersistent ? true : false;
    }

    set isPersistent(value: boolean) {
        localStorage.setItem(TokenService.IS_PERSISTENT_KEY, value.toString() );
    }
}
