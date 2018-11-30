import { AssociationType, RoleType, ObjectType, MetaObjectType } from '../../meta';
import { Tree } from './Tree';

const includeKey = 'include';

export class Step {
  public include: Tree;

  public propertyType: AssociationType | RoleType;

  public next: Step | Tree;

  constructor(fields?: Partial<Step> | ObjectType | MetaObjectType, stepName?: string, literal?) {
    if (fields instanceof ObjectType || fields && (fields as MetaObjectType).objectType) {
      const objectType = (fields as MetaObjectType).objectType ? (fields as MetaObjectType).objectType : fields as ObjectType;

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

      if (literal) {
        const keys = Object.keys(literal);

        if (keys.find(v => v === includeKey)) {
          const treeLiteral = literal[includeKey];
          this.include = new Tree(this.propertyType.objectType, treeLiteral);
        }

        const nextStepName = keys.find(v => v !== includeKey);
        if (nextStepName) {
          const nextStepLiteral = literal[nextStepName];
          this.next = new Step(this.propertyType.objectType, nextStepName, nextStepLiteral);
        }
      }

    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    return {
      include: this.include,
      propertytype: this.propertyType.id,
      next: this.next,
    };
  }
}
