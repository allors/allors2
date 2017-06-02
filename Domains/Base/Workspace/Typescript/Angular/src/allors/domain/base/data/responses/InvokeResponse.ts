import { ErrorResponse } from './ErrorResponse';
import { PullResponseDerivationError } from './PullResponseDerivationError';

export interface InvokeResponse extends ErrorResponse {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: PullResponseDerivationError[];
}
