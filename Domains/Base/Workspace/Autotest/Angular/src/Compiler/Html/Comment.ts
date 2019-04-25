import * as compiler from '@angular/compiler';

import { Node } from './Node';
import { textSpanIntersectsWith, textChangeRangeIsUnchanged } from 'typescript';

export class Comment implements Node {

    value: string;

    constructor(public node: compiler.Comment) {
        this.value = node.value;
    }

    public toJSON(): any {

        const { value } = this;

        return {
            type: 'comment',
            value
        };
    }
}

