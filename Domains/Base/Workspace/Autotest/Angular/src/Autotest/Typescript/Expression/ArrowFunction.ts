import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';
import { Statement } from '../Statements/Statement';

export class ArrowFunction implements Expression {
    expression: Expression;
    functionBody: Statement;

    constructor(statement: ts.ArrowFunction, program: Program) {
        const body = statement.body;

        if (body) {
            if (ts.isBlock(body)) {
                this.functionBody = program.createStatement(body);
            } else {
                this.expression = program.createExpression(body);
            }
        }
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'arrowFunction',
        };
    }
}

