import * as ts from 'typescript';

import { Program } from '../Program';
import { Expression } from './Expression';

export class SpreadElement implements Expression {
    constructor(statement: ts.SpreadElement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'spreadElement',
        };
    }
}

