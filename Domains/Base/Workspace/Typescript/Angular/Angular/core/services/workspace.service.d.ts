import { MetaPopulation, Workspace } from "@allors/framework";
import { Scope } from "../Scope";
import { DatabaseService } from "./database.service";
export declare class WorkspaceService {
    databaseService: DatabaseService;
    metaPopulation: MetaPopulation;
    workspace: Workspace;
    constructor(databaseService: DatabaseService);
    createScope(): Scope;
}
