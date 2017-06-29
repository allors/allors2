import { Response } from '../Response';
import { DerivationError } from '../DerivationError';
import { PushResponseNewObject } from './PushResponseNewObject';

export interface PushResponse extends Response {
    newObjects?: PushResponseNewObject[];
}
