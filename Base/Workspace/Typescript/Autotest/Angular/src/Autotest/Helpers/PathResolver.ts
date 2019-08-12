import * as path from 'path';

function forwardSlash(input: string): string {
    return input.replace(/\\/g, '/');
}


export class PathResolver {

    private base: string;

    constructor(base: string) {
        this.base = path.resolve(base);
    }

    relative(to: string): string {
        if (to) {
            return forwardSlash(path.relative(this.base, to));
        }
    }

    resolve(...pathSegments: string[]) {
        if (pathSegments && pathSegments.length > 0) {
            return forwardSlash(path.resolve(this.base, ...pathSegments));
        }
    }
}

