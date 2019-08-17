// <copyright file="OpenDocumentTemplateTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allos.Document.OpenDocument.Tests
{
    using System;
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

            public string[] Images { get; set; }
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
                         },
                Images = new[]
                             {
                                 "number1",
                                 "number2",
                                 "number3",
                             },
            };

            var document = this.GetResource("EmbeddedTemplate.odt");
            var template = new OpenDocumentTemplate<Model>(document);

            var images = new Dictionary<string, byte[]>
                             {
                                 { "logo", this.GetResource("logo.png") },
                                 { "logo2", this.GetResource("logo.png") },
                                 { "number1", this.GetResource("1.png") },
                                 { "number2", this.GetResource("2.png") },
                                 { "number3", this.GetResource("3.png") }
                             };

            var result = template.Render(model, images);

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllBytes(Path.Combine(desktopDir, "generated.odt"), result);
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
                                             },
                Images = new[]
                             {
                                 "number1",
                                 "number2",
                                 "number3",
                             },
            };

            var document = this.GetResource("EmbeddedTemplate.odt");
            var template = new OpenDocumentTemplate<Model>(document);
            var images = new Dictionary<string, byte[]>
                             {
                                 { "logo", this.GetResource("logo.png") },
                                 { "logo2", this.GetResource("logo.png") },
                                 { "number1", this.GetResource("1.png") },
                                 { "number2", this.GetResource("2.png") },
                                 { "number3", this.GetResource("3.png") }
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

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllBytes(Path.Combine(desktopDir, "generated.odt"), result);
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
