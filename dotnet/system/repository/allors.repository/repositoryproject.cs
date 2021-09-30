// -------------------------------------------------------------------------------------------------
// <copyright file="RepositoryProject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Repository.Roslyn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Domain;
    using Microsoft.CodeAnalysis;

    internal class RepositoryProject
    {
        internal RepositoryProject(Project project)
        {
            this.Project = project;
            this.Solution = project.Solution;

            // compilation = compilation.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            this.Compilation = project.GetCompilationAsync().Result;

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

                ms.Seek(0, SeekOrigin.Begin);
                this.Assembly = Assembly.Load(ms.ToArray());
            }
        }

        public Project Project { get; }

        public Solution Solution { get; }

        public Compilation Compilation { get; }

        public Dictionary<SyntaxTree, SemanticModel> SemanticModelBySyntaxTree { get; }

        public Dictionary<SyntaxTree, Document> DocumentBySyntaxTree { get; }

        public INamedTypeSymbol DomainAttributeType { get; }

        public INamedTypeSymbol ExtendAttributeType { get; }

        public Assembly Assembly { get; set; }
    }
}
