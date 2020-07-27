import * as compiler from '@angular/compiler';

import { Node } from './Node';

export class Comment implements Node {

    value: string;

    constructor(public node: compiler.Comment) {
        this.value = node.value;
    }

    public toJSON(): any {

        const { value } = this;

        return {
            kind: 'comment',
            value
        };
    }
}

