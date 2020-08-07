import * as compiler from '@angular/compiler';

import { Node } from './Node';

export class Attribute implements Node {

    name: string;

    value: string;

    constructor(public node: compiler.Attribute) {
        this.name = node.name;
        this.value = node.value;
    }

    public toJSON(): any {

        const { name, value } = this;

        return {
            kind: 'attribute',
            name,
            value
        };
    }
}

