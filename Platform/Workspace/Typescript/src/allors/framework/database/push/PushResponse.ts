import { DerivationError } from '../DerivationError';
import { Response } from '../Response';
import { PushResponseNewObject } from './PushResponseNewObject';

export interface PushResponse extends Response {
    newObjects?: PushResponseNewObject[];
}
