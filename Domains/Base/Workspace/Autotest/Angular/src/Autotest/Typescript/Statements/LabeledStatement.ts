import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class LabeledStatement implements Statement {
    constructor(statement: ts.LabeledStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'labeledStatement',
        };
    }
}

