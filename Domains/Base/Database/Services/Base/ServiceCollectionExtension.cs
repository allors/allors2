//------------------------------------------------------------------------------------------------- 
// <copyright file="ServiceCollectionExtension.cs" company="Allors bvba">
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

namespace Allors.Services
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Internal;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.CodeAnalysis;
    using Microsoft.CSharp.RuntimeBinder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.ObjectPool;

    public static class ServiceCollectionExtension
    {
        public static void AddAllors(this IServiceCollection services, string serverDirectory, string applicationName = null)
        {
            services.AddScoped<ISessionService, SessionService>();
            services.AddAllorsShared();

            var serverDirectoryFullName = new DirectoryInfo(serverDirectory).FullName;
            IFileProvider fileProvider = new PhysicalFileProvider(serverDirectoryFullName);
            var hostingEnvironment = new HostingEnvironment
                                         {
                                             ApplicationName = applicationName ?? Path.GetFileName(serverDirectory),
                                             WebRootFileProvider = fileProvider,
                                         };
            services.AddSingleton<IHostingEnvironment>(hostingEnvironment);
            services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.FileProviders.Clear();
                    options.FileProviders.Add(fileProvider);

                  var previous = options.CompilationCallback;
                  options.CompilationCallback = (context) =>
                    {
                      previous?.Invoke(context);

                      var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(v => MetadataReference.CreateFromFile(Assembly.Load(v).Location)).ToList();
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Private.CoreLib")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Abstractions")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.ViewFeatures")).Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Html.Abstractions")).Location));

                      assemblies.Add(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));
                      assemblies.Add(MetadataReference.CreateFromFile(typeof(RuntimeBinderException).GetTypeInfo().Assembly.Location));
                      assemblies.Add(MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location));
                      assemblies.Add(MetadataReference.CreateFromFile(typeof(ExpressionType).GetTypeInfo().Assembly.Location));
                      assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location));
                      
                      context.Compilation = context.Compilation.AddReferences(assemblies);
                    };
                });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));
            services.AddLogging();
            services.AddMvc();
        }

        public static void AddAllorsEmbedded(this IServiceCollection services)
        {
            services.AddScoped<ISessionService, EmbeddedSessionService>();
            services.AddAllorsShared();
        }

        private static void AddAllorsShared(this IServiceCollection services)
        {
            services.AddSingleton<ITreeService, TreeService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddSingleton<IStickyService, StickyService>();
            services.AddSingleton<IStateService, StateService>();
            services.AddSingleton<ITimeService, TimeService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IPublishingService, PublishingService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemplateService, TemplateService>();  // Required for running Razor in Server mode
        }
    }
}