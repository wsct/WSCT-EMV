using System;
using System.IO;
using WSCT.EMV.Personalization;
using WSCT.EMV.Security;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.IssuerCertificateGenerationConsole
{
    class Program
    {
        private static ConsoleColor defaultColor;
        private const string CertificateAuthorityFileName = @"certificate-authority.json";
        private const string IssuerCertificateDataFileName = @"issuer-certificate-data.json";
        private const string OutputJsonFileName = @"emv-issuer-context.json";

        private static void Main( /*string[] args*/)
        {
            RegisterPcl.Register();

            ShowHeader();

            var caKey = LoadFile<PrivateKey>(CertificateAuthorityFileName);
            if (caKey == null)
            {
                return;
            }

            var issuerCertificateData = LoadFile<IssuerCertificateData>(IssuerCertificateDataFileName);
            if (issuerCertificateData == null)
            {
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

        private static T LoadFile<T>(string fileName)
        {
            try
            {
                return fileName.CreateFromJsonFile<T>();
            }
            catch (FileNotFoundException)
            {
                ShowFileNotFound(fileName);
                return default(T);
            }
            catch (Exception e)
            {
                ShowReadError(fileName, e);
                return default(T);
            }
        }

        private static void ShowReadError(string fileName, Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error reading file '{0}':", fileName);
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(e);
        }

        private static void ShowFileNotFound(string fileName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File not found: '{0}'", fileName);
            Console.ForegroundColor = defaultColor;
        }

        private static void ShowHeader()
        {
            defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("wsct-emvissuer :: EMV Issuer Data Preparation");
            Console.WriteLine("  input files: {0}, {1}", CertificateAuthorityFileName, IssuerCertificateDataFileName);
            Console.WriteLine("  output file: {0}", OutputJsonFileName);
            Console.WriteLine();

            Console.ForegroundColor = defaultColor;
        }
    }
}
