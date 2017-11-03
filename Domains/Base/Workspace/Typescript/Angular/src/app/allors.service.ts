import { Location } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";

import { constructorByName } from "@generatedDomain/domain.g";
import { data, MetaDomain } from "@generatedMeta/meta.g";

import { Database, MetaPopulation, Workspace } from "@allors";
import {
  AllorsService,
  AuthenticationService,
  ENVIRONMENT,
  Environment,
} from "@allors";

@Injectable()
export class DefaultAllorsService extends AllorsService implements CanActivate {

  public workspace: Workspace;
  public database: Database;
  public m: MetaDomain;

  constructor(
    public http: HttpClient,
    private authService: AuthenticationService,
    private location: Location,
    private router: Router,
    @Inject(ENVIRONMENT) private environment: Environment,
  ) {
    super();

    const metaPopulation = new MetaPopulation(data);
    this.database = new Database(http, environment.url);
    this.workspace = new Workspace(metaPopulation, constructorByName);

    this.m = metaPopulation.metaDomain;
  }

  public canActivate() {
    if (this.authService.token) {
      return true;
    } else {
      this.router.navigate(["login"]);
      return false;
    }
  }

  public back() {
    this.location.back();
  }

  public onError(error) {
    alert(error);
  }
}
