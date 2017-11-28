import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Database } from "../Database";
import { ENVIRONMENT, Environment } from "../Environment";

@Injectable()
export class DatabaseService {
  public database: Database;

  constructor(
    @Inject(ENVIRONMENT) private environment: Environment,
    public http: HttpClient,
  ) {
    this.database = new Database(http, environment.url);
  }
}
