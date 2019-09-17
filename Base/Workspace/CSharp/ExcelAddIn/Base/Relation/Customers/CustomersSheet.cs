// <copyright file="CustomersSheet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel.Customers
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Protocol.Remote;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using ExcelAddIn.Base.Extensions;
    using ExcelAddIn.Base.Relation.Customers;
    using Microsoft.Office.Interop.Excel;
    using ListObject = Microsoft.Office.Tools.Excel.ListObject;
    using Result = Allors.Workspace.Result;
    using Sheets = Allors.Excel.Sheets;
    using Task = System.Threading.Tasks.Task;
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public class CustomersSheet : Sheet
    {
        private const string CustomersListObjectName = "CustomersListObject";
        private DataSet dataSet;
        private ListObject listObject;

        private Result result;

        public CustomersSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public Organisation[] Customers { get; private set; }

        public ListObject CustomersListObject
        {
            get
            {
                if (this.listObject == null)
                {
                    this.listObject = this.FindListObject(CustomersListObjectName);
                    if (this.listObject == null)
                    {
                        var cell = this.Worksheet.Range["$A$1:$A$1"];
                        this.listObject = this.Worksheet.Controls.AddListObject(cell, CustomersListObjectName);
                    }
                }

                return this.listObject;
            }
        }

        private INode[] PartyContactMechanismsTree
            => new[]
            {
                new Node(M.PartyContactMechanism.ContactPurposes),
                new Node(M.PartyContactMechanism.ContactMechanism, this.ContactMechanismTree),
            };

        private INode[] CurrentOrganisationContactRelationshipTree
            => new[]
            {
                new Node(M.OrganisationContactRelationship.Organisation),
                new Node(M.OrganisationContactRelationship.Contact, this.ContactTree),
            };

        private INode[] ContactTree
            => new[]
            {
                new Node(M.Person.Salutation),
                new Node(M.Person.GeneralCorrespondence, this.GeneralCorrespondenceTree),
            };

        private INode[] ContactMechanismTree
            => new[]
            {
                new Node(M.PostalAddress.Country),
            };

        private INode[] GeneralCorrespondenceTree
            => new[]
            {
                new Node(M.ContactMechanism.ContactMechanismType),
            };

        public override async Task Refresh()
        {
            await this.Load();

            this.ToListObject();

            this.Sheets.Mediator.OnStateChanged();
        }

        protected override async Task OnSaving()
        {
            await this.Load();

            this.ToWorkspace();
        }

        protected override void OnSaved(ErrorResponse response)
        {
            if (!response.HasErrors)
            {
                MessageBox.Show(@"Successfully saved");
            }
        }

        private void ToListObject()
        {
            this.dataSet = new DataSet();

            this.dataSet.Customers.SetColumnsOrder(
                this.dataSet.Customers.NameColumn.ColumnName,
                this.dataSet.Customers.ContactNameColumn.ColumnName,
                this.dataSet.Customers.StreetColumn.ColumnName,
                this.dataSet.Customers.PlaceColumn.ColumnName,
                this.dataSet.Customers.CountryColumn.ColumnName,
                this.dataSet.Customers.PostalCodeColumn.ColumnName,
                this.dataSet.Customers.TaxNumberColumn.ColumnName
            );

            // Threat everything as text
            this.CustomersListObject.Range.NumberFormat = "@";

            foreach (var customer in this.Customers)
            {
                var row = this.dataSet.Customers.NewCustomersRow();

                row.Name = customer.Name;
                var contactMechanism = customer.PartyContactMechanisms
                    .FirstOrDefault(v => v.ContactPurposes.Any(w => string.Equals(w.Name, "General correspondence address", StringComparison.OrdinalIgnoreCase)))?.ContactMechanism;

                if (contactMechanism is PostalAddress)
                {
                    var postalAddress = contactMechanism as PostalAddress;
                    row.Street = postalAddress.Address1;
                    row.Place = postalAddress.Locality;
                    row.Country = postalAddress.Country?.Name;
                    row.PostalCode = postalAddress.PostalCode;
                }

                var contacts = customer.CurrentOrganisationContactRelationships.Select(v => v.Contact);
                row.ContactName = string.Join("\n", contacts.Select(v => $"{v?.Salutation?.Name} {v?.PartyName}"));

                row.TaxNumber = customer.TaxNumber;

                this.dataSet.Customers.Rows.Add(row);
            }

            this.CustomersListObject.SetDataBinding(this.dataSet, this.dataSet.Customers.TableName);

            // Headers
            var index = -1;
            var headers = this.CustomersListObject.HeaderRowRange;

            var data = new object[headers.Rows.Count, headers.Columns.Count];
            data[0, ++index] = "Bedrijfsnaam";
            data[0, ++index] = "Contact";
            data[0, ++index] = "Adres";
            data[0, ++index] = "Plaats";
            data[0, ++index] = "Land";
            data[0, ++index] = "PostCode";
            data[0, ++index] = "BTW nr Klant";

            headers.Value2 = data;
            headers.Style = "headerStyle";
            headers.EntireColumn.AutoFit();
        }

        private void ToWorkspace()
        {
            var listRows = this.CustomersListObject.ListRows;
            foreach (ListRow row in listRows)
            {
                var range = row.Range;
                var cells = range.Cells;

                var values = cells.Cast<Range>().Select(cell => cell.Value).ToArray();
            }
        }

        private async Task Load()
        {
            var pull = new Pull
            {
                Extent = new Workspace.Data.Filter(M.Organisation.ObjectType),

                Results = new[]
                {
                    new Workspace.Data.Result()
                    {
                        Fetch = new Fetch()
                        {
                            Include = new []
                                {
                                    new Node(M.Organisation.PartyContactMechanisms, this.PartyContactMechanismsTree),
                                    new Node(M.Organisation.CurrentOrganisationContactRelationships, this.CurrentOrganisationContactRelationshipTree),
                                }
                        },
                    },
                },
            };
            this.result = await this.Load(pull);
            this.Customers = this.result.GetCollection<Organisation>("Organisations");
        }
    }
}
