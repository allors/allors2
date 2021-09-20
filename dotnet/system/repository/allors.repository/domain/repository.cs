// <copyright file="Repository.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NLog;
    using Roslyn;

    public class Repository
    {
        public const string RepositoryNamespaceName = "Allors.Repository";

        public const string AttributeNamespace = RepositoryNamespaceName + ".Attributes";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Inflector inflector;

        public Repository(Project project)
        {
            this.DomainByName = new Dictionary<string, Domain>();
            this.UnitBySingularName = new Dictionary<string, Unit>();
            this.InterfaceBySingularName = new Dictionary<string, Interface>();
            this.ClassBySingularName = new Dictionary<string, Class>();
            this.CompositeByName = new Dictionary<string, Composite>();
            this.TypeBySingularName = new Dictionary<string, Type>();

            this.inflector = new Inflector(new CultureInfo("en"));

            var projectInfo = new RepositoryProject(project);

            this.CreateDomains(projectInfo);

            this.CreateUnits();
            this.CreateTypes(projectInfo);
            this.CreateHierarchy(projectInfo);
            this.CreateMembers(projectInfo);

            this.FromReflection(projectInfo);

            this.LinkImplementations();

            this.CreateInheritedProperties();
            this.CreateReverseProperties();

            var ids = new HashSet<Guid>();

            foreach (var composite in this.Composites)
            {
                this.CheckId(ids, composite.Id, $"{composite.SingularName}", "id");

                // TODO: Add id checks for properties
                foreach (var property in composite.DefinedProperties)
                {
                    this.CheckId(ids, property.Id, $"{composite.SingularName}.{property.RoleName}", "id");
                }

                foreach (var method in composite.DefinedMethods)
                {
                    if (!method.AttributeByName.ContainsKey(AttributeNames.Id))
                    {
                        this.HasErrors = true;
                        Logger.Error($"{method} has no {AttributeNames.Id} attribute.");
                    }
                }
            }
        }

        public Domain[] Domains => this.DomainByName.Values.ToArray();

        public Unit[] Units => this.UnitBySingularName.Values.ToArray();

        public Interface[] Interfaces => this.InterfaceBySingularName.Values.ToArray();

        public Class[] Classes => this.ClassBySingularName.Values.ToArray();

        public Type[] Types => this.Composites.Cast<Type>().Union(this.Units).ToArray();

        public Composite[] Composites => this.ClassBySingularName.Values.Cast<Composite>().Union(this.InterfaceBySingularName.Values).ToArray();

        public Dictionary<string, Domain> DomainByName { get; }

        public Dictionary<string, Unit> UnitBySingularName { get; }

        public Dictionary<string, Interface> InterfaceBySingularName { get; }

        public Dictionary<string, Class> ClassBySingularName { get; }

        public Dictionary<string, Type> TypeBySingularName { get; }

        public Dictionary<string, Composite> CompositeByName { get; }

        public Domain[] SortedDomains
        {
            get
            {
                var assemblies = this.Domains.ToList();
                assemblies.Sort((x, y) => x.Base == y ? 1 : -1);
                return assemblies.ToArray();
            }
        }

        public bool HasErrors { get; set; }

        protected void CreateUnits()
        {
            var binary = new Unit(UnitIds.Binary, UnitNames.Binary);
            this.UnitBySingularName.Add(binary.SingularName, binary);
            this.TypeBySingularName.Add(binary.SingularName, binary);

            var boolean = new Unit(UnitIds.Boolean, UnitNames.Boolean);
            this.UnitBySingularName.Add(boolean.SingularName, boolean);
            this.TypeBySingularName.Add(boolean.SingularName, boolean);

            var datetime = new Unit(UnitIds.DateTime, UnitNames.DateTime);
            this.UnitBySingularName.Add(datetime.SingularName, datetime);
            this.TypeBySingularName.Add(datetime.SingularName, datetime);

            var @decimal = new Unit(UnitIds.Decimal, UnitNames.Decimal);
            this.UnitBySingularName.Add(@decimal.SingularName, @decimal);
            this.TypeBySingularName.Add(@decimal.SingularName, @decimal);

            var @float = new Unit(UnitIds.Float, UnitNames.Float);
            this.UnitBySingularName.Add(@float.SingularName, @float);
            this.TypeBySingularName.Add(@float.SingularName, @float);

            var integer = new Unit(UnitIds.Integer, UnitNames.Integer);
            this.UnitBySingularName.Add(integer.SingularName, integer);
            this.TypeBySingularName.Add(integer.SingularName, integer);

            var @string = new Unit(UnitIds.String, UnitNames.String);
            this.UnitBySingularName.Add(@string.SingularName, @string);
            this.TypeBySingularName.Add(@string.SingularName, @string);

            var unique = new Unit(UnitIds.Unique, UnitNames.Unique);
            this.UnitBySingularName.Add(unique.SingularName, unique);
            this.TypeBySingularName.Add(unique.SingularName, unique);
        }

        private void CreateDomains(RepositoryProject repositoryProject)
        {
            try
            {
                var parentByChild = new Dictionary<string, string>();

                foreach (var syntaxTree in repositoryProject.DocumentBySyntaxTree.Keys)
                {
                    var root = syntaxTree.GetRoot();
                    foreach (var structDeclaration in root.DescendantNodes().OfType<StructDeclarationSyntax>())
                    {
                        var semanticModel = repositoryProject.Compilation.GetSemanticModel(syntaxTree);
                        var structureModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(structDeclaration);
                        var domainAttribute = structureModel.GetAttributes()
                            .FirstOrDefault(v => v.AttributeClass.Name.Equals("DomainAttribute"));

                        if (domainAttribute != null)
                        {
                            var id = Guid.Parse((string)domainAttribute.ConstructorArguments.First().Value);

                            var document = repositoryProject.DocumentBySyntaxTree[syntaxTree];
                            var fileInfo = new FileInfo(document.FilePath);
                            var directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);

                            var domain = new Domain(id, structureModel.Name, directoryInfo);
                            this.DomainByName.Add(domain.Name, domain);

                            var extendsAttribute = structureModel.GetAttributes()
                                .FirstOrDefault(v => v.AttributeClass.Name.Equals("ExtendsAttribute"));
                            var parent = (string)extendsAttribute?.ConstructorArguments.First().Value;

                            if (!string.IsNullOrEmpty(parent))
                            {
                                parentByChild.Add(domain.Name, parent);
                            }
                        }
                    }
                }

                foreach (var child in parentByChild.Keys)
                {
                    var parent = parentByChild[child];
                    this.DomainByName[child].Base = this.DomainByName[parent];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void CreateTypes(RepositoryProject repositoryProject)
        {
            foreach (var syntaxTree in repositoryProject.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = repositoryProject.SemanticModelBySyntaxTree[syntaxTree];
                var document = repositoryProject.DocumentBySyntaxTree[syntaxTree];
                var fileInfo = new FileInfo(document.FilePath);

                foreach (var interfaceDeclaration in root.DescendantNodes().OfType<InterfaceDeclarationSyntax>())
                {
                    var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                    var idAttribute = typeModel.GetAttributes().FirstOrDefault(v => v.AttributeClass.Name.Equals("IdAttribute"));

                    if (idAttribute != null)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Domains.First(v => v.DirectoryInfo.Contains(fileInfo));

                        var symbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                        if (RepositoryNamespaceName.Equals(symbol.ContainingNamespace.ToDisplayString()))
                        {
                            var interfaceSingularName = symbol.Name;

                            var partialInterface = new PartialInterface(interfaceSingularName);
                            domain.PartialInterfaceByName.Add(interfaceSingularName, partialInterface);
                            domain.PartialTypeBySingularName.Add(interfaceSingularName, partialInterface);

                            if (!this.InterfaceBySingularName.TryGetValue(interfaceSingularName, out var @interface))
                            {
                                @interface = new Interface(this.inflector, id, interfaceSingularName);
                                this.InterfaceBySingularName.Add(interfaceSingularName, @interface);
                                this.CompositeByName.Add(interfaceSingularName, @interface);
                                this.TypeBySingularName.Add(interfaceSingularName, @interface);
                            }

                            @interface.PartialByDomainName.Add(domain.Name, partialInterface);
                            var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                            @interface.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                        }
                    }
                }

                foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(classDeclaration);
                    var idAttribute = typeModel.GetAttributes().FirstOrDefault(v => v.AttributeClass.Name.Equals("IdAttribute"));

                    if (idAttribute != null)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Domains.FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));
                        if (domain == null)
                        {
                            throw new Exception($"No domain for file {fileInfo}");
                        }

                        var symbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                        if (RepositoryNamespaceName.Equals(symbol.ContainingNamespace.ToDisplayString()))
                        {
                            var classSingularName = symbol.Name;

                            var partialClass = new PartialClass(classSingularName);
                            domain.PartialClassBySingularName.Add(classSingularName, partialClass);
                            domain.PartialTypeBySingularName.Add(classSingularName, partialClass);

                            if (!this.ClassBySingularName.TryGetValue(classSingularName, out var @class))
                            {
                                @class = new Class(this.inflector, id, classSingularName);
                                this.ClassBySingularName.Add(classSingularName, @class);
                                this.CompositeByName.Add(classSingularName, @class);
                                this.TypeBySingularName.Add(classSingularName, @class);
                            }

                            @class.PartialByDomainName.Add(domain.Name, partialClass);

                            var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                            @class.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                        }
                    }
                }
            }
        }

        private void CreateHierarchy(RepositoryProject repositoryProject)
        {
            var definedTypeBySingularName = repositoryProject.Assembly.DefinedTypes.Where(v => RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            var composites = this.CompositeByName.Values.ToArray();

            foreach (var composite in composites)
            {
                var definedType = definedTypeBySingularName[composite.SingularName];
                var allInterfaces = definedType.GetInterfaces();
                foreach (var definedImplementedInterface in allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())))
                {
                    if (this.InterfaceBySingularName.TryGetValue(definedImplementedInterface.Name, out var implementedInterface))
                    {
                        composite.ImplementedInterfaces.Add(implementedInterface);
                    }
                    else
                    {
                        throw new Exception("Can not find implemented interface " + definedImplementedInterface.Name + " on " + composite.SingularName);
                    }
                }
            }
            
            foreach (var composite in composites)
            {
                composite.Subtypes = composites.Where(v => v.Interfaces.Contains(composite)).ToArray();
            }
        }

        private void CreateMembers(RepositoryProject repositoryProject)
        {
            foreach (var syntaxTree in repositoryProject.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = repositoryProject.SemanticModelBySyntaxTree[syntaxTree];
                var document = repositoryProject.DocumentBySyntaxTree[syntaxTree];
                var fileInfo = new FileInfo(document.FilePath);
                var domain = this.Domains.FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));

                if (domain != null)
                {
                    var typeDeclaration = root.DescendantNodes().SingleOrDefault(v => v is InterfaceDeclarationSyntax || v is ClassDeclarationSyntax);
                    if (typeDeclaration != null)
                    {
                        var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                        var typeName = typeSymbol.Name;

                        if (domain.PartialTypeBySingularName.TryGetValue(typeName, out var partialType))
                        {
                            var composite = this.CompositeByName[typeName];
                            foreach (var propertyDeclaration in typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                            {
                                var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration);
                                var propertyRoleName = propertySymbol.Name;

                                var xmlDocString = propertySymbol.GetDocumentationCommentXml(null, true);
                                var xmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

                                var property = new Property(this.inflector, composite, propertyRoleName)
                                {
                                    XmlDoc = xmlDoc,
                                };

                                partialType.PropertyByName.Add(propertyRoleName, property);
                                composite.PropertyByRoleName.Add(propertyRoleName, property);
                            }

                            foreach (var methodDeclaration in typeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
                            {
                                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
                                var methodName = methodSymbol.Name;

                                var xmlDocString = methodSymbol.GetDocumentationCommentXml(null, true);
                                var xmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

                                var method = new Method(composite, methodName)
                                {
                                    XmlDoc = xmlDoc,
                                };

                                partialType.MethodByName.Add(methodName, method);
                                composite.MethodByName.Add(methodName, method);
                            }
                        }
                    }
                }
            }
        }

        private void FromReflection(RepositoryProject repositoryProject)
        {
            var declaredTypeBySingularName = repositoryProject.Assembly.DefinedTypes.Where(v => RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            foreach (var composite in this.CompositeByName.Values)
            {
                var reflectedType = declaredTypeBySingularName[composite.SingularName];

                // Type attributes
                {
                    foreach (var group in reflectedType.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
                    {
                        var type = group.Key;
                        var typeName = type.Name;
                        if (typeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            typeName = typeName.Substring(0, typeName.Length - "attribute".Length);
                        }

                        var attributeUsage = type.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            composite.AttributesByName[typeName] = group.ToArray();
                        }
                        else
                        {
                            composite.AttributeByName[typeName] = group.First();
                        }
                    }
                }

                // Property attributes
                foreach (var property in composite.Properties)
                {
                    var reflectedProperty = reflectedType.GetProperty(property.RoleName);
                    if (reflectedProperty == null)
                    {
                        this.HasErrors = true;
                        Logger.Error($"{reflectedType.Name}.{property.RoleName} should be public");
                        continue;
                    }

                    var propertyAttributesByTypeName = reflectedProperty.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    var reflectedPropertyType = reflectedProperty.PropertyType;
                    var typeName = this.GetTypeName(reflectedPropertyType);
                    property.Type = this.TypeBySingularName[typeName];

                    foreach (var group in propertyAttributesByTypeName)
                    {
                        var attributeType = group.Key;
                        var attributeTypeName = attributeType.Name;
                        if (attributeTypeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            attributeTypeName = attributeTypeName.Substring(
                                0,
                                attributeTypeName.Length - "attribute".Length);
                        }

                        var attributeUsage =
                            attributeType.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            property.AttributesByName.Add(attributeTypeName, group.ToArray());
                        }
                        else
                        {
                            property.AttributeByName.Add(attributeTypeName, group.First());
                        }
                    }

                    if (property.AttributeByName.Keys.Contains("Multiplicity"))
                    {
                        if (reflectedPropertyType.Name.EndsWith("[]"))
                        {
                            if (property.IsRoleOne)
                            {
                                this.HasErrors = true;
                                Logger.Error($"{reflectedType.Name}.{property.RoleName} should be many");
                            }
                        }
                        else if (property.IsRoleMany)
                        {
                            this.HasErrors = true;
                            Logger.Error($"{reflectedType.Name}.{property.RoleName} should be one");
                        }
                    }
                }

                foreach (var method in composite.Methods)
                {
                    var reflectedMethod = reflectedType.GetMethod(method.Name);
                    foreach (var group in reflectedMethod.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
                    {
                        var attributeType = group.Key;
                        var attributeTypeName = attributeType.Name;
                        if (attributeTypeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            attributeTypeName = attributeTypeName.Substring(0, attributeTypeName.Length - "attribute".Length);
                        }

                        var attributeUsage = attributeType.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            method.AttributesByName.Add(attributeTypeName, group.ToArray());
                        }
                        else
                        {
                            method.AttributeByName.Add(attributeTypeName, group.First());
                        }
                    }
                }
            }
        }

        private void LinkImplementations()
        {
            foreach (var @class in this.Classes)
            {
                foreach (var property in @class.Properties)
                {
                    property.DefiningProperty = @class.GetImplementedProperty(property);
                }

                foreach (var method in @class.Methods)
                {
                    method.DefiningMethod = @class.GetImplementedMethod(method);
                }
            }
        }

        private string GetTypeName(System.Type type)
        {
            var typeName = type.Name;
            if (typeName.EndsWith("[]"))
            {
                typeName = typeName.Substring(0, typeName.Length - "[]".Length);
            }

            switch (typeName)
            {
                case "Byte":
                case "byte":
                    return "Binary";

                case "Boolean":
                case "bool":
                    return "Boolean";

                case "Decimal":
                case "decimal":
                    return "Decimal";

                case "DateTime":
                    return "DateTime";

                case "Double":
                case "double":
                    return "Float";

                case "Int32":
                case "int":
                    return "Integer";

                case "String":
                case "string":
                    return "String";

                case "Guid":
                    return "Unique";

                default:
                    return typeName;
            }
        }

        private void CreateInheritedProperties()
        {
            foreach (var @interface in this.Interfaces)
            {
                foreach (var supertype in @interface.Interfaces)
                {
                    foreach (var property in supertype.DefinedProperties)
                    {
                        @interface.InheritedPropertyByRoleName.Add(property.RoleName, property);
                    }
                }
            }
        }

        private void CreateReverseProperties()
        {
            foreach (var composite in this.Composites)
            {
                foreach (var property in composite.DefinedProperties)
                {
                    var reverseType = property.Type;
                    var reverseComposite = reverseType as Composite;
                    reverseComposite?.DefinedReversePropertyByAssociationName.Add(property.AssociationName, property);
                }
            }

            foreach (var composite in this.Composites)
            {
                foreach (var supertype in composite.Interfaces)
                {
                    foreach (var property in supertype.DefinedReverseProperties)
                    {
                        composite.InheritedReversePropertyByAssociationName.Add(property.AssociationName, property);
                    }
                }
            }
        }

        private void CheckId(ISet<Guid> ids, string id, string name, string key)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                this.HasErrors = true;
                Logger.Error($"{name} has a non GUID {key}: {id}");
            }

            this.CheckId(ids, idGuid, name, key);
        }

        private void CheckId(ISet<Guid> ids, Guid id, string name, string key)
        {
            if (ids.Contains(id))
            {
                this.HasErrors = true;
                Logger.Error($"{name} has a duplicate {key}: {id}");
            }

            ids.Add(id);
        }
    }
}
