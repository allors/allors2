import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class BreakStatement implements Statement {
    constructor(statement: ts.BreakStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'breakStatement',
        };
    }
}

