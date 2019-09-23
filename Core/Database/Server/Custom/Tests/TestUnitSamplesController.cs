// <copyright file="TestUnitSamplesController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Allors.Domain;
    using Server;
    using Allors.Services;

    using Microsoft.AspNetCore.Mvc;

    public class TestUnitSamplesController : Controller
    {
        public TestUnitSamplesController(ISessionService sessionService, ITreeService treeService)
        {
            this.Session = sessionService.Session;
            this.TreeService = treeService;
        }

        private ISession Session { get; }

        public ITreeService TreeService { get; }


        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] TestUnitSamplesParams @params)
        {
            try
            {
                var unitSample = new UnitSamples(this.Session).Extent().First;
                if (unitSample == null)
                {
                    unitSample = new UnitSampleBuilder(this.Session).Build();
                    this.Session.Commit();
                }

                var responseBuilder = new PullResponseBuilder(this.Session.GetUser(), this.TreeService);

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

                this.Session.Commit();

                responseBuilder.AddObject("unitSample", unitSample);

                return this.Ok(responseBuilder.Build());
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        public class TestUnitSamplesParams
        {
            public int Step
            {
                get;
                set;
            }
        }
    }
}
