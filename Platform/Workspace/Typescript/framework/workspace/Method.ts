import { SessionObject } from './SessionObject';
import { MethodType } from '../meta/MethodType';

export class Method {
  constructor(public object: SessionObject, public methodType: MethodType) {}
}
