import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ObjectLiteralExpression implements Expression {
    constructor(statement: ts.ObjectLiteralExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'objectLiteralExpression',
        };
    }
}

