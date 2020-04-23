import { ObjectType } from '../meta';
import { Predicate } from './Predicate';
import { Sort } from './Sort';

export class Filter {
    public objectType: ObjectType;

    public predicate: Predicate;

    public sort: Sort[];

    constructor(fields?: Partial<Filter> | ObjectType) {
        if (fields instanceof ObjectType) {
            this.objectType = fields;
        } else {
            Object.assign(this, fields);
        }
    }

    public toJSON(): any {
        return {
            kind: 'Filter',
            objecttype: this.objectType.id,
            predicate: this.predicate,
            sorting: this.sort,
        };
    }
}
