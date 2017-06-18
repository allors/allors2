import { Injectable, Inject } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { MdSnackBar } from '@angular/material';
import { TdDialogService } from '@covalent/core';

import { MetaDomain } from '../allors/meta/generated/meta.g';
import { Workspace } from '../allors/domain/base/Workspace';
import { workspace, ErrorResponse, DerivationError } from '../allors/domain';

import { Database } from '../allors/angular';
import { ENVIRONMENT, Environment, AuthenticationService } from '../allors/angular';

@Injectable()
export class AllorsService {

  workspace: Workspace;
  database: Database;
  meta: MetaDomain;

  constructor(
    public http: Http,
    public dialogService: TdDialogService,
    private authService: AuthenticationService,
    @Inject(ENVIRONMENT) private environment: Environment) {

    this.database = new Database(http, environment.url, (requestOptions: RequestOptions) => this.authService.postProcessRequestOptions(requestOptions));
    this.workspace = workspace;
    this.meta = workspace.metaPopulation.createMetaDomain();
  }

  onSaveError(error: ErrorResponse): void {
    let title: string;
    let message: string;

    if (error.accessErrors && error.accessErrors.length > 0) {
      title = 'Access Error';
      message = 'You do not have the required rights.';
    } else if ((error.versionErrors && error.versionErrors.length > 0) ||
      (error.missingErrors && error.missingErrors.length > 0)) {
      title = 'Concurrency Error';
      message += 'Modifications were detected since last access.';
    } else if (error.derivationErrors && error.derivationErrors.length > 0) {
      title = 'Derivation Errors';
      error.derivationErrors.map( (derivationError: DerivationError) => {
        message += `\n- ${derivationError.m}`;
      });
    } else {
      title = 'Error';
      const anyError: any = error;
      if (anyError._body) {
        message = `${anyError._body}`;
      } else {
        message = JSON.stringify(anyError);
      }
    }

    this.dialogService.openAlert({
      message: message,
      title: title,
      closeButton: 'Close',
    });
  }
}
