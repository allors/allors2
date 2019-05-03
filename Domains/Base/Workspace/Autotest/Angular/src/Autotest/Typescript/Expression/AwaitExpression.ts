import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class AwaitExpression implements Expression {
    expression: Expression;

    constructor(statement: ts.AwaitExpression, program: Program) {
        this.expression = statement.expression ? program.createExpression(statement.expression) : null;
    }

    public toJSON(): any {

        const { expression} = this;

        return {
            kind: 'awaitExpression',
            expression
        };
    }
}

