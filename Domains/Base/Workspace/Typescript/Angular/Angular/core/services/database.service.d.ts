import { HttpClient } from "@angular/common/http";
import { Database } from "../Database";
import { DatabaseConfig } from "./database.config";
export declare class DatabaseService {
    http: HttpClient;
    databaseConfig: DatabaseConfig;
    database: Database;
    constructor(http: HttpClient, databaseConfig: DatabaseConfig);
}
