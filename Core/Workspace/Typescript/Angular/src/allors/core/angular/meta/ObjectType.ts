import { Filter, FilterDefinition } from '@allors/angular/filter';

declare module '@allors/framework/meta/ObjectType' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
