import { Component, Inject, Injectable, Input } from "@angular/core";
import { Http, RequestOptions } from "@angular/http";
import { MatDialogRef, MatSnackBar } from "@angular/material";
import { TdAlertDialogComponent, TdDialogService } from "@covalent/core";

import { DerivationError, Response, ResponseError } from "@baseDomain";

import { ErrorService } from "@baseAngular/core";

import { errorDialog } from "./errorDialog";

@Injectable()
export class DefaultErrorService extends ErrorService {
  constructor(private dialogService: TdDialogService, private snackBar: MatSnackBar) {
    super();
  }

  public message(error: Error): void {
    const message: string = (error as any)._body || error.message;
    this.snackBar.open(message, "close", { duration: 5000 });
  }

  public dialog(error: Error): MatDialogRef<any> {
    return errorDialog(this.dialogService, error);
  }
}
