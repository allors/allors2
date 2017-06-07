import { ErrorResponse } from '../ErrorResponse';
import { DerivationError } from '../DerivationError';
import { PushResponseNewObject } from './PushResponseNewObject';

export interface PushResponse extends ErrorResponse {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: DerivationError[];

    newObjects?: PushResponseNewObject[];
}
