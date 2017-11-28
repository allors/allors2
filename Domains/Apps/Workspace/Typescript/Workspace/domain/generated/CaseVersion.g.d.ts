import { SessionObject } from "@allors/framework";
import { Version } from './Version.g';
import { CaseState } from './CaseState.g';
export declare class CaseVersion extends SessionObject implements Version {
    readonly CanReadCaseState: boolean;
    readonly CaseState: CaseState;
    readonly CanReadDerivationTimeStamp: boolean;
    readonly CanWriteDerivationTimeStamp: boolean;
    DerivationTimeStamp: Date;
}
