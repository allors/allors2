const moduleAlias = require('module-alias')

const libs = `${__dirname}/libs`
moduleAlias.addAliases({
  "@allors/data/system": `${libs}/data/system/src/index.js`,
  "@allors/domain/core": `${libs}/domain/core/src/index.js`,
  "@allors/domain/custom": `${libs}/domain/custom/src/index.js`,
  "@allors/domain/generated": `${libs}/domain/generated/src/index.js`,
  "@allors/domain/system": `${libs}/domain/system/src/index.js`,
  "@allors/gatsby/source/core": `${libs}/gatsby/source/core/src/index.js`,
  "@allors/gatsby/source/custom": `${libs}/gatsby/source/custom/src/index.js`,
  "@allors/gatsby/source/system": `${libs}/data/source/system/src/index.js`,
  "@allors/meta/generated": `${libs}/meta/generated/src/index.js`,
  "@allors/meta/system": `${libs}/meta/system/src/index.js`,
  "@allors/promise/core": `${libs}/promise/core/src/index.js`,
  "@allors/protocol/system": `${libs}/protocol/system/src/index.js`
});

const custom = require('@allors/gatsby/source/custom');

exports.createSchemaCustomization = (args, options) => {
  custom.createSchemaCustomization(args, options);
};

exports.sourceNodes = async (args, options) => {
  await custom.sourceNodes(args, options);
};

exports.onCreateNode = async (args, options) => {
  await custom.onCreateNode(args, options);
};
