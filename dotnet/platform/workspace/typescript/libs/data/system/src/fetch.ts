import { ObjectType } from '@allors/meta/system';

import { Tree } from './Tree';
import { Step } from './Step';
import { Node } from './Node';

const includeKey = 'include';

export class Fetch {
  public step?: Step;

  public include?: Tree | Node[];

  constructor(args?: Partial<Fetch> | ObjectType, literal?: any) {
    if (args instanceof ObjectType) {
      const objectType = args as ObjectType;

      if (literal) {
        const keys = Object.keys(literal);

        if (keys.find((v) => v === includeKey)) {
          const treeLiteral = literal[includeKey];
          this.include = new Tree(objectType, treeLiteral);
        }

        if (keys.length > 0) {
          const stepName = keys.find((v) => v !== includeKey);
          if (!stepName) {
            throw new Error(`Can not find step: ${stepName}`);
          }

          const stepLiteral = literal[stepName];
          this.step = new Step(objectType, stepName, stepLiteral);
        }
      }
    } else {
      Object.assign(this, args);
    }
  }

  public toJSON(): any {
    return {
      step: this.step,
      include: this.include instanceof Tree ? this.include.nodes : this.include,
    };
  }
}
