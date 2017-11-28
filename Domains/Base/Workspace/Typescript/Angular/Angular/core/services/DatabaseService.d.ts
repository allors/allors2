import { HttpClient } from "@angular/common/http";
import { Database } from "../Database";
import { Environment } from "../Environment";
export declare class DatabaseService {
    private environment;
    http: HttpClient;
    database: Database;
    constructor(environment: Environment, http: HttpClient);
}
