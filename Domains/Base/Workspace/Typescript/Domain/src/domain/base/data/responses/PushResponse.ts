import { ErrorResponse } from './ErrorResponse';
import { PullResponseDerivationError } from './PullResponseDerivationError';
import { PushResponseNewObject } from './PushResponseNewObject';

export interface PushResponse extends ErrorResponse {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: PullResponseDerivationError[];

    newObjects?: PushResponseNewObject[];
}
