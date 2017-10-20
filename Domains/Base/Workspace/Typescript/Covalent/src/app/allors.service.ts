import { Injectable, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http";

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
    http: HttpClient,
    authService: AuthenticationService,
    @Inject(ENVIRONMENT) private environment: Environment) {

    super();

    this.database = new Database(http, environment.url);
    this.workspace = workspace;
    this.meta = workspace.metaPopulation.createMetaDomain();
  }
}
