using Bogus;

namespace Allors
{
    public partial class Config
    {
        public Faker faker { get; set; } = new Faker("en");
    }
}