// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTask.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent()
                .Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.ResetPrintDocument();
        }

        public void AppsPrint(PrintablePrint method)
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
                    var barcode = barcodeService.Generate(this.WorkEffortNumber, BarcodeType.CODE_128, 320, 80);
                    images["Barcode"] = barcode;
                }

                var model = new Print.WorkTaskModel.Model(this);
                this.RenderPrintDocument(this.TakenBy?.WorkTaskTemplate, model, images);

                this.PrintDocument.Media.FileName = $"{this.WorkEffortNumber}.odt";
            }
        }

        //public void AppsDelete(DeletableDelete method)
        //{
        //    foreach (WorkEffortStatus workEffortStatus in this.WorkEffortStatuses)
        //    {
        //        workEffortStatus.Delete();
        //    }

        //    foreach (WorkEffortAssignment workEffortAssignment in this.WorkEffortAssignmentsWhereAssignment)
        //    {
        //        workEffortAssignment.Delete();
        //    }
        //}
    }
}
