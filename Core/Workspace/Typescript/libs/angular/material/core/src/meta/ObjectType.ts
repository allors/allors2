import { Sorter } from '../sorting/Sorter';

declare module '@allors/meta/system' {
  interface ObjectType {
    sorter: Sorter;
  }
}
