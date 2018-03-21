// ------------------------------------------------------------------------------------------------- 
// <copyright file="RepositoryProject.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Repository.Roslyn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;

    using Allors.Repository.Domain;

    using Microsoft.CodeAnalysis;

    using Document = Microsoft.CodeAnalysis.Document;

    internal class RepositoryProject 
    {
        public Project Project { get; }

        public Solution Solution { get; }

        public Compilation Compilation { get; }

        public Dictionary<SyntaxTree, SemanticModel> SemanticModelBySyntaxTree { get; }

        public Dictionary<SyntaxTree, Document> DocumentBySyntaxTree { get; }

        public INamedTypeSymbol DomainAttributeType { get; }

        public INamedTypeSymbol ExtendAttributeType { get; }

        public System.Reflection.Assembly Assembly { get; set; }

        internal RepositoryProject(Project project)
        {
            this.Project = project;
            this.Solution = project.Solution;
            var compilation = project.GetCompilationAsync().Result;
            compilation = compilation.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            this.Compilation = compilation;

            this.SemanticModelBySyntaxTree = this.Compilation.SyntaxTrees.ToDictionary(v => v, v => this.Compilation.GetSemanticModel(v));
            this.DocumentBySyntaxTree = this.Compilation.SyntaxTrees.ToDictionary(v => v, v => this.Solution.GetDocument(v));

            this.DomainAttributeType = this.Compilation.GetTypeByMetadataName(Repository.AttributeNamespace + ".DomainAttribute");
            this.ExtendAttributeType = this.Compilation.GetTypeByMetadataName(Repository.AttributeNamespace + ".ExtendsAttribute");

            using (var ms = new MemoryStream())
            {
                var result = this.Compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    var text = string.Join("\n", failures.Select(x => x.Id + ": " + x.GetMessage()));
                    throw new Exception(text);
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    this.Assembly = System.Reflection.Assembly.Load(ms.ToArray());
                }
            }

        }
    }
}
