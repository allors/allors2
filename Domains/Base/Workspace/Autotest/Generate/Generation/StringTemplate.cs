// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringTemplate.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Development.Repository.Generation
{
    using System;
    using System.IO;

    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Misc;

    using Storage;
    using System.Xml;

    using Autotest;

    public class StringTemplate
    {
        private const string DirectiveKey = "directive";
        private const string GenerationKey = "generation";
        private const string InputKey = "input";
        private const string MenuKey = "menu";
        private const string ModelKey = "model";
        private const string OutputKey = "output";
        private const string TemplateConfiguration = "TemplateConfiguration";
        private const string TemplateId = "TemplateId";
        private const string TemplateKey = "template";
        private const string TemplateName = "TemplateName";
        private const string TemplateVersion = "TemplateVersion";
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

        public override string ToString()
        {
            return this.Name;
        }

        internal void Generate(Model model, DirectoryInfo outputDirectory, Log log)
        {
            var validation = model.Validate();
            if (validation.HasErrors)
            {
                log.Error(this, "Model has validation errors.");
                foreach (var error in validation.Errors)
                {
                    log.Error(this, error);
                }

                return;
            }

            try
            {
                TemplateGroup templateGroup = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

                templateGroup.ErrorManager = new ErrorManager(new LogAdapter(log));

                var configurationTemplate = templateGroup.GetInstanceOf(TemplateConfiguration);
                configurationTemplate.Add(ModelKey, model);

                var configurationXml = new XmlDocument();
                configurationXml.LoadXml(configurationTemplate.Render());

                var location = new Location(outputDirectory);
                foreach (XmlElement generation in configurationXml.DocumentElement.SelectNodes(GenerationKey))
                {
                    var templateName = generation.GetAttribute(TemplateKey);
                    var template = templateGroup.GetInstanceOf(templateName);
                    var output = generation.GetAttribute(OutputKey);

                    template.Add(ModelKey, model);

                    if (generation.HasAttribute(InputKey))
                    {
                        var input = generation.GetAttribute(InputKey);
                        switch (input)
                        {
                            case MenuKey:
                                template.Add(MenuKey, model.Menu);
                                break;

                            default:

                                var project = model.Project;

                                if (project.DirectiveById.TryGetValue(input, out var directive))
                                {
                                    template.Add(DirectiveKey, directive);
                                }

                                break;
                        }
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
            return template?.Render();
        }

        private class LogAdapter : ITemplateErrorListener
        {
            private readonly Log log;

            public LogAdapter(Log log)
            {
                this.log = log;
            }

            public void CompiletimeError(TemplateMessage msg)
            {
                this.log.Error(msg, msg.ToString());
            }

            public void InternalError(TemplateMessage msg)
            {
                this.log.Error(msg, msg.ToString());
            }

            public void IOError(TemplateMessage msg)
            {
                this.log.Error(msg, msg.ToString());
            }

            public void RuntimeError(TemplateMessage msg)
            {
                this.log.Error(msg, msg.ToString());
            }
        }
    }
}