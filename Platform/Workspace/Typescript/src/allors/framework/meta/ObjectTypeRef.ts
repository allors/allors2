import { ObjectType } from './ObjectType';

export function objectTypeId(objectType: ObjectType | ObjectTypeRef | string): string {
    return objectType instanceof ObjectType ? objectType.id : ((objectType as ObjectTypeRef).objectType ? (objectType as ObjectTypeRef).objectType.id : objectType as string);
}

export interface ObjectTypeRef {
    objectType: ObjectType;
}
