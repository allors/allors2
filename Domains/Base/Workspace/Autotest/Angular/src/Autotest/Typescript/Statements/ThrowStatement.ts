import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ThrowStatement implements Statement {
    constructor(statement: ts.ThrowStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'throwStatement',
        };
    }
}

