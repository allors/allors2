import { Filter, FilterDefinition } from '../filter';

declare module '@allors/workspace/meta' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
