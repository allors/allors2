import { Member } from './Member';

export interface Type {
    name: string;
    
    decorators: string[];

    members: Member[];
}

