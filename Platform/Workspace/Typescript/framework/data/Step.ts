import { ObjectType, PropertyType } from '@allors/framework';
import { Tree } from './Tree';

const includeKey = 'include';

type StepArgs = Pick<Step, 'propertyType' | 'include' | 'next'>;

export class Step {
  public propertyType: PropertyType;

  public include?: Tree;

  public next?: Step | Tree;

  constructor(args: StepArgs);
  constructor(objectType: ObjectType, stepName: string, literal?: { [key: string]: any });
  constructor(args: StepArgs | ObjectType, stepName?: string, literal?: { [key: string]: any }) {
    if (args instanceof ObjectType) {
      const objectType = args as ObjectType;

      let propertyType: PropertyType | undefined;
      if (stepName) {
        propertyType = objectType.roleTypeByName.get(stepName);
        if (!propertyType) {
          propertyType = objectType.associationTypeByName.get(stepName);
        }

        if (!propertyType) {
          const metaPopulation = objectType.metaPopulation;
          const [subTypeName, subStepName] = stepName.split('_');

          const subType = metaPopulation.objectTypeByName.get(subTypeName);
          if (subType) {
            propertyType = subType.roleTypeByName.get(subStepName);

            if (!propertyType) {
              propertyType = subType.associationTypeByName.get(subStepName);
            }
          }
        }
      }

      if (!propertyType) {
        throw new Error('Unknown role or association: ' + stepName);
      }

      this.propertyType = propertyType;

      if (literal) {
        const keys = Object.keys(literal);

        if (keys.find((v) => v === includeKey)) {
          const treeLiteral = literal[includeKey];
          if (treeLiteral instanceof Tree) {
            this.include = treeLiteral;
          } else {
            this.include = new Tree(this.propertyType.objectType, treeLiteral);
          }
        }

        const nextStepName = keys.find((v) => v !== includeKey);
        if (nextStepName) {
          const nextStepLiteral = literal[nextStepName];
          this.next = new Step(this.propertyType.objectType, nextStepName, nextStepLiteral);
        }
      }
    } else {
      this.propertyType = args.propertyType;
      this.include = args.include;
      this.next = args.next;
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
