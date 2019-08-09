import * as compiler from '@angular/compiler';

import { Node, nodeFactory } from './Node';

export class ExpansionCase implements Node {

    expression: Node[];

    value: string;

    constructor(public node: compiler.ExpansionCase) {
        this.expression = node.expression.map((v) => nodeFactory(v));
        this.value = node.value;
    }

    public toJSON(): any {

        const { expression, value } = this;

        return {
            kind: 'expansionCase',
            expression,
            value
        };
    }
}

