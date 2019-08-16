//------------------------------------------------------------------------------------------------- 
// <copyright file="MediaTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Tests
{
    using System.IO;

    using Allors;
    using Allors.Domain;

    using Xunit;

    public class MediaTest : ContentTests
    {
        [Fact]
        public void DefaultValues()
        {
            var media = new MediaBuilder(this.Session).Build();
            Assert.True(media.ExistUniqueId);
        }

        [Fact]
        public void BuilderWithDataUrl()
        {
            const string DataUri = @"data:image/jpeg;base64,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";
            var photo = new MediaBuilder(this.Session).WithInDataUri(DataUri).Build();

            this.Session.Derive(true);

            Assert.False(photo.ExistInDataUri);
            Assert.Equal("image/jpeg", photo.MediaContent.Type);
            Assert.NotNull(photo.MediaContent.Data);
        }

        [Fact]
        public void BuilderWithData()
        {
            var binary = new byte[] { 0, 1, 2, 3 };

            this.Session.Commit();

            var media = new MediaBuilder(this.Session).WithInData(binary).Build();

            this.Session.Derive(true);

            Assert.True(media.ExistMediaContent);
            Assert.Equal(media.MediaContent.Type, "application/octet-stream");
        }

        [Fact]
        public void BuilderWithPng()
        {
            var resource = GetResource("Domain.Tests.Resources.logo.png");

            byte[] content;
            using (var output = new MemoryStream())
            {
                resource?.CopyTo(output);
                content = output.ToArray();
            }

            var media = new MediaBuilder(this.Session).WithInData(content).Build();

            this.Session.Derive(true);

            Assert.True(media.ExistMediaContent);
            Assert.True(media.MediaContent.Data.Length > 0);
            Assert.Equal(media.MediaContent.Type, "image/png");
        }


        [Fact]
        public void BuilderWithPdfWithJpegExtension()
        {
            var resource = GetResource("Domain.Tests.Resources.PdfAs.jpg");

            byte[] content;
            using (var output = new MemoryStream())
            {
                resource?.CopyTo(output);
                content = output.ToArray();
            }

            var media = new MediaBuilder(this.Session).WithInData(content).Build();

            this.Session.Derive(true);

            Assert.True(media.ExistMediaContent);
            Assert.Equal("application/pdf", media.MediaContent.Type);
        }
    }
}
