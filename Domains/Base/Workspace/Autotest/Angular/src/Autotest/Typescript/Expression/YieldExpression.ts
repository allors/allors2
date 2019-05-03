import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class YieldExpression implements Expression {
    constructor(statement: ts.YieldExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'yieldExpression',
        };
    }
}

