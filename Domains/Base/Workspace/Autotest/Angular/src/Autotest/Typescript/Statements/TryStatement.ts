import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class TryStatement implements Statement {
    constructor(statement: ts.TryStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'tryStatement',
        };
    }
}

