import * as compiler from '@angular/compiler';

import { Node } from './Node';

export class Text implements Node {

    value: string;

    constructor(public node: compiler.Text) {
        this.value = this.node.value;
    }

    public toJSON(): any {

        const { value } = this;

        return {
            kind: 'text',
            value,
        };
    }
}

