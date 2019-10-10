#! /usr/bin/env node

const path = require('path');
const lnk = require('lnk');

function link(src, dst){
    const basename = path.basename(src);

    lnk([src], dst)
    .then(() => console.log(basename + ' linked') )
    .catch(() =>  console.log(basename + ' already linked'))
}

link ('../Angular', 'wwwroot/Core');
link ('../Workspace', 'wwwroot/Core');
