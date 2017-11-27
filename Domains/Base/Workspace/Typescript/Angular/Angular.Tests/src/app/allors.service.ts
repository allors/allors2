import { Location } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";

import { MetaPopulation, Workspace } from "@allors/framework";
import { constructorByName, data, MetaDomain } from "@allors/workspace";

import { AuthenticationService, Database, ENVIRONMENT, Environment } from "@allors/base-angular";

@Injectable()
export class AllorsService implements CanActivate {

  public workspace: Workspace;
  public database: Database;
  public meta: MetaDomain;

  constructor(
    public http: HttpClient,
    private authService: AuthenticationService,
    private location: Location,
    private router: Router,
    @Inject(ENVIRONMENT) private environment: Environment) {

    const metaPopulation = new MetaPopulation(data);
    this.database = new Database(http, environment.url);
    this.workspace = new Workspace(metaPopulation, constructorByName);

    this.meta = metaPopulation.metaDomain;
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
