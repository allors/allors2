import { MetaPopulation, Workspace } from "@allors/framework";
import { Scope } from "../Scope";
import { DatabaseService } from "./DatabaseService";
export declare class WorkspaceService {
    databaseService: DatabaseService;
    metaPopulation: MetaPopulation;
    workspace: Workspace;
    constructor(databaseService: DatabaseService);
    createScope(): Scope;
}
