import { ObjectType, PropertyType } from '../meta';

import { Predicate } from './Predicate';

export class Instanceof implements Predicate {
    public propertyType: PropertyType;
    public objectType: ObjectType;

    constructor(fields?: Partial<Instanceof> | PropertyType) {
        if ((fields as PropertyType).objectType) {
            this.propertyType = fields as PropertyType;
        } else {
            Object.assign(this, fields);
        }
    }

    public toJSON(): any {
        return {
            kind: 'Instanceof',
            propertytype: this.propertyType.id,
            objecttype: this.objectType.id,
        };
    }
}
