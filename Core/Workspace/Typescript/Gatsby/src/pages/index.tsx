import { graphql, Link } from "gatsby"
import Img from "gatsby-image"
import * as React from "react"
import Layout from "../components/layout"

interface IndexPageProps {
  data: {
    site: {
      siteMetadata: {
        siteName: string
      }
    },
    allAllorsOrganisation: {
      edges: [{
        node: {
          slug: string,
          name: string,
        }
      }]
    },
  }
}

export default class IndexPage extends React.Component<IndexPageProps> {
  public render() {
    const { siteName } = this.props.data.site.siteMetadata;
    const { allAllorsOrganisation } = this.props.data;

    const { allFile } = this.props.data as any;
    const image = allFile.edges[0].node.childImageSharp.fluid;

    return (
      <Layout>
        <h1>{siteName}</h1>

        <Img fluid={image} />

        {allAllorsOrganisation.edges.map(v =>
          <ul>
            <li><Link to={v.node.slug}>{v.node.name}</Link></li>
          </ul>
        )
        }
      </Layout>
    )
  }
}

export const pageQuery = graphql`
  query IndexQuery {
    site {
      siteMetadata {
        siteName
      }
    },
    allAllorsOrganisation {
      edges {
        node {
          name
          slug
        }
      }
    },
    allFile {
      edges {
        node {
          name,
          childImageSharp{
            id,
            fluid {
              ...GatsbyImageSharpFluid
            }
          }
        }
      }
    }
  }
`
