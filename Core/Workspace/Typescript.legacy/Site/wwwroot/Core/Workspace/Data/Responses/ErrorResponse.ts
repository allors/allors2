namespace Allors.Data {
    export interface ErrorResponse extends Response {
        hasErrors: boolean;
        errorMessage?: string;
        versionErrors?: string[];
        accessErrors?: string[];
        missingErrors?: string[];
        derivationErrors?: Allors.Data.PullResponseDerivationError[];
    }
}