import { Workspace } from '../framework';

class Domain {

    private extensions: Array<(workspace: Workspace) => void> = [];

    public extend(extension: (workspace: Workspace) => void) {
        this.extensions.push(extension);
    }

    public apply(workspace: Workspace) {
        this.extensions.forEach((v) => {
            v(workspace);
        });
    }
}

export const domain = new Domain();
