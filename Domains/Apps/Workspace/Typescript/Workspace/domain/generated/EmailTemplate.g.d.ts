import { SessionObject } from "@allors/framework";
export declare class EmailTemplate extends SessionObject {
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
    readonly CanReadBodyTemplate: boolean;
    readonly CanWriteBodyTemplate: boolean;
    BodyTemplate: string;
    readonly CanReadSubjectTemplate: boolean;
    readonly CanWriteSubjectTemplate: boolean;
    SubjectTemplate: string;
}
