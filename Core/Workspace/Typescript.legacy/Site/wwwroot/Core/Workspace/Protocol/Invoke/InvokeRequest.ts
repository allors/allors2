namespace Allors.Protocol {

    export interface InvokeRequest {
        i: Invocation[];
        o?: InvokeOptions;
    }

    export interface Invocation {
        i: string;
        v: string;
        m: string;
    }

    export interface InvokeOptions {
        // Isolated
        i?: boolean;

        // ContinueOnError
        c?: boolean;
    }
}
