import { Invocation } from './Invocation';
import { InvokeOptions } from './InvokeOptions';

export interface InvokeRequest {
    i: Invocation[];
    o?: InvokeOptions;
}
