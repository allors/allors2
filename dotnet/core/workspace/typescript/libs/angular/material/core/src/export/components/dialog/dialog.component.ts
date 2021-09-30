import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';

import { PromptType } from '@allors/angular/material/services/core';
import { DialogData } from '../../services/dialog/dialog.data';

@Component({
  templateUrl: 'dialog.component.html',
})
export class AllorsMaterialDialogComponent {

  public alert: boolean | undefined;
  public confirmation: boolean | undefined;
  public prompt: boolean | undefined;
  public promptType: PromptType;

  public title: string | undefined;
  public message: string | undefined;
  public label: string | undefined;
  public placeholder: string | undefined;
  public value: string;

  constructor(public dialogRef: MatDialogRef<AllorsMaterialDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {

    this.alert = data.alert;
    this.confirmation = data.confirmation;
    this.prompt = data.prompt;

    const config = data.config;
    this.title = config.title;
    this.message = config.message;
    this.label = config.label;
    this.placeholder = config.placeholder;
    this.promptType = config.promptType || 'string';
  }
}


