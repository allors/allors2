namespace Allors.Server.OData
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.OData;

    public class InMemoryMessage : IODataRequestMessage, IODataResponseMessage, IContainerProvider, IDisposable
    {
        private readonly Dictionary<string, string> headers;

        public InMemoryMessage()
        {
            this.headers = new Dictionary<string, string>();
        }

        public IEnumerable<KeyValuePair<string, string>> Headers => this.headers;

        public int StatusCode { get; set; }

        public Uri Url { get; set; }

        public string Method { get; set; }

        public Stream Stream { get; set; }

        public IServiceProvider Container { get; set; }

        public Action DisposeAction { get; set; }

        public string GetHeader(string headerName)
        {
            return this.headers.TryGetValue(headerName, out var headerValue) ? headerValue : null;
        }

        public void SetHeader(string headerName, string headerValue)
        {
            this.headers[headerName] = headerValue;
        }

        public Stream GetStream()
        {
            return this.Stream;
        }

        void IDisposable.Dispose()
        {
            this.DisposeAction?.Invoke();
        }
    }
}
