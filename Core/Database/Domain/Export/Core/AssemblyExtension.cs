namespace Tests
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;

    public static class AssemblyExtensions
    {
        public static string Fingerprint(this Assembly assembly)
        {
            using (var md5 = MD5.Create())
            {
                var assemblyBytes = File.ReadAllBytes(assembly.Location);
                var assemblyHash = md5.ComputeHash(assemblyBytes);
                return string.Concat(assemblyHash.Select(v => v.ToString("X2")));
            }
        }
    }
}
