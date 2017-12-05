import { MetaPopulation, Workspace } from "@allors/framework";
import { constructorByName, data } from "@allors/workspace";
import { Injectable } from "@angular/core";
import { Scope } from "../Scope";
import { DatabaseService } from "./database.service";

@Injectable()
export class WorkspaceService {
  public metaPopulation: MetaPopulation;
  public workspace: Workspace;

  constructor(public databaseService: DatabaseService) {
    this.metaPopulation = new MetaPopulation(data);
    this.workspace = new Workspace(this.metaPopulation, constructorByName);
  }

  public createScope(): Scope {
    return new Scope(this.databaseService.database, this.workspace);
  }
}
