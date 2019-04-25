import { StaticSymbol } from '@angular/compiler';
import { ProviderSymbol } from "ngast";
import { PathResolver } from './Helpers';
import { Reference } from './Typescript/Reference';

export class Provider {

    tokenIdentifier: Reference;
    tokenValue: string;

    useClass: Reference;
    useValue: string;
    useExisting: any;
    useFactory: Reference;
    multi: boolean;

    constructor(public provider: ProviderSymbol, pathResolver: PathResolver) {

        const metadata = provider.getMetadata();
        const token = metadata.token;

        this.tokenIdentifier = token.identifier ? Reference.fromSymbol(token.identifier.reference, pathResolver) : undefined;
        this.tokenValue = token.value;

        if(metadata.useValue && metadata.useValue.separatorKeyCodes){
            console.log(metadata.useValue);
        }

        this.useClass = metadata.useClass ? Reference.fromSymbol(metadata.useClass.reference, pathResolver) : undefined;
        this.useValue = Reference.fromValue(metadata.useValue, pathResolver);
        this.useExisting = metadata.useExisting;
        this.useFactory = metadata.useFactory ? Reference.fromSymbol(metadata.useFactory.reference, pathResolver) : undefined;
        this.multi = metadata.multi;
    }

    public toJSON(): any {

        const { tokenIdentifier, tokenValue, useValue } = this;

        return {
            kind: 'provider',
            tokenIdentifier,
            tokenValue,
            useValue,
        };
    }

}

