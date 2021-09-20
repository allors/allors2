import { MethodType } from '@allors/meta/system';

import { SessionObject } from './SessionObject';

export class Method {
  constructor(public object: SessionObject, public methodType: MethodType) {}
}
