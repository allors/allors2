import { PropertyType } from '@allors/workspace/meta/system';

export interface Node {
  propertyType: PropertyType;
  nodes?: Node[];
}
