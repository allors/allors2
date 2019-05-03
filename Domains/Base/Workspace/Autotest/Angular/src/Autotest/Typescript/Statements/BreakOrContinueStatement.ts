import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class BreakOrContinueStatement implements Statement {
    constructor(statement: ts.BreakOrContinueStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'breakOrContinueStatement',
        };
    }
}

