import * as compiler from '@angular/compiler';

import { Node } from './Node';
import { ExpansionCase } from './ExpansionCase';

export class Expansion implements Node {

    switchValue: string;

    cases: ExpansionCase[];

    constructor(public node: compiler.Expansion) {
        this.switchValue = node.switchValue;
        this.cases = node.cases.map((v) => new ExpansionCase(v));
    }

    public toJSON(): any {

        const { switchValue, cases } = this;

        return {
            kind: 'expansion',
            name,
            cases
        };
    }
}

