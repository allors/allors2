// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenDocumentTemplateContentTests.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba
// This file is licenses under the Lesser General Public Licence v3 (LGPL)
// The LGPL License is included in the file LICENSE.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allos.Document.OpenDocument.Tests
{
    using Allors.Document.OpenDocument;

    using Xunit;

    public class OpenDocumentTemplateContentTests
    {
        [Fact]
        public void ToStringTemplate()
        {
            var input = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<office:document-content xmlns:office=""urn:oasis:names:tc:opendocument:xmlns:office:1.0"" xmlns:text=""urn:oasis:names:tc:opendocument:xmlns:text:1.0"">
  <office:body>
    <office:text>
        <text:placeholder text:placeholder-type=""text"">&lt;@if person.FirstName&gt;</text:placeholder>
        <text:placeholder text:placeholder-type=""text"">&lt;$person.FirstName&gt;</text:placeholder>
        <text:placeholder text:placeholder-type=""text"">&lt;@end&gt;</text:placeholder>

        <text:placeholder text:placeholder-type=""text"">&lt;@for p people&gt;</text:placeholder>
        <text:placeholder text:placeholder-type=""text"">&lt;$p.FirstName&gt;</text:placeholder>
        <text:placeholder text:placeholder-type=""text"">&lt;@end&gt;</text:placeholder>
    </office:text>
  </office:body>
</office:document-content>
";

            var byteArray = System.Text.Encoding.UTF8.GetBytes(input);
            var document = new OpenDocumentTemplateContent(byteArray, '¬', '¬');

            var actual = document.ToStringTemplate();

            var expected = @" ::= <<
<?xml version=""1.0"" encoding=""UTF-8""?>
<office:document-content xmlns:office=""urn:oasis:names:tc:opendocument:xmlns:office:1.0"" xmlns:text=""urn:oasis:names:tc:opendocument:xmlns:text:1.0"">
<office:body>
<office:text>
¬if(person.FirstName)¬
¬person.FirstName¬
¬endif¬
¬people:{p|¬for1(p)¬}¬
</office:text>
</office:body>
</office:document-content>
>>

for1(p) ::= <<
¬p.FirstName¬
>>";

            Assert.Equal(expected, actual);
        }
    }
}
