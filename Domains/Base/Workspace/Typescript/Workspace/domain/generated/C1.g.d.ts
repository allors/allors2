import { SessionObject, Method } from "@allors/framework";
import { I1 } from './I1.g';
export declare class C1 extends SessionObject implements I1 {
    readonly CanReadC1AllorsBinary: boolean;
    readonly CanWriteC1AllorsBinary: boolean;
    C1AllorsBinary: any;
    readonly CanReadC1AllorsString: boolean;
    readonly CanWriteC1AllorsString: boolean;
    C1AllorsString: string;
    readonly CanReadC1C1One2Manies: boolean;
    readonly CanWriteC1C1One2Manies: boolean;
    C1C1One2Manies: C1[];
    AddC1C1One2Many(value: C1): void;
    RemoveC1C1One2Many(value: C1): void;
    readonly CanReadC1C1One2One: boolean;
    readonly CanWriteC1C1One2One: boolean;
    C1C1One2One: C1;
    readonly CanReadI1AllorsString: boolean;
    readonly CanWriteI1AllorsString: boolean;
    I1AllorsString: string;
    readonly CanExecuteClassMethod: boolean;
    readonly ClassMethod: Method;
}
