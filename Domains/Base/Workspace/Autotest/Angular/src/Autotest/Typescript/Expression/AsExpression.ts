import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class AsExpression implements Expression {

    expression: Expression;

    constructor(statement: ts.AsExpression, program: Program) {
        this.expression = statement.expression ? program.createExpression(statement.expression) : null;
    }

    public toJSON(): any {

        const { expression } = this;

        return {
            kind: 'asExpression',
            expression
        };
    }
}

