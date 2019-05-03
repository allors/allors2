import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class CallExpression implements Expression {
    constructor(statement: ts.CallExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'callExpression',
        };
    }
}

