import { CreateNodeArgs, PluginOptions, SourceNodesArgs, CreateSchemaCustomizationArgs } from 'gatsby';
import { MediaNodeBuilder } from './allors/gatsby/MediaNodeBuilder';
import { NodeBuilder } from './allors/NodeBuilder';
import { SchemaBuilder } from './allors/SchemaBuilder';
import metaPopulation from './allors/metaPopulation';

export function createSchemaCustomization(args: CreateSchemaCustomizationArgs, options: PluginOptions){
  var schemaBuilder = new SchemaBuilder(metaPopulation, args, options);
  schemaBuilder.build();
}

export async function sourceNodes(args: SourceNodesArgs, options: PluginOptions) {
  var nodeBuilder = new NodeBuilder(metaPopulation, args, options);
  await nodeBuilder.build();
}

export async function onCreateNode(args: CreateNodeArgs, options: PluginOptions) {

  var mediaNodeBuilder = new MediaNodeBuilder(args, options);
  await mediaNodeBuilder.build();
}
