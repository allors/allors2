using System.Web.Configuration;

namespace Workspace.Web
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public class AllorsConfig
    {
        public static bool IsProduction
        {
            get
            {
                var production = WebConfigurationManager.AppSettings["production"];
                return string.IsNullOrWhiteSpace(production) || bool.Parse(production);
            }
        }

        public static void Register()
        {
            // Initialize meta population
            var metaPopulation = MetaPopulation.Instance;

            if (IsProduction)
            {
                var accessControlPrefetch = new PrefetchPolicyBuilder()
                    .WithRule(M.AccessControl.EffectiveUsers)
                    .WithRule(M.AccessControl.EffectivePermissions)
                    .Build();

                var securityTokenPrefetch = new PrefetchPolicyBuilder()
                    .WithRule(M.SecurityToken.AccessControls, accessControlPrefetch)
                    .Build();

                using (var session = Config.Default.CreateSession())
                {
                    var securityTokens = new SecurityTokens(session).Extent();
                    session.Prefetch(securityTokenPrefetch, securityTokens);
                }
            }
        }
    }
}
