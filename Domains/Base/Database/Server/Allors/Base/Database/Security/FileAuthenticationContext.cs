namespace Allors.Server
{
    using System.IO;
    using System.Security.Cryptography;

    using Microsoft.IdentityModel.Tokens;

    using Newtonsoft.Json;

    public class FileAuthenticationContext : IAuthenticationContext
    {
        public FileAuthenticationContext(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                fileInfo.Directory.Create();
                using (var provider = new RSACryptoServiceProvider(2048))
                {
                    var parameters = provider.ExportParameters(true);
                    var jsonParamters = new JsonParameters(parameters);
                    File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(jsonParamters));
                }
            }
            else
            {
                var jsonParameters = JsonConvert.DeserializeObject<JsonParameters>(File.ReadAllText(fileInfo.FullName));
                var rsaParams = jsonParameters.CreateRSAParameters();
                this.Key = new RsaSecurityKey(rsaParams);
            }
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public RsaSecurityKey Key { get; }
     
        private class JsonParameters
        {
            public JsonParameters()
            {
            }

            public JsonParameters(RSAParameters rsaParameters)
            {
                this.D = rsaParameters.D;
                this.DP = rsaParameters.DP;
                this.DQ = rsaParameters.DQ;
                this.Exponent = rsaParameters.Exponent;
                this.InverseQ = rsaParameters.InverseQ;
                this.Modulus = rsaParameters.Modulus;
                this.P = rsaParameters.P;
                this.Q = rsaParameters.Q;
            }

            public byte[] D { get; set; }

            public byte[] DP { get; set; }

            public byte[] DQ { get; set; }

            public byte[] Exponent { get; set; }

            public byte[] InverseQ { get; set; }

            public byte[] Modulus { get; set; }

            public byte[] P { get; set; }

            public byte[] Q { get; set; }

            public RSAParameters CreateRSAParameters()
            {
                return new RSAParameters
                           {
                               D = this.D,
                               DP = this.DP,
                               DQ = this.DQ,
                               Exponent = this.Exponent,
                               InverseQ = this.InverseQ,
                               Modulus = this.Modulus,
                               P = this.P,
                               Q = this.Q

                           };
            }
        }
    }
}

