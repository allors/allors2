#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch((e) =>  e.errno && e.errno != -4075 ? console.log(e) : console.log('already linked'))
}

link ('../../../../Platform/Workspace/Typescript/workspace/libs/workspace/data/src', 'libs/workspace/data');
link ('../../../../Platform/Workspace/Typescript/workspace/libs/workspace/domain/src', 'libs/workspace/domain');
link ('../../../../Platform/Workspace/Typescript/workspace/libs/workspace/meta/src', 'libs/workspace/meta');
link ('../../../../Platform/Workspace/Typescript/workspace/libs/workspace/protocol/src', 'libs/workspace/protocol');
