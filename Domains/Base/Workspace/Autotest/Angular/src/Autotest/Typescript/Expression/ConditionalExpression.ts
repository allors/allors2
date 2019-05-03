import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ConditionalExpression implements Expression {
    constructor(statement: ts.ConditionalExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'conditionalExpression',
        };
    }
}

