import { ClassDeclaration, isPropertyDeclaration, PropertyDeclaration, ClassElement, isMethodDeclaration, MethodDeclaration } from 'typescript';

import { Property } from './Property';
import { Member } from './Member';
import { Method } from './Method';
import { Decorator } from './Decorator';

export class Class {

    name: string;

    decorators: Decorator[];

    members: Member[];

    constructor(classDeclaration: ClassDeclaration) {

        this.name = classDeclaration.name.text;

        this.decorators = classDeclaration.decorators.map((v) => new Decorator(v));

        this.members = classDeclaration.members.map((v) => {

            if (isPropertyDeclaration(v)) {
                return new Property(v as PropertyDeclaration);
            }

            if (isMethodDeclaration(v)) {
                return new Method(v as MethodDeclaration);
            }

            return undefined;
        }).filter((v) => v);

    }

    public toJSON(): any {

        const { name, members } = this;

        return {
            kind: 'class',
            name,
            members,
        };
    }
}

