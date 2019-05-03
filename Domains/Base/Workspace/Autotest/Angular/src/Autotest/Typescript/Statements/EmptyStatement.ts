import * as ts from 'typescript';

import { Program } from '../Program';
import { Statement } from './Statement';

export class EmptyStatement implements Statement {
    constructor(statement: ts.EmptyStatement, program: Program) {
    }

    public toJSON(): any {

        const { } = this;

        return {
            kind: 'emptyStatement',
        };
    }
}

