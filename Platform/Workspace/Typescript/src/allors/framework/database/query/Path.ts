import { AssociationType, ObjectType, RoleType } from "../../meta";

export class Path {
  public step: AssociationType | RoleType;

  public next: Path;

  constructor(fields?: Partial<Path>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      next: this.next,
      step: this.step.id,
    };
  }

  public parse(json: any, objectType: ObjectType, stepName: string) {
    this.step = objectType.roleTypeByName[stepName];

    if (!this.step) {
      this.step = objectType.associationTypeByName[stepName];
    }

    if (!this.step) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = stepName.split("_");

      const subType = metaPopulation.objectTypeByName[subTypeName];
      if (subType) {
        this.step = subType.roleTypeByName[subStepName];

        if (!this.step) {
          this.step = subType.associationTypeByName[subStepName];
        }
      }
    }

    if (!this.step) {
      throw new Error("Unknown role: " + stepName);
    }

    const childJson = json[stepName];
    if (childJson) {
      this.next = Object.keys(childJson)
        .map((childStepName) => {
          const childTreeNode = new Path();
          childTreeNode.parse(childJson, this.step.objectType, childStepName);
          return childTreeNode;
        })[0];
    }
  }
}
