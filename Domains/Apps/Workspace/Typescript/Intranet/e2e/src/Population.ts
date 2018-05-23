import { Person } from "../src/allors/domain";
import { MetaObjectType, ObjectType, PullRequest, Query, Session } from "../src/allors/framework";

export class Population {
  constructor(private objectsByObjectType: Map<ObjectType, any[]>) {
  }

  public get(o: ObjectType | MetaObjectType): any[] {
    const metaObjectType = o as MetaObjectType;
    const objectType = metaObjectType._objectType ? metaObjectType._objectType : o as ObjectType;
    return this.objectsByObjectType.get(objectType);
  }
}
