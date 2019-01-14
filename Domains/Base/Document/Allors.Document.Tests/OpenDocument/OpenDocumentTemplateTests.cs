// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenDocumentTemplateTests.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allos.Document.OpenDocument.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Allors.Document;
    using Allors.Document.OpenDocument;

    using Xunit;

    public class OpenDocumentTemplateTests
    {
        public class Model
        {
            public class ModelPerson
            {
                public string FirstName { get; set; }
            }

            public ModelPerson Person { get; set; }

            public ModelPerson[] People { get; set; }
        }

        [Fact]
        public void Render()
        {
            var model = new Model
            {
                Person = new Model.ModelPerson { FirstName = "Jane" },
                People = new[]
                         {
                             new Model.ModelPerson { FirstName = "John" },
                             new Model.ModelPerson { FirstName = "Jenny" },
                         }
            };

            var document = this.GetResource("EmbeddedTemplate.odt");
            var template = new OpenDocumentTemplate<Model>(document);
            var images = new Dictionary<string, byte[]>
                             {
                                 { "logo", this.GetResource("logo.png") },
                                 { "logo2", this.GetResource("logo.png") }
                             };
            var result = template.Render(model, images);

            File.WriteAllBytes(@"C:\temp\generated.odt", result);
        }


        [Fact]
        public void Rerender()
        {
            var model = new Model
                            {
                                Person = new Model.ModelPerson { FirstName = "Jane" },
                                People = new[]
                                             {
                                                 new Model.ModelPerson { FirstName = "John" },
                                                 new Model.ModelPerson { FirstName = "Jenny" },
                                             }
                            };

            var document = this.GetResource("EmbeddedTemplate.odt");
            var template = new OpenDocumentTemplate<Model>(document);
            var images = new Dictionary<string, byte[]>
                             {
                                 { "logo", this.GetResource("logo.png") },
                                 { "logo2", this.GetResource("logo.png") }
                             };

            // warmup
            var result = template.Render(model, images);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            result = template.Render(model, images);

            watch.Stop();
            var run1 = watch.ElapsedMilliseconds;

            watch = System.Diagnostics.Stopwatch.StartNew();

            result = template.Render(model, images);

            watch.Stop();
            var run2 = watch.ElapsedMilliseconds;

            Assert.InRange(run2, run1 * 0.8, run1 * 1.2);

            File.WriteAllBytes(@"C:\temp\generated.odt", result);
        }

        private byte[] GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;

            var resourceName = assembly.GetManifestResourceNames().First(v => v.Contains(name));
            var resource = assembly.GetManifestResourceStream(resourceName);

            using (var output = new MemoryStream())
            {
                resource?.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}
