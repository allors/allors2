import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class ForOfStatement implements Statement {
    constructor(statement: ts.ForOfStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'forOfStatement',
        };
    }
}

