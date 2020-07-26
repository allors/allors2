import { ResourceResolver as NgastResourceResolver } from '../../ngast/resource-resolver';
import { readFile, readFileSync } from 'fs';

export class ResourceResolver implements NgastResourceResolver {

    public get(url: string): Promise<string> {
        return new Promise((resolve, reject) => {
            readFile(url, 'utf-8', (err, content) => {
                if (err) {
                    reject(err);
                } else {
                    resolve(content);
                }
            });
        });
    }

    getSync(url: string) {
        return readFileSync(url).toString();
    }
};
