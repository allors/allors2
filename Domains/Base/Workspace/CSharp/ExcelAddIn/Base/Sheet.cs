namespace Allors.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Allors.Protocol.Remote;
    using Allors.Protocol.Remote.Push;
    using Allors.Workspace;
    using Allors.Workspace.Client;

    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;

    using NLog;

    public abstract partial class Sheet
    {
        protected Sheet(string name, Sheets sheets, Worksheet worksheet)
        {
            this.Sheets = sheets;

            this.Worksheet = worksheet;

            this.Context = new Context(name, this.Sheets.Client.Database, this.Sheets.Client.Workspace);
        }

        public Sheets Sheets { get; }

        public Worksheet Worksheet { get; }

        public Context Context { get; }

        public CustomTaskPane TaskPane { get; set; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        // Commands
        public virtual async Task Load(object args)
        {
            await this.Context.Load(args);
        }

        public async Task<PushResponse> Save()
        {
            await this.OnSaving();

            var saveResponse = await this.Context.Save();

            if (saveResponse.HasErrors)
            {
                saveResponse.Log(this.Context.Session);
                saveResponse.Show();
            }

            this.OnSaved(saveResponse);

            return saveResponse;
        }

        public virtual void MediatorOnStateChanged()
        {
            if (this.TaskPane != null)
            {
                this.TaskPane.Visible = this.Sheets.Host.ActiveWorksheet.Equals(this.Worksheet);
            }
        }

        public abstract Task Refresh();
        
        public Worksheet WorksheetByName(string name)
        {
            var sheet = this.Sheets.Host.Application.ActiveWorkbook.Sheets
                .Cast<Microsoft.Office.Interop.Excel.Worksheet>()
                .FirstOrDefault(ws => name.Equals(ws.Name, StringComparison.OrdinalIgnoreCase));

            if (sheet != null)
            {
                return this.Sheets.Host.GetVstoWorksheet(sheet);
            }

            return null;
        }

        public async Task SaveAndInvokeSilent(Method method)
        {
            await this.OnSaving();

            ErrorResponse response = await this.Context.Save();

            if (response.HasErrors)
            {
                response.Log(this.Context.Session);
                response.Show();
            }
            else
            {
                response = await this.Sheets.Client.Database.Invoke(method);

                if (response.HasErrors)
                {
                    response.Log(this.Context.Session);
                    response.Show();
                }
            }
        }

        public async Task Invoke(Method method)
        {
            var response = await this.Sheets.Client.Database.Invoke(method);

            if (response.HasErrors)
            {
                response.Log(this.Context.Session);
                response.Show();
            }
            
            this.OnInvoked(response);
        }

        public async Task Invoke(string service, Dictionary<string, string> values)
        {
            var response = await this.Sheets.Client.Database.Invoke(service, values);

            if (response.HasErrors)
            {
                response.Log(this.Context.Session);
                response.Show();
            }

            this.OnInvoked(response);
        }

        public async Task SaveAndInvoke(Method method)
        {
            await this.OnSaving();

            ErrorResponse response = await this.Context.Save();

            if (response.HasErrors)
            {
                response.Log(this.Context.Session);
                response.Show();
            }
            else
            {
                response = await this.Sheets.Client.Database.Invoke(method);

                if (response.HasErrors)
                {
                    response.Log(this.Context.Session);
                    response.Show();
                }
            }

            this.OnInvoked(response);
        }

        public async Task SaveAndInvoke(string service, Dictionary<string, string> values)
        {
            await this.OnSaving();

            ErrorResponse response = await this.Context.Save();

            if (response.HasErrors)
            {
                response.Log(this.Context.Session);
                response.Show();
            }
            else
            {
                response = await this.Sheets.Client.Database.Invoke(service, values);

                if (response.HasErrors)
                {
                    response.Log(this.Context.Session);
                    response.Show();
                }
            }

            this.OnInvoked(response);
        }

        public async Task<Result> Query(string service, Dictionary<string, string> p)
        {
            var result = await this.Context.Query(service, p);
            return result;
        }

        public async Task<object> QueryResults(string service, Dictionary<string, string> p)
        {
            var result = await this.Context.Query(service, p);
            return result.Collections["results"];
        }

        protected SessionObject GetObject(string key)
        {
            return this.Context.Objects[key];
        }

        protected T GetObject<T>(string key)
              where T : SessionObject
        {
            return (T)this.Context.Objects[key];
        }

        protected SessionObject[] GetCollection(string key)
        {
            return this.Context.Collections[key];
        }

        protected T[] GetCollection<T>(string key)
        {
            var collection = this.Context.Collections[key];
            return collection?.Cast<T>().ToArray() ?? new T[0];
        }

        protected object GetValue(string key)
        {
            return this.Context.Values[key];
        }

        protected ListObject FindListObject(string name)
        {
            foreach (Microsoft.Office.Interop.Excel.ListObject interopListObject in this.Worksheet.ListObjects)
            {
                if (interopListObject.Name != null && interopListObject.Name.ToLowerInvariant().Equals(name?.ToLowerInvariant()))
                {
                    return this.Sheets.Host.GetVstoListObject(interopListObject);
                }
            }

            return null;
        }

        protected abstract Task OnSaving();

        protected virtual void OnSaved(ErrorResponse response)
        {
        }

        protected virtual void OnInvoked(ErrorResponse response)
        {
        }
    }
}
