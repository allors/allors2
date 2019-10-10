namespace Allors.Data {
    export interface Response {
        responseType: ResponseType,

        hasErrors: boolean;
        errorMessage?: string;
        versionErrors?: string[];
        accessErrors?: string[];
        missingErrors?: string[];
        derivationErrors?: PullResponseDerivationError[];
    }

    export interface PullResponseDerivationError {
      m: string;
      r: string[][];
    }
}
