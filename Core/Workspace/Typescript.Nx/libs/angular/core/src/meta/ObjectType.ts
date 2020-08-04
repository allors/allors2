import { Filter, FilterDefinition } from '../filter';

declare module '@allors/meta/system' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
