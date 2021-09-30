import { CreateNodeArgs, PluginOptions, SourceNodesArgs, CreateSchemaCustomizationArgs } from 'gatsby';
import { MediaNodeBuilder, SchemaBuilder } from '@allors/gatsby/source/core';
import { Workspace } from '@allors/domain/system';
import { MetaPopulation } from '@allors/meta/system';
import { data, Meta } from '@allors/meta/generated';

import { NodeBuilder } from './NodeBuilder';

import { extend } from './extend';
import { schema } from './schema';

const metaPopulation = new MetaPopulation(data);
const m = metaPopulation as Meta;
export const workspace = new Workspace(metaPopulation);
extend(workspace);
schema(m);

export function createSchemaCustomization(args: CreateSchemaCustomizationArgs, options: PluginOptions) {
  const schemaBuilder = new SchemaBuilder(workspace.metaPopulation, args, options);
  schemaBuilder.build();
}

export async function sourceNodes(args: SourceNodesArgs, options: PluginOptions) {
  const nodeBuilder = new NodeBuilder(workspace, args, options);
  await nodeBuilder.build();
}

export async function onCreateNode(args: CreateNodeArgs, options: PluginOptions) {
  const mediaNodeBuilder = new MediaNodeBuilder(args, options);
  await mediaNodeBuilder.build();
}
