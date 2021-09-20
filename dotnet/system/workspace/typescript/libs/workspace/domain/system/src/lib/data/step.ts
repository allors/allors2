import { PropertyType } from '@allors/workspace/meta/system';
import { Tree } from "./Tree";

export interface Step {
  propertyType: PropertyType;

  include?: Tree;

  next?: Step | Tree;
}
