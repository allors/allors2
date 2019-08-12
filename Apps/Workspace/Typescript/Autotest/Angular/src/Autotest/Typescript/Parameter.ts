import { ParameterDeclaration } from 'typescript';

export class Parameter {

    name: string;
    type: string;

    constructor(declaration: ParameterDeclaration) {

        this.name = declaration.name.getText();
        if (declaration.type) {
            this.type = declaration.type.getText();
        }
    }

    public toJSON(): any {

        const { name, type } = this;

        return {
            kind: 'parameter',
            name,
            type,
        };
    }
}

