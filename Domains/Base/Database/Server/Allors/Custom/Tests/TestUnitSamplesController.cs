using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    using System;

    using Allors.Domain;
    using Allors.Server;

    public class TestUnitSamplesParams
    {
        public int Step {
            get;
            set;
        }
    }

    public class TestUnitSamplesController : PullController
    {
        public TestUnitSamplesController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public ActionResult Pull([FromBody] TestUnitSamplesParams @params)
        {
            try
            {
                var unitSample = new UnitSamples(this.AllorsSession).Extent().First;
                if (unitSample == null)
                {
                    unitSample = new UnitSampleBuilder(this.AllorsSession).Build();
                    this.AllorsSession.Commit();
                }

                var responseBuilder = new PullResponseBuilder(this.AllorsUser);

                switch (@params.Step)
                {
                    case 0:
                        unitSample.RemoveAllorsBinary();
                        unitSample.RemoveAllorsBoolean();
                        unitSample.RemoveAllorsDateTime();
                        unitSample.RemoveAllorsDecimal();
                        unitSample.RemoveAllorsDouble();
                        unitSample.RemoveAllorsInteger();
                        unitSample.RemoveAllorsString();
                        unitSample.RemoveAllorsUnique();

                        break;

                    case 1:
                        unitSample.AllorsBinary = new byte[] { 1, 2, 3 };
                        unitSample.AllorsBoolean = true;
                        unitSample.AllorsDateTime = new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc);
                        unitSample.AllorsDecimal = 12.34m;
                        unitSample.AllorsDouble = 123d;
                        unitSample.AllorsInteger = 1000;
                        unitSample.AllorsString = "a string";
                        unitSample.AllorsUnique = new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF");

                        break;
                }

                this.AllorsSession.Commit();

                responseBuilder.AddObject("unitSample", unitSample);

                return this.Ok(responseBuilder.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}