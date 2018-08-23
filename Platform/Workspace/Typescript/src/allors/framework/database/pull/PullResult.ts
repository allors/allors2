import { PullPage } from './PullPage';
import { PullPath } from './PullPath';
import { PullTreeNode } from './PullTreeNode';

export class PullResult {

  public name: string;

  public page: PullPage;

  public path: PullPath;

  public include: PullTreeNode;

  constructor(fields?: Partial<PullResult>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      name: this.name,
      page: this.page,
      path: this.path,
      include: this.include,
    };
  }
}
