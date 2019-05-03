import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class DeleteExpression implements Expression {
    constructor(statement: ts.DeleteExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'deleteExpression',
        };
    }
}

