import { PipeSymbol } from "../ngast/pipe-symbol";
import { PathResolver } from '../Helpers';

export class Pipe {
    name: string;
    path: string;

    constructor(public pipe: PipeSymbol, pathResolver: PathResolver) {
        this.name = this.pipe.symbol.name;
        this.path = pathResolver.relative(pipe.symbol.filePath);
    }


    public toJSON(): any {

        const { name, path } = this;

        return {
            kind: 'pipe',
            name,
            path,
        };
    }
}

