//------------------------------------------------------------------------------------------------- 
// <copyright file="ServerTest.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Server.Tests
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;
    using Allors.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using ObjectFactory = Allors.ObjectFactory;

    public abstract class ServerTest : IDisposable
    {
        public const string Url = "http://localhost:5000";
        public const int RetryCount = 3;

        protected ServerTest()
        {
            var myAppSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/base.appSettings.json";

            var builder = new ConfigurationBuilder()
                .SetBasePath(new FileInfo("../../..").FullName)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(myAppSettings, true);
            this.Configuration = builder.Build();

            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            var configuration = new Allors.Adapters.Object.SqlClient.Configuration
            {
                ObjectFactory = this.ObjectFactory,
                ConnectionString = this.Configuration.GetConnectionString("DefaultConnection")
            };

            var services = new ServiceCollection();
            services.AddAllors();
            var serviceProvider = services.BuildServiceProvider();
            
            this.HttpClientHandler = new HttpClientHandler();
            this.HttpClient = new HttpClient(this.HttpClientHandler)
            {
                BaseAddress = new Uri(Url),
            };

            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var result = this.HttpClient.GetAsync("Test/Setup").Result;
            var database = new Allors.Adapters.Object.SqlClient.Database(serviceProvider, configuration);
            this.Session = database.CreateSession();
        }

        public IConfigurationRoot Configuration { get; set; }

        protected ObjectFactory ObjectFactory { get; }

        protected ISession Session { get; private set; }

        protected HttpClient HttpClient { get; set; }

        protected HttpClientHandler HttpClientHandler { get; set; }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;

            this.HttpClient.Dispose();
            this.HttpClient = null;
        }

        protected async System.Threading.Tasks.Task SignIn(User user)
        {
            var args = new AuthenticationTokenRequest
                           {
                               UserName = user.UserName,
                           };

            var uri = new Uri("TestAuthentication/Token", UriKind.Relative);
            var response = await this.PostAsJsonAsync(uri, args);
            var signInResponse = await this.ReadAsAsync<AuthenticationTokenResponse>(response);
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInResponse.Token);
        }

        protected void SignOut()
        {
            this.HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

        protected async Task<HttpResponseMessage> PostAsJsonAsync(Uri uri, object args)
        {
            var json = JsonConvert.SerializeObject(args);
            var response = await this.HttpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        protected async Task<T> ReadAsAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }
    }
}