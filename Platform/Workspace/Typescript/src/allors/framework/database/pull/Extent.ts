import { MetaObjectType, ObjectType } from "../../meta";
import { Predicate } from "./Predicate";

export class Extent {
  public kind: string;

  public objectType: ObjectType | MetaObjectType;

  public predicate: Predicate;

  constructor(fields?: Partial<Extent> | MetaObjectType | ObjectType) {
    if ((fields as MetaObjectType)._objectType) {
      this.objectType = fields as any;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as MetaObjectType;
    const objectType = metaObjectType._objectType ? metaObjectType._objectType : this.objectType as ObjectType;

    return {
      kind: this.kind || "Predicate",
      objectType: objectType.id,
    };
  }
}
