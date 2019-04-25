import { Decorator as TsDecorator } from 'typescript';

import { Member } from './Member';

export class Decorator implements Member {

    text: string;

    constructor(tsDecorator: TsDecorator) {

        this.text = tsDecorator.getText();
    }


    public toJSON(): any {

        const { text } = this;

        return {
            kind: 'decorator',
            text,
        };
    }

}

