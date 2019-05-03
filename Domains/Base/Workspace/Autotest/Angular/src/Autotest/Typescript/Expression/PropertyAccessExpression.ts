import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class PropertyAccessExpression implements Expression {
    constructor(statement: ts.PropertyAccessExpression, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'propertyAccessExpression',
        };
    }
}

