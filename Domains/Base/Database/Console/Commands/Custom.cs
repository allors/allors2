namespace Allors.Console
{
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Tests;

  public class Custom : Command
    {
        public override void Execute()
        {
            var database = this.CreateDatabase();
            using (var session = database.CreateSession())
            {
                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

              var model = new EmailViewModel
                            {
                              UserName = "Koen"
                            };

              var templateService = session.ServiceProvider.GetRequiredService<ITemplateService>();
              var result = templateService.Render("Views/EmailTemplate.cshtml", model).Result;


                session.Derive();
                session.Commit();
            }
        }
    }
}
