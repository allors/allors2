import { Component, Inject, Injectable, Input } from "@angular/core";
import { MatDialogRef, MatSnackBar } from "@angular/material";
import { TdAlertDialogComponent, TdDialogService } from "@covalent/core";

import { ErrorService, LoggingService } from "../../../../angular";
import { DerivationError, Response, ResponseError } from "../../../../framework";

import { errorDialog } from "./errorDialog";

@Injectable()
export class DefaultErrorService extends ErrorService {
  constructor(private loggingService: LoggingService, private dialogService: TdDialogService, private snackBar: MatSnackBar) {
    super();
  }

  public message(error: Error): void {
    const message: string = (error as any)._body || error.message;
    this.loggingService.error(message);
    this.snackBar.open(message, "close", { duration: 5000 });
  }

  public dialog(error: Error): MatDialogRef<any> {
    return errorDialog(this.dialogService, error);
  }
}
