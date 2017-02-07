namespace Allors.Tools.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Allors.Meta;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using NLog;

    public class Repository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Inflector.Inflector inflector;

        public Repository(Project project)
        {
            this.inflector = new Inflector.Inflector(new CultureInfo("en"));
            
            this.AssemblyByName = new Dictionary<string, Assembly>();
            this.UnitBySingularName = new Dictionary<string, Unit>();
            this.InterfaceBySingularName = new Dictionary<string, Interface>();
            this.ClassBySingularName = new Dictionary<string, Class>();
            this.CompositeByName = new Dictionary<string, Composite>();
            this.TypeBySingularName = new Dictionary<string, Type>();

            var projectInfo = new ProjectInfo(project);

            this.CreateUnits();

            this.CreateAssemblies(projectInfo);
            this.CreateAssemblyExtensions(projectInfo);

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

                foreach (var property in composite.DefinedProperties)
                {
                    this.CheckId(ids, property.Id, $"{composite.SingularName}.{property.RoleName}", "id");
                    this.CheckId(ids, property.AssociationId, $"{composite.SingularName}.{property.RoleName}", "association id");
                    this.CheckId(ids, property.RoleId, $"{composite.SingularName}.{property.RoleName}", "role id");
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

        public IEnumerable<Assembly> Assemblies => this.AssemblyByName.Values;

        public IEnumerable<Unit> Units => this.UnitBySingularName.Values;

        public IEnumerable<Interface> Interfaces => this.InterfaceBySingularName.Values;

        public IEnumerable<Class> Classes => this.ClassBySingularName.Values;

        public IEnumerable<Type> Types => this.Composites.Cast<Type>().Union(this.Units);

        public IEnumerable<Composite> Composites => this.ClassBySingularName.Values.Cast<Composite>().Union(this.InterfaceBySingularName.Values);

        public Dictionary<string, Assembly> AssemblyByName { get; }

        public Dictionary<string, Unit> UnitBySingularName { get; }

        public Dictionary<string, Interface> InterfaceBySingularName { get; }

        public Dictionary<string, Class> ClassBySingularName { get; }

        public Dictionary<string, Type> TypeBySingularName { get; }

        public Dictionary<string, Composite> CompositeByName { get; }

        public Assembly[] SortedAssemblies
        {
            get
            {
                var assemblies = this.Assemblies.ToList();
                assemblies.Sort((x, y) => x.Bases.Contains(y) ? 1 : -1);
                return assemblies.ToArray();
            }
        }
        
        public bool HasErrors { get; set; }

        private void CreateUnits()
        {
            var binary = new Unit(UnitNames.Binary, UnitIds.Binary);
            this.UnitBySingularName.Add(binary.SingularName, binary);
            this.TypeBySingularName.Add(binary.SingularName, binary);

            var boolean = new Unit(UnitNames.Boolean, UnitIds.Boolean);
            this.UnitBySingularName.Add(boolean.SingularName, boolean);
            this.TypeBySingularName.Add(boolean.SingularName, boolean);

            var datetime = new Unit(UnitNames.DateTime, UnitIds.DateTime);
            this.UnitBySingularName.Add(datetime.SingularName, datetime);
            this.TypeBySingularName.Add(datetime.SingularName, datetime);

            var @decimal = new Unit(UnitNames.Decimal, UnitIds.Decimal);
            this.UnitBySingularName.Add(@decimal.SingularName, @decimal);
            this.TypeBySingularName.Add(@decimal.SingularName, @decimal);

            var @float = new Unit(UnitNames.Float, UnitIds.Float);
            this.UnitBySingularName.Add(@float.SingularName, @float);
            this.TypeBySingularName.Add(@float.SingularName, @float);

            var integer = new Unit(UnitNames.Integer, UnitIds.Integer);
            this.UnitBySingularName.Add(integer.SingularName, integer);
            this.TypeBySingularName.Add(integer.SingularName, integer);

            var @string = new Unit(UnitNames.String, UnitIds.String);
            this.UnitBySingularName.Add(@string.SingularName, @string);
            this.TypeBySingularName.Add(@string.SingularName, @string);

            var unique = new Unit(UnitNames.Unique, UnitIds.Unique);
            this.UnitBySingularName.Add(unique.SingularName, unique);
            this.TypeBySingularName.Add(unique.SingularName, unique);
        }

        private void CreateAssemblies(ProjectInfo projectInfo)
        {
            foreach (var syntaxTree in projectInfo.DocumentBySyntaxTree.Keys)
            {
                var document = projectInfo.DocumentBySyntaxTree[syntaxTree];
                if (document.Folders.Count >= 1)
                {
                    var attributeLists =
                        syntaxTree.GetRoot()
                            .DescendantNodes()
                            .OfType<AttributeListSyntax>()
                            .Where(x => "assembly".Equals(x.Target?.Identifier.Text))
                            .ToArray();

                    var assemblyName = document.Folders[0];
                    if (attributeLists.Length > 0 && !this.AssemblyByName.ContainsKey(assemblyName))
                    {
                        var semanticModel = projectInfo.SemanticModelBySyntaxTree[syntaxTree];
                        foreach (var attributeList in attributeLists)
                        {
                            if (attributeList.Attributes.Count > 0)
                            {
                                var attribute = attributeList.Attributes[0];
                                var attributeType = semanticModel.GetSymbolInfo(attribute).Symbol.ContainingType;

                                if (attributeType.Equals(projectInfo.ExtendAttributeType))
                                {
                                    this.AssemblyByName.Add(assemblyName, new Assembly(assemblyName));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateAssemblyExtensions(ProjectInfo projectInfo)
        {
            foreach (var syntaxTree in projectInfo.DocumentBySyntaxTree.Keys)
            {
                var document = projectInfo.DocumentBySyntaxTree[syntaxTree];
                if (document.Folders.Count >= 1)
                {
                    var attributeLists =
                        syntaxTree.GetRoot()
                            .DescendantNodes()
                            .OfType<AttributeListSyntax>()
                            .Where(x => "assembly".Equals(x.Target?.Identifier.Text))
                            .ToArray();

                    var assemblyName = document.Folders[0];
                    if (attributeLists.Length > 0)
                    {
                        var semanticModel = projectInfo.SemanticModelBySyntaxTree[syntaxTree];
                        foreach (var attributeList in attributeLists)
                        {
                            if (attributeList.Attributes.Count > 0)
                            {
                                var attribute = attributeList.Attributes[0];
                                var symbol = semanticModel.GetSymbolInfo(attribute).Symbol;
                                var attributeType = symbol.ContainingType;

                                if (attributeType.Equals(projectInfo.DomainAttributeType))
                                {
                                    var idString = attribute.ArgumentList.Arguments[0].Expression.GetFirstToken().Value.ToString();
                                    var id = Guid.Parse(idString);

                                    if (!string.IsNullOrWhiteSpace(idString))
                                    {
                                        var assembly = this.AssemblyByName[assemblyName];
                                        assembly.Id = id;
                                    }
                                }

                                if (attributeType.Equals(projectInfo.ExtendAttributeType))
                                {
                                    var baseDomainName = attribute.ArgumentList.Arguments[0].Expression.GetFirstToken().Value.ToString();

                                    if (!string.IsNullOrWhiteSpace(baseDomainName))
                                    {
                                        var assembly = this.AssemblyByName[assemblyName];

                                        Assembly baseAssembly;
                                        if (!this.AssemblyByName.TryGetValue(baseDomainName, out baseAssembly))
                                        {
                                            Logger.Error("Can not find base domain " + baseDomainName);
                                        }

                                        assembly.Extend(baseAssembly);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateTypes(ProjectInfo projectInfo)
        {
            foreach (var syntaxTree in projectInfo.DocumentBySyntaxTree.Keys)
            {
                var document = projectInfo.DocumentBySyntaxTree[syntaxTree];

                if (document.Folders.Count >= 1)
                {
                    var semanticModel = projectInfo.SemanticModelBySyntaxTree[syntaxTree];

                    var assemblyName = document.Folders[0];
                    if (this.AssemblyByName.ContainsKey(assemblyName))
                    {
                        var assembly = this.AssemblyByName[assemblyName];

                        var root = syntaxTree.GetRoot();
                        
                        var interfaceDeclaration = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().SingleOrDefault();
                        if (interfaceDeclaration != null)
                        {
                            var symbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                            var interfaceSingularName = symbol.Name;

                            var partialInterface = new PartialInterface(interfaceSingularName);
                            assembly.PartialInterfaceByName.Add(interfaceSingularName, partialInterface);
                            assembly.PartialTypeBySingularName.Add(interfaceSingularName, partialInterface);

                            Interface @interface;
                            if (!this.InterfaceBySingularName.TryGetValue(interfaceSingularName, out @interface))
                            {
                                @interface = new Interface(this.inflector, interfaceSingularName);
                                this.InterfaceBySingularName.Add(interfaceSingularName,  @interface);
                                this.CompositeByName.Add(interfaceSingularName, @interface);
                                this.TypeBySingularName.Add(interfaceSingularName, @interface);
                            }

                            @interface.PartialByAssemblyName.Add(assemblyName, partialInterface);
                            var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                            @interface.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                        }

                        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().SingleOrDefault();
                        if (classDeclaration != null)
                        {
                            var symbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                            var classSingularName = symbol.Name;

                            var partialClass = new PartialClass(classSingularName);
                            assembly.PartialClassBySingularName.Add(classSingularName, partialClass);
                            assembly.PartialTypeBySingularName.Add(classSingularName, partialClass);
                            
                            Class @class;
                            if (!this.ClassBySingularName.TryGetValue(classSingularName, out @class))
                            {
                                @class = new Class(this.inflector, classSingularName);
                                this.ClassBySingularName.Add(classSingularName, @class);
                                this.CompositeByName.Add(classSingularName, @class);
                                this.TypeBySingularName.Add(classSingularName, @class);
                            }

                            @class.PartialByAssemblyName.Add(assemblyName, partialClass);

                            var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                            @class.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                        }
                    }
                }
            }
        }

        private void CreateHierarchy(ProjectInfo projectInfo)
        {
            var definedTypeBySingularName = projectInfo.Assembly.DefinedTypes.Where(v => "Allors.Repository.Domain".Equals(v.Namespace)).ToDictionary(v => v.Name);

            foreach (var composite in this.CompositeByName.Values)
            {
                var definedType = definedTypeBySingularName[composite.SingularName];

                var allInterfaces = definedType.GetInterfaces();
                var directInterfaces = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var definedImplementedInterface in directInterfaces)
                {
                    Interface implementedInterface;
                    if (this.InterfaceBySingularName.TryGetValue(definedImplementedInterface.Name, out implementedInterface))
                    {
                        composite.ImplementedInterfaces.Add(implementedInterface);
                    }
                    else
                    {
                        throw new Exception("Can not find implemented interface " + definedImplementedInterface.Name + " on " + composite.SingularName);
                    }
                }
            }
        }

        private void CreateMembers(ProjectInfo projectInfo)
        {
            foreach (var syntaxTree in projectInfo.DocumentBySyntaxTree.Keys)
            {
                var document = projectInfo.DocumentBySyntaxTree[syntaxTree];
                if (document.Folders.Count >= 1)
                {
                    var semanticModel = projectInfo.SemanticModelBySyntaxTree[syntaxTree];

                    var assemblyName = document.Folders[0];
                    if (this.AssemblyByName.ContainsKey(assemblyName))
                    {
                        var assembly = this.AssemblyByName[assemblyName];

                        var root = syntaxTree.GetRoot();

                        var typeDeclaration = root.DescendantNodes().SingleOrDefault(v => v is InterfaceDeclarationSyntax || v is ClassDeclarationSyntax);
                        if (typeDeclaration != null)
                        {
                            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                            var typeName = typeSymbol.Name;
                            var partialType = assembly.PartialTypeBySingularName[typeName];
                            var composite = this.CompositeByName[typeName];

                            var propertyDeclarations = typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();
                            foreach (var propertyDeclaration in propertyDeclarations)
                            {
                                var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration);
                                var propertyRoleName = propertySymbol.Name;

                                var xmlDocString = propertySymbol.GetDocumentationCommentXml(null, true);
                                var xmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

                                var property = new Property(this.inflector, composite, propertyRoleName)
                                                   {
                                                       XmlDoc = xmlDoc
                                };

                                partialType.PropertyByName.Add(propertyRoleName, property);
                                composite.PropertyByRoleName.Add(propertyRoleName, property);
                            }

                            var methodDeclarations = typeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
                            foreach (var methodDeclaration in methodDeclarations)
                            {
                                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
                                var methodName = methodSymbol.Name;

                                var xmlDocString = methodSymbol.GetDocumentationCommentXml(null, true);
                                var xmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

                                var method = new Method(composite, methodName)
                                                 {
                                                     XmlDoc = xmlDoc
                                };

                                partialType.MethodByName.Add(methodName, method);
                                composite.MethodByName.Add(methodName, method);
                            }
                        }
                    }
                }
            }
        }

        private void FromReflection(ProjectInfo projectInfo)
        {
            var declaredTypeBySingularName = projectInfo.Assembly.DefinedTypes.Where(v => "Allors.Repository.Domain".Equals(v.Namespace)).ToDictionary(v => v.Name);

            foreach (var composite in this.CompositeByName.Values)
            {
                var reflectedType = declaredTypeBySingularName[composite.SingularName];

                // Type attributes
                {
                    var typeAttributesByTypeName = reflectedType.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    foreach (var group in typeAttributesByTypeName)
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
                    }

                    var propertyAttributesByTypeName =
                        reflectedProperty.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

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
                }

                foreach (var method in composite.Methods)
                {
                    var reflectedMethod = reflectedType.GetMethod(method.Name);
                    var methodAttributesByType = reflectedMethod.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    foreach (var group in methodAttributesByType)
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

        private void CheckId(ISet<Guid> ids, string idAsString, string name, string key)
        {
            if (string.IsNullOrEmpty(idAsString))
            {
                this.HasErrors = true;
                Logger.Error($"{name} has a missing {key}");
            }
            else
            {
                Guid id;
                if (!Guid.TryParse(idAsString, out id))
                {
                    this.HasErrors = true;
                    Logger.Error($"{name} has an illegal {key}: {idAsString}");
                }
                else
                {
                    if (ids.Contains(id))
                    {
                        this.HasErrors = true;
                        Logger.Error($"{name} has a duplicate {key}: {idAsString}");
                    }

                    ids.Add(id);
                }
            }
        }
    }
}
