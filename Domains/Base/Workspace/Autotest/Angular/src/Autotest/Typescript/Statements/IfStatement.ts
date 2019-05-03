import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';
import { Expression } from '../Expression/Expression';

export class IfStatement implements Statement {

    expression: Expression;
    thenStatement: Statement;
    elseStatement: Statement;

    constructor(statement: ts.IfStatement, program: Program) {
        this.expression = statement.expression ? program.createExpression(statement.expression) : undefined;
        this.thenStatement = statement.thenStatement ? program.createStatement(statement.thenStatement) : undefined;
        this.elseStatement = statement.elseStatement ? program.createStatement(statement.elseStatement) : undefined;
    }

    public toJSON(): any {

        const { thenStatement, elseStatement} = this;

        return {
            kind: 'ifStatement',
            thenStatement,
            elseStatement,
        };
    }
}

