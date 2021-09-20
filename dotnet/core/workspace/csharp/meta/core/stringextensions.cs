namespace Allors.Workspace
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string Humanize(this string @this)
        {
            var regex1 = new Regex(@"([a-z\d])([A-Z])");
            var regex2 = new Regex(@"([A-Z]+)([A-Z][a-z\d]+)");

            var result = @this;

            result = regex1.Replace(result, "$1 $2");
            result = regex2.Replace(result, "$1 $2");

            return result;
        }
    }
}
