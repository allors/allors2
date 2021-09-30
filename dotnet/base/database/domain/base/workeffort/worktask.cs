// <copyright file="WorkTask.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public partial class WorkTask
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
              {
                new TransitionalConfiguration(M.WorkTask, M.WorkTask.WorkEffortState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent()
                .Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.ResetPrintDocument();
        }

        public void BasePrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.TakenBy?.ExistLogoImage == true ?
                               this.TakenBy.LogoImage.MediaContent.Data :
                               singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                                 {
                                     { "Logo", logo },
                                 };

                if (this.ExistWorkEffortNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.WorkEffortNumber, BarcodeType.CODE_128, 320, 80, pure: true);
                    images["Barcode"] = barcode;
                }

                var model = new Print.WorkTaskModel.Model(this);
                this.RenderPrintDocument(this.TakenBy?.WorkTaskTemplate, model, images);

                this.PrintDocument.Media.InFileName = $"{this.WorkEffortNumber}.odt";
            }
        }

        // public void BaseDelete(DeletableDelete method)
        // {
        //    foreach (WorkEffortStatus workEffortStatus in this.WorkEffortStatuses)
        //    {
        //        workEffortStatus.Delete();
        //    }

        // foreach (WorkEffortAssignment workEffortAssignment in this.WorkEffortAssignmentsWhereAssignment)
        //    {
        //        workEffortAssignment.Delete();
        //    }
        // }
    }
}
