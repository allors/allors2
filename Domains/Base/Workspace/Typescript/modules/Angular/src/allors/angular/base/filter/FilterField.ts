import { FilterFieldDefinition } from './filterFieldDefinition';
import { ValueConverter } from '@angular/compiler/src/render3/view/template';

export class FilterField {
  definition: FilterFieldDefinition;
  value: any;
  value2?: any;

  display: string;

  constructor(fields?: Partial<FilterField>) {
    Object.assign(this, fields);
  }
}

