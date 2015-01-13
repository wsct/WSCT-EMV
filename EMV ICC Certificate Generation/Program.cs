using System;
using System.IO;
using WSCT.EMV.Personalization;
using WSCT.EMV.Security;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.IccCertificateGenerationConsole
{
    class Program
    {
        private static ConsoleColor defaultColor;
        private const string IssuerCertificateDataFileName = @"issuer-certificate-data.json";
        private const string IccCertificateDataFileName = @"icc-certificate-data.json";
        private const string OutputJsonFileName = @"emv-icc-context.json";

        private static void Main( /*string[] args*/)
        {
            RegisterPcl.Register();

            ShowHeader();

            var iccCertificateData = LoadFile<IccCertificateData>(IccCertificateDataFileName);
            if (iccCertificateData == null)
            {
                return;
            }

            var issuerCertificateData = LoadFile<IssuerCertificateData>(IssuerCertificateDataFileName);
            if (issuerCertificateData == null)
            {
                return;
            }

            Console.WriteLine("Building ICC Public Key Certificate");

            IccCertificateBuilder iccCertificateBuilder;
            try
            {
                iccCertificateBuilder = new IccCertificateBuilder(iccCertificateData, issuerCertificateData.IssuerPrivateKey);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error building ICC Public Key Certificate:");
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Saving ICC context in {0} file", OutputJsonFileName);
            iccCertificateBuilder.IccContext.WriteToJsonFile(OutputJsonFileName, true);
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

            Console.WriteLine("wsct-emvicc :: EMV ICC Certificate Generation");
            Console.WriteLine("  input files: {0}, {1}", IssuerCertificateDataFileName, IccCertificateDataFileName);
            Console.WriteLine("  output file: {0}", OutputJsonFileName);
            Console.WriteLine();

            Console.ForegroundColor = defaultColor;
        }
    }
}
