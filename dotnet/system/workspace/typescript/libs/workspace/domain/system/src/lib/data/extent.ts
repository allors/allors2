import { ObjectType } from '@allors/workspace/meta/system';
import { Predicate } from "./Predicate";
import { Sort } from "./Sort";

export interface Extent {
  objectType: ObjectType;

  predicate?: Predicate;

  sort?: Sort[];
}
