// <copyright file="HomeController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Identity.Controllers
{
    using System.Diagnostics;

    using Allors.Services;

    using Identity.Models;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public HomeController(ISessionService sessionService)
        {
        }

        public IActionResult Index() => this.View();

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
