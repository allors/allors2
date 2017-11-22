import { MatDialogRef, MatSnackBar } from "@angular/material";
import { TdDialogService } from "@covalent/core";
import { ErrorService } from "@allors/base-angular";
export declare class DefaultErrorService extends ErrorService {
    private dialogService;
    private snackBar;
    constructor(dialogService: TdDialogService, snackBar: MatSnackBar);
    message(error: Error): void;
    dialog(error: Error): MatDialogRef<any>;
}
