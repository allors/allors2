import { Response } from '../Response';
import { SecurityResponseAccessControl } from './SecurityResponseAccessControl';

export interface SecurityResponse extends Response {
    accessControls?: SecurityResponseAccessControl[];
    permissions?: string[][];
}
