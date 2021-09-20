import { ObjectType } from '@allors/workspace/meta/system';

export interface Tree {
  objectType: ObjectType;

  nodes: Node[];
}
