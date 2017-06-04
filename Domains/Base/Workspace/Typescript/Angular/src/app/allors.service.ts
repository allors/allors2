import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Workspace } from '../allors/domain/base/Workspace';
import { workspace } from '../allors/domain';

import { Database } from '../allors/angular';
import { ENVIRONMENT, Environment, AuthenticationService } from '../allors/angular';

@Injectable()
export class AllorsService {

    workspace: Workspace;
    database: Database;

    constructor(
        public http: Http,
        private authService: AuthenticationService,
        @Inject(ENVIRONMENT) private environment: Environment) {

      this.database = new Database(http, environment.url, (requestOptions) => this.authService.postProcessRequestOptions(requestOptions));
      this.workspace = workspace;
    }

    onError(error) {
        alert(error);
    }
}
