namespace Allors.Excel
{
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        
        private Sheet Instantiate(Worksheet worksheet)
        {
            var allorsType = (string)worksheet.GetCustomPropertyValue("AllorsType");

            switch (allorsType)
            {
                //case nameof(PeopleSheet):
                //    return new PeopleSheet(this, worksheet);
            }

            return null;
        }
    }
}
