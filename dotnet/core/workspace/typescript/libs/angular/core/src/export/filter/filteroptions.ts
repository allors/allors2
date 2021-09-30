import { PropertyType } from '@allors/meta/system';

import { SearchFactory } from '../search/SearchFactory';

export class FilterOptions {
  search: () => SearchFactory;
  display: (v: any) => string;
  initialValue: any | ((x: any) => any);
  
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

