import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ArrayLiteralExpression implements Expression {

    elements: Expression[];

    constructor(statement: ts.ArrayLiteralExpression, program: Program) {
        this.elements = statement.elements ? statement.elements.map((v) => program.createExpression(v)) : null;
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'arrayLiteralExpression',
        };
    }
}

