import { Person, PullRequest, Query, Session } from "@allors";
import { ObjectType, ObjectTyped } from "@allors";

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
