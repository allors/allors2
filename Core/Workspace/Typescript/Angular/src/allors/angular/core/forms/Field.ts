// tslint:disable: directive-selector
// tslint:disable: directive-class-suffix
import { Injectable } from '@angular/core';

@Injectable()
export abstract class Field  {
  protected static counter = 0;
}
