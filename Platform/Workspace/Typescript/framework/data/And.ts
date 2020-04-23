import { Predicate } from './Predicate';

export class And implements Predicate {
    public operands: Predicate[];

    constructor(fields?: Partial<And> | Predicate[]) {
        if (fields instanceof Array) {
            this.operands = fields;
        } else {
            Object.assign(this, fields);
            this.operands = this.operands ? this.operands : [];
        }
    }

    public toJSON(): any {
        return {
            kind: 'And',
            operands: this.operands,
        };
    }
}
