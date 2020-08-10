import { FilterDefinition, Filter } from '@allors/angular/core';

declare module '@allors/meta/system' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
