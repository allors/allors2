namespace Allors.Excel
{
    using System;
    using System.Threading.Tasks;

    public partial class Commands
    {
        private void OnError(Exception e)
        {
            e.Handle();
        }

        public async Task PeopleNew()
        {
            var sheet = this.Sheets.CreatePeople();
            await sheet.Refresh();
        }
    }
}
