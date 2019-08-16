
// <copyright file="StringTemplate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>

namespace Allors.Development.Repository.Generation
{
    using System;
    using System.IO;

    using Allors.Meta;

    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Misc;

    using Storage;
    using System.Xml;

    public class StringTemplate
    {
        private const string TemplateId = "TemplateId";
        private const string TemplateName = "TemplateName";
        private const string TemplateVersion = "TemplateVersion";
        private const string TemplateConfiguration = "TemplateConfiguration";

        private const string TemplateKey = "template";
        private const string MetaKey = "meta";
        private const string InputKey = "input";
        private const string OutputKey = "output";
        private const string GenerationKey = "generation";
        private const string InheritanceKey = "inheritance";
        private const string ObjectTypeKey = "objectType";
        private const string RelationTypeKey = "relationType";
        private const string MethodTypeKey = "methodType";

        private readonly FileInfo fileInfo;

        internal StringTemplate(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;

            this.fileInfo.Refresh();
            if (!this.fileInfo.Exists)
            {
                throw new Exception("Template file not found: " + fileInfo.FullName);
            }

            TemplateGroup templateGroup = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

            this.Id = Render(templateGroup, TemplateId) != null ? new Guid(Render(templateGroup, TemplateId)) : Guid.Empty;
            this.Name = Render(templateGroup, TemplateName);
            this.Version = Render(templateGroup, TemplateVersion);

            if (this.Id == Guid.Empty)
            {
                throw new Exception("Template has no id");
            }
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Version { get; }

        public override string ToString() => this.Name;

        internal void Generate(MetaPopulation metaPopulation, DirectoryInfo outputDirectory, Log log)
        {
            var validation = metaPopulation.Validate();
            if (validation.ContainsErrors)
            {
                log.Error(this, "Meta population has validation errors.");
                foreach (var error in validation.Errors)
                {
                    log.Error(this, error.Message);
                }

                return;
            }

            try
            {
                TemplateGroup templateGroup = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

                templateGroup.ErrorManager = new ErrorManager(new LogAdapter(log));

                var configurationTemplate = templateGroup.GetInstanceOf(TemplateConfiguration);
                configurationTemplate.Add(MetaKey, metaPopulation);

                var configurationXml = new XmlDocument();
                configurationXml.LoadXml(configurationTemplate.Render());

                var location = new Location(outputDirectory);
                foreach (XmlElement generation in configurationXml.DocumentElement.SelectNodes(GenerationKey))
                {
                    var templateName = generation.GetAttribute(TemplateKey);
                    var template = templateGroup.GetInstanceOf(templateName);
                    var output = generation.GetAttribute(OutputKey);

                    template.Add(MetaKey, metaPopulation);

                    if (generation.HasAttribute(InputKey))
                    {
                        var input = new Guid(generation.GetAttribute(InputKey));
                        var objectType = metaPopulation.Find(input) as IObjectType;
                        if (objectType != null)
                        {
                            template.Add(ObjectTypeKey, objectType);
                        }
                        else
                        {
                            var relationType = metaPopulation.Find(input) as RelationType;
                            if (relationType != null)
                            {
                                template.Add(RelationTypeKey, relationType);
                            }
                            else
                            {
                                var inheritance = metaPopulation.Find(input) as Inheritance;
                                if (inheritance != null)
                                {
                                    template.Add(InheritanceKey, inheritance);
                                }
                                else
                                {
                                    var methodType = metaPopulation.Find(input) as MethodType;
                                    if (methodType != null)
                                    {
                                        template.Add(MethodTypeKey, methodType);
                                    }
                                    else
                                    {
                                        throw new ArgumentException(input + " was not found");
                                    }
                                }
                            }
                        }
                        //TODO: Super Domains
                    }

                    var result = template.Render();
                    location.Save(output, result);
                }
            }
            catch (Exception e)
            {
                log.Error(this, "Generation error : " + e.Message + "\n" + e.StackTrace);
            }
        }

        private static string Render(TemplateGroup templateGroup, string templateName)
        {
            var template = templateGroup.GetInstanceOf(templateName);
            if (template != null)
            {
                return template.Render();
            }

            return null;
        }

        private class LogAdapter : ITemplateErrorListener
        {
            private readonly Log log;

            public LogAdapter(Log log) => this.log = log;

            public void CompiletimeError(TemplateMessage msg) => this.log.Error(msg, msg.ToString());

            public void RuntimeError(TemplateMessage msg) => this.log.Error(msg, msg.ToString());

            public void IOError(TemplateMessage msg) => this.log.Error(msg, msg.ToString());

            public void InternalError(TemplateMessage msg) => this.log.Error(msg, msg.ToString());
        }
    }
}
