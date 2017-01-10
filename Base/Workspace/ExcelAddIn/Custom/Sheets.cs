namespace Allors.Excel
{
    using Allors.Excel.People;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        public PeopleSheet CreatePeople()
        {
            var worksheet = this.Host.ActiveWorksheet;
            worksheet.SetCustomPropertyValue("AllorsType", nameof(PeopleSheet));

            var sheet = new PeopleSheet(this, worksheet);
            this.SheetByWorksheet[worksheet.InnerObject] = sheet;
            return sheet;
        }
        
        private Sheet Instantiate(Worksheet worksheet)
        {
            var allorsType = (string)worksheet.GetCustomPropertyValue("AllorsType");

            switch (allorsType)
            {
                case nameof(PeopleSheet):
                    return new PeopleSheet(this, worksheet);
            }

            return null;
        }
    }
}
