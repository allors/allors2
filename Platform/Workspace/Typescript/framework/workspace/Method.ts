import { SessionObject } from './SessionObject';
import { MethodType } from '@allors/meta/MethodType';

export class Method {
  constructor(public object: SessionObject, public methodType: MethodType) {}
}
