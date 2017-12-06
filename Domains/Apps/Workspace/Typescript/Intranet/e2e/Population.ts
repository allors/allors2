import { Person } from "../src/allors/domain";
import { ObjectType, ObjectTyped, PullRequest, Query, Session } from "../src/allors/framework";

export class Population {
  constructor(private objectsByObjectType: Map<ObjectType, any[]>) {
  }

  public get(o: ObjectType | ObjectTyped): any[] {
    if (o instanceof ObjectType) {
      const objectType = o as ObjectType;
      return this.objectsByObjectType.get(objectType);
    } else {
      const objectTyped = o as ObjectTyped;
      return this.objectsByObjectType.get(objectTyped.ObjectType);
    }
  }
}
