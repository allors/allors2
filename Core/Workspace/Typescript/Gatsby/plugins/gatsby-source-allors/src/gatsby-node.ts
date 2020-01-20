import { NodeBuilder } from './allors/NodeBuilder';
import { CreateNodeArgs, PluginOptions, SourceNodesArgs } from 'gatsby';
import { MediaNodeBuilder } from './allors/gatsby/MediaNodeBuilder';

export async function sourceNodes(args: SourceNodesArgs, options: PluginOptions) {

  var nodeBuilder = new NodeBuilder(args, options);
  await nodeBuilder.build();
}

export async function onCreateNode(args: CreateNodeArgs, options: PluginOptions) {

  var mediaNodeBuilder = new MediaNodeBuilder(args, options);
  await mediaNodeBuilder.build();
}
