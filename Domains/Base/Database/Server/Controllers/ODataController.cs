namespace Identity.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using Allors.Meta;
    using Allors.Server.OData;
    using Allors.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OData;
    using Microsoft.OData.Edm;

    using MimeKit.Encodings;

    using Newtonsoft.Json;

    [AllowAnonymous]
    public class ODataController : Controller
    {
        private readonly EdmModel model;

        private readonly ISessionService session;

        public ODataController(ISessionService sessionService)
        {
            this.session = sessionService;

            // Build EdmModel
            this.model = new EdmModel();

            var metaPopulation = (MetaPopulation)this.session.Session.Database.MetaPopulation;
            foreach (var @class in metaPopulation.WorkspaceClasses)
            {
                var entityType = this.model.AddEntityType("Domain", @class.Name);
                foreach (var roleType in @class.WorkspaceRoleTypes.Where(v => v.ObjectType.IsUnit))
                {
                    EdmPrimitiveTypeKind kind;

                    var unitType = (Unit)roleType.ObjectType;
                    switch (unitType.UnitTag)
                    {
                        case UnitTags.Binary:
                            kind = EdmPrimitiveTypeKind.Binary;
                            break;

                        case UnitTags.Boolean:
                            kind = EdmPrimitiveTypeKind.Boolean;
                            break;

                        case UnitTags.DateTime:
                            kind = EdmPrimitiveTypeKind.Date;
                            break;

                        case UnitTags.Decimal:
                            kind = EdmPrimitiveTypeKind.Decimal;
                            break;

                        case UnitTags.Float:
                            kind = EdmPrimitiveTypeKind.Double;
                            break;

                        case UnitTags.Integer:
                            kind = EdmPrimitiveTypeKind.Int32;
                            break;

                        case UnitTags.String:
                            kind = EdmPrimitiveTypeKind.String;
                            break;

                        case UnitTags.Unique:
                            kind = EdmPrimitiveTypeKind.Guid;
                            break;

                        default:
                            throw new NotSupportedException(unitType.Name);
                    }

                    entityType.AddStructuralProperty(roleType.PropertyName, kind);
                }
            }
        }

        [HttpGet]
        public IActionResult Get(string request)
        {
            var stream = new MemoryStream();
            var message = new InMemoryMessage { Stream = stream };

            var settings = new ODataMessageWriterSettings
            {
                ODataUri = new ODataUri
                {
                    ServiceRoot = new Uri(this.Request.Scheme + "://" + this.Request.Host + "/" + "odata")
                }
            };

            var writer = new ODataMessageWriter((IODataResponseMessage)message, settings, this.model);

            // $metadata
            if (request == "$metadata")
            {
                writer.WriteMetadataDocument();

                var output = Encoding.UTF8.GetString(stream.ToArray());
                var xml = new XmlDocument();
                xml.LoadXml(output);

                return this.Ok(xml);
            }

            // service document
            if (request == null)
            {
                var serviceDocument = new ODataServiceDocument();

                var entitySets = new List<ODataEntitySetInfo>();
                var metaPopulation = (MetaPopulation)this.session.Session.Database.MetaPopulation;
                foreach (var @class in metaPopulation.WorkspaceClasses)
                {
                    var entitySet = new ODataEntitySetInfo
                                        {
                                            Name = @class.Name,
                                            Title = @class.Name,
                                            Url = new Uri(@class.PluralName, UriKind.Relative)
                                        };
                    entitySets.Add(entitySet);
                }

                serviceDocument.EntitySets = entitySets;
                writer.WriteServiceDocument(serviceDocument);

                var output = Encoding.UTF8.GetString(stream.ToArray());
                var json = JsonConvert.DeserializeObject(output);

                return this.Ok(json);
            }

            return this.NotFound();
        }

        [HttpPost]
        public IActionResult Post()
        {
            return this.Ok("OData Post: " + this.Request.Path);
        }

        [HttpPut]
        public IActionResult Put()
        {
            return this.Ok("OData Put: " + this.Request.Path);
        }

        [HttpPatch]
        public IActionResult Patch()
        {
            return this.Ok("OData Patch: " + this.Request.Path);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return this.Ok("OData Delete: " + this.Request.Path);
        }
    }
}
