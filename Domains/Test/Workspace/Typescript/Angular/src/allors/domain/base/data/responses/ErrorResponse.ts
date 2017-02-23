import { Response } from "./Response";
import { PullResponseDerivationError } from "./PullResponseDerivationError";

export interface ErrorResponse extends Response {
    hasErrors: boolean;
    errorMessage?: string;
    versionErrors?: string[];
    accessErrors?: string[];
    missingErrors?: string[];
    derivationErrors?: PullResponseDerivationError[];
}
