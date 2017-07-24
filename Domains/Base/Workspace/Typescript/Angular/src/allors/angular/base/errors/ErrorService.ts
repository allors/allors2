import { Injectable, Inject , ChangeDetectorRef } from '@angular/core';
import { MdDialogRef } from '@angular/material';

export abstract class ErrorService {
  abstract message(error: Error): void;
  abstract dialog(error: Error): MdDialogRef<any>;
}
