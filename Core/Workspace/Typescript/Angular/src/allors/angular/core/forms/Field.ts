// tslint:disable: directive-selector
// tslint:disable: directive-class-suffix
import { Directive } from '@angular/core';

// See https://github.com/angular/angular/issues/30080
@Directive({selector: 'ivy-workaround-field'})
export abstract class Field  {
  protected static counter = 0;
}
