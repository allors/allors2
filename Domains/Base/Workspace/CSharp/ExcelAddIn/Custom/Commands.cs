namespace Allors.Excel
{
    using System;
    using Nito.AsyncEx;

    public partial class Commands
    {
        private void OnError(Exception e)
        {
            e.Handle();
        }

        public void PeopleNew()
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                    {
                        var sheet = this.Sheets.CreatePeople();
                        await sheet.Refresh();
                    });
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }
    }
}
