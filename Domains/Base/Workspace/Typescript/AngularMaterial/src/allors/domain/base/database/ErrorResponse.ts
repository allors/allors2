import { Response } from './Response';
import { DerivationError } from './DerivationError';

export interface ErrorResponse extends Response {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: DerivationError[];
}
