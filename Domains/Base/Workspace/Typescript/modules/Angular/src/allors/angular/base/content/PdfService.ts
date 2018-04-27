import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Media } from "../../../domain";
import { ISessionObject } from "../../../framework";
import { DatabaseConfig } from "../framework/DatabaseConfig";

@Injectable()
export class PdfService {

  constructor(private databaseConfig: DatabaseConfig) {}

  public url(obj: ISessionObject): string {
    return `${this.databaseConfig.url}Pdf/Download/${obj.id}`;
  }

  public display(obj: ISessionObject): void {
    const newWindow = window.open();
    newWindow.location.href = this.url(obj);
  }
}
