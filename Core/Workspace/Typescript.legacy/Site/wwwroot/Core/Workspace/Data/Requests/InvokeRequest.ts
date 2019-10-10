namespace Allors.Data {

    export interface InvokeOptions {
        // Isolated
        i?: boolean;

        // ContinueOnError
        c?: boolean;
    }

    export interface Invocation {
        i: string;
        v: string;
        m: string;
    }

    export interface InvokeRequest {
        i: Invocation[];
        o?: InvokeOptions;
    }
}