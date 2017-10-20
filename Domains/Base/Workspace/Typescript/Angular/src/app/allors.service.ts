import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";

import { workspace } from "../allors/domain";
import { Workspace } from "../allors/domain/base/Workspace";
import { MetaDomain } from "../allors/meta/generated/meta.g";

import { Database } from "../allors/angular";
import { AuthenticationService, ENVIRONMENT, Environment } from "../allors/angular";

@Injectable()
export class AllorsService {

    public workspace: Workspace;
    public database: Database;
    public meta: MetaDomain;

    constructor(
        public http: HttpClient,
        private authService: AuthenticationService,
        @Inject(ENVIRONMENT) private environment: Environment) {

      this.database = new Database(http, environment.url);
      this.workspace = workspace;
      this.meta = workspace.metaPopulation.createMetaDomain();
    }

    public onError(error) {
        alert(error);
    }
}
