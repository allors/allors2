import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material';
import { DialogConfig } from './dialog.config';
import { DialogData } from './dialog.data';

import { AllorsMaterialDialogComponent } from '../../components/dialog';

@Injectable()
export class AllorsMaterialDialogService {

  constructor(private dialog: MatDialog) {
  }

  public alert(config: DialogConfig): Observable<any> {

    const data: DialogData = {
      alert: true,
      config
    };

    data.config.title = data.config.title || 'Alert';

    const dialogRef = this.dialog.open(AllorsMaterialDialogComponent, { data });
    return dialogRef.afterClosed();
  }

  public confirm(config: DialogConfig): Observable<boolean> {

    const data: DialogData = {
      confirmation: true,
      config
    };

    data.config.title = data.config.title || 'Confirm';

    const dialogRef = this.dialog.open(AllorsMaterialDialogComponent, { data });
    return dialogRef.afterClosed();
  }

  public prompt(config: DialogConfig): Observable<string> {

    const data: DialogData = {
      prompt: true,
      config
    };

    data.config.title = data.config.title || 'Prompt';

    const dialogRef = this.dialog.open(AllorsMaterialDialogComponent, { data });
    return dialogRef.afterClosed();
  }

}
