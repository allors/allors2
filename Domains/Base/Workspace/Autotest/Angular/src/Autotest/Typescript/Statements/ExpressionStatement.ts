import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ExpressionStatement implements Statement {
    constructor(statement: ts.ExpressionStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'expressionStatement',
        };
    }
}

