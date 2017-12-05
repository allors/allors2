import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Database } from "../Database";
import { DatabaseConfig } from "./database.config";

@Injectable()
export class DatabaseService {
  public database: Database;

  constructor(
    public http: HttpClient,
    public databaseConfig: DatabaseConfig,
  ) {
    this.database = new Database(http, databaseConfig.url);
  }
}
