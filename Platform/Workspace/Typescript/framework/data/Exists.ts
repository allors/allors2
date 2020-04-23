import { PropertyType, ObjectType } from '../meta';
import { ParametrizedPredicate } from './ParametrizedPredicate';

export class Exists extends ParametrizedPredicate {
    propertyType: PropertyType;

    constructor(fields?: Partial<Exists> | PropertyType) {
        super();

        if ((fields as PropertyType).objectType) {
            this.propertyType = fields as PropertyType;
        } else {
            Object.assign(this, fields);
        }
    }

    get objectType(): ObjectType {
        return this.propertyType.objectType;
    }

    public toJSON(): any {
        return {
            kind: 'Exists',
            propertytype: this.propertyType.id,
            parameter: this.parameter,
        };
    }
}
