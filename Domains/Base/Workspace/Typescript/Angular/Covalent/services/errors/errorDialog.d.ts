import { MatDialogRef } from "@angular/material";
import { TdAlertDialogComponent, TdDialogService } from "@covalent/core";
export declare function errorDialog(dialogService: TdDialogService, error: Error): MatDialogRef<TdAlertDialogComponent>;
