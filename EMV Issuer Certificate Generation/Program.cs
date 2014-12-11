using System;
using WSCT.EMV.Personalization;
using WSCT.EMV.Security;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.IssuerCertificateGenerationConsole
{
    class Program
    {
        private const string CertificateAuthorityFileName = @"certificate-authority.json";
        private const string IssuerCertificateDataFileName = @"issuer-certificate-data.json";
        private const string OutputJsonFileName = @"emv-issuer-context.json";

        private static void Main( /*string[] args*/)
        {
            RegisterPcl.Register();

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("wsct-emvissuer :: EMV Issuer Certificate");
            Console.WriteLine();
            Console.ForegroundColor = defaultColor;

            PrivateKey caKey;
            try
            {
                caKey = CertificateAuthorityFileName.CreateFromJsonFile<PrivateKey>();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error reading file '{0}':", CertificateAuthorityFileName);
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(e);
                return;
            }

            IssuerCertificateData issuerCertificateData;
            try
            {
                issuerCertificateData = IssuerCertificateDataFileName.CreateFromJsonFile<IssuerCertificateData>();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error reading file '{0}':", IssuerCertificateDataFileName);
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Building Issuer Public Key Certificate");

            IssuerCertificateBuilder issuerCertificateBuilder;
            try
            {
                issuerCertificateBuilder = new IssuerCertificateBuilder(issuerCertificateData, caKey);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error building Issuer Public Key Certificate:");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Saving issuer context in {0} file", OutputJsonFileName);
            issuerCertificateBuilder.IssuerContext.WriteToJsonFile(OutputJsonFileName, true);
        }
    }
}
