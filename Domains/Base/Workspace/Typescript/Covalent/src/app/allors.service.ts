import { Injectable, Inject } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';

import { MetaDomain } from '../allors/meta/generated/meta.g';
import { Workspace, } from '../allors/domain/base/Workspace';
import { workspace } from '../allors/domain';

import { AllorsService, Database } from '../allors/angular';
import { ENVIRONMENT, Environment, AuthenticationService } from '../allors/angular';

@Injectable()
export class DefaultAllorsService extends AllorsService {

  workspace: Workspace;
  database: Database;
  meta: MetaDomain;

  constructor(
    http: Http,
    authService: AuthenticationService,
    @Inject(ENVIRONMENT) private environment: Environment) {

    super();

    this.database = new Database(http, environment.url, (requestOptions: RequestOptions) => authService.postProcessRequestOptions(requestOptions));
    this.workspace = workspace;
    this.meta = workspace.metaPopulation.createMetaDomain();
  }
}
