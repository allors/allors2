import { Injectable } from '@angular/core';

import { Http, Response } from '@angular/http';

import { Database } from '../allors/angular';
import { Workspace } from '../allors/domain/base/Workspace';
import { workspace } from '../allors/domain';

import { environment } from '../environments/environment';

@Injectable()
export class AllorsService {

    workspace: Workspace;
    database: Database;

    url: string;

    constructor(public http: Http) {
      this.url = 'http://localhost:50001/';

      if (environment.production) {
          this.url = 'https://app.example.com/';
      }

      this.database = new Database(http, this.url);
      this.workspace = workspace;
  }
}
