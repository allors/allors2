import { SearchFactory } from '../data';
import { ISessionObject } from 'src/allors/framework';

export class FilterOptions {
  search: SearchFactory;
  display: (v: ISessionObject) => string;

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

