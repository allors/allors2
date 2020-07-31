import { PropertyType } from '@allors/framework';
import { SearchFactory } from '@allors/angular/search';

export class FilterOptions {
  search: SearchFactory;
  display: (v: any) => string;
  initialValue: any;
  exist: PropertyType;

  constructor(fields: Partial<FilterOptions>) {
    Object.assign(this, fields);

    if (!this.display) {
      this.display = (v: any) => {
        if (v.display) {
          return v.display;
        }

        return v.toString();
      };
    }
  }
}
