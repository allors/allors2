import { ObjectType, PropertyType, MetaObjectType } from '../../meta';

export class Path {
  public propertyType: PropertyType;

  public next: Path;

  constructor(fields?: Partial<Path> | ObjectType | MetaObjectType, literal?) {

    if (fields instanceof ObjectType || fields && (fields as MetaObjectType)._objectType) {
      const objectType = (fields as MetaObjectType)._objectType ? (fields as MetaObjectType)._objectType : fields as ObjectType;

      if (literal) {
        const keys = Object.keys(literal);
        if (keys.length > 0) {
          const roleName = keys[0];
          this.parse(literal, objectType, roleName);
        }
      }

    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    return {
      propertytype: this.propertyType ? this.propertyType.id : undefined,
      next: this.next,
    };
  }

  public parse(json: any, objectType: ObjectType, stepName: string) {
    this.propertyType = objectType.roleTypeByName[stepName];

    if (!this.propertyType) {
      this.propertyType = objectType.associationTypeByName[stepName];
    }

    if (!this.propertyType) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = stepName.split('_');

      const subType = metaPopulation.objectTypeByName[subTypeName];
      if (subType) {
        this.propertyType = subType.roleTypeByName[subStepName];

        if (!this.propertyType) {
          this.propertyType = subType.associationTypeByName[subStepName];
        }
      }
    }

    if (!this.propertyType) {
      throw new Error('Unknown role or association: ' + stepName);
    }

    const childJson = json[stepName];
    if (childJson) {
      this.next = Object.keys(childJson)
        .map((childStepName) => {
          const childTreeNode = new Path();
          childTreeNode.parse(childJson, this.propertyType.objectType, childStepName);
          return childTreeNode;
        })[0];
    }
  }
}
