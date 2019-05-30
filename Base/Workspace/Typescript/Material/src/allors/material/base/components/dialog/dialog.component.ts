import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ResponseError, Response, DerivationError } from '../../../../framework';
import { DialogData } from '../../services/dialog/dialog.data';
import { PromptType } from '../../services/dialog/dialog.config';

@Component({
  templateUrl: 'dialog.component.html',
})
export class AllorsMaterialDialogComponent {

  public alert: boolean;
  public confirmation: boolean;
  public prompt: boolean;
  public promptType: PromptType;

  public title: string;
  public message: string;
  public label: string;
  public placeholder: string;
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


