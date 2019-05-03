import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ParenthesizedExpression implements Expression {
    constructor(statement: ts.ParenthesizedExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'parenthesizedExpression',
        };
    }
}

