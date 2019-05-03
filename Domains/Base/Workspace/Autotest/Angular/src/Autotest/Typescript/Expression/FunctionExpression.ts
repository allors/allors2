import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class FunctionExpression implements Expression {
    constructor(statement: ts.FunctionExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'functionExpression',
        };
    }
}

