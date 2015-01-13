using System;
using System.IO;
using WSCT.EMV.Personalization;
using WSCT.Helpers.Desktop;
using WSCT.Helpers.Json;

namespace WSCT.EMV.CardPersonalisationConsole
{
    class Program
    {
        private static ConsoleColor defaultColor;
        private const string EmvCardModelFileName = @"emv-card-model.json";
        private const string EmvCardDataFileName = @"emv-card-data.json";
        private const string EmvIssuerContextFileName = @"emv-issuer-context.json";
        private const string EmvIccContextFileName = @"emv-icc-context.json";
        private const string OutputJsonFileName = @"emv-cardpersonalisation-dgi.json";

        static void Main(/*string[] args*/)
        {
            RegisterPcl.Register();

            ShowHeader();

            var model = LoadFile<EmvPersonalizationModel>(EmvCardModelFileName);
            if (model == null)
            {
                return;
            }

            var data = LoadFile<EmvPersonalizationData>(EmvCardDataFileName);
            if (data == null)
            {
                return;
            }

            var issuerContext = LoadFile<EmvIssuerContext>(EmvIssuerContextFileName);
            if (issuerContext == null)
            {
                return;
            }

            var iccContext = LoadFile<EmvIccContext>(EmvIccContextFileName);
            if (iccContext == null)
            {
                return;
            }

            var builder = new DgiBuilder(model, data, issuerContext, iccContext);

            var container = new DgiContainer();

            try
            {
                Console.WriteLine("Building DGI for FCI");
                var dgiFci = builder.BuildDgi(model.Fci);
                Console.WriteLine("FCI: {0}", dgiFci);
                container.Fci = dgiFci;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                Console.WriteLine("Building DGI for GPO");
                var dgiGpo = builder.BuildDgi(model.Gpo);
                Console.WriteLine("GPO: {0}", dgiGpo);
                container.Gpo = dgiGpo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                Console.WriteLine("Building DGI for ACID");
                var dgiAcid = builder.BuildDgi(model.Acid);
                Console.WriteLine("ACID: {0}", dgiAcid);
                container.Acid = dgiAcid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var record in model.Records)
            {
                try
                {
                    Console.WriteLine("Building DGI for Record {0}.{1}", record.Sfi, record.Index);
                    var dgiRecord = builder.BuildDgi(record);
                    Console.WriteLine("DGI for Record {0}.{1}: {2}", record.Sfi, record.Index, dgiRecord);
                    container.Records.Add(dgiRecord);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine("Saving DGI in {0} file", OutputJsonFileName);
            container.WriteToJsonFile(OutputJsonFileName, true);
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

            Console.WriteLine("wsct-emvcp :: EMV Card Personalisation");
            Console.WriteLine("  input files: {0}, {1}, {2}, {3}", EmvCardModelFileName, EmvCardDataFileName, EmvIssuerContextFileName, EmvIccContextFileName);
            Console.WriteLine("  output file: {0}", OutputJsonFileName);
            Console.WriteLine();

            Console.ForegroundColor = defaultColor;
        }
    }
}
