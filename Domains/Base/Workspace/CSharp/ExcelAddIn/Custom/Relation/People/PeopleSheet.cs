namespace Allors.Excel.People
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using Allors.Excel;
    using Allors.Workspace.Meta;
    using Allors.Protocol.Remote;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;

    using Microsoft.Office.Interop.Excel;

    using Filter = Microsoft.Office.Interop.Excel.Filter;
    using ListObject = Microsoft.Office.Tools.Excel.ListObject;
    using Result = Allors.Workspace.Client.Result;
    using Sheets = Allors.Excel.Sheets;
    using Task = System.Threading.Tasks.Task;
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public class PeopleSheet : Sheet
    {
        private const string PeopleListObjectName = "PeopleListObject";
        private DataSet dataSet;
        private ListObject listObject;

        private Result result;

        public PeopleSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public Person[] People { get; private set; }

        public ListObject PeopleListObject
        {
            get
            {
                if (this.listObject == null)
                {
                    this.listObject = this.FindListObject(PeopleListObjectName);
                    if (this.listObject == null)
                    {
                        var cell = this.Worksheet.Range["$A$1:$B$1"];
                        this.listObject = this.Worksheet.Controls.AddListObject(cell, PeopleListObjectName);
                    }
                }

                return this.listObject;
            }
        }
        
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

            var people = this.People;
            if (people != null)
            {
                foreach (var person in people)
                {
                    var row = this.dataSet.People.NewPeopleRow();

                    row.Id = person.Id;
                    row.UserName = person.UserName;
                    row.FirstName = person.FirstName;
                    row.LastName = person.LastName;

                    this.dataSet.People.Rows.Add(row);
                }
            }

            this.PeopleListObject.SetDataBinding(this.dataSet, this.dataSet.People.TableName);

            // Headers
            var headers = this.PeopleListObject.HeaderRowRange;

            var index = 0;

            Range idHeader = headers.Cells[1, ++index];
            Range userNameHeader = headers.Cells[1, ++index];
            Range firstNameHeader = headers.Cells[1, ++index];
            Range lastNameHeader = headers.Cells[1, ++index];

            idHeader.Value = "Id";
            userNameHeader.Value = "User Name";
            firstNameHeader.Value = "First Name";
            lastNameHeader.Value = "Last Name";
        }

        private void ToWorkspace()
        {
            var listRows = this.PeopleListObject.ListRows;
            foreach (ListRow row in listRows)
            {
                var range = row.Range;
                var cells = range.Cells;

                var values = cells.Cast<Range>().Select(cell => cell.Value).ToArray();

                var id = Convert.ToInt64(values[0].ToString());
                var person = (Person)this.Context.Session.Get(id);
                person.FirstName = values[2];
                person.LastName = values[3];
            }
        }

        private async Task Load()
        {
            var pull = new Pull { Extent = new Workspace.Data.Filter(M.Person.ObjectType) };
            this.result = await this.Load(pull);
            this.People = this.result.GetCollection<Person>("People");
        }
    }
}
