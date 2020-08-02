import { MethodType } from '@allors/workspace/meta';

import { SessionObject } from './SessionObject';

export class Method {
  constructor(public object: SessionObject, public methodType: MethodType) {}
}
