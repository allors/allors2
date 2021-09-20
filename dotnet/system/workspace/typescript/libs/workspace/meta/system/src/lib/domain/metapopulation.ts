import { Class } from './Class';
import { Interface } from './Interface';
import { MetaObject } from './MetaObject';
import { MethodType } from './MethodType';
import { Unit } from './Unit';
import { RelationType } from './RelationType';
import { Composite } from './Composite';

export interface MetaPopulation {
  metaObjectByTag: Map<number, MetaObject>;
  units: Set<Unit>;
  interfaces: Set<Interface>;
  classes: Set<Class>;
  composites: Set<Composite>;
  relationTypes: Set<RelationType>;
  methodTypes: Set<MethodType>;
}
