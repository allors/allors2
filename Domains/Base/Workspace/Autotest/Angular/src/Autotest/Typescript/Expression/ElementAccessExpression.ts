import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class ElementAccessExpression implements Expression {
    constructor(statement: ts.ElementAccessExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'elementAccessExpression',
        };
    }
}

