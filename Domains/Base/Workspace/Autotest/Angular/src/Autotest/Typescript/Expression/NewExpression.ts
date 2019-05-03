import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class NewExpression implements Expression {
    constructor(statement: ts.NewExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'newExpression',
        };
    }
}

