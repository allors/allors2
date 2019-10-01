import { ObjectType } from '../meta';
import { Tree } from './Tree';
import { Step } from './Step';
import { TreeNode } from './TreeNode';

const includeKey = 'include';

export class Fetch {
  public step: Step;

  public include: Tree | TreeNode[];

  constructor(fields?: Partial<Fetch> | ObjectType, literal?: any) {

    if (fields instanceof ObjectType) {
      const objectType = fields as ObjectType;

      if (literal) {
        const keys = Object.keys(literal);

        if (keys.find(v => v === includeKey)) {
          const treeLiteral = literal[includeKey];
          this.include = new Tree(objectType, treeLiteral);
        }

        if (keys.length > 0) {
          const stepName = keys.find(v => v !== includeKey);
          const stepLiteral = literal[stepName];
          this.step = new Step(objectType, stepName, stepLiteral);
        }
      }

    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {

    return {
      step: this.step,
      include: this.include instanceof Tree ? this.include.nodes : this.include,
    };
  }
}
