import { Injectable } from "@angular/core";
import { domain } from "../../../domain";
import { MetaPopulation, Workspace } from "../../../framework";
import { data } from "../../../meta";
import { DatabaseService } from "./DatabaseService";
import { Scope } from "./Scope";

@Injectable()
export class WorkspaceService {
  public metaPopulation: MetaPopulation;
  public workspace: Workspace;

  constructor(public databaseService: DatabaseService) {
    this.metaPopulation = new MetaPopulation(data);
    this.workspace = new Workspace(this.metaPopulation);
    domain.apply(this.workspace);
  }

  public createScope(): Scope {
    return new Scope(this.databaseService.database, this.workspace);
  }
}
