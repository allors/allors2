import { Injectable, Inject , ChangeDetectorRef } from '@angular/core';

export abstract class ErrorService {
  abstract message(error: Error): void;
  abstract dialog(error: Error): any;
}
