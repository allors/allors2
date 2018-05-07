namespace Allors.Domain
{
    using System;

    public partial class Settings
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistArticleNumberCounter)
            {
                this.ArticleNumberCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }
        }

        public string NextArticleNumber()
        {
            var articleNumber = this.ArticleNumberCounter.NextValue();
            return string.Concat(this.ArticleNumberPrefix, articleNumber);
        }
    }
}
