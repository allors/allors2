import { ResponseType } from './ResponseType';
import { DerivationError } from './DerivationError';

export interface Response {
    responseType: ResponseType;

    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: DerivationError[];
}
