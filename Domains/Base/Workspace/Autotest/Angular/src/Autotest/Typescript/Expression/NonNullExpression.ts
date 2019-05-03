import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class NonNullExpression implements Expression {
    constructor(statement: ts.NonNullExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'nonNullExpression',
        };
    }
}

