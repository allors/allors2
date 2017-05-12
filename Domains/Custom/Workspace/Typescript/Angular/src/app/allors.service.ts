import { Injectable } from '@angular/core';

import { Http, Response } from '@angular/http';

import { Database } from '../allors/angular/base/Database';
import { Workspace } from '../allors/domain/base/Workspace';
import { workspace } from '../allors/domain';

@Injectable()
export class AllorsService {

    workspace: Workspace;
    database: Database;

  constructor(public http: Http) {
      const url = 'http://localhost:50001/';
      this.database = new Database(http, url);
      this.workspace = workspace;
  }
}
