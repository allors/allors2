import { ErrorResponse } from '../ErrorResponse';
import { DerivationError } from '../DerivationError';

export interface InvokeResponse extends ErrorResponse {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: DerivationError[];
}
