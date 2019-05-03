import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class WithStatement implements Statement {
    constructor(statement: ts.WithStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'withStatement',
        };
    }
}

